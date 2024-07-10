using System.IO.Pipes;

namespace NoRain
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {


            // 尝试创建一个命名互斥体
            bool isNewInstance;
            using (Mutex mutex = new Mutex(true, Config.AppName, out isNewInstance))
            {
                if (isNewInstance)
                {
                    if (args != null && args.Length > 0)
                    {
                        string path = args[0];
                        StartApp(path);
                    }
                    else
                    {
                        StartApp();
                    }
                    GC.KeepAlive(mutex); // 防止互斥体在应用程序运行时被垃圾回收
                    Task.Run(() => ListenForMessages());
                }
                else
                {
                    if (args != null && args.Length > 0)
                    {
                        string path = args[0];
                        SendMessage(path);
                    }
                    else
                    {
                        Console.WriteLine("什么也不做");
                        SendMessage("123123");
                    }
                }
            }


        }


        static async void ListenForMessages()
        {
            using (var server = new NamedPipeServerStream(Config.PipeName))
            {
                while (true)
                {
                    server.WaitForConnection();
                    using (var reader = new StreamReader(server))
                    {
                        var message = reader.ReadLine();

                        await SendToHttp.Send(message ?? "", (percentage) =>
                             {
                                 Console.WriteLine($"上传进度: {percentage}%");
                             }, (success, message) =>
                             {
                                 Console.WriteLine(message);
                             });
                    }
                    server.Disconnect();
                }
            }
        }

        static void SendMessage(string message)
        {
            using (var client = new NamedPipeClientStream(Config.PipeName))
            {
                client.Connect(1000); // 等待1秒
                using (var writer = new StreamWriter(client))
                {
                    writer.WriteLine(message);
                    writer.Flush();
                }
            }
        }

        static async void StartApp(string path = "")
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (Config.ReadConfig())
            {
                MainForm.ShowTray();
            }
            else
            {
                MainForm.ShowView();
            }

            Register.Add();
            AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
            {
                Register.Remove();
                Console.WriteLine("程序退出");
            };

            if (path != null && path != "")
            {
                await SendToHttp.Send(path, (percentage) =>
                    {
                        Console.WriteLine($"上传进度: {percentage}%");
                    }, (success, message) =>
                    {
                        Console.WriteLine(message);
                    });
            }
            Console.WriteLine("程序启动");
            Application.Run();
        }
    }
}