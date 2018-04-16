using Lunggo.ApCommon.Activity.Model.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public GetTransactionStatementOutput GetTransactionStatement(string operatorId, DateTime startDate, DateTime endDate)
        {
            if(startDate.Equals(value: default(DateTime)))
            {
                endDate = DateTime.UtcNow;
                startDate = endDate.AddDays(-30);
            }
            var getTransactionStatementOutput = GetTransactionStatementFromDb(operatorId, startDate, endDate);
            return getTransactionStatementOutput;    
        }
    }
}
