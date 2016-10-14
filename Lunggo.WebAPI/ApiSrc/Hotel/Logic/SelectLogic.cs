using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Identity.Auth;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Util;
using Lunggo.Framework.Config;
using Lunggo.Framework.Constant;
using Lunggo.Framework.Extension;
using Lunggo.Framework.Log;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Hotel.Model;


namespace Lunggo.WebAPI.ApiSrc.Hotel.Logic
{
    public static partial class HotelLogic
    {
        public static ApiResponseBase SelectHotelRates(HotelSelectRoomApiRequest request)
        {
            if (IsValid(request))
            {
                var selectRateServiceRequest = PreprocessServiceRequest(request);
                var selectRateServiceResponse = HotelService.GetInstance().SelectHotelRoom(selectRateServiceRequest);
                var apiResponse = AssembleApiResponse(selectRateServiceResponse);
                if (apiResponse.StatusCode == HttpStatusCode.OK) return apiResponse;
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
                ErrorCode = "ERHSER01"
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
            else
            {
                return new HotelSelectRoomApiResponse
                {
                    //TimeLimit = selectHotelRoomServiceResponse
                    Token = selectHotelRoomServiceResponse.Token
                    //TODO: TIMELIMIT
                };
            }
            
        }
    }
}
