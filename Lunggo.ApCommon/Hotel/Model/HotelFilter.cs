using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Hotel.Model
{
    public class HotelFilter
    {
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public List<int> StarRating { get; set; }
        public List<string> Area { get; set; }
        public List<string> AccomodationType { get; set; }
        public List<string> Amenities { get; set; }
        public List<string> BoardCode { get; set; }
    }
}
