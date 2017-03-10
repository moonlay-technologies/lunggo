using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Http.Results;
using Lunggo.ApCommon.Identity.Model;
using Lunggo.ApCommon.Identity.Roles;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.WebAPI.ApiSrc.Account.Model;

namespace Lunggo.WebAPI.ApiSrc.Account.Logic
{
    public static partial class AccountLogic
    {
        public static GetUserResponse GetUser()
        {
            var userId = HttpContext.Current.User.Identity.GetUser().Id;
            var response = User.GetAllUserByCompanyId(userId);
            if (response == null)
                return new GetUserResponse
                {
                    StatusCode = HttpStatusCode.Accepted
                };
            foreach (var user in response)
            {
                user.RoleName = Role.GetFromDb(user.UserId);
                if(!string.IsNullOrEmpty(user.ApproverId))
                user.ApproverName = User.GetNameByUserId(user.ApproverId);
            }
            var roles = User.GetAllRoles();
            //var approvers = User.GetAvailableApprover();
            var approvers = response.Select(x => new ApproverData
            {
                UserId = x.UserId,
                Name = x.FirstName + " " + x.LastName
            }).ToList();
            return new GetUserResponse
            { 
                StatusCode = HttpStatusCode.OK,
                Users = response,
                Roles = roles,
                Approvers = approvers
            };
        }
    }
}