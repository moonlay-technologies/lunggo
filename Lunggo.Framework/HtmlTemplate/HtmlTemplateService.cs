using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Razor;

namespace Lunggo.Framework.HtmlTemplate
{
    public class HtmlTemplateService
    {
        private static readonly HtmlTemplateService Instance = new HtmlTemplateService();
        private bool _isInitialized;
        private static readonly RazorHtmlTemplateClient Client = RazorHtmlTemplateClient.GetClientInstance();

        private HtmlTemplateService()
        {
            
        }

        public void Init()
        {
            if (!_isInitialized)
            {
                Client.Init();
                _isInitialized = true;
            }
            else
            {
                throw new InvalidOperationException("HtmlTemplateService is already initialized");
            }
        }

        public static HtmlTemplateService GetInstance()
        {
            return Instance;
        }

        public string GenerateTemplate<T>(T objectParam, HtmlTemplateType type)
        {
            return Client.GenerateTemplate(objectParam, type);
        }
    }
}
