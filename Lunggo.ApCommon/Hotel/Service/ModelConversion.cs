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
                Passengers = ConvertToPaxForDisplay(hotelReservation.Pax),
                RsvNo = hotelReservation.RsvNo,
                Payment = PaymentService.GetInstance().ConvertToPaymentDetailsForDisplay(hotelReservation.Payment),
                RsvTime = hotelReservation.RsvTime,
            };

            return convertedRsv;
        }

        internal List<HotelDetailForDisplay> ConvertToHotelDetailForDisplay(List<HotelDetail> hotelDetails)
        {
            if (hotelDetails == null)
                return null;
            var convertedHotels = new List<HotelDetailForDisplay>();
            foreach (var hotelDetail in hotelDetails)
            {
                var hotel =   new HotelDetailForDisplay
                {
                HotelCode = hotelDetail.HotelCode,
                HotelName = hotelDetail.HotelName,
                Address = hotelDetail.Address,
                City = hotelDetail.City,
                ZoneName = GetHotelZoneNameFromDict(hotelDetail.DestinationCode+"-"+hotelDetail.ZoneCode),
                StarRating = hotelDetail.StarCode,
                ChainName = GetHotelChainDesc(hotelDetail.Chain),
                AccomodationName = GetHotelAccomodationDescId(hotelDetail.AccomodationType),
                MainImage = hotelDetail.ImageUrl != null ? hotelDetail.ImageUrl.Where(x=>x.Type=="GEN").Select(x=>x.Path).FirstOrDefault()
                : null,
                OriginalFare = hotelDetail.OriginalFare,
                NetFare = hotelDetail.NetFare,
                Rooms = ConvertToHotelRoomForDisplay(hotelDetail.Rooms)
            };
            convertedHotels.Add(hotel);
            }
            return convertedHotels;
        }

        internal HotelDetailForDisplay ConvertToHotelDetailsBaseForDisplay(HotelDetailsBase hotelDetail)
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
                ZoneName = GetHotelZoneNameFromDict(hotelDetail.DestinationCode + "-" + hotelDetail.ZoneCode),
                StarRating = hotelDetail.StarCode,
                ChainName = GetHotelChainDesc(hotelDetail.Chain),
                //Segments =  //TODO "List of Segment by SegmentCode"
                Pois = hotelDetail.Pois,
                Terminals =  hotelDetail.Terminals,
                Facilities = hotelDetail.Facilities.Select(x =>(GetHotelFacilityDescId(Convert.ToInt32(x.FullFacilityCode)))).ToList(),
                Review = hotelDetail.Review,
                Rooms = ConvertToHotelRoomForDisplay(hotelDetail.Rooms),
                AccomodationName = GetHotelAccomodationMultiDesc(hotelDetail.AccomodationType),
                ImageUrl = hotelDetail.ImageUrl.Select(x => x.Path).ToList(),
            };
            return hotel;
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
                    RoomName = roomDetail.RoomName,
                    Type = roomDetail.Type,
                    //TypeName = dictionary.GetHotelRoomRateTypeId(roomDetail.Type),
                    //CharacteristicCode = roomDetail.characteristicCd,
                    //CharacteristicName = dictionary.GetHotelRoomRateTypeId(roomDetail.characteristicCd),
                    Images = roomDetail.Images != null ? roomDetail.Images : null,
                    Facilities = roomDetail.Facilities != null ? roomDetail.Facilities : null,
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
                    Rates = ConvertToRateForDisplays(roomDetail.Rates)
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
                var rate = new HotelRateForDisplay()
                {
                    //RateKey = rateDetail.RateKey,
                    Type = rateDetail.Type,
                    TypeDescription = dictionary.GetHotelRoomRateTypeId(rateDetail.Type),
                    Class = rateDetail.Class,
                    ClassDescription = dictionary.GetHotelRoomRateClassId(rateDetail.Class),
                    RegsId = rateDetail.RegsId,
                    Price = rateDetail.Price!=null?rateDetail.Price:null,
                    AdultCount = rateDetail.AdultCount,
                    ChildCount = rateDetail.ChildCount,
                    Allotment = rateDetail.Allotment,
                    //Boards = rateDetail.Boards,
                    BoardDescription = GetHotelBoardDescId(rateDetail.Boards),
                    RoomCount = rateDetail.RoomCount,
                    TimeLimit = rateDetail.TimeLimit,
                    Cancellation = rateDetail.Cancellation,
                    Offers = rateDetail.Offers,
                };
                convertedRate.Add(rate);
            }
            return convertedRate;
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
