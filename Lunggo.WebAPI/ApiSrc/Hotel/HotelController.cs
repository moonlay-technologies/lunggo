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


        [HttpGet]
        [LunggoCorsPolicy]
        [Route("v1/hotel/GetHotelDetail/{hotelCd}")]
        public ApiResponseBase GetHotelDetail(int hotelCd)
        {
            //HotelBookApiRequest request = null;
            try
            {
                var task = DocumentService.AzureDocumentDbClient.ClientInstance._client.CreateDocumentAsync(
                    DocumentService.AzureDocumentDbClient.ClientInstance._collection.SelfLink, new {a = "aaa"});
                while (!task.IsCompleted)
                {
                    
                }
                //request = ApiRequestBase.DeserializeRequest<HotelDetailApiRequest>();
                //DocumentService.GetInstance().Upsert("aaa", new {a="aaaaa"}, new TimeSpan(1));
                var apiResponse = HotelService.GetInstance().GetHotelDetail(444942);
                return new ApiResponseBase();
                //return apiResponse;
            }
            catch (Exception e)
            {
                return new ApiResponseBase();
                //return ApiResponseBase.ExceptionHandling(e, request);
            }
        }


    }
    

}
