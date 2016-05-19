using System.Web.Http;
using Lunggo.Framework.Cors;
using Lunggo.Framework.Extension;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Flight.Logic;
using Lunggo.WebAPI.ApiSrc.Flight.Model;

namespace Lunggo.WebAPI.ApiSrc.Flight
{
    public class FlightController : ApiController
    {
        [HttpGet]
        [LunggoCorsPolicy]
        [Route("flight/{searchId}/{progress}")]
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
        [Route("flight/select")]
        public FlightSelectApiResponse SelectFlight()
            {
            var request = Request.Content.ReadAsStringAsync().Result.Deserialize<FlightSelectApiRequest>();
            var apiResponse = FlightLogic.SelectFlight(request);
                return apiResponse;
            }

        [HttpGet]
        [LunggoCorsPolicy]
        [Route("flight/revalidate/{token}")]
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
        [Route("flight/book")]
        public FlightBookApiResponse BookFlight()
        {
            var request = Request.Content.ReadAsStringAsync().Result.Deserialize<FlightBookApiRequest>();
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
        [Route("flight/issue")]
        public ApiResponseBase IssueFlight()
        {
            var request = Request.Content.ReadAsStringAsync().Result.Deserialize<FlightIssueApiRequest>();
            var apiResponse = FlightLogic.IssueFlight(request);
            return apiResponse;
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Route("flight/check/{rsvNo}")]
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
        //[Route("flight/{SearchId}/rules/{ItinIndex}")]
        //public FlightRulesApiResponse GetFlightRules(HttpRequestMessage httpRequest, [FromUri] FlightRulesApiRequest request)
        //{
        //    var apiResponse = FlightLogic.GetRules(request);
        //    return apiResponse;
        //}

        //[HttpPost]
        //[LunggoCorsPolicy]
        //[Route("flight/cancel")]
        //public FlightCancelApiResponse CancelFlightBooking(HttpRequestMessage httpRequest, [FromUri] FlightCancelApiRequest request)
        //{
        //    return FlightLogic.CancelBooking(request);
        //}

        //[HttpPost]
        //[LunggoCorsPolicy]
        //[Route("flight/expire")]
        //public ExpireReservationsApiResponse ExpireFlightReservations(HttpRequestMessage httpRequest)
        //{
        //    return FlightLogic.ExpireReservations();
        //}

        [HttpGet]
        [LunggoCorsPolicy]
        [Route("flight/top")]
        public TopDestinationsApiResponse TopDestinations()
        {
            var apiResponse = FlightLogic.TopDestinations();
            return apiResponse;
        }
    }
}
