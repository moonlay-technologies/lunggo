using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Payment.Model
{
    public class SavedCreditCard
    {
        public string CompanyId { get; set; }
        public string MaskedCardNumber { get; set; }
        public Boolean? IsPrimaryCard { get; set; }
        public string Token { get; set; }
        public string CardHolderName { get; set; }
        public DateTime CardExpiry { get; set; }
        public int CardExpiryMonth { get; set; }
        public int CardExpiryYear { get; set; }
        public DateTime TokenExpiry { get; set; }
    }
}
