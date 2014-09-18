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
