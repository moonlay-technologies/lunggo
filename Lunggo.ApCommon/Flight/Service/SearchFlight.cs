using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Utility;
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
            if (input.SearchId == null)
            {
                result = SearchByThirdPartyService(input);
            }
            else
            {
                var cacheItin = GetSearchResultFromCache(input.SearchId);
                if (cacheItin == null)
                    result = SearchByThirdPartyService(input);
                else
                {
                    result = new SearchFlightResult
                    {
                        IsSuccess = true,
                        FlightItineraries = cacheItin,
                        SearchId = input.SearchId
                    };
                }
            }

            if (result.IsSuccess)
            {
                output.IsSuccess = true;
                output.Itineraries = result.FlightItineraries;
                output.SearchId = result.SearchId;
            }
            else
            {
                output.IsSuccess = false;
                output.Errors = result.Errors;
                output.ErrorMessages = result.ErrorMessages;
            }
            return output;
        }

        private SearchFlightResult SearchByThirdPartyService(SearchFlightInput input)
        {
            var searchId = input.SearchId ??
                       FlightSearchIdSequence.GetInstance().GetNext().ToString(CultureInfo.InvariantCulture);
            var conditions = new SearchFlightConditions
            {
                AdultCount = input.Conditions.AdultCount,
                ChildCount = input.Conditions.ChildCount,
                InfantCount = input.Conditions.InfantCount,
                CabinClass = input.Conditions.CabinClass,
                TripInfos = input.Conditions.TripInfos
            };

            var result = SearchFlightInternal(conditions);
            if (result.FlightItineraries != null)
                SaveSearchResultToCache(searchId, result.FlightItineraries);
            result.SearchId = searchId;
            return result;
        }
    }
}
