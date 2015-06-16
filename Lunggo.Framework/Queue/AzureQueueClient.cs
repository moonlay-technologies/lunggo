using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Config;
using Lunggo.Framework.Core;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using RestSharp.Extensions;

namespace Lunggo.Framework.Queue
{
    public partial class QueueService
    {
        private class AzureQueueClient : QueueClient
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

            internal override void Init()
            {
                if (!_isInitialized)
                {
                    var connString = ConfigManager.GetInstance().GetConfigValue("azureStorage", "connectionString");
                    var cloudStorageAccount = CloudStorageAccount.Parse(connString);
                    _cloudQueueClient = cloudStorageAccount.CreateCloudQueueClient();
                    foreach (var reference in Enum.GetValues(typeof(Queue)).Cast<Queue>())
                        CreateIfNotExists(reference);
                    _isInitialized = true;
                }
                else
                {
                    throw new InvalidOperationException("AzureQueueClient is already initialized");
                }
            }


            internal override CloudQueue GetQueueByReference(Queue reference)
            {
                var referenceName = GetQueueReferenceName(reference);
                var queue = _cloudQueueClient.GetQueueReference(referenceName);
                return queue;
            }

            internal override bool CreateIfNotExists(Queue reference)
            {
                var queue = GetQueueByReference(reference);
                queue.CreateIfNotExists();
                return true;
            }

            protected override string GetQueueReferenceName(Queue reference)
            {
                var referenceName = reference.ToString().ToCamelCase(CultureInfo.InvariantCulture);
                return ConfigManager.GetInstance().GetConfigValue("azureQueue", referenceName);
            }
        }
    }
}
