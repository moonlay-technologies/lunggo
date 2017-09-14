using System.Text;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Product.Query
{
    internal class UpdateSavedPassengerQuery : NoReturnDbQueryBase<UpdateSavedPassengerQuery>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(CreateUpdateClause());
            queryBuilder.Append(CreateWhereClause());
            return queryBuilder.ToString();
        }

        private static string CreateUpdateClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"UPDATE SavedPassenger ");
            clauseBuilder.Append(@"SET ");
            clauseBuilder.Append(@"TypeCd = @TypeCd, ");
            clauseBuilder.Append(@"GenderCd = @GenderCd, ");
            clauseBuilder.Append(@"TitleCd = @TitleCd, ");
            clauseBuilder.Append(@"FirstName = @FirstName, ");
            clauseBuilder.Append(@"LastName = @LastName, ");
            clauseBuilder.Append(@"BirthDate = @BirthDate, ");
            clauseBuilder.Append(@"NationalityCd = @NationalityCd, ");
            clauseBuilder.Append(@"PassportNumber = @PassportNumber, ");
            clauseBuilder.Append(@"PassportExpiryDate = @PassportExpiryDate, ");
            clauseBuilder.Append(@"PassportCountryCd = @PassportCountryCd ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"WHERE ");
            clauseBuilder.Append(@"Id = @Id AND ");
            clauseBuilder.Append(@"Email = @Email ");
            return clauseBuilder.ToString();
        }
    }
}
