using System.Collections.Generic;
using System.Net;
using Lunggo.ApCommon.Hotel.Model;
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
        public static ApiResponseBase GetRateLogic(HotelRateApiRequest request)
        {
            if (IsValid(request))
            {
                var getRateServiceRequest = PreprocessServiceRequest(request);
                var getRateServiceResponse = HotelService.GetInstance().GetRate(getRateServiceRequest);
                var apiResponse = AssembleApiResponse(getRateServiceResponse);
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
            return new HotelRateApiResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                ErrorCode = "ERHGRA01"
            };
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

            var rooms = new List<HotelRoomForDisplay>();

            foreach (var room in getHotelRateServiceResponse.Rooms)
            {
                var rates = new List<HotelRateForDisplay>();
                foreach (var rate in room.Rates)
                {
                    var newRate = new HotelRateForDisplay
                    {
                        AdultCount = rate.AdultCount,
                        Boards = rate.Boards,
                        Cancellation = rate.Cancellation,
                        ChildCount = rate.ChildCount,
                        Class = rate.Class,
                        Price = rate.Price,
                        PaymentType = rate.PaymentType,
                        RateKey = rate.RateKey,
                        RoomCount = rate.RoomCount,
                        RegsId = rate.RegsId,
                        Offers = rate.Offers,
                        Type = rate.Type,
                    };
                    rates.Add(newRate);
                }

                var newroom = new HotelRoomForDisplay
                {
                    RoomCode = room.RoomCode,
                    RoomName = room.RoomName,
                    Type = room.Type,
                    TypeName = room.TypeName,
                    Facilities = room.Facilities,
                    Images = room.Images,
                    CharacteristicCode = room.characteristicCd,
                    Rates = rates
                };
                rooms.Add(newroom);
            }
            
            return new HotelRateApiResponse
            {
                SearchId = getHotelRateServiceResponse.SearchId,
                Rooms = rooms
            };
        }
    }
}
