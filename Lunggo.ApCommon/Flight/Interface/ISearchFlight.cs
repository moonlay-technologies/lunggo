using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Model;

namespace Lunggo.ApCommon.Flight.Interface
{
    public interface ISearchFlight
    {
        SearchFlightResult SearchFlight(SearchFlightConditions conditions);
    }
}
