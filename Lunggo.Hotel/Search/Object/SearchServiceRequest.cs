using System;
using System.Collections.Generic;
using Lunggo.Hotel.Object;

namespace Lunggo.Hotel.Search.Object
{
    public class SearchServiceRequest
    {
        public int CheckInDay { get; set; }
        public int CheckInMonth { get; set; }
        public int CheckInYear { get; set; }
        public int StayLength { get; set; }
        public IEnumerable<RoomOccupation> RoomOccupations { get; set; }
        public SortOrder ActiveSort { get; set; }
        public String CountryCd { get; set; }
        public String CityCd { get; set; }
        public String AreaCd { get; set; }
        public bool DisplayFiveStar { get; set; }
        public bool DisplayFourStar { get; set; }
        public bool DisplayThreeStar { get; set; }
        public bool DisplayTwoStar { get; set; }
        public BedType BedType { get; set; }
        public MealType MealType { get; set; }
    }

    
}
