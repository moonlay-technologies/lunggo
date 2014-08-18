using Lunggo.CustomerWeb.Areas.UW400.Models;
using Lunggo.Framework.Payment;
using Lunggo.Framework.Payment.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Web;
using System.Web.Mvc;

namespace Lunggo.CustomerWeb.Areas.UW400.Logic
{
    public class UW400BookingLogic
    {
        public ActionResult Book(UW400BookViewModel vm)
	    {
		    dynamic paymentProcessor;
		    if(vm.payment_type == "cimbclicks")
		    {
			    paymentProcessor = new CIMBProcessor();
		    }
            else if(vm.payment_type == "creditcard")
            {
                paymentProcessor = new CIMBProcessor();
            }
            else if (vm.payment_type == "clickpay")
            {
                paymentProcessor = new CIMBProcessor();
            }
            else
            {
                throw new Exception("No payment type matched");
            }
		    var paymentData = CreatePaymentData(vm);
            PaymentResult paymentResult = paymentProcessor.PaymentResult(paymentData);
		
		    //proses booking

            if (paymentResult.Result=="Sukses")
            {

            }

            if (String.IsNullOrEmpty( paymentResult.RedirectUrl))
            {
                var result = new ViewResult()
                {
                    ViewName = "../UW100Search/UW100Index"
                };
                result.ViewData.Model = paymentResult;
                return result;
		    }
		    else
		    {
                return new RedirectResult(paymentResult.RedirectUrl);
		    }
	    }

        private PaymentData CreatePaymentData(UW400BookViewModel vm)
        {
            PaymentData data;
            if (vm.payment_type == "cimbclicks")
            {
                data = CreateCIMBData(vm);
            }
            else if (vm.payment_type == "creditcard")
            {
                data = new CIMBPaymentData();
            }
            else if (vm.payment_type == "clickpay")
            {
                data = new CIMBPaymentData();
            }
            else
            {
                throw new Exception("No payment type matched");
            }
            return data;
        }

        CIMBPaymentData CreateCIMBData(UW400BookViewModel vm)
        {

            CIMBPaymentData data = new CIMBPaymentData();
            data.PaymentType = "cimb";


            data.CustomerDetails.BillingAddress.Address = "jalan";
            data.CustomerDetails.BillingAddress.City = "jakarta";
            data.CustomerDetails.BillingAddress.CountryCode = "jakarta";
            data.CustomerDetails.BillingAddress.Email = "jakarta";
            data.CustomerDetails.BillingAddress.FirstName = "jakarta";
            data.CustomerDetails.BillingAddress.LastName = "jakarta";
            data.CustomerDetails.BillingAddress.Phone = "jakarta";
            data.CustomerDetails.BillingAddress.PostalCode = "jakarta";

            data.CustomerDetails.Email = "jakarta";
            data.CustomerDetails.FirstName = "jakarta";
            data.CustomerDetails.LastName = "jakarta";
            data.CustomerDetails.Phone = "jakarta";

            List<ItemDetail> ListItemDetailDummy = new List<ItemDetail>();


            ItemDetail detailDummy = new ItemDetail();
            detailDummy.Id = "123213";
            detailDummy.Name = "asdsad";
            detailDummy.Price = 123;
            detailDummy.Quantity = 123;

            ItemDetail detailDummy2 = new ItemDetail();
            detailDummy2.Id = "1232123";
            detailDummy2.Name = "asdsad";
            detailDummy2.Price = 123;
            detailDummy2.Quantity = 123;

            ListItemDetailDummy.Add(detailDummy);
            ListItemDetailDummy.Add(detailDummy2);
            data.ItemDetails = ListItemDetailDummy;

            data.TransactionDetails.OrderId = "jakarta";
            data.TransactionDetails.GrossAmount = 2;

            data.CIMBClicks.Description = "jakarta";
            return data;
        }
    }
}