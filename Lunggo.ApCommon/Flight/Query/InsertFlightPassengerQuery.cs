using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Query
{
    internal class InsertFlightPassengerQuery : QueryBase<InsertFlightPassengerQuery, PassengerFareInfo>
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

            insertClause.Append(@"ItineraryId, ");
            valueClause.Append(@"@ItineraryId, ");

            insertClause.Append(@"TitleCd, ");
            valueClause.Append(@"@Passenger.Title, ");

            insertClause.Append(@"FirstName, ");
            valueClause.Append(@"@Passenger.FirstName, ");

            insertClause.Append(@"LastName, ");
            valueClause.Append(@"@Passenger.LastName, ");

            insertClause.Append(@"BirthDate, ");
            valueClause.Append(@"@Passenger.DateOfBirth, ");

            insertClause.Append(@"Gender, ");
            valueClause.Append(@"@Passenger.Gender, ");

            insertClause.Append(@"PassengerTypeCd, ");
            valueClause.Append(@"@Passenger.Type, ");

            insertClause.Append(@"CountryCd, ");
            valueClause.Append(@"@Passenger.PassportCountry, ");

            insertClause.Append(@"PassportOrIdCardNo, ");
            valueClause.Append(@"@Passenger.PassportOrIdNumber, ");

            insertClause.Append(@"PassportExpiryDate, ");
            valueClause.Append(@"@Passenger.PassportExpiryDate, ");

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
