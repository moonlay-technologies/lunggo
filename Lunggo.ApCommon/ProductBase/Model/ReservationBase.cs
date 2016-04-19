using System;
using System.Reflection;
using Lunggo.ApCommon.Identity.User;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.ProductBase.Constant;
using Lunggo.ApCommon.Sequence;

namespace Lunggo.ApCommon.ProductBase.Model
{
    public abstract partial class ReservationBase<T> where T : ReservationBase<T>, new()
    {
        protected abstract ProductType Type { get; }
        public string RsvNo { get; private set; }
        public DateTime RsvTime { get; private set; }
        public PaymentData Payment { get; private set; }
        public Contact Contact { get; private set; }
        public User User { get; private set; }

        public static T GenerateNew()
        {
            var rsv = new T();
            rsv.RsvNo = RsvNoSequence.GetInstance().GetNext(rsv.Type);
            rsv.RsvTime = DateTime.UtcNow;
            return rsv;
        }   

        public static T GetFromDb(string rsvNo)
        {
            var rsv = new T();
            var found = rsv.TryGetSpecificReservationDataFromDb(rsvNo);
            if (!found)
                return null;
            rsv.GetPaymentFromDb();
            rsv.GetContactFromDb();
            rsv.GetUserFromDb();
            return rsv;
        }

        protected abstract bool TryGetSpecificReservationDataFromDb(string rsvNo);

        private void GetPaymentFromDb()
        {
            throw new NotImplementedException();
        }

        private void GetContactFromDb()
        {
            throw new NotImplementedException();
        }

        private void GetUserFromDb()
        {
            throw new NotImplementedException();
        }
    }
}
