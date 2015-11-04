using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

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
                queue.CreateIfNotExists();
                return queue;
            }

            protected override string PreprocessQueueReferenceName(string reference)
            {
                return reference.ToLower();
            }
        }
    }
}
