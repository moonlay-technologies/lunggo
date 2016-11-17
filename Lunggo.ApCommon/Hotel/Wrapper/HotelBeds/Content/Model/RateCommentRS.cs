using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Content.Model
{
    public class RateCommentRS
    {
        public List<string> providerDetails { get; set; }
        public List<RateCommentContent> rateComments { get; set; }
        public int total { get; set; }
    }
}
