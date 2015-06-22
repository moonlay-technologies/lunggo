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
        internal abstract CloudQueue GetQueueByReference(Queue reference);
        internal abstract bool CreateIfNotExists(Queue reference);
        protected abstract string GetQueueReferenceName(Queue reference);
    }
}
