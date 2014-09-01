using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Payment.Data
{
    public abstract class PaymentData
    {
        public String PaymentType { get; set; }
        public List<ItemDetail> ItemDetails { get; set; }
        public TransactionDetail TransactionDetails { get; set; }
        public CustomerDetails CustomerDetails { get; set; }

        protected PaymentData()
        {
            TransactionDetails = new TransactionDetail();
            CustomerDetails = new CustomerDetails();
            ItemDetails = new List<ItemDetail>();
        }
        public abstract PaymentDataDummy ConvertToDummyObject();
        public static PaymentDataDummy ConvertPaymentDataToPaymentDataDummy(PaymentDataDummy Dummy, PaymentData Origin)
        {
            Dummy.payment_type = Origin.PaymentType;

            Dummy.customer_details.billing_address.address = Origin.CustomerDetails.BillingAddress.Address;
            Dummy.customer_details.billing_address.city = Origin.CustomerDetails.BillingAddress.City;
            Dummy.customer_details.billing_address.country_code = Origin.CustomerDetails.BillingAddress.CountryCode;
            Dummy.customer_details.billing_address.email = Origin.CustomerDetails.BillingAddress.Email;
            Dummy.customer_details.billing_address.first_name = Origin.CustomerDetails.BillingAddress.FirstName;
            Dummy.customer_details.billing_address.last_name = Origin.CustomerDetails.BillingAddress.LastName;
            Dummy.customer_details.billing_address.phone = Origin.CustomerDetails.BillingAddress.Phone;
            Dummy.customer_details.billing_address.postal_code = Origin.CustomerDetails.BillingAddress.PostalCode;

            Dummy.customer_details.email = Origin.CustomerDetails.Email;
            Dummy.customer_details.first_name = Origin.CustomerDetails.FirstName;
            Dummy.customer_details.last_name = Origin.CustomerDetails.LastName;
            Dummy.customer_details.phone = Origin.CustomerDetails.Phone;

            List<ItemDetailDummy> ListItemDetailDummy = new List<ItemDetailDummy>();
            foreach (ItemDetail itemDetail in Origin.ItemDetails)
            {
                ItemDetailDummy detailDummy = new ItemDetailDummy();
                detailDummy.id = itemDetail.Id;
                detailDummy.name = itemDetail.Name;
                detailDummy.price = itemDetail.Price;
                detailDummy.quantity = itemDetail.Quantity;
                ListItemDetailDummy.Add(detailDummy);
            }
            Dummy.item_details = ListItemDetailDummy;

            Dummy.transaction_details.order_id = Origin.TransactionDetails.OrderId;
            Dummy.transaction_details.gross_amount = Origin.TransactionDetails.GrossAmount;
            return Dummy;
        }
    }
    public class TransactionDetail
    {
        public String OrderId { get; set; }
        public decimal GrossAmount { get; set; }
    }
    public class ItemDetail
    {
        public String Id { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public String Name { get; set; }
    }
    public class CustomerDetails
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }
        public String Phone { get; set; }
        public BillingAddress BillingAddress { get; set; }
        public CustomerDetails()
        {
            BillingAddress = new BillingAddress();
        }
    }
    public class BillingAddress
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }
        public String Address { get; set; }
        public String City { get; set; }
        public String PostalCode { get; set; }
        public String Phone { get; set; }
        public String CountryCode { get; set; }
    }
}
