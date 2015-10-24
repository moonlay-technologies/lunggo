using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Razor;
using System.Web.UI.WebControls;
using Lunggo.Framework.TableStorage;

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
        }

        public static HtmlTemplateService GetInstance()
        {
            return Instance;
        }

        public string GenerateTemplate<T>(T objectParam, string type)
        {
            return Client.GenerateTemplate(objectParam, type);
        }
    }
}
