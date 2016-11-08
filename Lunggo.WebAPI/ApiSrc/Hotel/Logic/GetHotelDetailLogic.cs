using System.Net;
using Lunggo.ApCommon.Hotel.Constant;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Hotel.Model;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Logic
{
    public static partial class HotelLogic
    {
        public static ApiResponseBase GetHotelDetailLogic(HotelDetailApiRequest request)
        {
            if (!IsValid(request))
                return new HotelDetailApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERHGHD01"
                };
            var searchServiceRequest = PreprocessServiceRequest(request);
            var searchServiceResponse = HotelService.GetInstance().GetHotelDetail(searchServiceRequest);
            var apiResponse = AssembleApiResponse(searchServiceResponse);
            if (apiResponse.HotelDetails == null)
            {
                return new HotelDetailApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERHGHD02"
                };
            }
            
            return apiResponse;
        }

        private static bool IsValid(HotelDetailApiRequest request)
        {
            if (request == null)
                return false;
            return
                request.HotelCode != 0;

        }

        private static GetHotelDetailInput PreprocessServiceRequest(HotelDetailApiRequest request)
        {
            var searchServiceRequest = new GetHotelDetailInput
            {
                SearchId = request.SearchId,
                HotelCode = request.HotelCode,
            };
            return searchServiceRequest;
        }

        private static HotelDetailApiResponse AssembleApiResponse(GetHotelDetailOutput hotelDetailServiceResponse)
        {
            if (hotelDetailServiceResponse.IsSuccess)
            {
                return new HotelDetailApiResponse
                {
                    HotelDetails = hotelDetailServiceResponse.HotelDetail,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                if (hotelDetailServiceResponse.Errors == null)
                    return new HotelDetailApiResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorCode = "ERHSEA99"
                    };
                switch (hotelDetailServiceResponse.Errors[0])
                {
                    case HotelError.InvalidInputData:
                        return new HotelDetailApiResponse
                        {
                            StatusCode = HttpStatusCode.InternalServerError,
                            ErrorCode = "ERHGHD01"
                        };
                    case HotelError.SearchIdNoLongerValid:
                        return new HotelDetailApiResponse
                        {
                            StatusCode = HttpStatusCode.Accepted,
                            ErrorCode = "ERHGHD02"
                        };
                    default:
                        return new HotelDetailApiResponse
                        {
                            StatusCode = HttpStatusCode.InternalServerError,
                            ErrorCode = "ERRGEN99"
                        };
                }
            }
        }
    }
}