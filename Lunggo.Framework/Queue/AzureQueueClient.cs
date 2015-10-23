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

            internal override void Init(string connString)
            {
                if (!_isInitialized)
                {
                    var cloudStorageAccount = CloudStorageAccount.Parse(connString);
                    _cloudQueueClient = cloudStorageAccount.CreateCloudQueueClient();
                    _isInitialized = true;
                }
            }


            internal override CloudQueue GetQueueByReference(string reference)
            {
                var referenceName = PreprocessQueueReferenceName(reference);
                var queue = _cloudQueueClient.GetQueueReference(referenceName);
                CreateIfNotExists(reference);
                return queue;
            }

            internal override bool CreateIfNotExists(string reference)
            {
                var queue = GetQueueByReference(reference);
                return queue.CreateIfNotExists();
            }

            protected override string PreprocessQueueReferenceName(string reference)
            {
                return reference.ToLower();
            }
        }
    }
}
