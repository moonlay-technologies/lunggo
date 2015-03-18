using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Lunggo.ApCommon.Dictionary;
using Lunggo.ApCommon.Model;

namespace Lunggo.BackendWeb.Controllers
{
    public class AutoCompleteController : ApiController
    {
        [HttpGet]
        public IEnumerable<Airline> Airline(string prefix)
        {
            return TrieIndex.Airline.GetAllSuggestionIds(prefix).Select(id => Code.Airline[id]);
        }
        [HttpGet]
        public IEnumerable<Airport> Airport(string prefix)
        {
            return TrieIndex.Airport.GetAllSuggestionIds(prefix).Select(id => Code.Airport[id]);
        }
        [HttpGet]
        public IEnumerable<ApCommon.Dictionary.Hotel> Hotel(string prefix)
        {
            return TrieIndex.Hotel.GetAllSuggestionIds(prefix).Select(id => Code.Hotel[id]);
        }
    }
}
