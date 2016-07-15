using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Constant;

namespace Lunggo.ApCommon.Flight.Wrapper.Citilink

{
    internal partial class CitilinkWrapper
    {
        internal override Currency CurrencyGetter(string currency)
        {
            return Client.CurrencyGetter(currency);
        }

        private partial class CitilinkClientHandler
        {
            internal Currency CurrencyGetter(string currencyName)
            {
                return new Currency(currencyName, Supplier.Citilink);
            }
        }
     }
 }

