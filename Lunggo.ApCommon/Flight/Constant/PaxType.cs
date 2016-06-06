namespace Lunggo.ApCommon.Flight.Constant
{
    public enum PaxType
    {
        Undefined = 0,
        Adult = 1,
        Child = 2,
        Infant = 3
    }

    internal class PaxTypeCd
    {
        internal static string Mnemonic(PaxType type)
        {
            switch (type)
            {
                case PaxType.Adult:
                    return "ADT";
                case PaxType.Child:
                    return "CHD";
                case PaxType.Infant:
                    return "INF";
                default:
                    return null;
            }
        }

        internal static PaxType Mnemonic(string type)
        {
            switch (type)
            {
                case "ADT":
                    return PaxType.Adult;
                case "CHD":
                    return PaxType.Child;
                case "INF":
                    return PaxType.Infant;
                default:
                    return PaxType.Undefined;
            }
        }
    }
}
