using Lunggo.Framework.Exceptions;
using Lunggo.Framework.Util;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
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
    public class BlobStorageHandler : IBlobStorageHandler
    {

        CloudStorageAccount blobStorageAccount = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
        //CloudStorageAccount blobStorageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnection"].ConnectionString);
        CloudBlobClient blobStorageClient;
        FileBlobModel uploadFileModel;
        CloudBlockBlob newBlob;
        string newBlobName;
        string containerName="temp/";
        public BlobStorageHandler()
        {
            this.blobStorageClient = blobStorageAccount.CreateCloudBlobClient();
        }
        public bool uploadFileToBlob(HttpPostedFileBase uploadedFile)
        {
            try
            {
                this.uploadFileModel = new FileBlobModel() { FileName = uploadedFile.FileName, FileByte = new StreamUtil().HttpPostedFileBaseToArray(uploadedFile) };
                return processingUploadFileToBlob();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool uploadFileToBlob(HttpPostedFileBase uploadedFile, string wantedContainerName)
        {
            try
            {
                this.uploadFileModel = new FileBlobModel() { FileName = uploadedFile.FileName, FileByte = new StreamUtil().HttpPostedFileBaseToArray(uploadedFile) };
                this.containerName = wantedContainerName;
                return processingUploadFileToBlob();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool uploadFileToBlob(FileBlobModel fileBlobModel)
        {
            try
            {
                this.uploadFileModel = fileBlobModel;
                return processingUploadFileToBlob();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool uploadFileToBlob(FileBlobModel fileBlobModel, string wantedContainerName)
        {
            try
            {
                this.uploadFileModel = fileBlobModel;
                this.containerName = wantedContainerName;
                return processingUploadFileToBlob();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool processingUploadFileToBlob()
        {
            try
            {
                checkContainerAndGenerateNewBlobName();
                generateNewBlockBlob();
                uploadFromStreamIfBlobNotNull(blockBlobBySaveMethod());
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void checkContainerAndGenerateNewBlobName()
        {
            declareContainerAndCreateIfNotExist(this.containerName);
            string newBlobName = string.Format("{0}/{1}", this.containerName, this.uploadFileModel.FileName);
            this.newBlobName = newBlobName;
        }
        private CloudBlobContainer declareContainerAndCreateIfNotExist(string wantedContainerName)
        {
            CloudBlobContainer wantedContainer = this.blobStorageClient.GetContainerReference(wantedContainerName);
            return checkContainerAndCreateIfNotExist(wantedContainer);
        }
        
        private CloudBlobContainer checkContainerAndCreateIfNotExist(CloudBlobContainer wantedContainer)
        {
            if (wantedContainer.CreateIfNotExist())
            {
                var permissions = wantedContainer.GetPermissions();
                permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                wantedContainer.SetPermissions(permissions);
            }
            return wantedContainer;
        }
        private void generateNewBlockBlob()
        {
            this.newBlob = blobStorageClient.GetBlockBlobReference(this.newBlobName);
            this.newBlob.Properties.ContentType = this.uploadFileModel.ContentType;
        }
        private CloudBlockBlob blockBlobBySaveMethod()
        {
            switch (this.uploadFileModel.SaveMethod)
            {
                case SaveMethod.FORCE:
                    {
                        return this.newBlob;
                    }
                case SaveMethod.SKIP:
                    {
                        if (!isBlobExists(newBlob))
                            return this.newBlob;
                        else
                            return null;
                    }
                case SaveMethod.EXCEPTION:
                    {
                        if (!isBlobExists(newBlob))
                            return this.newBlob;
                        else
                            throw new FileAlreadyExistException("File with requested name already exist");
                    }
                case SaveMethod.GENERATE_NEW_NAME:
                    {
                        if (!isBlobExists(newBlob))
                            return this.newBlob;
                        else
                        {
                            return getNewNameForBlob();
                        }
                    }
            }
            throw new Exception("Save Method cannot be identified");

        }
        private CloudBlockBlob getNewNameForBlob()
        {
            int count = 1;

            string fileNameOnly = Path.GetFileNameWithoutExtension(this.newBlobName);
            string extension = Path.GetExtension(this.newBlobName);
            string path = Path.GetDirectoryName(this.newBlobName);
            string newFullName = this.newBlobName;

            CloudBlockBlob blobWithNewName;
            do
            {
                string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                newFullName = Path.Combine(path, tempFileName + extension);
                blobWithNewName = blobStorageClient.GetBlockBlobReference(newFullName);
                blobWithNewName.Properties.ContentType = this.uploadFileModel.ContentType;
            } while (isBlobExists(blobWithNewName));
            return blobWithNewName;
        }
        private static bool isBlobExists(CloudBlob blob)
        {
            try
            {
                blob.FetchAttributes();
                return true;
            }
            catch (StorageClientException e)
            {
                if (e.ErrorCode == StorageErrorCode.ResourceNotFound)
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }
        private void uploadFromStreamIfBlobNotNull(CloudBlockBlob newBlob)
        {
            if (newBlob!=null)
                newBlob.UploadFromStream(new MemoryStream(this.uploadFileModel.FileByte));
        }
        public bool RenameBlobs(string previousFileURIName, string newFileURIName)
        {
            try
            {
                CloudBlob previousBlob = getBlobFromStorage(previousFileURIName);
                CloudBlob newBlob = getBlobFromStorage(newFileURIName);

                CloudBlobContainer blobContainer = checkContainerAndCreateIfNotExist(newBlob.Container);

                newBlob.CopyFromBlob(previousBlob);
                previousBlob.Delete();
                return true;
            }
            catch (StorageClientException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string getContainerName(string newFileURIName)
        {
            string containerName = newFileURIName.Substring(0, newFileURIName.IndexOf("/"));
            return containerName;
        }

        public bool deleteBlob(string FileURIName)
        {
            try
            {
                CloudBlob blobToDelete = getBlobFromStorage(FileURIName);
                blobToDelete.Delete();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Image getImageFromBlobByFileURIName(string FileURIName)
        {
            Image returnImage = Image.FromStream(getStreamFromBlobByFileURIName(FileURIName));
            return returnImage;
        }
        public MemoryStream getStreamFromBlobByFileURIName(string FileURIName)
        {
            CloudBlob blobToGet = getBlobFromStorage(FileURIName);
            MemoryStream memoryStream = new MemoryStream();
            blobToGet.DownloadToStream(memoryStream);
            return memoryStream;
        }
        public byte[] getByteArrayByFileURIName(string FileURIName)
        {
            CloudBlob blobToGet = getBlobFromStorage(FileURIName);
            Stream streamOfBlob = new MemoryStream();
            blobToGet.DownloadToStream(streamOfBlob);
            byte[] byteArrayOfBlob = new StreamUtil().StreamToByteArray(streamOfBlob);
            return byteArrayOfBlob;
        }
        public CloudBlob getBlobFromStorage(string FileURIName)
        {
            CloudBlob theBlob = blobStorageClient.GetBlobReference(FileURIName);
            return theBlob;
        }
        
        public IEnumerable<IListBlobItem> getAllBlobsByContainer(string containerName)
        {
            CloudBlobContainer container = this.blobStorageClient.GetContainerReference(containerName);
            var listBlobs = container.ListBlobs();
            return listBlobs;
        }
        public IEnumerable<IListBlobItem> GetDirectoryList(string directoryName, string subDirectoryName)
        {
            CloudBlobDirectory directory = this.blobStorageClient.GetBlobDirectoryReference(directoryName);
            CloudBlobDirectory subDirectory = directory.GetSubdirectory(subDirectoryName);

            return subDirectory.ListBlobs();
        }
        
    }
}
