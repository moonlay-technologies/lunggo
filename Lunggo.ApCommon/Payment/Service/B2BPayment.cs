using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Model.Data;
using Lunggo.ApCommon.Payment.Query;
using Lunggo.Framework.Config;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Payment.Service
{
    public partial class PaymentService
    {
        public List<SavedCreditCard> GetSaveCreditCards(string userId)
        {
            var companyId = User.GetCompanyIdByUserId(userId);
            if (string.IsNullOrEmpty(companyId))
            {
                return null;
            }
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var savedCard = GetSavedCreditCardQuery.GetInstance()
                    .Execute(conn, new { CompanyId = companyId }).ToList();
                return savedCard;
            }
        }

        public void InsertCreditCard(SavedCreditCard model)
        {
            if (model != null)
            {
                var userId = HttpContext.Current.User.Identity.GetUser().Id;
                var companyId = User.GetCompanyIdByUserId(userId);
                model.CompanyId = companyId;
                var transactionDetails = ConstructFirstTransactionDetails(model.CompanyId);
                var paymentDetails = ConstructFirstPaymentDetail(model);
                var paymentResponse = VeritransWrapper.ProcessFirstB2BPayment(paymentDetails, transactionDetails, PaymentMethod.CreditCard);
                //Cancel Payment dong

                model.IsPrimaryCard = model.IsPrimaryCard ?? false;
                model.MaskedCardNumber = GenerateMaskedCardNumber(paymentResponse.MaskedCard);
                model.TokenExpiry = paymentResponse.TokenIdExpiry;
                model.Token = paymentResponse.SavedTokenId;
                model.CardExpiry = new DateTime(model.CardExpiryYear,model.CardExpiryMonth,1);
                //InsertSavedCreditCardToDb(model);
            }
        }

        public TransactionDetails ConstructFirstTransactionDetails(string companyId)
        {
            return new TransactionDetails
            {
                Amount = 15000,
                OrderId = GenerateOrderId(companyId),
                OrderTime = DateTime.Now
            };
        }

        public PaymentDetails ConstructFirstPaymentDetail(SavedCreditCard model)
        {
            return new PaymentDetails
            {
                Method = PaymentMethod.CreditCard,
                Data = new PaymentData
                {
                    CreditCard = new CreditCard
                    {
                        HolderName = model.CardHolderName,
                        TokenIdSaveEnabled = true,
                        TokenId = model.Token
                    }
                }
            };
        }

        public string GenerateOrderId(string companyId)
        {
            Guid id = Guid.NewGuid();
            var idSuffix = id.ToString().Replace("-", "");
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            var envPrefix = env != "production" ? "TEST" : "";
            idSuffix = idSuffix.Substring(idSuffix.Length - 5);
            return envPrefix + companyId + idSuffix;
        }

        public string GenerateMaskedCardNumber(string cardNo)
        {
            const string maskedNo = "***********";
            return cardNo[0] + maskedNo + cardNo.Substring(cardNo.Length-4);
        }

        public void UpdatePrimaryCardForCompany(string maskedCardNumber)
        {
            var userId = HttpContext.Current.User.Identity.GetUser().Id;
            var companyId = User.GetCompanyIdByUserId(userId);
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                //Take recent PrimaryCard
                var primaryMaskCardNumber = GetPrimarySavedCreditCardQuery.GetInstance()
                    .Execute(conn, new { CompanyId = companyId, IsPrimaryCard = true}).SingleOrDefault();
                if (!string.IsNullOrEmpty(primaryMaskCardNumber))
                {
                    UpdatePrimaryCardDb(primaryMaskCardNumber, companyId, false);
                }
                UpdatePrimaryCardDb(maskedCardNumber,companyId, true);
            }
        }

        public void DeleteSaveCreditCard(string companyId, string maskedNumber)
        {
            if(!string.IsNullOrEmpty(companyId) && !string.IsNullOrEmpty(maskedNumber))
            DeleteSaveCreditCardFromDb(companyId,maskedNumber);
        }

    }
}
