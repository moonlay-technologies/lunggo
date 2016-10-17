using System;
using System.Net.Http;
using System.Web.Http;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.Framework.Cors;
using Lunggo.Framework.Documents;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Flight.Logic;
using Lunggo.WebAPI.ApiSrc.Flight.Model;
using Lunggo.WebAPI.ApiSrc.Hotel.Logic;
using Lunggo.WebAPI.ApiSrc.Hotel.Model;


namespace Lunggo.WebAPI.ApiSrc.Hotel
{


    public class HotelController : ApiController
    {
        [HttpPost]
        [LunggoCorsPolicy]
        [Route("v1/hotel/book")]
        public ApiResponseBase BookHotel()
        {                 
            HotelBookApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<HotelBookApiRequest>();
                var apiResponse = HotelLogic.Book(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }

        [HttpPost]
        [LunggoCorsPolicy]
        [Route("v1/hotel/search")]
        public ApiResponseBase SearchHotel()
        {
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
        [Route("v1/hotel/GetHotelDetail/{hotelCd}")]
        public ApiResponseBase GetHotelDetail(int hotelCd)
        {
            try
            {
                var request = new HotelDetailApiRequest
                {
                    HotelCode = hotelCd
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


    }
    

}
