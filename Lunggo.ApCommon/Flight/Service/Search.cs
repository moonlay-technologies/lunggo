using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;

using Lunggo.ApCommon.Sequence;
using Lunggo.Framework.Config;
using Lunggo.Framework.Redis;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public SearchFlightOutput SearchFlight(SearchFlightInput input)
        {
            SearchFlightResult result;
            var output = new SearchFlightOutput();
            var searchId = HashEncodeConditions(input.Conditions);

            var searchTuple = GetSearchedItinerariesFromCache(searchId, input.Completeness);
            var searchCompleteness = searchTuple.Item1;
            var searchedItins = searchTuple.Item2;

            if (searchCompleteness == 0)
                SearchFlightInternal(input.Conditions);

            output.IsSuccess = true;
            output.Itineraries = searchedItins.Select(ConvertToItineraryForDisplay).ToList();
            output.SearchId = searchId;
            output.Itineraries.ForEach(itin => itin.SearchId = output.SearchId);
            output.ExpiryTime = GetSearchedItinerariesExpiry(searchId).GetValueOrDefault();

            return output;
        }
    }
}
