using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Http
{
    public class HttpFile
    {
        public String Name { get; set; }
        public String ContentType { get; set; }
        public String FileName { get; set; }
        public long ContentLength { get; set; }
        public byte[] Data { get; set; }
    }
}
