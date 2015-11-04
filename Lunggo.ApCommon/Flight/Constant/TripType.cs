namespace Lunggo.ApCommon.Flight.Constant
{
    public enum TripType
    {
        Undefined = 0,
        OneWay = 1,
        Return = 2,
        MultiCity = 3,
        OpenJaw = 4,
        Circle = 5,
        Other = 6
    }

    internal class TripTypeCd
    {
        internal static string Mnemonic(TripType type)
        {
            switch (type)
            {
                case TripType.OneWay:
                    return "ONE";
                case TripType.Return:
                    return "RET";
                case TripType.OpenJaw:
                    return "JAW";
                case TripType.Circle:
                    return "CRC";
                case TripType.Other:
                    return "OTH";
                default:
                    return null;
            }
        }

        internal static TripType Mnemonic(string type)
        {
            switch (type)
            {
                case "ONE":
                    return TripType.OneWay;
                case "RET":
                    return TripType.Return;
                case "JAW":
                    return TripType.OpenJaw;
                case "CRC":
                    return TripType.OpenJaw;
                case "OTH":
                    return TripType.Other;
                default:
                    return TripType.Undefined;
            }
        }
    }
}
