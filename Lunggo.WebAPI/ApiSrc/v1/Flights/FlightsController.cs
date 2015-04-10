using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Logic;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Model;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights
{
    public class FlightsController : ApiController
    {
        [EnableCors(origins: "http://localhost,https://localhost,http://localhost:23321,https://localhost:23321", headers: "*", methods: "*")]
        [Route("api/v1/flights")]
        public FlightSearchApiResponse GetFlights(HttpRequestMessage httpRequest, [FromUri] FlightSearchApiRequest request)
        {
            return FlightLogic.GetFlights(request);
        }

        [EnableCors(origins: "http://localhost,https://localhost,http://localhost:23321,https://localhost:23321", headers: "*", methods: "*")]
        [Route("api/v1/flights/revalidate")]
        public FlightRevalidateApiResponse RevalidateFlight(HttpRequestMessage httpRequest, [FromUri] FlightRevalidateApiRequest request)
        {
            return FlightLogic.RevalidateFlight(request);
        }

        [EnableCors(origins: "http://localhost,https://localhost,http://localhost:23321,https://localhost:23321", headers: "*", methods: "*")]
        [Route("api/v1/flights/book")]
        public FlightBookingApiResponse BookFlight(HttpRequestMessage httpRequest, [FromUri] FlightBookingApiRequest request)
        {
            return FlightLogic.BookFlight(request);
        }

        [EnableCors(origins: "http://localhost,https://localhost,http://localhost:23321,https://localhost:23321", headers: "*", methods: "*")]
        [Route("api/v1/flights/cancel")]
        public FlightCancelApiResponse CancelFlightBooking(HttpRequestMessage httpRequest, [FromUri] FlightCancelApiRequest request)
        {
            return FlightLogic.CancelBooking(request);
        }

        [EnableCors(origins: "http://localhost,https://localhost,http://localhost:23321,https://localhost:23321", headers: "*", methods: "*")]
        [Route("api/v1/flights/details")]
        public FlightDetailsApiResponse GetFlightTripDetails(HttpRequestMessage httpRequest, [FromUri] FlightDetailsApiRequest request)
        {
            return FlightLogic.GetTripDetails(request);
        }
    }
}
