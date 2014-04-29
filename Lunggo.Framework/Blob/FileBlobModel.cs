using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Blob
{
    public class FileBlobModel
    {
        public string FilePath { get; set; }
        public string ContentType { get; set; }
        public byte[] FileByte { get; set; }

    }
    
}
