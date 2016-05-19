using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using Lunggo.Framework.Cors;
using Lunggo.WebAPI.ApiSrc.Autocomplete.Logic;
using Lunggo.WebAPI.ApiSrc.Autocomplete.Model;

namespace Lunggo.WebAPI.ApiSrc.Autocomplete
{
    public class AutocompleteController : ApiController
    {
        [HttpGet]
        [LunggoCorsPolicy]
        [Route("autocomplete/airlines/{prefix}")]
        public AutocompleteAirlinesApiResponse Airlines(string prefix)
        {
            return AutocompleteLogic.GetAirlineAutocomplete(prefix);
        }

        [HttpGet]   
        [LunggoCorsPolicy]
        [Route("autocomplete/airlines")]
        public AutocompleteAirlinesApiResponse Airlines()
        {
            return AutocompleteLogic.GetAirlineAutocomplete("");
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Route("autocomplete/airports/{prefix}")]
        public AutocompleteAirportsApiResponse Airports(string prefix)
        {
            return AutocompleteLogic.GetAirportAutocomplete(prefix);
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Route("autocomplete/airports")]
        public AutocompleteAirportsApiResponse Airports()
        {
            return AutocompleteLogic.GetAirportAutocomplete("");
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Route("autocomplete/hotellocations/{prefix}")]
        public AutocompleteHotelLocationsApiResponse HotelLocations(string prefix)
        {
            return AutocompleteLogic.GetHotelLocationAutocomplete(prefix);
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Route("autocomplete/lastupdate")]
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
