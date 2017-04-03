﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Payment.Model;

namespace Lunggo.WebAPI.ApiSrc.Payment.Logic
{
    public static partial class PaymentLogic
    {
        public static ApiResponseBase SetPrimaryCard(SetPrimaryCardRequest request)
        {
            if (!IsValid(request))
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERRACC01"
                };
            var payment = PaymentService.GetInstance();
            payment.UpdatePrimaryCardForCompany(request.MaskedCardNumber);
            return new ApiResponseBase
            {
                StatusCode = HttpStatusCode.OK
            };
        }

        private static bool IsValid(SetPrimaryCardRequest request)
        {
            return
                request != null &&
                request.MaskedCardNumber != null;
        }
    }
}