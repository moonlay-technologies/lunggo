using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Payment.Model
{
    public class CheckoutInput
    {
        public string CartId;
        public List<string> RsvNoList;
        public decimal TotalPrice;
        public decimal Promo;
    }
}
