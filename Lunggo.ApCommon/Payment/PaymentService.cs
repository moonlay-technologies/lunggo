using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Veritrans;
using Lunggo.Framework.Payment.Data;

namespace Lunggo.ApCommon.Payment
{
    public class PaymentService
    {
        private static readonly PaymentService Instance = new PaymentService();
        private static readonly VeritransWrapper VeritransWrapper = VeritransWrapper.GetInstance();
        private bool _isInitialized;

        private PaymentService()
        {
            
        }

        public static PaymentService GetInstance()
        {
            return Instance;
        }

        public void Init()
        {
            if (!_isInitialized)
            {
                VeritransWrapper.Init();
                _isInitialized = true;
            }
        }

        public void ProcessViaThirdPartyWeb(TransactionDetails transactionDetails, List<ItemDetails> itemDetails, out string url)
        {
            url = GetThirdPartyPaymentUrl(transactionDetails, itemDetails);
        }


        private string GetThirdPartyPaymentUrl(TransactionDetails transactionDetails, List<ItemDetails> itemDetails)
        {
            var url = VeritransWrapper.GetPaymentUrl(transactionDetails, itemDetails);
            return url;
        }
    }
}
