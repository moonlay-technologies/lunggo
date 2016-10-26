using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.Framework.Documents;
using Microsoft.Azure.Documents;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {

        public GetHotelDetailOutput GetHotelDetail(GetHotelDetailInput input)
        {
            var hotelDetail = GetHotelDetailFromDb(input.HotelCode);
            SetHotelFullFacilityCode(hotelDetail);
            var hotelRoom = GetRoomFromSearchResult(input.HotelCode, input.SearchId);
            hotelDetail.Rooms = hotelRoom;
            return new GetHotelDetailOutput
            {
                HotelDetail = ConvertToHotelDetailsBaseForDisplay(hotelDetail)
            };
        }

        public HotelDetailsBase GetHotelDetailFromDb (int hotelCode)
        {
            /*Technologies using DocDB*/
            //var document = DocumentService.GetInstance();
            //return document.Retrieve<HotelDetailsBase>("HotelDetail:"+hotelCode);

            /*Technpologies using TableStorage*/
            return GetHotelDetailFromTableStorage(hotelCode);
        }

        public List<HotelRoom> GetRoomFromSearchResult(int hotelCode,string searchId)
        {
            var searchResultData = GetSearchHotelResultFromCache(searchId);
            var SearchResulthotel = searchResultData.HotelDetails.SingleOrDefault(p => p.HotelCode == hotelCode);
            return SearchResulthotel.Rooms;
        }

        public void SetHotelFullFacilityCode(HotelDetailsBase hotel)
        {
            foreach (var data in hotel.Facilities)
            {
                data.FullFacilityCode = data.FacilityGroupCode + "" + data.FacilityCode;
            }
        }

    }
}
