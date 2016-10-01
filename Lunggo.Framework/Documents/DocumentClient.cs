using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Documents
{
    internal abstract class DocumentClient
    {
        internal abstract void Init(string endpointUrl, string authKey, string databaseName, string collectionName);
        internal abstract void Upsert(string id, object document);
        internal abstract void Upsert(string id, object document, TimeSpan timeToLive);
        internal abstract Task UpsertAsync(string id, object document);
        internal abstract Task UpsertAsync(string id, object document, TimeSpan timeToLive);
        internal abstract void Delete(string id);
        internal abstract Task DeleteAsync(string id);
        internal abstract T Retrieve<T>(string id);
        internal abstract Task<T> RetrieveAsync<T>(string id);
        internal abstract IEnumerable<T> Execute<T>(DocQueryBase query, dynamic param = null, dynamic condition = null);
        internal abstract Task<IEnumerable<T>> ExecuteAsync<T>(DocQueryBase query, dynamic param = null, dynamic condition = null);
        internal abstract void Execute(DocQueryBase query, dynamic param = null, dynamic condition = null);
        internal abstract Task ExecuteAsync(DocQueryBase query, dynamic param = null, dynamic condition = null);
    }
}
