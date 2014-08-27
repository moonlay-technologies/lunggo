﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Lunggo.ApCommon.Identity.User;
using Lunggo.ApCommon.Query;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using Microsoft.AspNet.Identity;

namespace Lunggo.ApCommon.Identity.UserStore
{
    
    /*public class DapperUserStore<TUser> :
        DapperUserStore<TUser, IdentityRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>,
        IUserStore<TUser> where TUser : IdentityUser
    { 
        public DapperUserStore()
        {
            DisposeContext = true;
        }

    }*/

    public class DapperUserStore<TUser> :
        IUserLoginStore<TUser, long>,
        IUserClaimStore<TUser, long>,
        IUserRoleStore<TUser, long>,
        IUserPasswordStore<TUser, long>,
        IUserSecurityStampStore<TUser, long>,
        IUserEmailStore<TUser, long>,
        IUserPhoneNumberStore<TUser, long>,
        IUserTwoFactorStore<TUser, long>,
        IUserLockoutStore<TUser, long>
        where TKey : IEquatable<long>
        where TUser : UserBase<long>,new()
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
        
        /*
        public IQueryable<TUser> Users
        {
            get { using (var connection = DbService.GetInstance().GetOpenConnection())
                    return connection.Query<TUser>("select * from AspNetUsers").AsQueryable(); }
        }
        */
        #region IUserStore
        public virtual Task CreateAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
                
