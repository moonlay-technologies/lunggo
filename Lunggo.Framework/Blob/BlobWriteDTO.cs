using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Blob
{
    public class BlobWriteDTO
    {
        public FileBlobModel FileBlobModel { get; set; }
        private SaveMethod _saveMethod = SaveMethod.SKIP;
        public SaveMethod SaveMethod
        {
            get { return _saveMethod; }
            set { _saveMethod = value; }
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
