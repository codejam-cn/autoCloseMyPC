﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

/*
 这个文件暂时没用，什么都没做
 */

namespace autoCloseMyPc
{
    class Util
    {
        public void Util111()
        {
            string debugPath = Directory.GetCurrentDirectory(); //debug/
            string parentPath0 = Directory.GetParent(debugPath).FullName;
            string parentPath1 = Directory.GetParent(parentPath0).FullName;
            string pyPath = Path.Combine(parentPath1, "Settings.xml");

            string image = Path.Combine(parentPath1, "Resources/psb.jpg");

            string image1 = Path.Combine(parentPath1, "Settings.xml");
            var aa = File.Exists(image);
            var bb = File.Exists(image1);

            XDocument doc = XDocument.Load(image1);
            var weekdays = from p in doc.Descendants("activeWeekday")
                           select p.Value;
            List<int> days = weekdays.ToList().Select(int.Parse).ToList();




            var hour = from p in doc.Descendants("hour")
                       select p.Value;
            int hourInt = int.Parse(hour.First());

            var min = from p in doc.Descendants("minute")
                      select p.Value;
            int minInt = int.Parse(min.First());

            string txtFileName = DateTime.Now.ToString("hhmmssffffff") + ".txt";
            string filePath = Path.Combine(parentPath1, txtFileName);

            StreamWriter sw = new StreamWriter(filePath);
            string content = minInt.ToString();

            sw.Write(content);
            sw.Close();
            sw.Dispose();



            Console.WriteLine(filePath);
            Console.ReadLine();
            
        }

         
    }
}
