using Microsoft.WindowsAzure.Storage.Table;

namespace Lunggo.Framework.TableStorage
{
    public partial class TableStorageService
    {
        private static readonly TableStorageService Instance = new TableStorageService();
        private bool _isInitialized;
        private static readonly AzureTableStorageClient Client = AzureTableStorageClient.GetClientInstance();

        private TableStorageService()
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

        public static TableStorageService GetInstance()
        {
            return Instance;
        }

        public CloudTable GetTableByReference(string reference)
        {
            return Client.GetTableByReference(reference);
        }
        public void InsertEntityToTableStorage<T>(T objectParam, string reference) where T : ITableEntity, new()
        {
            Client.InsertEntityToTableStorage(objectParam, reference);
        }
        public void InsertOrReplaceEntityToTableStorage<T>(T objectParam, string reference) where T : ITableEntity, new()
        {
            Client.InsertOrReplaceEntityToTableStorage(objectParam, reference);
        }
    }
}