            return Task.Factory.StartNew(() =>
            {
                using (var connection = DbService.GetInstance().GetOpenConnection())
                {
                    var repo = UsersTableRepo.GetInstance();
                    var newUserRecord = ToUsersTableRecordForInsert(user);
                    repo.Insert(connection, newUserRecord);                                                      
                }    
            });
        }

        private UsersTableRecord ToUsersTableRecordForInsert(TUser user)
        {
            var record = new UsersTableRecord
            {
               AccessFailedCount = user.AccessFailedCount,
               Email = user.Email,
               EmailConfirmed = user.EmailConfirmed,
               Id = user.Id,
               LockoutEnabled = user.LockoutEnabled,
               LockoutEndDateUtc = user.LockoutEndDateUtc,
               PasswordHash = user.PasswordHash,
               PhoneNumber = user.PhoneNumber,
               PhoneNumberConfirmed = user.PhoneNumberConfirmed,
               SecurityStamp = user.SecurityStamp,
               TwoFactorEnabled = user.TwoFactorEnabled,
               UserName = user.UserName
            };
            return record;
        }

        private UsersTableRecord ToUsersTableRecordPkOnly(TUser user)
        {
            var record = new UsersTableRecord
            {
                Id = user.Id
            };
            return record;
        }


        public virtual Task DeleteAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
                
            return Task.Factory.StartNew(() =>
            {
                using (var connection = DbService.GetInstance().GetOpenConnection())
                {
                    var repo = UsersTableRepo.GetInstance();
                    var toBeDeletedUserRecord = ToUsersTableRecordPkOnly(user);
                    repo.Delete(connection, toBeDeletedUserRecord);
                }
                    
            });
        }

        public virtual Task<TUser> FindByIdAsync(long userId)
        { 
            return Task.Factory.StartNew(() =>
            {
                using (var connection = DbService.GetInstance().GetOpenConnection())
                {
                    var query = GetUserByIdQuery.GetInstance();
                    var record = query.Execute(connection, new { Id = userId }).SingleOrDefault();
                    var user = ToUser(record);
                    return user;
                }
                    
            });
        }

        private TUser ToUser(GetUserByAnyQueryRecord record)
        {
            var user = new TUser
            {
                Email = record.Email,
                AccessFailedCount = record.AccessFailedCount,
                UserName = record.UserName,
                EmailConfirmed = record.EmailConfirmed,
                Id = record.Id,
                LockoutEnabled = record.LockoutEnabled,
                LockoutEndDateUtc = record.LockoutEndDateUtc,
                PasswordHash = record.PasswordHash,
                PhoneNumber = record.PhoneNumber,
                PhoneNumberConfirmed = record.PhoneNumberConfirmed,
                SecurityStamp = record.SecurityStamp,
                TwoFactorEnabled = record.TwoFactorEnabled

            };
            return user;
        }

        public virtual Task<TUser> FindByNameAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentNullException("userName");
 
            return Task.Factory.StartNew(() =>
            {
                using (var connection = DbService.GetInstance().GetOpenConnection())
                {
                    var query = GetUserByNameQuery.GetInstance();
                    var record = query.Execute(connection, new {userName = userName}).SingleOrDefault();
                    var user = ToUser(record);
                    return user;
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
                {
                    var repo = UsersTableRepo.GetInstance();
                    var toBeUpdatedUserRecord = ToUsersTableRecordForInsert(user);
                    repo.Update(connection, toBeUpdatedUserRecord); 
                } 
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
                {
                    var repo = UserLoginsTableRepo.GetInstance();
                    var toBeInsertedRecord = ToUserLoginsTableRecord(user, login);
                    repo.Insert(connection, toBeInsertedRecord);
                }
                    
            });
        }

        private UserLoginsTableRecord ToUserLoginsTableRecord(TUser user, UserLoginInfo login)
        {
            var record = new UserLoginsTableRecord
            {
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey,
                UserId = user.Id
            };

            return record;
        }

        public virtual Task<TUser> FindAsync(UserLoginInfo login)
        {
            if (login == null)
                throw new ArgumentNullException("login");
 
            return Task.Factory.StartNew(() =>
            {
                using (var connection = DbService.GetInstance().GetOpenConnection())
                {
                    var query = GetUserByLoginInfoQuery.GetInstance();
                    var record = query.Execute(connection, login).SingleOrDefault();
                    var user = ToUser(record);
                    return user;
                }
                    
            });
        }

        public virtual Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
 
            return Task.Factory.StartNew(() =>
            {
                using (var connection = DbService.GetInstance().GetOpenConnection())
                {
                    var query = GetLoginInfoByUserIdQuery.GetInstance();
                    var recordList = query.Execute(connection, new {user.Id});
                    return ToUserLoginInfoList(recordList);
                }
            });
        }

        private IList<UserLoginInfo> ToUserLoginInfoList(IEnumerable<GetUserLoginInfoByAnyQueryRecord> list)
        {
            var outputList = new List<UserLoginInfo>();
            foreach (var info in outputList)
            {
                outputList.Add(new UserLoginInfo(info.LoginProvider, info.ProviderKey));
            }
            return outputList;
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
                {
                    var repo = UserLoginsTableRepo.GetInstance();
                    var toBeDeletedRecord = ToUserLoginTableRecordPkOnly(user,login);
                    repo.Delete(connection, toBeDeletedRecord);
                }
                    
            });
        }

        private UserLoginsTableRecord ToUserLoginTableRecordPkOnly(TUser user, UserLoginInfo login)
        {
            var record = new UserLoginsTableRecord
            {
                UserId = user.Id,
                ProviderKey = login.ProviderKey,
                LoginProvider = login.LoginProvider
            };

            return record;
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

        public Task<IList<System.Security.Claims.Claim>> GetClaimsAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            IList<System.Security.Claims.Claim> result = new List<System.Security.Claims.Claim>();
            using (var connection = DbService.GetInstance().GetOpenConnection())
            {
                var query = connection.Query("select * from AspNetUserClaims where UserId = @Id",
                    new {user.Id}).ToList();
                foreach (dynamic row in query)
                {
                    result.Add(new System.Security.Claims.Claim(row.ClaimType, row.ClaimValue));
                }
            }
            return Task.FromResult(result);
        }

        public Task AddClaimAsync(TUser user, System.Security.Claims.Claim claim)
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

        public virtual Task RemoveClaimAsync(TUser user, System.Security.Claims.Claim claim)
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
