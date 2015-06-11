using System;
using System.Collections.Generic;

namespace Lunggo.Framework.BlobStorage
{
    public class BlobStorageService
    {
        private static readonly BlobStorageService Instance = new BlobStorageService();
        private bool _isInitialized;
        private static readonly AzureBlobStorageClient Client = AzureBlobStorageClient.GetClientInstance();

        private BlobStorageService()
        {
            
        }
        public void Init()
        {
            if (!_isInitialized)
            {
                Client.Init();
                _isInitialized = true;
            }
            else
            {
                throw new InvalidOperationException("BlobStorageService is already initialized");
            }
        }
        public static BlobStorageService GetInstance()
        {
            return Instance;
        }
        public string WriteFileToBlob(BlobWriteDto fileDto)
        {
            return Client.WriteFileToBlob(fileDto);
        }
        public void RenameBlobs(string previousFileUriName, string newFileUriName)
        {
            Client.RenameBlobs(previousFileUriName, newFileUriName);
        }
        public void DeleteBlob(string fileUriName)
        {
            Client.DeleteBlob(fileUriName);
        }
        public byte[] GetByteArrayByFileUriName(string fileUriName)
        {
            return Client.GetByteArrayByFileUriName(fileUriName);
        }
        public List<string> GetDirectoryList(string directoryName)
        {
            return Client.GetDirectoryList(directoryName);
        }
        
        
    }
}
