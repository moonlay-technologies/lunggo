using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Lunggo.ApCommon.Campaign.Model;
using Lunggo.ApCommon.Campaign.Service;
using Lunggo.ApCommon.Flight.Constant;

using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Payment;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Sequence;
using Lunggo.ApCommon.Voucher;
using System.Diagnostics;
using Lunggo.Framework.Config;
using Lunggo.Framework.Context;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public decimal GetCurrency(string curr, Supplier supp)
        {
            
            var response = CurrencyGetterInternal(curr, supp);
            return response;
        }

        public decimal CurrencyGetterInternal(string currency, Supplier supplierName)
        {
            var supplier = Suppliers.Where(entry => entry.Value.SupplierName == supplierName).Select(entry => entry.Value).Single();
            var result = supplier.CurrencyGetter(currency);
                
            return result;
        }
    }
}
