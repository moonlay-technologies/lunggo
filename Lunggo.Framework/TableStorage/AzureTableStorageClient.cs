using Lunggo.Framework.Config;
using Lunggo.Framework.Core;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            internal override void Init()
            {
                if (!_isInitialized)
                {
                    var connString = ConfigManager.GetInstance().GetConfigValue("azureStorage", "connectionString");
                    Init(connString);
                }
            }

            internal override void Init(string connString)
            {
                if (!_isInitialized)
                {
                    var cloudStorageAccount = CloudStorageAccount.Parse(connString);
                    _cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
                    foreach (var tableStorage in Enum.GetValues(typeof (TableStorage)).Cast<TableStorage>())
                        CreateIfNotExist(tableStorage);
                    _isInitialized = true;
                }
            }

            internal override CloudTable GetTableByReference(TableStorage reference)
            {
                var referenceName = GetTableReferenceName(reference);
                var table = _cloudTableClient.GetTableReference(referenceName);
                return table;
            }

            protected override bool CreateIfNotExist(TableStorage reference)
            {
                var table = GetTableByReference(reference);
                return table.CreateIfNotExists();
            }

            protected override string GetTableReferenceName(TableStorage reference)
            {
                return reference.ToString().ToLower();
            }

            internal override void InsertEntityToTableStorage<T>(T objectParam, TableStorage reference)
            {
                var table = GetTableByReference(reference);
                var insertOp = TableOperation.Insert(objectParam);
                table.Execute(insertOp);
            }

            internal override void InsertOrReplaceEntityToTableStorage<T>(T objectParam, TableStorage reference)
            {
                var table = GetTableByReference(reference);
                var insertOp = TableOperation.InsertOrReplace(objectParam);
                table.Execute(insertOp);
            }
        }
    }
}
