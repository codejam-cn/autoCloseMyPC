using System.Xml;
using System.Xml.Linq;

namespace autoCloseMyPc
{
    internal class XmlConfig
    {

       
        public static XmlReader GetXmlReader(string filePath)
        {
            var doc = XDocument.Load(filePath);
            XmlReader rdr = doc.CreateReader();
            return rdr;
        }

        public static string GetValueStr(XmlReader rdr, string nodeName, string keyName)
        {
            while (rdr.Read())
            {
                if (rdr.Name == nodeName && rdr.GetAttribute("key") == keyName)
                {
                   return rdr.GetAttribute("value");
                   
                }
            }
            return "";
        }



    }
}
