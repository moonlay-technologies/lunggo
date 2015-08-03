using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Database.Logic;
using Lunggo.ApCommon.Flight.Database.Query;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public FlightSummary GetFlightSummary(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var summary = GetFlightDb.Summary(rsvNo);
                if (summary != null)
                {
                    var passengers = GetFlightPassengerSummaryQuery.GetInstance()
                        .Execute(conn, new {RsvNo = rsvNo})
                        .Select(ConvertToPassengerApi)
                        .ToList();
                    var payment =
                        GetFlightPaymentInfoQuery.GetInstance().Execute(conn, new {RsvNo = rsvNo}).ToList();
                    if (payment.Any())
                        return new FlightSummary
                        {
                            RsvNo = rsvNo,
                            Itinerary = summary,
                            Passengers = passengers,
                            PaymentInfo = ConvertFlightPaymentInfo(payment.Single()),
                        };
                    else
                        return new FlightSummary
                        {
                            RsvNo = rsvNo,
                            Itinerary = summary,
                            Passengers = passengers,
                            PaymentInfo = new PaymentInfo()
                        };
                }
                else
                {
                    return new FlightSummary
                    {
                        RsvNo = rsvNo,
                        Itinerary = null,
                        Passengers = null,
                        PaymentInfo = new PaymentInfo()
                    };
                }
            }
        }
    }
}
