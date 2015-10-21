using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Campaign.Model
{
    public class VoucherRequest
    {
        public String VoucherCode { get; set; }
        public String Email { get; set; }
        public decimal Price { get; set; }
    }
}
