using System;
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
        public static ApiResponseBase GetRateLogic(HotelRateApiRequest request)
        {
            if (!IsValid(request))
                return new HotelRateApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERHGRA01"
                };
            var getRateServiceRequest = PreprocessServiceRequest(request);
            var getRateServiceResponse = HotelService.GetInstance().GetRate(getRateServiceRequest);
            var apiResponse = AssembleApiResponse(getRateServiceResponse);
            if (apiResponse.Rooms == null || apiResponse.Rooms.Count == 0)
            {
                return new HotelRateApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERHGRA02"
                };
            }

            if (apiResponse.Rooms.Exists(r => r.Rates == null || r.Rates.Count == 0))
            {
                return new HotelRateApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERHGRA03"
                };
            }

            if (apiResponse.Rooms[0].Rates[0].TimeLimit <= DateTime.UtcNow)
            {
                return new HotelRateApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERHGRA04"
                };
            }
                
            var log = LogService.GetInstance();
            var env = EnvVariables.Get("general", "environment");
            
            return apiResponse;
        }

        private static bool IsValid(HotelRateApiRequest request)
        {
            return
                request != null &&
                request.SearchId != null &&
                request.HotelCode != null;
        }

        private static GetHotelRateInput PreprocessServiceRequest(HotelRateApiRequest request)
        {
            var getHotelRateServiceRequest = new GetHotelRateInput
            {
                HotelCode = request.HotelCode,
                SearchId = request.SearchId
            };
            return getHotelRateServiceRequest;
        }

        private static HotelRateApiResponse AssembleApiResponse(GetHotelRateOutput getHotelRateServiceResponse)
        {
            if (getHotelRateServiceResponse == null)
            {
                return new HotelRateApiResponse();
            }

            var rooms = HotelService.GetInstance().ConvertToHotelRoomsForDisplay(getHotelRateServiceResponse.Rooms);

            return new HotelRateApiResponse
            {
                SearchId = getHotelRateServiceResponse.SearchId,
                Rooms = rooms,
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}
