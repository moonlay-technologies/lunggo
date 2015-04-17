using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Hotel.Object
{
    public abstract class HotelRequestBase
    {
        public int LocationId { get; set; }
        public String StayDate { get; set; }
        public String StayLength { get; set; }
        public int RoomCount { get; set; }
        public String SearchId { get; set; }
        public String Lang { get; set; }
    }

    public abstract class HotelSearchRequestBase : HotelRequestBase
    {
        public String SortBy { get; set; }
        public String StartIndex { get; set; }
        public String ResultCount { get; set; }
        public String MinPrice { get; set; }
        public String MaxPrice { get; set; }
        public String StarRating { get; set; }
    }

    public abstract class HotelRoomsSearchRequestBase : HotelRequestBase
    {
        public String HotelId { get; set; }
    }



}
