using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.v1.Promo.Imlek.Model
{
    public class ImlekTable
    {
        public string Email { get; set; }
        public int RetryCount { get; set; }
        public DateTime LastTryDate { get; set; }
    }
}