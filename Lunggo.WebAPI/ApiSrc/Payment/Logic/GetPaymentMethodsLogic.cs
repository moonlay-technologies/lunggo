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
                                Account = "1234567890",
                                Branch = "Ranting",
                                Name = "PT. Travel Madezy Internasional",
                                LogoUrl = "https://ib.bankmandiri.co.id/retail/images/mandiri_logo.gif",
                                Available = true
                            },
                            new BankDetails
                            {
                                Bank = 0,
                                Account = "0987654321",
                                Branch = "Dahan",
                                Name = "PT. Travel Madezy Internasional",
                                LogoUrl = "https://upload.wikimedia.org/wikipedia/en/thumb/2/27/BankNegaraIndonesia46-logo.svg/1280px-BankNegaraIndonesia46-logo.svg.png",
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
                    }
                }
            };
        }
    }
}