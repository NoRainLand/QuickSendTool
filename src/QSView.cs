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
            ChangeForm();
        }

        private static void InitForm()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            form = new Form();
            form.Text = "QuickSendTool";

            form.Icon = new Icon("./img/Q32.ico");

            // 设置窗体不能放大缩小且不能最小化
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.MinimizeBox = false;
            form.MaximizeBox = false;



            label = new Label();
            label1 = new Label();
            textBox1 = new TextBox();
            label2 = new Label();
            textBox2 = new TextBox();
            label3 = new Label();
            textBox3 = new TextBox();
            button1 = new Button();
            label4 = new Label();
            button2 = new Button();
        }

        private static void ChangeForm()
        {

            label!.Text = "1，确认服务器配置";

            label1!.Text = "请输入服务器IP：";

            label2!.Text = "请输入服务器端口：";

            label3!.Text = "请输入服务器API：";

            button1!.Text = "保存配置";

            label4!.Text = "2，添加注册表";

            button2!.Text = "确认添加";



            label.Location = new System.Drawing.Point(0, 0);
            label.AutoSize = true;
            label1.Location = new System.Drawing.Point(20, 30);
            label1.Width = 120;
            textBox1!.Location = new System.Drawing.Point(label1.Location.X + label1.Width, label1.Location.Y - 5);
            textBox1.Width = 160;

            label2.Location = new System.Drawing.Point(20, 60);
            label2.Width = 120;
            textBox2!.Location = new System.Drawing.Point(label2.Location.X + label2.Width, label2.Location.Y - 5);
            textBox2.Width = 160;



            label3.Location = new System.Drawing.Point(20, 90);
            label3.Width = 120;
            textBox3!.Location = new System.Drawing.Point(label3.Location.X + label3.Width, label3.Location.Y - 5);
            textBox3.Width = 160;

            button1.Location = new System.Drawing.Point(20, 120);

            label4.Location = new System.Drawing.Point(0, 150);
            label4.AutoSize = true;
            button2.Location = new System.Drawing.Point(20, 180);


            form!.Controls.Add(label);
            form.Controls.Add(label1);
            form.Controls.Add(textBox1);
            form.Controls.Add(label2);
            form.Controls.Add(textBox2);
            form.Controls.Add(label3);
            form.Controls.Add(textBox3);
            form.Controls.Add(button1);
            form.Controls.Add(label4);
            form.Controls.Add(button2);

            form.Width = 330;
            form.Height = 250;

            Application.Run(form);
        }
    }
}