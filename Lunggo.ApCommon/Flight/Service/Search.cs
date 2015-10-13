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
            var cacheItin = GetSearchedItinerariesFromCache(searchId);
            if (cacheItin == null)
                result = SearchByThirdPartyService(input.Conditions);
            else
            {
                result = new SearchFlightResult
                {
                    IsSuccess = true,
                    Itineraries = cacheItin,
                    SearchId = searchId
                };
            }

            if (result.IsSuccess)
            {
                output.IsSuccess = true;
                output.Itineraries = result.Itineraries.Select(ConvertToItineraryForDisplay).ToList();
                output.Itineraries.ForEach(itin => itin.SequenceNo = output.Itineraries.IndexOf(itin));
                output.SearchId = result.SearchId;
                output.Itineraries.ForEach(itin => itin.SearchId = output.SearchId);
                output.ExpiryTime = GetSearchedItinerariesExpiry(searchId).GetValueOrDefault();
            }
            else
            {
                output.IsSuccess = false;
                output.Errors = result.Errors;
                output.ErrorMessages = result.ErrorMessages;
            }
            return output;
        }

        public void SearchFlightAndFillInSearchCache(string searchId, int timeout)
        {
            var condition = UnhashDecodeConditions(searchId);
            SearchByThirdPartyService(condition, timeout);
        }

        private SearchFlightResult SearchByThirdPartyService(SearchFlightConditions condition, int timeout = 0)
        {
            var conditions = new SearchFlightConditions
            {
                AdultCount = condition.AdultCount,
                ChildCount = condition.ChildCount,
                InfantCount = condition.InfantCount,
                CabinClass = condition.CabinClass,
                Trips = condition.Trips
            };

            var result = SearchFlightInternal(conditions);
            if (result.Itineraries != null)
            {
                var searchId = HashEncodeConditions(condition);
                SaveSearchedItinerariesToCache(result.Itineraries, searchId, timeout);
                result.SearchId = searchId;
            }
            return result;
        }
    }
}
