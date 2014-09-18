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
        private static readonly BlobStorageService Instance = new BlobStorageService();
        private BlobStorageService()
        {
            
        }
        public void Init(IBlobStorageClient client)
        {
            _blobStorageClient = client;
        }
        public static BlobStorageService GetInstance()
        {
            return Instance;
        }
        public string WriteFileToBlob(BlobWriteDTO fileDto)
        {
            return _blobStorageClient.WriteFileToBlob(fileDto);
        }
        public void RenameBlobs(string previousFileUriName, string newFileUriName)
        {
            _blobStorageClient.RenameBlobs(previousFileUriName, newFileUriName);
        }
        public void DeleteBlob(string fileUriName)
        {
            _blobStorageClient.DeleteBlob(fileUriName);
        }
        public byte[] GetByteArrayByFileUriName(string fileUriName)
        {
            return _blobStorageClient.GetByteArrayByFileURIName(fileUriName);
        }
        public List<string> GetDirectoryList(string directoryName)
        {
            return _blobStorageClient.GetDirectoryList(directoryName);
        }
        
        
    }
}
