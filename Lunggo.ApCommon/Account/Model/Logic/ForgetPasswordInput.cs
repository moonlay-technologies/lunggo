using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Account.Model.Logic
{
    public class RequestOtpInput
    {
        public string CountryCallCd { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
