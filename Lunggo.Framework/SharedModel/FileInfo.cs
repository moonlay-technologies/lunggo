using System.IO;

namespace Lunggo.Framework.SharedModel
{
    public class FileInfo
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] FileData { get; set; }
    }
}
