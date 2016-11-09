using System;
using System.Collections.Generic;
using System.Linq;
using CsQuery.ExtensionMethods;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.ApCommon.Product.Constant;

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
                HotelDetail = ConvertToHotelDetailForDisplay(
                new List<HotelDetail>
                {
                    hotelReservation.HotelDetails
                }).ToList()[0],
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
            var price = hotelDetail.Rooms.SelectMany(r => r.Rates).Sum(p => p.Price.Local);
            var convertedHotel = new HotelDetailForDisplay
            {
                HotelCode = hotelDetail.HotelCode,
                HotelName = hotelDetail.HotelName,
                Address = hotelDetail.Address,
                City = hotelDetail.City,
                ZoneName = GetZoneNameFromDict(hotelDetail.DestinationCode + "-" + hotelDetail.ZoneCode),
                StarRating = Convert.ToInt32(hotelDetail.StarRating.Substring(0,1)),
                //ChainName = GetHotelChainDesc(hotelDetail.Chain),
                AccomodationName = GetHotelAccomodationDescId(hotelDetail.AccomodationType),
                MainImage = hotelDetail.ImageUrl.Select(x => x.Path).FirstOrDefault(),// != null ? hotelDetail.ImageUrl.Where(x=>x.Type=="GEN").Select(x=>x.Path).FirstOrDefault(): null,
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
        internal List<HotelDetailForDisplay> ConvertToHotelDetailForDisplay(List<HotelDetail> hotelDetails)
        {
            if (hotelDetails == null)
                return null;
            int i = 0;
            var convertedHotels = new List<HotelDetailForDisplay>();
            foreach (var hotelDetail in hotelDetails)
            {
                i++;
                var hotel = new HotelDetailForDisplay
                {
                HotelCode = hotelDetail.HotelCode,
                HotelName = hotelDetail.HotelName,
                Address = hotelDetail.Address,
                City = hotelDetail.City,
                CountryName = GetCountryNameFromDict(hotelDetail.CountryCode).Name,
                DestinationName = GetDestinationNameFromDict(hotelDetail.DestinationCode).Name,
                ZoneName = GetZoneNameFromDict(hotelDetail.DestinationCode + "-" + hotelDetail.ZoneCode),
                StarRating = hotelDetail.StarCode != 0 ? hotelDetail.StarCode : (hotelDetail.StarRating != null ?
                Convert.ToInt32(hotelDetail.StarRating.Substring(0,1)) : 0),
                //ChainName = GetHotelChainDesc(hotelDetail.Chain),
                    //AccomodationName = GetHotelAccomodationDescId(hotelDetail.AccomodationType),
                MainImage = hotelDetail.ImageUrl == null ? null : hotelDetail.ImageUrl.FirstOrDefault().Path,
                    // != null ? hotelDetail.ImageUrl.Where(x=>x.Type=="GEN").Select(x=>x.Path).FirstOrDefault(): null,
                OriginalFare = hotelDetail.OriginalFare,
                NetFare = hotelDetail.NetFare,
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
                PhonesNumbers = hotelDetail.PhonesNumbers
            };
            convertedHotels.Add(hotel);
            }
            return convertedHotels;
        }


        public void CalculatePriceHotel(HotelDetail hotel)
        {
            decimal price = 0;
            foreach (var room in hotel.Rooms)
            {
                foreach (var rate in room.Rates)
                {
                    if (price == 0)
                    {
                        price = rate.Price.Local;
                    }
                    else
                    {
                        price = rate.Price.Local < price ? rate.Price.Local:price;
                    }
                }
            }
            hotel.NetFare = price;
            hotel.OriginalFare = price*1.01M;
        }

        internal HotelDetailForDisplay ConvertToHotelDetailsBaseForDisplay(HotelDetailsBase hotelDetail, decimal originalPrice, decimal netPrice)
        {
            if (hotelDetail == null)
                return null;
            var convertedHotels = new List<HotelDetailForDisplay>();
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
                ZoneName = GetZoneNameFromDict(hotelDetail.DestinationCode +"-"+ hotelDetail.ZoneCode),
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
                ImageUrl = hotelDetail.ImageUrl.Select(x => x.Path).ToList(),
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

        public HotelFacilityForDisplay ConvertFacilityForDisplay(List<HotelFacility> facilities)
        {
            var displayFacilities = new HotelFacilityForDisplay();
            var selected = facilities.Where(x => x.MustDisplay == true || x.IsAvailable == true);
            foreach (var data in selected)
            {
                switch (data.FacilityGroupCode)
                {
                    case 70:
                        if(displayFacilities.General == null)
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
                        displayFacilities.Entertainment.Add(GetHotelFacilityDescId(Convert.ToInt32(data.FullFacilityCode)));
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
                        if(displayFacilities.Other == null)
                            displayFacilities.Other = new List<string>();
                        if(data.MustDisplay == true)
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
            var dictionary = HotelService.GetInstance();
            var convertedRoom = new List<HotelRoomForDisplay>();
            foreach (var roomDetail in rooms)
            {
                var room = new HotelRoomForDisplay
                {
                    RoomCode = roomDetail.RoomCode,
                    RoomName = roomDetail.RoomName ?? GetHotelRoomDescId(roomDetail.RoomCode),
                    Type = roomDetail.Type,
                    PaxCapacity = GetPaxCapacity(roomDetail.RoomCode),
                    TypeName = dictionary.GetHotelRoomRateTypeId(roomDetail.Type),
                    CharacteristicCode = roomDetail.characteristicCd,
                    CharacteristicName = dictionary.GetHotelRoomRateTypeId(roomDetail.characteristicCd),
                    Images = roomDetail.Images != null ? roomDetail.Images : null,
                    Facilities = roomDetail.Facilities != null ? roomDetail.Facilities : null,
                    SingleRate = ConvertToSingleRateForDisplays(roomDetail.SingleRate),
                    Rates = ConvertToRateForDisplays(roomDetail.Rates)
                };
                convertedRoom.Add(room);
            }
            return convertedRoom;
        }


        internal HotelRoomForDisplay ConvertToSingleHotelRoomForDisplay(HotelRoom roomDetail)
        {
            if (roomDetail == null)
                return null;
            var dictionary = HotelService.GetInstance();
            return new HotelRoomForDisplay
                {
                    RoomCode = roomDetail.RoomCode,
                    RoomName = roomDetail.RoomName,
                    Type = roomDetail.Type,
                    TypeName = dictionary.GetHotelRoomRateTypeId(roomDetail.Type),
                    CharacteristicCode = roomDetail.characteristicCd,
                    CharacteristicName = dictionary.GetHotelRoomRateTypeId(roomDetail.characteristicCd),
                    Images = roomDetail.Images != null ? roomDetail.Images : null,
                    Facilities = roomDetail.Facilities != null ? roomDetail.Facilities : null,
                    Rates = ConvertToRateForDisplays(roomDetail.Rates),
                    SingleRate = ConvertToSingleRateForDisplays(roomDetail.SingleRate)
                };
        }

        internal List<HotelRateForDisplay> ConvertToRateForDisplays(List<HotelRate> rates)
        {
            if(rates == null)
                return new List<HotelRateForDisplay>();
            var convertedRate = new List<HotelRateForDisplay>();
            var dictionary = HotelService.GetInstance();
            foreach (var rateDetail in rates)
            {
                var cid = rateDetail.RateKey != null ? rateDetail.RateKey.Split('|')[0] : rateDetail.RegsId.Split('|')[0];
                var checkInDate = new DateTime(Convert.ToInt32(cid.Substring(0, 4)),
                    Convert.ToInt32(cid.Substring(4, 2)), Convert.ToInt32(cid.Substring(6, 2)));
                var rate = new HotelRateForDisplay()
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
                    Cancellation = rateDetail.Cancellation,
                    Offers = rateDetail.Offers,
                    TermAndCondition = GetRateCommentFromTableStorage(rateDetail.RateCommentsId,
                    checkInDate).Select(x => x.Description).ToList()
                    
                };
                SetDisplayPriceHotelRate(rate, rateDetail);
                convertedRate.Add(rate);
            }
            return convertedRate;
        }


        internal HotelRateForDisplay ConvertToSingleRateForDisplays(HotelRate rate)
        {
            if (rate == null)
                return new HotelRateForDisplay();
            var dictionary = HotelService.GetInstance();
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
                //Boards = rateDetail.Boards,
                BoardDescription = GetHotelBoardDescId(rate.Boards),
                RoomCount = rate.RoomCount,
                TimeLimit = rate.TimeLimit,
                Cancellation = rate.Cancellation,
                Offers = rate.Offers,
                TermAndCondition = GetRateCommentFromTableStorage(rate.RateCommentsId,
                    checkInDate).Select(x => x.Description).ToList()
            };
            SetDisplayPriceHotelRate(result, rate);
            return result;
        }



        public void SetDisplayPriceHotelRate(HotelRateForDisplay rateDisplay,HotelRate rate)
        {
            rateDisplay.NetPrice = rate.Price.Local;
            rateDisplay.OriginalPrice = rateDisplay.NetPrice*1.01M;
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
