using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Payment.Model
{
    public class BankAccount
    {
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string Branch { get; set; }
        public string OwnerName { get; set; }
    }
}
