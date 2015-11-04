using System.Text;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.ApCommon.Flight.Database.Query
{
    internal class GetUnpaidReservationQuery : QueryBase<GetUnpaidReservationQuery, FlightReservationTableRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(CreateSelectClause());
            queryBuilder.Append(CreateWhereClause(condition));
            return queryBuilder.ToString();
        }

        private static string CreateSelectClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"SELECT RsvNo, PaymentTimeLimit, FinalPrice ");
            clauseBuilder.Append(@"FROM FlightReservation ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause(dynamic condition)
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"WHERE PaymentMethodCd = 'TRF' AND PaymentStatusCd = 'PEN'");
            return clauseBuilder.ToString();
        }
    }
}
