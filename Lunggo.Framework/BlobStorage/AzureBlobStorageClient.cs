using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Lunggo.Framework.Config;
using Lunggo.Framework.Core;
using Lunggo.Framework.Exceptions;
using Lunggo.Framework.Util;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Lunggo.Framework.BlobStorage
{
    public partial class BlobStorageService
    {
        private class AzureBlobStorageClient : BlobStorageClient
        {
            private static readonly AzureBlobStorageClient ClientInstance = new AzureBlobStorageClient();
            private bool _isInitialized;
            private const string DefaultContainerName = "temp";
            private CloudBlobClient _blobStorageClient;

            private AzureBlobStorageClient()
            {

            }

            internal static AzureBlobStorageClient GetClientInstance()
            {
                return ClientInstance;
            }

            internal override void Init()
            {
                if (!_isInitialized)
                {
                    var connString = ConfigManager.GetInstance().GetConfigValue("azureStorage", "connectionString");
                    var blobStorageAccount = CloudStorageAccount.Parse(connString);
                    _blobStorageClient = blobStorageAccount.CreateCloudBlobClient();
                    foreach (var reference in Enum.GetValues(typeof (BlobContainer)).Cast<BlobContainer>())
                        DeclareContainerAndCreateIfNotExist(reference.ToString().ToLower());
                    _isInitialized = true;
                }
            }

            internal override string WriteFileToBlob(BlobWriteDto fileDto)
            {
                //bool ForceUsingDefaultContainerIfContainerNotDefined = true;
                //string containerInPath = ParseContainerFromFileUri(fileDto.FileBlobModel.Container.ToString().ToLower(),ForceUsingDefaultContainerIfContainerNotDefined);
                return ProcessingWriteFileToBlob(fileDto, fileDto.FileBlobModel.Container.ToString().ToLower());
            }

            private string ParseContainerFromFileUri(string fileUri,
                bool forceUsingDefaultContainerIfContainerNotDefined)
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
                BlobModel newBlobModel = CheckContainerAndGenerateNewBlobModel(blobWriteDto.FileBlobModel.FileInfo.FileName,
                    wantedContainerName);
                CloudBlockBlob newBlob = GenerateNewBlockBlob(newBlobModel,
                    blobWriteDto.FileBlobModel.FileInfo.ContentType);
                newBlob = FinalBlockBlobBySaveMethod(newBlob, newBlobModel.BlobName, blobWriteDto.SaveMethod);
                return UploadFromStreamIfBlobNotNull(newBlob, blobWriteDto.FileBlobModel.FileInfo.FileData);
            }

            private BlobModel CheckContainerAndGenerateNewBlobModel(string fileName, string wantedContainerName)
            {
                DeclareContainerAndCreateIfNotExist(wantedContainerName);
                //fileName = RemoveContainerFromUriFile(fileName, wantedContainerName);
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
                CloudBlobContainer wantedContainer =
                    this._blobStorageClient.GetContainerReference(newBlobModel.BlobContainer);
                CloudBlockBlob newBlob = wantedContainer.GetBlockBlobReference(newBlobModel.BlobName);
                newBlob.Properties.ContentType = contentType;
                return newBlob;
            }

            private CloudBlockBlob FinalBlockBlobBySaveMethod(CloudBlockBlob newBlob, string newBlobName,
                SaveMethod saveMethod)
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

            internal override void RenameBlobs(string previousFileUriName, string newFileUriName, BlobContainer container)
            {
                try
                {
                    CloudBlockBlob previousBlob = GetBlobFromStorage(previousFileUriName, container);
                    CloudBlockBlob newBlob = GetBlobFromStorage(newFileUriName, container);

                    CheckContainerAndCreateIfNotExist(newBlob.Container);

                    newBlob.StartCopyFromBlob(previousBlob);
                    previousBlob.Delete();
                }
                catch (StorageException ex)
                {
                    throw new StorageException("File Not Found!");
                }
            }

            internal override void CopyBlob(string previousFileUriName, string newFileUriName, BlobContainer srcContainer, BlobContainer destContainer)
            {
                try
                {
                    CloudBlockBlob previousBlob = GetBlobFromStorage(previousFileUriName, srcContainer);
                    CloudBlockBlob newBlob = GetBlobFromStorage(newFileUriName, destContainer);

                    CheckContainerAndCreateIfNotExist(newBlob.Container);

                    newBlob.StartCopyFromBlob(previousBlob);
                }
                catch (StorageException ex)
                {
                    throw new StorageException("File Not Found!");
                }
            }

            internal override void DeleteBlob(string fileUriName, BlobContainer container)
            {
                CloudBlockBlob blobToDelete = GetBlobFromStorage(fileUriName, container);
                blobToDelete.Delete();
            }

            internal override Image GetImageFromBlobByFileUriName(string fileUriName, BlobContainer container)
            {
                Image returnImage = Image.FromStream(GetStreamFromBlobByFileUriName(fileUriName, container));
                return returnImage;
            }

            internal MemoryStream GetStreamFromBlobByFileUriName(string fileUriName, BlobContainer container)
            {
                var blobToGet = GetBlobFromStorage(fileUriName, container);
                var memoryStream = new MemoryStream();
                blobToGet.DownloadToStream(memoryStream);
                return memoryStream;
            }

            internal override byte[] GetByteArrayByFileUriName(string fileUriName, BlobContainer container)
            {
                CloudBlockBlob blobToGet = GetBlobFromStorage(fileUriName, container);
                var streamOfBlob = new MemoryStream();
                blobToGet.DownloadToStream(streamOfBlob);
                byte[] byteArrayOfBlob = streamOfBlob.ToArray();
                return byteArrayOfBlob;
            }

            internal byte[] GetByteArrayByFileInContainer(string fileName, BlobContainer container)
            {
                CloudBlockBlob blobToGet = GetBlobFromStorage(fileName, container);
                var streamOfBlob = new MemoryStream();
                blobToGet.DownloadToStream(streamOfBlob);
                byte[] byteArrayOfBlob = streamOfBlob.ToArray();
                return byteArrayOfBlob;
            }

            internal CloudBlockBlob GetBlobFromStorage(string fileUriName, BlobContainer container)
            {
                try
                {
                    //bool ForceUsingDefaultContainer = true;
                    //string containerName = ParseContainerFromFileUri(fileUriName, ForceUsingDefaultContainer);
                    //if (IsFileUriContainBaseUri(fileUriName))
                    //{
                    //  fileUriName = fileUriName.Substring((container.ToString().ToLower() + @"/").Length);
                    //}
                    CloudBlobContainer wantedContainer = this._blobStorageClient.GetContainerReference(container.ToString().ToLower());
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
            }

            private bool IsFileUriContainBaseUri(string FileURIName)
            {
                return FileURIName.Contains(this._blobStorageClient.BaseUri.ToString());
            }

            internal IEnumerable<IListBlobItem> GetAllBlobsByContainer(string containerName)
            {
                CloudBlobContainer container = this._blobStorageClient.GetContainerReference(containerName);
                var listBlobs = container.ListBlobs();
                return listBlobs;
            }

            internal override List<string> GetDirectoryList(string directoryName)
            {
                IEnumerable<IListBlobItem> listBlob = GetListBlob(directoryName);
                List<string> listUri = new List<string>();
                foreach (IListBlobItem BlobItem in listBlob)
                {
                    listUri.Add(BlobItem.Uri.ToString());
                }
                return listUri;
            }

            private IEnumerable<IListBlobItem> GetListBlob(string directoryName)
            {
                bool ForceUsingDefaultContainerIfContainerNotDefined = false;
                string containerInPath = ParseContainerFromFileUri(directoryName,
                    ForceUsingDefaultContainerIfContainerNotDefined);
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
                internal string BlobName { get; set; }
                internal string BlobContainer { get; set; }
            }
        }
    }
}
