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
                        Banks = new List<BankDetails>
                        {
                            new BankDetails
                            {
                                Bank = 0,
                                Account = "1020006675802",
                                Branch = "KC Jakarta Sudirman",
                                Name = "Travel Madezy Internasional",
                                LogoUrl = "https://ib.bankmandiri.co.id/retail/images/mandiri_logo.gif",
                                Available = true
                            }
                        },
                        Available = true
                    },
                    new Option
                    {
                        Method = PaymentMethod.CreditCard,
                        Available = true
                    },
                    new Option
                    {
                        Method = PaymentMethod.MandiriClickPay,
                        Available = true
                    },
                    new Option
                    {
                        Method = PaymentMethod.VirtualAccount,
                        Available = true
                    },
                    new Option
                    {
                        Method = PaymentMethod.CimbClicks,
                        Available = true
                    }
                }
            };
        }
    }
}