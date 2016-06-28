using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.v1.Newsletter.Model
{
    public class NewsletterSubscribeOutput
    {
        public bool IsSuccess { get; set; }
        public bool IsMemberExist { get; set; }
    }
}