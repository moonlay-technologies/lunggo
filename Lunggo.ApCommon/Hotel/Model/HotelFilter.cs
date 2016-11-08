using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Hotel.Model
{

    public class HotelFilter
    {
        [JsonProperty("priceFilter")]
        public PriceFilter PriceFilter { get; set; }
        [JsonProperty("starFilter")]
        public StarFilter StarFilter { get; set; }
        [JsonProperty("accommodationTypeFilter")]
        public AccommodationTypeFilter AccommodationTypeFilter { get; set; }
        [JsonProperty("facilityFilter")]
        public FacilityFilter FacilityFilter { get; set; }
        [JsonProperty("boardFilter")]
        public BoardFilter BoardFilter { get; set; }
        [JsonProperty("areaFilter")]
        public AreaFilter AreaFilter { get; set; }
        [JsonProperty("zoneFilter")]
        public ZoneFilter ZoneFilter { get; set; }
    }

    public class PriceFilter
    {
        [JsonProperty("minPrice")]
        public decimal? MinPrice { get; set; }
        [JsonProperty("maxPrice")]
        public decimal? MaxPrice { get; set; }
    }

    //public class StarFilter
    //{
    //    [JsonProperty("oneStar")]
    //    public bool OneStar { get; set; }
    //    [JsonProperty("twoStar")]
    //    public bool TwoStar { get; set; }
    //    [JsonProperty("threeStar")]
    //    public bool ThreeStar { get; set; }
    //    [JsonProperty("fourStar")]
    //    public bool FourStar { get; set; }
    //    [JsonProperty("fiveStar")]
    //    public bool FiveStar { get; set; }
    //}

    public class AccommodationTypeFilter
    {
        [JsonProperty("accomodations")]
        public List<string> Accomodations { get; set; }
    }

    public class FacilityFilter
    {
        [JsonProperty("facilities")]
        public List<string> Facilities { get; set; } 
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

    public class ZoneFilter
    {
        [JsonProperty("zones")]
        public List<string> Zones { get; set; } 
    }

    public class StarFilter
    {
        [JsonProperty("stars")]
        public List<int> Stars { get; set; }
    }
}
