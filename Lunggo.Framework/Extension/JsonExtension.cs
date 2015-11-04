using Newtonsoft.Json;

namespace Lunggo.Framework.Extension
{
    public static class JsonExtension
    {
        public static string Serialize<T>(this T input)
        {
            return JsonConvert.SerializeObject(input);
        }

        public static T Deserialize<T>(this string jsonInput)
        {
            return JsonConvert.DeserializeObject<T>(jsonInput);
        }
    }
}
