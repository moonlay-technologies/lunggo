using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Hotel.Model
{
    public class RevalidateHotelResult
    {
        internal bool IsValid { get; set; }
        internal decimal? NewPrice { get; set; }
        internal bool IsPriceChanged { get; set; }
        internal string RateKey { get; set; }
        internal string OrderId { get; set; }
        internal DateTime TimeLimit { get; set; }
    }
}
