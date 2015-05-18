using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Cors;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Logic;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights
{
    public class FlightsController : ApiController
    {
        [HttpGet]
        [LunggoCorsPolicy]
        [Route("api/v1/flights")]
        public FlightSearchApiResponse SearchFlights(HttpRequestMessage httpRequest, [FromUri] string request)
        {
            var apiRequest = JsonConvert.DeserializeObject<FlightSearchApiRequest>(request);
            var apiResponse = FlightLogic.SearchFlights(apiRequest);
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
        [Route("api/v1/flights/filter")]
        public FlightFilterApiResponse FilterFlights(HttpRequestMessage httpRequest, [FromUri] string request)
        {
            try
            {
                var apiRequest = JsonConvert.DeserializeObject<FlightFilterApiRequest>(request);
                var apiResponse = FlightLogic.FilterFlights(apiRequest);
                return apiResponse;
            }
            catch (Exception e)
            {
                if (e.Source == "Newtonsoft.Json")
                {
                    return new FlightFilterApiResponse
                    {
                        FlightList = null,
                        TotalFlightCount = 0,
                        OriginalRequest = null
                    };
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Route("api/v1/flights/revalidate")]
        public FlightRevalidateApiResponse RevalidateFlight(HttpRequestMessage httpRequest, [FromUri] FlightRevalidateApiRequest request)
        {
            var apiResponse = FlightLogic.RevalidateFlight(request);
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

        [HttpGet]
        [LunggoCorsPolicy]
        [Route("api/v1/flights/book")]
        public FlightBookingApiResponse BookFlight(HttpRequestMessage httpRequest, [FromUri] FlightBookingApiRequest apiRequest)
        {
            apiRequest.PassengerData = JsonConvert.DeserializeObject<List<PassengerData>>(apiRequest.PassengerDataJson);
            return FlightLogic.BookFlight(apiRequest);
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Route("api/v1/flights/cancel")]
        public FlightCancelApiResponse CancelFlightBooking(HttpRequestMessage httpRequest, [FromUri] FlightCancelApiRequest request)
        {
            return FlightLogic.CancelBooking(request);
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Route("api/v1/flights/details")]
        public FlightDetailsApiResponse GetFlightTripDetails(HttpRequestMessage httpRequest, [FromUri] FlightDetailsApiRequest request)
        {
            return FlightLogic.GetTripDetails(request);
        }
    }
}
