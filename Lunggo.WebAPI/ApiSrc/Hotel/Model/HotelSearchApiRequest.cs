using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Model
{
    public class HotelSearchApiRequest
    {
        [JsonProperty("searchId")]
        public string SearchId { get; set; }
        [JsonProperty("location")]
        public long Location { get; set; }
        [JsonProperty("checkinDate")]
        public DateTime CheckinDate { get; set; }
        [JsonProperty("checkoutDate")]
        public DateTime CheckoutDate { get; set; }
        [JsonProperty("adultCount")]
        public int AdultCount { get; set; }
        [JsonProperty("childCount")]
        public int ChildCount { get; set; }
        [JsonProperty("nightCount")]
        public int NightCount { get; set; }
        [JsonProperty("from")]
        public int From { get; set; }
        [JsonProperty("to")]
        public int To { get; set; }
        [JsonProperty("roomCount")]
        public int RoomCount { get; set; }

        [JsonProperty("hotelFilter")]
        public HotelRequestFilter Filter { get; set; }
        [JsonProperty("hotelSorting")]
        public HotelRequestSorting Sorting { get; set; }
    }

    public class HotelRequestFilter
    {
        [JsonProperty("priceFilter")]
        public PriceFilter PriceFilter { get; set; }
        [JsonProperty("starFilter")]
        public StarFilter StarFilter { get; set; }
        [JsonProperty("accommodationTypeFilter")]
        public AccommodationTypeFilter AccommodationTypeFilter { get; set; }
        [JsonProperty("amenitiesFilter")]
        public AmenitiesFilter AmenitiesFilter { get; set; }
        [JsonProperty("boardFilter")]
        public BoardFilter BoardFilter { get; set; }
        [JsonProperty("areaFilter")]
        public AreaFilter AreaFilter { get; set; }
    }

    public class PriceFilter
    {
        [JsonProperty("MinPrice")]
        public decimal MinPrice { get; set; }
        [JsonProperty("MaxPrice")]
        public decimal MaxPrice { get; set; }
    }

    public class StarFilter
    {
        [JsonProperty("oneStar")]
        public bool OneStar { get; set; }
        [JsonProperty("twoStar")]
        public bool TwoStar { get; set; }
        [JsonProperty("threeStar")]
        public bool ThreeStar { get; set; }
        [JsonProperty("fourStar")]
        public bool FourStar { get; set; }
        [JsonProperty("fiveStar")]
        public bool FiveStar { get; set; }
    }

    public class AccommodationTypeFilter
    {
        [JsonProperty("apartment")]
        public bool Apartment { get; set; }
        [JsonProperty("apartHotel")]
        public bool ApartHotel { get; set; }
        [JsonProperty("camping")]
        public bool Camping { get; set; }
        [JsonProperty("villa")]
        public bool Villa { get; set; }
        [JsonProperty("hostel")]
        public bool Hostel { get; set; }
        [JsonProperty("hotel")]
        public bool Hotel { get; set; }
        [JsonProperty("resort")]
        public bool Resort { get; set; }
        [JsonProperty("rural")]
        public bool Rural { get; set; }
    }

    public class AmenitiesFilter
    {
        [JsonProperty("parkingSpace")]
        public bool ParkingSpace { get; set; }
        [JsonProperty("internet")]
        public bool Internet { get; set; }
        [JsonProperty("frontDesk24Hour")]
        public bool FrontDesk24Hour { get; set; }
        [JsonProperty("swimmingPool")]
        public bool SwimmingPool { get; set; }
        [JsonProperty("restaurant")]
        public bool Restaurant { get; set; }
        [JsonProperty("meetingFacilities")]
        public bool MeetingFacilities { get; set; }
        [JsonProperty("nonSmokingRoom")]
        public bool NonSmokingRoom { get; set; }
        [JsonProperty("sportFacilities")]
        public bool SportFacilities { get; set; }
        [JsonProperty("airConditioner")]
        public bool AirConditioner { get; set; }
        [JsonProperty("spa")]
        public bool Spa { get; set; }
        [JsonProperty("disabilityFriendly")]
        public bool DisabilityFriendly { get; set; }
        [JsonProperty("wheelchair")]
        public bool Wheelchair { get; set; }
        [JsonProperty("lift")]
        public bool Lift { get; set; }
        [JsonProperty("gymAndFitness")]
        public bool GymAndFitness { get; set; }
    }

    public class BoardFilter
    {
        [JsonProperty("americanBreakfast")]
        public bool AmericanBreakfast { get; set; }
        [JsonProperty("allInclusive")]
        public bool AllInclusive { get; set; }
        [JsonProperty("allInclusiveSpecial")]
        public bool AllInclusiveSpecial { get; set; }
        [JsonProperty("bnb")]
        public bool BedAndBreakfast { get; set; }
        [JsonProperty("bnbHalfBoard")]
        public bool BnbHalfBoard { get; set; }
        [JsonProperty("lift")]
        public bool ContinentalBreakfast { get; set; }
        [JsonProperty("continentalBreakfast")]
        public bool BuffetBreakfast { get; set; }
        [JsonProperty("fb")]
        public bool FullBoard { get; set; }
        [JsonProperty("hbFb")]
        public bool HalfBoardFullBoard { get; set; }
        [JsonProperty("englishBreakfast")]
        public bool EnglishBreakfast { get; set; }
        [JsonProperty("hb")]
        public bool HalfBoard { get; set; }
        [JsonProperty("irishBreakfast")]
        public bool IrishBreakfast { get; set; }
        [JsonProperty("hbWithBeverages")]
        public bool HbWithBeverages { get; set; }
        [JsonProperty("fbWithBevareges")]
        public bool FbWithBeverages { get; set; }
        [JsonProperty("ro")]
        public bool RoomOnly { get; set; }
        [JsonProperty("scottishBreakfast")]
        public bool ScottishBreakfast { get; set; }
        [JsonProperty("selfCatering")]
        public bool SelfCatering { get; set; }
    }

    public class AreaFilter
    {
        [JsonProperty("areas")]
        public List<string> Areas { get; set; } 
    }

    public class HotelRequestSorting
    {
        [JsonProperty("lowestPrice")]
        public bool LowestPrice { get; set; }
        [JsonProperty("highestStar")]
        public bool HighestStar { get; set; }
        [JsonProperty("highestReviewScore")]
        public bool HighestReviewScore { get; set; }
    }
}
