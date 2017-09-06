using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Lunggo.ApCommon.Campaign.Model;
using Lunggo.ApCommon.Campaign.Service;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Query;
using Lunggo.ApCommon.Payment.Wrapper.Nicepay;
using Lunggo.ApCommon.Payment.Wrapper.Veritrans;
using Lunggo.Framework.BlobStorage;
using Lunggo.Framework.Database;
using Lunggo.Framework.Pattern;
using Lunggo.Framework.Queue;
using Lunggo.Framework.SharedModel;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Lunggo.ApCommon.Payment.Service
{
    public partial class PaymentService : SingletonBase<PaymentService>
    {
        private static readonly VeritransWrapper VeritransWrapper = VeritransWrapper.GetInstance();
        private static readonly NicepayWrapper NicepayWrapper = NicepayWrapper.GetInstance();

        public void Init()
        {
            Currency.SyncCurrencyData();
            VeritransWrapper.Init();
            NicepayWrapper.Init();
        }
    }
}
