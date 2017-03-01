using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.Framework.Http;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Payment.Model;

namespace Lunggo.WebAPI.ApiSrc.Payment.Logic
{
    public static partial class PaymentLogic
    {
        public static ApiResponseBase GetSavedCreditCard(string userId)
        {
            var creditCardList = PaymentService.GetInstance().GetSaveCreditCards(userId);
            if (creditCardList != null)
            {
                var response = new List<CreditCard>(); 
                foreach (var savedCreditCard in creditCardList)
                {
                    var singleCc = new CreditCard
                    {
                        MaskedCardNumber = savedCreditCard.MaskedCardNumber,
                        IsPrimaryCard = savedCreditCard.IsPrimaryCard,
                        CardHolderName = savedCreditCard.CardHolderName,
                        CardExpiry = savedCreditCard.CardExpiry
                    };
                    response.Add(singleCc);
                }
                return new GetSavedCreditCardResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    SavedCreditCard = response
                };
            }
            return new GetSavedCreditCardResponse
            {
                StatusCode = HttpStatusCode.Accepted
            };
        }
    }
}