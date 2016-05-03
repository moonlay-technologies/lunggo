using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.ApCommon.Payment.Query
{
    internal class GetRefundQuery : QueryBase<GetRefundQuery, RefundTableRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "SELECT Time, BeneficiaryBank, BeneficiaryAccount, " +
                   "RemitterBank, RemitterAccount, CurrencyCd, Rate, " +
                   "Amount, AmountIdr " +
                   "FROM Refund " +
                   "WHERE PaymentId = @Id";
        }
    }
}
