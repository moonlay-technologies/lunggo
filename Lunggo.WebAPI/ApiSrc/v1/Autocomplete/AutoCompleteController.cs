using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using Lunggo.ApCommon.Autocomplete;
using Lunggo.ApCommon.Dictionary;
using Lunggo.Framework.Cors;

namespace Lunggo.WebAPI.ApiSrc.v1.Autocomplete
{
    public class AutocompleteController : ApiController
    {
        [HttpGet]
        [LunggoCorsPolicy]
        [Route("api/v1/autocomplete/lastupdate")]
        public IEnumerable<UpdateSet> LastUpdate()
        {
            var autocompleteManager = AutocompleteManager.GetInstance();
            return autocompleteManager.GetDictionaryLastUpdate();
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Route("api/v1/autocomplete/airline/{prefix}")]
        public IEnumerable<AirlineDict> Airline(string prefix)
        {
            var autocompleteManager = AutocompleteManager.GetInstance();
            return autocompleteManager.GetAirlineAutocomplete(prefix);
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Route("api/v1/autocomplete/airport/{prefix}")]
        public IEnumerable<AirportDict> Airport(string prefix)
        {
            var autocompleteManager = AutocompleteManager.GetInstance();
            return autocompleteManager.GetAirportAutocomplete(prefix);
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Route("api/v1/autocomplete/airport")]
        public IEnumerable<AirportDict> GetAllAirport()
        {
            var autocompleteManager = AutocompleteManager.GetInstance();
            return autocompleteManager.GetAirportAutocomplete("");
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Route("api/v1/autocomplete/hotellocation/{prefix}")]
        public IEnumerable<HotelLocationApi> HotelLocation(string prefix)
        {
            var autocompleteManager = AutocompleteManager.GetInstance();
            return autocompleteManager.GetHotelLocationAutocomplete(prefix);
        }
    }
}
