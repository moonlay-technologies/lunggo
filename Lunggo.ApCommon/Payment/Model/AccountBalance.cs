using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Model
{
    public class AccountBalance
    {
        public decimal Balance { get; set; }
        public decimal Withdrawable { get; set; }
    }
}
