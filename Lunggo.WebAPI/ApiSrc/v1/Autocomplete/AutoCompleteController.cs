using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using Lunggo.ApCommon.Autocomplete;
using Lunggo.ApCommon.Dictionary;
using Lunggo.Framework.Cors;
using Lunggo.WebAPI.ApiSrc.v1.Autocomplete.Logic;
using Lunggo.WebAPI.ApiSrc.v1.Autocomplete.Model;

namespace Lunggo.WebAPI.ApiSrc.v1.Autocomplete
{
    public class AutocompleteController : ApiController
    {
        [HttpGet]
        [LunggoCorsPolicy]
        [Route("v1/autocomplete/airlines/{prefix}")]
        public AutocompleteAirlinesApiResponse Airlines(string prefix)
        {
            return AutocompleteLogic.GetAirlineAutocomplete(prefix);
        }

        [HttpGet]   
        [LunggoCorsPolicy]
        [Route("v1/autocomplete/airlines")]
        public AutocompleteAirlinesApiResponse Airlines()
        {
            return AutocompleteLogic.GetAirlineAutocomplete("");
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Route("v1/autocomplete/airports/{prefix}")]
        public AutocompleteAirportsApiResponse Airports(string prefix)
        {
            return AutocompleteLogic.GetAirportAutocomplete(prefix);
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Route("v1/autocomplete/airports")]
        public AutocompleteAirportsApiResponse Airports()
        {
            return AutocompleteLogic.GetAirportAutocomplete("");
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Route("v1/autocomplete/hotellocations/{prefix}")]
        public AutocompleteHotelLocationsApiResponse HotelLocations(string prefix)
        {
            return AutocompleteLogic.GetHotelLocationAutocomplete(prefix);
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Route("v1/autocomplete/lastupdate")]
        public AutocompleteLastUpdateApiResponse LastUpdate()
        {
            return new AutocompleteLastUpdateApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Test Success.",
                UpdateSets = new List<UpdateSet>
                {
                    new UpdateSet
                    {
                        Type = "airport",
                        UpdateTime = new DateTime(2015, 1, 1)
                    }
                }
            };
        }
    }
}
