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
using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Account.Model;

namespace Lunggo.ApCommon.Account.Service
{
    public partial class AccountService
    {
        public List<string> GetUserIdFromDb (RequestOtpInput requestOtpInput)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                if (!string.IsNullOrEmpty(requestOtpInput.PhoneNumber))
                {
                    var userId = GetUserIdFromDbQuery.GetInstance().Execute(conn, new { Contact = requestOtpInput.PhoneNumber, PhoneNumber = requestOtpInput.PhoneNumber, CountryCallCd = requestOtpInput.CountryCallCd }, new { Contact = requestOtpInput.PhoneNumber }).ToList();
                    return userId;
                }
                else
                {
                    var userId = GetUserIdFromDbQuery.GetInstance().Execute(conn, new { Contact = requestOtpInput.Email, PhoneNumber = "1", CountryCallCd = "1"}, new { Contact = requestOtpInput.Email }).ToList();
                    return userId;
                }
                
            }
        }

        public List<string> GetUserIdFromDb(string input)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                string countryCallCd = null;
                string phoneNumber = null;
                if(input.Contains(" "))
                {
                    var phone = input.Split(' ');
                    countryCallCd = phone[0];
                    phoneNumber = phone[1];
                    input = input.Replace(" ", string.Empty);
                }
                var userId = GetUserIdFromDbQuery.GetInstance().Execute(conn, new { Contact = input, PhoneNumber = phoneNumber, CountryCallCd = countryCallCd }, new { Contact = input }).ToList();
                return userId;
            }
        }

        public List<string> GetOtpHashFromDb (RequestOtpInput requestOtpInput)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var otpHash = new List<string>();
                if (!string.IsNullOrEmpty(requestOtpInput.PhoneNumber))
                {
                    otpHash = GetOtpHashFromDbQuery.GetInstance().Execute(conn, new { Contact = requestOtpInput.PhoneNumber, CountryCallCd = requestOtpInput.CountryCallCd }).ToList();
                }
                else
                {
                    otpHash = GetOtpHashFromDbQuery.GetInstance().Execute(conn, new { Contact = requestOtpInput.Email, CountryCallCd = requestOtpInput.CountryCallCd }).ToList();
                }
                return otpHash;
            }
        }

        public void InsertDataResetPasswordToDb(RequestOtpInput requestOtpInput, string otp, DateTime expireTime)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var resetPasswordInput = new ResetPasswordTableRecord
                {
                    PhoneNumber = requestOtpInput.PhoneNumber,
                    ExpireTime = expireTime,
                    Email = requestOtpInput.Email,
                    CountryCallCd = requestOtpInput.CountryCallCd,
                    OtpHash = otp.Sha512Encode()
                };
                ResetPasswordTableRepo.GetInstance().Insert(conn,resetPasswordInput);
            }
        }

        public void UpdateDateResetPasswordToDb(RequestOtpInput requestOtpInput, string otp, DateTime expireTime)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                if (!string.IsNullOrEmpty(requestOtpInput.PhoneNumber))
                {
                    UpdateDateResetPasswordToDbQuery.GetInstance().Execute(conn, new { OtpHash = otp.Sha512Encode(), ExpireTime = expireTime, Contact = requestOtpInput.PhoneNumber, CountryCallCd = requestOtpInput.CountryCallCd });
                }
                else
                {
                    UpdateDateResetPasswordToDbQuery.GetInstance().Execute(conn, new { OtpHash = otp.Sha512Encode(), ExpireTime = expireTime, Contact = requestOtpInput.Email });
                }
            }
        }

        public List<DateTime> GetExpireTimeFromDb(string otp, string countryCallCd = null, string phoneNumber = null, string email = null)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var expireTime = new List<DateTime>();
                if (!string.IsNullOrEmpty(phoneNumber))
                {
                    expireTime = GetExpireTimeFromDbQuery.GetInstance().Execute(conn, new { CountryCallCd = countryCallCd, Contact = phoneNumber }).ToList();
                }
                else
                {
                    expireTime = GetExpireTimeFromDbQuery.GetInstance().Execute(conn, new { CountryCallCd = countryCallCd, Contact = email }).ToList();
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
                };
                ReferralCodeTableRepo.GetInstance().Insert(conn, tableRecord);

                var tableRecordCredit = new ReferralCreditTableRecord
                {
                    UserId = userId,
                    ReferralCredit = 0,
                    ExpDate = new DateTime(DateTime.UtcNow.Year, 12, 31, 16, 59, 59)
                };
                ReferralCreditTableRepo.GetInstance().Insert(conn, tableRecordCredit);
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
                string referrerId = null;
                if (referralCode.ReferrerCode != null)
                {
                    referrerId = GetIdByReferralCodeFromDb(referralCode.ReferrerCode);
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
                if(referralCode.Count == 1)
                {
                    return referralCode.First();
                }
                else if (referralCode.Count > 1)
                {
                    var refer = referralCode.Where(referral => referral.ExpDate > DateTime.UtcNow).ToList();
                    if(refer.Count > 0)
                    {
                        return refer.First();
                    }
                    else
                    {
                        return null;
                    }
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
                if(referralCodeRecord == null)
                {
                    return null;
                }
                var referralCredit = ReferralCreditTableRepo.GetInstance().Find1(conn, new ReferralCreditTableRecord { UserId = referralCodeRecord.UserId });
                return new ReferralCodeModel
                {
                    ReferralCode = referralCodeRecord.ReferralCode,
                    ReferrerCode = referralCodeRecord.ReferrerCode,
                    ReferralCredit = (decimal)referralCredit.ReferralCredit,
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

        public void AddCreditReferralFromDb(string userId, decimal referralCredit)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var referral = GetReferralCodeByIdFromDb(userId);
                if(referral == null)
                {
                    return;
                }                                
                referral.ReferralCredit = referral.ReferralCredit + referralCredit;                
                var year = DateTime.UtcNow.AddHours(7).Year;
                var expDate = new DateTime(year, 12, 31, 16, 59, 59);
                UpdateReferralCreditDbQuery.GetInstance().Execute(conn, new { UserId = userId, ReferralCredit = referral.ReferralCredit, ExpDate = expDate });                
                if(referral.ReferrerCode != null)
                {
                    var referrerId = GetIdByReferralCodeFromDb(referral.ReferrerCode);
                    var referrer = GetReferralCodeByIdFromDb(referrerId);
                    referrer.ReferralCredit = referrer.ReferralCredit + referralCredit;
                    UpdateReferralCreditDbQuery.GetInstance().Execute(conn, new { UserId = referrer.UserId, ReferralCredit = referrer.ReferralCredit, ExpDate = expDate });
                }
            }
        }

        public void UpdateCreditReferralFirstTimeBooking(string userId, decimal referralCredit)
        {

        }

        public void UpdateReferrerCodeDb(string userId, string referrerCode)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                UpdateReferrerCodeDbQuery.GetInstance().Execute(conn, new { UserId = userId, ReferrerCode = referrerCode });
            }                
        }

        public void UseReferralCreditFromDb(string userId, decimal referralCreditUsed)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var referral = GetReferralCodeByIdFromDb(userId);
                if (referral == null)
                {
                    return;
                }
                if (referral.ExpDate < DateTime.UtcNow)
                {
                    return;
                }
                referral.ReferralCredit = referral.ReferralCredit - referralCreditUsed;
                UseReferralCreditFromDbQuery.GetInstance().Execute(conn, new { UserId = userId, ReferralCredit = referral.ReferralCredit });
            }
        }

        public List<string> GetReferreeIdsFromDb(string referrerCode)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                List<string> referreesIds = GetReferreeCodeFromDbQuery.GetInstance().Execute(conn, new { ReferrerCode = referrerCode }).ToList();
                return referreesIds;
            }
        }

        public void UpdateProfileToDb(User user)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                UpdateProfileToDbQuery.GetInstance().Execute(conn, user);
            }
        }
    }
}
