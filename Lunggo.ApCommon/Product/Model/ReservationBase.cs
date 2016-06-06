using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Identity.User;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Constant;

namespace Lunggo.ApCommon.Product.Model
{
    public abstract class ReservationBase<TRsv> where TRsv : ReservationBase<TRsv>
    {
        public abstract ProductType Type { get; }
        public string RsvNo { get; set; }
        public DateTime RsvTime { get; set; }
        public RsvStatus RsvStatus { get; set; }
        public CancellationType CancellationType { get; set; }
        public DateTime? CancellationTime { get; set; }
        public PaymentDetails Payment { get; set; }
        public Contact Contact { get; set; }
        public User User { get; set; }
        public ReservationState State { get; set; }
        public List<Pax> Pax { get; set; }

        //protected ReservationBase()
        //{
                
        //}

        //private static TRsv CreateInstance()
        //{
        //    var constructor = typeof(TRsv).GetConstructor(
        //        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
        //        null,
        //        System.Type.EmptyTypes,
        //        null
        //    );

        //    return (TRsv)constructor.Invoke(null);
        //}

        //public static TRsv GenerateNew()
        //{
        //    var rsv = CreateInstance();
        //    rsv.RsvNo = RsvNoSequence.GetInstance().GetNext(rsv.Type);
        //    rsv.RsvTime = DateTime.UtcNow;
        //    rsv.RsvStatus = RsvStatus.Pending;
        //    rsv.CancellationType = CancellationType.NotCancelled;
        //    rsv.CancellationTime = null;
        //    rsv.Payment = new PaymentData
        //    {
        //        Status = PaymentStatus.Pending,
        //        LocalCurrency = new Currency(OnlineContext.GetActiveCurrencyCode()),
        //        OriginalPriceIdr = rsv.Orders.Sum(order => order.Price.FinalIdr),
        //        TimeLimit = rsv.Orders.Min(order => order.TimeLimit)
        //    };
        //    rsv.User = HttpContext.Current.User.Identity.GetUser();
        //    return rsv;
        //}
    }
}
