using Lunggo.ApCommon.Account.Model.Database;
using Lunggo.ApCommon.Account.Model.Logic;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Encoder;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Repository.TableRepository;

namespace Lunggo.ApCommon.Account.Service
{
    public partial class AccountService
    {
        public List<string> GetUserIdFromDb (ForgetPasswordInput forgetPasswordInput)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var userId = GetUserIdFromDbQuery.GetInstance().Execute(conn, new { PhoneNumber = forgetPasswordInput.PhoneNumber }).ToList();
                return userId;
            }
        }

        public List<string> GetOtpHashFromDb (ForgetPasswordInput forgetPasswordInput)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var otpHash = GetOtpHashFromDbQuery.GetInstance().Execute(conn, new {PhoneNumber = forgetPasswordInput.PhoneNumber }).ToList();
                return otpHash;
            }
        }

        public void InsertDataResetPasswordToDb(ForgetPasswordInput forgetPasswordInput, string otp, DateTime expireTime)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var resetPasswordInput = new ResetPasswordTableRecord
                {
                    PhoneNumber = forgetPasswordInput.PhoneNumber,
                    ExpireTime = expireTime,
                    OtpHash = otp.Sha512Encode()
                };
                ResetPasswordTableRepo.GetInstance().Insert(conn,resetPasswordInput);
            }
        }

        public void UpdateDateResetPasswordToDb(ForgetPasswordInput forgetPasswordInput, string otp, DateTime expireTime)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                UpdateDateResetPasswordToDbQuery.GetInstance().Execute(conn, new { OtpHash = otp.Sha512Encode(), ExpireTime = expireTime, PhoneNumber = forgetPasswordInput.PhoneNumber });
            }
        }

        public List<DateTime> GetExpireTimeFromDb(CheckOtpInput checkOtpInput)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var expireTime = GetExpireTimeFromDbQuery.GetInstance().Execute(conn, new { CountryCallCd = checkOtpInput.CountryCallCd, PhoneNumber = checkOtpInput.PhoneNumber }).ToList();
                return expireTime;
            }                
        }

        public void UpdatePasswordToUserDb(ResettingPasswordInput resettingPasswordInput)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                UpdatePasswordToDbQuery.GetInstance().Execute(conn, new { NewPasswordHash = resettingPasswordInput.NewPassword.Sha512Encode(), PhoneNumber = resettingPasswordInput.PhoneNumber });
            }
        }
        
        public void DeleteDataOtpFromDb(string phoneNumber)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                DeleteDataOtpFromDbQuery.GetInstance().Execute(conn, new { PhoneNumber = phoneNumber });
            }
        }
    }
}
