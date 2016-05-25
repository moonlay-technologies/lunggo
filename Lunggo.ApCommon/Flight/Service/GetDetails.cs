using System;
using System.Linq;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Query;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public GetDetailsOutput GetAndUpdateNewDetails(GetDetailsInput input)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var conditions = GetBookingIdAndSupplierQuery.GetInstance().Execute(conn, new {input.RsvNo})
                    .Select(r => new TripDetailsConditions
                    {
                        BookingId = r.BookingId,
                        Supplier = SupplierCd.Mnemonic(r.SupplierCd)
                    })
                    .ToList();

                var output = new GetDetailsOutput();
                Parallel.ForEach(conditions, condition =>
                {
                    var tripInfoRecords = GetTripInfoQuery.GetInstance().Execute(conn, new { condition.BookingId });
                    condition.Trips = tripInfoRecords.Select(record => new FlightTrip
                    {
                        OriginAirport = record.OriginAirportCd,
                        DestinationAirport = record.DestinationAirportCd,
                        DepartureDate = record.DepartureDate.GetValueOrDefault()
                    }).ToList();
                    var detailsResult = new DetailsResult();
                    var response = GetTripDetailsInternal(condition);
                    if (response.IsSuccess)
                    {
                        detailsResult = MapDetails(response);
                        detailsResult.IsSuccess = true;

                        UpdateDetailsToDb(response);
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

        private GetTripDetailsResult GetTripDetailsInternal(TripDetailsConditions conditions)
        {
            var supplierName = conditions.Supplier;
            var supplier = Suppliers.Where(entry => entry.Value.SupplierName == supplierName).Select(entry => entry.Value).Single();
            var result = supplier.GetTripDetails(conditions);
            return result;
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
