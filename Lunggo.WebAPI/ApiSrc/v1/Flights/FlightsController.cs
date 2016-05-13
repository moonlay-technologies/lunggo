using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.Framework.Cors;
using Lunggo.Framework.Extension;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Logic;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Model;
using Lunggo.ApCommon.Flight.Model;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights
{
    public class FlightsController : ApiController
    {
        [HttpGet]
        [LunggoCorsPolicy]
        [Route("api/v1/flights")]
        public FlightSearchApiResponse SearchFlights(HttpRequestMessage httpRequest, [FromUri] string request)
        {
            FlightSearchApiRequest structuredRequest;
            try
            {
                structuredRequest = request.Deserialize<FlightSearchApiRequest>();
            }
            catch
            {
                return new FlightSearchApiResponse
                {
                    SearchId = null,
                    OriginalRequest = null,
                    TotalFlightCount = 0,
                    FlightList = new List<FlightItineraryForDisplay>(),
                    GrantedRequests = new List<int>(),
                    ExpiryTime = null,
                    MaxRequest = 0
                };
            }

            var apiResponse = FlightLogic.SearchFlights(structuredRequest);
            return apiResponse;

            /*
            try
            {
                var apiRequest = JsonConvert.DeserializeObject<FlightSearchApiRequest>(request);
                var apiResponse = FlightLogic.SearchFlights(apiRequest);
                return apiResponse;
            }
            catch (Exception e)
            {
                if (e.Source == "Newtonsoft.Json")
                {
                    return new FlightSearchApiResponse
                    {
                        SearchId = null,
                        OriginalRequest = null,
                        TotalFlightCount = 0,
                        FlightList = null
                    };
                }
                else
                {
                    throw;
                }
            }
            */
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
            // HARDCODE-AN
            // HARDCODE-AN
            // HARDCODE-AN
            // HARDCODE-AN
            // HARDCODE-AN
            // HARDCODE-AN
            // HARDCODE-AN
            // HARDCODE-AN
            // HARDCODE-AN
            /*var now = DateTime.UtcNow.AddHours(7);
            if (now.DayOfWeek == DayOfWeek.Wednesday && now.Date >= new DateTime(2016, 1, 2) &&
                now.Date <= new DateTime(2016, 3, 31) && request.Payment.Method == PaymentMethod.CreditCard &&
                request.Payment.Data != null && request.Payment.Data.Data0 != null &&
                request.Payment.Data.Data0.StartsWith("4"))
                request.Payment.DiscountCode = "VWWVWW";
            if (now.DayOfWeek == DayOfWeek.Sunday && now.Date == new DateTime(2016, 2, 14) &&
                request.Payment.Method == PaymentMethod.CreditCard &&
                request.Payment.Data != null && request.Payment.Data.Data0 != null &&
                (request.Payment.Data.Data0.StartsWith("456798") ||
                request.Payment.Data.Data0.StartsWith("456799") ||
                request.Payment.Data.Data0.StartsWith("432449") ||
                request.Payment.Data.Data0.StartsWith("425857") ||
                request.Payment.Data.Data0.StartsWith("456798") ||
                request.Payment.Data.Data0.StartsWith("432449") ||
                request.Payment.Data.Data0.StartsWith("542260") ||
                request.Payment.Data.Data0.StartsWith("540731") ||
                request.Payment.Data.Data0.StartsWith("552239") ||
                request.Payment.Data.Data0.StartsWith("552338") ||
                request.Payment.Data.Data0.StartsWith("559228") ||
                request.Payment.Data.Data0.StartsWith("516634") ||
                request.Payment.Data.Data0.StartsWith("542260") ||
                request.Payment.Data.Data0.StartsWith("552239") ||
                request.Payment.Data.Data0.StartsWith("523983") ||
                request.Payment.Data.Data0.StartsWith("523983") ||
                request.Payment.Data.Data0.StartsWith("552338") ||
                request.Payment.Data.Data0.StartsWith("552338")))
                request.Payment.DiscountCode = "DSVDSV16";*/
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
            //var apiResponse = new FlightBookApiResponse
            //{
            //    IsSuccess = true,
            //    NewPrice = Convert.ToDecimal(200000000),

            //};
            return apiResponse;
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Route("api/v1/flights/pay")]
        public FlightPaymentApiResponse Pay(HttpRequestMessage httpRequest, FlightPaymentApiRequest request)
        {
            var apiResponse = FlightLogic.SetPaymentFlight(request);
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
        [Route("api/v1/flights/topdestinations")]
        public TopDestinations TopDestinations(HttpRequestMessage httpRequest)
        {
            return FlightLogic.TopDestinations();
        }
    }
}
