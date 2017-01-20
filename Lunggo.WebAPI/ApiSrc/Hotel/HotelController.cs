using System;
using System.Web.Http;
using Lunggo.Framework.Context;
using Lunggo.Framework.Cors;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Hotel.Logic;
using Lunggo.WebAPI.ApiSrc.Hotel.Model;


namespace Lunggo.WebAPI.ApiSrc.Hotel
{
    public class HotelController : ApiController
    {
        [HttpPost]
        [LunggoCorsPolicy]
        [Authorize]
        [Route("v1/hotel/book")]
        public ApiResponseBase BookHotel()
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);
            HotelBookApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<HotelBookApiRequest>();
                var apiResponse = HotelLogic.BookLogic(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Authorize]
        [Route("v1/hotel/select")]
        public ApiResponseBase SelectHotel()
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);
            HotelSelectRoomApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<HotelSelectRoomApiRequest>();
                var apiResponse = HotelLogic.SelectHotelRatesLogic(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Route("v1/hotel/getroomdetail")]
        public ApiResponseBase GetRoomDetail()
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);
            HotelRoomDetailApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<HotelRoomDetailApiRequest>();
                var apiResponse = HotelLogic.GetHotelRoomDetailLogic(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Route("v1/hotel/getrate")]
        public ApiResponseBase GetRate()
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);
            HotelRateApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<HotelRateApiRequest>();
                var apiResponse = HotelLogic.GetRateLogic(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Authorize]
        [Route("v1/hotel/search")]
        public ApiResponseBase SearchHotel()
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);
            HotelSearchApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<HotelSearchApiRequest>();
                var apiResponse = HotelLogic.Search(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }


        [HttpGet]
        [LunggoCorsPolicy]
        [Authorize]
        [Route("v1/hotel/GetHotelDetail/{searchId}/{hotelCd}")]
        public ApiResponseBase GetHotelDetail(string searchId, int hotelCd)
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);
            try
            {
                var request = new HotelDetailApiRequest
                {
                    HotelCode = hotelCd,
                    SearchId = searchId
                };
                var apiResponse = HotelLogic.GetHotelDetailLogic(request);
                return apiResponse;
                //var apiResponse = HotelService.GetInstance().GetHotelDetailFromDb(444942);
                //return new ApiResponseBase();
                
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Route("v1/hotel/GetSelectedHotelDetail/{token}")]
        public ApiResponseBase GetSelectedHotelDetail(string token)
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);
            try
            {
                var request = new HotelSelectedRoomApiRequest
                {
                    Token = token
                };
                var apiResponse = HotelLogic.GetSelectedHotelDetailLogic(request);
                return apiResponse;
                //var apiResponse = HotelService.GetInstance().GetHotelDetailFromDb(444942);
                //return new ApiResponseBase();

            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpGet]
        [LunggoCorsPolicy]
        [Route("v1/hotel/CancelHotelBooking/{bookingId}")]
        public ApiResponseBase CancelHotelBooking(string bookingId)
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);
            try
            {
                var request = new HotelCancelBookingApiRequest
                {
                    BookingId = bookingId
                };
                var apiResponse = HotelLogic.CancelBookingLogic(request);
                return apiResponse;
                //var apiResponse = HotelService.GetInstance().GetHotelDetailFromDb(444942);
                //return new ApiResponseBase();

            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Authorize]
        [Route("v1/hotel/availableRate")]
        public ApiResponseBase GetAvailableRate()
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);
            HotelAvailableRateApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<HotelAvailableRateApiRequest>();
                var apiResponse = HotelLogic.GetAvailableRates(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }
    }
    

}
