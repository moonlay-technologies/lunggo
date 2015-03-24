using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Lunggo.Framework.Util
{
    public class CompressionUtil
    {
        public static byte[] Compress(String input)
        {
            var raw = Encoding.UTF8.GetBytes(input);
            using (var memory = new MemoryStream())
            {
                using (var gzip = new GZipStream(memory, CompressionMode.Compress, true))
                {
                    gzip.Write(raw, 0, raw.Length);
                }
                return memory.ToArray();
            }
        }

        public static String Decompress(byte[] gzip)
        {
            using (var stream = new GZipStream(new MemoryStream(gzip), CompressionMode.Decompress))
            {
                const int size = 4096;
                var buffer = new byte[size];
                using (var memory = new MemoryStream())
                {
                    var count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return Encoding.UTF8.GetString(memory.ToArray());
                }
            }
        }
    }
}
