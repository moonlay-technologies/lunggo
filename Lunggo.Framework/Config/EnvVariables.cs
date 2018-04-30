using System;
using System.Collections.Generic;

namespace Lunggo.Framework.Config
{
    public static partial class EnvVariables
    {
        private static readonly Dictionary<string, string> VarDictionary = PopulateDictionary();

        public static string Get(string categoryKey, string nameKey)
        {
            var key = string.Join(".", categoryKey, nameKey);
            var isExist = VarDictionary.TryGetValue(key, out var value);
            return isExist
                ? value
                : throw new ArgumentException("Environment variable with key " + key + " does not exist");
        }
    }
}
