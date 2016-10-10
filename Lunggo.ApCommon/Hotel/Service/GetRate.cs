using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Query;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model;
using Lunggo.Framework.Documents;
using Lunggo.Hotel.ViewModels;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {
        public GetHotelRateOutput GetRate (GetHotelRateInput hotelRateInput)
        {
            var document = DocumentService.GetInstance();
            var searchResultData =  document.Execute<HotelDetail>(new GetHotelDetailFromSearchResult(),new {hotelRateInput.SearchId, hotelRateInput.HotelCode}).SingleOrDefault();
            if (searchResultData != null)
            {
                foreach (var room in searchResultData.Rooms)
                {
                    foreach (var rate in room.Rates)
                    {
                        rate.RegsId = EncryptRegsId(searchResultData.HotelCode, room.RoomCode, rate.RateKey);
                    }
                }
            }
            else
            {
                Console.WriteLine("Failed to get Hotel by HoteCode");
            }

            return new GetHotelRateOutput
            {
                SearchId = hotelRateInput.SearchId,
                Rooms = searchResultData.Rooms != null ? searchResultData.Rooms : null,
            };

        }
        public string EncryptRegsId(int hotelCode, string roomCode, string rateKey)
        {
            return hotelCode + "|" + roomCode + "|" + rateKey;
        }
    }
}
