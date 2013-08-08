using System.Diagnostics;
using System.Xml;

namespace SDLite
{
    /// <summary>
    /// Build helper.
    /// </summary>
    public static class BuildHelper
    {
        public static bool IsProject40(string projectXml)
        {
            bool result = false;
            double targetFrameworkVersion;
            XmlDocument projectDocument = new XmlDocument();
            
            projectDocument.LoadXml(projectXml);
            
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(projectDocument.NameTable);
            namespaceManager.AddNamespace("msbd", projectDocument.DocumentElement.NamespaceURI); 
            
            XmlNodeList nodes = projectDocument.SelectNodes("//msbd:PropertyGroup//msbd:TargetFrameworkVersion", namespaceManager);
            
            Debug.Assert(nodes != null, "TargetFrameworkVersion node not foud (null).");
            Debug.Assert(nodes.Count > 0, "TargetFrameworkVersion node not foud (count = 0).");

            // Parse <TargetFrameworkVersion>v4.0<TargetFrameworkVersion/>
            if(double.TryParse(nodes[0].InnerText.Remove(0, 1),
                               out targetFrameworkVersion) && targetFrameworkVersion >= 4)
            {
                result = true;
            }
            
            return result;
        }
    }
}
