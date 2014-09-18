using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Lunggo.Framework.Context;
using Lunggo.Framework.Message;

namespace Lunggo.Framework.HtmlHelperExtension
{
    public static class HtmlHelperExtension
    {
        private class ScriptBlock : IDisposable
        {
            private const string ScriptsKey = "scripts";
            public static List<string> PageScripts
            {
                get
                {
                    if (HttpContext.Current.Items[ScriptsKey] == null)
                        HttpContext.Current.Items[ScriptsKey] = new List<string>();
                    return (List<string>)HttpContext.Current.Items[ScriptsKey];
                }
            }

            WebViewPage webPageBase;

            public ScriptBlock(WebViewPage webPageBase)
            {
                this.webPageBase = webPageBase;
                this.webPageBase.OutputStack.Push(new StringWriter());
            }

            public void Dispose()
            {
                PageScripts.Add(((StringWriter)this.webPageBase.OutputStack.Pop()).ToString());
            }
        }


        public static IDisposable BeginScripts(this HtmlHelper helper)
        {
            return new ScriptBlock((WebViewPage)helper.ViewDataContainer);
        }

        public static MvcHtmlString PageScripts(this HtmlHelper helper)
        {
            return MvcHtmlString.Create(string.Join(Environment.NewLine, ScriptBlock.PageScripts.Select(s => s.ToString())));
        }

        public static MvcHtmlString ConvertToReadMore(this HtmlHelper helper, string str, int length)
        {
            return MvcHtmlString.Create(str.Length <= length ? str : string.Format("{0}...", str.Substring(0, length - 3)));
        }

        public static MvcHtmlString I18NString(this HtmlHelper helper, String code)
        {
            var messageManager = MessageManager.GetInstance();
            var activeLangCode = OnlineContext.GetActiveLanguageCode();
            return MvcHtmlString.Create(messageManager.GetMessageValue(code,LangToIsoCodeMapper.GetIsoCode(activeLangCode)));
        }

    }
}
