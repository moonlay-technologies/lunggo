using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace Lunggo.FunctionTest
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static void Run([QueueTrigger("functiontest", Connection = "DefaultEndpointsProtocol=https;AccountName=travoramalocal;AccountKey=t9BOHU0NktEB4qvBd7eSdXtSYabT/wDxnC2PndRtDNdQWymLUko6q0oKGICBZ0FoX7GLvGV9v4QSNYZPu98ZWw==;EndpointSuffix=core.windows.net")]string myQueueItem, TraceWriter log)
        {
            log.Info($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
