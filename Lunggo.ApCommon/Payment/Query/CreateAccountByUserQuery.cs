using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Payment.Query
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
