using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Payment.Data;

namespace Lunggo.ApCommon.Veritrans.Model
{
    internal class Request
    {
        public string PaymentType { get; set; }
        public TransactionDetail TransactionDetail { get; set; }
        public List<ItemDetail> ItemDetail { get; set; }
    }
}
