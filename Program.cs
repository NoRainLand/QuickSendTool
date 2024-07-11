using System.Diagnostics;
using System.IO.Pipes;

namespace NoRain
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                string path = args[0];
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    // FileName = $"{Appt}", // 指定要运行的程序
                    CreateNoWindow = true, // 不创建窗口
                    UseShellExecute = false // 不使用shell执行程序
                };

                Process process = new Process
                {
                    StartInfo = startInfo
                };

                process.Start();
            }
            else
            {
                StartApp();
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

        static void StartApp(string path = "")
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
            Console.WriteLine("程序启动");
            Application.Run();
        }
    }
}