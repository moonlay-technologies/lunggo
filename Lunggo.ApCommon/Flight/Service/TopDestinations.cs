using System.Collections.Generic;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Config;
using Lunggo.Framework.Redis;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public List<TopDestination> GetTopDestination()
        {
            return GetTopDestinationsFromCache();
        }
        
    }
}
