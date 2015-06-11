using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Lunggo.Framework.Core;
using Lunggo.Framework.Exceptions;
using Lunggo.Framework.Util;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Lunggo.Framework.BlobStorage
{
    public class AzureBlobStorageClient : IBlobStorageClient
    {
        private const string DefaultContainerName = "temp";
        CloudBlobClient _blobStorageClient;
        public AzureBlobStorageClient()
        {
            
        }
        public void Init(string connString)
        {
            CloudStorageAccount blobStorageAccount = CloudStorageAccount.Parse(connString);
            this._blobStorageClient = blobStorageAccount.CreateCloudBlobClient();

        }
        public string WriteFileToBlob(BlobWriteDto fileDto)
        {
            try
            {
                bool ForceUsingDefaultContainerIfContainerNotDefined = true;
                string containerInPath = ParseContainerFromFileUri(fileDto.FileBlobModel.FilePath, ForceUsingDefaultContainerIfContainerNotDefined);
                return ProcessingWriteFileToBlob(fileDto, containerInPath);
            }
            catch (Exception ex)
            {
                LunggoLogger.Error(ex.Message,ex);
                throw;
            }
        }
        private string ParseContainerFromFileUri(string fileUri, bool forceUsingDefaultContainerIfContainerNotDefined)
        {
            try
            {
                string containerName = fileUri.Substring(0, fileUri.IndexOf(@"/"));
                return containerName;
            }
            catch (ArgumentOutOfRangeException notFoundDirectoryException)
            {
                if (forceUsingDefaultContainerIfContainerNotDefined)
                    return DefaultContainerName;
                else
                    return string.Empty;
            }
        }
        
        private string ProcessingWriteFileToBlob(BlobWriteDto blobWriteDto, string wantedContainerName)
        {
            try
            {
                BlobModel newBlobModel = CheckContainerAndGenerateNewBlobModel(blobWriteDto.FileBlobModel.FilePath, wantedContainerName);
                CloudBlockBlob newBlob = GenerateNewBlockBlob(newBlobModel, blobWriteDto.FileBlobModel.FileInfo.ContentType);
                newBlob = FinalBlockBlobBySaveMethod(newBlob, newBlobModel.BlobName, blobWriteDto.SaveMethod);
                return UploadFromStreamIfBlobNotNull(newBlob, blobWriteDto.FileBlobModel.FileInfo.FileData);
            }
            catch (Exception ex)
            {
                LunggoLogger.Error(ex.Message, ex);
                throw;
            }
        }

        private BlobModel CheckContainerAndGenerateNewBlobModel(string fileName, string wantedContainerName)
        {
            DeclareContainerAndCreateIfNotExist(wantedContainerName);
            fileName = RemoveContainerFromUriFile(fileName,wantedContainerName);
            var newBlobModel = new BlobModel {BlobName = fileName, BlobContainer = wantedContainerName};

            return newBlobModel;
        }

        private void DeclareContainerAndCreateIfNotExist(string wantedContainerName)
        {
            CloudBlobContainer wantedContainer = this._blobStorageClient.GetContainerReference(wantedContainerName);
            CheckContainerAndCreateIfNotExist(wantedContainer);
        }
        private string RemoveContainerFromUriFile(string filePath, string containerInPath)
        {
            bool isFileHasDirectory = Path.GetDirectoryName(filePath) != "";
            if (isFileHasDirectory)
                filePath = filePath.Substring((containerInPath + @"/").Length);
            return filePath;
        }
        private void CheckContainerAndCreateIfNotExist(CloudBlobContainer wantedContainer)
        {
            if (wantedContainer.CreateIfNotExists())
            {
                var permissions = wantedContainer.GetPermissions();
                permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                wantedContainer.SetPermissions(permissions);
            }
        }
        private CloudBlockBlob GenerateNewBlockBlob(BlobModel newBlobModel, string contentType)
        {
            CloudBlobContainer wantedContainer = this._blobStorageClient.GetContainerReference(newBlobModel.BlobContainer);
            CloudBlockBlob newBlob = wantedContainer.GetBlockBlobReference(newBlobModel.BlobName);
            newBlob.Properties.ContentType = contentType;
            return newBlob;
        }
        private CloudBlockBlob FinalBlockBlobBySaveMethod(CloudBlockBlob newBlob, string newBlobName, SaveMethod saveMethod)
        {
            switch (saveMethod)
            {
                case SaveMethod.Force:
                    {
                        return newBlob;
                    }
                case SaveMethod.Skip:
                    {
                        if (!IsBlobExists(newBlob))
                            return newBlob;
                        else
                            return null;
                    }
                case SaveMethod.Exception:
                    {
                        if (!IsBlobExists(newBlob))
                            return newBlob;
                        else
                            throw new FileAlreadyExistException("File with requested name already exist");
                    }
                case SaveMethod.GenerateNewName:
                    {
                        if (!IsBlobExists(newBlob))
                            return newBlob;
                        else
                        {
                            return GetNewNameForBlob(newBlobName, newBlob);
                        }
                    }
            }
            throw new Exception("Save Method cannot be identified");

        }
        private CloudBlockBlob GetNewNameForBlob(string currentWantedBlobName, CloudBlockBlob newBlob)
        {
            int count = 1;

            var fileNameOnly = Path.GetFileNameWithoutExtension(currentWantedBlobName);
            var extension = Path.GetExtension(currentWantedBlobName);
            var path = Path.GetDirectoryName(currentWantedBlobName);
            var newFullName = currentWantedBlobName;

            var blobContainer = this._blobStorageClient.GetContainerReference(newBlob.Container.Name);
            CloudBlockBlob blobWithNewName;
            do
            {
                var tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                newFullName = Path.Combine(path, tempFileName + extension);
                blobWithNewName = blobContainer.GetBlockBlobReference(newFullName);
                blobWithNewName.Properties.ContentType = newBlob.Properties.ContentType;
            } while (IsBlobExists(blobWithNewName));
            return blobWithNewName;
        }
        private static bool IsBlobExists(CloudBlockBlob blob)
        {
            try
            {
                blob.FetchAttributes();
                return true;
            }
            catch (StorageException e)
            {
                return false;
            }
            catch(Exception ex)
            {
                LunggoLogger.Error(ex.Message, ex);
                throw;
            }
        }
        private string UploadFromStreamIfBlobNotNull(CloudBlockBlob newBlob, byte[] fileByte)
        {
            if (newBlob != null)
            {
                newBlob.UploadFromStream(new MemoryStream(fileByte));
                return newBlob.Name;
            }
            else
                return null;
        }

        public void RenameBlobs(string previousFileUriName, string newFileUriName)
        {
            try
            {
                CloudBlockBlob previousBlob = GetBlobFromStorage(previousFileUriName);
                CloudBlockBlob newBlob = GetBlobFromStorage(newFileUriName);

                CheckContainerAndCreateIfNotExist(newBlob.Container);

                newBlob.StartCopyFromBlob(previousBlob);
                previousBlob.Delete();
            }
            catch (StorageException ex)
            {
                throw new StorageException("File Not Found!");
            }
            catch (Exception ex)
            {
                LunggoLogger.Error(ex.Message, ex);
                throw;
            }
        }

        public void CopyBlob(string previousFileUriName, string newFileUriName)
        {
            try
            {
                CloudBlockBlob previousBlob = GetBlobFromStorage(previousFileUriName);
                CloudBlockBlob newBlob = GetBlobFromStorage(newFileUriName);

                CheckContainerAndCreateIfNotExist(newBlob.Container);

                newBlob.StartCopyFromBlob(previousBlob);
            }
            catch (StorageException ex)
            {
                throw new StorageException("File Not Found!");
            }
            catch (Exception ex)
            {
                LunggoLogger.Error(ex.Message, ex);
                throw;
            }
        }

        public void DeleteBlob(string fileUriName)
        {
            try
            {
                CloudBlockBlob blobToDelete = GetBlobFromStorage(fileUriName);
                blobToDelete.Delete();
            }
            catch (Exception ex)
            {
                LunggoLogger.Error(ex.Message, ex);
                throw;
            }
        }

        public Image GetImageFromBlobByFileUriName(string fileUriName)
        {
            Image returnImage = Image.FromStream(GetStreamFromBlobByFileUriName(fileUriName));
            return returnImage;
        }
        public MemoryStream GetStreamFromBlobByFileUriName(string fileUriName)
        {
            var blobToGet = GetBlobFromStorage(fileUriName);
            var memoryStream = new MemoryStream();
            blobToGet.DownloadToStream(memoryStream);
            return memoryStream;
        }
        public byte[] GetByteArrayByFileUriName(string fileUriName)
        {
            CloudBlockBlob blobToGet = GetBlobFromStorage(fileUriName);
            Stream streamOfBlob = new MemoryStream();
            blobToGet.DownloadToStream(streamOfBlob);
            byte[] byteArrayOfBlob = new StreamUtil().StreamToByteArray(streamOfBlob);
            return byteArrayOfBlob;
        }
        public CloudBlockBlob GetBlobFromStorage(string fileUriName)
        {
            try
            {
                bool ForceUsingDefaultContainer = false;
                string containerName = ParseContainerFromFileUri(fileUriName, ForceUsingDefaultContainer);
                CloudBlobContainer wantedContainer = this._blobStorageClient.GetContainerReference(containerName);
                if (!IsFileUriContainBaseUri(fileUriName))
                {
                    fileUriName = fileUriName.Substring((containerName + @"/").Length);
                }
                CloudBlockBlob theBlob = wantedContainer.GetBlockBlobReference(fileUriName);
                return theBlob;
            }
            catch (ArgumentException ex)
            {
                if (ex.Source == "Microsoft.WindowsAzure.Storage")
                    throw new ArgumentException("Container Not Found!");
                else
                    throw;
            }
            catch (Exception ex)
            {
                LunggoLogger.Error(ex.Message, ex);
                throw;
            }
        }
        private bool IsFileUriContainBaseUri(string FileURIName)
        {
            return FileURIName.Contains(this._blobStorageClient.BaseUri.ToString());
        }

        public IEnumerable<IListBlobItem> GetAllBlobsByContainer(string containerName)
        {
            CloudBlobContainer container = this._blobStorageClient.GetContainerReference(containerName);
            var listBlobs = container.ListBlobs();
            return listBlobs;
        }
        public List<string> GetDirectoryList(string directoryName)
        {
            try
            {
                IEnumerable<IListBlobItem> listBlob = GetListBlob(directoryName);
                List<string> listUri = new List<string>();
                foreach (IListBlobItem BlobItem in listBlob)
                {
                    listUri.Add(BlobItem.Uri.ToString());
                }
                return listUri;
            }
            catch (Exception ex)
            {
                LunggoLogger.Error(ex.Message, ex);
                throw;
            }
        }
        private IEnumerable<IListBlobItem> GetListBlob(string directoryName)
        {
            bool ForceUsingDefaultContainerIfContainerNotDefined = false;
            string containerInPath = ParseContainerFromFileUri(directoryName, ForceUsingDefaultContainerIfContainerNotDefined);
            CloudBlobContainer container;
            if (string.IsNullOrEmpty(containerInPath))
            {
                container = this._blobStorageClient.GetContainerReference(directoryName);
                directoryName = string.Empty;
            }
            else
            {
                container = this._blobStorageClient.GetContainerReference(containerInPath);
                directoryName = RemoveContainerFromUriFile(directoryName, containerInPath);
            }
            
            IEnumerable<IListBlobItem> blobs = container.ListBlobs(directoryName, true);
            return blobs;
        }

        private class BlobModel
        {
            public string BlobName { get; set; }
            public string BlobContainer { get; set; }
        }
    }
}
