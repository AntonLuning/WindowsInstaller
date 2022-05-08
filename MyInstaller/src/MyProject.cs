using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace MyInstaller
{
    public static class MyProject
    {
        public static void Save(Dictionary<string, string> GUIData, string filePath)
        {
            XmlDocument doc = new XmlDocument();

            // XML Declaration
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.InsertBefore(xmlDeclaration, doc.DocumentElement);

            // Root Element
            XmlElement rootElement = doc.CreateElement("Project");
            doc.AppendChild(rootElement);

            // Project Info
            XmlElement projectElement = doc.CreateElement("Info");
            rootElement.AppendChild(projectElement);

            foreach (KeyValuePair<string, string> item in GUIData)
            {
                XmlElement infoElement = doc.CreateElement(item.Key);
                projectElement.AppendChild(infoElement);

                XmlText infoText = doc.CreateTextNode(item.Value);
                infoElement.AppendChild(infoText);
            }

            doc.Save(filePath);
        }

        public static Dictionary<string, string> Open(string filePath)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            // Project Info
            XmlNodeList? projectNodes = doc.DocumentElement?.GetElementsByTagName("Info");
            if (projectNodes != null && projectNodes.Count > 0)
            {
                XmlNode projectInfo = projectNodes[0];

                foreach (XmlNode node in projectInfo.ChildNodes)
                    result.Add(node.Name, node.InnerText);
            }

            return result;
        }
    }
}
