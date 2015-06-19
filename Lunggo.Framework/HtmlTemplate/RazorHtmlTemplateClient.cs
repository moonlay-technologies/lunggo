using System;
using System.Linq;
using Lunggo.Framework.Config;
using Lunggo.Framework.Mail;
using Lunggo.Framework.TableStorage;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace Lunggo.Framework.HtmlTemplate
{
    internal class RazorHtmlTemplateClient : HtmlTemplateClient
    {
        private static readonly RazorHtmlTemplateClient ClientInstance = new RazorHtmlTemplateClient();
        private bool _isInitialized;

        private const string MailTable = @"htmlTemplate";
        private const string RowKey = @"default";

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
                TableStorageService.GetInstance().Init();
                _isInitialized = true;
            }
        }

        internal override string GenerateTemplate<T>(T objectParam , HtmlTemplateType type)
        {
            var template = GetTemplateByPartitionKey(type.ToString());
            var razorConfig = new TemplateServiceConfiguration
            {
                DisableTempFileLocking = true,
                CachingProvider = new DefaultCachingProvider(t => { })
            };
            var razorService = RazorEngineService.Create(razorConfig);
            razorService.AddTemplate(type.ToString(), template);
            var result = razorService.RunCompile(type.ToString(), model: objectParam);
            return result;
        }

        private string GetTemplateByPartitionKey(string partitionKey)
        {
            var table = TableStorageService.GetInstance().GetTableByReference(MailTable);
            var query = (from tabel in table.CreateQuery<MailTemplateModel>()
                         where tabel.PartitionKey == partitionKey && tabel.RowKey == RowKey
                         select tabel).FirstOrDefault();
            var mailTemplate = (query != null) ? query.Template : null;

            return mailTemplate;
        }
    }
}
