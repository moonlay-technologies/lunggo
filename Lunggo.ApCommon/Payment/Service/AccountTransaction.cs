using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Query;
using Lunggo.ApCommon.Sequence;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Payment.Service
{
    public partial class PaymentService
    {
        public AccountBalance GetBalance()
        {
            if (!HttpContext.Current.User.Identity.IsUserAuthorized())
                return null;

            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var userId = HttpContext.Current.User.Identity.GetId();
                var balanceRecord = GetBalanceQuery.GetInstance().Execute(conn, new {userId}).Single();
                var balance = ConvertToAccountBalance(balanceRecord);
                return balance;
            }
        }

        public List<Transaction> GetTransactions(DateTime fromDate, DateTime toDate)
        {
            if (!HttpContext.Current.User.Identity.IsUserAuthorized())
                return null;

            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var userId = HttpContext.Current.User.Identity.GetId();
                var transactionRecords = GetTransactionsQuery.GetInstance().Execute(conn, new {userId, fromDate, toDate});
                var transactions = transactionRecords.Select(ConvertToTransaction).ToList();
                return transactions;
            }
        }

        internal bool CreateAccountIfNotExist(string userId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var accountUser = GetAccountUserQuery.GetInstance().Execute(conn, new {userId});
                if (accountUser.Any())
                    return true;

                var accountNo = AccountNoSequence.GetInstance().GetNext();
                CreateAccountByUserQuery.GetInstance().Execute(conn, new {accountNo, userId});
                return true;
            }
            return false;
        }
    }
}
