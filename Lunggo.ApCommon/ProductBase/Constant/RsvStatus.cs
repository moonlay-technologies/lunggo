using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.ProductBase.Constant
{
    public enum RsvStatus
    {
        Undefined = 0,
        Pending = 1,
        Paid = 2,
        Completed = 3,
        Cancelled = 4,
        Expired = 5
    }

    internal class RsvStatusCd
    {
        internal static string Mnemonic(RsvStatus rsvStatus)
        {
            switch (rsvStatus)
            {
                case RsvStatus.Pending:
                    return "PEND";
                case RsvStatus.Paid:
                    return "PAID";
                case RsvStatus.Completed:
                    return "COMP";
                case RsvStatus.Cancelled:
                    return "CANC";
                case RsvStatus.Expired:
                    return "EXPR";
                default:
                    return null;
            }
        }

        internal static RsvStatus Mnemonic(string rsvStatus)
        {
            switch (rsvStatus)
            {
                case "PEND":
                    return RsvStatus.Pending;
                case "PAID":
                    return RsvStatus.Paid;
                case "COMP":
                    return RsvStatus.Completed;
                case "CANC":
                    return RsvStatus.Cancelled;
                case "EXPR":
                    return RsvStatus.Expired;
                default:
                    return RsvStatus.Undefined;
            }
        }
    }
}
