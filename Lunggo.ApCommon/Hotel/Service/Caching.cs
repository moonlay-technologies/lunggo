using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Model;
using Lunggo.Framework.Config;
using Lunggo.Framework.Extension;
using Lunggo.Framework.Redis;
using StackExchange.Redis;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {
        public void SaveAllCurrencyToCache(string searchId, Dictionary<string, Currency> currencies)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "searchId:" + searchId;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var redisValue =  currencies.Serialize();
            //var i = 1;

            for (var i = 0; i < ApConstant.RedisMaxRetry; i++)
            {
                try
                {
                    redisDb.StringSet(redisKey, redisValue, TimeSpan.FromMinutes(30));
                    return;
                }       
                catch
                {

                }
            }           
        }


        public Dictionary<string, Currency> GetAllCurrenciesFromCache(string searchId)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "searchId:" + searchId;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var cacheObject = "";
            for (var i = 0; i < ApConstant.RedisMaxRetry; i++)
            {
                try
                {
                    cacheObject = (string)redisDb.StringGet(redisKey);
                    break;
                }
                catch
                {

                }
            }

            if (cacheObject == "")
            {
                return new Dictionary<string, Currency>();
            }
            var currencies = cacheObject.Deserialize<Dictionary<string, Currency>>();
            return currencies;
        }

        public void SaveAvailableRateToCache(string token, List<HotelRoom> rooms)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "HotelAvailableRates:" + token;
            var timeout = int.Parse(ConfigManager.GetInstance().GetConfigValue("hotel", "hotelSearchResultCacheTimeout")); //TODO Change this
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var redisValue = rooms.ToCacheObject();
            for (var i = 0; i < ApConstant.RedisMaxRetry; i++)
            {
                try
                {
                    redisDb.StringSet(redisKey, redisValue, TimeSpan.FromMinutes(timeout));
                    return;
                }
                catch
                {

                }
            }

        }

        public DateTime? GetAvailableRatesExpiry(string token)
        {
            for (var i = 0; i < ApConstant.RedisMaxRetry; i++)
            {
                try
                {
                    var redisService = RedisService.GetInstance();
                    var redisKey = "HotelAvailableRates:" + token;
                    var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
                    var timeToLive = redisDb.KeyTimeToLive(redisKey).GetValueOrDefault();
                    var expiryTime = DateTime.UtcNow + timeToLive;
                    return expiryTime;
                }
                catch
                {
                }
            }
            return DateTime.UtcNow;
        }

        public List<HotelRoom> GetAvailableRatesFromCache(string token)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "HotelAvailableRates:" + token;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var cacheObject = new RedisValue();
            for (var i = 0; i < ApConstant.RedisMaxRetry; i++)
            {
                try
                {
                    cacheObject = redisDb.StringGet(redisKey);
                    var availableRates = cacheObject.DeconvertTo<List<HotelRoom>>();
                    return availableRates;
                }
                catch
                {

                }
            }
            return new List<HotelRoom>();

        }
        public void SaveSelectedHotelDetailsToCache(string token, HotelDetailsBase hotel)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "hotelToken:" + token;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var timeout = int.Parse(ConfigManager.GetInstance().GetConfigValue("hotel", "selectCacheTimeOut"));
            var redisValue = hotel.Serialize();
            for (var i = 0; i < ApConstant.RedisMaxRetry; i++)
            {
                try
                {
                    redisDb.StringSet(redisKey, redisValue, TimeSpan.FromMinutes(timeout));
                    return;
                }
                catch
                {

                }
            }
             //, expiry - timeNow
        }

        public void SaveSearchResultintoDatabaseToCache(string token, SearchHotelResult searchResult)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "HotelSearchResult:" + token;
            var timeout = int.Parse(ConfigManager.GetInstance().GetConfigValue("hotel", "hotelSearchResultCacheTimeout"));
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var redisValue = searchResult.ToCacheObject();
            for (var i = 0; i < ApConstant.RedisMaxRetry; i++)
            {
                try
                {
                    redisDb.StringSet(redisKey, redisValue, TimeSpan.FromMinutes(timeout));
                    return;
                }
                catch
                {

                }
            }
           
        }

        public SearchHotelResult GetSearchHotelResultFromCache(string token)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "HotelSearchResult:" + token;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var cacheObject = new RedisValue();
            for (var i = 0; i < ApConstant.RedisMaxRetry; i++)
            {
                try
                {
                    cacheObject = redisDb.StringGet(redisKey);
                    var searchResult = cacheObject.DeconvertTo<SearchHotelResult>();
                    return searchResult;
                }
                catch
                {

                }
            }
            return new SearchHotelResult();
            
        }

        public DateTime? GetSearchHotelResultExpiry(string token)
        {
            for (var i = 0; i < ApConstant.RedisMaxRetry; i++)
            {
                try
                {
                    var redisService = RedisService.GetInstance();
                    var redisKey = "HotelSearchResult:" + token;
                    var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
                    var timeToLive = redisDb.KeyTimeToLive(redisKey).GetValueOrDefault();
                    var expiryTime = DateTime.UtcNow + timeToLive;
                    return expiryTime;
                }
                catch
                {
                }
            }
            return DateTime.UtcNow;
        }
        
        public HotelDetailsBase GetSelectedHotelDetailsFromCache(string token)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "hotelToken:" + token;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var cacheObject = "";
            for (var i = 0; i < ApConstant.RedisMaxRetry; i++)
            {
                try
                {
                    cacheObject = (string)redisDb.StringGet(redisKey);
                    break;
                }
                catch
                {

                }
            }
            if (cacheObject == "")
            {
                return new HotelDetailsBase();
            }
            var hotelDetails = cacheObject.Deserialize<HotelDetailsBase>();
            return hotelDetails;
        }

        public DateTime? GetSelectionExpiry(string token)
        {
                var redisService = RedisService.GetInstance();
                var redisKey = "hotelToken:" + token;
                var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var cacheObject = "";
            for (var i = 0; i < ApConstant.RedisMaxRetry; i++)
            {
                try
                {
                    cacheObject = (string)redisDb.StringGet(redisKey);
                    break;
            }
            catch
            {

                }
            }

            if (cacheObject == "")
            {
                return new DateTime();
            }
            var timeToLive = redisDb.KeyTimeToLive(redisKey).GetValueOrDefault();
            var expiryTime = DateTime.UtcNow + timeToLive;
            return expiryTime;
            //}
            //catch
            //{
            //    return DateTime.UtcNow;
            //}
        }

        public DateTime? GetSearchedHotelDetailsExpiry(string searchId)
        {
            for (var i = 0; i < ApConstant.RedisMaxRetry; i++)
            {
            try
            {
                var redisService = RedisService.GetInstance();
                var redisKey = "searchedHotelItineraries:0:" + searchId + ":";
                var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
                var timeToLive = redisDb.KeyTimeToLive(redisKey);
                var expiryTime = DateTime.UtcNow + timeToLive;
                return expiryTime;
            }
            catch
            {
                   
            }
            }
                return null;
            }

        private void SaveActiveMarginRulesToCache(List<HotelMarginRule> marginRules)
        {
            var redisService = RedisService.GetInstance();
            var redisMarginsKey = "activeHotelMargins";
            var redisRulesKey = "activeHotelMarginRules";
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var marginsCacheObject = marginRules.Select(mr => mr.Margin).ToCacheObject();
            var rulesCacheObject = marginRules.Select(mr => mr.Rule).ToCacheObject();
            for (var i = 0; i < ApConstant.RedisMaxRetry; i++)
            {
                try
                {
            redisDb.StringSet(redisMarginsKey, marginsCacheObject);
            redisDb.StringSet(redisRulesKey, rulesCacheObject);
                    return;
                }
                catch
                {
                    
                }
            }
            
        }

        private void SaveActiveMarginRulesInBufferCache(List<HotelMarginRule> marginRules)
        {
            var redisService = RedisService.GetInstance();
            var marginsRedisKey = "activeHotelMarginsBuffer";
            var rulesRedisKey = "activeHotelMarginRulesBuffer";
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var marginsCacheObject = marginRules.Select(mr => mr.Margin).ToCacheObject();
            var rulesCacheObject = marginRules.Select(mr => mr.Rule).ToCacheObject();
            for (var i = 0; i < ApConstant.RedisMaxRetry; i++)
            {
                try
                {
            redisDb.StringSet(marginsRedisKey, marginsCacheObject);
            redisDb.StringSet(rulesRedisKey, rulesCacheObject);
                    return;
                }
                catch
                {

                }
            }
        }

        private void SaveDeletedMarginRulesInBufferCache(List<HotelMarginRule> deletedMarginRules)
        {
            var redisService = RedisService.GetInstance();
            var deletedMarginsRedisKey = "deletedHotelMarginsBuffer";
            var deletedRulesRedisKey = "deletedHotelMarginRulesBuffer";
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var deletedMarginsCacheObject = deletedMarginRules.Select(mr => mr.Margin).ToCacheObject();
            var deletedRulesCacheObject = deletedMarginRules.Select(mr => mr.Rule).ToCacheObject();
            for (var i = 0; i < ApConstant.RedisMaxRetry; i++)
            {
                try
                {
            redisDb.StringSet(deletedMarginsRedisKey, deletedMarginsCacheObject);
            redisDb.StringSet(deletedRulesRedisKey, deletedRulesCacheObject);
                    return;
                }
                catch
                {

                }
            }
        }

        private List<HotelMarginRule> GetAllActiveMarginRulesFromCache()
        {
            for (var i = 0; i < ApConstant.RedisMaxRetry; i++)
            {
            try
            {
                var redisService = RedisService.GetInstance();
                var redisMarginKey = "activeHotelMargins";
                var redisRuleKey = "activeHotelMarginRules";
                var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
                var marginCacheObject = redisDb.StringGet(redisMarginKey);
                var margins = marginCacheObject.DeconvertTo<List<Margin>>();
                var ruleCacheObject = redisDb.StringGet(redisRuleKey);
                var rules = ruleCacheObject.DeconvertTo<List<HotelRateRule>>();
                var marginRules = margins.Zip(rules, (margin, rule) => new HotelMarginRule(margin, rule)).ToList();
                return marginRules;
            }
            catch
            {
                    
                }  
            }
                return new List<HotelMarginRule>();
            }

        private List<HotelMarginRule> GetActiveMarginRulesFromBufferCache()
        {
            for (var i = 0; i < ApConstant.RedisMaxRetry; i++)
            {
            try
            {
                var redisService = RedisService.GetInstance();
                var redisMarginsKey = "activeHotelMarginsBuffer";
                var redisRulesKey = "activeHotelMarginRulesBuffer";
                var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
                var marginsCacheObject = redisDb.StringGet(redisMarginsKey);
                var rulesCacheObject = redisDb.StringGet(redisRulesKey);
                var margins = marginsCacheObject.DeconvertTo<List<Margin>>();
                var rules = rulesCacheObject.DeconvertTo<List<HotelRateRule>>();
                var marginRules = margins.Zip(rules, (margin, rule) => new HotelMarginRule(margin, rule)).ToList();
                return marginRules;
            }
            catch
            {
                    
                }
            }
                return new List<HotelMarginRule>();
            }

        private List<HotelMarginRule> GetDeletedMarginRulesFromBufferCache()
        {
            for (var i = 0; i < ApConstant.RedisMaxRetry; i++)
            {
            try
            {
                var redisService = RedisService.GetInstance();
                var redisMarginsKey = "deletedHotelMarginsBuffer";
                var redisRulesKey = "deletedHotelMarginRulesBuffer";
                var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
                var marginCacheObject = redisDb.StringGet(redisMarginsKey);
                var rulesCacheObject = redisDb.StringGet(redisRulesKey);
                var margins = marginCacheObject.DeconvertTo<List<Margin>>();
                var rules = rulesCacheObject.DeconvertTo<List<HotelRateRule>>();
                var marginRules = margins.Zip(rules, (margin, rule) => new HotelMarginRule(margin, rule)).ToList();
                return marginRules;
            }
            catch
            {
                    
                }
            }
                return new List<HotelMarginRule>();
        }

        public List<decimal> GetLowestPricesForRangeOfDate(string location, DateTime startingTime,
            DateTime endTime)
        {
            //var keyRoute = SetRoute(origin, destination);
            var listofDates = new List<string>();
            for (var date = startingTime.Date; date <= endTime; date = date.AddDays(1))
            {
                listofDates.Add(date.ToString("ddMMyy", CultureInfo.InvariantCulture));
            }
            var redis = RedisService.GetInstance();
            var redisDb = redis.GetDatabase(ApConstant.SearchResultCacheName);
            for (var i = 0; i < ApConstant.RedisMaxRetry; i++)
            {
                try
                {
                    var values = redisDb.HashGet(location, Array.ConvertAll(listofDates.ToArray(), item => (RedisValue)item)).ToList();
                    return values.Select(value => Convert.ToDecimal(value)).ToList();
                }
                catch { }
            }
            return new List<decimal>();

        }

        public LowestPrice GetLowestPriceInRangeOfDate(string location, DateTime startDate,
            DateTime endDate)
        {
            var listofDates = new List<string>();
            for (var date = startDate.Date; date <= endDate; date = date.AddDays(1))
            {
                listofDates.Add(date.ToString("ddMMyy", CultureInfo.InvariantCulture));
            }
            var redis = RedisService.GetInstance();
            var redisDb = redis.GetDatabase(ApConstant.SearchResultCacheName);
            var values = new List<RedisValue>();
            for (var i = 0; i < ApConstant.RedisMaxRetry; i++)
            {
                try
                {
                    values = redisDb.HashGet(location, Array.ConvertAll(listofDates.ToArray(), item => (RedisValue)item)).ToList();
                    break;
                }
                catch { }
            }

            if (values.Count == 0)
            {
                return new LowestPrice();
            }

            var listOfPrices = values.Select(value => Convert.ToDecimal(value)).ToList();
            var minPrice = listOfPrices.ElementAt(0);
            var minDate = listofDates.ElementAt(0);
            for (var ind = 1; ind < listOfPrices.Count; ind++)
            {
                if (listOfPrices.ElementAt(ind) >= minPrice) continue;
                minPrice = listOfPrices.ElementAt(ind);
                minDate = listofDates.ElementAt(ind);
            }

            return new LowestPrice
            {
                CheapestDate = minDate,
                CheapestPrice = minPrice
            };
        }

        public void SetLowestPriceToCache(List<HotelDetailForDisplay> hotelRsv, string locationCd)
        {
            
            var lowestvalue = GetLowestPrice(hotelRsv);
            var redis = RedisService.GetInstance();
            var redisDb = redis.GetDatabase(ApConstant.SearchResultCacheName);
            for (var i = 0; i < ApConstant.RedisMaxRetry; i++)
            {
                try
                {
                    foreach (DateTime day in EachDay(hotelRsv[0].CheckInDate, hotelRsv[0].CheckOutDate))
                    {
                        var keyDate = SetDate(day);
                        redisDb.HashSet(locationCd, keyDate, Convert.ToString(lowestvalue));
                        Console.WriteLine("Lowest value for location: " + locationCd + keyDate + " is " + lowestvalue);
                    }
                    break;
                }
                catch { }
            }
        }

        public void SetLowestPriceToCache(string checkInDate, string locationCd, decimal minPrice)
        {
            var redis = RedisService.GetInstance();
            var redisDb = redis.GetDatabase(ApConstant.SearchResultCacheName);
            for (var i = 0; i < ApConstant.RedisMaxRetry; i++)
            {
                try
                {
                    redisDb.HashSet(locationCd, checkInDate, Convert.ToString(minPrice));
                    Console.WriteLine("Lowest value for location: " + locationCd + checkInDate + " is " + minPrice);
                    
                    break;
                }
                catch { }
            }
        }

        public void SetLowestPriceToCache(DateTime checkinDate, int nights, string locationCd, decimal lowestvalue)
        {
            var redis = RedisService.GetInstance();
            var redisDb = redis.GetDatabase(ApConstant.SearchResultCacheName);
            for (var i = 0; i < ApConstant.RedisMaxRetry; i++)
            {
                try
                {
                    for(var j = 0; j < nights; j++)
                    {
                        var day = checkinDate.AddDays(j);
                        var keyDate = SetDate(day);
                        redisDb.HashSet(locationCd, keyDate, Convert.ToString(lowestvalue));
                        Console.WriteLine("Lowest value for location: " + locationCd + keyDate + " is " + lowestvalue);
                    }
                    break;
                }
                catch { }
            }
        }

        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date < thru.Date; day = day.AddDays(1))
                yield return day;
        }
    }
}
