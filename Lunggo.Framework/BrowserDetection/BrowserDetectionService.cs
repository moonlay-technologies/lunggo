using System;
using System.Web;
using Lunggo.Framework.Constant;
using WURFL;
using WURFL.Config;

namespace Lunggo.Framework.BrowserDetection
{
    public class BrowserDetectionService
    {
        private static readonly BrowserDetectionService Instance = new BrowserDetectionService();
        private IWURFLManager _wurflManager;

        private BrowserDetectionService()
        {

        }

        public void Init(String dataFilePath)
        {
            var configurer = new InMemoryConfigurer().MainFile(dataFilePath);
            configurer.SetMatchMode(MatchMode.Performance);
            var manager = WURFLManagerBuilder.Build(configurer);
            _wurflManager = manager;
        }

        public static BrowserDetectionService GetInstance()
        {
            return Instance;
        }

        public bool IsRequestFromAndroidOrIphone(HttpRequest httpRequest)
        {
            var isApp = httpRequest.Headers["X-Client-ID"] != null || httpRequest.Headers["X-Requested-With"] != "host.exp.exponent";
            var userAgent = httpRequest.UserAgent.ToLower();
            return !isApp && (userAgent.Contains("android") || userAgent.Contains("iphone"));
        }

        public bool IsRequestFromTablet()
        {
            var device = GetDevice();
            if (device != null)
            {
                var isTablet = false;
                var capability = device.GetCapability("is_tablet");
                Boolean.TryParse(capability, out isTablet);
                return isTablet;
            }
            else
            {
                return false;
            }
        }

        public bool IsRequestFromSmartphone()
        {
            var device = GetDevice();
            if (device != null)
            {
                var isSmartPhone = false;
                var capability = device.GetCapability("is_smartphone");
                Boolean.TryParse(capability, out isSmartPhone);
                return isSmartPhone;
            }
            else
            {
                return false;
            }
        }

        public bool IsRequestFromSmartphone(String userAgent)
        {
            var device = GetDevice(userAgent);
            if (device != null)
            {
                var isSmartPhone = false;
                var capability = device.GetCapability("is_smartphone");
                Boolean.TryParse(capability, out isSmartPhone);
                return isSmartPhone;
            }
            else
            {
                return false;
            }
        }

        private IDevice GetDevice()
        {
            var device = HttpContext.Current.Items[SystemConstant.HttpContextDevice] as IDevice;
            return device;
        }

        public IDevice GetDevice(String userAgent)
        {
            var device = _wurflManager.GetDeviceForRequest(userAgent);
            return device;
        }
    }
}
