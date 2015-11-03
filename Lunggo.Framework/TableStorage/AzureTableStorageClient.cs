using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lunggo.Framework.TableStorage
{
    public partial class TableStorageService {
        private class AzureTableStorageClient : TableStorageClient
        {
            private static readonly AzureTableStorageClient ClientInstance = new AzureTableStorageClient();
            private bool _isInitialized;
            private CloudTableClient _cloudTableClient;

            private AzureTableStorageClient()
            {
                
            }

            internal static AzureTableStorageClient GetClientInstance()
            {
                return ClientInstance;
            }

            internal override void Init(string connString)
            {
                if (!_isInitialized)
                {
                    var cloudStorageAccount = CloudStorageAccount.Parse(connString);
                    _cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
                    _isInitialized = true;
                }
            }

            internal override CloudTable GetTableByReference(string reference)
            {
                var referenceName = PreprocessTableReferenceName(reference);
                var table = _cloudTableClient.GetTableReference(referenceName);
                table.CreateIfNotExists();
                return table;
            }

            protected override string PreprocessTableReferenceName(string reference)
            {
                return reference.ToLower();
            }

            internal override void InsertEntityToTableStorage<T>(T objectParam, string reference)
            {
                var table = GetTableByReference(reference);
                var insertOp = TableOperation.Insert(objectParam);
                table.Execute(insertOp);
            }

            internal override void InsertOrReplaceEntityToTableStorage<T>(T objectParam, string reference)
            {
                var table = GetTableByReference(reference);
                var insertOp = TableOperation.InsertOrReplace(objectParam);
                table.Execute(insertOp);
            }
        }
    }
}
