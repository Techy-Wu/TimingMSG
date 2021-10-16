using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using File = System.IO.File;

namespace 定时任务
{
    public partial class KeyForm : Form
    {
        string key, hkey;

        public KeyForm()
        {
            InitializeComponent();
        }

        private void KeyForm_Load(object sender, EventArgs e)
        {
            key = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
            hkey = Class1.Md5(key);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text.ToString().ToUpper();
            if(hkey == textBox1.Text.ToString())
            {
                MessageBox.Show("激活成功，请重启软件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                File.WriteAllText(Class1.path, Class1.Md5(Class1.m_name));
                Class1.activated = true;
            }
            else
            {
                MessageBox.Show("激活码错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
