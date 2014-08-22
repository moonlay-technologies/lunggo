using Dapper;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Lunggo.CustomerWeb.Models
{
    public class CustomUser : IUser
    {
        public Guid UserId { get; set; }
        public string Id { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTime LockoutEndDateUtc { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<CustomUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class DapperUserStore<TUser> : IUserStore<TUser>, IUserStore<TUser, string>,IUserEmailStore<TUser>, IUserPasswordStore<TUser>, IUserSecurityStampStore<TUser>, IDisposable where TUser : CustomUser
    {
        private readonly string connectionString;
 
        public DapperUserStore(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException("connectionString");
 
            this.connectionString = connectionString;
        }

        public DapperUserStore()
        {
            this.connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }
 
        public void Dispose()
        {
 
        }
 
        #region IUserStore
        public virtual Task CreateAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.Factory.StartNew(() => {
                user.UserId = Guid.NewGuid();
                user.Id = user.UserId.ToString();
                using (SqlConnection connection = new SqlConnection(connectionString))
                    connection.Execute(
                        "insert into AspNetUsers(Id, Email, Emailconfirmed, PasswordHash, SecurityStamp, " +
                        "PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEndDateUtc, LockoutEnabled, " +
                        "AccessFailedCount, UserName)" +
                        "values(@Id, @Email, @Emailconfirmed, @PasswordHash, @SecurityStamp, " +
                        "@PhoneNumber, @PhoneNumberConfirmed, @TwoFactorEnabled, @LockoutEndDateUtc, @LockoutEnabled, " +
                        "@AccessFailedCount, @UserName)", user);
            });
        }

        public virtual Task DeleteAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
 
            return Task.Factory.StartNew(() =>
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                    connection.Execute("delete from AspNetUsers where Id = @Id", new { user.Id });
            });
        }

        public virtual Task<TUser> FindByIdAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException("userId");
 
            Guid parsedUserId;
            if (!Guid.TryParse(userId, out parsedUserId))
                throw new ArgumentOutOfRangeException("Id", string.Format("'{0}' is not a valid GUID.", new { userId }));
 
            return Task.Factory.StartNew(() =>
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                    return connection.Query<TUser>("select * from AspNetUsers where Id = @Id", new { Id = userId }).SingleOrDefault();
            });
        }

        public virtual Task<TUser> FindByNameAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentNullException("userName");
 
            return Task.Factory.StartNew(() =>
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    return connection.Query<TUser>("select * from AspNetUsers where lower(UserName) = lower(@userName)",
                    new {userName}).SingleOrDefault();
                }
            });
        }

        public virtual Task UpdateAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
 
            return Task.Factory.StartNew(() =>
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                    connection.Execute(
                        "update AspNetUsers set Email = @Email, Emailconfirmed = @Emailconfirmed, " +
                        "PasswordHash = @PasswordHash, SecurityStamp = @SecurityStamp, " +
                        "PhoneNumber = @PhoneNumber, PhoneNumberConfirmed = @PhoneNumberConfirmed " +
                        "TwoFactorEnabled = @TwoFactorEnabled, LockoutEndDateUtc = @LockoutEndDateUtc " +
                        "LockoutEnabled = @LockoutEnabled, AccessFailedCount = @AccessFailedCount " +
                        "UserName = @UserName where Id = @Id", user);
            });
        }
        #endregion
 
        #region IUserLoginStore
        public virtual Task AddLoginAsync(TUser user, UserLoginInfo login)
        {
            if (user == null)
                throw new ArgumentNullException("user");
 
            if (login == null)
                throw new ArgumentNullException("login");
 
            return Task.Factory.StartNew(() =>
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                    connection.Execute("insert into AspNetUserLogins(Id, LoginProvider, ProviderKey) values(@Id, @loginProvider, @providerKey)",
                        new { Id = user.Id, loginProvider = login.LoginProvider, providerKey = login.ProviderKey });
            });
        }

        public virtual Task<TUser> FindAsync(UserLoginInfo login)
        {
            if (login == null)
                throw new ArgumentNullException("login");
 
            return Task.Factory.StartNew(() =>
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                    return connection.Query<TUser>("select u.* from AspNetUsers u inner join AspNetUserLogins l on l.Id = u.Id where l.LoginProvider = @loginProvider and l.ProviderKey = @providerKey",
                        login).SingleOrDefault();
            });
        }

        public virtual Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
 
            return Task.Factory.StartNew(() =>
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                    return (IList<UserLoginInfo>)connection.Query<UserLoginInfo>("select LoginProvider, ProviderKey from AspNetUserLogins where Id = @Id", new { user.Id }).ToList();
            });
        }

        public virtual Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {
            if (user == null)
                throw new ArgumentNullException("user");
 
            if (login == null)
                throw new ArgumentNullException("login");
 
            return Task.Factory.StartNew(() =>
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                    connection.Execute("delete from AspNetUserLogins where Id = @Id and LoginProvider = @loginProvider and ProviderKey = @providerKey",
                        new { user.Id, login.LoginProvider, login.ProviderKey });
            });
        }
        #endregion
 
        #region IUserPasswordStore
        public virtual Task<string> GetPasswordHashAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
 
            return Task.FromResult(user.PasswordHash);
        }

        public virtual Task<bool> HasPasswordAsync(TUser user)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
        }

        public virtual Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            if (user == null)
                throw new ArgumentNullException("user");
 
            user.PasswordHash = passwordHash;
 
            return Task.FromResult(0);
        }
 
        #endregion
 
        #region IUserSecurityStampStore
        public virtual Task<string> GetSecurityStampAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
 
            return Task.FromResult(user.SecurityStamp);
        }

        public virtual Task SetSecurityStampAsync(TUser user, string stamp)
        {
            if (user == null)
                throw new ArgumentNullException("user");
 
            user.SecurityStamp = stamp;
 
            return Task.FromResult(0);
        }
 
        #endregion

        public Task SetEmailAsync(TUser user, string email)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetEmailAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(TUser user)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailConfirmedAsync(TUser user, bool confirmed)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.EmailConfirmed = confirmed;
            return UpdateAsync(user);
        }

        public Task<TUser> FindByEmailAsync(string Email)
        {
            return Task.Factory.StartNew(() =>
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                    return connection.Query<TUser>("select * from AspNetUsers where Email = @Email",
                        new { Email }).SingleOrDefault();
            }
            );
        }
    }
}
