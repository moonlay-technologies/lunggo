using System.Security.Cryptography;
using System.Text;
using Lunggo.Framework.Encoder;

namespace Lunggo.CustomerWeb.Helper
{
    public static class Generator
    {
        public static string GenerateTrxIdRegId(string trxId)
        {
            var sha1 = trxId.Sha1Encode();
            var base64 = sha1.Base64Encode();
            return base64;
        }
    }
}
