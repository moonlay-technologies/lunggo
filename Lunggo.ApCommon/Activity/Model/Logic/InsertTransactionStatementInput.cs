using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Model.Logic
{
    public class InsertTransactionStatementInput
    {
        public string TrxNo { get; set; }
        public string Remarks { get; set; }
        public DateTime? DateTime { get; set; }
        public Decimal? Amount { get; set; }
        public string OperatorId { get; set; }
    }
}
