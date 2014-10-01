using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lunggo.Framework.Payment.Data
{
    public abstract class PaymentData
    {
        [JsonProperty("payment_type")]
        public String PaymentType { get; set; }
        [JsonProperty("item_details")]
        public List<ItemDetail> ItemDetails { get; set; }
        [JsonProperty("transaction_details")]
        public TransactionDetail TransactionDetails { get; set; }
        [JsonProperty("customer_details")]
        public CustomerDetails CustomerDetails { get; set; }

        protected PaymentData()
        {
            TransactionDetails = new TransactionDetail();
            CustomerDetails = new CustomerDetails();
            ItemDetails = new List<ItemDetail>();
        }
        [Obsolete("use jsonproperty attribute instead")]
        public abstract PaymentDataDummy ConvertToDummyObject();

        [Obsolete("use jsonproperty attribute instead")]
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
        [JsonProperty("order_id")]
        public String OrderId { get; set; }
        [JsonProperty("gross_amount")]
        public decimal GrossAmount { get; set; }
    }
    public class ItemDetail
    {
        [JsonProperty("id")]
        public String Id { get; set; }
        [JsonProperty("price")]
        public decimal Price { get; set; }
        [JsonProperty("quantity")]
        public decimal Quantity { get; set; }
        [JsonProperty("name")]
        public String Name { get; set; }
    }
    public class CustomerDetails
    {
        [JsonProperty("first_name")]
        public String FirstName { get; set; }
        [JsonProperty("last_name")]
        public String LastName { get; set; }
        [JsonProperty("email")]
        public String Email { get; set; }
        [JsonProperty("phone")]
        public String Phone { get; set; }
        [JsonProperty("billing_address")]
        public BillingAddress BillingAddress { get; set; }
        public CustomerDetails()
        {
            BillingAddress = new BillingAddress();
        }
    }
    public class BillingAddress
    {
        [JsonProperty("first_name")]
        public String FirstName { get; set; }
        [JsonProperty("last_name")]
        public String LastName { get; set; }
        [JsonProperty("email")]
        public String Email { get; set; }
        [JsonProperty("address")]
        public String Address { get; set; }
        [JsonProperty("city")]
        public String City { get; set; }
        [JsonProperty("postal_code")]
        public String PostalCode { get; set; }
        [JsonProperty("phone")]
        public String Phone { get; set; }
        [JsonProperty("country_code")]
        public String CountryCode { get; set; }
    }
}
