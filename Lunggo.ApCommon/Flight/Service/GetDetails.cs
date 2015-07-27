using System.Linq;
using System.Web.UI;
using Lunggo.ApCommon.Flight.Database.Logic;
using Lunggo.ApCommon.Flight.Database.Query;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Query;
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
                var tripInfos = GetFlightTripInfoQuery.GetInstance().Execute(conn, new { input.BookingId }).ToList();

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

                    DeleteFlightTripPerItineraryQuery.GetInstance().Execute(conn, details);
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

        public FlightReservation GetDetails(string rsvNo)
        {
            return GetFlightDb.Reservation(rsvNo);
        }

        private static GetDetailsOutput MapDetails(GetTripDetailsResult details)
        {
            return new GetDetailsOutput
            {
                BookingId = details.BookingId,
                BookingNotes = details.BookingNotes,
                FlightSegmentCount = details.FlightSegmentCount,
                FlightItineraryDetails = details.FlightItineraries,
                TotalFare = details.TotalFare,
                AdultTotalFare = details.AdultTotalFare,
                ChildTotalFare = details.ChildTotalFare,
                InfantTotalFare = details.InfantTotalFare,
                Currency = details.Currency
            };
        }
    }
}
