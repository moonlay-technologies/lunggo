using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.Hotel.ViewModels;

namespace Lunggo.CustomerWeb.Areas.UW100.Models
{
    public class UW100SearchParamViewModel : HotelSearchBase
    {
        public string SortCode { get; set; }
        public int Page { get; set; }
    }
}