using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using File = System.IO.File;

namespace 定时任务
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        static extern IntPtr FindWindow(string strClass, string strWindow);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern bool SetFocus(IntPtr hWnd);


        [DllImport("user32.dll")]
        private static extern bool PostMessage(int hhwnd, uint msg, IntPtr wparam, IntPtr lparam);

        [DllImport("user32.dll")]
        static extern void BlockInput(bool Block);

        [DllImport("user32.dll")]
        private static extern IntPtr LoadKeyboardLayout(string pwszKLID, uint Flags);

        private static uint WM_INPUTLANGCHANGEREQUEST = 0x0050;
        private static int HWND_BROADCAST = 0xffff;
        private static string en_US = "00000409";                                       //英文
        private static string cn_ZH = "00000804";
        private static uint KLF_ACTIVATE = 1;

        //调用此方法时，将屏蔽中文输入法(操作系统级别，即使使用快捷键ctrl+shift也还原不回中文输入法)
        private static void En_Language_Only()
        {
            PostMessage(HWND_BROADCAST, WM_INPUTLANGCHANGEREQUEST, IntPtr.Zero, LoadKeyboardLayout(en_US, KLF_ACTIVATE));
        }

        //调用此方法时，将可以使用中文输入法(中文输入法有效)
        private void Allow_MultiLanguage()
        {
            PostMessage(HWND_BROADCAST, WM_INPUTLANGCHANGEREQUEST, IntPtr.Zero, LoadKeyboardLayout(cn_ZH, KLF_ACTIVATE));
        }

        IntPtr hwnd;
        long qnum;
        string text;
        int hour, minute;
        Class1.Task thisTask = new Class1.Task();
        int para = -2;

        public Form1(int reason)
        {
            para = reason;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            switch (para)
            {
                case -1:
                    //增加
                    label 
                    break;

                case 0:
                    //删除
                    break;

                default:
                    //修改
                    break;
            }
            label6.Text = Class1.version;

            System.IO.FileStream fs;
            string temp;

            fs = File.Open(Class1.path + "\\log.t", System.IO.FileMode.OpenOrCreate);
            fs.Close();
            temp = File.ReadAllText(Class1.path + "\\log.t");
            temp = temp + "\n" + "[" + DateTime.Now.Date.ToString() + " " + DateTime.Now.TimeOfDay.ToString() + "]";
            temp = temp + " Programme Started.";
            File.WriteAllText(Class1.path + "\\log.t", temp);

            fs = File.Open(Class1.path + "\\jcs.t", System.IO.FileMode.OpenOrCreate);
            fs.Close();
            File.WriteAllText(Class1.path + "\\jcs.t", Class1.Md5(temp));

            Class1.path = Class1.path + "\\psw.t";
            fs = File.Open(Class1.path, System.IO.FileMode.OpenOrCreate);
            fs.Close();
            Class1.ac_info = File.ReadAllText(Class1.path);
            Class1.m_name = Environment.MachineName;
            if(Class1.Md5(Class1.m_name) == Class1.ac_info)
            {
                Class1.activated = true;
            }
            else
            {
                File.WriteAllText(Class1.path, string.Empty);
                textBox2.Text = "您尚未激活软件，本项不可用";
                textBox2.Enabled = false;
                richTextBox1.Text = "此副本尚未激活，请联系作者激活软件。\n详情请双击版本号";
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text.ToString() == string.Empty)
            {
                MessageBox.Show("空窗口名！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //读取设置值
            hwnd = IntPtr.Zero;
            qnum = 0;
            hwnd = FindWindow(null, textBox1.Text.ToString());
            if (textBox2.Text.ToString() != string.Empty && Class1.activated == true)
            {
                qnum = long.Parse(textBox2.Text.ToString());
            }
            text = richTextBox1.Text.ToString();
            hour = (int)numericUpDown1.Value;
            minute = (int)numericUpDown2.Value;

            //锁定设置交互
            button2.Enabled = false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            richTextBox1.Enabled = false;
            numericUpDown1.Enabled = false;
            numericUpDown2.Enabled = false;

            //进入计时模式
            button3.Visible = true;
            this.Text = "定时消息任务_运行中";
            timer1.Start();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            int nhour, nminute;
            nhour = DateTime.Now.Hour;
            nminute = DateTime.Now.Minute;

            if(nhour == hour && nminute == minute)
            {
                this.Text = "定时消息任务_时间到，执行中";

                BlockInput(true);
                En_Language_Only();
                Thread.Sleep(500);
                ShowWindow(hwnd, 1);
                SetFocus(hwnd);
                Thread.Sleep(500);
                if (qnum != 0)
                {
                    SendKeys.Send("@");
                    SendKeys.Send(qnum.ToString());
                    Thread.Sleep(500);
                    SendKeys.Send("\n");
                    Thread.Sleep(500);
                }
                SendKeys.Send(text.ToString());
                Thread.Sleep(500);
                SendKeys.Send("^\n");
                Thread.Sleep(500);
                Allow_MultiLanguage();
                BlockInput(false);

                Task_Stop();
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Task_Stop();
        }

        void Task_Stop()
        {
            this.Text = "定时消息任务_编辑任务";

            button2.Enabled = true;
            textBox1.Enabled = true;
            textBox2.Enabled = (Class1.activated == true ? true : false);
            richTextBox1.Enabled = true;
            numericUpDown1.Enabled = true;
            numericUpDown2.Enabled = true;

            timer1.Stop();

            button3.Visible = false;
            this.Focus();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.ToString() == string.Empty)
                return;

            hwnd = IntPtr.Zero;
            hwnd = FindWindow(null, textBox1.Text.ToString());

            if (hwnd == IntPtr.Zero)
                MessageBox.Show("找不到对应的窗口", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                MessageBox.Show("成功找到对应的窗口，句柄为\n" + hwnd.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            Only_NumPress(e);
        }

        void Only_NumPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            if (textBox1.ReadOnly == true)
                return;

            if ((int)e.KeyChar == 127 /*del*/ || (int)e.KeyChar == 8 /*Backspace*/)
                return;

            if ((int)e.KeyChar < 48 || (int)e.KeyChar > 57)
            {
                e.Handled = true;
                return;
            }
        }

        private void Label6_DoubleClick(object sender, EventArgs e)
        {
            Form abf = new AboutForm1();
            abf.ShowDialog();
        }

        private void NumericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown2.Value == 60)
                numericUpDown2.Value = 0;
            else if (numericUpDown2.Value == -1)
                numericUpDown2.Value = 59;
        }

        private void CheckBox1_Click(object sender, EventArgs e)
        {
            checkBox1.CheckState = CheckState.Checked;
            checkBox2.CheckState = CheckState.Unchecked;
        }

        private void CheckBox3_Click(object sender, EventArgs e)
        {
            checkBox3.CheckState = CheckState.Checked;
            checkBox4.CheckState = CheckState.Unchecked;
        }

        private void CheckBox2_Click(object sender, EventArgs e)
        {
            checkBox2.CheckState = CheckState.Checked;
            checkBox1.CheckState = CheckState.Unchecked;
        }
    }
}
