using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Lunggo.ApCommon.Identity.Query;
using Lunggo.Framework.Database;
using Microsoft.AspNet.Identity;

namespace Lunggo.ApCommon.Identity.Users
{
    public class User : UserBase<string>
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User,string> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        internal static User GetFromDb(string userId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var record = GetUserByIdQuery.GetInstance().Execute(conn, new {Id = userId}).SingleOrDefault();

                if (record == null)
                    return null;

                var user = UserExtension.ToCustomUser(record);
                return user;
            }
        }

        internal static List<string> GetListApproverEmail(string userId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var user = GetListApproverEmailQuery.GetInstance().Execute(conn, new { Id = userId }).ToList();
                return user;
            }
        }

        internal static string GetApprover(string userId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var user = GetApproverByUserIdQuery.GetInstance().Execute(conn, new {userId }).FirstOrDefault();
                return user;
            }
        }

        internal static string GetApproverEmailByUserId(string userId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var user = GetApproverEmailByUserIdQuery.GetInstance().Execute(conn, new {userId }).FirstOrDefault();
                return user;
            }
        }

        internal static string GetCompanyIdByUserId(string userId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var companyId = GetCompanyIdByUserQuery.GetInstance().Execute(conn, new { userId }).FirstOrDefault();
                return companyId;
            }
        }

        public List<User> GetAllUserByCompanyId(string userId)
        {
            var test = new List<User>();
            return test;
        }
    }
}