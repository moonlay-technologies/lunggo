using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Product.Model;
using Lunggo.Framework.Extension;
using Lunggo.Framework.Redis;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {
        public void SaveSelectedHotelDetailsToCache(string token, HotelDetailsBase hotel)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "token:" + token;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var timeNow = DateTime.UtcNow;
            var expiry = timeNow.AddHours(1);
            var redisValue = "hoteldetails:" + hotel.Serialize(); 
            redisDb.StringSet(redisKey, redisValue, expiry - timeNow);
        }

        public void SaveSearchResultintoDatabaseToCache(string token, SearchHotelResult searchResult)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "HotelSearchResult:" + token;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var redisValue = searchResult.ToCacheObject();
            redisDb.StringSet(redisKey, redisValue, TimeSpan.FromMinutes(60));
        }

        public SearchHotelResult GetSearchHotelResultFromCache(string token)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "HotelSearchResult:" + token;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var cacheObject = redisDb.StringGet(redisKey);
            var searchResult = cacheObject.DeconvertTo<SearchHotelResult>();
            return searchResult;
        }

        public HotelDetailsBase GetSelectedHotelDetailsFromCache(string token)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "token:" + token;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var cacheObject = (string) redisDb.StringGet(redisKey);
            var hotelDetails = cacheObject.Substring(13).Deserialize<HotelDetailsBase>();
            return hotelDetails;
        }

        public DateTime? GetSearchedHotelDetailsExpiry(string searchId)
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
                return null;
            }
        }

        private void SaveActiveMarginRulesToCache(List<HotelMarginRule> marginRules)
        {
            var redisService = RedisService.GetInstance();
            var redisMarginsKey = "activeHotelMargins";
            var redisRulesKey = "activeHotelMarginRules";
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var marginsCacheObject = marginRules.Select(mr => mr.Margin).ToCacheObject();
            var rulesCacheObject = marginRules.Select(mr => mr.Rule).ToCacheObject();
            redisDb.StringSet(redisMarginsKey, marginsCacheObject);
            redisDb.StringSet(redisRulesKey, rulesCacheObject);
        }

        private void SaveActiveMarginRulesInBufferCache(List<HotelMarginRule> marginRules)
        {
            var redisService = RedisService.GetInstance();
            var marginsRedisKey = "activeHotelMarginsBuffer";
            var rulesRedisKey = "activeHotelMarginRulesBuffer";
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var marginsCacheObject = marginRules.Select(mr => mr.Margin).ToCacheObject();
            var rulesCacheObject = marginRules.Select(mr => mr.Rule).ToCacheObject();
            redisDb.StringSet(marginsRedisKey, marginsCacheObject);
            redisDb.StringSet(rulesRedisKey, rulesCacheObject);
        }

        private void SaveDeletedMarginRulesInBufferCache(List<HotelMarginRule> deletedMarginRules)
        {
            var redisService = RedisService.GetInstance();
            var deletedMarginsRedisKey = "deletedHotelMarginsBuffer";
            var deletedRulesRedisKey = "deletedHotelMarginRulesBuffer";
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var deletedMarginsCacheObject = deletedMarginRules.Select(mr => mr.Margin).ToCacheObject();
            var deletedRulesCacheObject = deletedMarginRules.Select(mr => mr.Rule).ToCacheObject();
            redisDb.StringSet(deletedMarginsRedisKey, deletedMarginsCacheObject);
            redisDb.StringSet(deletedRulesRedisKey, deletedRulesCacheObject);
        }

        private List<HotelMarginRule> GetAllActiveMarginRulesFromCache()
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
                return new List<HotelMarginRule>();
            }
        }

        private List<HotelMarginRule> GetActiveMarginRulesFromBufferCache()
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
                return new List<HotelMarginRule>();
            }
        }

        private List<HotelMarginRule> GetDeletedMarginRulesFromBufferCache()
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
                return new List<HotelMarginRule>();
            }
        }
    }
}
