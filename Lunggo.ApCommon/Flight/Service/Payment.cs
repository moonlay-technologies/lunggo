using Lunggo.ApCommon.Campaign.Model;
using Lunggo.ApCommon.Campaign.Service;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Payment;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public PayFlightOutput SetPayment(PayFlightInput input) 
        {
            var output = new PayFlightOutput();
            var getReservation = GetReservation(input.RsvNo);
            var updatedReservation = SetPaymentInfo(getReservation,input);
            //Update Database
            var isUpdated = UpdateDb.SetPayment(input.RsvNo, updatedReservation);
            if (isUpdated)
            {
                //Setelah di update database nya
                if (updatedReservation.Payment.Method == PaymentMethod.BankTransfer)
                    SendPendingPaymentReservationNotifToCustomer(updatedReservation.RsvNo);
                if (updatedReservation.Payment.Method == PaymentMethod.VirtualAccount)
                    SendInstantPaymentReservationNotifToCustomer(updatedReservation.RsvNo);
                SavePaymentRedirectionUrlInCache(updatedReservation.RsvNo, updatedReservation.Payment.Url, updatedReservation.Payment.TimeLimit);
                output.RsvNo = updatedReservation.RsvNo;
                output.IsSuccess = true;
                return output;
            }
            else 
            {
                return new PayFlightOutput
                {
                    IsSuccess = false,
                    RsvNo = input.RsvNo
                };
            }
            //Harus nya pas udh POST Pembayaran baru itin itu dihapus
            //Delete Itinerary From Cache
            //DeleteItineraryFromCache(input.ItinCacheId);
            //DeleteItinerarySetFromCache(input.ItinCacheId);
        }

        private FlightReservation SetPaymentInfo(FlightReservation reservation, PayFlightInput input) 
        {
            reservation.Payment.Medium = PaymentService.GetInstance().GetPaymentMedium(input.Payment.Method);
            reservation.Payment.Method = input.Payment.Method;
            reservation.Payment.Currency = input.Payment.Currency;
            var originalPrice = reservation.Itineraries.Sum(itin => itin.LocalPrice);
            var campaign = CampaignService.GetInstance().UseVoucherRequest(new VoucherRequest
            {
                Email = reservation.Contact.Email,
                Price = originalPrice,
                VoucherCode = input.Payment.DiscountCode
            });
            if (campaign.CampaignVoucher != null)
            {
                reservation.Payment.FinalPrice = campaign.DiscountedPrice;
                reservation.Discount = new DiscountData
                {
                    Code = input.Payment.DiscountCode,
                    Id = campaign.CampaignVoucher.CampaignId.GetValueOrDefault(),
                    Name = campaign.CampaignVoucher.DisplayName,
                    Percentage = campaign.CampaignVoucher.ValuePercentage.GetValueOrDefault(),
                    Constant = campaign.CampaignVoucher.ValueConstant.GetValueOrDefault(),
                    Nominal = campaign.TotalDiscount
                };
            }
            else
            {
                reservation.Payment.FinalPrice = originalPrice;
                reservation.Discount = new DiscountData();
            }
            if (reservation.Payment.Method == PaymentMethod.BankTransfer)
            {
                reservation.TransferCode = FlightService.GetInstance().GetTransferCodeByTokeninCache(input.Payment.TransferToken);
                reservation.Payment.FinalPrice -= reservation.TransferCode;
            }
            else
            {
                //Penambahan disini buat menghapus Transfer Code dan Token Transfer Code jika tidak milih Bank Transfer
                var dummyTransferCode = FlightService.GetInstance().GetTransferCodeByTokeninCache(input.Payment.TransferToken);
                var dummyPrice = reservation.Payment.FinalPrice - dummyTransferCode;
                FlightService.GetInstance().DeleteUniquePriceFromCache(dummyPrice.ToString());
                FlightService.GetInstance().DeleteTokenTransferCodeFromCache(input.Payment.TransferToken);
            }
            var transactionDetails = ConstructTransactionDetails(reservation); //Data buat Veritrans
            var itemDetails = ConstructItemDetails(reservation); //Data buat Veritrans
            var payment = PaymentService.GetInstance();
            payment.ProcessPayment(reservation.Payment, transactionDetails, itemDetails, reservation.Payment.Method);
            return reservation;
        }

        private List<ItemDetails> ConstructItemDetails(FlightReservation reservation)
        {
            var itemDetails = new List<ItemDetails>();
            var trips = reservation.Itineraries.SelectMany(itin => itin.Trips).ToList();
            var itemNameBuilder = new StringBuilder();
            foreach (var trip in trips)
            {
                itemNameBuilder.Append(trip.OriginAirport + "-" + trip.DestinationAirport);
                itemNameBuilder.Append(" " + trip.DepartureDate.ToString("dd-MM-yyyy"));
                if (trip != trips.Last())
                {
                    itemNameBuilder.Append(", ");
                }
            }
            var itemName = itemNameBuilder.ToString();
            itemDetails.Add(new ItemDetails
            {
                Id = "1",
                Name = itemName,
                Price = (long)reservation.Itineraries.Sum(itin => itin.LocalPrice),
                Quantity = 1
            });
            if (reservation.Discount.Nominal != 0)
                itemDetails.Add(new ItemDetails
                {
                    Id = "2",
                    Name = "Discount",
                    Price = (long)-reservation.Discount.Nominal,
                    Quantity = 1
                });
            return itemDetails;
        }

        private TransactionDetails ConstructTransactionDetails(FlightReservation reservation)
        {
            return new TransactionDetails
            {
                OrderId = reservation.RsvNo,
                OrderTime = reservation.RsvTime,
                Amount = (long)reservation.Payment.FinalPrice
            };
        }
        
        public void UpdateFlightPayment(string rsvNo, PaymentInfo payment)
        {
            var isUpdated = UpdateDb.Payment(rsvNo, payment);
            if (isUpdated && payment.Status == PaymentStatus.Settled)
            {
                var issueInput = new IssueTicketInput { RsvNo = rsvNo };
                IssueTicket(issueInput);
            }
        }

    }
}

