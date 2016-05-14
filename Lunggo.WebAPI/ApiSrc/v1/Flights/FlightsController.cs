using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.Framework.Cors;
using Lunggo.Framework.Extension;
using Lunggo.WebAPI.ApiSrc.v1.Common.Model;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Logic;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Model;
using Lunggo.ApCommon.Flight.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights
{
    public class FlightsController : ApiController
    {
        [HttpGet]
        [LunggoCorsPolicy]
        [Route("v1/flights/{searchId}/{progress}")]
        public FlightSearchApiResponse SearchFlights(string searchId, int progress)
        {
            var request = new FlightSearchApiRequest
            {
                SearchId = searchId,
                Progress = progress
                };
            var apiResponse = FlightLogic.SearchFlights(request);
            return apiResponse;
            }

        [HttpPost]
        [LunggoCorsPolicy]
        [Route("v1/flights/select")]
        public FlightSelectApiResponse SelectFlight()
            {
            var request = Request.Content.ReadAsStringAsync().Result.Deserialize<FlightSelectApiRequest>();
            var apiResponse = FlightLogic.SelectFlight(request);
                return apiResponse;
            }

        [HttpGet]
        [LunggoCorsPolicy]
        [Route("v1/flights/revalidate/{token}")]
        public FlightRevalidateApiResponse RevalidateFlight(string token)
        {
            var request = new FlightRevalidateApiRequest
            {
                Token = token
            };
            var apiResponse = FlightLogic.RevalidateFlight(request);
            return apiResponse;
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Route("v1/flights/book")]
        public FlightBookApiResponse BookFlight()
        {
            var request = Request.Content.ReadAsStringAsync().Result.Deserialize<FlightBookApiRequest>();
            var apiResponse = FlightLogic.BookFlight(request);
            return apiResponse;
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Route("v1/flights/issue")]
        public ApiResponseBase IssueFlight()
        {
            var request = Request.Content.ReadAsStringAsync().Result.Deserialize<FlightIssueApiRequest>();
            var apiResponse = FlightLogic.IssueFlight(request);
            return apiResponse;
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Route("v1/flights/check/{rsvNo}")]
        public FlightIssuanceApiResponse CheckFlightIssuance(string rsvNo)
        {
            var request = new FlightIssuanceApiRequest
        {
                RsvNo = rsvNo
            };
            var apiResponse = FlightLogic.CheckFlightIssuance(request);
            return apiResponse;
        }

        //[HttpGet]
        //[LunggoCorsPolicy]
        //[Route("v1/flights/{SearchId}/rules/{ItinIndex}")]
        //public FlightRulesApiResponse GetFlightRules(HttpRequestMessage httpRequest, [FromUri] FlightRulesApiRequest request)
        //{
        //    var apiResponse = FlightLogic.GetRules(request);
        //    return apiResponse;
        //}

        //[HttpPost]
        //[LunggoCorsPolicy]
        //[Route("v1/flights/cancel")]
        //public FlightCancelApiResponse CancelFlightBooking(HttpRequestMessage httpRequest, [FromUri] FlightCancelApiRequest request)
        //{
        //    return FlightLogic.CancelBooking(request);
        //}

        //[HttpPost]
        //[LunggoCorsPolicy]
        //[Route("v1/flights/expire")]
        //public ExpireReservationsApiResponse ExpireFlightReservations(HttpRequestMessage httpRequest)
        //{
        //    return FlightLogic.ExpireReservations();
        //}

        [HttpGet]
        [LunggoCorsPolicy]
        [Route("v1/flights/top")]
        public TopDestinationsApiResponse TopDestinations()
        {
            var apiResponse = FlightLogic.TopDestinations();
            return apiResponse;
        }
    }
}
