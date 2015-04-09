using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Logic;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Model;
using Lunggo.WebAPI.ApiSrc.v1.Hotels.Logic;
using Lunggo.WebAPI.ApiSrc.v1.Hotels.Object;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights
{
    public class FlightsController : ApiController
    {
        [EnableCors(origins: "http://localhost,https://localhost,http://localhost:23321,https://localhost:23321", headers: "*", methods: "*")]
        [Route("api/v1/flights")]
        public FlightSearchApiResponse GetFlights(HttpRequestMessage httpRequest, [FromUri] FlightSearchApiRequest request)
        {
            var checkedRequest = CreateFlightSearchApiRequestIfNull(request);
            var response = GetFlightLogic.GetFlights(checkedRequest);
            return response;
        }

        private FlightSearchApiRequest CreateFlightSearchApiRequestIfNull(FlightSearchApiRequest request)
        {
            return request ?? new FlightSearchApiRequest();
        }
    }
}
