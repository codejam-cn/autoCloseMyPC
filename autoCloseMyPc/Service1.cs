using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Timers;
using System.Xml;

namespace autoCloseMyPc
{
    public partial class Service1 : ServiceBase
    {
        private const string FilePath0 = @"E:\My\autoCloseMyPC\autoCloseMyPc\Settings.xml";
        private const string LogFilePath = @"E:\My\autoCloseMyPC\autoCloseMyPc\log.txt";

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Log.AddLog(LogFilePath, "service start");
            XmlReader rdr = XmlConfig.GetXmlReader(FilePath0);
            int timeIntervalHour = int.Parse(XmlConfig.GetValueStr(rdr, "timeInterval", "hour"));
            int timeIntervalMin = int.Parse(XmlConfig.GetValueStr(rdr, "timeInterval", "minute"));
            int timeIntervalSec = int.Parse(XmlConfig.GetValueStr(rdr, "timeInterval", "second"));
            int timeIntervalMs = int.Parse(XmlConfig.GetValueStr(rdr, "timeInterval", "ms"));
            int msTotal = (3600 * 1000) * timeIntervalHour + 60 * 1000 * timeIntervalMin + 1000 * timeIntervalSec + timeIntervalMs;
            Timer t = new Timer(msTotal); //特定时间间隔检测一次
            t.Elapsed += ShutDownMyPc;
            t.Start();
        }

        protected override void OnStop()
        {
            Log.AddLog(LogFilePath, "service end");
        }

        public static void ShutDownMyPc(object sender, ElapsedEventArgs e)
        {
            try
            {
                XmlReader rdr = XmlConfig.GetXmlReader(FilePath0);
                string activeWeekdayStr = XmlConfig.GetValueStr(rdr, "activeWeekday", "active");
                List<int> activeWeekdays = activeWeekdayStr.Split(',').Select(int.Parse).ToList(); // [0,1,2,3,4]
                int maxDayofWeek = activeWeekdays.Max();
                int minDayofWeek = activeWeekdays.Min();

                int shutdownTimeHour = int.Parse(XmlConfig.GetValueStr(rdr, "time", "hour"));
                int shutdownTimeMin = int.Parse(XmlConfig.GetValueStr(rdr, "time", "minute"));

                var hour = e.SignalTime.Hour;
                var min = e.SignalTime.Minute;
                var dayofWeek = e.SignalTime.DayOfWeek;
                if (hour == shutdownTimeHour && min >= shutdownTimeMin && dayofWeek >= (DayOfWeek)minDayofWeek && dayofWeek <= (DayOfWeek)maxDayofWeek)
                {
                    using (var myPro = new Process())
                    {
                        myPro.StartInfo.FileName = "cmd.exe";
                        myPro.StartInfo.UseShellExecute = false;
                        myPro.StartInfo.RedirectStandardInput = true;
                        myPro.StartInfo.RedirectStandardOutput = true;
                        myPro.StartInfo.RedirectStandardError = true;
                        myPro.StartInfo.CreateNoWindow = true;
                        myPro.Start();
                        const string str = @"shutdown -f -s -t 300";
                        myPro.StandardInput.WriteLine(str);
                        myPro.StandardInput.AutoFlush = true;
                        myPro.WaitForExit();
                    }
                }
            }
            catch (Exception exception)
            {
                if (exception.InnerException != null)
                {
                    Log.AddLog(LogFilePath, exception.InnerException.ToString());
                }

                throw;
            }
        }
    }
}