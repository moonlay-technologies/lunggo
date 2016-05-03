using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.ProductBase.Model;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.ApCommon.ProductBase.Query
{
    internal class GetAllOrderMarginsQuery : QueryBase<GetAllOrderMarginsQuery, MarginTableRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "SELECT RuleId, Name, Description, Coefficient, Constant, Currency, IsFlat, IsActive " +
                   "FROM Margin " +
                   "WHERE RuleId LIKE @ProductType + '%'";
        }
    }
}
