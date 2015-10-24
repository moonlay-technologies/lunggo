using System.Collections.Generic;
using System.Drawing;

namespace Lunggo.Framework.BlobStorage
{
    internal abstract class BlobStorageClient
    {
        internal abstract void Init(string connString);
        internal abstract string WriteFileToBlob(BlobWriteDto fileDto);
        internal abstract void RenameBlobs(string previousFileUriName, string newFileUriName, string container);
        internal abstract void CopyBlob(string previousFileUriName, string newFileUriName, string srcContainer, string destContainer);
        internal abstract void DeleteBlob(string fileUriName, string container);
        internal abstract Image GetImageFromBlobByFileUriName(string fileUriName, string container);
        internal abstract byte[] GetByteArrayByFileUriName(string fileUriName, string container);
        internal abstract List<string> GetDirectoryList(string directoryName);
    }
}
