using Microsoft.WindowsAzure.Storage.Queue;

namespace Lunggo.Framework.Queue
{
    internal abstract class QueueClient
    {
        internal abstract void Init(string connString);
        internal abstract CloudQueue GetQueueByReference(string reference);
        protected abstract string PreprocessQueueReferenceName(string reference);
    }
}
