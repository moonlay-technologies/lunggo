using System.Collections.Generic;

namespace Lunggo.Framework.BlobStorage
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
        public string WriteFileToBlob(BlobWriteDto fileDto)
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
            return _blobStorageClient.GetByteArrayByFileUriName(fileUriName);
        }
        public List<string> GetDirectoryList(string directoryName)
        {
            return _blobStorageClient.GetDirectoryList(directoryName);
        }
        
        
    }
}
