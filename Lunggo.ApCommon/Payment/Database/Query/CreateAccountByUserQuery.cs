using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Payment.Database.Query
{
    internal class CreateAccountByUserQuery : NoReturnDbQueryBase<CreateAccountByUserQuery>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var query = "";
            query += "BEGIN TRANSACTION;";
            query += "INSERT INTO AccountBalance (AccountNo, Balance, Withdrawable, LastUpdate) VALUES (@accountNo, 0, 0, GETDATE());";
            query += "INSERT INTO AccountUser (AccountNo, UserId) VALUES (@accountNo, @userId);";
            query += "COMMIT TRANSACTION;";
            return query;
        }
    }
}
