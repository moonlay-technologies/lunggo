using System.Web.Mvc;
using Lunggo.Framework.Filter;

namespace Lunggo.CustomerWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(CreateGlobalErrorHandler());
            filters.Add(new LanguageFilterAttribute());
            filters.Add(new DeviceDetectionFilterAttribute());
        }

        private static HandleErrorAttribute CreateGlobalErrorHandler()
        {
            return new HandleErrorAttribute
            {
                Order = 1,
                View = "GlobalError"
            };
        }

    }
}
