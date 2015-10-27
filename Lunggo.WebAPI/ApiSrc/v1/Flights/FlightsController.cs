using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Cors;
using Lunggo.Framework.Extension;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Logic;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Model;
using Newtonsoft.Json;
using Lunggo.ApCommon.Flight.Model;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights
{
    public class FlightsController : ApiController
    {
        [HttpGet]
        [LunggoCorsPolicy]
        [Route("api/v1/flights")]
        public FlightSearchApiResponse SearchFlights(HttpRequestMessage httpRequest, [FromUri] FlightSearchApiRequest request)
        {
            var apiResponse = FlightLogic.SearchFlights(request);
            return apiResponse;
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Route("api/v1/flights/revalidate")]
        public FlightRevalidateApiResponse RevalidateFlight(HttpRequestMessage httpRequest, [FromUri] FlightRevalidateApiRequest request)
        {
            var apiResponse = FlightLogic.RevalidateFlight(request);
            return apiResponse;
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Route("api/v1/flights/book")]
        public FlightBookApiResponse BookFlight(HttpRequestMessage httpRequest, FlightBookApiRequest request)
        {
            var apiResponse = FlightLogic.BookFlight(request);
            return apiResponse;
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Route("api/v1/flights/issue")]
        public FlightIssueApiResponse IssueFlight(HttpRequestMessage httpRequest, FlightIssueApiRequest request)
        {
            var apiResponse = FlightLogic.IssueFlight(request);
            return apiResponse;
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Route("api/v1/flights/rules")]
        public FlightRulesApiResponse GetFlightRules(HttpRequestMessage httpRequest, [FromUri] FlightRulesApiRequest request)
        {
            var apiResponse = FlightLogic.GetRules(request);
            return apiResponse;
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Route("api/v1/flights/cancel")]
        public FlightCancelApiResponse CancelFlightBooking(HttpRequestMessage httpRequest, [FromUri] FlightCancelApiRequest request)
        {
            return FlightLogic.CancelBooking(request);
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Route("api/v1/flights/expire")]
        public void ExpireFlightReservations(HttpRequestMessage httpRequest)
        {
            FlightLogic.ExpireReservations();
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Route("api/v1/flights/sync")]
        public void SyncPriceMarginRules(HttpRequestMessage httpRequest)
        {
            FlightLogic.SyncFlightMarginRules();
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Route("api/v1/flights/topdestinations")]
        public TopDestinations TopDestinations(HttpRequestMessage httpRequest)
        {
            return FlightLogic.TopDestinations();
        }
    }
}
