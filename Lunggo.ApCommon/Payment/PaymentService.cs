using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            else
            {
                throw new InvalidOperationException("PaymentService is already initialized");
            }
        }

        public string GetThirdPartyPaymentUrl(TransactionDetail transactionDetail, List<ItemDetail> itemDetails)
        {
            VeritransWrapper.VtWeb(transactionDetail, itemDetails);
        }
    }
}
