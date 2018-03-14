using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Account.Model.Logic
{
    public class GetReferralOutput
    {
        public string UserId { get; set; }
        public decimal ReferralCredit { get; set; }
        public string ReferralCode { get; set; }
        public DateTime ExpDate { get; set; }
        public string ShareableLink { get; set; }
    }
}
