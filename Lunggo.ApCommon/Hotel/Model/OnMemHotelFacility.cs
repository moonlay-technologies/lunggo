

using System;

namespace Lunggo.ApCommon.Hotel.Model
{
    public abstract class HotelFacilityBase
    {
        public int FacilityId { get; set; }
        
    }

    public class OnMemHotelFacility : HotelFacilityBase
    {

    }

    public class HotelFacility : HotelFacilityBase
    {
        public String FacilityName { get; set; }
    }
}
