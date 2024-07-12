using System.IO.Pipes;
using System.Text;

namespace NoRain
{
    public static class ProcessMsgListener
    {
        public static void SendMessageToProcess(string message, bool isExit = true)
        {
            using (var client = new NamedPipeClientStream(Config.PipeName))
            {
                client.Connect();
                using (var writer = new StreamWriter(client))
                {
                    writer.Write(message);
                    writer.Flush();
                }
            }
            if (isExit)
            {
                Environment.Exit(0);
            }
        }

        // 定义回调函数的签名
        public delegate void MessageReceivedCallback(string message);
        public static void ListenForMessages(MessageReceivedCallback callback, bool isOnce = false)
        {
            Task.Run(() =>
            {
                while (!isOnce)
                {
                    using (var server = new NamedPipeServerStream(Config.PipeName))
                    {
                        Console.WriteLine("等待连接...");
                        server.WaitForConnection();
                        using (var reader = new StreamReader(server, Encoding.UTF8))
                        {
                            string message = reader.ReadToEnd();
                            Console.WriteLine($"接收到消息: {message}");
                            // 调用回调函数
                            callback?.Invoke(message);
                        }
                    }
                }
            });
        }
    }
}