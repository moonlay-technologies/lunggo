using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using Lunggo.ApCommon.Hotel.Constant;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model;
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
            decimal originalPrice, netFare;
            SetDetailFromSearchResult(hotelDetail, input.SearchId, out originalPrice, out netFare);
            if (hotelDetail == null)
                return new GetHotelDetailOutput
                {
                    IsSuccess = false,
                    Errors = new List<HotelError> { HotelError.SearchIdNoLongerValid },
                    ErrorMessages = new List<string> {"SearchID no longer valid"}
                };
            hotelDetail.Rooms = SetRoomPerRate(hotelDetail.Rooms);
            hotelDetail.Rooms = FilterRoomByCapacity(hotelDetail.Rooms);
            foreach (var room in hotelDetail.Rooms)
            {
                    var cid = room.SingleRate.RateKey != null ? room.SingleRate.RateKey.Split('|')[0] : room.SingleRate.RegsId.Split('|')[0];
                    var checkInDate = new DateTime(Convert.ToInt32(cid.Substring(0, 4)),
                         Convert.ToInt32(cid.Substring(4, 2)), Convert.ToInt32(cid.Substring(6, 2)));
                    room.SingleRate.TermAndCondition = GetRateCommentFromTableStorage(room.SingleRate.RateCommentsId,
                        checkInDate).Select(x => x.Description).ToList();
            }

            return new GetHotelDetailOutput
            {
                IsSuccess = true,
                HotelDetail = ConvertToHotelDetailsBaseForDisplay(hotelDetail,originalPrice,netFare)
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

        public void SetDetailFromSearchResult(HotelDetailsBase hotel ,string searchId, out decimal originalPrice, out decimal netFare)
        {
            var searchResultData = GetSearchHotelResultFromCache(searchId);
            var searchResulthotel = searchResultData.HotelDetails.SingleOrDefault(p => p.HotelCode == hotel.HotelCode);
            if (searchResulthotel == null)
                hotel = null;
            hotel.Rooms = searchResulthotel.Rooms;
            foreach (var room in hotel.Rooms)
            {
                room.Images = hotel.ImageUrl.Where(x=>x.Type == "HAB").Select(x => x.Path).ToList();
            }
            SetRegIdsAndTnc(hotel.Rooms, searchResultData.CheckIn, hotel.HotelCode);
            originalPrice = searchResulthotel.OriginalFare;
            netFare = searchResulthotel.NetFare;
        }

        public void SetHotelFullFacilityCode(HotelDetailsBase hotel)
        {
            foreach (var data in hotel.Facilities)
            {
                data.FullFacilityCode = data.FacilityGroupCode + "" + data.FacilityCode;
            }
        }
        public List<HotelRoom> SetRoomPerRate(List<HotelRoom> hotelRoom)
        {
            var roomList = new List<HotelRoom>();
            foreach (var room in hotelRoom)
            {

                foreach (var rate in room.Rates)
                {
                    var singleRoom = new HotelRoom
                    {
                        RoomCode = room.RoomCode,
                        RoomName = room.RoomName,
                        Type = room.Type,
                        TypeName = room.TypeName,
                        Images = room.Images,
                        Facilities = room.Facilities,
                        characteristicCd = room.characteristicCd,
                        SingleRate = rate
                    };
                    roomList.Add(singleRoom);
                }
            }
            return roomList;
        }

        public List<HotelRoom> FilterRoomByCapacity(List<HotelRoom> rooms)
        {
            var maxPax = 0;
            var maxAdult = 0;
            var maxChild = 0;
            var roomList = new List<HotelRoom>();
            //Take Max Pax first
            foreach (var room in rooms)
            {
                var totalPax = room.SingleRate.AdultCount + room.SingleRate.ChildCount;
                if (maxPax == 0)
                {
                    maxPax = totalPax;
                }
                if (totalPax > maxPax)
                {
                    maxPax = totalPax;
                }
                if (room.SingleRate.AdultCount > maxAdult)
                    maxAdult = room.SingleRate.AdultCount;
                if (room.SingleRate.ChildCount > maxChild)
                    maxChild = room.SingleRate.ChildCount;
            }

            foreach (var room in rooms)
            {
                var roomPax = GetPaxCapacity(room.RoomCode);
                var roomAdult = GetMaxAdult(room.RoomCode);
                var roomChild = GetMaxChild(room.RoomCode);
                if (roomPax >= maxPax && roomAdult >= maxAdult && roomChild >= maxChild)
                {
                    roomList.Add(room);
                }
            }
            return roomList;
        } 

    }
}
