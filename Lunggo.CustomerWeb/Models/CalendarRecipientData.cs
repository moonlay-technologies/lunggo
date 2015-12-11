using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Lunggo.ApCommon.Flight.Model;

namespace Lunggo.CustomerWeb.Models
{
    public class CalendarRecipientData
    {
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Address")]
        [DataType(DataType.MultilineText)] 
        public string Address { get; set; }
    }
}