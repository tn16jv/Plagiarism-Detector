using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Floss.Api.FileToModel
{
    class ValidExtensions
    {
        private static string xmlPath = @".\extensions.xml";
        private static ICollection<string> javaExt;
        private static ICollection<string> cppExt;
        private static ICollection<string> cExt;

        static ValidExtensions()
        {
            javaExt = readXmlInfo("java");
            cppExt = readXmlInfo("cpp");
            cExt = readXmlInfo("c");
        }

        public static ICollection<string> getExtensions(string type)
        {
            switch (type)
            {
                case "java":
                    return javaExt;
                case "cpp":
                    return cppExt;
                case "c":
                    return cExt;
                default:
                    return new HashSet<string>();
            }
        }

        private static ICollection<string> readXmlInfo(string fileType)
        {
            ICollection<string> extensions = new HashSet<string>();
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlPath);
                XmlNode node = doc.SelectSingleNode(String.Format(@"//FileType[@id='{0}']", fileType));
                foreach (XmlNode innerNode in node)
                    extensions.Add(innerNode.InnerText);
            }
            catch (Exception e) { }
            return extensions;
        }
    }
}
