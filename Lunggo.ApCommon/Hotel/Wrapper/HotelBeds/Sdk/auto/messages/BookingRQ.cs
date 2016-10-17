using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.messages
{
    public class BookingRQ : AbstractGenericRequest
    {
        public Holder holder { get; set; }
        public List<BookingRoom> rooms { get; set; }
        public PaymentData paymentData { get; set; }
        public string clientReference { get; set; }
        public string remark { get; set; }

        public BookingRQ()
        {
            rooms = new List<BookingRoom>();
        }

        public void Validate()
        {
            if(holder == null || String.IsNullOrEmpty(holder.name) || String.IsNullOrEmpty(holder.surname))
                throw new ArgumentException("Holder object can't be null");

            if(rooms == null || rooms.Count == 0)
                throw new ArgumentException("Rooms list can't be null");

            for(int r = 0; r < rooms.Count; r++)
            {
                if(String.IsNullOrEmpty(rooms[r].rateKey))
                    throw new ArgumentException("RateKey Room can't be null");

                if(rooms[r].paxes == null || rooms[r].paxes.Count == 0)
                    throw new ArgumentException("Paxes list can't be null");

                for(int p = 0; p < rooms[r].paxes.Count; p++)
                {
                    if ( rooms[r].paxes[p].type == null )
                        throw new ArgumentException("Paxes Type can't be null");

                    if (String.IsNullOrEmpty(rooms[r].paxes[p].name))
                        throw new ArgumentException("Paxes name can't be null");

                    if (String.IsNullOrEmpty(rooms[r].paxes[p].surname))
                        throw new ArgumentException("Paxes surname can't be null");

                    if (!rooms[r].paxes[p].age.HasValue)
                        throw new ArgumentException("Paxes age can't be null");

                    if (!rooms[r].paxes[p].roomId.HasValue)
                        throw new ArgumentException("RoomId can't be null");
                }
            }

            if(String.IsNullOrEmpty(clientReference))                
                throw new ArgumentException("Client Reference can't be null");

            if(paymentData != null)
            {
                if( paymentData.paymentCard == null )
                    throw new ArgumentException("PaymentCard can't be null");

                if (String.IsNullOrEmpty(paymentData.paymentCard.cardType))
                    throw new ArgumentException("CardType can't be null");

                if (String.IsNullOrEmpty(paymentData.paymentCard.cardNumber))
                    throw new ArgumentException("CardNumber can't be null");

                if(String.IsNullOrEmpty(paymentData.paymentCard.expiryDate))
                    throw new ArgumentException("ExpiryDate can't be null");

                if(String.IsNullOrEmpty(paymentData.paymentCard.cardCVC))
                    throw new ArgumentException("CardCVC can't be null");

                if(paymentData.contactData == null)
                    throw new ArgumentException("ContactData can't be null");

                if (String.IsNullOrEmpty(paymentData.contactData.email))
                    throw new ArgumentException("Email can't be null or empty");

                if (String.IsNullOrEmpty(paymentData.contactData.phoneNumber))
                    throw new ArgumentException("PhoneNumber can't be null or empty");
            }
        }
    }
}
