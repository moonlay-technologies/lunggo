using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Model;

namespace Lunggo.ApCommon.Hotel.Model
{
    public abstract class HotelDetailBase
    {
        public String HotelName { get; set; }
        public String HotelId { get; set; }
        public String Address { get; set; }
        public int StarRating { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsLatLongSet { get; set; }
        public String Country { get; set; }
        public String Province { get; set; }
        public String Area { get; set; }
        public IEnumerable<HotelImage> ImageUrlList { get; set; }
    }

    public class OnMemHotelDetail : HotelDetailBase
    {
        public IEnumerable<OnMemHotelDescription> DescriptionList { get; set; }
        public IEnumerable<OnMemHotelFacility> FacilityList { get; set; } 
    }

    public class HotelDetails : HotelDetailBase
    {
        public IEnumerable<HotelDescription> HotelDescriptions { get; set; }
        public Price LowestPrice { get; set; }
        public IEnumerable<HotelFacility> Facilities { get; set; }
        
    }

    public class HotelExcerpt : HotelDetailBase
    {
        public Price LowestPrice { get; set; }
        public IEnumerable<HotelFacility> Facilities { get; set; }
    }

    


}
