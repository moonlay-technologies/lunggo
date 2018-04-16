using System.Linq;
using Lunggo.Framework.Mail;
using Lunggo.Framework.TableStorage;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using System;

namespace Lunggo.Framework.HtmlTemplate
{
    internal class RazorHtmlTemplateClient : HtmlTemplateClient
    {
        private static readonly RazorHtmlTemplateClient ClientInstance = new RazorHtmlTemplateClient();
        private bool _isInitialized;

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
                _isInitialized = true;
            }
        }

        internal override string GenerateTemplate<T>(T objectParam, string template)
        {
            var typeName = Guid.NewGuid().ToString("N");
            var razorConfig = new TemplateServiceConfiguration
            {
                DisableTempFileLocking = true,
                CachingProvider = new DefaultCachingProvider(t => { }),
            };
            var razorService = RazorEngineService.Create(razorConfig);
            razorService.AddTemplate(typeName, template);

            var result = razorService.RunCompile("typeName", model: objectParam);
            return result;
        }

        internal string GenerateTemplateFromTable<T>(T objectParam , string type)
        {
            var typeName = PreprocessTemplateType(type);
            var template = GetTemplateByPartitionKey(typeName);
            var razorConfig = new TemplateServiceConfiguration
            {
                DisableTempFileLocking = true,
                CachingProvider = new DefaultCachingProvider(t => { }),
            };
            var razorService = RazorEngineService.Create(razorConfig);
            razorService.AddTemplate(typeName, template);
            
            var result = razorService.RunCompile(typeName, model: objectParam);
            return result;
        }

        private string PreprocessTemplateType(string type)
        {
            return type.ToLower();
        }

        private string GetTemplateByPartitionKey(string partitionKey)
        {
            var table = TableStorageService.GetInstance().GetTableByReference("HtmlTemplate");
            var query = (from tabel in table.CreateQuery<MailTemplateModel>()
                         where tabel.PartitionKey == partitionKey && tabel.RowKey == RowKey
                         select tabel).FirstOrDefault();
            var mailTemplate = (query != null) ? query.Template : null;

            return mailTemplate;
        }
    }
}
