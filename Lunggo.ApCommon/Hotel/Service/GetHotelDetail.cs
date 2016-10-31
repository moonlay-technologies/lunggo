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
        private decimal NetFare = 0;
        private decimal OriginalPrice = 0;
        public GetHotelDetailOutput GetHotelDetail(GetHotelDetailInput input)
        {
            var hotelDetail = GetHotelDetailFromDb(input.HotelCode);
            SetHotelFullFacilityCode(hotelDetail);
            SetDetailFromSearchResult(hotelDetail, input.SearchId);
            return new GetHotelDetailOutput
            {
                HotelDetail = ConvertToHotelDetailsBaseForDisplay(hotelDetail,OriginalPrice,NetFare)
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

        public void SetDetailFromSearchResult(HotelDetailsBase hotel ,string searchId)
        {
            var searchResultData = GetSearchHotelResultFromCache(searchId);
            var SearchResulthotel = searchResultData.HotelDetails.SingleOrDefault(p => p.HotelCode == hotel.HotelCode);
            hotel.Rooms = SearchResulthotel.Rooms;
            OriginalPrice = SearchResulthotel.OriginalFare;
            NetFare = SearchResulthotel.NetFare;
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
