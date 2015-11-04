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

        public void Init(string connString)
        {
            if (!_isInitialized)
            {
                Client.Init(connString);
                _isInitialized = true;
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
    }
}
