namespace Lunggo.ApCommon.Activity.Constant
{
    public enum BookingStatus
    {
        Undefined = 0,
        Booked = 1,
        ForwardedToOperator = 2,
        Confirmed = 3,
        Ticketing = 4,
        Ticketed = 5,
        Cancelled = 6,
        ScheduleChanged = 7,
        Failed = 8,
        Denied = 9,
        CancelledByCustomer = 10,
        CancelledByAdmin = 11,
        CancelledByOperator = 12
    }
    internal class BookingStatusCd
    {
        internal static string Mnemonic(BookingStatus bookingStatus)
        {
            switch (bookingStatus)
            {
                case BookingStatus.Booked:
                    return "BOOK";
                case BookingStatus.ForwardedToOperator:
                    return "FORW";
                case BookingStatus.Confirmed:
                    return "CONF";
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
                case BookingStatus.Denied:
                    return "DENY";
                case BookingStatus.CancelledByCustomer:
                    return "CACU";
                case BookingStatus.CancelledByOperator:
                    return "CAOP";
                case BookingStatus.CancelledByAdmin:
                    return "CAAD";
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
                case "FORW":
                    return BookingStatus.ForwardedToOperator;
                case "CONF":
                    return BookingStatus.Confirmed;
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
                case "DENY":
                    return BookingStatus.Denied;
                case "CACU":
                    return BookingStatus.CancelledByCustomer;
                case "CAOP":
                    return BookingStatus.CancelledByOperator;
                case "CAAD":
                    return BookingStatus.CancelledByAdmin;
                default:
                    return BookingStatus.Undefined;
            }
        }
    }
}
