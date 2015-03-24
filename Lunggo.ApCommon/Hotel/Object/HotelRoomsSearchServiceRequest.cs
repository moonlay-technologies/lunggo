using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Hotel.Model;

namespace Lunggo.ApCommon.Hotel.Object
{
    public class HotelRoomsSearchServiceRequest : HotelSearchServiceRequestBase
    {
        public int HotelId { get; set; }
    }
}
