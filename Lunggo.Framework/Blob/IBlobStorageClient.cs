using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Blob
{
    public interface IBlobStorageClient
    {
        void init(string connString);
        string WriteFileToBlob(BlobWriteDto fileDto);
        void RenameBlobs(string previousFileUriName, string newFileUriName);
        void CopyBlob(string previousFileUriName, string newFileUriName);
        void DeleteBlob(string fileUriName);
        Image GetImageFromBlobByFileURIName(string fileUriName);
        byte[] GetByteArrayByFileURIName(string fileUriName);
        List<string> GetDirectoryList(string directoryName);
    }
}
