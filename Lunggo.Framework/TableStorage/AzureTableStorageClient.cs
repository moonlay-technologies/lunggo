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
                else
                {
                    throw new InvalidOperationException("AzureTableStorageClient is already initialized");
                }
            }

            internal override void Init(string connString)
            {
                if (!_isInitialized)
                {
                    var cloudStorageAccount = CloudStorageAccount.Parse(connString);
                    _cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
                    _isInitialized = true;
                }
                else
                {
                    throw new InvalidOperationException("AzureTableStorageClient is already initialized");
                }
            }

            internal override CloudTable GetTableByReference(string reference)
            {
                var table = _cloudTableClient.GetTableReference(reference);
                return table;
            }

            private CloudTable GetTableByReferenceAndCreateIfNotExist(string reference)
            {
                var table = _cloudTableClient.GetTableReference(reference);
                table.CreateIfNotExists();
                return table;
            }

            internal override void InsertEntityToTableStorage<T>(T objectParam, string nameReference)
            {
                var table = GetTableByReferenceAndCreateIfNotExist(nameReference);
                var insertOp = TableOperation.Insert(objectParam);
                table.Execute(insertOp);
            }

            internal override void InsertOrReplaceEntityToTableStorage<T>(T objectParam, string nameReference)
            {
                var table = GetTableByReferenceAndCreateIfNotExist(nameReference);
                var insertOp = TableOperation.InsertOrReplace(objectParam);
                table.Execute(insertOp);
            }
        }
    }
}
