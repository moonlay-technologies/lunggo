using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Activity.Constant;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Payment.Model;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public bool SetRsvRefundBankAccount(string rsvNo, BankAccount account)
        {
            return SetRsvRefundBankAccountToDb(rsvNo, account);
        }

        public BankAccount GetRsvRefundBankAccount(string rsvNo)
        {
            return GetRsvRefundBankAccountFromDb(rsvNo);
        }
    }
}
