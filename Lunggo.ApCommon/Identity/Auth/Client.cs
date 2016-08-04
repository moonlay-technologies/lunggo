using Lunggo.ApCommon.Product.Constant;
using Lunggo.Framework.Encoder;

namespace Lunggo.ApCommon.Identity.Auth
{
    public class Client
    {
        public string Id { get; set; }
        public string Secret { get; set; }
        public string Name { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public bool IsActive { get; set; }
        public string AllowedOrigin { get; set; }

        public static PlatformType GetPlatformType(string clientId)
        {
            try
            {
                var decodedClientId = clientId.Base64Decode().Base64Decode().Base64Decode();
                var platformCode = decodedClientId.Split(':')[0];
                switch (platformCode)
                {
                    case "mpns":
                        return PlatformType.WindowsPhoneApp;
                    case "gcm":
                        return PlatformType.AndroidApp;
                    case "apns":
                        return PlatformType.IosApp;
                    case "dws":
                        return PlatformType.DesktopWebsite;
                    case "mws":
                        return PlatformType.MobileWebsite;
                    default:
                        return PlatformType.Undefined;
                }
            }
            catch
            {
                return PlatformType.Undefined;
            }
        }
    }
}
