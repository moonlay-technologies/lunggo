using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Lunggo.Framework.Queue
{
    public class AzureQueueClient : IQueueClient
    {
        CloudQueueClient _cloudQueueClient;
        public void init(string connString)
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connString);
            this._cloudQueueClient = cloudStorageAccount.CreateCloudQueueClient();
        }
        public AzureQueueClient()
        {
            
        }
        public CloudQueue GetQueueByReference(string reference)
        {
            try
            {
                CloudQueue queue = _cloudQueueClient.GetQueueReference(reference);
                return queue;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool CreateIfNotExistsQueueAndAddMessage(string reference, CloudQueueMessage message)
        {
            try
            {
                CloudQueue queue = GetQueueByReference(reference);
                queue.AddMessage(message);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
