using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public bool MarkPendingPaymentAsFailed(string rsvNo, string pendingPaymentStatus)
        {
            var output = MarkPendingPaymentAsFailedDb(rsvNo, pendingPaymentStatus);
            return output;
        }
    }
}
