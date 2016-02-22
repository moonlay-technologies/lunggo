using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.Framework.Cors;
using Lunggo.Framework.Extension;
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
        [Route("v1/flights/{searchId}")]
        public FlightSearchApiResponse SearchFlights(HttpRequestMessage httpRequest, string searchId, string requests)
        {
            var request = new FlightSearchApiRequest
            {
                SearchId = searchId,
                Requests = (List<int>)new ListConverter<int>().ConvertFrom(requests)
            };
            var apiResponse = FlightLogic.SearchFlights(request);
            return apiResponse;
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Route("v1/flights/{searchId}/revalidate")]
        public FlightRevalidateApiResponse RevalidateFlight(HttpRequestMessage httpRequest, string searchId, string registers)
        {
            var request = new FlightRevalidateApiRequest
            {
                SearchId = searchId,
                ItinIndices = (List<int>)new ListConverter<int>().ConvertFrom(registers)
            };
            var apiResponse = FlightLogic.RevalidateFlight(request);
            return apiResponse;
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Route("v1/flights/book")]
        public FlightBookApiResponse BookFlight(HttpRequestMessage httpRequest, FlightBookApiRequest request)
        {
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
                now.Date <= new DateTime(2016, 3, 31) && request.Payment.Method == PaymentMethod.CreditCard &&
                request.Payment.Data != null && request.Payment.Data.Data0 != null &&
                request.Payment.Data.Data0.StartsWith("4"))
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
        public FlightIssueApiResponse IssueFlight(HttpRequestMessage httpRequest, FlightIssueApiRequest request)
        {
            var apiResponse = FlightLogic.IssueFlight(request);
            return apiResponse;
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Route("v1/flights/{SearchId}/rules/{ItinIndex}")]
        public FlightRulesApiResponse GetFlightRules(HttpRequestMessage httpRequest, [FromUri] FlightRulesApiRequest request)
        {
            var apiResponse = FlightLogic.GetRules(request);
            return apiResponse;
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Route("v1/flights/cancel")]
        public FlightCancelApiResponse CancelFlightBooking(HttpRequestMessage httpRequest, [FromUri] FlightCancelApiRequest request)
        {
            return FlightLogic.CancelBooking(request);
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Route("v1/flights/expire")]
        public ExpireReservationsApiResponse ExpireFlightReservations(HttpRequestMessage httpRequest)
        {
            return FlightLogic.ExpireReservations();
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Route("v1/flights/topdestinations")]
        public TopDestinationsApiResponse TopDestinations(HttpRequestMessage httpRequest)
        {
            return FlightLogic.TopDestinations();
        }
    }
}
