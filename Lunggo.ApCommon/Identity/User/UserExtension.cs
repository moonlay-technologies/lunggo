using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Identity.Query;
using Lunggo.ApCommon.Identity.Query.Record;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Identity.User
{
    public static class UserExtension
    {
        public static CustomUser GetCustomUser(this IIdentity identity)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var userRecord = GetUserByNameQuery.GetInstance().Execute(conn, new {userName = identity.Name}).Single();
                var customUser = ToCustomUser(userRecord);
                return customUser;
            }
        }

        public static string GetFirstName(this IIdentity identity)
        {
            var customUser = GetCustomUser(identity);
            return customUser.FirstName;
        }

        public static string GetLastName(this IIdentity identity)
        {
            var customUser = GetCustomUser(identity);
            return customUser.LastName;
        }

        public static string GetEmail(this IIdentity identity)
        {
            var customUser = GetCustomUser(identity);
            return customUser.Email;
        }

        public static string GetPhoneNumber(this IIdentity identity)
        {
            var customUser = GetCustomUser(identity);
            return customUser.PhoneNumber;
        }

        private static CustomUser ToCustomUser(GetUserByAnyQueryRecord userRecord)
        {
            return new CustomUser
            {
                Email = userRecord.Email,
                FirstName = userRecord.FirstName,
                LastName = userRecord.LastName,
                Id = userRecord.Id,
                PasswordHash = userRecord.PasswordHash,
                UserName = userRecord.UserName,
                AccessFailedCount = userRecord.AccessFailedCount,
                EmailConfirmed = userRecord.EmailConfirmed,
                PhoneNumber = userRecord.PhoneNumber,
                LockoutEnabled = userRecord.LockoutEnabled,
                LockoutEndDateUtc = userRecord.LockoutEndDateUtc,
                PhoneNumberConfirmed = userRecord.PhoneNumberConfirmed,
                SecurityStamp = userRecord.SecurityStamp,
                TwoFactorEnabled = userRecord.TwoFactorEnabled
            };
        }
    }
}
