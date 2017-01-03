using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;
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
            var convertedHotel = new HotelDetailForDisplay
            {
                HotelCode = hotelDetail.HotelCode,
                HotelName = hotelDetail.HotelName,
                Address = hotelDetail.Address,
                City = hotelDetail.City,
                ZoneName = hotelDetail.ZoneCode.Split('-').Length == 2 ?
                    GetZoneNameFromDict(hotelDetail.ZoneCode) : GetZoneNameFromDict(hotelDetail.DestinationCode + "-" + hotelDetail.ZoneCode),
                StarRating = Convert.ToInt32(GetSimpleCodeByCategoryCode(hotelDetail.StarRating)),
                //ChainName = GetHotelChainDesc(hotelDetail.Chain),
                AccomodationName = GetHotelAccomodationDescId(hotelDetail.AccomodationType),
                ImageUrl = hotelDetail.ImageUrl.Where(x => x.Type == "HAB").ToList().Select(y => y.Path).ToList(),
                MainImage = hotelDetail.ImageUrl != null ? hotelDetail.ImageUrl.Where(x=>x.Type=="GEN").Select(x=>x.Path).FirstOrDefault(): null,
                IsRestaurantAvailable = hotelDetail.IsRestaurantAvailable,
                IsWifiAccessAvailable = hotelDetail.WifiAccess,
                Rooms = ConvertToHotelRoomsForDisplay(hotelDetail.Rooms),
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
                PostalCode = hotelDetail.PostalCode
            };
            convertedHotel.OriginalTotalFare = convertedHotel.Rooms.SelectMany(r => r.Rates).Sum(r => r.Breakdowns[0].OriginalTotalFare);
            convertedHotel.NetTotalFare = convertedHotel.Rooms.SelectMany(r => r.Rates).Sum(r => r.Breakdowns[0].NetTotalFare);
            convertedHotel.OriginalCheapestFare = convertedHotel.Rooms.SelectMany(r => r.Rates).Min(r => r.Breakdowns[0].OriginalFare);
            convertedHotel.NetCheapestFare = convertedHotel.Rooms.SelectMany(r => r.Rates).Min(r => r.Breakdowns[0].NetFare);
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
                    ZoneName = hotelDetail.ZoneCode.Split('-').Length == 2 ?
                    GetZoneNameFromDict(hotelDetail.ZoneCode) : GetZoneNameFromDict(hotelDetail.DestinationCode + "-" + hotelDetail.ZoneCode),
                    StarRating = hotelDetail.StarCode != 0
                        ? hotelDetail.StarCode
                        : (hotelDetail.StarRating != null
                            ? GetSimpleCodeByCategoryCode(hotelDetail.StarRating)
                            : 0),
                    //ChainName = GetHotelChainDesc(hotelDetail.Chain),
                    //AccomodationName = GetHotelAccomodationDescId(hotelDetail.AccomodationType),
                    MainImage =
                        hotelDetail.ImageUrl == null
                            ? null
                        : hotelDetail.ImageUrl == null ? null : string.Concat(baseUrl, hotelDetail.ImageUrl.Where(x => x.Type == "GEN").Select(x => x.Path).FirstOrDefault()),
                OriginalTotalFare = hotelDetail.OriginalTotalFare,
                NetTotalFare = hotelDetail.NetTotalFare,
                IsWifiAccessAvailable = hotelDetail.Facilities != null &&
                        ((hotelDetail.Facilities != null || hotelDetail.Facilities.Count != 0) &&
                        hotelDetail.Facilities.Any(f => (f.FacilityGroupCode == 60 && f.FacilityCode == 261)
                        || (f.FacilityGroupCode == 70 && f.FacilityCode == 550))),
                IsRestaurantAvailable = hotelDetail.Facilities != null && ((hotelDetail.Facilities != null || hotelDetail.Facilities.Count != 0) &&
                    hotelDetail.Facilities.Any(f => (f.FacilityGroupCode == 71 && f.FacilityCode == 200)
                    || (f.FacilityGroupCode == 75 && f.FacilityCode == 840)
                    || (f.FacilityGroupCode == 75 && f.FacilityCode == 845))),
                    Rooms = ConvertToHotelRoomsForDisplay(hotelDetail.Rooms),
                    CheckInDate = hotelDetail.CheckInDate,
                    CheckOutDate = hotelDetail.CheckOutDate,
                    NightCount = hotelDetail.NightCount,
                    SpecialRequest = hotelDetail.SpecialRequest,
                    SupplierVat = hotelDetail.SupplierVat,
                    SupplierName = hotelDetail.SupplierName,
                    BookingReference = hotelDetail.BookingReference,
                    ClientReference = hotelDetail.ClientReference,
                    PhonesNumbers = hotelDetail.PhonesNumbers,
                    PostalCode = hotelDetail.PostalCode
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
                    AreaName = GetAreaNameFromDict(hotelDetail.AreaCode),
                    ZoneName = hotelDetail.ZoneCode.Split('-').Length == 2 ?
                    GetZoneNameFromDict(hotelDetail.ZoneCode) : GetZoneNameFromDict(hotelDetail.DestinationCode + "-" + hotelDetail.ZoneCode),
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
                    OriginalTotalFare = hotelDetail.OriginalTotalFare,
                    OriginalCheapestFare = hotelDetail.OriginalCheapestFare,
                    NetTotalFare = hotelDetail.NetTotalFare,
                    NetCheapestFare = hotelDetail.NetCheapestFare,
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
                    PhonesNumbers = hotelDetail.PhonesNumbers,
                    PostalCode = hotelDetail.PostalCode
                };
                convertedHotels.Add(hotel);
            };
            return convertedHotels.ToList();
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
                AreaName = GetAreaNameFromDict(hotelDetail.AreaCode),
                ZoneName = hotelDetail.ZoneCode.Split('-').Length == 2 ?
                    GetZoneNameFromDict(hotelDetail.ZoneCode) : GetZoneNameFromDict(hotelDetail.DestinationCode + "-" + hotelDetail.ZoneCode),
                StarRating = GetSimpleCodeByCategoryCode(hotelDetail.StarRating),
                ChainName = GetHotelChainDesc(hotelDetail.Chain),
                DestinationName = hotelDetail.DestinationName,
                OriginalTotalFare = originalPrice,
                NetTotalFare = netPrice,
                Pois = hotelDetail.Pois,
                Terminals =  hotelDetail.Terminals,
                Facilities = ConvertFacilityForDisplay(hotelDetail.Facilities),
                Review = hotelDetail.Review,
                Rooms = ConvertToHotelRoomsForDisplay(hotelDetail.Rooms),
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
                        if(!data.IsFree)
                            displayFacilities.General.Add(GetHotelFacilityDescId(Convert.ToInt32(data.FullFacilityCode)) +" *");
                        else
                            displayFacilities.General.Add(GetHotelFacilityDescId(Convert.ToInt32(data.FullFacilityCode)));
                        break;
                    case 71:
                        if (displayFacilities.Meal == null)
                            displayFacilities.Meal = new List<string>();
                        if (!data.IsFree)
                            displayFacilities.Meal.Add(GetHotelFacilityDescId(Convert.ToInt32(data.FullFacilityCode)) + " *");
                        else
                            displayFacilities.Meal.Add(GetHotelFacilityDescId(Convert.ToInt32(data.FullFacilityCode)));
                        break;
                    case 72:
                        if (displayFacilities.Business == null)
                            displayFacilities.Business = new List<string>();
                        if (!data.IsFree)
                            displayFacilities.Business.Add(GetHotelFacilityDescId(Convert.ToInt32(data.FullFacilityCode)) + " *");
                        else
                            displayFacilities.Business.Add(GetHotelFacilityDescId(Convert.ToInt32(data.FullFacilityCode)));
                        break;
                    case 73:
                        if (displayFacilities.Entertainment == null)
                            displayFacilities.Entertainment = new List<string>();
                        if (!data.IsFree)
                            displayFacilities.Entertainment.Add(GetHotelFacilityDescId(Convert.ToInt32(data.FullFacilityCode)) + " *");
                        else
                            displayFacilities.Entertainment.Add(
                            GetHotelFacilityDescId(Convert.ToInt32(data.FullFacilityCode)));
                        break;
                    case 74:
                        if (displayFacilities.Health == null)
                            displayFacilities.Health = new List<string>();
                        if (!data.IsFree)
                            displayFacilities.Health.Add(GetHotelFacilityDescId(Convert.ToInt32(data.FullFacilityCode)) + " *");
                        else
                            displayFacilities.Health.Add(GetHotelFacilityDescId(Convert.ToInt32(data.FullFacilityCode)));
                        break;
                    case 90:
                        if (displayFacilities.Sport == null)
                            displayFacilities.Sport = new List<string>();
                        if (!data.IsFree)
                            displayFacilities.Sport.Add(GetHotelFacilityDescId(Convert.ToInt32(data.FullFacilityCode)) + " *");
                        else
                            displayFacilities.Sport.Add(GetHotelFacilityDescId(Convert.ToInt32(data.FullFacilityCode)));
                        break;
                    default:
                        if (displayFacilities.Other == null)
                            displayFacilities.Other = new List<string>();
                        if (data.MustDisplay)
                            if (!data.IsFree)
                                displayFacilities.Other.Add(GetHotelFacilityDescId(Convert.ToInt32(data.FullFacilityCode)) + " *");
                            else
                                displayFacilities.Other.Add(GetHotelFacilityDescId(Convert.ToInt32(data.FullFacilityCode)));
                        break;
                }
            }
            return displayFacilities;
        }

        public List<HotelRoomForDisplay> ConvertToHotelRoomsForDisplay(List<HotelRoom> rooms)
        {
            if (rooms == null)
                return null;
            return rooms.Select(room =>
                new HotelRoomForDisplay
                {
                    RoomCode = room.RoomCode,
                    RoomName = room.RoomName ?? GetHotelRoomDescId(room.RoomCode),
                    Type = room.Type,
                    PaxCapacity = GetPaxCapacity(room.RoomCode),
                    CharacteristicCode = room.characteristicCd,
                    Images = room.Images != null ? ConcateRoomImageUrl(room.Images) : null,
                    Facilities = room.Facilities,
                    SingleRate = ConvertToSingleRateForDisplay(room.SingleRate),
                    Rates = ConvertToRatesForDisplay(room.Rates)
                }).ToList();
        }


        public HotelRoomForDisplay ConvertToSingleHotelRoomForDisplay(HotelRoom roomDetail)
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
                    Rates = ConvertToRatesForDisplay(roomDetail.Rates),
                    SingleRate = ConvertToSingleRateForDisplay(roomDetail.SingleRate)
                };
        }

        public List<string> ConcateRoomImageUrl(List<string> imagesPath)
        {
            if (imagesPath == null)
                return null;
            var baseUrl = ConfigManager.GetInstance().GetConfigValue("hotel", "standardSizeImage");
            return imagesPath.Select(image => string.Concat(baseUrl, image)).ToList();
        }

        internal List<HotelRateForDisplay> ConvertToRatesForDisplay(List<HotelRate> rates)
        {
            if(rates == null)
                return new List<HotelRateForDisplay>();
            var convertedRates = new List<HotelRateForDisplay>();
            foreach (var rateDetail in rates)
            {
                var rate = new HotelRateForDisplay
                {
                    Type = rateDetail.Type,
                    TypeDescription = GetHotelRoomRateTypeId(rateDetail.Type),
                    Class = rateDetail.Class,
                    ClassDescription = GetHotelRoomRateClassId(rateDetail.Class),
                    RegsId = rateDetail.RateKey,
                    Breakdowns = new List<RateBreakdown>
                    {
                        new RateBreakdown
                        {
                            RateCount = rateDetail.RateCount,
                            AdultCount = rateDetail.AdultCount,
                            ChildCount = rateDetail.ChildCount,
                            ChildrenAges = rateDetail.ChildrenAges,
                            Board = rateDetail.Board,
                            BoardDescription = GetHotelBoardDescId(rateDetail.Board),
                        }
                    },
                    Allotment = rateDetail.Allotment,
                    TimeLimit = rateDetail.TimeLimit,
                    Offers = rateDetail.Offers,
                    TermAndCondition = rateDetail.TermAndCondition
                };
                if (rateDetail.Price != null)
                {
                    SetCancellationTime(rate, rateDetail);
                    SetDisplayPriceHotelRate(rate, rateDetail);
                }
                convertedRates.Add(rate);
            }

            var bundledRates = BundleRatesForDisplay(convertedRates);

            return bundledRates;
        }

        private List<HotelRateForDisplay> BundleRatesForDisplay(List<HotelRateForDisplay> rates)
        {
            var bundledRates = new List<HotelRateForDisplay>();

            foreach (var rate in rates)
            {
                var foundRate = bundledRates.FirstOrDefault(x => IsSimilarRate(rate, x));
                if (foundRate == null)
                    bundledRates.Add(rate);
                else
                {
                    foundRate.Allotment = Math.Min(foundRate.Allotment, rate.Allotment);
                    var foundBreakdown = foundRate.Breakdowns.FirstOrDefault(x => IsSimilarBreakdown(rate.Breakdowns[0], x));
                    if (foundBreakdown == null)
                        foundRate.Breakdowns.AddRange(rate.Breakdowns);
                    else
                    {
                        foundBreakdown.RateCount += rate.Breakdowns[0].RateCount;
                        foundBreakdown.OriginalTotalFare += rate.Breakdowns[0].OriginalTotalFare;
                        foundBreakdown.OriginalFare += rate.Breakdowns[0].OriginalFare;
                        foundBreakdown.NetTotalFare += rate.Breakdowns[0].NetTotalFare;
                        foundBreakdown.NetFare += rate.Breakdowns[0].NetFare;
                    }
                }
            }

            return bundledRates;
        }

        private bool IsSimilarBreakdown(RateBreakdown breakdown1, RateBreakdown breakdown2)
        {
            if (breakdown1 == null || breakdown2 == null)
                return false;

            return
                breakdown1.AdultCount == breakdown2.AdultCount &&
                breakdown1.ChildCount == breakdown2.ChildCount &&
                breakdown1.ChildrenAges.Count == breakdown2.ChildrenAges.Count &&
                breakdown1.ChildrenAges.Zip(breakdown2.ChildrenAges, (a, b) => a == b).All(x => x) &&
                breakdown1.Board == breakdown2.Board;
        }

        private bool IsSimilarRate(HotelRateForDisplay rate1, HotelRateForDisplay rate2)
        {
            if (rate1 == null || rate2 == null)
                return false;

            return
                rate1.Type == rate2.Type &&
                rate1.Class == rate2.Class &&
                rate1.TermAndCondition.Zip(rate2.TermAndCondition, (a,b) => a == b).All(x => x) &&
                rate1.PaymentType == rate2.PaymentType &&
                IsSimilarCancellation(rate1.Cancellation, rate2.Cancellation);
        }

        internal HotelRateForDisplay ConvertToSingleRateForDisplay(HotelRate rate)
        {
            if (rate == null)
                return new HotelRateForDisplay();
            var cid = rate.RateKey != null ? rate.RateKey.Split('|')[0] : rate.RegsId.Split('|')[0];
            var checkInDate = new DateTime(Convert.ToInt32(cid.Substring(0, 4)),
                Convert.ToInt32(cid.Substring(4, 2)), Convert.ToInt32(cid.Substring(6, 2)));
            var result = new HotelRateForDisplay
            {
                Type = rate.Type,
                TypeDescription = GetHotelRoomRateTypeId(rate.Type),
                Class = rate.Class,
                ClassDescription = GetHotelRoomRateClassId(rate.Class),
                RegsId = rate.RegsId,
                Breakdowns = new List<RateBreakdown>
                {
                    new RateBreakdown
                    {
                        RateCount = rate.RateCount,
                        AdultCount = rate.AdultCount,
                        ChildCount = rate.ChildCount,
                        ChildrenAges = rate.ChildrenAges,
                        Board = rate.Board,
                        BoardDescription = GetHotelBoardDescId(rate.Board),
                    }
                },
                Allotment = rate.Allotment,
                TimeLimit = rate.TimeLimit,
                Offers = rate.Offers,
                TermAndCondition = GetRateCommentFromTableStorage(rate.RateCommentsId,
                    checkInDate).Select(x => x.Description).ToList()
            };
            SetCancellationTime(result, rate);
            SetDisplayPriceHotelRate(result, rate);
            return result;
        }


        public void SetCancellationTime(HotelRateForDisplay rateDisplay, HotelRate rate)
        {
            var idTimezone = TimeZoneInfo.CreateCustomTimeZone("id", new TimeSpan(0, 7, 0, 0), "Indonesia WIB", "Standar Indonesia");
            rateDisplay.IsRefundable = (rate.Cancellation != null);
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
            rateDisplay.Breakdowns[0].NetTotalFare = rate.Price.Local;
            rateDisplay.Breakdowns[0].OriginalTotalFare = Math.Round(rateDisplay.Breakdowns[0].NetTotalFare*1.01M);
            rateDisplay.Breakdowns[0].NetFare = Math.Round((rateDisplay.Breakdowns[0].NetTotalFare / rate.RateCount) / rate.NightCount);
            rateDisplay.Breakdowns[0].OriginalFare = Math.Round(rateDisplay.Breakdowns[0].NetFare * 1.01M);

            if (rateDisplay.Cancellation != null)
            {
                var margin = rate.Price.MarginNominal / rate.Price.Supplier;
                foreach (var data in rateDisplay.Cancellation)
                {
                    data.Fee = Math.Round(data.Fee * (1 + margin));
                    data.SingleFee = Math.Round((data.Fee / rate.RateCount / rate.NightCount));
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
