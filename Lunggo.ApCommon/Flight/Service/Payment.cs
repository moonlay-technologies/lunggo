using Lunggo.ApCommon.Payment.Model;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public bool UpdateFlightPayment(string rsvNo, PaymentInfo payment)
        {
            return UpdateDb.Payment(rsvNo, payment);
        }

        public void ConfirmReservationPayment(string rsvNo, decimal paidAmount)
        {
            UpdateDb.ConfirmPayment(rsvNo, paidAmount);
        }
    }
}

