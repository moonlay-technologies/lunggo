using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace Lunggo.CloudApp.EticketHandler
{
    public partial class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent _runCompleteEvent = new ManualResetEvent(false);

        public override void Run()
        {
            Trace.TraceInformation("Lunggo.CloudApp.EticketHandler is running");

            try
            {
                RunAsync(_cancellationTokenSource.Token).Wait();
            }
            finally
            {
                _runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            var result = base.OnStart();

            Trace.TraceInformation("Lunggo.CloudApp.EticketHandler has been started");

            Init();

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("Lunggo.CloudApp.EticketHandler is stopping");

            _cancellationTokenSource.Cancel();
            _runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("Lunggo.CloudApp.EticketHandler has stopped");
        }

        private static async Task RunAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await ProcessEticketQueue.ProcessQueue();
                await ProcessHotelEticketQueue.ProcessQueue();
                await ProcessChangedEticketQueue.ProcessQueue();
                await Task.Delay(1000, cancellationToken);
            }
        }
    }
}
