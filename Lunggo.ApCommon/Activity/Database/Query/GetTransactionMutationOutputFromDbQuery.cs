using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    internal class GetTransactionStatementOutputFromDbQuery : DbQueryBase<GetTransactionStatementOutputFromDbQuery, TransactionStatement>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "SELECT TrxNo AS TrxNo, Remarks AS Remarks, DateTime AS DateTime, Amount AS Amount FROM TransactionStatement WHERE OperatorId = @OperatorId AND [DateTime] BETWEEN @StartDate AND @EndDate";
        }
    }
}
