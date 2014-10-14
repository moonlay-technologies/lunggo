using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Core;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Lunggo.Framework.Queue
{
    public class QueueService
    {
        IQueueClient _cloudQueueClient;
        private static QueueService instance = new QueueService();


        private QueueService()
        {
            
        }
        public void Init(IQueueClient client)
        {
            _cloudQueueClient = client;
        }
        public static QueueService GetInstance()
        {
            return instance;
        }
        public CloudQueue GetQueueByReference(string reference)
        {
            try
            {
                return _cloudQueueClient.GetQueueByReference(reference);
            }
            catch (Exception ex)
            {
                LunggoLogger.Error(ex.Message, ex);
                throw;
            }
        }
        public bool CreateIfNotExistsQueueAndAddMessage(string reference, CloudQueueMessage message)
        {
            try
            {
                return _cloudQueueClient.CreateIfNotExistsQueueAndAddMessage(reference, message);
            }
            catch (Exception ex)
            {
                LunggoLogger.Error(ex.Message, ex);
                throw;
            }
        }
    }
}
