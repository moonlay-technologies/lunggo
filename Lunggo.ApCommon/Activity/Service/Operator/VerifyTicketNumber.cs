using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public bool VerifyTicketNumber(long activityId, string ticketNumber)
        {
            var output = VerifyTicketNumberDb(activityId, ticketNumber);
            return output;
        }
    }
}
