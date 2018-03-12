using Lunggo.ApCommon.Account.Model;
using Lunggo.ApCommon.Activity.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Product.Model;

namespace Lunggo.ApCommon.Account.Service
{
    public partial class AccountService
    {
        public ReferralHistoryModelForDisplay ConvertToReferralHistoryModelDisplay(ReferralHistoryModel referralHistoryModel)
        {
            var user = ActivityService.GetInstance().GetUserByIdFromDb(referralHistoryModel.ReferreeId);
            var username = ActivityService.GetInstance().ConvertToPaxForDisplay(new Pax { FirstName = user.FirstName, LastName = user.LastName }).Name;
            return new ReferralHistoryModelForDisplay
            {
                History = referralHistoryModel.History,
                ReferralCredit = referralHistoryModel.ReferralCredit,
                User = username,
                DateTime = referralHistoryModel.TimeStamp
            };
        }

        public List<ReferralHistoryModelForDisplay> ConvertToReferralHistoryModelDisplay(List<ReferralHistoryModel> referralHistoryModel)
        {
            var displays = new List<ReferralHistoryModelForDisplay>();
            foreach (var refer in referralHistoryModel)
            {
                var user = ActivityService.GetInstance().GetUserByIdFromDb(refer.ReferreeId);
                var username = ActivityService.GetInstance().ConvertToPaxForDisplay(new Pax { FirstName = user.FirstName, LastName = user.LastName }).Name;
                var display = new ReferralHistoryModelForDisplay
                {
                    History = refer.History,
                    ReferralCredit = refer.ReferralCredit,
                    User = username,
                    DateTime = refer.TimeStamp
                };
                displays.Add(display);
            }
            return displays;
        }
    }
}
