using System;
using System.Collections.Generic;
using Lunggo.Framework.Constant;

namespace Lunggo.Framework.I18nMessage
{
    public static class LangToIsoCodeMapper
    {
        private static readonly Dictionary<String, String> LangIsoCodeDictionary;

        static LangToIsoCodeMapper()
        {
            LangIsoCodeDictionary = new Dictionary<String, String>();
            LangIsoCodeDictionary[SystemConstant.IndonesianLanguageCode] = "id_ID";
            LangIsoCodeDictionary[SystemConstant.EnglishLanguageCode] = "en_US";
        }

        public static String GetIsoCode(String langCode)
        {
            return LangIsoCodeDictionary[langCode];
        }
    }
}
