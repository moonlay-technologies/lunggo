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
                var cacheItin = GetItinerariesFromCache(input.SearchId);
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
                TripInfos = input.Conditions.TripInfos.Select(data => new TripInfo
                {
                    OriginAirport = data.OriginAirport,
                    DestinationAirport = data.DestinationAirport,
                    DepartureDate = data.DepartureDate
                }).ToList()
            };

            var result = SearchFlightInternal(conditions);
            if (result.FlightItineraries != null)
                SaveSearchResultToCache(searchId, result.FlightItineraries);
            result.SearchId = searchId;
            return result;
        }

        private static void SaveSearchResultToCache(string searchId, List<FlightItineraryFare> itineraryList)
        {
            var redisService = RedisService.GetInstance();
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var flightCacheObject = FlightCacheUtil.ConvertItineraryListToFlightCacheObject(itineraryList);
            redisDb.StringSet(searchId, flightCacheObject, TimeSpan.FromMinutes(
                Int32.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "flightSearchResultCacheTimeout"))));
        }

        private static List<FlightItineraryFare> GetItinerariesFromCache(string searchId)
        {
            var redisService = RedisService.GetInstance();
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var rawItineraryListFromCache = redisDb.StringGet(searchId);

            if (!rawItineraryListFromCache.IsNullOrEmpty)
            {
                var itineraryList = FlightCacheUtil.ConvertFlightCacheObjectToItineraryList(rawItineraryListFromCache);
                return itineraryList;
            }
            else
            {
                return null;
            }

        }
    }
}
