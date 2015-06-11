

namespace Lunggo.Framework.BlobStorage
{
    public class BlobWriteDto
    {
        public FileBlobModel FileBlobModel { get; set; }
        private SaveMethod _saveMethod = SaveMethod.Skip;
        public SaveMethod SaveMethod
        {
            get { return _saveMethod; }
            set { _saveMethod = value; }
        }

    }
    public enum SaveMethod
    {
        Force,
        Skip,
        Exception,
        GenerateNewName
    };
}
