using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Account.Service
{
    public partial class AccountService
    {
        public void DeleteDataOtp(string phoneNumber)
        {
            DeleteDataOtpFromDb(phoneNumber);
        }
    }
}
