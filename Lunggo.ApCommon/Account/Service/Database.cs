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
                if (!string.IsNullOrEmpty(forgetPasswordInput.PhoneNumber))
                {
                    var userId = GetUserIdFromDbQuery.GetInstance().Execute(conn, new { Contact = forgetPasswordInput.PhoneNumber }).ToList();
                    return userId;
                }
                else
                {
                    var userId = GetUserIdFromDbQuery.GetInstance().Execute(conn, new { Contact = forgetPasswordInput.Email }).ToList();
                    return userId;
                }
                
            }
        }

        public List<string> GetOtpHashFromDb (ForgetPasswordInput forgetPasswordInput)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var otpHash = new List<string>();
                if (!string.IsNullOrEmpty(forgetPasswordInput.PhoneNumber))
                {
                    otpHash = GetOtpHashFromDbQuery.GetInstance().Execute(conn, new { Contact = forgetPasswordInput.PhoneNumber }).ToList();
                }
                else
                {
                    otpHash = GetOtpHashFromDbQuery.GetInstance().Execute(conn, new { Contact = forgetPasswordInput.Email }).ToList();
                }
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
                    Email = forgetPasswordInput.Email,
                    OtpHash = otp.Sha512Encode()
                };
                ResetPasswordTableRepo.GetInstance().Insert(conn,resetPasswordInput);
            }
        }

        public void UpdateDateResetPasswordToDb(ForgetPasswordInput forgetPasswordInput, string otp, DateTime expireTime)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                if (!string.IsNullOrEmpty(forgetPasswordInput.PhoneNumber))
                {
                    UpdateDateResetPasswordToDbQuery.GetInstance().Execute(conn, new { OtpHash = otp.Sha512Encode(), ExpireTime = expireTime, Contact = forgetPasswordInput.PhoneNumber });
                }
                else
                {
                    UpdateDateResetPasswordToDbQuery.GetInstance().Execute(conn, new { OtpHash = otp.Sha512Encode(), ExpireTime = expireTime, Contact = forgetPasswordInput.Email });
                }
            }
        }

        public List<DateTime> GetExpireTimeFromDb(CheckOtpInput checkOtpInput)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var expireTime = new List<DateTime>();
                if (!string.IsNullOrEmpty(checkOtpInput.PhoneNumber))
                {
                    expireTime = GetExpireTimeFromDbQuery.GetInstance().Execute(conn, new { CountryCallCd = checkOtpInput.CountryCallCd, Contact = checkOtpInput.PhoneNumber }).ToList();
                }
                else
                {
                    expireTime = GetExpireTimeFromDbQuery.GetInstance().Execute(conn, new { CountryCallCd = checkOtpInput.CountryCallCd, Contact = checkOtpInput.Email }).ToList();
                }
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
        
        public void DeleteDataOtpFromDb(string contact)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                DeleteDataOtpFromDbQuery.GetInstance().Execute(conn, new { Contact = contact });
            }
        }
    }
}
