using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Hotel.Model.Logic
{
    public class GetRoomDetailInput
    {
        public string RoomCode { get; set; }
        public string HotelCode { get; set; }
        public string SearchId { get; set; }
    }
}
