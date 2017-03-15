using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
using Lunggo.Framework.Queue;
using Microsoft.WindowsAzure.Storage.Queue;

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
            return GetCreditCardByCompanyId(companyId);
        }

        public bool? CheckBookingDisabilityStatus(string userId)
        {
            var companyId = User.GetCompanyIdByUserId(userId);
            if (string.IsNullOrEmpty(companyId))
            {
                return null;
            }
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var disabilityStatus= GetBookingDisabilityStatusQuery.GetInstance()
                    .Execute(conn, new { CompanyId = companyId }).ToList();
                return disabilityStatus[0];
            }
        }

        public void SetBookingDisabilityStatus(string userId, bool status)
        {
            var companyId = User.GetCompanyIdByUserId(userId);
            if (string.IsNullOrEmpty(companyId)) return;
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                SetBookingDisabilityStatusQuery.GetInstance().Execute(conn, new { CompanyId = companyId, Status = status });
            }

            SendNotificationToCustomer(companyId, status);
        }

        public void SendNotificationToCustomer(string companyId, bool status)
        {
            var approverEmailList = User.GetListApproverEmailByCompanyId(companyId);
            var bookerEmailList = User.GetListBookerEmailByCompanyId(companyId);

            var emailList = approverEmailList.Aggregate("", (current, email) => current + (email + "|"));

            if (bookerEmailList.Count > 0)
            {
                emailList = bookerEmailList.Aggregate(emailList, (current, email) => current + (email + "|"));
            }
           
            emailList = emailList.Substring(0, emailList.Length - 1);
            var stringStatus = status ? "true" : "false";
            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference("BookingDisabilityNotifEmail");
            queue.AddMessage(new CloudQueueMessage(emailList + "&" + stringStatus));
        }

        public List<SavedCreditCard> GetCreditCardByCompanyId(string companyId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var savedCard = GetSavedCreditCardQuery.GetInstance()
                    .Execute(conn, new { CompanyId = companyId }).ToList();
                var sortedCard = from d in savedCard
                    orderby d.IsPrimaryCard descending 
                    select d;
                return sortedCard.ToList();
            }
        }

        public bool InsertCreditCard(SavedCreditCard model)
        {
            if (model != null)
            {
                var userId = HttpContext.Current.User.Identity.GetUser().Id;
                var companyId = User.GetCompanyIdByUserId(userId);
                model.CompanyId = companyId;
                var ccList = GetCreditCardByCompanyId(companyId);
                var transactionDetails = ConstructFirstTransactionDetails(model.CompanyId);
                var paymentDetails = ConstructPaymentDetail(model.CardHolderName, model.Token);
                var paymentResponse = VeritransWrapper.ProcessFirstB2BPayment(paymentDetails, transactionDetails, PaymentMethod.CreditCard);
                if (paymentResponse != null)
                {
                    model.IsPrimaryCard = (ccList == null || ccList.Count == 0);
                    model.MaskedCardNumber = GenerateMaskedCardNumber(paymentResponse.MaskedCard);
                    model.TokenExpiry = paymentResponse.TokenIdExpiry;
                    model.Token = paymentResponse.SavedTokenId;
                    model.CardExpiry = new DateTime(model.CardExpiryYear, model.CardExpiryMonth, 1);

                    var cancelResponse = VeritransWrapper.CancelTransaction(paymentResponse.OrderId,
                        paymentResponse.TransactionId);
                    InsertSavedCreditCardToDb(model);
                    return true;
                }
            }
            return false;
        }

        public bool EditCreditCard(SavedCreditCard model)
        {
            if (model != null)
            {
                var userId = HttpContext.Current.User.Identity.GetUser().Id;
                var companyId = User.GetCompanyIdByUserId(userId);
                model.CompanyId = companyId;
                var ccList = GetCreditCardByCompanyId(companyId);
                var transactionDetails = ConstructFirstTransactionDetails(model.CompanyId);
                var paymentDetails = ConstructPaymentDetail(model.CardHolderName, model.Token);
                var paymentResponse = VeritransWrapper.ProcessFirstB2BPayment(paymentDetails, transactionDetails, PaymentMethod.CreditCard);
                if (paymentResponse != null)
                {
                    model.IsPrimaryCard = ccList == null;
                    model.MaskedCardNumber = GenerateMaskedCardNumber(paymentResponse.MaskedCard);
                    model.TokenExpiry = paymentResponse.TokenIdExpiry;
                    model.Token = paymentResponse.SavedTokenId;
                    model.CardExpiry = new DateTime(model.CardExpiryYear, model.CardExpiryMonth, 1);

                    var cancelResponse = VeritransWrapper.CancelTransaction(paymentResponse.OrderId,
                        paymentResponse.TransactionId);
                    InsertSavedCreditCardToDb(model);
                    return true;
                }
            }
            return false;
        }

        public TransactionDetails ConstructFirstTransactionDetails(string companyId)
        {
            return new TransactionDetails
            {
                Amount = 10000,
                OrderId = GenerateOrderId(companyId),
                OrderTime = DateTime.Now
            };
        }

        public PaymentDetails ConstructPaymentDetail(string cardHolderName, string tokenId)
        {
            return new PaymentDetails
            {
                Method = PaymentMethod.CreditCard,
                Data = new PaymentData
                {
                    CreditCard = new CreditCard
                    {
                        HolderName = cardHolderName,
                        TokenIdSaveEnabled = true,
                        TokenId = tokenId
                    }
                }
            };
        }

        public bool ProcessB2BPayment(string rsvNo)
        {
            var userId = HttpContext.Current.User.Identity.GetUser().Id;
            var companyId = User.GetCompanyIdByUserId(userId);
            var primaryCreditCard = new SavedCreditCard();
            int i = 0;
            var otherCreditCard = GetCreditCardByCompanyId(companyId).Where(x=>x.IsPrimaryCard == false).ToList();
            bool isUpdated;
            bool isSucces = false;
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                primaryCreditCard = GetPrimarySavedCreditCardQuery.GetInstance()
                    .Execute(conn, new { CompanyId = companyId, IsPrimaryCard = true }).SingleOrDefault();
            }
            if (primaryCreditCard != null && primaryCreditCard.Token != null)
            {
                var paymentDetails = ConstructPaymentDetail(primaryCreditCard.CardHolderName, primaryCreditCard.Token);
                var response = SubmitPayment(rsvNo, PaymentMethod.CreditCard, PaymentSubMethod.Mandiri, paymentDetails.Data, null, out isUpdated);
                if (isUpdated)
                {
                    return true;
                }
            }
            do
            {
                if (otherCreditCard.ElementAt(i) != null && otherCreditCard.ElementAt(i).Token != null)
                {
                    var paymentDetails = ConstructPaymentDetail(otherCreditCard.ElementAt(i).CardHolderName, otherCreditCard.ElementAt(i).Token);
                    var response = SubmitPayment(rsvNo, PaymentMethod.CreditCard, PaymentSubMethod.Mandiri, paymentDetails.Data, null, out isUpdated);
                    if (isUpdated)
                    {
                        isSucces = true;
                    }
                    i++;
                }
            } while (!isSucces && i == otherCreditCard.Count());
            if (isSucces) return true;
            return false;
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
                var primaryCardNumber = GetPrimarySavedCreditCardQuery.GetInstance()
                    .Execute(conn, new { CompanyId = companyId, IsPrimaryCard = true}).SingleOrDefault();
                if (primaryCardNumber != null && primaryCardNumber.MaskedCardNumber != null)
                {
                    UpdatePrimaryCardDb(primaryCardNumber.MaskedCardNumber, companyId, false);
                }
                UpdatePrimaryCardDb(maskedCardNumber,companyId, true);
            }
        }

        public bool DeleteSaveCreditCard(string maskedNumber)
        {
            var userId = HttpContext.Current.User.Identity.GetUser().Id;
            var companyId = User.GetCompanyIdByUserId(userId);
            if(!string.IsNullOrEmpty(companyId) && !string.IsNullOrEmpty(maskedNumber))
            DeleteSaveCreditCardFromDb(companyId,maskedNumber);
            return true;
        }

    }
}
