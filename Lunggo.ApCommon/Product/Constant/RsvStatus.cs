namespace Lunggo.ApCommon.Product.Constant
{
    public enum RsvStatus
    {
        Undefined = 0,
        Reserved = 1,
        Expired = 2,
        Cancelled = 3,
        Failed = 4
    }

    public enum RsvDisplayStatus
    {
        Undefined = 0,
        Reserved = 1,
        PendingPayment = 2,
        VerifyingPayment = 3,
        Paid = 4,
        Issued = 5,
        Expired = 6,
        PaymentDenied = 7,
        Cancelled = 8,
        FailedPaid = 9,
        FailedUnpaid = 10
    }

    internal class RsvStatusCd
    {
        internal static string Mnemonic(RsvStatus rsvStatus)
        {
            switch (rsvStatus)
            {
                case RsvStatus.Reserved:
                    return "RSVD";
                case RsvStatus.Expired:
                    return "EXPD";
                case RsvStatus.Cancelled:
                    return "CANC";
                case RsvStatus.Failed:
                    return "FAIL";
                default:
                    return null;
            }
        }

        internal static RsvStatus Mnemonic(string rsvStatus)
        {
            switch (rsvStatus)
            {
                case "RSVD":
                    return RsvStatus.Reserved;
                case "EXPD":
                    return RsvStatus.Expired;
                case "CANC":
                    return RsvStatus.Cancelled;
                case "FAIL":
                    return RsvStatus.Failed;
                default:
                    return RsvStatus.Undefined;
            }
        }
    }
}
