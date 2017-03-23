using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Product.Model;

namespace Lunggo.ApCommon.Hotel.Model.Logic
{
    public class BookHotelInput
    {
        public string Token { get; set; }
        public List<Pax> Passengers { get; set; }
        public Contact Contact { get; set; }
        public string SpecialRequest { get; set; }
        public string BookerMessageTitle { get; set; }
        public string BookerMessageDescription { get; set; }
        public bool IsBookingNoteNew { get; set; }
        public string UserId { get; set; }
    }
}
