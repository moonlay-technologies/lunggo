using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using Lunggo.Framework.BlobStorage;
using Lunggo.Framework.SharedModel;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using FileInfo = Lunggo.Framework.SharedModel.FileInfo;

namespace Lunggo.Framework.SnowMaker
{
    public class BlobOptimisticDataStore : IOptimisticDataStore
    {
        const string DefaultSeedValue = "1";

        readonly string _blobContainer;

        readonly IDictionary<string, ICloudBlob> _blobReferences;
        readonly object _blobReferencesLock = new object();

        public Func<String, long> SeedValueInitializer { get; set; }

        public BlobOptimisticDataStore(string containerName)
        {
            _blobContainer = containerName;
            _blobReferences = new Dictionary<string, ICloudBlob>();
        }

        public string GetData(string blockName)
        {
            var blobReference = GetBlobReference(blockName);
            using (var stream = new MemoryStream())
            {
                blobReference.DownloadToStream(stream);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        public bool TryOptimisticWrite(string scopeName, string data)
        {
            try
            {
                UploadText(
                    scopeName,
                    data);
            }
            catch (StorageException exc)
            {
                if (exc.RequestInformation.HttpStatusCode == (int)HttpStatusCode.PreconditionFailed)
                    return false;

                throw;
            }
            return true;
        }

        ICloudBlob GetBlobReference(string blockName)
        {
            return _blobReferences.GetValue(
                blockName,
                _blobReferencesLock,
                () => InitializeBlobReference(blockName));
        }

        private ICloudBlob InitializeBlobReference(string blockName)
        {
            var blobContainer = BlobStorageService.GetInstance().GetBlobContainer(_blobContainer);
            var blobReference = blobContainer.GetBlockBlobReference(blockName);

            if (blobReference.Exists())
                return blobReference;

            var seedValue = SeedValueInitializer == null ? DefaultSeedValue : SeedValueInitializer.Invoke(blockName).ToString(CultureInfo.InvariantCulture);

            try
            {
                UploadText(blockName, seedValue);
            }
            catch (StorageException uploadException)
            {
                if (uploadException.RequestInformation.HttpStatusCode != (int)HttpStatusCode.Conflict)
                    throw;
            }

            return blobReference;
        }

        void UploadText(string scopeName, string text)
        {
            BlobStorageService.GetInstance().WriteFileToBlob(new BlobWriteDto
            {
                FileBlobModel = new FileBlobModel
                {
                    FileInfo = new FileInfo
                    {
                        ContentType = "text/plain",
                        FileName = scopeName,
                        FileData = Encoding.UTF8.GetBytes(text)
                    },
                    Container = _blobContainer
                },
                SaveMethod = SaveMethod.Force
            });
        }
    }
}
