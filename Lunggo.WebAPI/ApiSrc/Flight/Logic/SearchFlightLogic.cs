using System.Linq;
using System.Net;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Config;
using Lunggo.Framework.Extension;
using Lunggo.Framework.Log;
using Lunggo.Framework.Pattern;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Flight.Model;

namespace Lunggo.WebAPI.ApiSrc.Flight.Logic
{
    public partial class FlightLogic : SingletonBase<FlightLogic>
    {
        public  ApiResponseBase SearchFlights(FlightSearchApiRequest request)
        {
            if (IsValid(request))
            {
                var searchServiceRequest = PreprocessServiceRequest(request);
                var searchServiceResponse = FlightService.GetInstance().SearchFlight(searchServiceRequest);
                var apiResponse = AssembleApiResponse(searchServiceResponse);
                return apiResponse;
            }
            else
            {
                return new FlightSearchApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERFSEA01"
                };
            }
        }

        private static bool IsValid(FlightSearchApiRequest request)
        {
            return FlightService.GetInstance().IsSearchIdValid(request.SearchId);
        }

        private static FlightSearchApiResponse AssembleApiResponse(SearchFlightOutput searchServiceResponse)
        {
            var apiResponse = new FlightSearchApiResponse
            {
                Flights = searchServiceResponse.ItineraryLists.Select(itinList => new Model.Flight
                {
                    Count = itinList.Count,
                    Itineraries = itinList
                }).ToList(),
                Combos = searchServiceResponse.Combos,
                ExpiryTime = searchServiceResponse.ExpiryTime.TruncateMilliseconds(),
                Progress = searchServiceResponse.Progress,
                StatusCode = HttpStatusCode.OK
            };
            return apiResponse;
        }

        private static SearchFlightInput PreprocessServiceRequest(FlightSearchApiRequest request)
        {
            var searchServiceRequest = new SearchFlightInput
            {
                SearchId = request.SearchId,
                Progress = request.Progress
            };
            return searchServiceRequest;
        }
    }
}