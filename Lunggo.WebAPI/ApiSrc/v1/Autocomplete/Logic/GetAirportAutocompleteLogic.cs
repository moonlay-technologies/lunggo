using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Lunggo.ApCommon.Autocomplete;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.WebAPI.ApiSrc.v1.Autocomplete.Model;

namespace Lunggo.WebAPI.ApiSrc.v1.Autocomplete.Logic
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
            });
            return new AutocompleteAirportsApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Success.",
                Airports = airports
            };
        }
    }
}