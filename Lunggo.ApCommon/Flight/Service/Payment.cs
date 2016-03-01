using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public void UpdateFlightPayment(string rsvNo, Payment.Model.Payment payment)
        {
            var isUpdated = UpdateDb.Payment(rsvNo, payment);
            if (isUpdated && payment.Status == PaymentStatus.Settled)
            {
                var issueInput = new IssueTicketInput { RsvNo = rsvNo };
                IssueTicket(issueInput);
            }
        }
    }
}

