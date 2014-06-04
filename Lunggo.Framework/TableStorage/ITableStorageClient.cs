using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.TableStorage
{
    public interface ITableStorageClient
    {
        void init(string connString);
        CloudTable GetTableByReference(string reference);
        void InsertEntityToTableStorage<T>(T ObjectParam, string NameReference) where T : ITableEntity, new();
        void InsertOrReplaceEntityToTableStorage<T>(T ObjectParam, string NameReference) where T : ITableEntity, new();
    }
}
