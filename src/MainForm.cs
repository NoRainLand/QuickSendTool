using System.CodeDom;

namespace NoRain
{
    public static class MainForm
    {

        public static Form? form = null;
        private static Label? label = null;

        private static Label? label1 = null;

        private static TextBox? textBox1 = null;

        private static Label? label2 = null;

        private static TextBox? textBox2 = null;

        private static Label? label3 = null;

        private static TextBox? textBox3 = null;

        private static Button? button1 = null;



        private static NotifyIcon? notifyIcon = null;

        private static ContextMenuStrip? trayMenu = null;

        public static void ShowView()
        {
            form = new Form
            {
                Text = "QuickSendTool",

                Icon = new Icon("./img/Q32.ico"),

                // 设置窗体不能放大缩小且不能最小化
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MinimizeBox = false,
                MaximizeBox = false,
                Width = 330,
                Height = 250
            };



            label = new Label
            {
                Text = "请确认服务器配置",
                Location = new Point(0, 0),
                AutoSize = true,
            };
            label1 = new Label
            {
                Text = "请输入服务器IP：",
                Location = new Point(20, 30),
                Width = 120
            };
            textBox1 = new TextBox
            {
                Text = Config.host,
                Location = new Point(label1.Location.X + label1.Width, label1.Location.Y - 5),
                Width = 160
            };
            label2 = new Label
            {
                Text = "请输入服务器端口：",
                Location = new Point(20, 60),
                Width = 120
            };
            textBox2 = new TextBox
            {
                Text = Config.port.ToString(),
                Location = new Point(label2.Location.X + label2.Width, label2.Location.Y - 5),
                Width = 160
            };
            label3 = new Label
            {
                Text = "请输入服务器API：",
                Location = new Point(20, 90),
                Width = 120
            };
            textBox3 = new TextBox
            {
                Text = Config.api,
                Location = new Point(label3.Location.X + label3.Width, label3.Location.Y - 5),
                Width = 160
            };
            button1 = new Button
            {
                Text = "保存配置",
                Location = new Point(20, 120)
            };





            button1.Click += new EventHandler((sender, e) =>
                {
                    Config.WriteConfig(textBox1?.Text ?? Config.host, int.TryParse(textBox2?.Text, out int tempPort) ? tempPort : Config.port, textBox3?.Text ?? Config.api);
                    form?.Hide();
                });



            form.Controls.Add(label);
            form.Controls.Add(label1);
            form.Controls.Add(textBox1);
            form.Controls.Add(label2);
            form.Controls.Add(textBox2);
            form.Controls.Add(label3);
            form.Controls.Add(textBox3);
            form.Controls.Add(button1);




            form.Show();
        }

        public static void ShowTray()
        {
            notifyIcon = new NotifyIcon
            {
                Icon = new Icon("./img/Q32.ico"),
                Text = "QuickSendTool",
                Visible = true
            };

            trayMenu = new ContextMenuStrip();

            trayMenu.Items.Add("显示", null, (sender, e) =>
            {
                if (form != null)
                {
                    form.Show();
                }
                else
                {
                    ShowView();
                }
            });

            trayMenu.Items.Add("退出", null, (sender, e) =>
            {
                notifyIcon?.Dispose();
                Application.Exit();
            });

            notifyIcon.ContextMenuStrip = trayMenu;
        }

        // 显示加载进度

        private static Form? loadingForm = null;

        private static Label? loadingLabel = null;

        private static Label? loadingLabel1 = null;

        public async static Task HideLoading(bool success)
        {
            if (loadingLabel != null)
            {
                if (success)
                {
                    loadingLabel.Text = "上传成功";
                    loadingLabel.ForeColor = Color.Green;
                }
                else
                {
                    loadingLabel.Text = "上传失败";
                    loadingLabel.ForeColor = Color.Red;
                }
            }
            await Task.Delay(400);
            loadingForm?.Hide();
        }

        public static void ShowLoading(double value)
        {
            if (loadingForm == null)
            {
                loadingForm = new Form
                {
                    Text = "上传进度：0.00%",
                    // 设置窗体不能放大缩小且不能最小化
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Width = 300,
                    Height = 100,
                    TopMost = true, // 设置窗体为顶置
                    StartPosition = FormStartPosition.CenterScreen,
                    // 禁用关闭按钮需要在窗体的 ControlBox 属性设置为 false
                    ControlBox = false,
                    ShowInTaskbar = false
                };
                loadingLabel = new Label
                {
                    Location = new Point(5, 5),
                    ForeColor = Color.Black,
                };

                loadingLabel1 = new Label
                {
                    Location = new Point(5, 25),
                    ForeColor = Color.Green,
                    Size = new Size(300, 20),

                };
                loadingLabel1.Font = new Font(loadingLabel1.Font.Name, 12, loadingLabel1.Font.Style);

                loadingForm.Controls.Add(loadingLabel);
                loadingForm.Controls.Add(loadingLabel1);
                UpdateProgress(value);
            }
            else
            {
                UpdateProgress(value);
            }
        }

        private static void UpdateProgress(double value)
        {
            if (loadingForm != null)
            {

                if (loadingForm.Visible == false)
                {
                    loadingForm.Show();
                    loadingForm.Activate();
                }
                loadingForm.Text = $"上传进度：{value.ToString("F2")}%";

                if (loadingLabel1 != null)
                {
                    int v = (int)Math.Ceiling(value / 5);
                    string text = "";
                    for (int i = 0; i < v; i++)
                    {
                        text += "#";
                    }
                    loadingLabel1.Text = text;
                    if (loadingLabel != null)
                    {
                        if (v % 2 == 1)
                        {
                            loadingLabel.Text = "上传中…";
                        }
                        else
                        {
                            loadingLabel.Text = "上传中……";
                        }
                    }
                }
            }
        }
    }
}
