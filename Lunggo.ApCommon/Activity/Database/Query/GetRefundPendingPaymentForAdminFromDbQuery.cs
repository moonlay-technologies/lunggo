using Lunggo.ApCommon.Activity.Model;
using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    internal class GetRefundPendingPaymentForAdminFromDbQuery : DbQueryBase<GetRefundPendingPaymentForAdminFromDbQuery, PendingPayment>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "SELECT rc.RsvNo AS RsvNo, " +
                   "rrba.OwnerName AS Name, " +
                   "rc.RefundAmount AS Amount, " +
                   "rc.RefundProcessDate AS Date, " +
                   "rrba.AccountNumber AS AccountNumber," +
                   "rrba.BankName AS BankName " +
                   "FROM RefundCustomer AS rc " +
                   "INNER JOIN RsvRefundBankAccount AS rrba ON rc.RsvNo = rrba.RsvNo " +
                   "WHERE rc.RefundProcessDate < @Date AND rc.RefundStatus = 'Pending'";
        }
    }
}
