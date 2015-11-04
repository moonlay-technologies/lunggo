using System.Net.Http;
using System.Web.Http;
using Lunggo.Framework.Cors;
using Lunggo.WebAPI.ApiSrc.v1.Hotels.Logic;
using Lunggo.WebAPI.ApiSrc.v1.Hotels.Object;

namespace Lunggo.WebAPI.ApiSrc.v1.Hotels
{

    public class HotelsController : ApiController
    {
        [LunggoCorsPolicy]
        [Route("api/v1/hotels")]
        public HotelSearchApiResponse GetHotels(HttpRequestMessage httpRequest,[FromUri] HotelSearchApiRequest request)
        {
            var checkedRequest = CreateHotelSearchApiRequestIfNull(request);
            var response = GetHotelLogic.GetHotels(checkedRequest);
            return response;
        }

        private HotelSearchApiRequest CreateHotelSearchApiRequestIfNull(HotelSearchApiRequest request)
        {
            return request ?? new HotelSearchApiRequest();
        }
    }
    

}
