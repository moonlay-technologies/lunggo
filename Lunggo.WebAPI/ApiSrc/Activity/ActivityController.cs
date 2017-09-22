using System;
using System.Web.Http;
using Lunggo.Framework.Context;
using Lunggo.Framework.Cors;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Activity.Logic;
using Lunggo.WebAPI.ApiSrc.Activity.Model;

namespace Lunggo.WebAPI.ApiSrc.Activity
{
    public class ActivityController : ApiController
    {
        [HttpPost]
        [LunggoCorsPolicy]
        [Authorize]
        [Route("v1/activity/search")]
        public ApiResponseBase SearchActivity()
        {
            var lang = ApiRequestBase.GetHeaderValue("Language");
            OnlineContext.SetActiveLanguageCode(lang);
            var currency = ApiRequestBase.GetHeaderValue("Currency");
            OnlineContext.SetActiveCurrencyCode(currency);
            ActivitySearchApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<ActivitySearchApiRequest>();
                var apiResponse = ActivityLogic.Search(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }
        //[HttpPost]
        //[LunggoCorsPolicy]
        //[Authorize]
        //[Route("v1/hotel/book")]
        //public ApiResponseBase BookHotel()
        //{
        //    var lang = ApiRequestBase.GetHeaderValue("Language");
        //    OnlineContext.SetActiveLanguageCode(lang);
        //    var currency = ApiRequestBase.GetHeaderValue("Currency");
        //    OnlineContext.SetActiveCurrencyCode(currency);
        //    HotelBookApiRequest request = null;
        //    try
        //    {
        //        request = ApiRequestBase.DeserializeRequest<HotelBookApiRequest>();
        //        var apiResponse = HotelLogic.BookLogic(request);
        //        return apiResponse;
        //    }
        //    catch (Exception e)
        //    {
        //        return ApiResponseBase.ExceptionHandling(e, request);
        //    }
        //}

        //[HttpPost]
        //[LunggoCorsPolicy]
        //[Authorize]
        //[Route("v1/hotel/select")]
        //public ApiResponseBase SelectHotel()
        //{
        //    var lang = ApiRequestBase.GetHeaderValue("Language");
        //    OnlineContext.SetActiveLanguageCode(lang);
        //    var currency = ApiRequestBase.GetHeaderValue("Currency");
        //    OnlineContext.SetActiveCurrencyCode(currency);
        //    HotelSelectRoomApiRequest request = null;
        //    try
        //    {
        //        request = ApiRequestBase.DeserializeRequest<HotelSelectRoomApiRequest>();
        //        var apiResponse = HotelLogic.SelectHotelRatesLogic(request);
        //        return apiResponse;
        //    }
        //    catch (Exception e)
        //    {
        //        return ApiResponseBase.ExceptionHandling(e, request);
        //    }
        //}

        //[HttpPost]
        //[LunggoCorsPolicy]
        //[Route("v1/hotel/getroomdetail")]
        //public ApiResponseBase GetRoomDetail()
        //{
        //    var lang = ApiRequestBase.GetHeaderValue("Language");
        //    OnlineContext.SetActiveLanguageCode(lang);
        //    var currency = ApiRequestBase.GetHeaderValue("Currency");
        //    OnlineContext.SetActiveCurrencyCode(currency);
        //    HotelRoomDetailApiRequest request = null;
        //    try
        //    {
        //        request = ApiRequestBase.DeserializeRequest<HotelRoomDetailApiRequest>();
        //        var apiResponse = HotelLogic.GetHotelRoomDetailLogic(request);
        //        return apiResponse;
        //    }
        //    catch (Exception e)
        //    {
        //        return ApiResponseBase.ExceptionHandling(e, request);
        //    }
        //}

        //[HttpPost]
        //[LunggoCorsPolicy]
        //[Route("v1/hotel/getrate")]
        //public ApiResponseBase GetRate()
        //{
        //    var lang = ApiRequestBase.GetHeaderValue("Language");
        //    OnlineContext.SetActiveLanguageCode(lang);
        //    var currency = ApiRequestBase.GetHeaderValue("Currency");
        //    OnlineContext.SetActiveCurrencyCode(currency);
        //    HotelRateApiRequest request = null;
        //    try
        //    {
        //        request = ApiRequestBase.DeserializeRequest<HotelRateApiRequest>();
        //        var apiResponse = HotelLogic.GetRateLogic(request);
        //        return apiResponse;
        //    }
        //    catch (Exception e)
        //    {
        //        return ApiResponseBase.ExceptionHandling(e, request);
        //    }
        //}


        //[HttpGet]
        //[LunggoCorsPolicy]
        //[Authorize]
        //[Route("v1/hotel/GetHotelDetail/{searchId}/{hotelCd}")]
        //public ApiResponseBase GetHotelDetail(string searchId, int hotelCd)
        //{
        //    var lang = ApiRequestBase.GetHeaderValue("Language");
        //    OnlineContext.SetActiveLanguageCode(lang);
        //    var currency = ApiRequestBase.GetHeaderValue("Currency");
        //    OnlineContext.SetActiveCurrencyCode(currency);
        //    try
        //    {
        //        var request = new HotelDetailApiRequest
        //        {
        //            HotelCode = hotelCd,
        //            SearchId = searchId
        //        };
        //        var apiResponse = HotelLogic.GetHotelDetailLogic(request);
        //        return apiResponse;
        //        //var apiResponse = HotelService.GetInstance().GetHotelDetailFromDb(444942);
        //        //return new ApiResponseBase();

        //    }
        //    catch (Exception e)
        //    {
        //        return ApiResponseBase.ExceptionHandling(e);
        //    }
        //}

        //[HttpGet]
        //[LunggoCorsPolicy]
        //[Route("v1/hotel/GetSelectedHotelDetail/{token}")]
        //public ApiResponseBase GetSelectedHotelDetail(string token)
        //{
        //    var lang = ApiRequestBase.GetHeaderValue("Language");
        //    OnlineContext.SetActiveLanguageCode(lang);
        //    var currency = ApiRequestBase.GetHeaderValue("Currency");
        //    OnlineContext.SetActiveCurrencyCode(currency);
        //    try
        //    {
        //        var request = new HotelSelectedRoomApiRequest
        //        {
        //            Token = token
        //        };
        //        var apiResponse = HotelLogic.GetSelectedHotelDetailLogic(request);
        //        return apiResponse;
        //        //var apiResponse = HotelService.GetInstance().GetHotelDetailFromDb(444942);
        //        //return new ApiResponseBase();

        //    }
        //    catch (Exception e)
        //    {
        //        return ApiResponseBase.ExceptionHandling(e);
        //    }
        //}

        //[HttpGet]
        //[LunggoCorsPolicy]
        //[Route("v1/hotel/CancelHotelBooking/{bookingId}")]
        //public ApiResponseBase CancelHotelBooking(string bookingId)
        //{
        //    var lang = ApiRequestBase.GetHeaderValue("Language");
        //    OnlineContext.SetActiveLanguageCode(lang);
        //    var currency = ApiRequestBase.GetHeaderValue("Currency");
        //    OnlineContext.SetActiveCurrencyCode(currency);
        //    try
        //    {
        //        var request = new HotelCancelBookingApiRequest
        //        {
        //            BookingId = bookingId
        //        };
        //        var apiResponse = HotelLogic.CancelBookingLogic(request);
        //        return apiResponse;
        //        //var apiResponse = HotelService.GetInstance().GetHotelDetailFromDb(444942);
        //        //return new ApiResponseBase();

        //    }
        //    catch (Exception e)
        //    {
        //        return ApiResponseBase.ExceptionHandling(e);
        //    }
        //}

        //[HttpPost]
        //[LunggoCorsPolicy]
        //[Authorize]
        //[Route("v1/hotel/availableRate")]
        //public ApiResponseBase GetAvailableRate()
        //{
        //    var lang = ApiRequestBase.GetHeaderValue("Language");
        //    OnlineContext.SetActiveLanguageCode(lang);
        //    var currency = ApiRequestBase.GetHeaderValue("Currency");
        //    OnlineContext.SetActiveCurrencyCode(currency);
        //    HotelAvailableRateApiRequest request = null;
        //    try
        //    {
        //        request = ApiRequestBase.DeserializeRequest<HotelAvailableRateApiRequest>();
        //        var apiResponse = HotelLogic.GetAvailableRates(request);
        //        return apiResponse;
        //    }
        //    catch (Exception e)
        //    {
        //        return ApiResponseBase.ExceptionHandling(e, request);
        //    }
        //}

        //[HttpGet]
        //[LunggoCorsPolicy]
        //[Route("v1/hotel/pricecalendar/{location}/{startDate}/{endDate}/{currency}")]
        //public ApiResponseBase PriceCalendar(string location, string currency, string startDate, string endDate)
        //{
        //    try
        //    {
        //        var startYear = "20" + startDate.Substring(4, 2);
        //        var endYear = "20" + endDate.Substring(4, 2);
        //        var startTime = new DateTime(Convert.ToInt32(startYear), Convert.ToInt32(startDate.Substring(2, 2)),
        //            Convert.ToInt32(startDate.Substring(0, 2)));
        //        var endTime = new DateTime(Convert.ToInt32(endYear), Convert.ToInt32(endDate.Substring(2, 2)),
        //            Convert.ToInt32(endDate.Substring(0, 2)));
        //        var request = new HotelPriceCalendarApiRequest
        //        {
        //            LocationCode = location,
        //            Currency = currency,
        //            StartDate = startTime,
        //            EndDate = endTime,
        //        };
        //        var apiResponse = HotelLogic.FindLowestPrices(request);
        //        return apiResponse;
        //    }
        //    catch (Exception e)
        //    {
        //        return ApiResponseBase.ExceptionHandling(e);
        //    }
        //}
    }
    

}
