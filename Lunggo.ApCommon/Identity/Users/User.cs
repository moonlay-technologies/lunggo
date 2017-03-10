using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Lunggo.ApCommon.Identity.Model;
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

        public static List<ApproverData> GetAvailableApprover()
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var userList = GetAvailableApproverQuery.GetInstance().Execute(conn, new {}).ToList();
                return userList;
            }
        }

        internal static string GetApproverEmailByUserId(string userId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var user = GetApproverEmailByUserIdQuery.GetInstance().Execute(conn, new {userId}).FirstOrDefault();
                return user;
            }
        }

        public static string GetCompanyIdByUserId(string userId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var companyId = GetCompanyIdByUserQuery.GetInstance().Execute(conn, new { userId }).FirstOrDefault();
                return companyId;
            }
        }

        public static List<UserData> GetAllUserByCompanyId(string userId)
        {
            var companyId = GetCompanyIdByUserId(userId);
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var userList = GetAllUserByAdminQuery.GetInstance().Execute(conn, new { CompanyId = companyId }).ToList();
                return userList;
            }
        }

        public static List<string> GetAllRoles()
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var roleList = GetListRolesQuery.GetInstance().Execute(conn, new { }).ToList();
                return roleList;
            }

        }

        public static string GetNameByUserId(string userId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var name = GetNameByUserIdQuery.GetInstance().Execute(conn, new { userId }).FirstOrDefault();
                return name;
            }
        }
    }
}