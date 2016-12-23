using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.ApCommon.Product.Query
{
    internal class GetAllOrderMarginsQuery : DbQueryBase<GetAllOrderMarginsQuery, MarginTableRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "SELECT RuleId, Name, Description, Coefficient, Constant, Currency, IsFlat, IsActive " +
                   "FROM Margin " +
                   "WHERE RuleId LIKE @ProductType + '%'";
        }
    }
}
