using System;

namespace Lunggo.Hotel.Search.Object
{
    public class SearchServiceResponse
    {
        public String RoomCd { get; set; }
        public String RoomName { get; set; }
        public Decimal NetPrice { get; set; }
        public Decimal PriceMarkup { get; set; }

    }
}
