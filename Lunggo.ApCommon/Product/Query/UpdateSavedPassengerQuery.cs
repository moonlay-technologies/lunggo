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
            queryBuilder.Append(CreateFromClause());
            queryBuilder.Append(CreateWhereClause());
            return queryBuilder.ToString();
        }

        private static string CreateUpdateClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"UPDATE sp ");
            clauseBuilder.Append(@"SET ");
            clauseBuilder.Append(@"sg.Email = @Email, ");
            clauseBuilder.Append(@"sg.TypeCd = @TypeCd, ");
            clauseBuilder.Append(@"sg.GenderCd = @GenderCd, ");
            clauseBuilder.Append(@"sg.TitleCd = @TitleCd, ");
            clauseBuilder.Append(@"sg.FirstName = @FirstName, ");
            clauseBuilder.Append(@"sg.LastName = @LastName, ");
            clauseBuilder.Append(@"sg.BirthDate = @BirthDate, ");
            clauseBuilder.Append(@"sg.NationalityCd = @NationalityCd, ");
            clauseBuilder.Append(@"sg.PassportNumber = @PassportNumber, ");
            clauseBuilder.Append(@"sg.PassportExpiryDate = @PassportExpiryDate, ");
            clauseBuilder.Append(@"sg.PassportCountryCd = @PassportCountryCd ");
            return clauseBuilder.ToString();
        }

        private static string CreateFromClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"FROM SavedPassenger AS sg ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"WHERE ");
            clauseBuilder.Append(@"sg.Id = @Id AND ");
            clauseBuilder.Append(@"sg.Email = @Email ");
            return clauseBuilder.ToString();
        }
    }
}
