using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Campaign.Database.Query
{
    internal class GetVoucherRecipientsRecordQuery : QueryBase<GetVoucherRecipientsRecordQuery, VoucherRecipientsTableRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(CreateSelectClause());
            queryBuilder.Append(CreateWhereClause());
            return queryBuilder.ToString();
        }

        private static string CreateSelectClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("SELECT * ");
            clauseBuilder.Append("FROM [VoucherRecipients] ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE VoucherCode = @VoucherCode ");
            clauseBuilder.Append("AND Email = @Email");
            return clauseBuilder.ToString();
        }
    }
}
