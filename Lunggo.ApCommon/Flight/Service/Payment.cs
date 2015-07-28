using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lunggo.ApCommon.Flight.Database.Query;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public bool UpdateFlightPayment(string rsvNo, PaymentInfo info)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var prevStatusCd = GetFlightPaymentStatusQuery.GetInstance().Execute(conn, new { RsvNo = rsvNo }).Single();
                var prevStatus = PaymentStatusCd.Mnemonic(prevStatusCd);
                if (info.Status != prevStatus)
                {
                    var queryParam = new FlightReservationTableRecord
                    {
                        RsvNo = rsvNo,
                        PaymentMediumCd = PaymentMediumCd.Mnemonic(info.Medium),
                        PaymentMethodCd = PaymentMethodCd.Mnemonic(info.Method),
                        PaymentStatusCd = PaymentStatusCd.Mnemonic(info.Status),
                        PaymentTime = info.Time,
                        PaymentId = info.Id,
                        PaymentTargetAccount = info.TargetAccount,
                        FinalPrice = info.FinalPrice
                    };
                    UpdateFlightPaymentQuery.GetInstance().Execute(conn, queryParam, queryParam);
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

