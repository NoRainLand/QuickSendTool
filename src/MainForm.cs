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

            Button button2 = new Button
            {
                Text = "测试连接",
                Location = new Point(20, 150)
            };

            button2.Click += new EventHandler(async (sender, e) =>
                            {
                                string path = "C:\\Users\\NoRain_C\\Downloads\\yuanshen_4.2.0.apk";
                                await SendToHttp.Send(path, (percentage) =>
                                    {
                                        Console.WriteLine($"上传进度: {percentage}%");
                                    }, (success, message) =>
                                    {
                                        Console.WriteLine($"上传结果: {message}");
                                    });
                            });



            button1.Click += new EventHandler((sender, e) =>
                {
                    Config.WriteConfig(textBox1?.Text ?? Config.host, int.TryParse(textBox2?.Text, out int tempPort) ? tempPort : Config.port, textBox3?.Text ?? Config.api);


                });



            form.Controls.Add(label);
            form.Controls.Add(label1);
            form.Controls.Add(textBox1);
            form.Controls.Add(label2);
            form.Controls.Add(textBox2);
            form.Controls.Add(label3);
            form.Controls.Add(textBox3);
            form.Controls.Add(button1);

            form.Controls.Add(button2);
            form.ShowDialog();
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
                    form.ShowDialog();
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
    }
}
