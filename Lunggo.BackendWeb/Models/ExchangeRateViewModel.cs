using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lunggo.ApCommon.Payment.Constant;

namespace Lunggo.BackendWeb.Models
{
    public class ExchangeRateViewModel
    {
        
        public string CurrencyCode{ get; set; }
        public decimal Rate { get; set; }
        public int Hidden { get; set; }
    }



}