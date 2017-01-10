using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Hotel.Model.Logic
{
    public class AvailableRatesInput
    {
        public int HotelCode { get; set; }
        public int Nights { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime Checkout { get; set; }
        public List<Occupancy> Occupancies { get; set; }
    }
}
