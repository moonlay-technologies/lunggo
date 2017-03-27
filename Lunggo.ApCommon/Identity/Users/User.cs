using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model;
using Lunggo.ApCommon.Identity.Model;
using Lunggo.ApCommon.Identity.Query;
using Lunggo.ApCommon.Identity.Roles;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.Framework.Database;
using Lunggo.Framework.Queue;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using Microsoft.AspNet.Identity;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Lunggo.ApCommon.Identity.Users
{
    public class User : UserBase<string>
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User, string> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public static User GetFromDb(string userId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var record = GetUserByIdQuery.GetInstance().Execute(conn, new { Id = userId }).SingleOrDefault();
                if (record == null)
                    return null;

                var user = record.ToCustomUser();
                user.Roles = Role.GetFromDb(userId);

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

        internal static List<string> GetListApproverEmailByCompanyId(string companyId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var user = GetListApproverEmailByCompanyIdQuery.GetInstance().Execute(conn, new { CompanyId = companyId }).ToList();
                return user;
            }
        }

        internal static string GetEmailByUserId(string userId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var user = GetEmailByUserIdQuery.GetInstance().Execute(conn, new { userId = userId }).ToList();
                if (user.Count == 0)
                {
                    return null;
                }
                else
                {
                    return user[0];
                }
            }
        }

        internal static List<string> GetListFinanceEmailByCompanyId(string companyId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var user = GetListFinanceEmailByCompanyIdQuery.GetInstance().Execute(conn, new { CompanyId = companyId }).ToList();
                return user;
            }
        }

        internal static List<string> GetListBookerEmail(string userId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var user = GetListBookerEmailQuery.GetInstance().Execute(conn, new { Id = userId }).ToList();
                return user;
            }
        }

        internal static List<string> GetListBookerEmailByCompanyId(string companyId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var user = GetListBookerEmailByCompanyIdQuery.GetInstance().Execute(conn, new { CompanyId = companyId }).ToList();
                return user;
            }
        }

        internal static string GetApprover(string userId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var user = GetApproverByUserIdQuery.GetInstance().Execute(conn, new { userId }).FirstOrDefault();
                return user;
            }
        }

        public static List<ApproverData> GetAvailableApprover()
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var userList = GetAvailableApproverQuery.GetInstance().Execute(conn, new { }).ToList();
                return userList;
            }
        }

        internal static string GetApproverEmailByUserId(string userId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var user = GetApproverEmailByUserIdQuery.GetInstance().Execute(conn, new { userId }).FirstOrDefault();
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

        public static List<UserData> GetAllUserByCompanyId(string userId, FilterSortingModel model)
        {
            var companyId = GetCompanyIdByUserId(userId);
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var userList = GetAllUserByAdminQuery.GetInstance().Execute(conn, new { CompanyId = companyId }).ToList();
                foreach (var user in userList)
                {
                    user.RoleName = Role.GetFromDb(user.UserId);
                    if(!string.IsNullOrEmpty(user.ApproverId))
                        user.ApproverName = GetNameByUserId(user.ApproverId);
                }
                if (model != null)
                {
                    //By Name
                    if (!string.IsNullOrEmpty(model.Name))
                    {
                        userList = userList.Where(x => x.FirstName != null).ToList();
                        userList = userList.Where(x => (x.FirstName.ToLower()+ " " + x.LastName.ToLower()).Contains(model.Name)).ToList();
                    }
                    
                    //By Email
                    if (!string.IsNullOrEmpty(model.Email))
                    {
                        userList = userList.Where(x => x.Email != null).ToList();
                        userList = userList.Where(x => x.Email.ToLower().Contains(model.Email)).ToList();
                    }
                    
                    //By Position
                    if (!string.IsNullOrEmpty(model.Position))
                    {
                        userList = userList.Where(x => x.Position != null).ToList();
                        userList = userList.Where(x => x.Position.ToLower().Contains(model.Position)).ToList();
                    }
                   
                    
                    //By Department
                    if (!string.IsNullOrEmpty(model.Department))
                    {
                        userList = userList.Where(x => x.Department != null).ToList();
                        userList = userList.Where(x => x.Department.ToLower().Contains(model.Department)).ToList();
                    }
                    
                    
                    //By Branch
                    if (!string.IsNullOrEmpty(model.Branch))
                    {
                        userList = userList.Where(x => x.Branch != null).ToList();
                        userList = userList.Where(x => x.Branch.ToLower().Contains(model.Branch)).ToList(); 
                    }
                    
                    //By Role
                    if (model.Roles != null && model.Roles.Count > 0)
                    {
                        userList = userList.Where(x =>x.RoleName.Intersect(model.Roles).Any()).ToList();
                    }
                    
                    if (model.Sorting != null)
                    {
                        userList = SortUser(userList, model.Sorting);
                    }
                }
                return userList;
            }
        }

        public  static List<UserData> SortUser(List<UserData> hotels, string param)
        {
            var sortedUser = new List<UserData>();
            switch (UserSortingTypeCd.Mnemonic(param))
            {
                case UserSortingType.AscendingName:
                    sortedUser = hotels.OrderBy(p => p.FirstName).ToList();
                    break;
                case UserSortingType.DescendingName:
                    sortedUser = hotels.OrderByDescending(p => p.FirstName).ToList();
                    break;
                case UserSortingType.AscendingPosition:
                    sortedUser = hotels.OrderBy(p => p.Position).ToList();
                    break;
                case UserSortingType.DescendingPosition:
                    sortedUser = hotels.OrderByDescending(p => p.Position).ToList();
                    break;
                case UserSortingType.AscendingDepartment:
                    sortedUser = hotels.OrderBy(p => p.Department).ToList();
                    break;
                case UserSortingType.DescendingDepartment:
                    sortedUser = hotels.OrderByDescending(p => p.Department).ToList();
                    break;
                case UserSortingType.AscendingBranch:
                    sortedUser = hotels.OrderBy(p => p.Branch).ToList();
                    break;
                case UserSortingType.DescendingBranch:
                    sortedUser = hotels.OrderByDescending(p => p.Branch).ToList();
                    break;
                case UserSortingType.AscendingEmail:
                    sortedUser = hotels.OrderBy(p => p.Email).ToList();
                    break;
                case UserSortingType.DescendingEmail:
                    sortedUser = hotels.OrderByDescending(p => p.Email).ToList();
                    break;
                default:
                    sortedUser = hotels.OrderBy(p => p.Email).ToList();
                    break;
            }
            return sortedUser;
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

        public static bool UpdateUser(User user)
        {
            try
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    UserTableRepo.GetInstance().Update(conn, new UserTableRecord
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        CountryCallCd = user.CountryCallCd,
                        PhoneNumber = user.PhoneNumber,
                        Position = user.Position,
                        Department = user.Department,
                        Branch = user.Branch,
                        ApproverId = user.Approver.Id
                    });
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static UserForDisplay ConvertUserForDisplay(User user)
        {
            if (user == null)
                return null;
            var displayUser = new UserForDisplay
            {
                Id = user.Id,
                Name = user.FirstName == user.LastName ? user.FirstName : user.FirstName + " " + user.LastName,
                Email = user.Email,
                CountryCallCd = user.CountryCallCd,
                PhoneNumber = user.PhoneNumber,
                Position = user.Position,
                Branch = user.Branch,
                Department = user.Department
                //ApproverName = user.Approver != null ? GetNameByUserId(user.Approver.Id) : null
            };
            return displayUser;
        }

        public static bool UpdateUserLock(string userId, bool isLocked)
        {
            try
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    UserTableRepo.GetInstance().Update(conn, new UserTableRecord
                    {
                        Id = userId,
                        LockoutEnabled = isLocked
                    });
                }

                SendSuspendNotificationToCustomer(userId, isLocked);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static List<UserBookingNotes> GetBookingNotes(string userId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var notes = GetBookingNotesByUserIdQuery.GetInstance().Execute(conn, new { userId }).ToList();
                return notes;
            }
        }

        public static void InsertBookingNotes(string userId, string title, string description)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var bookingNote = new UserBookingNotesTableRecord
                {
                    UserId = userId,
                    Title = title,
                    Description = description
                };

                UserBookingNotesTableRepo.GetInstance().Insert(conn, bookingNote);
            }
            
        }

       public static void SendSuspendNotificationToCustomer(string userId, bool isLocked)
       {
            var email = User.GetEmailByUserId(userId);
            var stringStatus = isLocked? "true" : "false";
            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference("UserSuspendNotifEmail");
            queue.AddMessage(new CloudQueueMessage(email+ "&" + stringStatus));
        }
    }
}