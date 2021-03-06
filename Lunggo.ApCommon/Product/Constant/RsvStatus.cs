﻿namespace Lunggo.ApCommon.Product.Constant
{
    public enum RsvStatus
    {
        Undefined = 0,
        InProcess = 1,
        Completed = 2,
        Expired = 3,
        Cancelled = 4,
        Failed = 5
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
                case RsvStatus.InProcess:
                    return "PROC";
                case RsvStatus.Completed:
                    return "COMP";
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
                case "PROC":
                    return RsvStatus.InProcess;
                case "COMP":
                    return RsvStatus.Completed;
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
