using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace 定时任务
{
    public static class Class1
    {
        public struct Task
        {
            public struct time
            {
                public static int hour;
                public static int minute;
            }

            public static int num;
            public static string address;
            public static string qnum;
            public static string text;
            public time plan;
        }

        public static string version = "v0.1";
        public static bool activated = false;
        public static string hkey = string.Empty;
        public static string m_name = string.Empty;
        public static string path = string.Empty;
        public static string ac_info = string.Empty;

        public static List<Task> tasks = new List<Task>();

         public static string Md5(string str)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(str)), 4, 8);
            t2 = t2.Replace("-", "");
            return t2;
        }
    }
}
