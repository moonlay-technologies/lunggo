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
            });
            return new AutocompleteAirlinesApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Success.",
                Airlines = airlines
            };
        }
    }
}