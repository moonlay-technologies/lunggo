using System;
using System.Text;
using System.Web;
using Lunggo.Framework.Constant;

namespace Lunggo.Framework.Context
{
    public static class OnlineContext
    {
        public static String GetActiveCurrencyCode()
        {
            return (String)HttpContext.Current.Items[SystemConstant.HttpContextCurrencyCode] ?? "IDR";
        }

        public static void SetActiveCurrencyCode(string currCode)
        {
            HttpContext.Current.Items[SystemConstant.HttpContextCurrencyCode] = currCode.ToUpper();
        }

        public static String GetActiveLanguageCode()
        {
            return (String)HttpContext.Current.Items[SystemConstant.HttpContextLangCode] ?? "ID";
        }

        public static void SetActiveLanguageCode(string langCode)
        {
            HttpContext.Current.Items[SystemConstant.HttpContextLangCode] = langCode.ToUpper();
        }

        public static String GetActivePlatformCode()
        {
            return (String)HttpContext.Current.Items[SystemConstant.HttpContextPlatformCode];
        }

        public static void SetActivePlatformCode(string platformCode)
        {
            HttpContext.Current.Items[SystemConstant.HttpContextPlatformCode] = platformCode;
        }

        public static String GetActiveDeviceCode()
        {
            return (String)HttpContext.Current.Items[SystemConstant.HttpContextDevice];
        }

        public static void SetActiveDeviceCode(string deviceCode)
        {
            HttpContext.Current.Items[SystemConstant.HttpContextDevice] = deviceCode;
        }

        public static String GetDefaultHomePageUrl()
        {
            var host = HttpContext.Current.Request.Url.Host;
            var port = HttpContext.Current.Request.Url.Port;

            var urlBuilder = new StringBuilder();
            urlBuilder.Append("http");
            urlBuilder.Append(System.Uri.SchemeDelimiter);
            urlBuilder.Append(host);
            if (port != 80 && port > 0)
            {
                urlBuilder.Append(":");
                urlBuilder.Append(port);
            }
            return urlBuilder.ToString();
        }
    }
}
