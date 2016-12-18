using System.IO;

namespace autoCloseMyPc
{
    public class Log
    {
        public static void AddLog(string filePath, int msg)
        {
            AddLog(filePath, msg.ToString());
        }

        public static void AddLog(string filePath,string msg)
        {
            StreamWriter ss = File.AppendText(filePath);
            ss.Write(msg + "\n");
            ss.Close();
        }

    }
}