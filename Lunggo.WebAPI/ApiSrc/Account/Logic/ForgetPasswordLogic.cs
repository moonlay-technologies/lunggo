using Lunggo.WebAPI.ApiSrc.Account.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace Lunggo.WebAPI.ApiSrc.Account.Logic
{
    public static partial class AccountLogic
    {
        public static ForgetPasswordApiResponse ForgetPassword(ForgetPasswordApiRequest request)
        {
            if (request == null)
            {
                return new ForgetPasswordApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"
                };
            }

            var rng = new RNGCryptoServiceProvider();
            byte[] a = { 1, 2, 3, 4, 5, 6 };
            rng.GetBytes(a);
            var b = BitConverter.ToInt32(a,0);
            

            return null;
        }
    }
}