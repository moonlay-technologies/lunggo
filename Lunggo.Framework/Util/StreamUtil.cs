using System.IO;
using System.Web;

namespace Lunggo.Framework.Util
{
    public static class StreamUtil
    {

        public static byte[] StreamToByteArray(this Stream inputStream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                inputStream.CopyTo(ms);
                return ms.ToArray();
            }
        }

        public static byte[] HttpPostedFileBaseToArray(this HttpPostedFileBase file)
        {
            MemoryStream target = new MemoryStream();
            file.InputStream.CopyTo(target);
            byte[] arrayData = target.ToArray();
            return arrayData;
        }
    }
}
