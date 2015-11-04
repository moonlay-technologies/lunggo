using Microsoft.WindowsAzure.Storage.Table;

namespace Lunggo.Framework.TableStorage
{
    internal abstract class TableStorageClient
    {
        internal abstract void Init(string connString);
        internal abstract CloudTable GetTableByReference(string reference);
        protected abstract string PreprocessTableReferenceName(string reference);
        internal abstract void InsertEntityToTableStorage<T>(T objectParam, string reference) where T : ITableEntity, new();
        internal abstract void InsertOrReplaceEntityToTableStorage<T>(T objectParam, string reference) where T : ITableEntity, new();
    }
}
