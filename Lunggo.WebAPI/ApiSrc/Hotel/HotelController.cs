using System.Net.Http;
using System.Web.Http;
using Lunggo.Framework.Cors;
using Lunggo.WebAPI.ApiSrc.Hotel.Logic;
using Lunggo.WebAPI.ApiSrc.Hotel.Object;

namespace Lunggo.WebAPI.ApiSrc.Hotel
{

    public class HotelController : ApiController
    {
        [LunggoCorsPolicy]
        [Route("api/v1/hotel")]
        //public HotelSearchApiResponse GetHotels(HttpRequestMessage httpRequest,[FromUri] HotelSearchApiRequest request)
        //{
        //    var checkedRequest = CreateHotelSearchApiRequestIfNull(request);
        //    var response = GetHotelLogic.GetHotels(checkedRequest);
        //    return response;
        //}

        private HotelSearchApiRequest CreateHotelSearchApiRequestIfNull(HotelSearchApiRequest request)
        {
            return request ?? new HotelSearchApiRequest();
        }
    }
    

}
