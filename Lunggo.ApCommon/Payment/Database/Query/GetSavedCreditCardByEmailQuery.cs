using Lunggo.ApCommon.Payment.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Payment.Database.Query
{
    internal class GetSavedCreditCardByEmailQuery : DbQueryBase<GetSavedCreditCardByEmailQuery, SavedCreditCard>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "SELECT * FROM SavedCreditCard WHERE Email = @Email AND TokenExpiry > DATEADD(day, -7, GETUTCDATE())";
        }
    }
}
