using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.CustomerWeb.Models
{
    public class RejectionModel
    {
        public string RsvNo { get; set; }
        public string Token { get; set; }
        public string Status { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}