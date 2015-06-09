using System.Linq;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Query;
using Lunggo.ApCommon.Flight.Query.Logic;
using Lunggo.ApCommon.Flight.Query.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public GetDetailsOutput GetAndUpdateNewDetails(GetDetailsInput input)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                if (input.BookingId == null)
                    input.BookingId = GetFlightBookingIdQuery.GetInstance().Execute(conn, input.RsvNo).Single();
                var tripInfos = GetFlightTripInfoQuery.GetInstance().Execute(conn, input.RsvNo).ToList();

                var output = new GetDetailsOutput();
                var request = new TripDetailsConditions
                {
                    BookingId = input.BookingId,
                    TripInfos = tripInfos
                };
                var details = GetTripDetailsInternal(request);
                if (details.IsSuccess)
                {
                    output = MapDetails(details);
                    output.IsSuccess = true;
                    
                    var detailsRecord = new FlightDetailsRecord
                    {
                        BookingId = details.BookingId,
                        Segments = details.FlightItineraryDetails.FlightTrips.SelectMany(trip => trip.FlightSegments).ToList(),
                        Passengers = details.FlightItineraryDetails.PassengerInfo
                    };
                    InsertFlightDb.Details(detailsRecord);
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

        private static GetDetailsOutput MapDetails(GetTripDetailsResult details)
        {
            return new GetDetailsOutput
            {
                BookingId = details.BookingId,
                BookingNotes = details.BookingNotes,
                FlightSegmentCount = details.FlightSegmentCount,
                FlightItineraryDetails = details.FlightItineraryDetails,
                TotalFare = details.TotalFare,
                AdultTotalFare = details.AdultTotalFare,
                ChildTotalFare = details.ChildTotalFare,
                InfantTotalFare = details.InfantTotalFare,
                Currency = details.Currency
            };
        }
    }
}
