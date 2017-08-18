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
using Lunggo.Framework.Config;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {
        public AvailableRatesOutput GetAvailableRates(AvailableRatesInput input)
        {
            //var occupancies = PreProcessOccupancies(input.Occupancies);
            //var hotelSearchResult = GetSearchHotelResultFromCache(input.SearchId);
            //SetHotelFullFacilityCode(hotelDetail);
            //if (hotelSearchResult == null)
            //    return new AvailableRatesOutput
            //    {
            //        IsSuccess = false,
            //        Errors = new List<HotelError> { HotelError.TechnicalError },
            //        ErrorMessages = new List<string> { "Search Id is not vaild" }
            //    };

            //Find Hotel Uri based on HotelCode
            //var hotel = hotelSearchResult.HotelDetails.SingleOrDefault(x => x.HotelCode == input.HotelCode);
            var hotel = GetAvailableRatesFromCache(input.HotelDetailId);
            if (hotel == null)
                return new AvailableRatesOutput
                {
                    IsSuccess = false,
                    Errors = new List<HotelError> { HotelError.TechnicalError },
                    ErrorMessages = new List<string> { "Hotel Code is not valid" }
                };

            //Guid generatedSearchId = Guid.NewGuid();
            //var hotelBedsClient = new HotelBedsSearchHotel();
            var allCurrency = Currency.GetAllCurrencies();
            SaveAllCurrencyToCache(input.HotelDetailId, allCurrency);
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

                var rooms = ProcessRoomFromTiketHotelDetail(hotel,resultDetail, input);
                var hotelDetail =  new HotelDetail
                {
                    SearchId = hotel.SearchId,
                    DestinationName = hotel.DestinationName,
                    StarCode = hotel.StarCode,
                    Supplier = hotel.Supplier,
                    Longitude = hotel.Longitude,
                    Latitude = hotel.Latitude,
                    Address = hotel.Address,
                    HotelCode = hotel.HotelCode,
                    HotelName = hotel.HotelName,
                    City = hotel.City,
                    ZoneCode =
                        hotel.ZoneCode,
                    ImageUrl = hotel.ImageUrl,
                    PrimaryPhoto = hotel.PrimaryPhoto,
                    HotelUri = hotel.HotelUri,
                    CheckInDate = hotel.CheckInDate,
                    CheckOutDate = hotel.CheckOutDate,
                    NightCount = hotel.NightCount,
                    CountryCode = hotel.CountryCode,
                    Facilities =
                        hotel.Facilities,
                        TotalChildren = hotel.TotalChildren,
                        TotalAdult =  hotel.TotalAdult,
                    TiketToken = hotel.TiketToken,
                    Description = hotel.Description
                };
                hotelDetail.Rooms = rooms;
                var hotelList = new List<HotelDetail>();
                hotelList.Add(hotelDetail);
                
                AddPriceMargin(hotelList);
                hotel.Rooms = hotelDetail.Rooms;
                SaveAvailableRateToCache(input.HotelDetailId, hotel);
                return new AvailableRatesOutput
                {
                    Id = input.HotelDetailId,
                    IsSuccess = true,
                    Total = rooms == null ? 0 : rooms.Count,
                    Rooms = rooms == null ? null : ConvertToTiketHotelRoomsForDisplay(hotel.Rooms),
                    ExpiryTime = GetAvailableRatesExpiry(input.HotelDetailId)
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


        public List<HotelRoom> ProcessRoomFromTiketHotelDetail(HotelDetailsBase hotel,TiketHotelDetailResponse result, AvailableRatesInput input)
        {
            var roomList = new List<HotelRoom>();
            if (result.Results == null || result.Results.RoomDetail == null)
                return null;
             var allCurrencies = HotelService.GetInstance().GetAllCurrenciesFromCache(input.HotelDetailId);
            foreach (var room in result.Results.RoomDetail)
            {
                var singleRoom = new HotelRoom
                {
                    RoomCode = room.room_id,
                    Images = room.PhotoRooms,
                    RoomName = room.room_name,
                    RoomAvailable = room.RoomAvailable,
                    Type = "SGL",
                    RoomDescription = room.RoomDescription,
                    SingleRate = new HotelRate
                    {
                        RateKey = ConstructRateKey(hotel, room.Id),
                        Board = room.WithBreakfasts == "1" ? "BB" : "RO",
                        NightCount = input.Nights,
                        AdultCount = hotel.TotalAdult,
                        ChildCount = hotel.TotalChildren,
                        RateCount = hotel.RoomCount,
                        BookingUri = room.BookUri,
                        PaymentType = PaymentTypeEnum.AT_WEB,
                        Cancellation = ProcessCancellation(room.RefundPolicy),
                        Price = new Price()
                    },
                    Rates = new List<HotelRate>()
                };

                /*
                 * Harga Tiket merupakan harga satuan, jadi untuk mendapat harga total harus dikalikan
                 * dengan jumlah kamar dan jumlah malam
                 */
                room.Price = room.Price*input.Nights*hotel.RoomCount;
                
                singleRoom.SingleRate.Price.SetSupplier(room.Price,
                    result.Diagnostic.Currency != null
                        ? allCurrencies[result.Diagnostic.Currency]
                        : ConfigManager.GetInstance().GetConfigValue("general", "environment") == "production"
                            ? allCurrencies["IDR"]
                            : room.Price < 10000
                                ? allCurrencies["USD"]
                                : allCurrencies["IDR"]);
                singleRoom.Rates.Add(singleRoom.SingleRate);
                roomList.Add(singleRoom);
            }

            SetRegIdsAndTnc(roomList, input.CheckIn, input.HotelCode);
            //var rooms = new List<HotelRoom>();
            // rooms = SetRoomPerRate(singleHotel.Rooms);
            return roomList;
        }


        public string ConstructRateKey(HotelDetailsBase hotel, string roomId)
        {
            string rateKey = hotel.CheckInDate.ToString("yyyy") + "" + hotel.CheckInDate.ToString("MM") + "" + hotel.CheckInDate.ToString("dd") + "|"
                             + hotel.CheckOutDate.ToString("yyyy") + "" + hotel.CheckOutDate.ToString("MM") + "" + hotel.CheckOutDate.ToString("dd") + "|"
                             + "W|1|6914|DUS.ST|CG-TODOS|BB||" + hotel.NightCount + "~" + hotel.TotalAdult + "~" + hotel.TotalChildren + "||" + roomId;
            return rateKey;
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
