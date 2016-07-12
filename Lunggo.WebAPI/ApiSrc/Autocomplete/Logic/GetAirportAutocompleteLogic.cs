using System.Linq;
using System.Net;
using Lunggo.ApCommon.Autocomplete;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.WebAPI.ApiSrc.Autocomplete.Model;

namespace Lunggo.WebAPI.ApiSrc.Autocomplete.Logic
{
    public partial class AutocompleteLogic
    {
        public static AutocompleteAirportsApiResponse GetAirportAutocomplete(string prefix)
        {
            var flight = FlightService.GetInstance();
            var autocomplete = AutocompleteManager.GetInstance();
            var airportIds = autocomplete.GetAirportIdsAutocomplete(prefix);
            var airports = airportIds.Select(id =>
            {
                var airportDict = flight.AirportDict[id]; 
                return new AirportApi
                {
                    Code = airportDict.Code,
                    Name = airportDict.Name,
                    City = airportDict.City,
                    Country = airportDict.Country
                };
            }).ToList();
            return new AutocompleteAirportsApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                Airports = airports,
                Count = airports.Count
            };
        }
    }
}