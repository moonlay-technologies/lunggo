using System.Linq;
using System.Net;
using Lunggo.ApCommon.Autocomplete;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.WebAPI.ApiSrc.Autocomplete.Model;

namespace Lunggo.WebAPI.ApiSrc.Autocomplete.Logic
{
    public static partial class AutocompleteLogic
    {
        public static AutocompleteAirlinesApiResponse GetAirlineAutocomplete(string prefix)
        {
            var flight = FlightService.GetInstance();
            var autocomplete = AutocompleteManager.GetInstance();
            var airlineIds = autocomplete.GetAirlineIdsAutocomplete(prefix);
            var airlines = airlineIds.Select(id =>
            {
                var airlineDict = flight.AirlineDict[id];
                return new AirlineApi
                {
                    Code = airlineDict.Code,
                    Name = airlineDict.Name
                };
            }).ToList();
            return new AutocompleteAirlinesApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                Airlines = airlines,
                Count = airlines.Count
            };
        }
    }
}