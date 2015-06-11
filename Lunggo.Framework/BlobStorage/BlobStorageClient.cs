using System.Collections.Generic;
using System.Drawing;

namespace Lunggo.Framework.BlobStorage
{
    internal abstract class BlobStorageClient
    {
        internal abstract void Init();
        internal abstract string WriteFileToBlob(BlobWriteDto fileDto);
        internal abstract void RenameBlobs(string previousFileUriName, string newFileUriName);
        internal abstract void CopyBlob(string previousFileUriName, string newFileUriName);
        internal abstract void DeleteBlob(string fileUriName);
        internal abstract Image GetImageFromBlobByFileUriName(string fileUriName);
        internal abstract byte[] GetByteArrayByFileUriName(string fileUriName);
        internal abstract List<string> GetDirectoryList(string directoryName);
    }
}
