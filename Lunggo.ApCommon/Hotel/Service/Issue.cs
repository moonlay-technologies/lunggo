using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {
        public IssueHotelTicketOutput IssueHotel(IssueHotelTicketInput input)
        {
            var rsvData = GetReservationFromDb(input.RsvNo);
            if (rsvData == null)
            {
                return new IssueHotelTicketOutput
                {
                    IsSuccess = false,
                };
            }
            var issueInfo = new HotelIssueInfo
            {
                RsvNo = rsvData.RsvNo,
                Pax = rsvData.Pax,
                Contact = rsvData.Contact,
                Rooms = rsvData.HotelDetails.Rooms
            };

            var issue = new HotelBedsIssue();
            var issueResult = issue.IssueHotel(issueInfo);
            UpdateRsvStatusDb(rsvData.RsvNo, issueResult.IsSuccess ? RsvStatus.Completed : RsvStatus.Failed);
            if (issueResult.IsSuccess == false)
            {
                return new IssueHotelTicketOutput
                {
                    IsSuccess = false,
                    ErrorMessages = new List<string>{issueResult.Status}
                };
            }
            
            {
                var order = issueResult.BookingId.Select(id => new OrderResult
                {
                    BookingId = id, BookingStatus = BookingStatus.Ticketed, IsSuccess = true
                }).ToList();


                return new IssueHotelTicketOutput
                {
                    IsSuccess = true,
                    OrderResults = order
                };
            }
        }


        private void UpdateRsvStatusDb(string rsvNo, RsvStatus status)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                ReservationTableRepo.GetInstance().Update(conn, new ReservationTableRecord
                {
                    RsvNo = rsvNo,
                    RsvStatusCd = RsvStatusCd.Mnemonic(status)
                });
            }

        }

        //private void SendHotelEticket(string rsvNo)
        //{
        //    //TODO Update THIS
        //}

        //private void SendFailedHotelNotif(string rsvNo)
        //{
        //    //TODO Update THIS
        //}
    }       
}
