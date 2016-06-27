using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace autoCloseMyPc
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            System.Timers.Timer t = new System.Timers.Timer(3000);  //10分钟检测一次
            //System.Timers.Timer t = new System.Timers.Timer(600000);  //10分钟检测一次
            t.Elapsed += new System.Timers.ElapsedEventHandler(ShutDownMyPc); //到达时间的时候执行事件；   
         
            t.Start();


        }

        protected override void OnStop()
        {
        }

        protected void ShutDownMyPc(object sender, ElapsedEventArgs e)
        {
            var hour = e.SignalTime.Hour;
            var min = e.SignalTime.Minute;
            using (Process myPro = new Process())
            {
                myPro.StartInfo.FileName = "cmd.exe";
                myPro.StartInfo.UseShellExecute = false;
                myPro.StartInfo.RedirectStandardInput = true;
                myPro.StartInfo.RedirectStandardOutput = true;
                myPro.StartInfo.RedirectStandardError = true;
                myPro.StartInfo.CreateNoWindow = true;
                myPro.Start();
                //如果调用程序路径中有空格时，cmd命令执行失败，可以用双引号括起来 ，在这里两个引号表示一个引号（转义）
                string str = @"shutdown -f -s -t 300";
                myPro.StandardInput.WriteLine(str);
                myPro.StandardInput.AutoFlush = true;
                myPro.WaitForExit();

            }
        }
    }
}
