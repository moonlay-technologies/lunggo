using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Wrapper;
using Lunggo.ApCommon.Flight.Wrapper.AirAsia;
using Lunggo.ApCommon.Flight.Wrapper.Citilink;
using Lunggo.ApCommon.Flight.Wrapper.Garuda;
using Lunggo.ApCommon.Flight.Wrapper.LionAir;
using Lunggo.ApCommon.Flight.Wrapper.Mystifly;
using Lunggo.ApCommon.Flight.Wrapper.Sriwijaya;
using Lunggo.ApCommon.Product.Service;
using Lunggo.ApCommon.Voucher;
using Lunggo.Framework.Config;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService : ProductServiceBase<HotelService>
    {
        private bool _isInitialized;

        internal override void Issue(string rsvNo)
        {
            throw new NotImplementedException();
        }

        public HotelService()
        {

        }

        public void Init(string dictionaryFolderName)
        {
            if (!_isInitialized)
            {
                InitDictionary(dictionaryFolderName);

                //foreach (var supplier in Suppliers.Select(entry => entry.Value))
                //{
                //    supplier.Init();
                //}

                //ServicePointManager.ServerCertificateValidationCallback +=
                //    (sender, certificate, chain, sslPolicyErrors) => true;

                //VoucherService.GetInstance().Init();
                //InitPriceMarginRules();
                _isInitialized = true;
            }
        }
    }
}
