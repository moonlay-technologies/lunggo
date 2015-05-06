using System;
using System.Collections.Generic;
using System.Globalization;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Object;
using System.Linq;
using Lunggo.ApCommon.Sequence;
using Lunggo.ApCommon.Travolutionary;
using Lunggo.Framework.Config;
using Lunggo.Framework.Redis;

namespace Lunggo.ApCommon.Hotel.Logic.Search
{
    public class HotelsSearchService
    {
        public static HotelsSearchServiceResponse SearchHotel(HotelsSearchServiceRequest request)
        {
            var hotelSearchResult = GetAvailableHotelIdList(request);
            var totalHotelCount = 0;
            var totalFilteredHotelCount = 0;
            IEnumerable<HotelDetail> hotelListToReturn = null;

            if (hotelSearchResult.HotelIdList!=null && hotelSearchResult.HotelIdList.Any())
            {
                var completeHotelList = RetrieveHotelsDetail(hotelSearchResult.HotelIdList);
                var enumeratedCompleteHotelList = completeHotelList as IList<HotelDetail> ?? completeHotelList.ToList();
                var filteredList = FilterList(enumeratedCompleteHotelList, request);
                var enumeratedFilteredList = filteredList as IList<HotelDetail> ?? filteredList.ToList();
                var sortedList = SortList(enumeratedFilteredList, request);
                var pagedList = PageList(sortedList, request);

                totalHotelCount = enumeratedCompleteHotelList.Count();
                totalFilteredHotelCount = enumeratedFilteredList.Count();
                hotelListToReturn = pagedList;
            }
            
            return new HotelsSearchServiceResponse
            {
                HotelList = hotelListToReturn,
                SearchId = hotelSearchResult.SearchId,
                TotalHotelCount = totalHotelCount,
                TotalFilteredHotelCount = totalFilteredHotelCount
            };
        }

        public static HotelDetail GetHotelDetail(int hotelId)
        {
            var hotelDetailOnMem = GetHotelDetailFromCache(hotelId);
            return hotelDetailOnMem != null ? ToHotelDetail(hotelDetailOnMem) : null;
        }

        private static IEnumerable<HotelDetail> PageList(IEnumerable<HotelDetail> hotelList,
            HotelsSearchServiceRequest request)
        {
            return hotelList.Skip(request.StartIndex).Take(request.ResultCount);
        }

        private static IEnumerable<HotelDetail> SortList(IEnumerable<HotelDetail> hotelList,
            HotelsSearchServiceRequest request)
        {
            var comparer = HotelsSortComparer.GetComparer(request.SortBy);
            return hotelList.OrderBy(p => p, comparer);
        }

        private static IEnumerable<HotelDetail> FilterList(IEnumerable<HotelDetail> hotelList, HotelsSearchServiceRequest request)
        {
            var list = FilterByPrice(hotelList, request);
            list = FilterByStarRating(list,request);
            return list;
        }

        private static IEnumerable<HotelDetail> FilterByPrice(IEnumerable<HotelDetail> hotelList, HotelsSearchServiceRequest request)
        {
            var minValue = request.SearchFilter.MinPrice;
            var maxValue = request.SearchFilter.MaxPrice;
            return hotelList.Where(p => p.LowestPrice.Value >= minValue && p.LowestPrice.Value <= maxValue);
        }

        private static IEnumerable<HotelDetail> FilterByStarRating(IEnumerable<HotelDetail> hotelList, HotelsSearchServiceRequest request)
        {
            if (request.SearchFilter.StarRatingsToDisplay == null || !request.SearchFilter.StarRatingsToDisplay.Any())
            {
                return hotelList;
            }
            else
            {
                var starFilter = request.SearchFilter.StarRatingsToDisplay;
                return hotelList.Where(p => starFilter.Contains(p.StarRating));
            }
        }
    
        private static IEnumerable<HotelDetail> RetrieveHotelsDetail(IEnumerable<int> hotelIdList)
        {
            var onMemHotelsDetail = GetHotelsDetail(hotelIdList);
            var hotelDetailList = ToHotelDetailList(onMemHotelsDetail);
            return hotelDetailList;
        }

        private static HotelsSearchResult GetAvailableHotelIdList(HotelsSearchServiceRequest request)
        {
            var needApiCall = false;
            HotelsSearchResult searchResult = null;
            if (!String.IsNullOrEmpty(request.SearchId))
            {
                searchResult = ExecuteSearchUsingCache(request);
                if (searchResult.HotelIdList == null || !searchResult.HotelIdList.Any())
                {
                    needApiCall = true;
                }
            }
            else
            {
                needApiCall = true;
            }

            if (needApiCall)
            {
                searchResult = ExecuteSearchUsingThirdPartyService(request);
            }

            return searchResult;
        }

        private static HotelsSearchResult ExecuteSearchUsingCache(HotelsSearchServiceRequest request)
        {
            return new HotelsSearchResult
            {
                HotelIdList = SearchHotelInCache(request),
                SearchId = request.SearchId
            };
        }

        private static HotelsSearchResult ExecuteSearchUsingThirdPartyService(HotelsSearchServiceRequest request)
        {
            var searchId = GenerateNewHotelSearchId();
            var searchResponse = TravolutionaryHotelService.SearchHotel(request);

            if (searchResponse.HotelIdList != null && searchResponse.HotelIdList.Any())
            {
                SaveSearchResultToCache(searchId.ToString(CultureInfo.InvariantCulture),searchResponse.HotelIdList);
            }

            return new HotelsSearchResult
            {
                SearchId = searchId.ToString(CultureInfo.InvariantCulture),
                HotelIdList = searchResponse.HotelIdList
            };
        }

