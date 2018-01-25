using Lunggo.ApCommon.Account.Model.Logic;
using Lunggo.Framework.Pattern;
using Lunggo.Framework.SmsGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Lunggo.ApCommon.Account.Service
{
    public partial class AccountService : SingletonBase<AccountService>
    {
        public bool CheckPhoneNumber (ForgetPasswordInput forgetPasswordInput)
        {
            var userId = GetUserIdFromDb(forgetPasswordInput);
            if (userId.Count() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public ForgetPasswordOutput ForgetPassword(ForgetPasswordInput forgetPasswordInput)
        {
            
            var otp = GenerateOtp();
            var expireTime = DateTime.UtcNow.AddMinutes(30);
            if (CheckPhoneNumberFromResetPasswordDb(forgetPasswordInput) == false)
            {
                InsertDataResetPasswordToDb(forgetPasswordInput, otp, expireTime);
            }
            else
            {
                UpdateDateResetPasswordToDb(forgetPasswordInput, otp, expireTime);
            }

            SendOtpToUser(forgetPasswordInput.PhoneNumber, otp);

            return new ForgetPasswordOutput
            {
                CountryCallCd = forgetPasswordInput.CountryCallCd,
                PhoneNumber = forgetPasswordInput.PhoneNumber,
                Otp = otp,
                ExpireTime = expireTime
            };
        }
        
        public string GenerateOtp()
        {
            var rng = new RNGCryptoServiceProvider();
            byte[] randomByte = new byte[8];
            rng.GetBytes(randomByte);
            var randomInt = Math.Abs(BitConverter.ToInt32(randomByte, 0));
            var intOtp = randomInt % 1000000;
            var otp = intOtp.ToString("D6");
            return otp;
        }

        public bool CheckPhoneNumberFromResetPasswordDb(ForgetPasswordInput forgetPasswordInput)
        {
            var OtpHash = GetOtpHashFromDb(forgetPasswordInput);
            if (OtpHash.Count() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void SendOtpToUser (string phoneNumber, string otp)
        {
            var message = "Your Otp Is : " + otp;
            var smsGateway = new SmsGateway();
            var response = smsGateway.SendSms(phoneNumber, message);
        }
    }
}
