using Lunggo.ApCommon.Activity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public List<PendingPayment> GetPendingPaymentForAdmin()
        {
            var output = GetPendingPaymentForAdminFromDb();
            return output;
        }
    }
}
