using System.Collections.Generic;
using System.IO;

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
        public void Init(string connString)
        {
            if (!_isInitialized)
            {
                Client.Init(connString);
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
        public void RenameBlobs(string previousFileUriName, string newFileUriName, string container)
        {
            Client.RenameBlobs(previousFileUriName, newFileUriName, container);
        }
        public void DeleteBlob(string fileUriName, string container)
        {
            Client.DeleteBlob(fileUriName, container);
        }
        public byte[] GetByteArrayByFileUriName(string fileUriName, string container)
        {
            return Client.GetByteArrayByFileUriName(fileUriName, container);
        }
        public byte[] GetByteArrayByFileInContainer(string fileName, string container)
        {
            return Client.GetByteArrayByFileInContainer(fileName, container);
        }

        public MemoryStream GetMemoryStreamByFileInContainer(string fileName, string container)
        {
            return Client.GetMemoryStreamByFileInContainer(fileName, container);
        }

        public List<string> GetDirectoryList(string directoryName)
        {
            return Client.GetDirectoryList(directoryName);
        }

        public List<string> GetFileNameList(string containerName, string folderName)
        {
            return Client.GetListFileName(containerName, folderName);
        } 
    }
}
