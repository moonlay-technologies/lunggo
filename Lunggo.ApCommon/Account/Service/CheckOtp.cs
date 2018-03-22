using Lunggo.ApCommon.Account.Model.Logic;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.Framework.Encoder;
using Lunggo.Repository.TableRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Account.Service
{
    public partial class AccountService
    {
        public bool CheckOtp(string otp, string email)
        {
            return CheckOtp(otp, null, null, email);
        }

        public bool CheckOtp(string otp, string countryCallCd, string phoneNumber)
        {
            return CheckOtp(otp, countryCallCd, phoneNumber, null);
        }

        public bool CheckExpireTime(string otp, string countryCallCd, string phoneNumber)
        {
            return CheckExpireTime(otp, countryCallCd, phoneNumber, null);
        }

        public bool CheckExpireTime(string otp, string email)
        {
            return CheckExpireTime(otp, null, null, email);
        }

        public bool VerifyPhoneWithOtp(string otp, User user)
        {
            var isOtpValid = CheckOtp(otp, user.CountryCallCd, user.PhoneNumber);
            if (isOtpValid)
            {
                user.PhoneNumberConfirmed = true;
                UpdateProfileToDb(user);
                return true;
            }

            return false;
        }

        private bool CheckExpireTime(string otp, string countryCallCd, string phoneNumber, string email)
        {
            var expireTimeDb = GetExpireTimeFromDb(otp, countryCallCd, phoneNumber, email);
            if (expireTimeDb.Count() < 1)
            {
                return false;
            }
            if (DateTime.UtcNow > expireTimeDb.First())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool CheckOtp(string otp, string countryCallCd, string phoneNumber, string email)
        {
            var otpHash = otp.Sha512Encode();
            var otpHashDb = GetOtpHashFromDb(new RequestOtpInput { CountryCallCd = countryCallCd, PhoneNumber = phoneNumber, Email = email });
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
    }
}
