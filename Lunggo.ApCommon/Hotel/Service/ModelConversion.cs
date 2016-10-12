using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Content.Model;
using Lunggo.ApCommon.Travolutionary.WebService.Hotel;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {
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
                CountryCode = hotelDetail.CountryCode,
                //CountryName = CountryName //TODO "Get Country Name"
                Latitude = hotelDetail.Latitude,
                Longitude = hotelDetail.Longitude,
                Email = hotelDetail.Email,
                PostalCode = hotelDetail.PostalCode,
                DestinationCode = hotelDetail.DestinationCode,
                //DestinationName =  DestinationName //TODO "Get Destination Name"
                Description = hotelDetail.Description.Where(x=>x.languageCode.Equals("IND"))
                                .Select(x=>new HotelDescriptions
                                {
                                    languageCode = x.languageCode,
                                    Description = x.Description
                                }).SingleOrDefault(),
                PhonesNumbers = hotelDetail.PhonesNumbers,
                ZoneCode = hotelDetail.ZoneCode,
                //ZoneName = ZoneName, //TODO "Det Zone Name"
                StarRatingCd = hotelDetail.StarRating,
                //StarRatingDescription = StarRating //TODO "Get Star Rating"
                Chain = hotelDetail.Chain,
                //ChainName = Hotel.ChainName //TODO "Get Chain Name"
                //Segments =  //TODO "List of Segment by SegmentCode"
                Pois = hotelDetail.Pois,
                //Terminals =  //TODO "Perlu dtambahi dari data HotelDetailContent"
                //Facilities = "Mapping data berdasarkan FacilitiesCode" //TODO
                Review = hotelDetail.Review,
                Rooms = ConvertToHotelRoomForDisplay(hotelDetail.Rooms),
                AccomodationType = hotelDetail.AccomodationType,
            };
            convertedHotels.Add(hotel);
            }
            return convertedHotels;
        }

        internal List<HotelRoomForDisplay> ConvertToHotelRoomForDisplay(List<HotelRoom> rooms)
        {
            if (rooms == null)
                return null;
            var convertedRoom = new List<HotelRoomForDisplay>();
            foreach (var roomDetail in rooms)
            {
                var room = new HotelRoomForDisplay
                {
                    RoomCode = roomDetail.RoomCode,
                    RoomName = roomDetail.RoomName,
                    Type = roomDetail.Type,
                    //TypeName = //TODO "Mapping Type Name"
                    CharacteristicCode = roomDetail.characteristicCd,
                    //CharacteristicName = //TODO "Mapping Characteristic Name"
                    Images = roomDetail.Images != null ? roomDetail.Images : null,
                    Facilities = roomDetail.Facilities != null ? roomDetail.Facilities : null,
                    Rates = ConvertToRateForDisplays(roomDetail.Rates)
                };
                convertedRoom.Add(room);
            }
            return convertedRoom;
        }

        internal List<HotelRateForDisplay> ConvertToRateForDisplays(List<HotelRate> rates)
        {
            if(rates == null)
                return new List<HotelRateForDisplay>();
            var convertedRate = new List<HotelRateForDisplay>();
            foreach (var rateDetail in rates)
            {
                var rate = new HotelRateForDisplay()
                {
                    RateKey = rateDetail.RateKey,
                    Type = rateDetail.Type,
                    //TypeDescription = //TODO "Mapping Rate Type"
                    Class = rateDetail.Class,
                    //ClassDescription = //TODO 
                    RegsId = rateDetail.RegsId,
                    Price = rateDetail.Price!=null?rateDetail.Price:null,
                    AdultCount = rateDetail.AdultCount,
                    ChildCount = rateDetail.ChildCount,
                    Boards = rateDetail.Boards,
                    //BoardDescription = //TODO
                    RoomCount = rateDetail.RoomCount,
                    TimeLimit = rateDetail.TimeLimit,
                    Cancellation = rateDetail.Cancellation,
                    Offers = rateDetail.Offers,
                };
                convertedRate.Add(rate);
            }
            return convertedRate;
        } 

    }
}
