using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.TableStorage
{
    public class TableStorageService
    {
        ITableStorageClient _tableStorageClient;
        private static TableStorageService instance = new TableStorageService();


        private TableStorageService()
        {
            
        }
        public void init(ITableStorageClient client)
        {
            _tableStorageClient = client;
        }
        public static TableStorageService GetInstance()
        {
            return instance;
        }
        public CloudTable GetTableByReference(string reference)
        {
            try
            {
                return _tableStorageClient.GetTableByReference(reference);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void InsertEntityToTableStorage<T>(T ObjectParam, string NameReference) where T : ITableEntity, new()
        {
            try
            {
                _tableStorageClient.InsertEntityToTableStorage(ObjectParam, NameReference);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void InsertOrReplaceEntityToTableStorage<T>(T ObjectParam, string NameReference) where T : ITableEntity, new()
        {
            try
            {
                _tableStorageClient.InsertOrReplaceEntityToTableStorage(ObjectParam, NameReference);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
