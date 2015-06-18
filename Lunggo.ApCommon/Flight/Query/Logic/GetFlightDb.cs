using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages.Scope;
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
                var tripRecords = GetFlightTripSummaryQuery.GetInstance().Execute(conn, new { RsvNo = rsvNo }).ToList();
                return tripRecords.Any()
                    ? new FlightItineraryApi
                    {
                        FlightTrips = tripRecords.Select(tripRecord =>
                        {
                            var segmentRecords = GetFlightSegmentSummaryQuery.GetInstance()
                                .Execute(conn, new {tripRecord.TripId});
                            var tripSummary = service.ConvertToTripApi(tripRecord);
                            tripSummary.FlightSegments =
                                segmentRecords.Select(service.ConvertToSegmentApi).ToList();
                            return tripSummary;
                        }).ToList(),
                        TotalFare = GetFlightTotalFareQuery.GetInstance().Execute(conn, new {RsvNo = rsvNo}).Sum(),
                        Currency = GetFlightLocalCurrencyQuery.GetInstance().Execute(conn, new {RsvNo = rsvNo}).Single()
                    }
                    : null;
            }
        }

        internal static FlightItineraryDetails Details(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var service = FlightService.GetInstance();
                var tripRecords = GetFlightTripSummaryQuery.GetInstance().Execute(conn, new { RsvNo = rsvNo }).ToList();
                return tripRecords.Any()
                    ? new FlightItineraryDetails
                    {
                        FlightTrips = tripRecords.Select(tripRecord =>
                        {
                            var segmentRecords = GetFlightSegmentSummaryQuery.GetInstance()
                                .Execute(conn, new { tripRecord.TripId });
                            var tripSummary = service.ConvertToTripDetails(tripRecord);
                            tripSummary.FlightSegments =
                                segmentRecords.Select(service.ConvertToSegmentDetails).ToList();
                            return tripSummary;
                        }).ToList(),
                        PassengerInfo = GetFlightPassengerSummaryQuery.GetInstance().Execute(conn, new { RsvNo = rsvNo}).Select(service.ConvertToPassengerDetails).ToList()
                    }
                    : null;
            }
        }
    }
}
