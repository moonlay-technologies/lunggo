using System;
using System.Net;
using Lunggo.ApCommon.Hotel.Constant;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.Framework.Config;
using Lunggo.Framework.Log;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Hotel.Model;


namespace Lunggo.WebAPI.ApiSrc.Hotel.Logic
{
    public static partial class HotelLogic
    {
        public static ApiResponseBase SelectHotelRatesLogic(HotelSelectRoomApiRequest request)
        {
            if (IsValid(request))
            { 
                var selectRateServiceRequest = PreprocessServiceRequest(request);
                var selectRateServiceResponse = HotelService.GetInstance().SelectHotelRoom(selectRateServiceRequest);
                var apiResponse = AssembleApiResponse(selectRateServiceResponse);
                if (apiResponse.TimeLimit <= DateTime.UtcNow)
                {
                    return new HotelSelectRoomApiResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorCode = "ERHSEL02"
                    };
                }

                if (apiResponse.Token == null)
                {
                    return new HotelSelectRoomApiResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorCode = "ERHSEL03"
                    };
                }
                
                var log = LogService.GetInstance();
                var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
                //log.Post(
                //    "```Booking API Log```"
                //    + "\n`*Environment :* " + env.ToUpper()
                //    + "\n*REQUEST :*\n"
                //    + request.Serialize()
                //    + "\n*RESPONSE :*\n"
                //    + apiResponse.Serialize()
                //    + "\n*LOGIC RESPONSE :*\n"
                //    + selectRateServiceResponse.Serialize()
                //    + "\n*Platform :* "
                //    + Client.GetPlatformType(HttpContext.Current.User.Identity.GetClientId())
                //    + "\n*Itinerary :* \n"
                //    + HotelService.GetInstance().GetItineraryForDisplay(request.Token).Serialize());
                return apiResponse;
            }
            return new HotelSelectRoomApiResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                ErrorCode = "ERHSEL01"
            };
        }

        private static bool IsValid(HotelSelectRoomApiRequest request)
        {
            return
                request != null &&
                request.SearchId != null &&
                request.RegsIds != null;
        }

        private static SelectHotelRoomInput PreprocessServiceRequest(HotelSelectRoomApiRequest request)
        {
            var selectServiceRequest = new SelectHotelRoomInput
            {
                RegsIds = request.RegsIds,
                SearchId = request.SearchId
            };
            return selectServiceRequest;
        }

        private static HotelSelectRoomApiResponse AssembleApiResponse(SelectHotelRoomOutput selectHotelRoomServiceResponse)
        {
            if (selectHotelRoomServiceResponse == null)
            {
                return new HotelSelectRoomApiResponse();
            }

            if (selectHotelRoomServiceResponse.IsSuccess)
            {
                return new HotelSelectRoomApiResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Token = selectHotelRoomServiceResponse.Token,
                    TimeLimit = selectHotelRoomServiceResponse.Timelimit
                };
            }
            else
            {
                if (selectHotelRoomServiceResponse.Errors == null)
                    return new HotelSelectRoomApiResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorCode = "ERHSEA99"
                    };

                switch (selectHotelRoomServiceResponse.Errors[0])
                {
                    case HotelError.InvalidInputData:
                        return new HotelSelectRoomApiResponse
                        {
                            StatusCode = HttpStatusCode.InternalServerError,
                            ErrorCode = "ERRGEN99"
                        };
                    case HotelError.SearchIdNoLongerValid:
                        return new HotelSelectRoomApiResponse
                        {
                            StatusCode = HttpStatusCode.Accepted,
                            ErrorCode = "ERHSEA04"
                        };
                    case HotelError.RateKeyNotFound:
                        return new HotelSelectRoomApiResponse
                        {
                            StatusCode = HttpStatusCode.Accepted,
                            ErrorCode = "ERHSEA05"
                        };
                    default:
                        return new HotelSelectRoomApiResponse
                        {
                            StatusCode = HttpStatusCode.InternalServerError,
                            ErrorCode = "ERRGEN99"
                        };
                }
            }
            
        }
    }
}
