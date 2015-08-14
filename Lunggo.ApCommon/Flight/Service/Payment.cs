using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lunggo.ApCommon.Flight.Database.Logic;
using Lunggo.ApCommon.Flight.Database.Query;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public bool UpdateFlightPayment(string rsvNo, PaymentInfo payment)
        {
            return UpdateFlightDb.Payment(rsvNo, payment);
        }

        public void ConfirmReservationPayment(string rsvNo, decimal paidAmount)
        {
            UpdateFlightDb.ConfirmPayment(rsvNo, paidAmount);
        }
    }
}

