using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Query;
using Lunggo.Framework.Documents;
using Microsoft.Azure.Documents;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {
        public HotelRoom GetRoom(GetRoomDetailInput roomDetailInput)
        {
            var searchResultData = GetAvailableRatesFromCache(roomDetailInput.SearchId);
            var room = searchResultData.SingleOrDefault(p => p.RoomCode == roomDetailInput.RoomCode);
            return room ?? new HotelRoom();
        }
    }
}
