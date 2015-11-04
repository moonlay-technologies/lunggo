using System.Net.Http;
using System.Web.Http;
using Lunggo.Framework.Cors;
using Lunggo.WebAPI.ApiSrc.v1.Rooms.Logic;
using Lunggo.WebAPI.ApiSrc.v1.Rooms.Object;

namespace Lunggo.WebAPI.ApiSrc.v1.Rooms
{
    public class RoomsController : ApiController
    {
        [LunggoCorsPolicy]
        [Route("api/v1/rooms")]
        public RoomSearchApiResponse GetRooms(HttpRequestMessage httpRequest, [FromUri] RoomSearchApiRequest request)
        {
            var checkedRequest = CreateRoomSearchApiRequestIfNull(request);
            var apiResponse = GetRoomsLogic.GetRooms(checkedRequest);
            return apiResponse;
        }

        private RoomSearchApiRequest CreateRoomSearchApiRequestIfNull(RoomSearchApiRequest request)
        {
            return request ?? new RoomSearchApiRequest();
        }
    }
}
