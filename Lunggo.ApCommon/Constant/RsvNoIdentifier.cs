namespace Lunggo.ApCommon.Constant
{
    public static class RsvNoIdentifier
    {
        public const string Flight = "1";
        public const string Hotel = "2";
        public const string Activity = "4";

        public static bool IsFlightRsvNo(this string rsvNo)
        {
            return rsvNo.Substring(0, 1) == Flight;
        }

        public static bool IsHotelRsvNo(this string rsvNo)
        {
            return rsvNo.Substring(0, 1) == Hotel;
        }

        public static bool IsActivityRsvNo(this string rsvNo)
        {
            return rsvNo.Substring(0, 1) == Activity;
        }
    }
}
