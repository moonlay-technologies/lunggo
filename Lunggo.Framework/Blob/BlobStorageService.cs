using Lunggo.Framework.Exceptions;
using Lunggo.Framework.Util;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Lunggo.Framework.Blob
{
    public class BlobStorageService
    {

        IBlobStorageClient _blobStorageClient;
        private static BlobStorageService instance = new BlobStorageService();


        private BlobStorageService()
        {
            
        }
        public void init(IBlobStorageClient client)
        {
            _blobStorageClient = client;
        }
        public static BlobStorageService GetInstance()
        {
            return instance;
        }
        public string WriteFileToBlob(BlobWriteDTO fileDTO)
        {
            return _blobStorageClient.WriteFileToBlob(fileDTO);
        }
        public void RenameBlobs(string previousFileURIName, string newFileURIName)
        {
            _blobStorageClient.RenameBlobs(previousFileURIName, newFileURIName);
        }
        public void DeleteBlob(string FileURIName)
        {
            _blobStorageClient.DeleteBlob(FileURIName);
        }
        public byte[] GetByteArrayByFileURIName(string FileURIName)
        {
            return _blobStorageClient.GetByteArrayByFileURIName(FileURIName);
        }
        public List<string> GetDirectoryList(string directoryName)
        {
            return _blobStorageClient.GetDirectoryList(directoryName);
        }
        
        
    }
}
