namespace Lunggo.ApCommon.Identity.Auth
{
    public enum ApplicationType
    {
        Undefined = 0,
        JavaScript = 1,
        NativeConfidential = 2
    }

    internal class ApplicationTypeCd
    {
        internal static string Mnemonic(ApplicationType type)
        {
            switch (type)
            {
                case ApplicationType.JavaScript:
                    return "JS";
                case ApplicationType.NativeConfidential:
                    return "NC";
                default:
                    return "NA";
            }
        }

        internal static ApplicationType Mnemonic(string type)
        {
            switch (type)
            {
                case "JS":
                    return ApplicationType.JavaScript;
                case "NC":
                    return ApplicationType.NativeConfidential;
                default:
                    return ApplicationType.Undefined;
            }
        }
    }
}
