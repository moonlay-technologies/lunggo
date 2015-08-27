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
                var cacheItin = GetSearchedItinerariesFromCache(input.SearchId);
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
                output.Itineraries = result.FlightItineraries.Select(ConvertToItineraryApi).ToList();
                output.SearchId = result.SearchId;
                output.ExpiryTime = GetSearchedItinerariesExpiry(input.SearchId);
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
            var conditions = new SearchFlightConditions
            {
                AdultCount = input.Conditions.AdultCount,
                ChildCount = input.Conditions.ChildCount,
                InfantCount = input.Conditions.InfantCount,
                CabinClass = input.Conditions.CabinClass,
                Trips = input.Conditions.Trips
            };

            var result = SearchFlightInternal(conditions);
            if (result.FlightItineraries != null)
                result.SearchId = SaveSearchedItinerariesToCache(result.FlightItineraries);
            return result;
        }
    }
}
