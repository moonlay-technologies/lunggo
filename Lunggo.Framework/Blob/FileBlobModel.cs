using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Blob
{
    public class FileBlobModel
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] FileByte { get; set; }
        private SaveMethod saveMethod = SaveMethod.SKIP;
        public SaveMethod SaveMethod
        {
            get { return saveMethod; }
            set { saveMethod = value; }
        }

    }
    public enum SaveMethod
    {
        FORCE,
        SKIP,
        EXCEPTION,
        GENERATE_NEW_NAME
    };
}
