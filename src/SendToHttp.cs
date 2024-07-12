using System.Net;

namespace NoRain
{
    public static class SendToHttp
    {
        // 定义进度回调和完成回调的委托
        public delegate void ProgressCallback(double percentage);
        public delegate void CompletionCallback(bool success, string message);

        public static async void MainSend(string path)
        {
            await Send(path, (percentage) =>
                {
                    Console.WriteLine($"上传进度: {percentage}%");
                }, (success, message) =>
                {
                    Console.WriteLine(message);
                });
        }

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
                    bool isOverSize = fileStream.Length > Config.MinSize * 1024 * 1024;
                    var fileContent = new ProgressableStreamContent(fileStream, 4096, (sent, total) =>
                    {
                        double percentage = (double)sent / total * 100;
                        if (isOverSize)
                        {
                            progressCallback(percentage);
                            // 使用Invoke确保在UI线程上更新UI
                            if (MainForm.uiContext != null)
                            {
                                MainForm.uiContext.Post(_ =>
                                {
                                    MainForm.ShowLoading(percentage);
                                }, null);
                            }
                        }
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
                        if (MainForm.uiContext != null)
                        {
                            MainForm.uiContext.Post(async _ =>
                            {
                                await MainForm.HideLoading(response.StatusCode == HttpStatusCode.OK);
                            }, null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                completionCallback(false, $"发送文件时发生错误: {ex.Message}");
                if (MainForm.uiContext != null)
                {
                    MainForm.uiContext.Post(async _ =>
                    {
                        await MainForm.HideLoading(false);
                    }, null);
                }

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

        public async static void TestSend()
        {
            string path = "C:\\Users\\NoRain_C\\Downloads\\yuanshen_4.2.0.apk";
            await SendToHttp.Send(path, (percentage) =>
                {
                    // Console.WriteLine($"上传进度: {percentage}%");
                }, (success, message) =>
                {
                    Console.WriteLine($"上传结果: {message}");
                });
        }

    }
}