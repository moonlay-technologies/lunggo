using System;
using System.Text;
using System.Security.Cryptography;
using Lunggo.Framework.Encoder;

namespace Lunggo.CustomerWeb.Common
{
    public abstract class GeneratorIdRsvNo
    {
        public string GenerateId(string key)
        {
            SHA1 sha1 = SHA1.Create();
            string result = "";

            var x = key.Base64Encode();

            var getByte = Encoding.Default.GetBytes(x);
            result = sha1.ComputeHash(getByte).ToString();
            
            return result;
        }
    }
}
