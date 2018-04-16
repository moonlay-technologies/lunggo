using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.ApCommon.Payment.Query
{
    internal class GetTransactionsQuery : DbQueryBase<GetTransactionsQuery, TransactionJournalTableRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var getAccountNo = "(SELECT AccountNo FROM AccountUser WHERE UserId = @userId)";
            return "SELECT * FROM TransactionJournal WHERE ToAccountNo = " + getAccountNo + " AND Time >= @fromDate AND Time <= toDate";
        }
    }
}
