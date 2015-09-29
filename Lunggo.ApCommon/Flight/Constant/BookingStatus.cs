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
        Cancelled = 4,
        ScheduleChanged = 5,
        Failed = 6
    }
    internal class BookingStatusCd
    {
        internal static string Mnemonic(BookingStatus bookingStatus)
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
                case BookingStatus.ScheduleChanged:
                    return "CHGD";
                case BookingStatus.Failed:
                    return "FAIL";
                default:
                    return null;
            }
        }

        internal static BookingStatus Mnemonic(string bookingStatus)
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
                case "CHGD":
                    return BookingStatus.ScheduleChanged;
                case "FAIL":
                    return BookingStatus.Failed;
                default:
                    return BookingStatus.Undefined;
            }
        }
    }
}
