namespace Lunggo.ApCommon.Product.Constant
{
    public enum PlatformType
    {
        Website = 0,
        MobileApp = 1
    }

    internal class PlatformTypeCd
    {
        internal static string Mnemonic(PlatformType platformType)
        {
            switch (platformType)
            {
                case PlatformType.Website:
                    return "WEB";
                case PlatformType.MobileApp:
                    return "MAP";
                default:
                    return null;
            }
        }

        internal static PlatformType Mnemonic(string platformType)
        {
            switch (platformType)
            {
                case "WEB":
                    return PlatformType.Website;
                case "MAP":
                    return PlatformType.MobileApp;
                default:
                    return PlatformType.Website;
            }
        }

        internal static PlatformType FrameworkCode(string code)
        {
            switch (code)
            {
                case "web":
                    return PlatformType.Website;
                case "mobApp":
                    return PlatformType.MobileApp;
                default:
                    return PlatformType.Website;
            }
        }
    }
}
