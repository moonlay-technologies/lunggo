using System.Linq;
using Lunggo.ApCommon.Flight.Model;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public SearchFlightOutput SearchFlight(SearchFlightInput input)
        {
            var inputTuple = input.Conditions.TripInfos;
            var conditions = new SearchFlightConditions();
            var output = new SearchFlightOutput();
            conditions.AdultCount = input.Conditions.AdultCount;
            conditions.ChildCount = input.Conditions.ChildCount;
            conditions.InfantCount = input.Conditions.InfantCount;
            conditions.CabinClass = input.Conditions.CabinClass;
            conditions.TripInfos = input.Conditions.TripInfos.Select(data => new TripInfo
            {
                OriginAirport = data.OriginAirport,
                DestinationAirport = data.DestinationAirport,
                DepartureDate = data.DepartureDate
            }).ToList();

            var result = SearchFlightInternal(conditions);

            output.Itineraries = result.FlightItineraries;

            if (!result.IsSuccess)
            {
                output.Errors = result.Errors;
                output.ErrorMessages = result.ErrorMessages;
            }
            return output;
        }
    }
}
