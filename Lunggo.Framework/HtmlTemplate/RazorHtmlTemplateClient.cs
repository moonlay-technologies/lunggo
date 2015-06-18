using System;
using System.Linq;
using Lunggo.Framework.Config;
using Lunggo.Framework.Mail;
using Lunggo.Framework.TableStorage;
using RazorEngine;

namespace Lunggo.Framework.HtmlTemplate
{
    internal class RazorHtmlTemplateClient : HtmlTemplateClient
    {
        private static readonly RazorHtmlTemplateClient ClientInstance = new RazorHtmlTemplateClient();
        private bool _isInitialized;
        private string _mailTable;
        private string _rowKey;

        private RazorHtmlTemplateClient()
        {
            
        }

        internal static RazorHtmlTemplateClient GetClientInstance()
        {
            return ClientInstance;
        }

        internal override void Init()
        {
            if (!_isInitialized)
            {
                try
                {
                    var tableStorageService = TableStorageService.GetInstance();
                    tableStorageService.Init();
                }
                catch
                {
                    
                }
                _mailTable = ConfigManager.GetInstance().GetConfigValue("mandrill", "mailTableName");
                _rowKey = ConfigManager.GetInstance().GetConfigValue("mandrill", "mailRowName");
                _isInitialized = true;
            }
            else
            {
                throw new InvalidOperationException("RazorTemplateClient is already initialized");
            }
        }

        internal override string GenerateTemplate<T>(T objectParam , HtmlTemplateType type)
        {
            var template = GetTemplateByPartitionKey(type.ToString());
            var result = Razor.Parse(template, objectParam, type.ToString());
            return result;
        }

        private string GetTemplateByPartitionKey(string partitionKey)
        {
            var table = TableStorageService.GetInstance().GetTableByReference(this._mailTable);
            var query = (from tabel in table.CreateQuery<MailTemplateModel>()
                         where tabel.PartitionKey == partitionKey && tabel.RowKey == _rowKey
                         select tabel).FirstOrDefault();
            var mailTemplate = (query != null) ? query.Template : null;

            return mailTemplate;
        }
    }
}
