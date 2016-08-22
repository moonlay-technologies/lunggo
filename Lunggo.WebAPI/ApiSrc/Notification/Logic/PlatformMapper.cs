using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.Framework.Notifications;

namespace Lunggo.WebAPI.ApiSrc.Notification.Logic
{
    public static partial class RegistrationLogic
    {
        private static bool TryMapPlatform(PlatformType platformType, out Platform platform)
        {
            switch (platformType)
            {
                case PlatformType.IosApp:
                    platform = Platform.Ios;
                    return true;
                case PlatformType.AndroidApp:
                    platform = Platform.Android;
                    return true;
                default:
                    platform = Platform.Ios;
                    return false;
            }
        }
    }
}