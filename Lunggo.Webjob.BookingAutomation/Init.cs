using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Config;
using RestSharp;

namespace Lunggo.WebJob.BookingAutomation
{
    public partial class Program
    {
        public static void Init()
        {
            InitConfigurationManager();
            InitData();
        }
        private static void InitConfigurationManager()
        {
            var configManager = ConfigManager.GetInstance();
            configManager.Init(@"");
        }

        private static void InitData()
        {
            _apiUrl = ConfigManager.GetInstance().GetConfigValue("api", "apiUrl");
            _client = new RestClient(_apiUrl);
            GetAuthAccess();
        }
    }
}
