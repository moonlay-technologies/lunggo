using Lunggo.ApCommon.Activity.Model;
using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    internal class GetPaymentStepPendingPaymentForAdminFromDbQuery : DbQueryBase<GetPaymentStepPendingPaymentForAdminFromDbQuery, PendingPayment>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "SELECT DISTINCT arsc.RsvNo AS RsvNo, " +
                   "uba.OwnerName AS Name, " +
                   "arsc.StepAmount AS Amount, " +
                   "arsc.StepDate AS Date, " +
                   "uba.AccountNumber AS AccountNumber, " +
                   "arsc.StepName AS PaymentStatus," +
                   "uba.BankName AS BankName " +
                   "FROM (( ActivityReservationStepOperator AS arsc INNER JOIN Operator AS op ON op.ActivityId = arsc.ActivityId) " +
                   "INNER JOIN UserBankAccount AS uba ON uba.UserId = op.UserId) " +
                   "WHERE arsc.StepDate < @Date AND arsc.StepStatus = 'false'";
        }
    }
}
