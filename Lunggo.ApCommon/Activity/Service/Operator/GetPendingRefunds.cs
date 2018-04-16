using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.Repository.TableRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public List<PendingRefund> GetPendingRefunds(GetPendingRefundsInput input)
        {
            var output = GetPendingRefundsFromDb(input);            
            return output;
        }
    }
}
