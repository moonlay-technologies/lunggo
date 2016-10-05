using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Hotel.Model.Logic
{
    public class BookHotelOutput
    {
        public bool IsValid { get; set; }
        public bool IsPriceChanged { get; set; }
        public decimal? NewPrice { get; set; }
        public string RsvNo { get; set; }
        public DateTime TimeLimit { get; set; }
    }
}
