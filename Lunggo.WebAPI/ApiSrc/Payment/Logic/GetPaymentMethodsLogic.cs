using System.Collections.Generic;
using System.Net;
using System.Security.Principal;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Payment.Model;

namespace Lunggo.WebAPI.ApiSrc.Payment.Logic
{
    public static partial class PaymentLogic
    {
        public static ApiResponseBase GetMethods()
        {
            return new GetMethodsApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                Options = new List<Option>
                {
                    new Option
                    {
                        Method = PaymentMethod.BankTransfer,
                        Available = true
                    },
                    new Option
                    {
                        Method = PaymentMethod.CreditCard,
                        Available = true
                    },
                    new Option
                    {
                        Method = PaymentMethod.CimbClicks,
                        Available = true
                    },
                    new Option
                    {
                        Method = PaymentMethod.MandiriClickPay,
                        Available = true
                    },
                    new Option
                    {
                        Method = PaymentMethod.MandiriBillPayment,
                        Available = true
                    },
                }
            };
        }
    }
}