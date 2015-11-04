using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.Framework.Util;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Lunggo.ApCommon.Hotel.Logic
{
    public class HotelCacheUtil
    {
        public static RedisKey GetHotelDetailKeyInCache(String hotelId)
        {
            return "hoteldetail:" + hotelId;
        }

        public static RedisKey GetHotelDetailKeyInCache(int hotelId)
        {
            return GetHotelDetailKeyInCache(hotelId.ToString(CultureInfo.InvariantCulture));
        }

        public static RedisKey[] GetHotelDetailKeyInCacheArray(int[] hotelIdList)
        {
            return hotelIdList.Select(GetHotelDetailKeyInCache).ToArray();
        }

        public static RedisValue ConvertHotelDetailToHotelCacheObject(OnMemHotelDetail hotelDetail)
        {
            var hotelJson = SerializeHotelDetail(hotelDetail);
            var hotelJsonCompressed = CompressionUtil.Compress(hotelJson);
            return hotelJsonCompressed;
        }

        public static RedisValue ConvertHotelIdListToHotelCacheObject(IEnumerable<int> hotelIdList)
        {
            var hotelIdListJson = SerializeHotelIdList(hotelIdList);
            var hotelIdListJsonCompressed = CompressionUtil.Compress(hotelIdListJson);
            return hotelIdListJsonCompressed;
        }

        public static IEnumerable<int> ConvertHotelCacheObjectToHotelIdList(RedisValue hotelCacheObject)
        {
            if (hotelCacheObject.IsNullOrEmpty)
            {
                return null;
            }
            else
            {
                byte[] hotelIdListJsonCompressed = hotelCacheObject;
                var hotelIdListJson = CompressionUtil.Decompress(hotelIdListJsonCompressed);
                return DeserializeHotelIdList(hotelIdListJson);
            }
        }

        public static OnMemHotelDetail ConvertHotelCacheObjecttoHotelDetail(RedisValue hotelCacheObject)
        {
            if (hotelCacheObject.IsNullOrEmpty)
            {
                return null;
            }
            else
            {
                byte[] hotelJsonCompressed = hotelCacheObject;
                var hoteljson = CompressionUtil.Decompress(hotelJsonCompressed);
                return DeserializeHotelDetail(hoteljson);
            }
        }

        private static String SerializeHotelDetail(OnMemHotelDetail hotelDetail)
        {
            return JsonConvert.SerializeObject(hotelDetail);
        }

        private static OnMemHotelDetail DeserializeHotelDetail(String hotelDetailJsoned)
        {
            return JsonConvert.DeserializeObject<OnMemHotelDetail>(hotelDetailJsoned);
        }

        private static String SerializeHotelIdList(IEnumerable<int> hotelIdList)
        {
            return JsonConvert.SerializeObject(hotelIdList);
        }

        private static IEnumerable<int> DeserializeHotelIdList(String hotelIdListJsoned)
        {
            return JsonConvert.DeserializeObject<IEnumerable<int>>(hotelIdListJsoned);
        }
    }
}
