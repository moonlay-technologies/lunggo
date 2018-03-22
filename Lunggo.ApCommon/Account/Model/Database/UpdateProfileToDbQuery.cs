using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Account.Model.Database
{
    public class UpdateProfileToDbQuery : NoReturnDbQueryBase<UpdateProfileToDbQuery>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return 
                "UPDATE [User] SET Email = @Email, EmailConfirmed = @EmailConfirmed, CountryCallCd = @CountryCallCd, PhoneNumber = @PhoneNumber, " +
                "PhoneNumberConfirmed = @PhoneNumberConfirmed, FirstName = @FirstName, LastName = @LastName WHERE Id = @Id";
        }
    }
}
