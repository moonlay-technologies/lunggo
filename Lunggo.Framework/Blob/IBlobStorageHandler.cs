using Microsoft.WindowsAzure.StorageClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Lunggo.Framework.Blob
{
    public interface IBlobStorageHandler
    {
        bool uploadFileToBlob(HttpPostedFileBase uploadedFile);
        bool uploadFileToBlob(HttpPostedFileBase uploadedFile, string wantedContainerName);
        bool uploadFileToBlob(FileBlobModel fileBlobModel);
        bool uploadFileToBlob(FileBlobModel fileBlobModel, string wantedContainerName);


        bool RenameBlobs(string previousFileURIName, string newFileURIName);


        bool deleteBlob(string FileURIName);


        Image getImageFromBlobByFileURIName(string FileURIName);
        MemoryStream getStreamFromBlobByFileURIName(string FileURIName);
        byte[] getByteArrayByFileURIName(string FileURIName);
        CloudBlob getBlobFromStorage(string FileURIName);
        IEnumerable<IListBlobItem> getAllBlobsByContainer(string containerName);
        IEnumerable<IListBlobItem> GetDirectoryList(string directoryName, string subDirectoryName);
    }
}
