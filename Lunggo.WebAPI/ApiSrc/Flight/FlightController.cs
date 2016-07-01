using System;
using System.Web.Http;
using Lunggo.ApCommon.Identity.Auth;
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
        [Authorize]
        [Route("v1/flight/{searchId}/{progress}")]
        public ApiResponseBase SearchFlights(string searchId, int progress)
        {
            try
            {
                var request = new FlightSearchApiRequest
                {
                    SearchId = searchId,
                    Progress = progress
                };
                var apiResponse = FlightLogic.SearchFlights(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Authorize]
        [Route("v1/flight/select")]
        public ApiResponseBase SelectFlight()
        {
            try
            {
                var request = Request.Content.ReadAsStringAsync().Result.Deserialize<FlightSelectApiRequest>();
                var apiResponse = FlightLogic.SelectFlight(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Authorize]
        [Route("v1/flight/itin/{token}")]
        public ApiResponseBase GetItinerary(string token)
        {
            try
            {
                var apiResponse = FlightLogic.GetItinerary(token);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Authorize]
        [Route("v1/flight/revalidate/{token}")]
        public ApiResponseBase RevalidateFlight(string token)
        {
            try
            {
                var request = new FlightRevalidateApiRequest
                {
                    Token = token
                };
                var apiResponse = FlightLogic.RevalidateFlight(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Authorize]
        [Route("v1/flight/book")]
        public ApiResponseBase BookFlight()
        {
            try
            {
                var request = Request.Content.ReadAsStringAsync().Result.Deserialize<FlightBookApiRequest>();
                var apiResponse = FlightLogic.BookFlight(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [UserAuthorize]
        [Route("v1/flight/issue")]
        public ApiResponseBase IssueFlight()
        {
            try
            {
                var request = Request.Content.ReadAsStringAsync().Result.Deserialize<FlightIssueApiRequest>();
                var apiResponse = FlightLogic.IssueFlight(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Authorize]
        [Route("v1/flight/check/{rsvNo}")]
        public ApiResponseBase CheckFlightIssuance(string rsvNo)
        {
            try
            {
                var request = new FlightIssuanceApiRequest { RsvNo = rsvNo };
                var apiResponse = FlightLogic.CheckFlightIssuance(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
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
        [Authorize]
        [Route("v1/flight/top")]
        public ApiResponseBase TopDestinations()
        {
            try
            {
                var apiResponse = FlightLogic.TopDestinations();
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }
    }
}
