using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Flight.Constant
{
    public enum BookingStatus
    {
        Undefined = 0,
        Booked = 1,
        Ticketing = 2,
        Ticketed = 3,
        Cancelled = 4
    }
    public class BookingStatusCd
    {
        public static string Mnemonic(BookingStatus bookingStatus)
        {
            switch (bookingStatus)
            {
                case BookingStatus.Booked:
                    return "BOOK";
                case BookingStatus.Ticketing:
                    return "TKTG";
                case BookingStatus.Ticketed:
                    return "TKTD";
                case BookingStatus.Cancelled:
                    return "CANC";
                default:
                    return "";
            }
        }

        public static BookingStatus Mnemonic(string bookingStatus)
        {
            switch (bookingStatus)
            {
                case "BOOK":
                    return BookingStatus.Booked;
                case "TKTG":
                    return BookingStatus.Ticketing;
                case "TKTD":
                    return BookingStatus.Ticketed;
                case "CANC":
                    return BookingStatus.Cancelled;
                default:
                    return BookingStatus.Undefined;
            }
        }
    }
}
