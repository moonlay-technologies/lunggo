using System;
using System.Collections.Generic;

namespace Lunggo.Framework.Config
{
    public partial class ConfigManager
    {
        private static readonly ConfigManager Instance = new ConfigManager();
        private Dictionary<string, string> _configDictionary = PopulateDictionary();

        public static ConfigManager GetInstance()
        {
            return Instance;
        }

        public string GetConfigValue(string fileName, string keyName)
        {
            var key = string.Join(".", fileName, keyName);
            var isExist = _configDictionary.TryGetValue(key, out var value);
            if (isExist)
                return value;
            else
                throw new ArgumentException("Environment variable with key " + key + " does not exist");
        }
    }
}
