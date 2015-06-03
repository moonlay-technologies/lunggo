using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Query.Logic
{
    internal class GetFlightDb
    {
        internal static FlightItineraryApi Summary(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var service = FlightService.GetInstance();
                var tripRecords = GetFlightTripSummaryQuery.GetInstance().Execute(conn, new { RsvNo = rsvNo });
                return new FlightItineraryApi
                {
                    FlightTrips = tripRecords.Select(tripRecord =>
                    {
                        var segmentRecords = GetFlightSegmentSummaryQuery.GetInstance()
                            .Execute(conn, new { tripRecord.TripId });
                        var tripSummary = service.ConvertToTripApi(tripRecord);
                        tripSummary.FlightSegments =
                            segmentRecords.Select(service.ConvertToSegmentApi).ToList();
                        return tripSummary;
                    }).ToList()
                };
            }
        }
    }
}
