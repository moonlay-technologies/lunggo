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
    internal class InsertFlightItineraryQuery : QueryBase<InsertFlightItineraryQuery, FlightItineraryQueryRecord>
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

            insertClause.Append(@"BookingId, ");
            valueClause.Append(@"@BookResult.BookingId, ");

            insertClause.Append(@"BookingStatusCode, ");
            valueClause.Append(@"@BookResult.BookingStatus, ");

            insertClause.Append(@"TicketTimeLimit, ");
            valueClause.Append(@"@BookResult.TimeLimit, ");

            insertClause.Append(@"TripTypeCd, ");
            valueClause.Append(@"@TripType, ");

            insertClause.Append(@"SourceCd, ");
            valueClause.Append(@"@Itinerary.Source, ");

            insertClause.Append(@"SourcePrice, ");
            valueClause.Append(@"@SourcePrice, ");

            insertClause.Append(@"SourceCurrencyCd, ");
            valueClause.Append(@"@SourceCurrency, ");

            insertClause.Append(@"SourceIdrExchangeRate, ");
            valueClause.Append(@"@SourceExchangeRate, ");

            insertClause.Append(@"LocalPrice, ");
            valueClause.Append(@"@LocalPrice, ");
                                  
            insertClause.Append(@"LocalCurrencyCd, ");
            valueClause.Append(@"@LocalCurrency, ");
                                  
            insertClause.Append(@"LocalIdrExchangeRate, ");
            valueClause.Append(@"@LocalExchangeRate, ");

            insertClause.Append(@"IdrPrice, ");
            valueClause.Append(@"@IdrPrice, ");

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
