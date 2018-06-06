using System;
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

        public static (PlatformType type, string version) GetPlatform(string clientId)
        {
            var decodedClientId = DecodeClientId(clientId);
            var splitClientId = decodedClientId.Split(':');
            var platformType = ParsePlatformType(splitClientId[0]);
            var version = splitClientId[1];
            return (platformType, version);
        }

        public static string GetPlatfromVersion(string clientId)
        {
            var decodedClientId = DecodeClientId(clientId);
            var version = decodedClientId.Split(':')[1];
            return version;
        }

        public static PlatformType GetPlatformType(string clientId)
        {
            var decodedClientId = DecodeClientId(clientId);
            var platformCode = decodedClientId.Split(':')[0];
            return ParsePlatformType(platformCode);
        }

        private static PlatformType ParsePlatformType(string platformCode)
        {
            switch (platformCode)
            {
                case "gcm":
                case "rnandroid":
                    return PlatformType.AndroidApp;
                case "apns":
                case "rnios":
                    return PlatformType.IosApp;
                case "rniosop":
                    return PlatformType.IosAppOperator;
                case "rnandroidop":
                    return PlatformType.AndroidAppOperator;
                case "dws":
                    return PlatformType.DesktopWebsite;
                case "mws":
                    return PlatformType.MobileWebsite;
                default:
                    return PlatformType.Undefined;
            }
        }

        private static string DecodeClientId(string clientId)
        {
            return clientId.Base64Decode().Base64Decode().Base64Decode();
        }
    }
}
