using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Constant;

namespace Lunggo.ApCommon.Flight.Wrapper.Sriwijaya

{
    internal partial class SriwijayaWrapper
    {
        internal override Currency CurrencyGetter(string currency)
        {
            return Client.CurrencyGetter(currency);
        }

        private partial class SriwijayaClientHandler
        {
            internal Currency CurrencyGetter(string currencyName)
            {
                return new Currency(currencyName, Supplier.Sriwijaya);
            }
        }
     }
 }