        private static long GenerateNewHotelSearchId()
        {
            return HotelSearchIdSequence.GetInstance().GetNext();
        }

        private static void SaveSearchResultToCache(String searchId, IEnumerable<int> hotelIdList)
        {
            var redisService = RedisService.GetInstance();
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var hotelCacheObject = HotelCacheUtil.ConvertHotelIdListToHotelCacheObject(hotelIdList);
            redisDb.StringSet(searchId, hotelCacheObject, TimeSpan.FromMinutes(
                Int32.Parse(ConfigManager.GetInstance().GetConfigValue("hotel", "hotelSearchResultCacheTimeout"))));
        }

        private static IEnumerable<int> SearchHotelInCache(HotelsSearchServiceRequest request)
        {
            var redisService = RedisService.GetInstance();
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var rawHotelIdListFromCache =  redisDb.StringGet(request.SearchId);
            if (!rawHotelIdListFromCache.IsNullOrEmpty)
            {
                var hotelIdList = HotelCacheUtil.ConvertHotelCacheObjectToHotelIdList(rawHotelIdListFromCache);
                return hotelIdList;
            }
            else
            {
                return null;
            }
        }

        private static IEnumerable<HotelDetail> ToHotelDetailList(IEnumerable<OnMemHotelDetail> onMemHotelsDetail)
        {
            IEnumerable<HotelDetail> hotelsDetail = null;
            if (onMemHotelsDetail != null)
            {
                //Do not include hotel which doesn't have detail in hotel cache
                hotelsDetail = onMemHotelsDetail.Where(p => p != null).Select(ToHotelDetail);
            }
            return hotelsDetail;
        }

        private static HotelDetail ToHotelDetail(OnMemHotelDetail hotelDetailOnMem)
        {
            var hotelDetail = new HotelDetail
            {
                Address = hotelDetailOnMem.Address,
                Area = hotelDetailOnMem.Area,
                Country = hotelDetailOnMem.Country,
                HotelDescriptions = ChooseDescriptions(hotelDetailOnMem.DescriptionList),
                HotelId = hotelDetailOnMem.HotelId,
                HotelName = hotelDetailOnMem.HotelName,
                IsLatLongSet = hotelDetailOnMem.IsLatLongSet,
                Latitude = hotelDetailOnMem.Latitude,
                Longitude = hotelDetailOnMem.Longitude,
                Province = hotelDetailOnMem.Province,
                StarRating = hotelDetailOnMem.StarRating,
                LowestPrice = GetLowestPrice(hotelDetailOnMem),
                Facilities = GetFacilities(hotelDetailOnMem),
                ImageUrlList = hotelDetailOnMem.ImageUrlList
            };
            return hotelDetail;
        }
        


        private static Lunggo.ApCommon.Model.Price GetLowestPrice(OnMemHotelDetail hotelDetailOnMem)
        {
            //TODO Please replace below dummy logic
            return HotelPriceUtil.CountPrice(null);
        }

        private static IEnumerable<HotelFacility> GetFacilities(OnMemHotelDetail hotelDetailOnMem)
        {
            if (hotelDetailOnMem.FacilityList == null)
            {
                return null;
            }
            else
            {
                return hotelDetailOnMem.FacilityList.Select(p => new HotelFacility
                {
                    FacilityId = p.FacilityId
                });
            }
        }

        private static IEnumerable<HotelDescription> ChooseDescriptions(IEnumerable<OnMemHotelDescription> descriptions)
        {
            if (descriptions == null)
            {
                return null;
            }
            else
            {
                return descriptions.
                Select(p => new HotelDescription
                {
                    Description = p.Description,
                    Line = p.Line
                });
            }
        }

        private static IEnumerable<OnMemHotelDetail> GetHotelsDetail(IEnumerable<int> hotelIdList)
        {
            var idList = hotelIdList as IList<int> ?? hotelIdList.ToList();
            if(hotelIdList != null && idList.Any())
            {
                return GetHotelsDetailFromCache(idList);
            }
            else
            {
                return null;
            }
        }

        private static IEnumerable<OnMemHotelDetail> GetHotelsDetailFromCache(IEnumerable<int> hotelIdList)
        {
            var redisService = RedisService.GetInstance();
            var redisDb = redisService.GetDatabase(ApConstant.MasterDataCacheName);
            var cacheKeys = HotelCacheUtil.GetHotelDetailKeyInCacheArray(hotelIdList.ToArray());
            var rawHotelsDetailFromCache = redisDb.StringGet(cacheKeys);
            var hotelDetail =
                rawHotelsDetailFromCache.Select(HotelCacheUtil.ConvertHotelCacheObjecttoHotelDetail);
            return hotelDetail;
        }

        private static OnMemHotelDetail GetHotelDetailFromCache(int hotelId)
        {
            var redisService = RedisService.GetInstance();
            var redisDb = redisService.GetDatabase(ApConstant.MasterDataCacheName);
            var cacheKey = HotelCacheUtil.GetHotelDetailKeyInCache(hotelId);
            var rawHotelDetailFromCache = redisDb.StringGet(cacheKey);
            OnMemHotelDetail hotelDetailOnMem = null;

            if (!rawHotelDetailFromCache.IsNullOrEmpty)
            {
                hotelDetailOnMem = HotelCacheUtil.ConvertHotelCacheObjecttoHotelDetail(rawHotelDetailFromCache);
            }

            return hotelDetailOnMem;
        }
    }
}
