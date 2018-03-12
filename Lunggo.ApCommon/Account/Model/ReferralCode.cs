using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Account.Model
{
    public class ReferralCodeModel
    {
        public string UserId { get; set; }
        public string ReferralCode { get; set; }
        public string ReferrerCode { get; set; }
        public decimal? ReferralCredit { get; set; }
    }
}
