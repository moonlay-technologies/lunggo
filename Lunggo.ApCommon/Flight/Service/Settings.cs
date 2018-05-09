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
using Lunggo.ApCommon.Payment.Service;
using Lunggo.ApCommon.Product.Service;
using Lunggo.ApCommon.Voucher;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService : ProductServiceBase<FlightService>
    {
        private bool _isInitialized;
        private PaymentService _paymentService = new PaymentService();

        private static readonly MystiflyWrapper MystiflyWrapper = MystiflyWrapper.GetInstance();
        private static readonly AirAsiaWrapper AirAsiaWrapper = AirAsiaWrapper.GetInstance();
        private static readonly CitilinkWrapper CitilinkWrapper = CitilinkWrapper.GetInstance();
        private static readonly SriwijayaWrapper SriwijayaWrapper = SriwijayaWrapper.GetInstance();
        private static readonly LionAirWrapper LionAirWrapper = LionAirWrapper.GetInstance();
        private static readonly GarudaWrapper GarudaWrapper = GarudaWrapper.GetInstance();
        private static readonly Dictionary<int, FlightSupplierWrapperBase> Suppliers = new Dictionary<int, FlightSupplierWrapperBase>()
        {
            //{ 1, MystiflyWrapper},
            { 2, AirAsiaWrapper},
            { 3, CitilinkWrapper},
            { 4, SriwijayaWrapper},
            { 5, LionAirWrapper},
            //{ 6, GarudaWrapper}
        };

        private FlightService()
        {

        }

        public void Init(string dictionaryFolderName)
        {
            if (!_isInitialized)
            {
                InitDictionary(dictionaryFolderName);

                foreach (var supplier in Suppliers.Select(entry => entry.Value))
                {
                    supplier.Init();
                }

                ServicePointManager.ServerCertificateValidationCallback +=
                    (sender, certificate, chain, sslPolicyErrors) => true;

                VoucherService.GetInstance().Init();
                InitPriceMarginRules();
                _isInitialized = true;
            }
        }
    }
}
