using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 定时任务
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Class1.path = string.Empty;
            Class1.path = System.AppDomain.CurrentDomain.BaseDirectory.ToString();

            if (Class1.path == string.Empty)
            {
                MessageBox.Show("程序错误：无法获得启动参数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }

            Class1.path = Class1.path + "\\bin";
            System.IO.Directory.CreateDirectory(Class1.path);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
