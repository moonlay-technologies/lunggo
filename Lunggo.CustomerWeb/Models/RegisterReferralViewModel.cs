using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Lunggo.CustomerWeb.Models
{
    public class RegisterReferralViewModel
    {
        [Required()]
        [EmailAddress]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required()]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required()]
        [EmailAddress]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Required()]
        [EmailAddress]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required()]
        [EmailAddress]
        [Display(Name = "ReferrerCode")]
        public string ReferrerCode { get; set; }
    }
}