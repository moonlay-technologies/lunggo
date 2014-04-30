using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.TableStorage
{
    public class AzureTableStorageClient : ITableStorageClient
    {
        CloudTableClient _cloudTableClient;
        public void init(string connString)
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connString);
            this._cloudTableClient = cloudStorageAccount.CreateCloudTableClient();

        }

        public CloudTable GetTableByReference(string reference)
        {
            CloudTable table = _cloudTableClient.GetTableReference(reference);
            table.CreateIfNotExists();
            return table;
        }
        public void InsertEntityToTableStorage<T>(T ObjectParam, string mailReference) where T : ITableEntity, new()
        {
            try
            {
                CloudTable table = GetTableByReference(mailReference);
                TableOperation insertOp = TableOperation.Insert(ObjectParam);
                table.Execute(insertOp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
