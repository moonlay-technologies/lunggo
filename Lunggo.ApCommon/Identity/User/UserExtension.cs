using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using Lunggo.ApCommon.Identity.Query;
using Lunggo.ApCommon.Identity.Query.Record;
using Lunggo.Framework.Database;
using Lunggo.Framework.Extension;

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
            if (identity.IsAuthenticated)
            {
                var customUser = GetCustomUser(identity);
                return customUser.FirstName;
            }
            else
            {
                return null;
            }
            
        }

        public static string GetLastName(this IIdentity identity)
        {
            if (identity.IsAuthenticated)
            {
                var customUser = GetCustomUser(identity);
                return customUser.LastName;
            }
            else
            {
                return null;
            }
        }

        public static string GetEmail(this IIdentity identity)
        {
            if (identity.IsAuthenticated)
            {
                var customUser = GetCustomUser(identity);
                return customUser.Email;
            }
            else
            {
                return null;
            }
        }

        public static string GetCountryCd(this IIdentity identity)
        {
            if (identity.IsAuthenticated)
            {
                var customUser = GetCustomUser(identity);
                return customUser.CountryCd;
            }
            else
            {
                return null;
            }
        }

        public static string GetPhoneNumber(this IIdentity identity)
        {
            if (identity.IsAuthenticated)
            {
                var customUser = GetCustomUser(identity);
                return customUser.PhoneNumber;
            }
            else
            {
                return null;
            }
        }

        public static string GetAddress(this IIdentity identity)
        {
            if (identity.IsAuthenticated)
            {
                var customUser = GetCustomUser(identity);
                return customUser.Address;
            }
            else
            {
                return null;
            }
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
                CountryCd = userRecord.CountryCd,
                PhoneNumber = userRecord.PhoneNumber,
                Address = userRecord.Address,
                LockoutEnabled = userRecord.LockoutEnabled,
                LockoutEndDateUtc = userRecord.LockoutEndDateUtc,
                PhoneNumberConfirmed = userRecord.PhoneNumberConfirmed,
                SecurityStamp = userRecord.SecurityStamp,
                TwoFactorEnabled = userRecord.TwoFactorEnabled
            };
        }
    }
}
