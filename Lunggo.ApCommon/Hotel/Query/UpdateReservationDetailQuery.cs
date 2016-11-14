using System.Text;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Hotel.Query
{
    internal class UpdateReservationDetailQuery : NoReturnDbQueryBase<UpdateReservationDetailQuery>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(CreateUpdateClause());
            queryBuilder.Append(CreateSetClause());
            queryBuilder.Append(CreateWhereClause());
            return queryBuilder.ToString();
        }

        private static string CreateUpdateClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"UPDATE HotelReservationDetails ");
            return clauseBuilder.ToString();
        }

        private static string CreateSetClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"SET ");
            clauseBuilder.Append(@"BookingReference = @BookingReference ");
            clauseBuilder.Append(@",ClientReference = @ClientReference ");
            clauseBuilder.Append(@",SupplierName = @SupplierName ");
            clauseBuilder.Append(@",SupplierVat = @SupplierVat ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE RsvNo = @RsvNo");
            return clauseBuilder.ToString();
        }
    }
}
