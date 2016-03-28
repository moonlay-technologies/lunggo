using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Lunggo.CloudApp.CaptchaReader.Models;

namespace Lunggo.CloudApp.CaptchaReader.Controllers
{
    public class LionAirAccountController : ApiController
    {

        [HttpGet]
        public string ChooseUserId()
        {

            return Account.GetUserId();
        }

        [HttpGet]
        public bool LogOut(string userId)
        {
            return Account.LogOutAck(userId);
        }
    }
}
