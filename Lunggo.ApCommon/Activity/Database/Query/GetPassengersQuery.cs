using System.Text;
using Lunggo.Framework.Database;
using Lunggo.ApCommon.Product.Model;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    public class GetPassengersQuery : DbQueryBase<GetPassengersQuery, Pax, Pax, string, string, string>
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
            clauseBuilder.Append("SELECT FirstName AS FirstName, LastName AS LastName, BirthDate AS DateOfBirth, ");
            clauseBuilder.Append("NationalityCd AS Nationality, PassportNumber AS PassportNumber, ");
            clauseBuilder.Append("PassportExpiryDate AS PassportExpiryDate, PassportCountryCd AS PassportCountry, ");
            clauseBuilder.Append("TypeCd AS TypeCd, TitleCd AS TitleCd, GenderCd AS GenderCd ");
            clauseBuilder.Append("FROM Pax ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE RsvNo = @RsvNo ");
            return clauseBuilder.ToString();
        }
    }
}
