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
using Lunggo.WebAPI.ApiSrc.Common.Model;
using ServiceStack.Text;

namespace Lunggo.WebAPI.ApiSrc.Account.Logic
{
    public static partial class AccountLogic
    {
        public static GetUserResponse GetUser(GetUserRequest request)
        {
            var userId = HttpContext.Current.User.Identity.GetUser().Id;
            var apiRequest = PreProcessRequest(request);
            var response = User.GetAllUserByCompanyId(userId, apiRequest);
            if (response == null)
                return new GetUserResponse
                {
                    StatusCode = HttpStatusCode.Accepted
                };
            
            var roles = User.GetAllRoles();
            //var approvers = User.GetAvailableApprover();
            var approvers = (from user in response
                where user.RoleName.Contains("Approver")
                select new ApproverData
                {
                    UserId = user.UserId, Name = user.FirstName + " " + user.LastName
                }).ToList();
            //var approvers = response.Select(x => new ApproverData
            //{
            //    UserId = x.UserId,
            //    Name = x.FirstName + " " + x.LastName
            //}).ToList();
            return new GetUserResponse
            { 
                StatusCode = HttpStatusCode.OK,
                Users = response,
                Roles = roles,
                Approvers = approvers
            };
        }

        public static FilterSortingModel PreProcessRequest(GetUserRequest request)
        {
            if(request == null)
                return new FilterSortingModel();
            var userCondition = new FilterSortingModel
            {
                Sorting = request.Sorting,
                Name = request.Filter.Name == null ? null : request.Filter.Name.ToLower(),
                Email = request.Filter.Email == null ? null : request.Filter.Email.ToLower(),
                Branch = request.Filter.Branch == null ? null : request.Filter.Branch.ToLower(),
                Department = request.Filter.Department == null ? null : request.Filter.Department.ToLower(),
                Position = request.Filter.Position == null ? null : request.Filter.Position.ToLower(),
                Roles = request.Filter.Roles
            };
            return userCondition;
        }
    }
}