using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Query.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Query
{
    internal class InsertFlightReservationQuery : QueryBase<InsertFlightReservationQuery, FlightReservationQueryRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(CreateInsertValueClause());
            return queryBuilder.ToString();
        }

        private static string CreateInsertValueClause()
        {
            var insertClause = new StringBuilder();
            var valueClause = new StringBuilder();

            insertClause.Append(@"INSERT INTO FlightReservation (");
            valueClause.Append(@"VALUES (");

            insertClause.Append(@"RsvNo, ");
            valueClause.Append(@"@RsvNo, ");

            insertClause.Append(@"RsvTime, ");
            valueClause.Append(@"@RsvTime, ");

            insertClause.Append(@"RsvStatusCd, ");
            valueClause.Append(@"@RsvStatusCode, ");

            insertClause.Append(@"ContactName, ");
            valueClause.Append(@"@ContactData.Name, ");

            insertClause.Append(@"ContactEmail, ");
            valueClause.Append(@"@ContactData.Email, ");

            insertClause.Append(@"ContactPhone, ");
            valueClause.Append(@"@ContactData.Phone, ");

            insertClause.Append(@"ContactAddress, ");
            valueClause.Append(@"@ContactData.Address, ");

            insertClause.Append(@"PaymentMethodCd, ");
            valueClause.Append(@"' ', ");

            insertClause.Append(@"PaymentStatusCd, ");
            valueClause.Append(@"' ', ");

            insertClause.Append(@"LangCd, ");
            valueClause.Append(@"@LanguageCode, ");

            insertClause.Append(@"CancellationTypeCd, ");
            valueClause.Append(@"' ', ");

            insertClause.Append(@"TotalSourcePrice, ");
            valueClause.Append(@"@PriceData.TotalSourcePrice, ");

            insertClause.Append(@"PaymentFeeForCust, ");
            valueClause.Append(@"@PriceData.PaymentFeeForCustomer, ");

            insertClause.Append(@"PaymentFeeForUs, ");
            valueClause.Append(@"@PriceData.PaymentFeeForUs, ");

            insertClause.Append(@"GrossProfit, ");
            valueClause.Append(@"@PriceData.GrossProfit, ");

            insertClause.Append(@"InsertBy, ");
            valueClause.Append(@"' ', ");

            insertClause.Append(@"InsertDate, ");
            valueClause.Append(@"07/07/2015, ");

            insertClause.Append(@"InsertPgId, ");
            valueClause.Append(@"' ', ");

            insertClause.Remove(insertClause.Length - 2, 2);
            valueClause.Remove(insertClause.Length - 2, 2);

            insertClause.Append(@") ");
            valueClause.Append(@")");

            return insertClause.Append(valueClause).ToString();
        }
    }
}
