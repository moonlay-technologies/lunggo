using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void Init()
        {
            if (!_isInitialized)
            {
                Client.Init();
                _isInitialized = true;
            }
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

        public CloudTable GetTableByReference(TableStorage reference)
        {
            return Client.GetTableByReference(reference);
        }
        public void InsertEntityToTableStorage<T>(T objectParam, TableStorage reference) where T : ITableEntity, new()
        {
            Client.InsertEntityToTableStorage(objectParam, reference);
        }
        public void InsertOrReplaceEntityToTableStorage<T>(T objectParam, TableStorage reference) where T : ITableEntity, new()
        {
            Client.InsertOrReplaceEntityToTableStorage(objectParam, reference);
        }
    }
}
