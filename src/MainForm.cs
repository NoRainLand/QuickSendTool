using System.IO;
using System.Reflection;

namespace NoRain
{
    public class MainForm : Form
    {
        public Form? form = null;
        private Label? label = null;

        private Label? label1 = null;

        private TextBox? textBox1 = null;

        private Label? label2 = null;

        private TextBox? textBox2 = null;

        private Label? label3 = null;

        private TextBox? textBox3 = null;

        private Button? button1 = null;
        private Button? button2 = null;

        private Label? copyright = null;

        private Label? tips = null;

        private NotifyIcon? notifyIcon = null;

        private ContextMenuStrip? trayMenu = null;

        //UI上下文
        public SynchronizationContext? uiContext = null;

        CheckBox? radioButton = null;

        public Icon? GetIcon()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            // foreach (var resourceName in asm.GetManifestResourceNames())
            // {
            //     Console.WriteLine(resourceName);
            // }
            using (Stream? iconStream = asm.GetManifestResourceStream($"{Config.AppName}.img.QucikSendTool.ico"))
            {
                if (iconStream != null)
                {
                    return new Icon(iconStream);
                }
                else
                {
                    Console.WriteLine("资源未找到");
                    Application.Exit();//不正确，直接退出得了
                    return null;
                }
            }

        }

        public void ShowView()
        {
            form = new Form
            {
                Text = "QuickSendTool",

                Icon = GetIcon(),

                // 设置窗体不能放大缩小且不能最小化
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MinimizeBox = false,
                MaximizeBox = false,
                Width = 350,
                Height = 280
            };



            label = new Label
            {
                Text = "请确认服务器配置",
                Location = new Point(20, 15),
                AutoSize = true,
            };

            tips = new Label
            {
                Text = "保存配置之后对着文件右键“发送到服务器”即可实现上传",
                Location = new Point(20, 45),
                AutoSize = true
            };

            label1 = new Label
            {
                Text = "请输入服务器IP：",
                Location = new Point(20, 75),
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
                Location = new Point(20, 105),
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
                Location = new Point(20, 135),
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
                Location = new Point(20, 165)
            };

            button2 = new Button
            {
                Text = "移除配置",
                Location = new Point(100, 165)
            };

            radioButton = new CheckBox
            {
                Text = "开机自启",
                Location = new Point(200, 165),

            };


            if (Config.ReadConfig())
            {
                textBox1.Text = Config.host;
                textBox2.Text = Config.port.ToString();
                textBox3.Text = Config.api;
            }


            copyright = new Label
            {
                Text = "copyright by NoRain 2024",
                ForeColor = Color.FromArgb(255, 128, 128, 128),
                AutoSize = true,
                Location = new Point(20, 195)
            };


            button1.Click += new EventHandler((sender, e) =>
                {
                    Config.WriteConfig(textBox1?.Text ?? Config.host, int.TryParse(textBox2?.Text, out int tempPort) ? tempPort : Config.port, textBox3?.Text ?? Config.api);
                    form?.Hide();
                    // ShowLoading(100.0);
                });

            button2.Click += new EventHandler((sender, e) =>
            {
                Config.RemoveConfig();
            });


            radioButton.CheckedChanged += new EventHandler((sender, e) =>
            {
                if (radioButton.Checked)
                {
                    WinTask.Add();
                }
                else
                {
                    WinTask.Remove();
                }
            });


            form.Controls.Add(label);
            form.Controls.Add(tips);
            form.Controls.Add(label1);
            form.Controls.Add(textBox1);
            form.Controls.Add(label2);
            form.Controls.Add(textBox2);
            form.Controls.Add(label3);
            form.Controls.Add(textBox3);
            form.Controls.Add(button1);
            form.Controls.Add(button2);
            // form.Controls.Add(radioButton);
            form.Controls.Add(copyright);




            form.Show();
        }

        public void ShowTray()
        {
            notifyIcon = new NotifyIcon
            {
                Icon = GetIcon(),
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

            uiContext = SynchronizationContext.Current;

        }

        // 显示加载进度

        private Form? loadingForm = null;

        private Label? titleLabel = null;

        private Label? loadingLabel = null;

        private Label? loadingLabel1 = null;

        private Panel? titlePanel = null;

        private Panel? bgPanel = null;

        public async Task HideLoading(bool success)
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

        public void ShowLoading(double value)
        {
            if (loadingForm == null)
            {
                loadingForm = new Form
                {
                    Text = Config.AppName,
                    // 设置窗体不能放大缩小且不能最小化
                    FormBorderStyle = FormBorderStyle.None,
                    Width = 285,
                    Height = 100,
                    TopMost = true, // 设置窗体为顶置
                    StartPosition = FormStartPosition.CenterScreen,
                    // 禁用关闭按钮需要在窗体的 ControlBox 属性设置为 false
                    ControlBox = false,
                    ShowInTaskbar = false,
                    BackColor = Color.FromArgb(255, 100, 100, 100),

                };

                titlePanel = new Panel
                {
                    Location = new Point(1, 1),
                    Size = new Size(283, 30),
                    BackColor = Color.White
                };

                bgPanel = new Panel
                {
                    Location = new Point(1, 1),
                    Size = new Size(283, 98),
                    BackColor = Color.FromArgb(255, 240, 240, 240),
                };


                titleLabel = new Label
                {
                    Text = "上传进度:0.00%",
                    Location = new Point(5, 5),
                    ForeColor = Color.Black,
                    BackColor = Color.White,
                    AutoSize = true
                };

                loadingLabel = new Label
                {
                    Location = new Point(10, 45),
                    ForeColor = Color.Black,
                    BackColor = Color.FromArgb(255, 240, 240, 240),
                };

                loadingLabel1 = new Label
                {
                    Location = new Point(10, 65),
                    ForeColor = Color.Green,
                    Size = new Size(273, 20),
                    BackColor = Color.FromArgb(255, 240, 240, 240),

                };
                loadingLabel1.Font = new Font(loadingLabel1.Font.Name, 12, loadingLabel1.Font.Style);
                titleLabel.Font = new Font(titleLabel.Font.Name, 10, titleLabel.Font.Style);

                loadingForm.Controls.Add(titleLabel);
                loadingForm.Controls.Add(loadingLabel);
                loadingForm.Controls.Add(loadingLabel1);
                loadingForm.Controls.Add(titlePanel);
                loadingForm.Controls.Add(bgPanel);
                UpdateProgress(value);
            }
            else
            {
                UpdateProgress(value);
            }
        }

        private void UpdateProgress(double value)
        {
            if (loadingForm != null)
            {

                if (loadingForm.Visible == false)
                {
                    loadingForm.Show();
                    loadingForm.Activate();
                }
                if (titleLabel != null)
                {
                    titleLabel.Text = $"上传进度：{value.ToString("F2")}%";
                }

                if (loadingLabel1 != null)
                {
                    int v = (int)Math.Ceiling(value / 4);
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
