using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Payment.Data
{
    public class MandiriClickPayPaymentData : PaymentData
    {
        public MandiriClickpay MandiriClickpay { get; set; }
        public MandiriClickPayPaymentData()
        {
            this.MandiriClickpay = new MandiriClickpay();
        }
        public override PaymentDataDummy ConvertToDummyObject()
        {
            try
            {
                MandiriClickPayPaymentDataDummy dummy = new MandiriClickPayPaymentDataDummy();

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

                dummy.mandiri_clickpay.card_number = this.MandiriClickpay.CardNumber;
                dummy.mandiri_clickpay.input1 = this.MandiriClickpay.Input1;
                dummy.mandiri_clickpay.input2 = this.MandiriClickpay.Input2;
                dummy.mandiri_clickpay.input3 = this.MandiriClickpay.Input3;
                dummy.mandiri_clickpay.token = this.MandiriClickpay.Token;
                return dummy;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class MandiriClickpay
    {
        public string CardNumber { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
        public string Token { get; set; }
    }
}
