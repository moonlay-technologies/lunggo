using Lunggo.CustomerWeb.Areas.UW400.Models;
using Lunggo.Framework.Payment;
using Lunggo.Framework.Payment.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            if (vm.PaymentType == "cimb_clicks")
		    {
			    paymentProcessor = new CIMBProcessor();
		    }
            else if (vm.PaymentType == "credit_card")
            {
                paymentProcessor = new CreditCardProcessor();
            }
            else if (vm.PaymentType == "mandiri_clickpay")
            {
                paymentProcessor = new MandiriClickPayProcessor();
            }
            else
            {
                throw new Exception("Illegal payment type, no payment type matched");
            }

		    var paymentData = CreatePaymentData(vm);
            PaymentResult paymentResult = paymentProcessor.PaymentResult(paymentData);



            if (IsPaymentSuccess(paymentResult))
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
        private bool IsPaymentSuccess(PaymentResult paymentResult)
        {
            return paymentResult.Result == ((int)HttpStatusCode.OK).ToString() || paymentResult.Result == ((int)HttpStatusCode.Created).ToString();
        }



        private PaymentData CreatePaymentData(UW400BookViewModel vm)
        {
            PaymentData data;
            if (vm.PaymentType == "cimb_clicks")
            {
                data = CreateCIMBData(vm);
            }
            else if (vm.PaymentType == "mandiri_clickpay")
            {
                data = CreateClickPayData(vm);
            }
            else if (vm.PaymentType == "credit_card")
            {
                data = CreateCreditCardPayData(vm);
            }
            else
            {
                throw new Exception("No payment type matched");
            }
            return data;
        }

        CIMBPaymentData CreateCIMBData(UW400BookViewModel vm)
        {

            var data = new CIMBPaymentData
            {
                PaymentType = "cimb_clicks"
            };

            data.CustomerDetails.BillingAddress.Address = "jalan";
            data.CustomerDetails.BillingAddress.City = "jakarta";
            data.CustomerDetails.BillingAddress.CountryCode = "jakarta";
            data.CustomerDetails.BillingAddress.Email = "bayualvian@hotmail.com";
            data.CustomerDetails.BillingAddress.FirstName = "jakarta";
            data.CustomerDetails.BillingAddress.LastName = "jakarta";
            data.CustomerDetails.BillingAddress.Phone = "jakarta";
            data.CustomerDetails.BillingAddress.PostalCode = "jakarta";

            data.CustomerDetails.Email = "jakarta@as.com";
            data.CustomerDetails.FirstName = "jakarta";
            data.CustomerDetails.LastName = "jakarta";
            data.CustomerDetails.Phone = "jakarta";

            var listItemDetailDummy = new List<ItemDetail>();


            var detailDummy = new ItemDetail
            {
                Id = "123213", 
                Name = "asdsad", 
                Price = 100, 
                Quantity = 1
            };

            var detailDummy2 = new ItemDetail
            {
                Id = "1232123", 
                Name = "asdsad", 
                Price = 100, 
                Quantity = 1
            };

            listItemDetailDummy.Add(detailDummy);
            listItemDetailDummy.Add(detailDummy2);
            data.ItemDetails = listItemDetailDummy;

            data.TransactionDetails.OrderId = vm.OrderId;
            data.TransactionDetails.GrossAmount = 200;

            data.CIMBClicks.Description = "jakarta";
            return data;
        }
        MandiriClickPayPaymentData CreateClickPayData(UW400BookViewModel vm)
        {

            var data = new MandiriClickPayPaymentData
            {
                PaymentType = "mandiri_clickpay",
                CustomerDetails =
                {
                    Email = "jakarta@as.com",
                    FirstName = "jakarta",
                    LastName = "jakarta",
                    Phone = "jakarta",
                }
            };

            var listItemDetailDummy = new List<ItemDetail>();


            var detailDummy = new ItemDetail
            {
                Id = "123213", 
                Name = "asdsad", 
                Price = 100, 
                Quantity = 1
            };

            var detailDummy2 = new ItemDetail
            {
                Id = "1232123", 
                Name = "asdsad", 
                Price = 100, 
                Quantity = 1
            };

            listItemDetailDummy.Add(detailDummy);
            listItemDetailDummy.Add(detailDummy2);
            data.ItemDetails = listItemDetailDummy;

            data.TransactionDetails.OrderId = vm.OrderId;
            data.TransactionDetails.GrossAmount = 200;

            data.MandiriClickpay.CardNumber = "4111111111111111";
            data.MandiriClickpay.Input1 = "1111111111";
            data.MandiriClickpay.Input2 = "200";
            data.MandiriClickpay.Input3 = "00000";
            data.MandiriClickpay.Token = "000000";
            return data;
        }
        CreditCardPaymentData CreateCreditCardPayData(UW400BookViewModel vm)
        {

            var data = new CreditCardPaymentData
            {
                PaymentType = "credit_card",
                CustomerDetails =
                {
                    Email = "jakarta@as.com",
                    FirstName = "jakarta",
                    LastName = "jakarta",
                    Phone = "jakarta",
                    BillingAddress =
                    {
                        Address = "jalan",
                        City = "jakarta",
                        CountryCode = "IDN",
                        Email = "bayualvian@hotmail.com",
                        FirstName = "jakarta",
                        LastName = "jakarta",
                        Phone = "jakarta",
                        PostalCode = "jakarta"
                    }    
                }
                
            };

            var listItemDetailDummy = new List<ItemDetail>();

            var detailDummy = new ItemDetail
            {
                Id = "123213", 
                Name = "asdsad", 
                Price = 100, 
                Quantity = 1
            };

            var detailDummy2 = new ItemDetail
            {
                Id = "1232123", 
                Name = "asdsad", 
                Price = 100, 
                Quantity = 1
            };

            listItemDetailDummy.Add(detailDummy);
            listItemDetailDummy.Add(detailDummy2);
            data.ItemDetails = listItemDetailDummy;

            data.TransactionDetails.OrderId = vm.OrderId;
            data.TransactionDetails.GrossAmount = 200;

            data.CreditCard.TokenId = vm.TokenId;
            data.CreditCard.Bank = "";
            return data;
        }
    }
}