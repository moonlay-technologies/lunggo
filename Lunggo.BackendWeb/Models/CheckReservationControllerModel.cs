using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;

namespace Lunggo.BackendWeb.Models
{
    public class CheckReservationControllerModel
    {
        public string NoRsv { get; set; }
        public PaymentInfo PaymentInfo { get; set; }
        public PaymentStatus Status { get; set; }
    }
}