using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace Lunggo.Framework.Config
{
    public class ConfigManager
    {
        ConfigManager _configManager;
        private static ConfigManager instance = new ConfigManager();
        Dictionary<string, PropertyConfig> ConfigDictionary;
        string fileExtension = "*properties";
        private ConfigManager()
        {

        }
        public void init(string directoryPath)
        {
            string[] AllFilesInPath = getAllFilesInPath(directoryPath);
            Dictionary<string, PropertyConfig> dictionary = ReadAndWriteAllFilesToDictionary(AllFilesInPath);
            ConfigDictionary = dictionary;
        }
        public static ConfigManager GetInstance()
        {
            return instance;
        }
        public string GetConfigValue(string FileName, string KeyName)
        {
            PropertyConfig propertyConfig = ConfigDictionary[FileName];
            return propertyConfig.Dictionary[KeyName];
        }

        private string[] getAllFilesInPath(string directoryPath)
        {
            string[] AllFilesInPath = System.IO.Directory.GetFiles(directoryPath, this.fileExtension);
            return AllFilesInPath;
        }
        private Dictionary<string, PropertyConfig> ReadAndWriteAllFilesToDictionary(string[] AllFilesInPath)
        {
            Dictionary<string, PropertyConfig> dictionary =  new Dictionary<string,PropertyConfig>();
            foreach (string aFilePath in AllFilesInPath)
            {
                PropertyConfig propertyConfig = GeneratePropertyConfigFromAFile(aFilePath);
                string FileName = GetFileNameWithoutExtension(aFilePath);
                dictionary.Add(FileName, propertyConfig);
            }
            return dictionary;
        }
        private string GetFileNameWithoutExtension(string aFilePath)
        {
            return Path.GetFileNameWithoutExtension(aFilePath);
        }
        private PropertyConfig GeneratePropertyConfigFromAFile(string aFilePath)
        {
            PropertyConfig propertyConfig = new PropertyConfig();
            propertyConfig.Dictionary = getAllKeyAndValueFromFileInPath(aFilePath);
            return propertyConfig;
        }
        private Dictionary<string, string> getAllKeyAndValueFromFileInPath(string aFilePath)
        {
            var doc = XDocument.Load(aFilePath);
            var rootNodes = doc.Root.DescendantNodes().OfType<XElement>();
            var allItems = rootNodes.ToDictionary(n => n.Attribute("name").Value, n => n.Value);
            return allItems;
        }

        private class PropertyConfig
        {
            public Dictionary<string,string> Dictionary{get;set;}
        }
    }
}
