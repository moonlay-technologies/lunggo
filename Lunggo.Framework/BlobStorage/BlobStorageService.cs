using System;
using System.Collections.Generic;

namespace Lunggo.Framework.BlobStorage
{
    public partial class BlobStorageService
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
        }
        public static BlobStorageService GetInstance()
        {
            return Instance;
        }
        public string WriteFileToBlob(BlobWriteDto fileDto)
        {
            return Client.WriteFileToBlob(fileDto);
        }
        public void RenameBlobs(string previousFileUriName, string newFileUriName, BlobContainer container)
        {
            Client.RenameBlobs(previousFileUriName, newFileUriName, container);
        }
        public void DeleteBlob(string fileUriName, BlobContainer container)
        {
            Client.DeleteBlob(fileUriName, container);
        }
        public byte[] GetByteArrayByFileUriName(string fileUriName, BlobContainer container)
        {
            return Client.GetByteArrayByFileUriName(fileUriName, container);
        }
        public byte[] GetByteArrayByFileInContainer(string fileName, BlobContainer container)
        {
            return Client.GetByteArrayByFileInContainer(fileName, container);
        }
        public List<string> GetDirectoryList(string directoryName)
        {
            return Client.GetDirectoryList(directoryName);
        }
        
        
    }
}
