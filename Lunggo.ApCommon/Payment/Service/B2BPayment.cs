using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Query;
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
                if (string.IsNullOrEmpty(model.CompanyId))
                {
                    var userId = HttpContext.Current.User.Identity.GetUser().Id;
                    var companyId = User.GetCompanyIdByUserId(userId);
                    model.CompanyId = companyId;
                }
                model.IsPrimaryCard = model.IsPrimaryCard ?? false;
                model.TokenExpiry = DateTime.Now.AddYears(1);
                model.Token = "32khg452hdsajhdkasdg";
                InsertSavedCreditCardToDb(model);
            }
                
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
