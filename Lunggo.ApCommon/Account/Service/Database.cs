﻿using Lunggo.ApCommon.Account.Model.Database;
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
using Lunggo.ApCommon.Account.Model;

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

        public List<string> GetUserIdFromDb(string input)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {               
                var userId = GetUserIdFromDbQuery.GetInstance().Execute(conn, new { Contact = input }).ToList();
                return userId;
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

        public void InsertReferralCodeToDb(string userId, string referralCode, string referrerCode)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var tableRecord = new ReferralCodeTableRecord
                {
                    UserId = userId,
                    ReferralCode = referralCode,
                    ReferrerCode = referrerCode,
                    ReferralCredit = 0M
                };
                ReferralCodeTableRepo.GetInstance().Insert(conn, tableRecord);
            }
        }

        public void InsertReferralHistoryToDb(string userId, string history, decimal referralCredit)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var referralCode = GetReferralCodeByIdFromDb(userId);
                if (referralCode == null)
                {
                    return;
                }

                var referrerId = GetIdByReferralCodeFromDb(referralCode.ReferrerCode);
                if (referrerId == null)
                {
                    return;
                }
                var tableRecord = new ReferralHistoryTableRecord
                {
                    UserId = referrerId,
                    ReferreeId = userId,
                    History = history,
                    ReferralCredit = referralCredit,
                    TimeStamp = DateTime.UtcNow
                };
                ReferralHistoryTableRepo.GetInstance().Insert(conn, tableRecord);
            }
        }

        public ReferralCodeModel GetReferralCodeByIdFromDb(string userId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var referralCode = GetReferralCodeFromDbQuery.GetInstance().Execute(conn, new { UserId = userId }).ToList();
                if(referralCode.Count > 0)
                {
                    return referralCode.First();
                }
                else
                {
                    return null;
                }
            }
        }

        public ReferralCodeModel GetReferralCodeDataFromDb(string referralCode)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var referralCodeRecord = ReferralCodeTableRepo.GetInstance().Find1(conn, new ReferralCodeTableRecord { ReferralCode = referralCode });
                if(referralCode == null)
                {
                    return null;
                }
                return new ReferralCodeModel
                {
                    ReferralCode = referralCodeRecord.ReferralCode,
                    ReferrerCode = referralCodeRecord.ReferrerCode,
                    ReferralCredit = referralCodeRecord.ReferralCredit,
                    UserId = referralCodeRecord.UserId
                };
            }
        }
                


        public string GetIdByReferralCodeFromDb(string referralCode)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var ids = GetIdByReferralCodeFromDbQuery.GetInstance().Execute(conn, new { ReferralCode = referralCode }).ToList();
                if(ids.Count > 0)
                {
                    return ids.First();
                }
                else
                {
                    return null;
                }
            }
        }

        public List<ReferralHistoryModel> GetReferralHistoryModelFromDb(string userId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var referralHistoryRecord = ReferralHistoryTableRepo.GetInstance().Find(conn, new ReferralHistoryTableRecord { UserId = userId }).ToList();
                var referralHistory = referralHistoryRecord.Select(referral => new ReferralHistoryModel { UserId = referral.UserId, ReferreeId = referral.ReferreeId, History = referral.History, ReferralCredit = referral.ReferralCredit, TimeStamp = referral.TimeStamp }).ToList();
                return referralHistory;
            }
                
        }

        public ReferralHistoryModel GetUserByRefereeIdAndHistoryFromDb(string userId, string history)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var referralHistoryRecord = GetReferralHistoryByIdAndHistoryDbQuery.GetInstance().Execute(conn, new { ReferreeId = userId, History = history }).ToList();
                if (referralHistoryRecord.Count == 0)
                {
                    return null;
                }
                else
                {
                    return referralHistoryRecord.First();
                }
            }
        }

        public void UpdateCreditReferralFromDb(string userId, decimal referralCredit)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var referral = GetReferralCodeByIdFromDb(userId);
                if(referral.ReferrerCode == null)
                {
                    return;
                }
                var referrer = GetReferralCodeDataFromDb(referral.ReferrerCode);
                referral.ReferralCredit = referral.ReferralCredit + referralCredit;
                referrer.ReferralCredit = referrer.ReferralCredit + referralCredit;
                UpdateReferralCreditDbQuery.GetInstance().Execute(conn, new { UserId = userId, ReferralCredit = referral.ReferralCredit });
                UpdateReferralCreditDbQuery.GetInstance().Execute(conn, new { UserId = referrer.UserId, ReferralCredit = referrer.ReferralCredit });
            }
        }
    }
}
