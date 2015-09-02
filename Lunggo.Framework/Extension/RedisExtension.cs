using Lunggo.Framework.Util;
using StackExchange.Redis;

namespace Lunggo.Framework.Extension
{
    public static class RedisExtensionMethod
    {
        public static RedisValue ToCacheObject<T>(this T input)
        {
            var serializedInput = input.Serialize();
            var compressedInput = CompressionUtil.Compress(serializedInput);
            return compressedInput;
        }

        public static T DeconvertTo<T>(this RedisValue cacheObject)
        {
            if (cacheObject.IsNullOrEmpty)
            {
                return default(T);
            }
            else
            {
                byte[] compressedJson = cacheObject;
                var json = CompressionUtil.Decompress(compressedJson);
                return json.Deserialize<T>();
            }
        }
    }
}
