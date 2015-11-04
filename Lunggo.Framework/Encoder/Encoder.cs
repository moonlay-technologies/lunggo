using System;
using System.Text;

namespace Lunggo.Framework.Encoder
{
    public static class Encoder
    {
        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(this string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        internal static string Hash(this string plain)
        {
            var hash = plain;
            return hash;
        }

        internal static string Unhash(this string hash)
        {
            var plain = hash;
            return plain;
        }
    }
}
