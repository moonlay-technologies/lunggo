using System;

namespace Lunggo.Framework.Constant
{
    public static class SystemConstant
    {
        //Start of HttpContext Items Key
        public static readonly String HttpContextLangCode = "--LunggoSystem.LangCode--";
        public static readonly String HttpContextCurrencyCode = "--LunggoSystem.CurrencyCode--";
        public static readonly String HttpContextPlatformCode = "--LunggoSystem.PlatformCode--";
        public static readonly String HttpContextDevice = "--LunggoSystem.Device--";
        //End of HttpContext Items Key

        public static readonly String LangCodeUrlVariable = "langCode";

        //Start of LanguageCode
        public static readonly String IndonesianLanguageCode = "id";
        public static readonly String EnglishLanguageCode = "en";
        //End of LanguageCode

        //Start of PlatformCode
        public static readonly String WebsitePlatformCode = "web";
        public static readonly String MobileAppPlatformCode = "mobApp";
        //End of PlatformCode
    }
}
