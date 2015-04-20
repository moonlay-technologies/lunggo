using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using Lunggo.ApCommon.Autocomplete;
using Lunggo.ApCommon.Model;
using Lunggo.ApCommon.Trie;

namespace Lunggo.WebAPI.ApiSrc.v1.Autocomplete
{
    public class AutocompleteController : ApiController
    {
        [HttpGet]
        [EnableCors(origins: "http://localhost,https://localhost,http://localhost:23321,https://localhost:23321", headers: "*", methods: "*")]
        [Route("api/v1/autocomplete/lastupdate/{type}")]
        public DateTime LastUpdate(string type)
        {
            var autocompleteManager = AutocompleteManager.GetInstance();
            return autocompleteManager.GetDictionaryLastUpdate(type);
        }
        [HttpGet]
        [EnableCors(origins: "http://localhost,https://localhost,http://localhost:23321,https://localhost:23321", headers: "*", methods: "*")]
        [Route("api/v1/autocomplete/airline/{prefix}")]
        public IEnumerable<AirlineDict> Airline(string prefix)
        {
            var autocompleteManager = AutocompleteManager.GetInstance();
            return autocompleteManager.GetAirlineAutocomplete(prefix);
        }

        [HttpGet]
        [EnableCors(origins: "http://localhost,https://localhost,http://localhost:23321,https://localhost:23321", headers: "*", methods: "*")]
        [Route("api/v1/autocomplete/airport/{prefix}")]
        public IEnumerable<AirportDict> Airport(string prefix)
        {
            var autocompleteManager = AutocompleteManager.GetInstance();
            return autocompleteManager.GetAirportAutocomplete(prefix);
        }

        [HttpGet]
        [EnableCors(origins: "http://localhost,https://localhost,http://localhost:23321,https://localhost:23321", headers: "*", methods: "*")]
        [Route("api/v1/autocomplete/airport")]
        public IEnumerable<AirportDict> GetAllAirport()
        {
            var autocompleteManager = AutocompleteManager.GetInstance();
            return autocompleteManager.GetAirportAutocomplete("");
        }

        [HttpGet]
        [EnableCors(origins: "http://localhost,https://localhost,http://localhost:23321,https://localhost:23321", headers: "*", methods: "*")]
        [Route("api/v1/autocomplete/hotellocation/{prefix}")]
        public IEnumerable<object> HotelLocation(string prefix)
        {
            var autocompleteManager = AutocompleteManager.GetInstance();
            return autocompleteManager.GetHotelLocationAutocomplete(prefix);
        }
    }
}
