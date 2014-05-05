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
        string WriteFileToBlob(BlobWriteDTO fileDTO);
        void RenameBlobs(string previousFileURIName, string newFileURIName);
        void CopyBlob(string previousFileURIName, string newFileURIName);
        void DeleteBlob(string FileURIName);
        Image GetImageFromBlobByFileURIName(string FileURIName);
        byte[] GetByteArrayByFileURIName(string FileURIName);
        List<string> GetDirectoryList(string directoryName);
    }
}
