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
