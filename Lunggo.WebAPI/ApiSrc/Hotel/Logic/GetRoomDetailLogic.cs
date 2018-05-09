using System.Linq;
using System.Net;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.Framework.Environment;
using Lunggo.Framework.Log;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Hotel.Model;


namespace Lunggo.WebAPI.ApiSrc.Hotel.Logic
{
    public static partial class HotelLogic
    {
        public static ApiResponseBase GetHotelRoomDetailLogic(HotelRoomDetailApiRequest request)
        {
            if (!IsValid(request))
                return new HotelRoomDetailApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERHGRD01"
                };
            var getRoomDetailServiceRequest = PreprocessServiceRequest(request);
            var getRoomDetailServiceResponse = HotelService.GetInstance().GetRoomDetail(getRoomDetailServiceRequest);
            var apiResponse = AssembleApiResponse(getRoomDetailServiceResponse);
            if (apiResponse.RoomDetails == null)
            {
                return new HotelRoomDetailApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERHGHD02"
                };
            }
            //if (apiResponse.RoomDetails.Rates == null || apiResponse.RoomDetails.Rates.Count == 0)
            //{
            //    return new HotelRoomDetailApiResponse
            //    {
            //        StatusCode = HttpStatusCode.BadRequest,
            //        ErrorCode = "ERHGHD03"
            //    };
            //}
            var log = LogService.GetInstance();
            var env = EnvVariables.Get("general", "environment");          
            return apiResponse;
        }

        private static bool IsValid(HotelRoomDetailApiRequest request)
        {
            return
                request != null &&
                request.SearchId != null &&
                request.HotelCode != null &&
                request.RoomCode != null;
        }

        private static GetRoomDetailInput PreprocessServiceRequest(HotelRoomDetailApiRequest request)
        {
            var getRoomDetailServiceRequest = new GetRoomDetailInput
            {
                RoomCode = request.RoomCode,
                HotelCode = request.HotelCode,
                SearchId = request.SearchId
            };
            return getRoomDetailServiceRequest;
        }

        private static HotelRoomDetailApiResponse AssembleApiResponse(GetRoomDetailOutput getRoomDetailServiceResponse)
        {
            if (getRoomDetailServiceResponse == null)
            {
                return new HotelRoomDetailApiResponse();
            }

            var room = HotelService.GetInstance().ConvertToSingleHotelRoomForDisplay(getRoomDetailServiceResponse.Room);
            return new HotelRoomDetailApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                RoomDetails = room,
            };
        }
    }
}
