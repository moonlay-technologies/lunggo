using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Constant
{
    public static class RsvNoIdentifier
    {
        public const string Flight = "1";
        public const string Hotel = "2";
        public const string Activity = "4";

        public static bool IsFlightRsvNo(this string rsvNo)
        {
            return rsvNo.Substring(1, 1) == Flight;
        }

        public static bool IsHotelRsvNo(this string rsvNo)
        {
            return rsvNo.Substring(1, 1) == Hotel;
        }

        public static bool IsActiviryRsvNo(this string rsvNo)
        {
            return rsvNo.Substring(1, 1) == Activity;
        }
    }
}
