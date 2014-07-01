using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
