using System.Collections.Generic;
using System.Drawing;

namespace Lunggo.Framework.BlobStorage
{
    internal abstract class BlobStorageClient
    {
        internal abstract void Init();
        internal abstract string WriteFileToBlob(BlobWriteDto fileDto);
        internal abstract void RenameBlobs(string previousFileUriName, string newFileUriName, BlobContainer container);
        internal abstract void CopyBlob(string previousFileUriName, string newFileUriName, BlobContainer srcContainer, BlobContainer destContainer);
        internal abstract void DeleteBlob(string fileUriName, BlobContainer container);
        internal abstract Image GetImageFromBlobByFileUriName(string fileUriName, BlobContainer container);
        internal abstract byte[] GetByteArrayByFileUriName(string fileUriName, BlobContainer container);
        internal abstract List<string> GetDirectoryList(string directoryName);
    }
}
