using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Lunggo.ApCommon.Identity.Query;
using Lunggo.ApCommon.Identity.Query.Record;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Identity.Users
{
    public static class UserExtension
    {
        public static bool IsUserAuthorized(this IIdentity identity)
        {
            var claimsIdentity = (identity as ClaimsIdentity) ?? new ClaimsIdentity();
            return claimsIdentity.HasClaim(ClaimTypes.Authentication, "password");
        }

        public static User GetUser(this IIdentity identity)
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
            if (identity.IsUserAuthorized())
            {
                var customUser = GetUser(identity);
                return customUser.FirstName;
            }
            else
            {
                return null;
            }
            
        }

        public static string GetLastName(this IIdentity identity)
        {
            if (identity.IsUserAuthorized())
            {
                var customUser = GetUser(identity);
                return customUser.LastName;
            }
            else
            {
                return null;
            }
        }

        public static string GetEmail(this IIdentity identity)
        {
            if (identity.IsUserAuthorized())
            {
                var customUser = GetUser(identity);
                return customUser.Email;
            }
            else
            {
                return null;
            }
        }

        public static string GetCountryCd(this IIdentity identity)
        {
            if (identity.IsUserAuthorized())
            {
                var customUser = GetUser(identity);
                return customUser.CountryCd;
            }
            else
            {
                return null;
            }
        }

        public static string GetPhoneNumber(this IIdentity identity)
        {
            if (identity.IsUserAuthorized())
            {
                var customUser = GetUser(identity);
                return customUser.PhoneNumber;
            }
            else
            {
                return null;
            }
        }

        public static string GetAddress(this IIdentity identity)
        {
            if (identity.IsUserAuthorized())
            {
                var customUser = GetUser(identity);
                return customUser.Address;
            }
            else
            {
                return null;
            }
        }

        public static string GetClientId(this IIdentity identity)
        {
            if (identity.IsAuthenticated)
            {
                var claimsIdentity = identity as ClaimsIdentity;
                if (claimsIdentity == null)
                    return null;
                var clientIdKey = claimsIdentity.Claims.SingleOrDefault(claim => claim.Type == "Client ID");
                if (clientIdKey == null)
                    return null;
                return clientIdKey.Value;
            }
            else
            {
                return null;
            }
        }

        public static string GetDeviceId(this IIdentity identity)
        {
            if (identity.IsAuthenticated)
            {
                var claimsIdentity = identity as ClaimsIdentity;
                if (claimsIdentity == null)
                    return null;
                var clientIdKey = claimsIdentity.Claims.SingleOrDefault(claim => claim.Type == "Device ID");
                if (clientIdKey == null)
                    return null;
                return clientIdKey.Value;
            }
            else
            {
                return null;
            }
        }

        internal static User ToCustomUser(GetUserByAnyQueryRecord userRecord)
        {
            return new User
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
