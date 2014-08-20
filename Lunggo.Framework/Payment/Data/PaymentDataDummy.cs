using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Payment.Data
{
    public abstract class PaymentDataDummy
    {
        public String payment_type { get; set; }
        public List<ItemDetailDummy> item_details { get; set; }
        public TransactionDetailDummy transaction_details { get; set; }
        public CustomerDetailsDummy customer_details { get; set; }
        public PaymentDataDummy()
        {
            transaction_details = new TransactionDetailDummy();
            customer_details = new CustomerDetailsDummy();
            item_details = new List<ItemDetailDummy>();
        }
    }
    public class TransactionDetailDummy
    {
        public String order_id { get; set; }
        public decimal gross_amount { get; set; }
    }
    public class ItemDetailDummy
    {
        public String id { get; set; }
        public decimal price { get; set; }
        public decimal quantity { get; set; }
        public String name { get; set; }
    }
    public class CustomerDetailsDummy
    {
        public String first_name { get; set; }
        public String last_name { get; set; }
        public String email { get; set; }
        public String phone { get; set; }
        public BillingAddressDummy billing_address { get; set; }
        public CustomerDetailsDummy()
        {
            billing_address = new BillingAddressDummy();
        }
    }
    public class BillingAddressDummy
    {
        public String first_name { get; set; }
        public String last_name { get; set; }
        public String email { get; set; }
        public String address { get; set; }
        public String city { get; set; }
        public String postal_code { get; set; }
        public String phone { get; set; }
        public String country_code { get; set; }
    }
}
