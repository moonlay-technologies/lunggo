using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.Framework.Database;
using Lunggo.Framework.Queue;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {
        internal override void Issue(string rsvNo)
        {
            IssueHotel(new IssueHotelTicketInput { RsvNo = rsvNo });
        }

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
            
            var output = new IssueHotelTicketOutput();
            if (rsvData.Payment.Method == PaymentMethod.Credit ||
               (rsvData.Payment.Method != PaymentMethod.Credit &&
                rsvData.Payment.Status == PaymentStatus.Settled))
            {
                var queueService = QueueService.GetInstance();
                var queue = queueService.GetQueueByReference("HotelIssueTicket");
                queue.AddMessage(new CloudQueueMessage(input.RsvNo));
                output.IsSuccess = true;
                return output;
            }
            else
            {
                output.IsSuccess = false;
                //output.Errors = new List<FlightError> { FlightError.NotEligibleToIssue };
                return output;
            }
        }

        public IssueHotelTicketOutput CommenceIssueHotel(IssueHotelTicketInput input)
        {
            var rsvData = GetReservationFromDb(input.RsvNo);
            if (rsvData == null)
            {
                return new IssueHotelTicketOutput
                {
                    IsSuccess = false,
                };
            }
            var newRooms = new List<HotelRoom>();
            foreach (var room in rsvData.HotelDetails.Rooms)
            {
                foreach (var rate in room.Rates)
                {
                    if (rate.TimeLimit >= DateTime.UtcNow) continue;
                    var sampleRatekey = rate.RateKey.Split('|');
                    var checkInDate = new DateTime(Convert.ToInt32(sampleRatekey[0].Substring(0, 4)), Convert.ToInt32(sampleRatekey[0].Substring(4, 2)),
                        Convert.ToInt32(sampleRatekey[0].Substring(6, 2)));
                    var checkOutDate = new DateTime(Convert.ToInt32(sampleRatekey[1].Substring(0, 4)), Convert.ToInt32(sampleRatekey[1].Substring(4, 2)),
                        Convert.ToInt32(sampleRatekey[1].Substring(6, 2)));
                    var hotelCd = Convert.ToInt32(sampleRatekey[4]);
                    var roomCd = sampleRatekey[5];
                    var someData = sampleRatekey[6];
                    var board = sampleRatekey[7];
                    var roomCount = Convert.ToInt32(sampleRatekey[9].Split('~')[0]);
                    var adultCount = Convert.ToInt32(sampleRatekey[9].Split('~')[1]);
                    var childCount = Convert.ToInt32(sampleRatekey[9].Split('~')[2]);

                    var searchResult = Search(new SearchHotelInput
                    {
                        AdultCount = adultCount,
                        CheckIn = checkInDate,
                        Checkout = checkOutDate,
                        ChildCount = childCount,
                        Rooms = roomCount,
                        HotelCode = hotelCd
                    });

                    foreach (var ratea in searchResult.HotelDetailLists.SelectMany(hotel => hotel.Rooms.SelectMany(rooma => (from ratea in rooma.Rates
                        let rateKey = rate.RateKey.Split('|')
                        where rateKey[5] == roomCd && rateKey[6] == someData && rateKey[7] == board
                        select ratea))))
                    {
                        rate.Price.SetSupplier(ratea.Price.OriginalIdr, new Currency("IDR"));
                    }
                }
            }
            

            var issueInfo = new HotelIssueInfo
            {
                RsvNo = rsvData.RsvNo,
                Pax = rsvData.Pax,
                Contact = rsvData.Contact,
                Rooms = newRooms,
                SpecialRequest = rsvData.HotelDetails.SpecialRequest
            };

            var issue = new HotelBedsIssue();
            var issueResult = issue.IssueHotel(issueInfo);
            UpdateRsvStatusDb(rsvData.RsvNo, issueResult.IsSuccess ? RsvStatus.Completed : RsvStatus.Failed);
            if (issueResult.IsSuccess == false)
            {
                SendIssueFailedNotifToDeveloper(rsvData.RsvNo);
                SendEticketSlightDelayNotifToCustomer(rsvData.RsvNo);
                return new IssueHotelTicketOutput
                {
                    IsSuccess = false,
                    ErrorMessages = new List<string>{issueResult.Status}
                };
            }
            
            {
                SendEticketToCustomer(rsvData.RsvNo);
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
