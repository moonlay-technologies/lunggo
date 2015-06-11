using System.Collections.Generic;
using System.Drawing;

namespace Lunggo.Framework.BlobStorage
{
    public interface IBlobStorageClient
    {
        void Init(string connString);
        string WriteFileToBlob(BlobWriteDto fileDto);
        void RenameBlobs(string previousFileUriName, string newFileUriName);
        void CopyBlob(string previousFileUriName, string newFileUriName);
        void DeleteBlob(string fileUriName);
        Image GetImageFromBlobByFileUriName(string fileUriName);
        byte[] GetByteArrayByFileUriName(string fileUriName);
        List<string> GetDirectoryList(string directoryName);
    }
}
