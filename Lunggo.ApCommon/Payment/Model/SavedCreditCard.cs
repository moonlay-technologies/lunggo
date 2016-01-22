using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Payment.Model
{
    public class SavedCreditCard
    {
        public string Email { get; set; }
        public string MaskedCardNumber { get; set; }
        public string Token { get; set; }
        public string CardHolderName { get; set; }
        public DateTime TokenExpiry { get; set; }
    }
}
