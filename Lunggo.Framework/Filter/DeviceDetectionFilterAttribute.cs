using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Lunggo.Framework.BrowserDetection;
using Lunggo.Framework.Constant;

namespace Lunggo.Framework.Filter
{
    public class DeviceDetectionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            SetDeviceInHttpContext(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            SetDeviceDetectionResultInControllerViewbag(filterContext);
        }

        private void SetDeviceInHttpContext(ActionExecutingContext filterContext)
        {
            var userAgent = filterContext.HttpContext.Request.UserAgent;
            var browserDetectionService = BrowserDetectionService.GetInstance();
            var device = browserDetectionService.GetDevice(userAgent);
            HttpContext.Current.Items[SystemConstant.HttpContextDevice] = device;
        }

        private void SetDeviceDetectionResultInControllerViewbag(ActionExecutedContext filterContext)
        {
            var browserDetectionService = BrowserDetectionService.GetInstance();
            var deviceType = "";
            if (browserDetectionService.IsRequestFromTablet())
            {
                deviceType = "tablet";
            }
            else if (browserDetectionService.IsRequestFromSmartphone())
            {
                deviceType = "smartphone";
            }
            else
            {
                deviceType = "unknown";
            }
            filterContext.Controller.ViewBag.DeviceTypeFromDeviceDetectionFilter = deviceType;
        }
    }
}
