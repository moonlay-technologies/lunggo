using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    internal class InsertTransactionStatementToDbQuery : NoReturnDbQueryBase<InsertTransactionStatementToDbQuery>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "INSERT INTO TransactionStatement (TrxNo, Remarks, DateTime, Amount, OperatorId) VALUES (@TrxNo, @Remarks, @DateTime, @Amount, @OperatorId)";
        }
    }
}
