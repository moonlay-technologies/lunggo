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
                stepName = referralHistoryModel.History,
                ReferralCredit = referralHistoryModel.ReferralCredit,
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
                    stepName = refer.History,
                    ReferralCredit = refer.ReferralCredit,
                    DateTime = refer.TimeStamp
                };
                displays.Add(display);
            }
            return displays;
        }

        public List<ReferralDetail> ConvertFromReferralHistoryToReferralDetail(List<ReferralHistoryModel> referralHistory)
        {
            var displays = new List<ReferralDetail>();
            var referreeIds = referralHistory.Select(refer => refer.ReferreeId).Distinct().ToList();
            var steps = new List<ReferralHistoryModelForDisplay>();
            steps.Add(new ReferralHistoryModelForDisplay
            {
                stepName = "First Time Login",
                StepDetail = "Login For The First Time",
                DateTime = null,
                ReferralCredit = 100000,
                StepStatus = false
            });
            steps.Add(new ReferralHistoryModelForDisplay
            {
                stepName = "First Time Booking",
                StepDetail = "Booking For The First Time",
                DateTime = null,
                ReferralCredit = 100000,
                StepStatus = false
            });
            foreach (var referreeId in referreeIds)
            {
                var user = ActivityService.GetInstance().GetUserByIdFromDb(referreeId);
                var username = ActivityService.GetInstance().ConvertToPaxForDisplay(new Pax { FirstName = user.FirstName, LastName = user.LastName }).Name;
                var historyModel = referralHistory.Where(refer => refer.ReferreeId == referreeId).ToList();
                foreach (var step in steps)
                {
                    var history = historyModel.Where(his => his.History == step.stepName).ToList();
                    if (history.Count > 0)
                    {
                        step.StepStatus = true;
                        step.DateTime = history[0].TimeStamp;
                    }
                }
                var display = new ReferralDetail
                {
                    Name = username,
                    History = steps,
                };
                displays.Add(display);
            }
            return displays;
        }
    }
}
