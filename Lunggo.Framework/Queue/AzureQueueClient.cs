using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Config;
using Lunggo.Framework.Core;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Lunggo.Framework.Queue
{
    public partial class QueueService
    {
        private class AzureQueueClient : IQueueClient
        {
            private static readonly AzureQueueClient ClientInstance = new AzureQueueClient();
            private bool _isInitialized;
            private CloudQueueClient _cloudQueueClient;

            private AzureQueueClient()
            {

            }

            internal static AzureQueueClient GetClientInstance()
            {
                return ClientInstance;
            }

            public void Init()
            {
                if (!_isInitialized)
                {
                    var connString = ConfigManager.GetInstance().GetConfigValue("azurestorage", "connectionString");
                    var cloudStorageAccount = CloudStorageAccount.Parse(connString);
                    _cloudQueueClient = cloudStorageAccount.CreateCloudQueueClient();
                    _isInitialized = true;
                }
                else
                {
                    throw new InvalidOperationException("AzureQueueClient is already initialized");
                }
            }


            public CloudQueue GetQueueByReference(string reference)
            {
                var queue = _cloudQueueClient.GetQueueReference(reference);
                return queue;
            }

            public bool CreateIfNotExistsQueueAndAddMessage(string reference, CloudQueueMessage message)
            {
                var queue = GetQueueByReference(reference);
                queue.CreateIfNotExists();
                queue.AddMessage(message);
                return true;
            }
        }
    }
}
