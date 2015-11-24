using System.Linq;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Database.Query;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public GetDetailsOutput GetAndUpdateNewDetails(GetDetailsInput input)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                if (input.BookingIds == null)
                    input.BookingIds = GetBookingIdQuery.GetInstance().Execute(conn, new { input.RsvNo }).ToList();
                var tripInfoRecords = GetTripInfoQuery.GetInstance().Execute(conn, new { BookingId = input.BookingIds });
                var tripInfos = tripInfoRecords.Select(record => new FlightTrip
                {
                    OriginAirport = record.OriginAirportCd,
                    DestinationAirport = record.DestinationAirportCd,
                    DepartureDate = record.DepartureDate.GetValueOrDefault()
                }).ToList();

                var output = new GetDetailsOutput();
                Parallel.ForEach(input.BookingIds, bookingId =>
                {
                    var detailsResult = new DetailsResult();
                    var request = new TripDetailsConditions
                    {
                        BookingId = bookingId,
                        Trips = tripInfos
                    };
                    var response = GetTripDetailsInternal(request);
                    if (response.IsSuccess)
                    {
                        detailsResult = MapDetails(response);
                        detailsResult.IsSuccess = true;

                        UpdateDb.Details(response);
                    }
                    else
                    {
                        detailsResult.IsSuccess = false;
                        response.Errors.ForEach(output.AddError);
                        response.ErrorMessages.ForEach(output.AddError);
                    }
                    output.DetailsResults.Add(detailsResult);
                });
                if (output.DetailsResults.TrueForAll(result => result.IsSuccess))
                {
                    output.IsSuccess = true;
                }
                else
                {
                    output.IsSuccess = false;
                    if (output.DetailsResults.Any(result => result.IsSuccess))
                        output.PartiallySucceed();
                    output.DistinguishErrors();
                }
                return output;
            }
        }

        public FlightReservationForDisplay GetDetails(string rsvNo)
        {
            var rsv = GetDb.Reservation(rsvNo);
            return ConvertToReservationForDisplay(rsv);
        }

        private static DetailsResult MapDetails(GetTripDetailsResult details)
        {
            return new DetailsResult
            {
                BookingId = details.BookingId,
                BookingNotes = details.BookingNotes,
                SegmentCount = details.FlightSegmentCount,
                Itinerary = details.Itinerary,
                TotalFare = details.TotalFare,
                AdultTotalFare = details.AdultTotalFare,
                ChildTotalFare = details.ChildTotalFare,
                InfantTotalFare = details.InfantTotalFare,
                Currency = details.Currency
            };
        }
    }
}
