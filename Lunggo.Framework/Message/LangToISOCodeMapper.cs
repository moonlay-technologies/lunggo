using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Constant;
using Microsoft.SqlServer.Server;

namespace Lunggo.Framework.Message
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
