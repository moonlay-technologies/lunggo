using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Lunggo.Framework.Queue
{
    public interface IQueueClient
    {
        void Init();
        CloudQueue GetQueueByReference(string reference);
        bool CreateIfNotExistsQueueAndAddMessage(string reference, CloudQueueMessage message);
    }
}
