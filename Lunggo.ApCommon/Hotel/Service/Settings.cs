using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Product.Service;
using Lunggo.ApCommon.Voucher;
using Lunggo.Framework.Config;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService : ProductServiceBase<HotelService>
    {
        private bool _isInitialized;

        private HotelService()
        {

        }

        public void Init(string dictionaryFolderName)
        {
            if (!_isInitialized)
            {
                InitDictionary(dictionaryFolderName);
                InitPriceMarginRules();
                //foreach (var supplier in Suppliers.Select(entry => entry.Value))
                //{
                //    supplier.Init();
                //}

                //ServicePointManager.ServerCertificateValidationCallback +=
                //    (sender, certificate, chain, sslPolicyErrors) => true;

                //VoucherService.GetInstance().Init();
                _isInitialized = true;
            }
        }
    }
}
