using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OEE
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {   
            Process current = Process.GetCurrentProcess();
            Process[] process = Process.GetProcessesByName(current.ProcessName);
            //if (process .Length >1)
            //{
            //    MessageBox.Show("程序已启动，请勿重复打开");
            //    return;
            //}

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
