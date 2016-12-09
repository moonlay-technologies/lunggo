namespace Lunggo.ApCommon.Hotel.Constant
{
    public enum CheckRateStatus
    {
        Recheck= 0,
        Bookable = 1,
        Undefined = 2
    }
    internal class BookingStatusCd
    {
        internal static string Mnemonic(CheckRateStatus bookingStatus)
        {
            switch (bookingStatus)
            {
                case CheckRateStatus.Bookable:
                    return "BOOKABLE";
                case CheckRateStatus.Recheck:
                    return "RECHECK";
                default:
                    return null;
            }
        }

        internal static CheckRateStatus Mnemonic(string checkRateStatus)
        {
            switch (checkRateStatus)
            {
                case "BOOKABLE":
                    return CheckRateStatus.Bookable;
                case "RECHECK":
                    return CheckRateStatus.Recheck;
                default:
                    return CheckRateStatus.Undefined;
            }
        }
    }
}
