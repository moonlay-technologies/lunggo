namespace Lunggo.ApCommon.Flight.Constant
{
    public enum FareType
    {
        Undefined = 0,
        Published = 1,
        Private = 2,
        Consolidated = 3
    }

    internal class FareTypeCd
    {
        internal static string Mnemonic(FareType fareType)
        {
            switch (fareType)
            {
                case FareType.Published:
                    return "PUB";
                case FareType.Private:
                    return "PRI";
                case FareType.Consolidated:
                    return "CON";
                default:
                    return null;
            }
        }
        internal static FareType Mnemonic(string fareType)
        {
            switch (fareType)
            {
                case "PUB":
                    return FareType.Published;
                case "PRI":
                    return FareType.Private;
                case "CON":
                    return FareType.Consolidated;
                default:
                    return FareType.Undefined;
            }
        }
    }
}
