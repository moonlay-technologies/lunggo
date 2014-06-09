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
        private static readonly ConfigManager Instance = new ConfigManager();
        private Dictionary<string, PropertyConfig> _configDictionary;
        private readonly string FileExtension = "*properties";
        private bool _isInitialized;
        private ConfigManager()
        {

        }
        public void Init(string directoryPath)
        {
            if (!_isInitialized)
            {
                var allFilesInPath = GetAllFilesInPath(directoryPath);
                _configDictionary = ReadAndWriteAllFilesToDictionary(allFilesInPath);
                _isInitialized = true;
            }
            else
            {
                throw new InvalidOperationException("Config Manager is already initialized");
            }
        }
        public static ConfigManager GetInstance()
        {
            return Instance;
        }
        public string GetConfigValue(string fileName, string keyName)
        {
            var propertyConfig = _configDictionary[fileName];
            if (propertyConfig != null)
            {
                return propertyConfig.Dictionary[keyName];
            }
            else
            {
                throw new ArgumentException(String.Format("Configuration File {0}.properties is not exist",fileName));
            }
        }

        private string[] GetAllFilesInPath(string directoryPath)
        {
            var allFilesInPath = System.IO.Directory.GetFiles(directoryPath, this.FileExtension);
            return allFilesInPath;
        }
        private Dictionary<string, PropertyConfig> ReadAndWriteAllFilesToDictionary(string[] allFilesInPath)
        {
            var dictionary =  new Dictionary<string,PropertyConfig>();
            foreach (string aFilePath in allFilesInPath)
            {
                PropertyConfig propertyConfig = GeneratePropertyConfigFromAFile(aFilePath);
                string fileName = GetFileNameWithoutExtension(aFilePath);
                dictionary.Add(fileName, propertyConfig);
            }
            return dictionary;
        }
        private string GetFileNameWithoutExtension(string aFilePath)
        {
            return Path.GetFileNameWithoutExtension(aFilePath);
        }
        private PropertyConfig GeneratePropertyConfigFromAFile(string aFilePath)
        {
            var propertyConfig = new PropertyConfig
            {
                Dictionary = getAllKeyAndValueFromFileInPath(aFilePath)
            };
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
            public Dictionary<string,string> Dictionary { get; set;}
        }
    }
}
