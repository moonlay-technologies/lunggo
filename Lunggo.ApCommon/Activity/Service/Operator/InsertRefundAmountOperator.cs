using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public void InsertRefundAmountOperator(string rsvNo)
        {
            InsertRefundAmountOperatorToDb(rsvNo);
        }
    }
}
