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
        [Route("v1/flights/{searchId}/select/{registers}")]
        public FlightSelectApiResponse SelectFlight(string searchId, string registers)
        {
            var request = new FlightSelectApiRequest
            {
                SearchId = searchId,
                RegisterNumbers = (List<int>)new ListConverter<int>().ConvertFrom(registers)
            };
            var apiResponse = FlightLogic.SelectFlight(request);
            return apiResponse;
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Route("v1/flights/{searchId}/revalidate/{token}")]
        public FlightRevalidateApiResponse RevalidateFlight(string searchId, string token)
        {
            var request = new FlightRevalidateApiRequest
            {
                SearchId = searchId,
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
            // HARDCODE-AN
            // HARDCODE-AN
            // HARDCODE-AN
            // HARDCODE-AN
            // HARDCODE-AN
            // HARDCODE-AN
            // HARDCODE-AN
            // HARDCODE-AN
            // HARDCODE-AN
            var now = DateTime.UtcNow.AddHours(7);
            if (now.DayOfWeek == DayOfWeek.Wednesday && now.Date >= new DateTime(2016, 1, 2) &&
                now.Date <= new DateTime(2016, 3, 31) && request.PaymentData.Method == PaymentMethod.CreditCard &&
                request.PaymentData.Data != null && request.PaymentData.Data.DataString0 != null &&
                request.PaymentData.Data.DataString0.StartsWith("4"))
                request.DiscountCode = "VWWVWW";
            // HARDCODE-AN
            // HARDCODE-AN
            // HARDCODE-AN
            // HARDCODE-AN
            // HARDCODE-AN
            // HARDCODE-AN
            // HARDCODE-AN
            // HARDCODE-AN
            // HARDCODE-AN
            // HARDCODE-AN
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
            var apiResponse = FlightLogic.CheckFlightIssuance(rsvNo);
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

        [HttpPost]
        [LunggoCorsPolicy]
        [Route("v1/flights/topdestinations")]
        public TopDestinationsApiResponse TopDestinations(HttpRequestMessage httpRequest)
        {
            return FlightLogic.TopDestinations();
        }
    }
}
