using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.v1.Newsletter.Model
{
    public class NewsletterSubscribeInput
    {
        public string Address { get; set; }
        public string Name { get; set; }
    }
}