using Lunggo.ApCommon.Account.Model.Logic;
using Lunggo.Framework.Encoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Account.Service
{
    public partial class AccountService
    {
        public bool CheckOtp(CheckOtpInput checkOtpInput)
        {
            var otpHash = checkOtpInput.Otp.Sha512Encode();
            var otpHashDb = GetOtpHashFromDb(new ForgetPasswordInput { CountryCallCd = checkOtpInput.CountryCallCd, PhoneNumber = checkOtpInput.PhoneNumber });
            if (otpHashDb.Count() < 1)
            {
                return false;
            }

            if (otpHash != otpHashDb.First())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool CheckExpireTime(CheckOtpInput checkOtpInput)
        {
            var expireTimeDb = GetExpireTimeFromDb(checkOtpInput).First();
            if(DateTime.UtcNow > expireTimeDb)
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
