using Lunggo.ApCommon.Account.Model.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Account.Service
{
    public partial class AccountService
    {
        public void GenerateReferralCode(string userId, string referrerCode)
        {
            var referralCode = RandomString(10);
            InsertReferralCodeToDb(userId, referralCode, referrerCode);
        }

        public string RandomString(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (length-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    res.Append(valid[(int)(num % (uint)valid.Length)]);
                }
            }

            return res.ToString();
        }

        public void InsertLoginReferralHistory(string userId)
        {
            var history = "First Time Login";
            decimal referralCredit = 100000M;
            var referree = GetUserByRefereeIdAndHistoryFromDb(userId, history);
            if(referree != null)
            {
                return;
            }
            UpdateCreditReferralFromDb(userId, referralCredit);
            InsertReferralHistoryToDb(userId, history, referralCredit);
        }

        public GetReferralDetailOutput GetReferralDetail(string userId)
        {
            var referralDetail = GetReferralHistoryModelFromDb(userId);
            return new GetReferralDetailOutput
            {
                ReferralDetail = referralDetail
            };
        }

        public GetReferralOutput GetReferral(string userId)
        {
            var referralCode = GetReferralCodeByIdFromDb(userId);
            if (referralCode == null)
            {
                return null;
            }
            return new GetReferralOutput
            {
                ReferralCode = referralCode.ReferralCode,
                ReferralCredit = referralCode.ReferralCredit
            };
        }
    }
}
