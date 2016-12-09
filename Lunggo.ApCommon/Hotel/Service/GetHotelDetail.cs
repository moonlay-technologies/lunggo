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
            if (hotelDetail == null)
                return new GetHotelDetailOutput
                {
                    IsSuccess = false,
                    Errors = new List<HotelError> { HotelError.SearchIdNoLongerValid },
                    ErrorMessages = new List<string> { "SearchID no longer valid" }
                };

            bool searchIdExpired = false;
            if (SetDetailFromSearchResult(ref hotelDetail, input.SearchId, out originalPrice, out netFare) == searchIdExpired) {
                return new GetHotelDetailOutput
                {
                    IsSuccess = false,
                    Errors = new List<HotelError> { HotelError.SearchIdNoLongerValid },
                    ErrorMessages = new List<string> { "SearchID no longer valid" }
                };
            };
           
            hotelDetail.Rooms = SetRoomPerRate(hotelDetail.Rooms);
            //hotelDetail.Rooms = FilterRoomByCapacity(hotelDetail.Rooms);
            if (hotelDetail.Rooms.Count != 0)
            {
                foreach (var room in hotelDetail.Rooms)
                {
                    var cid = room.SingleRate.RateKey != null ? room.SingleRate.RateKey.Split('|')[0] : room.SingleRate.RegsId.Split('|')[0];
                    var checkInDate = new DateTime(Convert.ToInt32(cid.Substring(0, 4)),
                         Convert.ToInt32(cid.Substring(4, 2)), Convert.ToInt32(cid.Substring(6, 2)));
                    room.SingleRate.TermAndCondition = GetRateCommentFromTableStorage(room.SingleRate.RateCommentsId,
                        checkInDate).Select(x => x.Description).ToList();
                }
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

        public bool SetDetailFromSearchResult(ref HotelDetailsBase hotel ,string searchId, out decimal originalPrice, out decimal netFare)
        {
            originalPrice = 0;
            netFare = 0;
            var hotelTemp = hotel;

            var searchResultData = GetSearchHotelResultFromCache(searchId);
            
            if (searchResultData == null) return false;

            var searchResulthotel = searchResultData.HotelDetails.SingleOrDefault(p => p.HotelCode == hotelTemp.HotelCode);
            if (searchResulthotel == null)
                hotel = null;
            hotel.Rooms = searchResulthotel.Rooms;
            hotel.DestinationName = searchResultData.DestinationName;

            if (hotel.ImageUrl != null)
            {
                foreach (var room in hotel.Rooms)
                {
                    room.Images = hotel.ImageUrl.Where(x => x.Type == "HAB").Select(x => x.Path).ToList();
                }
            }
           
            SetRegIdsAndTnc(hotel.Rooms, searchResultData.CheckIn, hotel.HotelCode);
            originalPrice = searchResulthotel.OriginalFare;
            netFare = searchResulthotel.NetFare;

            return true;
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
                    maxAdult = room.SingleRate.AdultCount;
                    maxChild = room.SingleRate.ChildCount;
                }
                if (totalPax > maxPax)
                {
                    maxPax = totalPax;
                    maxAdult = room.SingleRate.AdultCount;
                    maxChild = room.SingleRate.ChildCount;
                }
            }

            foreach (var room in rooms)
            {
                if (room.SingleRate.AdultCount == maxAdult && room.SingleRate.ChildCount == maxChild)
                {
                    roomList.Add(room);
                }
            }
            return roomList;
        } 

    }
}
