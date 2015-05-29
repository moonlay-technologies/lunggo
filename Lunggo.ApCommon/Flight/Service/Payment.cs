using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lunggo.ApCommon.Flight.Query;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public bool UpdateFlightPayment(string rsvNo, PaymentMethod method, PaymentStatus status)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var prevStatusCd = GetFlightPaymentStatusQuery.GetInstance().Execute(conn, new { RsvNo = rsvNo }).Single();
                var prevStatus = PaymentStatusCd.Mnemonic(prevStatusCd);
                if (status != prevStatus)
                {
                    UpdateFlightPaymentQuery.GetInstance().Execute(conn, new
                    {
                        RsvNo = rsvNo,
                        PaymentMethodCd = PaymentMethodCd.Mnemonic(method),
                        PaymentStatusCd = PaymentStatusCd.Mnemonic(status)
                    });
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
