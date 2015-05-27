using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Query;
using Lunggo.ApCommon.Flight.Query.Logic;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public FlightSummary GetFlightSummary(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            return new FlightSummary
            {
                RsvNo = rsvNo,
                Itinerary = GetFlightDb.Summary(rsvNo),
                Passengers = GetFlightPassengerSummaryQuery.GetInstance().Execute(conn, new {RsvNo = rsvNo}).Select(ConvertToPassengerApi).ToList(),
                PaymentInfo = ConvertFlightPaymentInfo(GetFlightPaymentInfoQuery.GetInstance().Execute(conn, new {RsvNo = rsvNo}).Single())
            };
        }
    }
}
