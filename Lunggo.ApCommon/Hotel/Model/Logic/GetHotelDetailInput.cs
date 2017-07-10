using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Hotel.Model.Logic
{
    public class GetHotelDetailInput
    {
        public string SearchId { get; set; }
        public int HotelCode { get; set; }
        public string HotelName { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int Nights { get; set; }
        public int Rooms { get; set; }
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
    }
}
