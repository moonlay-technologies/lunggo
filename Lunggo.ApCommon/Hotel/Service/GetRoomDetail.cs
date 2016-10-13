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
        public GetRoomDetailOutput GetRoomDetail(GetRoomDetailInput roomDetailInput)
        {
            //First, take the single room from docDB
            var document = DocumentService.GetInstance();
            var searchResultData = document.Execute<HotelRoom>(new GetRoomDetailFromSearchResult(), new { roomDetailInput.SearchId, roomDetailInput.HotelCode,roomDetailInput.RoomCode }).SingleOrDefault();
            if (searchResultData != null)
            {
                return new GetRoomDetailOutput
                {
                    Room = ConvertToSingleHotelRoomForDisplay(searchResultData)
                };
            }
            
            else
                return new GetRoomDetailOutput();
        }
    }

}
