using Lunggo.ApCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Flight.Model
{
    public class TopDestination
    {
        public string OriginCity { get; set; }
        public string DestinationCity { get; set; }
        public Price CheapestPrice { get; set; }
    }
    public class TopDestinations
    {
        public List<TopDestination> TopDestinationList { get; set; }
    }
}
