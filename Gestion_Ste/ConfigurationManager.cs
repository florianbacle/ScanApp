using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
namespace SmartDeviceProject3
{
    class ConfigurationManager
    {

        private static XmlTextReader GetReaderConfigFile()
        {
            // Récupération du chemin de l'éxécutable:
            string appPath = System.IO.Path.GetFullPath(System.Reflection.
                Assembly.GetExecutingAssembly().GetName().CodeBase) + ".config";
            // Ouverture du fichier de config en lecture:
            FileStream file = new FileStream(appPath, FileMode.Open,
                FileAccess.Read, FileShare.Read);

            return new XmlTextReader(file);
        }

        public static T GetKey<T>(string key)
        {
            XmlTextReader xmlRead = null;
            try
            {
                // Récupère le flux XML du fichier de config:
                xmlRead = GetReaderConfigFile();
                // Recherche la clef dans le fichier de config:
                while (xmlRead.Read())
                {
                    XmlNodeType nodeType = xmlRead.NodeType;

                    if (nodeType == XmlNodeType.Element)
                    {
                        if (xmlRead.Name.Equals("add") &&
                            xmlRead.GetAttribute("key").Equals(key))
                        {
                            // Retourne la valeur de la clef sous le bon type:
                            return (T)Convert.ChangeType(xmlRead.GetAttribute("value"),
                                typeof(T), null);
                        }
                    }
                }
            }
            catch (XmlException ex)
            {
                throw ex;
            }
            finally
            {
                if (xmlRead != null)
                    xmlRead.Close();
            }

            return default(T);
        }
    }
}
/*// Récupération d'une valeur de type 'string' à partir du fichier de config:
string value1 = ConfigurationManager.GetKey<string>("key1");

// Récupération d'une valeur de type 'int' à partir du fichier de config:
int value2 = ConfigurationManager.GetKey<int>("key2");*/