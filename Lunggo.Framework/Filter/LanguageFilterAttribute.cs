using System;
using System.Web;
using System.Web.Mvc;
using Lunggo.Framework.Constant;
using Lunggo.Framework.Context;

namespace Lunggo.Framework.Filter
{
    public class LanguageFilterAttribute : ActionFilterAttribute
    {
        private static readonly String DefaultLanguageCode = SystemConstant.IndonesianLanguageCode;
        
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var langCode = GetLangCodeFromUrl(filterContext);
            SetLangCodeInHttpContext(langCode);
        }

        private String GetLangCodeFromUrl(ActionExecutingContext filterContext)
        {
            var langCode = (String)filterContext.RouteData.Values[SystemConstant.LangCodeUrlVariable];
            if (String.IsNullOrEmpty(langCode))
            {
                langCode = DefaultLanguageCode;
            }
            return langCode;
        }

        private void SetLangCodeInHttpContext(String langCode)
        {
            OnlineContext.SetActiveLanguageCode(langCode);
        }
    }
}
