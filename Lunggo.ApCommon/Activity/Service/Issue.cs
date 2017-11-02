using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.Framework.Queue;
using Microsoft.WindowsAzure.Storage.Queue;
using System;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        internal override void Issue(string rsvNo)
        {
            IssueActivity(new IssueActivityTicketInput() { RsvNo = rsvNo });
        }

        public IssueActivityTicketOutput IssueActivity(IssueActivityTicketInput input)
        {
            var rsvData = GetReservationFromDb(input.RsvNo);
            if (rsvData == null)
            {
                return new IssueActivityTicketOutput
                {
                    IsSuccess = false
                };
            }

            var output = new IssueActivityTicketOutput();
            if (rsvData.Payment.Method == PaymentMethod.Credit ||
                (rsvData.Payment.Method != PaymentMethod.Credit &&
                 rsvData.Payment.Status == PaymentStatus.Settled))
            {
                UpdateRsvStatusDb(rsvData.RsvNo, RsvStatus.Completed );
                SendNotificationToAdmin(rsvData.RsvNo);
                output.IsSuccess = true;
                return output;
            }
            output.IsSuccess = false;
            return output;
        }
    }
}
