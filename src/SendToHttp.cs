using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace NoRain
{
    public static class SendToHttp
    {
        // 定义进度回调和完成回调的委托
        public delegate void ProgressCallback(double percentage);
        public delegate void CompletionCallback(bool success, string message);

        //     public async static Task Send(string path, ProgressCallback progressCallback, CompletionCallback completionCallback)
        //     {
        //         string url = $"{Config.host}:{Config.port}{Config.api}";
        //         // 确保文件存在
        //         if (!File.Exists(path))
        //         {
        //             Console.WriteLine("文件不存在");
        //             completionCallback(false, "文件不存在");
        //             return;
        //         }

        //         try
        //         {
        //             using (var client = new HttpClient())
        //             using (var content = new MultipartFormDataContent())
        //             using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
        //             {
        //                 var fileContent = new StreamContent(fileStream);
        //                 fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
        //                 {
        //                     Name = "\"file\"", // 服务器端接收文件的参数名
        //                     FileName = "\"" + Path.GetFileName(path) + "\""
        //                 };
        //                 content.Add(fileContent);

        //                 // 创建进度报告器并设置回调
        //                 var progress = new Progress<long>(totalBytes =>
        //                 {
        //                     double percentage = (double)totalBytes / fileStream.Length * 100;
        //                     progressCallback(percentage);
        //                 });



        //                 // 发送POST请求到服务器
        //                 var response = await client.PostAsync(url, content);

        //                 completionCallback(true, $"服务器响应状态码: {response.StatusCode}");
        //             }
        //         }
        //         catch (Exception ex)
        //         {
        //             completionCallback(false, $"发送文件时发生错误: {ex.Message}");
        //         }
        //     }
        // }
        // 修改 Send 方法中的文件上传部分
        public async static Task Send(string path, Action<double> progressCallback, Action<bool, string> completionCallback)
        {
            string url = $"{Config.host}:{Config.port}{Config.api}";
            if (!File.Exists(path))
            {
                Console.WriteLine("文件不存在");
                completionCallback(false, "文件不存在");
                return;
            }

            try
            {
                using (var client = new HttpClient())
                using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    var fileContent = new ProgressableStreamContent(fileStream, 4096, (sent, total) =>
                    {
                        double percentage = (double)sent / total * 100;
                        progressCallback(percentage);
                    });

                    fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                    {
                        Name = "\"file\"",
                        FileName = "\"" + Path.GetFileName(path) + "\""
                    };

                    using (var content = new MultipartFormDataContent())
                    {
                        content.Add(fileContent);

                        var response = await client.PostAsync(url, content);
                        completionCallback(true, $"服务器响应状态码: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                completionCallback(false, $"发送文件时发生错误: {ex.Message}");
            }
        }

        // 进度报告的 HttpContent
        public class ProgressableStreamContent : HttpContent
        {
            private readonly Stream content;
            private readonly int bufferSize;
            private readonly Action<long, long> progress;

            public ProgressableStreamContent(Stream content, int bufferSize, Action<long, long> progress)
            {
                this.content = content ?? throw new ArgumentNullException(nameof(content));
                this.bufferSize = bufferSize;
                this.progress = progress ?? throw new ArgumentNullException(nameof(progress));
                this.Headers.ContentLength = content.Length;
            }

            protected override async Task SerializeToStreamAsync(Stream stream, TransportContext? context)
            {
                var buffer = new byte[bufferSize];
                long uploaded = 0;

                using (content)
                {
                    while (true)
                    {
                        var length = await content.ReadAsync(buffer, 0, buffer.Length);
                        if (length <= 0) break;

                        await stream.WriteAsync(buffer, 0, length);
                        uploaded += length;
                        progress(uploaded, content.Length);
                    }
                }
            }

            protected override bool TryComputeLength(out long length)
            {
                length = content.Length;
                return true;
            }
        }

    }
}