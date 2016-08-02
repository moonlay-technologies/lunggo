namespace Lunggo.ApCommon.Product.Constant
{
    public enum PlatformType
    {
        Undefined = 0,
        DesktopWebsite = 1,
        MobileWebsite = 2,
        IosApp = 3,
        AndroidApp = 4,
        WindowsPhoneApp = 5
    }

    internal class PlatformTypeCd
    {
        internal static string Mnemonic(PlatformType platformType)
        {
            switch (platformType)
            {
                case PlatformType.DesktopWebsite:
                    return "DWS";
                case PlatformType.MobileWebsite:
                    return "MWS";
                case PlatformType.IosApp:
                    return "IOA";
                case PlatformType.AndroidApp:
                    return "ANA";
                case PlatformType.WindowsPhoneApp:
                    return "WPA";
                default:
                    return null;
            }
        }

        internal static PlatformType Mnemonic(string platformType)
        {
            switch (platformType)
            {
                case "DWS":
                    return PlatformType.DesktopWebsite;
                case "MWS":
                    return PlatformType.MobileWebsite;
                case "IOA":
                    return PlatformType.IosApp;
                case "ANA":
                    return PlatformType.AndroidApp;
                case "WPA":
                    return PlatformType.WindowsPhoneApp;
                default:
                    return PlatformType.Undefined;
            }
        }
    }
}
