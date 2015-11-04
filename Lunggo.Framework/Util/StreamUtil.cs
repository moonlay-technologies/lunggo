using System.IO;
using System.Web;

namespace Lunggo.Framework.Util
{
    public static class StreamUtil
    {
        public static void WriteIntoStream(this Stream stream, string message)
        {
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(message);
            }
        }

        public static string ReadStream(this Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

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
