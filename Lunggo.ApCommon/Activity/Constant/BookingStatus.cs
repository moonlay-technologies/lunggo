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
            switch (bookingStatus.ToLower())
            {
                case "booked":
                    return BookingStatus.Booked;
                case "forwardedtooperator":
                    return BookingStatus.ForwardedToOperator;
                case "confirmed":
                    return BookingStatus.Confirmed;
                case "ticketing":
                    return BookingStatus.Ticketing;
                case "ticketed":
                    return BookingStatus.Ticketed;
                case "canc":
                    return BookingStatus.Cancelled;
                case "chgd":
                    return BookingStatus.ScheduleChanged;
                case "fail":
                    return BookingStatus.Failed;
                case "denied":
                    return BookingStatus.Denied;
                case "cancelbycustomer":
                    return BookingStatus.CancelByCustomer;
                case "cancelbyoperator":
                    return BookingStatus.CancelByOperator;
                case "cancelbyadmin":
                    return BookingStatus.CancelByAdmin;
                default:
                    return BookingStatus.Undefined;
            }
        }
    }
}
