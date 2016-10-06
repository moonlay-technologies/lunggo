using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;

namespace Lunggo.Framework.Documents
{
    public partial class DocumentService
    {
        private static readonly DocumentService Instance = new DocumentService();
        private bool _isInitialized;
        private static readonly DocumentClient Client = AzureDocumentDbClient.GetClientInstance();
        
        private DocumentService()
        {
            
        }

        public void Init(string endpointUrl, string authKey, string databaseName, string collectionName)
        {
            if (!_isInitialized)
            {
                Client.Init(endpointUrl, authKey, databaseName, collectionName);
                _isInitialized = true;
            }
        }

        public static DocumentService GetInstance()
        {
            return Instance;
        }

        public void Upsert(string id, object document)
        {
            Client.Upsert(id, document);
        }

        public async Task UpsertAsync(string id, object document)
        {
            await Client.UpsertAsync(id, document);
        }

        public void Upsert(string id, object document, TimeSpan timeToLive)
        {
            Client.Upsert(id, document, timeToLive);
        }

        public async Task UpsertAsync(string id, object document, TimeSpan timeToLive)
        {
            await Client.UpsertAsync(id, document, timeToLive);
        }

        public void Delete(string id)
        {
            Client.Delete(id);
        }

        public async Task DeleteAsync(string id)
        {
            await Client.DeleteAsync(id);
        }

        public T Retrieve<T>(string id)
        {
            return Client.Retrieve<T>(id);
        }

        public async Task<T> RetrieveAsync<T>(string id)
        {
            return await Client.RetrieveAsync<T>(id);
        }

        public IEnumerable<T> Execute<T>(DocQueryBase query, dynamic param = null, dynamic condition = null)
        {
            return Client.Execute<T>(query, param, condition);
        }

        public async Task<IEnumerable<T>> ExecuteAsync<T>(DocQueryBase query, dynamic param = null, dynamic condition = null)
        {
            return await Client.ExecuteAsync<T>(query, param, condition);
        }

        public void Execute(DocQueryBase query, dynamic param = null, dynamic condition = null)
        {
            Client.Execute(query, param, condition);
        }

        public async Task ExecuteAsync(DocQueryBase query, dynamic param = null, dynamic condition = null)
        {
            await Client.ExecuteAsync(query, param, condition);
        }
    }
}
