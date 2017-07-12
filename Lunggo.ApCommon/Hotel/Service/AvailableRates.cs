using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Constant;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds;
using Lunggo.ApCommon.Hotel.Wrapper.Tiket;
using Lunggo.ApCommon.Hotel.Wrapper.Tiket.Model;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Model;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {
        public AvailableRatesOutput GetAvailableRates(AvailableRatesInput input)
        {
            //var occupancies = PreProcessOccupancies(input.Occupancies);
            var hotelSearchResult = GetSearchHotelResultFromCache(input.SearchId);
            //SetHotelFullFacilityCode(hotelDetail);
            if (hotelSearchResult == null)
                return new AvailableRatesOutput
                {
                    IsSuccess = false,
                    Errors = new List<HotelError> { HotelError.TechnicalError },
                    ErrorMessages = new List<string> { "Search Id is not vaild" }
                };

            //Find Hotel Uri based on HotelCode
            var hotel = hotelSearchResult.HotelDetails.SingleOrDefault(x => x.HotelCode == input.HotelCode);
            if (hotel == null)
                return new AvailableRatesOutput
                {
                    IsSuccess = false,
                    Errors = new List<HotelError> { HotelError.TechnicalError },
                    ErrorMessages = new List<string> { "Hotel Code is not valid" }
                };

            Guid generatedSearchId = Guid.NewGuid();
            //var hotelBedsClient = new HotelBedsSearchHotel();
            var allCurrency = Currency.GetAllCurrencies();
            SaveAllCurrencyToCache(generatedSearchId.ToString(), allCurrency);
            //var request = new SearchHotelCondition();
            if (input.HotelCode != 0)
            {
                var detailClient = new TiketHotelDetail();
                var resultDetail = detailClient.GetHotelDetail(hotel.HotelUri);
                if (resultDetail == null)
                    return new AvailableRatesOutput
                    {
                        IsSuccess = false,
                        Errors = new List<HotelError> { HotelError.TechnicalError },
                        ErrorMessages = new List<string> { "Hotel Code is not vaild" }
                    };

                var rooms = ProcessRoomFromTiketHotelDetail(resultDetail, input);
                SaveAvailableRateToCache(generatedSearchId.ToString(), rooms);
                return new AvailableRatesOutput
                {
                    Id = generatedSearchId.ToString(),
                    IsSuccess = true,
                    Total = rooms.Count,
                    Rooms = ConvertToTiketHotelRoomsForDisplay(rooms),
                    ExpiryTime = GetAvailableRatesExpiry(generatedSearchId.ToString())
                };
                //request.Occupancies = occupancies;
                //request.HotelCode = input.HotelCode;
                //request.CheckIn = input.CheckIn;
                //request.Nights = input.Nights;
                //request.Checkout = input.CheckIn.AddDays(input.Nights);
                //request.SearchId = generatedSearchId.ToString();
                //var realOccupancies = input.Occupancies;
                //var result = hotelBedsClient.SearchHotel(request);
                //AddPriceMargin(result.HotelDetails);
                //result.Occupancies = realOccupancies;
                //if (result.HotelDetails == null)
                //{
                //    return new AvailableRatesOutput
                //    {
                //        IsSuccess = true,
                //        Total = 0
                //    };
                //}
                //var rooms = ProcessRoomFromSearchResult(result);
                //SaveAvailableRateToCache(generatedSearchId.ToString(), rooms);
                //return new AvailableRatesOutput
                //{
                //    Id = generatedSearchId.ToString(),
                //    IsSuccess = true,
                //    Total = rooms.Count,
                //    Rooms = ConvertToHotelRoomsForDisplay(rooms),
                //    ExpiryTime = GetAvailableRatesExpiry(generatedSearchId.ToString())
                //};
            }
            return new AvailableRatesOutput
            {
                IsSuccess = false,
                Errors = new List<HotelError> { HotelError.TechnicalError},
                ErrorMessages = new List<string> {"Hotel Code is not vaild"}
            };
        }


        public List<HotelRoom> ProcessRoomFromTiketHotelDetail(TiketHotelDetailResponse result, AvailableRatesInput input)
        {
            var roomList = new List<HotelRoom>();
            if (result.Results == null || result.Results.RoomDetail == null)
                return null;

            foreach (var room in result.Results.RoomDetail)
            {
                var singleRoom = new HotelRoom
                {
                    RoomCode = room.room_id,
                    Images = room.PhotoRooms,
                    RoomName = room.room_name,
                    RoomAvailable = room.RoomAvailable,

                    RoomDescription = room.RoomDescription,
                    SingleRate = new HotelRate
                    {
                        RateKey = room.Id,
                        NightCount = input.Nights,
                        RateCount = 1,
                        Price = new Price
                        {
                            Supplier = room.Price,
                            Local = room.Price
                        },
                        Cancellation = ProcessCancellation(room.RefundPolicy)
                    }
                };
                //singleRoom.Facilities = new List<HotelRoomFacilities>();
                //foreach (var item in room.RoomFacility)
                //{
                //    var singleFacility = new HotelRoomFacilities
                //    {
                        
                //    };
                //}
                //singleRoom.SingleRate.Cancellation.Add(new Cancellation
                //{
                //    Description = room.RefundPolicy
                //});
                roomList.Add(singleRoom);
            }

            SetRegIdsAndTnc(roomList, input.CheckIn, input.HotelCode);
            //var rooms = new List<HotelRoom>();
            // rooms = SetRoomPerRate(singleHotel.Rooms);
            return roomList;
        }

        public List<Cancellation> ProcessCancellation(string refundPolicy)
        {
            var cancellationList = new List<Cancellation>();
            if (string.IsNullOrEmpty(refundPolicy) || refundPolicy == " ")
                return null;
            var temp = refundPolicy.Split(new string[] { "<br/><br/>" }, StringSplitOptions.None);
            foreach (var x in temp)
            {
                var splitPolicy = x.Split(':');
                if (splitPolicy.Length > 1)
                {
                    var splitItem = splitPolicy[1].Split(new string[] {"<br/>"}, StringSplitOptions.None);
                    foreach (var item in splitItem)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            cancellationList.Add(new Cancellation
                            {
                                Description = item
                            });    
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(x))
                    {
                        cancellationList.Add(new Cancellation
                        {
                            Description = x
                        });
                    }
                }
            }
            return cancellationList;

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
