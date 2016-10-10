using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Model.Logic;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {
        public GetRoomDetailOutput GetRoomDetail(GetRoomDetailInput roomDetailInput)
        {
            var roomType =  GetHotelRoomTypeId(roomDetailInput.RoomCode);
            //Get Room Detail by RoomCode from room.csv and roomDetail in DocDB
            //Mapping data nya ke dalam Output
            return new GetRoomDetailOutput();
        }
    }

}
