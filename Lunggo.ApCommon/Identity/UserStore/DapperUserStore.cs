using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Lunggo.ApCommon.Identity.Query;
using Lunggo.ApCommon.Identity.Query.Record;

using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Sequence;
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
    IUserLoginStore<TUser, string>,
    IUserClaimStore<TUser, string>,
    IUserRoleStore<TUser, string>,
    IUserPasswordStore<TUser, string>,
    IUserSecurityStampStore<TUser, string>,
    IUserEmailStore<TUser, string>,
    IUserPhoneNumberStore<TUser, string>,
    IUserTwoFactorStore<TUser, string>,
    IQueryableUserStore<TUser>,
    IUserLockoutStore<TUser, string>, IUserStore<TUser> where TUser : UserBase<string>, new()
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
            get
            {
                using (var connection = DbService.GetInstance().GetOpenConnection())
                {
                    var query = GetAllUserQuery.GetInstance();
                    var record = query.Execute(connection,new{});
                    List<TUser> ListData = new List<TUser>();
                    return record.Select(x => ToUser(x)).AsQueryable();
                }
            }
        }

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
                    var repo = UserTableRepo.GetInstance();
                    user.Id = UserIdSequence.GetInstance().GetNext().ToString(CultureInfo.InvariantCulture);
                    var newUserRecord = ToUsersTableRecordForInsert(user);
                    repo.Insert(connection, newUserRecord);
                }
            });
        }

        private UserTableRecord ToUsersTableRecordForInsert(TUser user)
        {
            var record = new UserTableRecord
            {
                AccessFailedCount = user.AccessFailedCount,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                Id = user.Id,
                CompanyId = user.CompanyId,
                LockoutEnabled = user.LockoutEnabled,
                LockoutEndDateUtc = user.LockoutEndDateUtc,
                PasswordHash = user.PasswordHash,
                CountryCallCd = user.CountryCallCd,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                SecurityStamp = user.SecurityStamp,
                TwoFactorEnabled = user.TwoFactorEnabled,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                Position = user.Position,
                Branch = user.Branch,
                Department = user.Department,
                ApproverId = user.Approver == null? null : user.Approver.Id
            };
            return record;
        }

        private UserRoleTableRecord ToUserRolesTableRecord(string roleId, string userId)
        {
            var record = new UserRoleTableRecord
            {
                RoleId = roleId,
                UserId = userId
            };
            return record;
        }

        private UserClaimTableRecord ToUserClaimsTableRecord(string userId, string claimValue, string claimType)
        {
            var record = new UserClaimTableRecord
            {
                UserId = userId,
                ClaimType = claimType,
                ClaimValue = claimValue
            };
            return record;
        }

        private UserTableRecord ToUsersTableRecordPkOnly(TUser user)
        {
            var record = new UserTableRecord
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
                    var repo = UserTableRepo.GetInstance();
                    var toBeDeletedUserRecord = ToUsersTableRecordPkOnly(user);
                    repo.Delete(connection, toBeDeletedUserRecord);
                }

            });
        }

        public virtual Task<TUser> FindByIdAsync(string userId)
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
            if (record == null)
                return null;
            var user = new TUser
            {
                Email = record.Email,
                AccessFailedCount = record.AccessFailedCount,
                UserName = record.UserName,
                FirstName = record.FirstName,
                LastName = record.LastName,
                Address = record.Address,
                EmailConfirmed = record.EmailConfirmed,
                CompanyId = record.CompanyId,
                Id = record.Id,
                LockoutEnabled = record.LockoutEnabled,
                LockoutEndDateUtc = record.LockoutEndDateUtc,
                PasswordHash = record.PasswordHash,
                CountryCallCd = record.CountryCallCd,
                PhoneNumber = record.PhoneNumber,
                PhoneNumberConfirmed = record.PhoneNumberConfirmed,
                SecurityStamp = record.SecurityStamp,
                TwoFactorEnabled = record.TwoFactorEnabled,
                Position = record.Position,
                Branch = record.Branch,
                Department = record.Department,
                Approver = User.GetFromDb(record.ApproverId)
            };
            return user;
        }

        public virtual Task<TUser> FindByNameAsync(string userName)
        {
            return Task.Factory.StartNew(() =>
            {
                if (string.IsNullOrWhiteSpace(userName))
                    return null;

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
                    var repo = UserTableRepo.GetInstance();
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
                    var repo = UserLoginTableRepo.GetInstance();
                    var toBeInsertedRecord = ToUserLoginsTableRecord(user, login);
                    repo.Insert(connection, toBeInsertedRecord);
                }

            });
        }

        private UserLoginTableRecord ToUserLoginsTableRecord(TUser user, UserLoginInfo login)
        {
            var record = new UserLoginTableRecord
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
                    var repo = UserLoginTableRepo.GetInstance();
                    var toBeDeletedRecord = ToUserLoginTableRecordPkOnly(user, login);
                    repo.Delete(connection, toBeDeletedRecord);
                }

            });
        }

        private UserLoginTableRecord ToUserLoginTableRecordPkOnly(TUser user, UserLoginInfo login)
        {
            var record = new UserLoginTableRecord
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
                {
                    var query = GetUserByEmailQuery.GetInstance();
                    var record = query.Execute(connection, new {Email = Email}).SingleOrDefault();
                    var user = ToUser(record);
                    return user;
                }
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
                var query = GetListClaimByUserIdQuery.GetInstance();
                dynamic claims = query.Execute(connection, new {Id = user.Id}).ToList();
                foreach (dynamic row in claims)
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
            {
                var repo = UserClaimTableRepo.GetInstance();
                var toUserClaimsTableRecord = ToUserClaimsTableRecord(user.Id, claim.Value, claim.Type);
                repo.Insert(connection, toUserClaimsTableRecord);
            }
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
            using (var connection = DbService.GetInstance().GetOpenConnection())
            {
                var repo = UserClaimTableRepo.GetInstance();
                var toUserClaimsTableRecord = ToUserClaimsTableRecord(user.Id, claim.Value, claim.Type);
                repo.Delete(connection, toUserClaimsTableRecord);
            }
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
                var query = GetRoleByNameQuery.GetInstance();
                roleEntity = query.Execute(connection, new {Name = roleName}).SingleOrDefault();
            }
            if (roleEntity == null)
            {
                throw new Exception("Role name not found");
                //Role not found
            }
            using (var connection = DbService.GetInstance().GetOpenConnection())
            {
                var repo = UserRoleTableRepo.GetInstance();
                var toUserRolesTableRecord = ToUserRolesTableRecord(roleEntity.Id, user.Id);
                repo.Insert(connection, toUserRolesTableRecord);
            }
            return Task.FromResult(0);
        }

        public Task AddToRolesAsync(TUser user, List<string>rolesNameList )
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (rolesNameList == null)
            {
                throw new Exception("Role must not null or empty");
                //input Role null
            }
            foreach (var role in rolesNameList)
            {
                dynamic roleEntity;
                using (var connection = DbService.GetInstance().GetOpenConnection())
                {
                    var query = GetRoleByNameQuery.GetInstance();
                    roleEntity = query.Execute(connection, new { Name = role }).SingleOrDefault();
                }
                if (roleEntity == null)
                {
                    throw new Exception("Role name not found");
                    //Role not found
                }
                using (var connection = DbService.GetInstance().GetOpenConnection())
                {
                    var repo = UserRoleTableRepo.GetInstance();
                    var toUserRolesTableRecord = ToUserRolesTableRecord(roleEntity.Id, user.Id);
                    repo.Insert(connection, toUserRolesTableRecord);
                }
                
            }
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
                var query = GetRoleByNameQuery.GetInstance();
                roleEntity = query.Execute(connection, new {Name = roleName}).SingleOrDefault();
            }
            if (roleEntity != null)
            {
                var roleId = roleEntity.Id;
                var userId = user.Id;
                using (var connection = DbService.GetInstance().GetOpenConnection())
                {
                    var repo = UserRoleTableRepo.GetInstance();
                    var toUserRolesTableRecord = ToUserRolesTableRecord(roleEntity.Id, user.Id);
                    repo.Delete(connection, toUserRolesTableRecord);
                }
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
            {
                var query = GetListRoleByUserIdQuery.GetInstance();
                listRole = query.Execute(connection, new {UserId = user.Id}).ToList();
            }
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
            dynamic role, recordAny;

            using (var connection = DbService.GetInstance().GetOpenConnection())
            {
                var query = GetRoleByNameQuery.GetInstance();
                role = query.Execute(connection, new {Name = roleName}).SingleOrDefault();
            }
            if (role != null)
            {
                using (var connection = DbService.GetInstance().GetOpenConnection())
                {
                    var query = GetUserRolesByRoleIdAndUserIdQuery.GetInstance();
                    recordAny = query.Execute(connection, new {RoleId = role.Id, UserId = user.Id}).SingleOrDefault();
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
            user.LockoutEndDateUtc = lockoutEnd == DateTimeOffset.MinValue ? (DateTime?) null : lockoutEnd.UtcDateTime;
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

        public Task<bool> IsLockedOut(string userId)
        {
            //ThrowIfDisposed();
            //if (userId == null)
            //{
            //    throw new ArgumentNullException("user");
            //}

            //using (var connection = DbService.GetInstance().GetOpenConnection())
            //{
            //    var userData = false;
            //    var user = User.GetFromDb(userId);
            //    if (user == null)
            //        userData = false;
            //    else
            //        any = true;
            //}
            return Task.FromResult(true);
        }
    
        public Task SetLockoutEnabledAsync(TUser user, bool enabled)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.LockoutEnabled = enabled;
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                UserTableRepo.GetInstance().Update(conn, new UserTableRecord
                {
                    Id = user.Id,
                    LockoutEnabled = true
                });
            }
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
