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
        CancelByCustomer = 10,
        CancelByAdmin = 11,
        CancelByOperator = 12
    }
    internal class BookingStatusCd
    {
        internal static string Mnemonic(BookingStatus bookingStatus)
        {
            switch (bookingStatus)
            {
                case BookingStatus.Booked:
                    return "Booked";
                case BookingStatus.ForwardedToOperator:
                    return "ForwardedToOperator";
                case BookingStatus.Confirmed:
                    return "Confirmed";
                case BookingStatus.Ticketing:
                    return "Ticketing";
                case BookingStatus.Ticketed:
                    return "Ticketed";
                case BookingStatus.Cancelled:
                    return "CANC";
                case BookingStatus.ScheduleChanged:
                    return "CHGD";
                case BookingStatus.Failed:
                    return "FAIL";
                case BookingStatus.Denied:
                    return "Denied";
                case BookingStatus.CancelByCustomer:
                    return "CancelByCustomer";
                case BookingStatus.CancelByOperator:
                    return "CancelByOperator";
                case BookingStatus.CancelByAdmin:
                    return "CancelByAdmin";
                default:
                    return null;
            }
        }

        internal static BookingStatus Mnemonic(string bookingStatus)
        {
            switch (bookingStatus)
            {
                case "Booked":
                    return BookingStatus.Booked;
                case "ForwardedToOperator":
                    return BookingStatus.ForwardedToOperator;
                case "Confirmed":
                    return BookingStatus.Confirmed;
                case "Ticketing":
                    return BookingStatus.Ticketing;
                case "Ticketed":
                    return BookingStatus.Ticketed;
                case "CANC":
                    return BookingStatus.Cancelled;
                case "CHGD":
                    return BookingStatus.ScheduleChanged;
                case "FAIL":
                    return BookingStatus.Failed;
                case "Denied":
                    return BookingStatus.Denied;
                case "CancelByCustomer":
                    return BookingStatus.CancelByCustomer;
                case "CancelByOperator":
                    return BookingStatus.CancelByOperator;
                case "CancelByAdmin":
                    return BookingStatus.CancelByAdmin;
                default:
                    return BookingStatus.Undefined;
            }
        }
    }
}
