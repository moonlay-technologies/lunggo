using System;

namespace Lunggo.ApCommon.Product.Constant
{
    public enum PlatformType
    {
        Undefined = 0,
        DesktopWebsite = 1,
        MobileWebsite = 2,
        IosApp = 3,
        AndroidApp = 4,
        IosAppOperator = 5,
        AndroidAppOperator = 6,
    }

    public class PlatformTypeCd
    {
        public static string Mnemonic(PlatformType platformType)
        {
            switch (platformType)
            {
                case PlatformType.DesktopWebsite:
                    return "DesktopWebsite";
                case PlatformType.MobileWebsite:
                    return "MobileWebsite";
                case PlatformType.IosApp:
                    return "IosApp";
                case PlatformType.AndroidApp:
                    return "AndroidApp";
                case PlatformType.IosAppOperator:
                    return "IosAppOperator";
                case PlatformType.AndroidAppOperator:
                    return "AndroidAppOperator";
                default:
                    throw new NotImplementedException("Platform type not implemented: " + platformType);
            }
        }

        public static PlatformType Mnemonic(string platformType)
        {
            switch (platformType)
            {
                case "DWS":
                case "DesktopWebsite":
                    return PlatformType.DesktopWebsite;
                case "MWS":
                case "MobileWebsite":
                    return PlatformType.MobileWebsite;
                case "IOA":
                case "IosApp":
                    return PlatformType.IosApp;
                case "ANA":
                case "AndroidApp":
                    return PlatformType.AndroidApp;
                case "IosAppOperator":
                    return PlatformType.IosAppOperator;
                case "AndroidAppOperator":
                    return PlatformType.AndroidAppOperator;
                case null:
                    return PlatformType.Undefined;
                default:
                    throw new NotImplementedException("Platform type not implemented: " + platformType);
            }
        }
    }
}
