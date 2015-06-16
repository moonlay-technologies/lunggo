using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Lunggo.Framework.Queue
{
    internal abstract class QueueClient
    {
        internal abstract void Init();
        internal abstract CloudQueue GetQueueByReference(QueueService.Queue reference);
        internal abstract bool CreateIfNotExists(QueueService.Queue reference);
        protected abstract string GetQueueReferenceName(QueueService.Queue reference);
    }
}
