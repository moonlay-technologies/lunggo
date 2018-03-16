using Lunggo.ApCommon.Account.Model.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Account.Service
{
    public partial class AccountService
    {
        public string GetIdByEmailOrPhoneNumber(string input)
        {
            var ids = GetUserIdFromDb(input);
            return ids.First();
        }
    }
}
