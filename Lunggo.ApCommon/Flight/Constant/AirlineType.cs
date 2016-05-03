namespace Lunggo.ApCommon.Flight.Constant
{
    public enum AirlineType
    {
        Undefined = 0,
        Fsc = 1,
        Lcc = 2
    }

    internal class AirlineTypeCd
    {
        internal static string Mnemonic(AirlineType airlineType)
        {
            switch (airlineType)
            {
                case AirlineType.Fsc:
                    return "FSC";
                case AirlineType.Lcc:
                    return "LCC";
                default:
                    return null;
            }
        }
        internal static AirlineType Mnemonic(string airlineType)
        {
            switch (airlineType)
            {
                case "FSC":
                    return AirlineType.Fsc;
                case "LCC":
                    return AirlineType.Lcc;
                default:
                    return AirlineType.Undefined;
            }
        }
    }
}
