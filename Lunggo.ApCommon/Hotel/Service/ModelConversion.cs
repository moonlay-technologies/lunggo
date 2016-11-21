using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.Framework.Config;
using Lunggo.Framework.Extension;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {
        internal HotelReservationForDisplay ConvertToReservationForDisplay(HotelReservation hotelReservation)
        {
            if (hotelReservation == null)
                return null;

            var convertedRsv = new HotelReservationForDisplay
            {
                CancellationTime = hotelReservation.CancellationTime,
                CancellationType = hotelReservation.CancellationType,
                Contact = hotelReservation.Contact,
                HotelDetail = ConvertToHotelDetailWithRoomForDisplay(hotelReservation.HotelDetails),
                Pax = ConvertToPaxForDisplay(hotelReservation.Pax),
                RsvNo = hotelReservation.RsvNo,
                Payment = PaymentService.GetInstance().ConvertToPaymentDetailsForDisplay(hotelReservation.Payment),
                RsvTime = hotelReservation.RsvTime,
                RsvDisplayStatus = MapReservationStatus(hotelReservation)
            };

            return convertedRsv;
        }

        internal HotelDetailForDisplay ConvertToHotelDetailForDisplay(HotelDetailsBase hotelDetail)
        {
            if (hotelDetail == null)
                return null;
            var baseUrl = ConfigManager.GetInstance().GetConfigValue("hotel", "standardSizeImage");
            var price = hotelDetail.Rooms.SelectMany(r => r.Rates).Sum(p => p.Price.Local);
            var convertedHotel = new HotelDetailForDisplay
            {
                HotelCode = hotelDetail.HotelCode,
                HotelName = hotelDetail.HotelName,
                Address = hotelDetail.Address,
                City = hotelDetail.City,
                ZoneName = GetZoneNameFromDict(hotelDetail.ZoneCode),
                StarRating = Convert.ToInt32(hotelDetail.StarRating.Substring(0,1)),
                //ChainName = GetHotelChainDesc(hotelDetail.Chain),
                AccomodationName = GetHotelAccomodationDescId(hotelDetail.AccomodationType),
                MainImage = hotelDetail.ImageUrl != null ? string.Concat(baseUrl ,hotelDetail.ImageUrl.Where(x => x.Type == "GEN").Select(x => x.Path).FirstOrDefault()) : null,
                OriginalFare = price * 1.01M,
                NetFare = price,
                IsRestaurantAvailable = hotelDetail.IsRestaurantAvailable,
                IsWifiAccessAvailable = hotelDetail.WifiAccess,
                Rooms = ConvertToHotelRoomForDisplay(hotelDetail.Rooms),
                CheckInDate = hotelDetail.CheckInDate,
                CheckOutDate = hotelDetail.CheckOutDate,
                NightCount = hotelDetail.NightCount,
                SpecialRequest = hotelDetail.SpecialRequest,
                SupplierVat = hotelDetail.SupplierVat,
                SupplierName = hotelDetail.SupplierName,
                BookingReference = hotelDetail.BookingReference,
                ClientReference = hotelDetail.ClientReference,
                PhonesNumbers = hotelDetail.PhonesNumbers,
                CountryName = GetCountryNameFromDict(hotelDetail.CountryCode).Name,
                DestinationName = GetDestinationNameFromDict(hotelDetail.DestinationCode).Name,

            };
            return convertedHotel;
        }

        internal HotelDetailForDisplay ConvertToHotelDetailWithRoomForDisplay(HotelDetail hotelDetail)
        {
            if (hotelDetail == null)
                return null;
            var baseUrl = ConfigManager.GetInstance().GetConfigValue("hotel", "standardSizeImage");
            var hotel = new HotelDetailForDisplay
            {
                HotelCode = hotelDetail.HotelCode,
                HotelName = hotelDetail.HotelName,
                Address = hotelDetail.Address,
                City = hotelDetail.City,
                CountryName = GetCountryNameFromDict(hotelDetail.CountryCode).Name,
                DestinationName = GetDestinationNameFromDict(hotelDetail.DestinationCode).Name,
                ZoneName = GetZoneNameFromDict(hotelDetail.ZoneCode),
                StarRating = hotelDetail.StarCode != 0
                    ? hotelDetail.StarCode
                    : (hotelDetail.StarRating != null
                        ? Convert.ToInt32(hotelDetail.StarRating.Substring(0, 1))
                        : 0),
                //ChainName = GetHotelChainDesc(hotelDetail.Chain),
                //AccomodationName = GetHotelAccomodationDescId(hotelDetail.AccomodationType),
                MainImage =
                    hotelDetail.ImageUrl == null
                        ? null
                        : hotelDetail.ImageUrl == null ? null : string.Concat(baseUrl, hotelDetail.ImageUrl.Where(x => x.Type == "GEN").Select(x => x.Path).FirstOrDefault()),
                OriginalFare = hotelDetail.OriginalFare,
                OriginalTotalFare = hotelDetail.OriginalTotalFare,
                NetFare = hotelDetail.NetFare,
                NetTotalFare = hotelDetail.NetTotalFare,
                IsWifiAccessAvailable = hotelDetail.Facilities != null &&
                        ((hotelDetail.Facilities != null || hotelDetail.Facilities.Count != 0) &&
                        hotelDetail.Facilities.Any(f => (f.FacilityGroupCode == 60 && f.FacilityCode == 261)
                        || (f.FacilityGroupCode == 70 && f.FacilityCode == 550))),
                IsRestaurantAvailable = hotelDetail.Facilities != null && ((hotelDetail.Facilities != null || hotelDetail.Facilities.Count != 0) &&
                    hotelDetail.Facilities.Any(f => (f.FacilityGroupCode == 71 && f.FacilityCode == 200)
                    || (f.FacilityGroupCode == 75 && f.FacilityCode == 840)
                    || (f.FacilityGroupCode == 75 && f.FacilityCode == 845))),
                Rooms = ConvertToHotelRoomForDisplay(hotelDetail.Rooms),
                CheckInDate = hotelDetail.CheckInDate,
                CheckOutDate = hotelDetail.CheckOutDate,
                NightCount = hotelDetail.NightCount,
                SpecialRequest = hotelDetail.SpecialRequest,
                SupplierVat = hotelDetail.SupplierVat,
                SupplierName = hotelDetail.SupplierName,
                BookingReference = hotelDetail.BookingReference,
                ClientReference = hotelDetail.ClientReference,
                PhonesNumbers = hotelDetail.PhonesNumbers
            };
            return hotel;
        }

        internal List<HotelDetailForDisplay> ConvertToHotelDetailForDisplay(List<HotelDetail> hotelDetails)
        {
            if (hotelDetails == null)
                return null;
            var baseUrl = ConfigManager.GetInstance().GetConfigValue("hotel", "standardSizeImage");
            var convertedHotels = new List<HotelDetailForDisplay>();
            foreach (var hotelDetail in hotelDetails)
            {
                var hotel = new HotelDetailForDisplay
                {
                    HotelCode = hotelDetail.HotelCode,
                    HotelName = hotelDetail.HotelName,
                    Address = hotelDetail.Address,
                    City = hotelDetail.City,
                    CountryName = GetCountryNameFromDict(hotelDetail.CountryCode).Name,
                    DestinationName = GetDestinationNameFromDict(hotelDetail.DestinationCode).Name,
                    ZoneName = GetZoneNameFromDict(hotelDetail.ZoneCode),
                    StarRating = hotelDetail.StarCode != 0
                        ? hotelDetail.StarCode
                        : (hotelDetail.StarRating != null
                            ? Convert.ToInt32(hotelDetail.StarRating.Substring(0, 1))
                            : 0),
                    //ChainName = GetHotelChainDesc(hotelDetail.Chain),
                    //AccomodationName = GetHotelAccomodationDescId(hotelDetail.AccomodationType),
                    MainImage =
                        hotelDetail.ImageUrl == null
                            ? null
                            : hotelDetail.ImageUrl == null ? null : string.Concat(baseUrl, hotelDetail.ImageUrl.Where(x => x.Type == "GEN").Select(x => x.Path).FirstOrDefault()),
                    OriginalFare = hotelDetail.OriginalFare,
                    OriginalTotalFare = hotelDetail.OriginalTotalFare,
                    NetFare = hotelDetail.NetFare,
                    NetTotalFare = hotelDetail.NetTotalFare,
                    IsWifiAccessAvailable = hotelDetail.Facilities != null &&
                            ((hotelDetail.Facilities != null || hotelDetail.Facilities.Count != 0) &&
                            hotelDetail.Facilities.Any(f => (f.FacilityGroupCode == 60 && f.FacilityCode == 261)
                            || (f.FacilityGroupCode == 70 && f.FacilityCode == 550))),
                    IsRestaurantAvailable = hotelDetail.Facilities != null && ((hotelDetail.Facilities != null || hotelDetail.Facilities.Count != 0) &&
                        hotelDetail.Facilities.Any(f => (f.FacilityGroupCode == 71 && f.FacilityCode == 200)
                        || (f.FacilityGroupCode == 75 && f.FacilityCode == 840)
                        || (f.FacilityGroupCode == 75 && f.FacilityCode == 845))),
                    CheckInDate = hotelDetail.CheckInDate,
                    CheckOutDate = hotelDetail.CheckOutDate,
                    NightCount = hotelDetail.NightCount,
                    SpecialRequest = hotelDetail.SpecialRequest,
                    SupplierVat = hotelDetail.SupplierVat,
                    SupplierName = hotelDetail.SupplierName,
                    BookingReference = hotelDetail.BookingReference,
                    ClientReference = hotelDetail.ClientReference,
                    PhonesNumbers = hotelDetail.PhonesNumbers
                };
                convertedHotels.Add(hotel);
            };
            return convertedHotels.ToList();
        }


        public void CalculatePriceHotel(HotelDetail hotel)
        {
            decimal price = 0;
            int night = 0;
            int roomCount = 0;
            foreach (var room in hotel.Rooms)
            {
                foreach (var rate in room.Rates)
                {
                    if (price == 0)
                    {
                        price = rate.Price.Local;
                        night = rate.NightCount;
                        roomCount = rate.RoomCount;
                    }
                    else
                    {
                        price = rate.Price.Local < price ? rate.Price.Local:price;
                        night = rate.NightCount;
                        roomCount = rate.RoomCount;
                    }
                }
            }
            hotel.NetTotalFare = price;
            hotel.OriginalTotalFare = price * 1.01M;
            hotel.NetFare = Math.Round((hotel.NetTotalFare / roomCount) / night);
            hotel.OriginalFare = hotel.NetFare*1.01M;
            
        }

        internal HotelDetailForDisplay ConvertToHotelDetailsBaseForDisplay(HotelDetailsBase hotelDetail, decimal originalPrice, decimal netPrice)
        {
            if (hotelDetail == null)
                return null;
            var hotel = new HotelDetailForDisplay
            {
                HotelCode = hotelDetail.HotelCode,
                HotelName = hotelDetail.HotelName,
                Address = hotelDetail.Address,
                City = hotelDetail.City,
                CountryName = GetHotelCountryName(hotelDetail.CountryCode),
                Latitude = hotelDetail.Latitude,
                Longitude = hotelDetail.Longitude,
                Email = hotelDetail.Email,
                PostalCode = hotelDetail.PostalCode,
                Description = hotelDetail.Description == null ? null : hotelDetail.Description.Where(x => x.languageCode.Equals("IND"))
                                .Select(x => x.Description).SingleOrDefault(),
                PhonesNumbers = hotelDetail.PhonesNumbers,
                ZoneName = GetZoneNameFromDict(hotelDetail.ZoneCode),
                StarRating = GetSimpleCodeByCategoryCode(hotelDetail.StarRating),
                ChainName = GetHotelChainDesc(hotelDetail.Chain),
                OriginalFare = originalPrice,
                NetFare = netPrice,
                Pois = hotelDetail.Pois,
                Terminals =  hotelDetail.Terminals,
                Facilities = ConvertFacilityForDisplay(hotelDetail.Facilities),
                Review = hotelDetail.Review,
                Rooms = ConvertToHotelRoomForDisplay(hotelDetail.Rooms),
                AccomodationName = GetHotelAccomodationMultiDesc(hotelDetail.AccomodationType),
                ImageUrl = ConcateHotelImageUrl(hotelDetail.ImageUrl),
                IsWifiAccessAvailable = hotelDetail.Facilities != null &&
                            ((hotelDetail.Facilities != null || hotelDetail.Facilities.Count != 0) &&
                            hotelDetail.Facilities.Any(f => (f.FacilityGroupCode == 60 && f.FacilityCode == 261)
                            || (f.FacilityGroupCode == 70 && f.FacilityCode == 550))),
                IsRestaurantAvailable = hotelDetail.Facilities != null && ((hotelDetail.Facilities != null || hotelDetail.Facilities.Count != 0) &&
                    hotelDetail.Facilities.Any(f => (f.FacilityGroupCode == 71 && f.FacilityCode == 200)
                    || (f.FacilityGroupCode == 75 && f.FacilityCode == 840)
                    || (f.FacilityGroupCode == 75 && f.FacilityCode == 845))),
                Policy = hotelDetail.Facilities == null ? null : hotelDetail.Facilities.Where(x => x.FacilityGroupCode == 85).Select(x => (GetHotelFacilityDescId(Convert.ToInt32(x.FullFacilityCode)))).ToList()
            };
            return hotel;
        }

        public List<string> ConcateHotelImageUrl(List<Image> images )
        {
            if (images == null)
                return null;
            var baseUrl = ConfigManager.GetInstance().GetConfigValue("hotel", "bigSizeImage");
            var imagePath = images.Select(x => x.Path).ToList();
            return imagePath.Select(image => string.Concat(baseUrl, image)).ToList();
        }

        public HotelFacilityForDisplay ConvertFacilityForDisplay(List<HotelFacility> facilities)
        {
            var displayFacilities = new HotelFacilityForDisplay();
            var selected = facilities.Where(x => x.MustDisplay || x.IsAvailable);
            foreach (var data in selected)
            {
                switch (data.FacilityGroupCode)
                {
                    case 70:
                        if (displayFacilities.General == null)
                            displayFacilities.General = new List<string>();
                        displayFacilities.General.Add(GetHotelFacilityDescId(Convert.ToInt32(data.FullFacilityCode)));
                        break;
                    case 71:
                        if (displayFacilities.Meal == null)
                            displayFacilities.Meal = new List<string>();
                        displayFacilities.Meal.Add(GetHotelFacilityDescId(Convert.ToInt32(data.FullFacilityCode)));
                        break;
                    case 72:
                        if (displayFacilities.Business == null)
                            displayFacilities.Business = new List<string>();
                        displayFacilities.Business.Add(GetHotelFacilityDescId(Convert.ToInt32(data.FullFacilityCode)));
                        break;
                    case 73:
                        if (displayFacilities.Entertainment == null)
                            displayFacilities.Entertainment = new List<string>();
                        displayFacilities.Entertainment.Add(
                            GetHotelFacilityDescId(Convert.ToInt32(data.FullFacilityCode)));
                        break;
                    case 74:
                        if (displayFacilities.Health == null)
                            displayFacilities.Health = new List<string>();
                        displayFacilities.Health.Add(GetHotelFacilityDescId(Convert.ToInt32(data.FullFacilityCode)));
                        break;
                    case 90:
                        if (displayFacilities.Sport == null)
                            displayFacilities.Sport = new List<string>();
                        displayFacilities.Sport.Add(GetHotelFacilityDescId(Convert.ToInt32(data.FullFacilityCode)));
                        break;
                    default:
                        if (displayFacilities.Other == null)
                            displayFacilities.Other = new List<string>();
                        if (data.MustDisplay)
                            displayFacilities.Other.Add(GetHotelFacilityDescId(Convert.ToInt32(data.FullFacilityCode)));
                        break;
                }
            }
            return displayFacilities;
        }

        internal List<HotelRoomForDisplay> ConvertToHotelRoomForDisplay(List<HotelRoom> rooms)
        {
            if (rooms == null)
                return null;
            var dictionary = GetInstance();
            var convertedRoom = new ConcurrentBag<HotelRoomForDisplay>();
            Parallel.ForEach(rooms, roomDetail =>
            {
                var room = new HotelRoomForDisplay
                {
                    RoomCode = roomDetail.RoomCode,
                    RoomName = roomDetail.RoomName ?? GetHotelRoomDescId(roomDetail.RoomCode),
                    Type = roomDetail.Type,
                    PaxCapacity = GetPaxCapacity(roomDetail.RoomCode),
                    //TypeName = dictionary.GetHotelRoomRateTypeId(roomDetail.Type),
                    CharacteristicCode = roomDetail.characteristicCd,
                    //CharacteristicName = dictionary.GetHotelRoomRateTypeId(roomDetail.characteristicCd),
                    Images = roomDetail.Images != null ? ConcateRoomImageUrl(roomDetail.Images) : null,
                    Facilities = roomDetail.Facilities != null ? roomDetail.Facilities : null,
                    SingleRate = ConvertToSingleRateForDisplays(roomDetail.SingleRate),
                    Rates = ConvertToRateForDisplays(roomDetail.Rates)
                };
                convertedRoom.Add(room);
            });
            return convertedRoom.ToList();
        }


        internal HotelRoomForDisplay ConvertToSingleHotelRoomForDisplay(HotelRoom roomDetail)
        {
            if (roomDetail == null)
                return null;
            var dictionary = GetInstance();
            return new HotelRoomForDisplay
                {
                    RoomCode = roomDetail.RoomCode,
                    RoomName = roomDetail.RoomName,
                    Type = roomDetail.Type,
                    TypeName = dictionary.GetHotelRoomRateTypeId(roomDetail.Type),
                    CharacteristicCode = roomDetail.characteristicCd,
                    CharacteristicName = dictionary.GetHotelRoomRateTypeId(roomDetail.characteristicCd),
                    Images = roomDetail.Images != null ? ConcateRoomImageUrl(roomDetail.Images) : null,
                    Facilities = roomDetail.Facilities != null ? roomDetail.Facilities : null,
                    Rates = ConvertToRateForDisplays(roomDetail.Rates),
                    SingleRate = ConvertToSingleRateForDisplays(roomDetail.SingleRate)
                };
        }

        public List<string> ConcateRoomImageUrl(List<string> imagesPath)
        {
            if (imagesPath == null)
                return null;
            var baseUrl = ConfigManager.GetInstance().GetConfigValue("hotel", "standardSizeImage");
            return imagesPath.Select(image => string.Concat(baseUrl, image)).ToList();
        }

        internal List<HotelRateForDisplay> ConvertToRateForDisplays(List<HotelRate> rates)
        {
            if(rates == null)
                return new List<HotelRateForDisplay>();
            var convertedRate = new ConcurrentBag<HotelRateForDisplay>();
            var dictionary = GetInstance();
            Parallel.ForEach(rates,rateDetail =>
            {
                var rate = new HotelRateForDisplay
                {
                    RateKey = rateDetail.RateKey,
                    Type = rateDetail.Type,
                    TypeDescription = dictionary.GetHotelRoomRateTypeId(rateDetail.Type),
                    Class = rateDetail.Class,
                    ClassDescription = dictionary.GetHotelRoomRateClassId(rateDetail.Class),
                    RegsId = rateDetail.RateKey,
                    AdultCount = rateDetail.AdultCount,
                    ChildCount = rateDetail.ChildCount,
                    ChildrenAges = rateDetail.ChildrenAges,
                    Allotment = rateDetail.Allotment,
                    Boards = rateDetail.Boards,
                    BoardDescription = GetHotelBoardDescId(rateDetail.Boards),
                    RoomCount = rateDetail.RateCount == 0 ? rateDetail.RoomCount : rateDetail.RateCount,
                    TimeLimit = rateDetail.TimeLimit,
                    //Cancellation = (rateDetail.Class != "NRF" && rateDetail.Cancellation != null) ? rateDetail.Cancellation : null,
                    //IsRefundable = (rateDetail.Class != "NRF" && rateDetail.Cancellation != null),
                    Offers = rateDetail.Offers,
                    TermAndCondition = rateDetail.TermAndCondition
                };
                SetTimeCancellation(rate, rateDetail);
                SetDisplayPriceHotelRate(rate, rateDetail);
                convertedRate.Add(rate);
            });
            return convertedRate.ToList();
        }


        internal HotelRateForDisplay ConvertToSingleRateForDisplays(HotelRate rate)
        {
            if (rate == null)
                return new HotelRateForDisplay();
            var dictionary = GetInstance();
            var cid = rate.RateKey != null ? rate.RateKey.Split('|')[0] : rate.RegsId.Split('|')[0];
            var checkInDate = new DateTime(Convert.ToInt32(cid.Substring(0, 4)),
                Convert.ToInt32(cid.Substring(4, 2)), Convert.ToInt32(cid.Substring(6, 2)));
            var result = new HotelRateForDisplay
            {
                //RateKey = rateDetail.RateKey,
                Type = rate.Type,
                TypeDescription = dictionary.GetHotelRoomRateTypeId(rate.Type),
                Class = rate.Class,
                ClassDescription = dictionary.GetHotelRoomRateClassId(rate.Class),
                RegsId = rate.RegsId,
                AdultCount = rate.AdultCount,
                ChildCount = rate.ChildCount,
                Allotment = rate.Allotment,
                Boards = rate.Boards,
                BoardDescription = GetHotelBoardDescId(rate.Boards),
                RoomCount = rate.RoomCount,
                TimeLimit = rate.TimeLimit,
                Offers = rate.Offers,
                TermAndCondition = GetRateCommentFromTableStorage(rate.RateCommentsId,
                    checkInDate).Select(x => x.Description).ToList()
            };
            SetTimeCancellation(result, rate);
            SetDisplayPriceHotelRate(result, rate);
            return result;
        }


        public void SetTimeCancellation(HotelRateForDisplay rateDisplay, HotelRate rate)
        {
            var idTimezone = TimeZoneInfo.CreateCustomTimeZone("id", new TimeSpan(0, 7, 0, 0), "Indonesia WIB", "Standar Indonesia");
            rateDisplay.IsRefundable = (rate.Class != "NRF" && rate.Cancellation != null);
            //rateDisplay.Cancellation = (rate.Class != "NRF" && rate.Cancellation != null) ? rate.Cancellation : null;
            if (rateDisplay.IsRefundable)
            {
                rateDisplay.Cancellation = new List<Cancellation>();
                rate.Cancellation= rate.Cancellation.OrderBy(e => e.StartTime).ToList();
                foreach (var data in rate.Cancellation)
                {
                    var obj = new Cancellation
                    {
                        Fee = data.Fee,
                        StartTime = TimeZoneInfo.ConvertTimeFromUtc(data.StartTime.AddDays(-1),idTimezone)
                    };
                    rateDisplay.Cancellation.Add(obj);
                }
                if (rate.Cancellation[0].StartTime.AddDays(-1) > DateTime.UtcNow)
                {
                    DateTime freeUntil = rate.Cancellation[0].StartTime.AddDays(-1).AddMinutes(-1);
                    rateDisplay.IsFreeCancel = true;
                    freeUntil = TimeZoneInfo.ConvertTimeFromUtc(freeUntil, idTimezone);
                    rateDisplay.FreeUntil = freeUntil;
                }
            }
        }

        public void SetDisplayPriceHotelRate(HotelRateForDisplay rateDisplay,HotelRate rate)
        {
            rateDisplay.NetTotalPrice = rate.Price.Local;
            rateDisplay.OriginalTotalPrice = rateDisplay.NetTotalPrice*1.01M;

            rateDisplay.NetPrice = Math.Round((rateDisplay.NetTotalPrice / rate.RoomCount) / rate.NightCount);
            rateDisplay.OriginalPrice = rateDisplay.NetPrice * 1.01M;

            if (!rate.Class.Equals("NRF") && rateDisplay.Cancellation != null)
            {
                var margin = rate.Price.MarginNominal / rate.Price.Supplier;
                foreach (var data in rateDisplay.Cancellation)
                {
                    data.Fee = data.Fee * (1 + margin);
                    data.SingleFee = Math.Round((data.Fee/rate.RoomCount)/rate.NightCount);
                }
            }
        }

        private static RsvDisplayStatus MapReservationStatus(HotelReservation reservation)
        {
            var paymentStatus = reservation.Payment.Status;
            var paymentMethod = reservation.Payment.Method;
            var rsvStatus = reservation.RsvStatus;

            if (rsvStatus == RsvStatus.Cancelled || paymentStatus == PaymentStatus.Cancelled)
                return RsvDisplayStatus.Cancelled;
            if (rsvStatus == RsvStatus.Expired || paymentStatus == PaymentStatus.Expired)
                return RsvDisplayStatus.Expired;
            if (paymentStatus == PaymentStatus.Denied)
                return RsvDisplayStatus.PaymentDenied;
            if (paymentStatus == PaymentStatus.Failed)
                return RsvDisplayStatus.FailedUnpaid;
            if (rsvStatus == RsvStatus.Failed)
                return paymentStatus == PaymentStatus.Settled
                    ? RsvDisplayStatus.FailedPaid
                    : RsvDisplayStatus.FailedUnpaid;
            if (paymentMethod == PaymentMethod.Undefined)
                return RsvDisplayStatus.Reserved;
            if (paymentStatus == PaymentStatus.Settled)
                return reservation.RsvStatus == RsvStatus.Completed
                    ? RsvDisplayStatus.Issued
                    : RsvDisplayStatus.Paid;
            if (paymentStatus != PaymentStatus.Settled)
                return (paymentMethod == PaymentMethod.VirtualAccount || paymentMethod == PaymentMethod.BankTransfer)
                    ? RsvDisplayStatus.PendingPayment
                    : RsvDisplayStatus.VerifyingPayment;
            return RsvDisplayStatus.Undefined;
        }

    }
}
