using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Constant
{
    public enum ActivityCancellationEnum
    {

    }

    public enum CustomerRefundStatus
    {
        Undefined = 0,
        NoRefund = 1,
        WaitCustomer = 2,
        Pending = 3,
        Completed = 4,
        Failed = 5,
    }

    internal class CustomerRefundStatusCd
    {
        internal static string Mnemonic(CustomerRefundStatus refundStatus)
        {
            switch (refundStatus)
            {
                case CustomerRefundStatus.NoRefund:
                    return "NoRefund";
                case CustomerRefundStatus.WaitCustomer:
                    return "WaitCustomer";
                case CustomerRefundStatus.Pending:
                    return "Pending";
                case CustomerRefundStatus.Completed:
                    return "Completed";
                case CustomerRefundStatus.Failed:
                    return "Failed";
                default:
                    return null;
            }
        }

        internal static CustomerRefundStatus Mnemonic(string refundStatus)
        {
            switch (refundStatus.ToLower())
            {
                case "NoRefund":
                    return CustomerRefundStatus.NoRefund;
                case "WaitCustomer":
                    return CustomerRefundStatus.WaitCustomer;
                case "Pending":
                    return CustomerRefundStatus.Pending;
                case "Completed":
                    return CustomerRefundStatus.Completed;
                case "Failed":
                    return CustomerRefundStatus.Failed;
                default:
                    return CustomerRefundStatus.Undefined;
            }
        }
    }
}
