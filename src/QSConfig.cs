using Newtonsoft.Json.Linq;
using System.Diagnostics;
namespace NoRain
{
    public static class QSConfig
    {
        //服务器ip
        public static string host = "http://127.0.0.1";
        //端口
        public static int port = 4100;
        //接口
        public static string api = "/upload";

        private static string configPath = "./QSConfig.json";

        public static bool ReadConfig()
        {
            if (File.Exists(configPath))
            {
                string json = File.ReadAllText(configPath);
                JObject jObject = JObject.Parse(json);
                if (jObject == null)
                {
                    Console.WriteLine("配置文件格式错误");
                    return false;
                }

                host = jObject["host"]?.ToString() ?? host; // 使用 null 条件运算符和 null 合并运算符提供默认值
                port = int.TryParse(jObject["port"]?.ToString(), out int tempPort) ? tempPort : port;
                api = jObject["api"]?.ToString() ?? api;

                Console.WriteLine($"服务器 IP: {host}");
                Console.WriteLine($"端口: {port}");
                Console.WriteLine($"接口: {api}");
                return true;
            }
            else
            {
                Console.WriteLine("配置文件读取失败");
                return false;
            }
        }

        public static void WriteConfig(string host, int port, string api)
        {

            QSConfig.host = host;
            QSConfig.port = port;
            QSConfig.api = api;

            JObject jObject = new JObject
            {
                ["host"] = host,
                ["port"] = port,
                ["api"] = api
            };
            File.WriteAllText(configPath, jObject.ToString());
        }
    }
}