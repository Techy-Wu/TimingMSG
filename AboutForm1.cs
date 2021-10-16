using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 定时任务
{
    public partial class AboutForm1 : Form
    {
        public AboutForm1()
        {
            InitializeComponent();
        }

        private void AboutForm1_Load(object sender, EventArgs e)
        {
            textBox_ApplicationName.Text = "定时消息发送器";
            textBox_Version.Text = Class1.version;
            textBox_Copyright.Text = "copyright © 2020-2021";
            textBox_Author.Text = "Techy_Wu@outlook.com";
            if(Class1.activated == false)
            {
                textBox_AccessState.BackColor = Color.PaleVioletRed;
                textBox_AccessState.Text = "此副本尚未激活，单击此处以激活";
            }
            else
            {
                textBox_AccessState.Text = "本副本已成功授权给 " + Class1.m_name;
            }
        }

        private void TextBox_AccessState_Click(object sender, EventArgs e)
        {
            if (Class1.activated == true)
                return;

            Form kf = new KeyForm();
            kf.ShowDialog();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
