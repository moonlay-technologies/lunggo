using System;
using System.Text;
using System.Web;
using Lunggo.Framework.Constant;

namespace Lunggo.Framework.Context
{
    public static class OnlineContext
    {
        public static String GetActiveLanguageCode()
        {
            return (String) HttpContext.Current.Items[SystemConstant.HttpContextLangCode];
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
