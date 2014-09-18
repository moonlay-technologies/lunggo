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
        private static readonly TableStorageService Instance = new TableStorageService();


        private TableStorageService()
        {
            
        }
        public void init(ITableStorageClient client)
        {
            _tableStorageClient = client;
        }
        public static TableStorageService GetInstance()
        {
            return Instance;
        }
        public CloudTable GetTableByReference(string reference)
        {
            try
            {
                return _tableStorageClient.GetTableByReference(reference);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public void InsertEntityToTableStorage<T>(T objectParam, string nameReference) where T : ITableEntity, new()
        {
            try
            {
                _tableStorageClient.InsertEntityToTableStorage(objectParam, nameReference);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void InsertOrReplaceEntityToTableStorage<T>(T objectParam, string nameReference) where T : ITableEntity, new()
        {
            try
            {
                _tableStorageClient.InsertOrReplaceEntityToTableStorage(objectParam, nameReference);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
