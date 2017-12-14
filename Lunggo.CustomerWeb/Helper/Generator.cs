using System.Security.Cryptography;
using System.Text;
using Lunggo.Framework.Encoder;

namespace Lunggo.CustomerWeb.Helper
{
    public static class Generator
    {
        public static string GenerateRsvNoId(string rsvNo)
        {
            var sha1 = rsvNo.Sha1Encode();
            var base64 = sha1.Base64Encode();
            return base64;
        }
    }
}
