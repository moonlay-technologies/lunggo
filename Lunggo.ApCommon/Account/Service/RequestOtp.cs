using Lunggo.ApCommon.Account.Model.Logic;
using Lunggo.ApCommon.Constant;
using Lunggo.Framework.Encoder;
using Lunggo.Framework.Pattern;
using Lunggo.Framework.Queue;
using Lunggo.Framework.Redis;
using Lunggo.Framework.SmsGateway;
using Microsoft.WindowsAzure.Storage.Queue;
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
        public bool CheckContactData (RequestOtpInput requestOtpInput)
        {
            var userId = GetUserIdFromDb(requestOtpInput);
            if (userId.Count() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public RequestOtpOutput RequestOtp(RequestOtpInput requestOtpInput)
        {
            var otp = GenerateOtp();
            if (!string.IsNullOrEmpty(requestOtpInput.PhoneNumber))
            {
                
                var isSendSmsSuccess = SendOtpToUser(requestOtpInput.PhoneNumber, otp);
                if (isSendSmsSuccess == false)
                {
                    return new RequestOtpOutput
                    {
                        isSuccess = false
                    };
                }
                var expireTime = DateTime.UtcNow.AddMinutes(30);
                if (CheckPhoneNumberFromResetPasswordDb(requestOtpInput) == false)
                {
                    InsertDataResetPasswordToDb(requestOtpInput, otp, expireTime);
                }
                else
                {
                    UpdateDateResetPasswordToDb(requestOtpInput, otp, expireTime);
                }


                if (isSendSmsSuccess == false)
                {
                    return new RequestOtpOutput
                    {
                        isSuccess = false
                    };
                }
                InsertSmsTimeToCache(requestOtpInput.PhoneNumber, otp);


                return new RequestOtpOutput
                {
                    CountryCallCd = requestOtpInput.CountryCallCd,
                    PhoneNumber = requestOtpInput.PhoneNumber,
                    Otp = otp,
                    ExpireTime = expireTime,
                    isSuccess = true
                };
            }
            else
            {
                SendOtpToUserByEmail(requestOtpInput.Email, otp);
                var expireTime = DateTime.UtcNow.AddMinutes(30);
                if (CheckPhoneNumberFromResetPasswordDb(requestOtpInput) == false)
                {
                    InsertDataResetPasswordToDb(requestOtpInput, otp, expireTime);
                }
                else
                {
                    UpdateDateResetPasswordToDb(requestOtpInput, otp, expireTime);
                }
                InsertEmailTimeToCache(requestOtpInput.Email, otp);


                return new RequestOtpOutput
                {
                    CountryCallCd = requestOtpInput.CountryCallCd,
                    Email = requestOtpInput.Email,
                    Otp = otp,
                    ExpireTime = expireTime,
                    isSuccess = true
                };
            }
            
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

        public bool CheckPhoneNumberFromResetPasswordDb(RequestOtpInput requestOtpInput)
        {
            var OtpHash = GetOtpHashFromDb(requestOtpInput);
            if (OtpHash.Count() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool SendOtpToUser (string phoneNumber, string otp)
        {
            var message = "Your Otp Is : " + otp;
            var smsGateway = new SmsGateway();
            var response = smsGateway.SendSms(phoneNumber, message);
            if (response.Content.Contains("<text>Success</text>"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void InsertSmsTimeToCache (string phoneNumber, string otp)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "ForgotPasswordSmsTimer:" + phoneNumber;
            var redisValue = DateTime.UtcNow.AddSeconds(90).ToString();
            var redisDb = redisService.GetDatabase(ApConstant.MasterDataCacheName);
            redisDb.StringSet(redisKey, redisValue, TimeSpan.FromSeconds(90));
        }

        public void InsertEmailTimeToCache (string email, string otp)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "ForgotPasswordEmailTimer:" + email;
            var redisValue = DateTime.UtcNow.AddSeconds(90).ToString();
            var redisDb = redisService.GetDatabase(ApConstant.MasterDataCacheName);
            redisDb.StringSet(redisKey, redisValue, TimeSpan.FromSeconds(90));
        }
        public bool CheckTimerSms(string phoneNumber, out int? resendCooldownSeconds)
        {
            if (phoneNumber.StartsWith("0"))
            {
                phoneNumber = phoneNumber.Substring(1);
            }
            
            var redisService = RedisService.GetInstance();
            var redisKey = "ForgotPasswordSmsTimer:" + phoneNumber;
            var redisDb = redisService.GetDatabase(ApConstant.MasterDataCacheName);
            var result = redisDb.KeyExists(redisKey);
            var resendCooldown = redisDb.StringGetWithExpiry(redisKey).Expiry;
            
            if (result == false)
            {
                resendCooldownSeconds = null;
                return true;
                
            }
            else
            {
                resendCooldownSeconds = Convert.ToInt32(resendCooldown.Value.TotalSeconds) + 30;
                return false;              
            }
        }

        public bool CheckTimerEmail(string email, out int? resendCooldownSeconds)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "ForgotPasswordEmailTimer:" + email;
            var redisDb = redisService.GetDatabase(ApConstant.MasterDataCacheName);
            var result = redisDb.KeyExists(redisKey);
            var resendCooldown = redisDb.StringGetWithExpiry(redisKey).Expiry;

            if (result == false)
            {
                resendCooldownSeconds = null;
                return true;

            }
            else
            {
                resendCooldownSeconds = Convert.ToInt32(resendCooldown.Value.TotalSeconds) + 30;
                return false;
            }
        }

        public void SendOtpToUserByEmail(string email, string otp)
        {
            var activityQueue = QueueService.GetInstance().GetQueueByReference("ForgotPasswordByOtpEmail");
            activityQueue.AddMessage(new CloudQueueMessage(email + ":" + otp));
        }
    }
}
