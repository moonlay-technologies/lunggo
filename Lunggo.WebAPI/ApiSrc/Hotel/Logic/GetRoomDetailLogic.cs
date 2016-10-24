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
        public static ApiResponseBase GetHotelRoomDetailLogic(HotelRoomDetailApiRequest request)
        {
            if (IsValid(request))
            {
                var getRoomDetailServiceRequest = PreprocessServiceRequest(request);
                var getRoomDetailServiceResponse = HotelService.GetInstance().GetRoomDetail(getRoomDetailServiceRequest);
                var apiResponse = AssembleApiResponse(getRoomDetailServiceResponse);
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
            return new HotelRoomDetailApiResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                ErrorCode = "ERHGRD01"
            };
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

            var rates = getRoomDetailServiceResponse.Room.Rates.Select(rate => new HotelRate
            {
                RateKey = rate.RateKey, AdultCount = rate.AdultCount, Boards = rate.Boards, Cancellation = rate.Cancellation, ChildCount = rate.ChildCount, Class = rate.Class, RoomCount = rate.RoomCount, RegsId = rate.RegsId, PaymentType = rate.PaymentType, Price = rate.Price, Offers = rate.Offers,
            }).ToList();

            var room = new HotelRoom
            {
                characteristicCd = getRoomDetailServiceResponse.Room.CharacteristicCode,
                RoomCode = getRoomDetailServiceResponse.Room.RoomCode,
                RoomName = getRoomDetailServiceResponse.Room.RoomName,
                Type = getRoomDetailServiceResponse.Room.Type,
                TypeName = getRoomDetailServiceResponse.Room.TypeName,
                Facilities = getRoomDetailServiceResponse.Room.Facilities,
                Images = getRoomDetailServiceResponse.Room.Images,
                Rates = rates
            };
            return new HotelRoomDetailApiResponse
            {
                
                RoomDetails = room,
            };
        }
    }
}
