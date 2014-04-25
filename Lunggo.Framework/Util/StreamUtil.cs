using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Lunggo.Framework.Util
{
    public class StreamUtil
    {
        public byte[] StreamToByteArray(Stream inputStream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                inputStream.CopyTo(ms);
                return ms.ToArray();
            }
        }
        public byte[] HttpPostedFileBaseToArray(HttpPostedFileBase file)
        {
            MemoryStream target = new MemoryStream();
            file.InputStream.CopyTo(target);
            byte[] arrayData = target.ToArray();
            return arrayData;
        }
    }
}
