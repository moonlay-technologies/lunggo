using System.Linq;
using System.Web.UI;

using Lunggo.ApCommon.Flight.Database.Query;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRepository;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public GetDetailsOutput GetAndUpdateNewDetails(GetDetailsInput input)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                if (input.BookingId == null)
                    input.BookingId = GetFlightBookingIdQuery.GetInstance().Execute(conn, new { input.RsvNo }).Single();
                var tripInfoRecords = GetFlightTripInfoQuery.GetInstance().Execute(conn, new { input.BookingId });
                var tripInfos = tripInfoRecords.Select(record => new FlightTrip
                {
                    OriginAirport = record.OriginAirportCd,
                    DestinationAirport = record.DestinationAirportCd,
                    DepartureDate = record.DepartureDate.GetValueOrDefault()
                }).ToList();

                var output = new GetDetailsOutput();
                var request = new TripDetailsConditions
                {   
                    BookingId = input.BookingId,
                    Trips = tripInfos
                };
                var details = GetTripDetailsInternal(request);
                if (details.IsSuccess)
                {
                    output = MapDetails(details);
                    output.IsSuccess = true;

                    DeleteFlightTripPerItineraryQuery.GetInstance().Execute(conn, new {details.BookingId});
                    InsertFlightDb.Details(details);
                }
                else
                {
                    output.IsSuccess = false;
                    output.Errors = details.Errors;
                    output.ErrorMessages = details.ErrorMessages;
                }
                return output;
            }
        }

        public FlightReservationForDisplay GetDetails(string rsvNo)
        {
            var rsv = GetFlightDb.Reservation(rsvNo);
            return ConvertToReservationApi(rsv);
        }

        private static GetDetailsOutput MapDetails(GetTripDetailsResult details)
        {
            return new GetDetailsOutput
            {
                BookingId = details.BookingId,
                BookingNotes = details.BookingNotes,
                FlightSegmentCount = details.FlightSegmentCount,
                FlightItinerary = details.FlightItineraries,
                TotalFare = details.TotalFare,
                AdultTotalFare = details.AdultTotalFare,
                ChildTotalFare = details.ChildTotalFare,
                InfantTotalFare = details.InfantTotalFare,
                Currency = details.Currency
            };
        }
    }
}
