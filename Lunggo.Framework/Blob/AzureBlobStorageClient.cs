using Lunggo.Framework.Core;
using Lunggo.Framework.Exceptions;
using Lunggo.Framework.Util;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
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
    public class AzureBlobStorageClient : IBlobStorageClient
    {
        string defaultContainerName = "temp";
        CloudBlobClient _blobStorageClient;
        public AzureBlobStorageClient()
        {
            
        }
        public void init(string connString)
        {
            CloudStorageAccount blobStorageAccount = CloudStorageAccount.Parse(connString);
            this._blobStorageClient = blobStorageAccount.CreateCloudBlobClient();

        }
        public string WriteFileToBlob(BlobWriteDTO fileDTO)
        {
            try
            {
                bool ForceUsingDefaultContainerIfContainerNotDefined = true;
                string containerInPath = ParseContainerFromFileUri(fileDTO.FileBlobModel.FilePath, ForceUsingDefaultContainerIfContainerNotDefined);
                return ProcessingWriteFileToBlob(fileDTO, containerInPath);
            }
            catch (Exception ex)
            {
                LunggoLogger.Error(ex.Message,ex);
                throw;
            }
        }
        private string ParseContainerFromFileUri(string FileURI, bool ForceUsingDefaultContainerIfContainerNotDefined)
        {
            try
            {
                string containerName = FileURI.Substring(0, FileURI.IndexOf(@"/"));
                return containerName;
            }
            catch (ArgumentOutOfRangeException NotFoundDirectoryException)
            {
                if (ForceUsingDefaultContainerIfContainerNotDefined)
                    return this.defaultContainerName;
                else
                    return string.Empty;
            }
        }
        private string removeBaseUri(string FileURI)
        {
            int IndexAfterContainerInFileName = (this._blobStorageClient.BaseUri.ToString() + @"/").Length;
            string newFileURI = FileURI.Substring(IndexAfterContainerInFileName);
            return newFileURI;
        }
        private bool isPathHasContainer(string containerInPath)
        {
            return (string.IsNullOrEmpty(containerInPath) ? false : true);
        }
        private string ProcessingWriteFileToBlob(BlobWriteDTO blobWriteDTO, string wantedContainerName)
        {
            try
            {
                BlobModel newBlobModel = CheckContainerAndGenerateNewBlobModel(blobWriteDTO.FileBlobModel.FilePath, wantedContainerName);
                CloudBlockBlob newBlob = GenerateNewBlockBlob(newBlobModel, blobWriteDTO.FileBlobModel.FileInfo.ContentType);
                newBlob = FinalBlockBlobBySaveMethod(newBlob, newBlobModel.BlobName, blobWriteDTO.SaveMethod);
                return UploadFromStreamIfBlobNotNull(newBlob, blobWriteDTO.FileBlobModel.FileInfo.FileData);
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
            BlobModel newBlobModel = new BlobModel();

            newBlobModel.BlobName = fileName;
            newBlobModel.BlobContainer = wantedContainerName;
            return newBlobModel;
        }
        private void DeclareContainerAndCreateIfNotExist(string wantedContainerName)
        {
            CloudBlobContainer wantedContainer = this._blobStorageClient.GetContainerReference(wantedContainerName);
            CheckContainerAndCreateIfNotExist(wantedContainer);
        }
        private string RemoveContainerFromUriFile(string filePath, string containerInPath)
        {
            bool isFileHasDirectory = Path.GetDirectoryName(filePath) == "" ? false : true;
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
        private CloudBlockBlob GenerateNewBlockBlob(BlobModel newBlobModel, string ContentType)
        {
            CloudBlobContainer wantedContainer = this._blobStorageClient.GetContainerReference(newBlobModel.BlobContainer);
            CloudBlockBlob newBlob = wantedContainer.GetBlockBlobReference(newBlobModel.BlobName);
            newBlob.Properties.ContentType = ContentType;
            return newBlob;
        }
        private CloudBlockBlob FinalBlockBlobBySaveMethod(CloudBlockBlob newBlob, string newBlobName, SaveMethod saveMethod)
        {
            switch (saveMethod)
            {
                case SaveMethod.FORCE:
                    {
                        return newBlob;
                    }
                case SaveMethod.SKIP:
                    {
                        if (!isBlobExists(newBlob))
                            return newBlob;
                        else
                            return null;
                    }
                case SaveMethod.EXCEPTION:
                    {
                        if (!isBlobExists(newBlob))
                            return newBlob;
                        else
                            throw new FileAlreadyExistException("File with requested name already exist");
                    }
                case SaveMethod.GENERATE_NEW_NAME:
                    {
                        if (!isBlobExists(newBlob))
                            return newBlob;
                        else
                        {
                            return GetNewNameForBlob(newBlobName, newBlob);
                        }
                    }
            }
            throw new Exception("Save Method cannot be identified");

        }
        private CloudBlockBlob GetNewNameForBlob(string CurrentWantedBlobName, CloudBlockBlob newBlob)
        {
            int count = 1;

            string fileNameOnly = Path.GetFileNameWithoutExtension(CurrentWantedBlobName);
            string extension = Path.GetExtension(CurrentWantedBlobName);
            string path = Path.GetDirectoryName(CurrentWantedBlobName);
            string newFullName = CurrentWantedBlobName;

            CloudBlobContainer blobContainer = this._blobStorageClient.GetContainerReference(newBlob.Container.Name);
            CloudBlockBlob BlobWithNewName;
            do
            {
                string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                newFullName = Path.Combine(path, tempFileName + extension);
                BlobWithNewName = blobContainer.GetBlockBlobReference(newFullName);
                BlobWithNewName.Properties.ContentType = newBlob.Properties.ContentType;
            } while (isBlobExists(BlobWithNewName));
            return BlobWithNewName;
        }
        private static bool isBlobExists(CloudBlockBlob blob)
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
        public void RenameBlobs(string previousFileURIName, string newFileURIName)
        {
            try
            {
                CloudBlockBlob previousBlob = GetBlobFromStorage(previousFileURIName);
                CloudBlockBlob newBlob = GetBlobFromStorage(newFileURIName);

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
        public void CopyBlob(string previousFileURIName, string newFileURIName)
        {
            try
            {
                CloudBlockBlob previousBlob = GetBlobFromStorage(previousFileURIName);
                CloudBlockBlob newBlob = GetBlobFromStorage(newFileURIName);

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

        public void DeleteBlob(string FileURIName)
        {
            try
            {
                CloudBlockBlob blobToDelete = GetBlobFromStorage(FileURIName);
                blobToDelete.Delete();
            }
            catch (Exception ex)
            {
                LunggoLogger.Error(ex.Message, ex);
                throw;
            }
        }

        public Image GetImageFromBlobByFileURIName(string FileURIName)
        {
            Image returnImage = Image.FromStream(GetStreamFromBlobByFileURIName(FileURIName));
            return returnImage;
        }
        public MemoryStream GetStreamFromBlobByFileURIName(string FileURIName)
        {
            CloudBlockBlob blobToGet = GetBlobFromStorage(FileURIName);
            MemoryStream memoryStream = new MemoryStream();
            blobToGet.DownloadToStream(memoryStream);
            return memoryStream;
        }
        public byte[] GetByteArrayByFileURIName(string FileURIName)
        {
            CloudBlockBlob blobToGet = GetBlobFromStorage(FileURIName);
            Stream streamOfBlob = new MemoryStream();
            blobToGet.DownloadToStream(streamOfBlob);
            byte[] byteArrayOfBlob = new StreamUtil().StreamToByteArray(streamOfBlob);
            return byteArrayOfBlob;
        }
        public CloudBlockBlob GetBlobFromStorage(string FileURIName)
        {
            try
            {
                CloudBlobContainer wantedContainer;
                bool ForceUsingDefaultContainer = false;
                string containerName = ParseContainerFromFileUri(FileURIName, ForceUsingDefaultContainer);
                wantedContainer = this._blobStorageClient.GetContainerReference(containerName);
                if (!isFileUriContainBaseUri(FileURIName))
                {
                    FileURIName = FileURIName.Substring((containerName + @"/").Length);
                }
                CloudBlockBlob theBlob = wantedContainer.GetBlockBlobReference(FileURIName);
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
        private bool isFileUriContainBaseUri(string FileURIName)
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
                IEnumerable<IListBlobItem> listBlob = getListBlob(directoryName);
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
        private IEnumerable<IListBlobItem> getListBlob(string directoryName)
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
