using System;
using System.Diagnostics;
using System.ServiceProcess;
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
            Timer t = new Timer(300000);  //5分钟检测一次
            t.Elapsed += ShutDownMyPc; 
            t.Start();
        }

        protected override void OnStop()
        {
        }

        protected void ShutDownMyPc(object sender, ElapsedEventArgs e)
        {
            var hour = e.SignalTime.Hour;
            var min = e.SignalTime.Minute;
            var dayofWeek = e.SignalTime.DayOfWeek;
            if (hour == 0 && min >= 30 && dayofWeek >= (DayOfWeek)1 && dayofWeek <= (DayOfWeek)5)
            {
                using (Process myPro = new Process())
                {
                    myPro.StartInfo.FileName = "cmd.exe";
                    myPro.StartInfo.UseShellExecute = false;
                    myPro.StartInfo.RedirectStandardInput = true;
                    myPro.StartInfo.RedirectStandardOutput = true;
                    myPro.StartInfo.RedirectStandardError = true;
                    myPro.StartInfo.CreateNoWindow = true;
                    myPro.Start();
                    string str = @"shutdown -f -s -t 300";
                    myPro.StandardInput.WriteLine(str);
                    myPro.StandardInput.AutoFlush = true;
                    myPro.WaitForExit();

                }
            }
        }
    }
}
