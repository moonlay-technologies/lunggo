using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Hotel.Model
{
    public class RevalidateHotelResult : ResultBase
    {
        internal bool IsValid { get; set; }
        internal decimal? NewPrice { get; set; }
        internal bool IsPriceChanged { get; set; }
        internal string RateKey { get; set; }
        internal string OrderId { get; set; }
        internal string OrderDetailId { get; set; }
        internal DateTime TimeLimit { get; set; }
    }
}
