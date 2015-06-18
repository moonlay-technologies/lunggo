using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Core;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Lunggo.Framework.Queue
{
    public partial class QueueService
    {
        private static readonly QueueService Instance = new QueueService();
        private bool _isInitialized;
        private static readonly AzureQueueClient Client = AzureQueueClient.GetClientInstance();

        private QueueService()
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
                throw new InvalidOperationException("QueueService is already initialized");
            }
        }
        public static QueueService GetInstance()
        {
            return Instance;
        }
        public CloudQueue GetQueueByReference(string reference)
        {
            return Client.GetQueueByReference(reference);
        }
        public bool CreateIfNotExistsQueueAndAddMessage(string reference, CloudQueueMessage message)
        {
            return Client.CreateIfNotExistsQueueAndAddMessage(reference, message);
        }
    }
}
