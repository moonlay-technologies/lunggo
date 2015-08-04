using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Veritrans;
using Lunggo.ApCommon.Veritrans.Model;
using Lunggo.Framework.Http;
using Lunggo.Framework.Payment.Data;
using HttpRequest = System.Web.HttpRequest;

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

        public string GetPaymentUrl(TransactionDetails transactionDetails, List<ItemDetails> itemDetails, PaymentMethod method)
        {
            var url = new UrlHelper();
            return method == PaymentMethod.BankTransfer 
                ? null
                : GetThirdPartyPaymentUrl(transactionDetails, itemDetails, method);
        }


        private static string GetThirdPartyPaymentUrl(TransactionDetails transactionDetails, List<ItemDetails> itemDetails, PaymentMethod method)
        {
            var url = VeritransWrapper.GetPaymentUrl(transactionDetails, itemDetails, method);
            return url;
        }
    }
}
