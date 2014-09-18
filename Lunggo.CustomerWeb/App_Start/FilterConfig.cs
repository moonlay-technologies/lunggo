using System.Web;
using System.Web.Mvc;
using Lunggo.Framework.Filter;

namespace Lunggo.CustomerWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new LanguageFilterAttribute());
        }
    }
}
