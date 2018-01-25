using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Account.Service
{
    public partial class AccountService
    {
        public bool CheckCountryCallCdFormat (string CountryCallCd)
        {
            if (!long.TryParse(CountryCallCd, out long m) || CountryCallCd.Count() > 3)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool CheckPhoneNumberFormat (string PhoneNumber)
        {
            if (!long.TryParse(PhoneNumber, out long n) || PhoneNumber.Count() < 6 || PhoneNumber.Count() > 15)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
