using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using Lunggo.ApCommon.Dictionary;
using Lunggo.ApCommon.Model;

namespace Lunggo.WebAPI.ApiSrc.v1.Autocomplete
{
    public class AutocompleteController : ApiController
    {
        [HttpGet]
        [EnableCors(origins: "http://localhost,https://localhost,http://localhost:23321,https://localhost:23321", headers: "*", methods: "*")]
        [Route("api/v1/autocomplete/airline")]
        public IEnumerable<Airline> Airline(string prefix)
        {
            return TrieIndex.Airline.GetAllSuggestionIds(prefix).Select(id => Code.Airline[id]);
        }

        [HttpGet]
        [EnableCors(origins: "http://localhost,https://localhost,http://localhost:23321,https://localhost:23321", headers: "*", methods: "*")]
        [Route("api/v1/autocomplete/airport")]
        public IEnumerable<Airport> Airport(string prefix)
        {
            return TrieIndex.Airport.GetAllSuggestionIds(prefix).Select(id => Code.Airport[id]);
        }

        [HttpGet]
        [EnableCors(origins: "http://localhost,https://localhost,http://localhost:23321,https://localhost:23321", headers: "*", methods: "*")]
        [Route("api/v1/autocomplete/hotellocation")]
        public IEnumerable<ApCommon.Dictionary.Hotel> Hotel(string prefix)
        {
            return TrieIndex.Hotel.GetAllSuggestionIds(prefix).Select(id => Code.Hotel[id]);
        }
    }
}
