using Lunggo.ApCommon.Activity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public ActivityETicketAndInvoiceDataForEmailAndPdf GetActivityInvoice(string cartId)
        {
            var cartList = GetCartListData(cartId);
            var activityReservations = new List<ActivityReservationForDisplay>();
            var rsvNoList = GetCartRsvNoListFromDb(cartId);
            foreach (var rsvNo in rsvNoList)
            {
                var activityReservationNonDisplay = GetReservation(rsvNo);
                var activityReservationForDisplay = ConvertToReservationForDisplay(activityReservationNonDisplay);
                activityReservations.Add(activityReservationForDisplay);
            }
            var invoiceData = new ActivityETicketAndInvoiceData();
            invoiceData.ActivityReservations = activityReservations;
            invoiceData.TrxList = cartList;
            return new ActivityETicketAndInvoiceDataForEmailAndPdf
            {
                CartId = cartId,
                ActivityETicketAndInvoice = invoiceData
            };          
        }
    }
    
}
