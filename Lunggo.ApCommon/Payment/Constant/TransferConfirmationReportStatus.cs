using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Payment.Constant
{
    public enum TransferConfirmationReportStatus
    {
        Unchecked = 0,
        Confirmed = 1,
        Invalid = 2
    }

    internal class TransferConfirmationReportStatusCd
    {
        internal static string Mnemonic(TransferConfirmationReportStatus transferConfirmationReportStatus)
        {
            switch (transferConfirmationReportStatus)
            {
                case TransferConfirmationReportStatus.Unchecked:
                    return "UNC";
                case TransferConfirmationReportStatus.Confirmed:
                    return "CON";
                case TransferConfirmationReportStatus.Invalid:
                    return "INV";
                default:
                    return "";
            }
        }

        internal static TransferConfirmationReportStatus Mnemonic(string transferConfirmationReportStatus)
        {
            switch (transferConfirmationReportStatus)
            {
                case "UNC":
                    return TransferConfirmationReportStatus.Unchecked;
                case "CON":
                    return TransferConfirmationReportStatus.Confirmed;
                case "INV":
                    return TransferConfirmationReportStatus.Invalid;
                default:
                    return TransferConfirmationReportStatus.Unchecked;
            }
        }
    }
}
