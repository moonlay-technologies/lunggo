using System.Net;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Hotel.Model;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Logic
{
    public static partial class HotelLogic
    {
        public static ApiResponseBase GetSelectedHotelDetailLogic(HotelSelectedRoomApiRequest request)
        {
            if (!IsValid(request))
                return new HotelDetailApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERHGSH01"
                };
            //var searchServiceRequest = PreprocessServiceRequest(request);
            var selectedHotelServiceResponse = HotelService.GetInstance().GetSelectedHotelDetailsFromCache(request.Token);
            var apiResponse = AssembleApiResponse(selectedHotelServiceResponse);
            if (apiResponse.HotelDetails == null)
            {
                return new HotelSelectedRoomApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERHGSH02"
                };
            }
            if (apiResponse.HotelDetails.Rooms == null || apiResponse.HotelDetails.Rooms.Count == 0)
            {
                return new HotelSelectedRoomApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERHGSH03"
                };
            }

            if (apiResponse.HotelDetails.Rooms.Exists(r => r.Rates == null || r.Rates.Count == 0))
            {
                return new HotelRateApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERHGSH04"
                };
            }
            return apiResponse;
        }

        private static bool IsValid(HotelSelectedRoomApiRequest request)
        {
            if (request == null)
                return false;
            return
                request.Token != null;
        }
        
        private static HotelSelectedRoomApiResponse AssembleApiResponse(HotelDetailsBase response)
        {
            var apiResponse = new HotelSelectedRoomApiResponse
            {
                HotelDetails = response,
                StatusCode = HttpStatusCode.OK
            };
            return apiResponse;
        }
    }
}