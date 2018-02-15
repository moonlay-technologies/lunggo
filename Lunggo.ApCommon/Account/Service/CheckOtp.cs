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
            var otpHashDb = GetOtpHashFromDb(new ForgetPasswordInput { CountryCallCd = checkOtpInput.CountryCallCd, PhoneNumber = checkOtpInput.PhoneNumber, Email = checkOtpInput.Email });
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
            var expireTimeDb = GetExpireTimeFromDb(checkOtpInput);
            if (expireTimeDb.Count() < 1)
            {
                return false;
            }
            if(DateTime.UtcNow > expireTimeDb.First())
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
