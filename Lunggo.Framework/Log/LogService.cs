using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Log
{
    public partial class LogService
    {
        private static readonly LogService Instance = new LogService();
        private bool _isInitialized;
        private static readonly LogClient Client = SlackLogClient.GetClientInstance();

        private LogService()
        {
            
        }

        public void Init(string apiKey)
        {
            if (!_isInitialized)
            {
                Client.Init(apiKey);
                _isInitialized = true;
            }
        }

        public static LogService GetInstance()
        {
            return Instance;
        }

        public void Post(string text)
        {
            Client.Post(text);
        }

        public void Post(string text, string recipient)
        {
            Client.Post(text, recipient);
        }
    }
}
