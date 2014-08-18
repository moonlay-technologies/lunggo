using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Payment.Data
{
    public class CreditCardPaymentData : PaymentData
    {
        public CreditCard CreditCard { get; set; }
        public CreditCardPaymentData()
        {
            CreditCard = new CreditCard();
        }
        public override PaymentDataDummy ConvertToDummyObject()
        {
            try
            {
                CreditCardPaymentDataDummy dummy = new CreditCardPaymentDataDummy();

                dummy.payment_type = this.PaymentType;

                dummy.customer_details.billing_address.address = this.CustomerDetails.BillingAddress.Address;
                dummy.customer_details.billing_address.city = this.CustomerDetails.BillingAddress.City;
                dummy.customer_details.billing_address.country_code = this.CustomerDetails.BillingAddress.CountryCode;
                dummy.customer_details.billing_address.email = this.CustomerDetails.BillingAddress.Email;
                dummy.customer_details.billing_address.first_name = this.CustomerDetails.BillingAddress.FirstName;
                dummy.customer_details.billing_address.last_name = this.CustomerDetails.BillingAddress.LastName;
                dummy.customer_details.billing_address.phone = this.CustomerDetails.BillingAddress.Phone;
                dummy.customer_details.billing_address.postal_code = this.CustomerDetails.BillingAddress.PostalCode;

                dummy.customer_details.email = this.CustomerDetails.Email;
                dummy.customer_details.first_name = this.CustomerDetails.FirstName;
                dummy.customer_details.last_name = this.CustomerDetails.LastName;
                dummy.customer_details.phone = this.CustomerDetails.Phone;

                List<ItemDetailDummy> ListItemDetailDummy = new List<ItemDetailDummy>();
                foreach (ItemDetail itemDetail in this.ItemDetails)
                {
                    ItemDetailDummy detailDummy = new ItemDetailDummy();
                    detailDummy.id = itemDetail.Id;
                    detailDummy.name = itemDetail.Name;
                    detailDummy.price = itemDetail.Price;
                    detailDummy.quantity = itemDetail.Quantity;
                    ListItemDetailDummy.Add(detailDummy);
                }
                dummy.item_details = ListItemDetailDummy;

                dummy.transaction_details.order_id = this.TransactionDetails.OrderId;
                dummy.transaction_details.gross_amount = this.TransactionDetails.GrossAmount;

                dummy.credit_card.bank = this.CreditCard.Bank;
                dummy.credit_card.token_id = this.CreditCard.TokenId;

                return dummy;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class CreditCard
    {
        public String TokenId { get; set; }
        public String Bank { get; set; }
    }
}
