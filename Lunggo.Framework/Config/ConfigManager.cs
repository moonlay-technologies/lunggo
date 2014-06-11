using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Lunggo.Framework.Config
{
    public class ConfigManager
    {
        private const String ConfigFileName = @"application.properties";
        private static readonly ConfigManager Instance = new ConfigManager();
        private Dictionary<string, PropertyConfig> _configDictionary;
        private bool _isInitialized;
       
        private ConfigManager()
        {

        }

        public static ConfigManager GetInstance()
        {
            return Instance;
        }

        public void Init(string directoryPath)
        {
            if (!_isInitialized)
            {
                _configDictionary = LoadConfigFileToDictionary(Path.Combine(directoryPath,ConfigFileName));
                _isInitialized = true;
            }
            else
            {
                throw new InvalidOperationException("Config Manager is already initialized");
            }
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

        private Dictionary<string, PropertyConfig> LoadConfigFileToDictionary(String filePath)
        {
            var configDoc = LoadConfigurationFile(filePath);
            return ConfigToDictionary(configDoc);   
        }
        

        private XElement LoadConfigurationFile(String filePath)
        {
            return XElement.Load(filePath);
        }
        
        private Dictionary<string, PropertyConfig> ConfigToDictionary(XElement doc)
        {
            var configNodes = doc.Elements();
            var configDictionary = configNodes.
                GroupBy
                (
                    n => n.Attribute("file").Value
                ).
                ToDictionary
                (
                    @group => @group.Key.ToString(CultureInfo.InvariantCulture), 
                    @group => new PropertyConfig
                    {
                        Dictionary = @group.ToDictionary(n => n.Attribute("name").Value, n => n.Value)
                    }
                );
            return configDictionary;
        }

        private class PropertyConfig
        {
            public Dictionary<string,string> Dictionary { get; set;}
        }
    }
}
