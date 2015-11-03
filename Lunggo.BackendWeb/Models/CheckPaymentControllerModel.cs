using Lunggo.ApCommon.Payment.Constant;

namespace Lunggo.BackendWeb.Models
{
    public class CheckPaymentControllerModel
    {
        public string RsvNo { get; set; }
        public TransferConfirmationReportStatus Status { get; set; }
    }
}