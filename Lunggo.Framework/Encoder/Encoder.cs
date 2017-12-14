using System;
using System.Security.Cryptography;
using System.Text;

namespace Lunggo.Framework.Encoder
{
    public static class Encoder
    {
        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return plainTextBytes.Base64Encode();
        }

        public static string Base64Encode(this byte[] byteArray)
        {
            return Convert.ToBase64String(byteArray);
        }

        public static string Base64Decode(this string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string Sha512Encode(this string input)
        {
            var data = Encoding.UTF8.GetBytes(input);
            using (SHA512 shaM = new SHA512Managed())
            {
                var hash = shaM.ComputeHash(data);
                var stringBuilder = new StringBuilder();
                foreach (var hashByte in hash)
                {
                    stringBuilder.AppendFormat("{0:x2}", hashByte);
                }
                return stringBuilder.ToString();
            }
        }

        public static byte[] Sha1Encode(this string input)
        {
            var data = Encoding.UTF8.GetBytes(input);
            using (SHA1 shaM = new SHA1Managed())
            {
                var hash = shaM.ComputeHash(data);
                //var stringBuilder = new StringBuilder();
                //foreach (var hashByte in hash)
                //{
                //    stringBuilder.AppendFormat("{0:x2}", hashByte);
                //}
                //return stringBuilder.ToString();
                return hash;
            }
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
