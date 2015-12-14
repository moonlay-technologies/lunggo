using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Lunggo.ApCommon.Flight.Model;

namespace Lunggo.CustomerWeb.Models
{
    public class CalendarRecipientData
    {
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name = "Address")]
        [DataType(DataType.MultilineText)] 
        public string Address { get; set; }
    }
}