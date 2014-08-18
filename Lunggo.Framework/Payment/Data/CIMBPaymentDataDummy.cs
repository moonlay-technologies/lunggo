using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Payment.Data
{
    public class CIMBPaymentDataDummy : PaymentDataDummy
    {
        public CIMBClickDummy cimb_clicks { get; set; }

        public CIMBPaymentDataDummy()
        {
            cimb_clicks = new CIMBClickDummy();
        }
    }
    public class CIMBClickDummy
    {
        public string description { get; set; }
    }
}
