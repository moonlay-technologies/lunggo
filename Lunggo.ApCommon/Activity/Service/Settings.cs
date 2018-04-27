using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.ApCommon.Product.Service;
using Lunggo.ApCommon.Voucher;
using Lunggo.Framework.Config;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService : ProductServiceBase<ActivityService>
    {
        private bool _isInitialized;
        private PaymentService _paymentService = new PaymentService();

        private ActivityService()
        {

        }

        public void Init(string dictionaryFolderName)
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
            }
        }

        //public static object GetInstance()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
