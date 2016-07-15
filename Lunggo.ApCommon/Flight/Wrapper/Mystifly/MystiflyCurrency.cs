using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Constant;

namespace Lunggo.ApCommon.Flight.Wrapper.Mystifly

{
    internal partial class MystiflyWrapper
    {
       
        internal override Currency CurrencyGetter(string currencyName)
        {
            return new Currency(currencyName, Supplier.Mystifly);
        }   
    }
}

