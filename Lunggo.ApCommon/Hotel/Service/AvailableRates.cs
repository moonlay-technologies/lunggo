using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Constant;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds;
using Lunggo.ApCommon.Payment.Model;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {
        public AvailableRatesOutput GetAvailableRates(AvailableRatesInput input)
        {
            var occupancies = PreProcessOccupancies(input.Occupancies);
            Guid generatedSearchId = Guid.NewGuid();
            var hotelBedsClient = new HotelBedsSearchHotel();
            var allCurrency = Currency.GetAllCurrencies();
            SaveAllCurrencyToCache(generatedSearchId.ToString(), allCurrency);
            var request = new SearchHotelCondition();
            if (input.HotelCode != 0)
            {
                request.Occupancies = occupancies;
                request.HotelCode = input.HotelCode;
                request.CheckIn = input.CheckIn;
                request.Nights = input.Nights;
                request.Checkout = input.CheckIn.AddDays(input.Nights);
                request.SearchId = generatedSearchId.ToString();
                var realOccupancies = input.Occupancies;
                var result = hotelBedsClient.SearchHotel(request);
                AddPriceMargin(result.HotelDetails);
                result.Occupancies = realOccupancies;
                if (result.HotelDetails == null)
                {
                    return new AvailableRatesOutput
                    {
                        IsSuccess = true,
                        Total = 0
                    };
                }
                var rooms = ProcessRoomFromSearchResult(result);
                SaveAvailableRateToCache(generatedSearchId.ToString(), rooms);
                return new AvailableRatesOutput
                {
                    Id = generatedSearchId.ToString(),
                    IsSuccess = true,
                    Total = rooms.Count,
                    Rooms = ConvertToHotelRoomsForDisplay(rooms)
                };
            }
            return new AvailableRatesOutput
            {
                IsSuccess = false,
                Errors = new List<HotelError> { HotelError.TechnicalError},
                ErrorMessages = new List<string> {"Hotel Code is not vaild"}
            };
        }

        public List<HotelRoom> ProcessRoomFromSearchResult(SearchHotelResult result)
        {
            var singleHotel = result.HotelDetails.FirstOrDefault();
            var hotelDetail = GetHotelDetailFromTableStorage(singleHotel.HotelCode);
            if (hotelDetail == null)
                return null;
            if (hotelDetail.ImageUrl != null)
            {
                foreach (var room in singleHotel.Rooms)
                {
                    room.Images = hotelDetail.ImageUrl.Where(x => x.Type == "HAB").Select(x => x.Path).ToList();
                }
            }
            SetRegIdsAndTnc(singleHotel.Rooms, result.CheckIn, singleHotel.HotelCode);
            //var rooms = new List<HotelRoom>();
           // rooms = SetRoomPerRate(singleHotel.Rooms);
            return singleHotel.Rooms;
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

        
    }
}
