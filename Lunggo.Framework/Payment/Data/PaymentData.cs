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
        public PaymentData()
        {
            TransactionDetails = new TransactionDetail();
            CustomerDetails = new CustomerDetails();
            ItemDetails = new List<ItemDetail>();
        }
        public abstract PaymentDataDummy ConvertToDummyObject();
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
