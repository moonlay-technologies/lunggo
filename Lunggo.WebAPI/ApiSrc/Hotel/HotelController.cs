using System;
using System.Net.Http;
using System.Web.Http;
using Lunggo.Framework.Cors;
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
        [Route("v1/hotel/select")]
        public ApiResponseBase SelectHotel()
        {
            HotelSelectRoomApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<HotelSelectRoomApiRequest>();
                var apiResponse = HotelLogic.SelectHotelRates(request);
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
    }
    

}
