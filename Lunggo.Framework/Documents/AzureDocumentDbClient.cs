using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Extension;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json.Linq;

namespace Lunggo.Framework.Documents
{
    public partial class DocumentService
    {
        private class AzureDocumentDbClient : DocumentClient
        {
            private static readonly AzureDocumentDbClient ClientInstance = new AzureDocumentDbClient();
            private bool _isInitialized;
            private Microsoft.Azure.Documents.Client.DocumentClient _client;
            private DocumentCollection _collection;

            private AzureDocumentDbClient()
            {

            }

            internal static AzureDocumentDbClient GetClientInstance()
            {
                return ClientInstance;
            }

            internal override void Init(string endpointUrl, string authKey, string databaseName, string collectionName)
            {
                if (!_isInitialized)
                {
                    _client = new Microsoft.Azure.Documents.Client.DocumentClient(new Uri(endpointUrl), authKey);
                    Microsoft.Azure.Documents.Database database =
                        _client.CreateDatabaseQuery("SELECT * FROM c WHERE c.id = '" + databaseName + "'")
                            .AsEnumerable().First();
                    _collection = _client.CreateDocumentCollectionQuery(database.CollectionsLink,
                        "SELECT * FROM c WHERE c.id = '" + collectionName + "'")
                        .AsEnumerable().First();
                    _isInitialized = true;
                }
            }

            internal override void Upsert(string id, object document)
            {
                UpsertAsync(id, document).Wait();
            }

            internal override void Upsert(string id, object document, TimeSpan timeToLive)
            {
                UpsertAsync(id, document, timeToLive).Wait();
            }

            internal override async Task UpsertAsync(string id, object document)
            {
                await UpsertAsync(id, document, new TimeSpan(-1));
            }

            internal override async Task UpsertAsync(string id, object document, TimeSpan timeToLive)
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.Append(document.Serialize());
                stringBuilder.Insert(1, "\"id\":\"" + id + "\",");
                if (timeToLive.Ticks > 0)
                    stringBuilder.Insert(1, "\"ttl\":" + timeToLive.TotalSeconds + ",");
                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(stringBuilder.ToString())))
                    await _client.UpsertDocumentAsync(_collection.DocumentsLink, JsonSerializable.LoadFrom<Document>(ms));
            }

            internal override void Delete(string id)
            {
                DeleteAsync(id).Wait();
            }

            internal override async Task DeleteAsync(string id)
            {
                var doc = Retrieve<Document>(id);
                await _client.DeleteDocumentAsync(doc.SelfLink);
            }

            internal override T Retrieve<T>(string id)
            {
                return RetrieveAsync<T>(id).Result;
            }

            internal override async Task<T> RetrieveAsync<T>(string id)
            {
                var query = new RetrieveQuery();
                return (await ExecuteAsync<T>(query, new { id })).SingleOrDefault();
            }

            internal override IEnumerable<T> Execute<T>(DocQueryBase query, dynamic param = null, dynamic condition = null)
            {
                return ExecuteAsync<T>(query, param, condition).Result;
            }

            internal override async Task<IEnumerable<T>> ExecuteAsync<T>(DocQueryBase query, dynamic param = null, dynamic condition = null)
            {
                return await Task.Run(() =>
                {
                    var querySpec = new SqlQuerySpec(query.GetQueryString(condition), GenerateParams(param));
                    return _client.CreateDocumentQuery<T>(_collection.SelfLink, querySpec).AsEnumerable();
                });
            }

            internal override void Execute(DocQueryBase query, dynamic param = null, dynamic condition = null)
            {
                ExecuteAsync(query, param, condition).Wait();
            }

            internal override async Task ExecuteAsync(DocQueryBase query, dynamic param = null, dynamic condition = null)
            {
                await Task.Run(() =>
                {
                    var querySpec = new SqlQuerySpec(query.GetQueryString(condition), GenerateParams(param));
                    _client.CreateDocumentQuery(_collection.SelfLink, querySpec);
                });
            }

            private static SqlParameterCollection GenerateParams(dynamic param)
            {
                var paramList =
                    ((PropertyInfo[])param.GetType().GetProperties()).Select(
                        prop =>
                        {
                            var value = prop.GetValue(param);
                            return value is IEnumerable
                                ? (value.GetType().GetElementType() == typeof (string)
                                    ? new SqlParameter("@" + prop.Name, "('" + string.Join("','", value) + "')")
                                    : new SqlParameter("@" + prop.Name, "(" + string.Join(",", value) + ")"))
                                : new SqlParameter("@" + prop.Name, value);
                        });
                var paramCollection = new SqlParameterCollection(paramList);
                return paramCollection;
            }

            private class RetrieveQuery : DocQueryBase
            {
                public override string GetQueryString(dynamic condition = null)
                {
                    return "SELECT * FROM c WHERE c.id = @id";
                }
            }
        }
    }
}
