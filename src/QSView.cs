using System;
using System.Windows.Forms;

namespace NoRain
{
    public static class QSView
    {

        private static Form? form = null;
        private static Label? label = null;

        private static Label? label1 = null;

        private static TextBox? textBox1 = null;

        private static Label? label2 = null;

        private static TextBox? textBox2 = null;

        private static Label? label3 = null;

        private static TextBox? textBox3 = null;

        private static Button? button1 = null;

        private static Label? label4 = null;

        private static Button? button2 = null;


        public static void ShowView()
        {
            InitForm();
        }

        private static void InitForm()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
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
                Text = "1，确认服务器配置",
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
                Location = new Point(label3.Location.X + label3.Width, label3.Location.Y - 5),
                Width = 160
            };
            button1 = new Button
            {
                Text = "保存配置",
                Location = new Point(20, 120)
            };
            label4 = new Label
            {
                Text = "2，添加注册表",
                Location = new Point(0, 150),
                AutoSize = true
            };
            button2 = new Button
            {
                Text = "确认添加",
                Location = new Point(20, 180)
            };


            form.Controls.Add(label);
            form.Controls.Add(label1);
            form.Controls.Add(textBox1);
            form.Controls.Add(label2);
            form.Controls.Add(textBox2);
            form.Controls.Add(label3);
            form.Controls.Add(textBox3);
            form.Controls.Add(button1);
            form.Controls.Add(label4);
            form.Controls.Add(button2);
            Application.Run(form);
        }

    }
}