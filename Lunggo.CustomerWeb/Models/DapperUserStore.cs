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
using Lunggo.Framework.Database;
namespace Lunggo.CustomerWeb.Models
{
    public class CustomUser : IdentityUser
    {
        public Guid UserId { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<CustomUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
    public class DapperUserStore<TUser> :
        DapperUserStore<TUser, IdentityRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>,
        IUserStore<TUser> where TUser : IdentityUser
    { 
        public DapperUserStore()
        {
            DisposeContext = true;
        }

    }
    public class DapperUserStore<TUser, TRole, TKey, TUserLogin, TUserRole, TUserClaim> :
        IUserLoginStore<TUser, TKey>,
        IUserClaimStore<TUser, TKey>,
        IUserRoleStore<TUser, TKey>,
        IUserPasswordStore<TUser, TKey>,
        IUserSecurityStampStore<TUser, TKey>,
        IUserEmailStore<TUser, TKey>,
        IQueryableUserStore<TUser, TKey>,
        IUserPhoneNumberStore<TUser, TKey>,
        IUserTwoFactorStore<TUser, TKey>,
        IUserLockoutStore<TUser, TKey>
        where TKey : IEquatable<TKey>
        where TUser : IdentityUser<TKey, TUserLogin, TUserRole, TUserClaim>
        where TRole : IdentityRole<TKey, TUserRole>
        where TUserLogin : IdentityUserLogin<TKey>, new()
        where TUserRole : IdentityUserRole<TKey>, new()
        where TUserClaim : IdentityUserClaim<TKey>, new()
    {
        private bool _disposed;
        public bool DisposeContext { get; set; }
        public DapperUserStore()
        {
        }
 
        public void Dispose()
        {
 
        }
        protected virtual void Dispose(bool disposing)
        {
            _disposed = true;
        }
        public IQueryable<TUser> Users
        {
            get { using (var connection = DbService.GetInstance().GetOpenConnection())
                    return connection.Query<TUser>("select * from AspNetUsers").AsQueryable(); }
        }
        #region IUserStore
        public virtual Task CreateAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.Factory.StartNew(() => {
                using (var connection = DbService.GetInstance().GetOpenConnection())
                    connection.Execute(
                        "insert into AspNetUsers(Id, Email, EmailConfirmed, PasswordHash, SecurityStamp, " +
                        "PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEndDateUtc, LockoutEnabled, " +
                        "AccessFailedCount, UserName)" +
                        "values(@Id, @Email, @Emailconfirmed, @PasswordHash, @SecurityStamp, " +
                        "@PhoneNumber, @PhoneNumberConfirmed, @TwoFactorEnabled, @LockoutEndDateUtc, @LockoutEnabled, " +
                        "@AccessFailedCount, @UserName)", 
                        new
                        {
                            Id = user.Id,
                            Email = user.Email,
                            EmailConfirmed = user.EmailConfirmed,
                            PasswordHash = user.PasswordHash,
                            SecurityStamp = user.SecurityStamp,
                            PhoneNumber = user.PhoneNumber,
                            PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                            TwoFactorEnabled = user.TwoFactorEnabled,
                            LockoutEndDateUtc = user.LockoutEndDateUtc,
                            LockoutEnabled = user.LockoutEnabled,
                            AccessFailedCount = user.AccessFailedCount,
                            UserName = user.UserName
                        });
            });
        }

        public virtual Task DeleteAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
 
            return Task.Factory.StartNew(() =>
            {
                using (var connection = DbService.GetInstance().GetOpenConnection())
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
                using (var connection = DbService.GetInstance().GetOpenConnection())
                    return connection.Query<TUser>("select * from AspNetUsers where Id = @Id", new { Id = userId }).SingleOrDefault();
            });
        }

        public virtual Task<TUser> FindByNameAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentNullException("userName");
 
            return Task.Factory.StartNew(() =>
            {
                using (var connection = DbService.GetInstance().GetOpenConnection())
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
                using (var connection = DbService.GetInstance().GetOpenConnection())
                    connection.Execute(
                        "update AspNetUsers set Email = @Email, Emailconfirmed = @Emailconfirmed, " +
                        "PasswordHash = @PasswordHash, SecurityStamp = @SecurityStamp, " +
                        "PhoneNumber = @PhoneNumber, PhoneNumberConfirmed = @PhoneNumberConfirmed, " +
                        "TwoFactorEnabled = @TwoFactorEnabled, LockoutEndDateUtc = @LockoutEndDateUtc, " +
                        "LockoutEnabled = @LockoutEnabled, AccessFailedCount = @AccessFailedCount, " +
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
                using (var connection = DbService.GetInstance().GetOpenConnection())
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
                using (var connection = DbService.GetInstance().GetOpenConnection())
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
                using (var connection = DbService.GetInstance().GetOpenConnection())
                    return (IList<UserLoginInfo>)connection.Query<UserLoginInfo>("select LoginProvider, ProviderKey from AspNetUserLogins where UserId = @Id", new { user.Id }).ToList();
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
                using (var connection = DbService.GetInstance().GetOpenConnection())
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
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.Email = email;
            return Task.FromResult(0);
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
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.EmailConfirmed);
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
                using (var connection = DbService.GetInstance().GetOpenConnection())
                    return connection.Query<TUser>("select * from AspNetUsers where Email = @Email",
                        new { Email }).SingleOrDefault();
            }
            );
        }

        public Task<bool> GetTwoFactorEnabledAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task SetTwoFactorEnabledAsync(TUser user, bool enabled)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.TwoFactorEnabled = enabled;
            return Task.FromResult(0);
        }


        public virtual Task<TUser> FindByIdAsync(TKey userId)
        {
            ThrowIfDisposed();
            return FindByIdAsync(userId.ToString());
        }

        public Task<IList<Claim>> GetClaimsAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            IList<Claim> result = new List<Claim>();
            using (var connection = DbService.GetInstance().GetOpenConnection())
            {
                var query = connection.Query("select * from AspNetUserClaims where UserId = @Id",
                    new {user.Id}).ToList();
                foreach (dynamic row in query)
                {
                    result.Add(new Claim(row.ClaimType, row.ClaimValue));
                }
            }
            return Task.FromResult(result);
        }

        public Task AddClaimAsync(TUser user, Claim claim)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (claim == null)
            {
                throw new ArgumentNullException("claim");
            }
            using (var connection = DbService.GetInstance().GetOpenConnection())
                connection.Execute("insert into AspNetUserClaims(UserId, ClaimType, ClaimValue) values(@Id, @Type, @Value)",
                    new { UserId = user.Id, ClaimType = claim.Type, ClaimValue = claim.Value });
            return Task.FromResult(0);
        }

        public virtual Task RemoveClaimAsync(TUser user, Claim claim)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (claim == null)
            {
                throw new ArgumentNullException("claim");
            }
            var claims =
                user.Claims.Where(uc => uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type).ToList();
            foreach (var c in claims)
            {
                user.Claims.Remove(c);
            }
            using (var connection = DbService.GetInstance().GetOpenConnection())
                connection.Execute("delete from AspNetUserClaims where UserId = @Id and ClaimType = @Type and ClaimValue = @Value",
                    new { user.Id, claim.Value, claim.Type });
            return Task.FromResult(0);
        }

        public Task AddToRoleAsync(TUser user, string roleName)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (String.IsNullOrWhiteSpace(roleName))
            {
                throw new Exception("Role must not null or empty");
                //input Role null
            }
            dynamic roleEntity;
            using (var connection = DbService.GetInstance().GetOpenConnection())
            {
                roleEntity = connection.Query("select * from AspNetRoles where Upper(Name) = Upper(@Name)",
                    new { Name = roleName }).SingleOrDefault();
            }

            if (roleEntity == null)
            {
                throw new Exception("Role name not found");
                //Role not found
            }
            using (var connection = DbService.GetInstance().GetOpenConnection())
                connection.Execute("insert into AspNetUserRoles(UserId, RoleId) values(convert(nvarchar(128),@UserId), convert(nvarchar(128),@RoleId))",
                    new { UserId = user.Id, RoleId = roleEntity.Id.ToString() });
            return Task.FromResult(0);
        }

        public Task RemoveFromRoleAsync(TUser user, string roleName)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (String.IsNullOrWhiteSpace(roleName))
            {
                throw new Exception("Role must not null or empty");
            }
            dynamic roleEntity;
            using (var connection = DbService.GetInstance().GetOpenConnection())
            {
                roleEntity = connection.Query("select * from AspNetRoles where Upper(Name) = Upper(@Name)",
                    new { Name = roleName }).SingleOrDefault();
            }
            if (roleEntity != null)
            {
                var roleId = roleEntity.Id;
                var userId = user.Id;
                using (var connection = DbService.GetInstance().GetOpenConnection())
                    connection.Execute("delete from AspNetUserRoles where UserId = @userId and RoleID = @roleId",
                        new { roleId, userId });
            }
            return Task.FromResult(0);
        }

        public Task<IList<string>> GetRolesAsync(TUser user)
        {

            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            IList<string> listRole = new List<string>();
            using (var connection = DbService.GetInstance().GetOpenConnection())
                listRole = connection.Query<string>("select b.Name from AspNetUserRoles a left join  AspNetRoles b on a.RoleId = b.Id where a.UserId =@UserId",
                    new { UserId = user.Id }).ToList();
            return Task.FromResult<IList<string>>(listRole);
        }

        public Task<bool> IsInRoleAsync(TUser user, string roleName)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (String.IsNullOrWhiteSpace(roleName))
            {
                throw new Exception("Role must not null or empty");
            }

            var any = false;
            dynamic role,recordAny;
            using (var connection = DbService.GetInstance().GetOpenConnection())
                role = connection.Query("select * from AspNetRoles where Upper(Name) =@Name",
                    new { Name = roleName }).SingleOrDefault();
            if (role != null)
            {
                using (var connection = DbService.GetInstance().GetOpenConnection())
                {
                    recordAny = connection.Query(
                            "select * from AspNetUserRoles where RoleId = @RoleId and UserId = @UserId",
                            new {RoleId = role.Id, UserId = user.Id}).SingleOrDefault();

                    if (recordAny == null)
                        any = false;
                    else
                        any = true;
                }
            }
            return Task.FromResult(any);
        }

        public Task SetPhoneNumberAsync(TUser user, string phoneNumber)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.PhoneNumber = phoneNumber;
            return Task.FromResult(0);
        }

        public Task<string> GetPhoneNumberAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.PhoneNumberConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return
                Task.FromResult(user.LockoutEndDateUtc.HasValue
                    ? new DateTimeOffset(DateTime.SpecifyKind(user.LockoutEndDateUtc.Value, DateTimeKind.Utc))
                    : new DateTimeOffset());
        }

        public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.LockoutEndDateUtc = lockoutEnd == DateTimeOffset.MinValue ? (DateTime?)null : lockoutEnd.UtcDateTime;
            return Task.FromResult(0);
        }

        public Task<int> IncrementAccessFailedCountAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.AccessFailedCount++;
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.AccessFailedCount = 0;
            return Task.FromResult(0);
        }

        public Task<int> GetAccessFailedCountAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.LockoutEnabled);
        }

        public Task SetLockoutEnabledAsync(TUser user, bool enabled)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.LockoutEnabled = enabled;
            return Task.FromResult(0);
        }
        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
    }
}
