using System.Diagnostics;
using System.IO.Pipes;

namespace NoRain
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {

            //判断是否存在进程
            Process[] processes = Process.GetProcessesByName(Config.AppName);
            string path = args.Length > 0 ? args[0] : "";
            if (processes.Length == 1)
            {
                StartApp(path);//没有进程直接启动
            }
            else
            {
                ProcessMsgListener.SendMessageToProcess(path);
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

            AddEvent();
            if (path != "")
            {
                OnSendFile(path);
            }
            Application.Run();
            Console.WriteLine("程序启动");
        }


        static void AddEvent()
        {
            Register.Add();


            AppDomain.CurrentDomain.ProcessExit += OnExit;

            ProcessMsgListener.ListenForMessages(OnMsg);

        }

        static async void OnSendFile(string path)
        {
            await SendToHttp.Send(path, (percentage) =>
                {
                    Console.WriteLine($"上传进度: {percentage}%");
                }, (success, message) =>
                {
                    Console.WriteLine(message);
                });
        }

        static void OnExit(Object? sender, EventArgs args)
        {

            Register.Remove();
            Console.WriteLine("程序退出");
        }

        static void OnMsg(string msg)
        {
            OnSendFile(msg);
        }
    }
}