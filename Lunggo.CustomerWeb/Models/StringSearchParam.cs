using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Lunggo.CustomerWeb.Models
{
    public class StringSearchParam
    {
        [Required]
        public string sourceAirportOrArea{get;set;}
        [Required]
        public string destinationAirportOrArea{get;set;}
        [Required]
        public string flightDate{get;set;}
        [Required]
        public string optionsRadios{get;set;}
        public string returnDate{get;set;}
        [Required]
        public string adultPassenger{get;set;}
        [Required]
        public string childPassenger{get;set;}
        [Required]
        public string infantPassenger{get;set;}
    }
}