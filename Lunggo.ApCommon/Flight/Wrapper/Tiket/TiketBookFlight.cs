using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Flight.Wrapper.Tiket.Model;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.Framework.Extension;
using RestSharp;
using Supplier = Lunggo.ApCommon.Payment.Constant.Supplier;

namespace Lunggo.ApCommon.Flight.Wrapper.Tiket
{
    internal partial class TiketWrapper
    {
        internal override BookFlightResult BookFlight(FlightBookingInfo bookInfo)
        {
            return Client.BookFlight(bookInfo);
        }


        private partial class TiketClientHandler
        {
            internal BookFlightResult BookFlight(FlightBookingInfo bookInfo)
            {

                var bookResult = new BookFlightResult();

                //Step 1 : Add Order
                var addOrderResponse = AddOrder(bookInfo);
                if(addOrderResponse == null)
                    return new BookFlightResult
                    {
                        IsSuccess = false,
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Failed
                        },
                        Errors = new List<FlightError> { FlightError.TechnicalError },
                        ErrorMessages = new List<string> { "[Tiket] Failed to Add Order" }
                    };

                //Step 2 : Order
                var orderResult = Order(token);
                if (orderResult == null || orderResult.Myorder == null)
                    return new BookFlightResult
                    {
                        IsSuccess = false,
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Failed
                        },
                        Errors = new List<FlightError> { FlightError.TechnicalError },
                        ErrorMessages = new List<string> { "[Tiket] Failed to Order" }
                    };
                
                bookResult = new BookFlightResult
                    {
                        IsSuccess = true,
                        IsValid = true,
                        IsItineraryChanged = false,
                        IsPriceChanged = false,
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Booked,
                            BookingId = orderResult.Myorder == null ? "" : orderResult.Myorder.Order_id,
                            TimeLimit = orderResult.Myorder == null ? new DateTime() : orderResult.Myorder.Data[0].order_expire_datetime
                        }
                    };

                //Step 3 : Checkout Request
                var checkoutResponse = CheckoutPage(token, orderResult.Myorder.Order_id);
                if (checkoutResponse == null)
                    return new BookFlightResult
                    {
                        IsSuccess = false,
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Failed
                        },
                        Errors = new List<FlightError> { FlightError.TechnicalError },
                        ErrorMessages = new List<string> { "[Tiket] Failed to Checkout Page Request" }
                    };

                //Step 4 : Checkout Page Customer
                var checkoutCustResponse = CheckoutPageCustomer(token);
                if (checkoutCustResponse == null)
                    return new BookFlightResult
                    {
                        IsSuccess = false,
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Failed
                        },
                        Errors = new List<FlightError> { FlightError.TechnicalError },
                        ErrorMessages = new List<string> { "[Tiket] Failed to Checkout Page Request" }
                    };


                return bookResult;
            }


            internal TiketBaseResponse AddOrder(FlightBookingInfo bookInfo)
            {
                var isLion = false;
                var lionSessionId = "";
                var lionCaptcha = "";
                if (bookInfo.Itinerary.Trips[0].Segments[0].AirlineCode.Equals("JT"))
                {
                    isLion = true;
                    var lionData = GetLionCaptcha(bookInfo.Token);
                    if (lionData != null)
                    {
                        lionSessionId = lionData.Lionsessionid;
                        lionCaptcha = lionData.Lioncaptcha;
                    } 
                }
                var adults = bookInfo.Passengers.Where(x => x.Type == PaxType.Adult).ToList();
                var childs = bookInfo.Passengers.Where(x => x.Type == PaxType.Child).ToList();
                var infants = bookInfo.Passengers.Where(x => x.Type == PaxType.Infant).ToList();

                var client = CreateTiketClient();
                var url = "/order/add/flight";
                var request = new RestRequest(url, Method.GET);
                var first = "";
                var last = "";
                int i = 1;
                Random r = new Random();
                var generateId = r.Next(1000, 10000);
                if (bookInfo.Contact.Name == null)
                {
                    first = bookInfo.Contact.Name;
                    last = bookInfo.Contact.Name;
                }
                else
                {
                    var splittedName = adults.Take(1).FirstOrDefault().FirstName.Split(' ');
                    if (splittedName.Length == 1)
                    {
                        first = bookInfo.Contact.Name;
                        last = bookInfo.Contact.Name;
                    }
                    else
                    {
                        first = bookInfo.Contact.Name.Substring(0, bookInfo.Contact.Name.LastIndexOf(' '));
                        last = splittedName[splittedName.Length - 1];
                    }
                }

                request.AddQueryParameter("token", bookInfo.Token);
                request.AddQueryParameter("flight_id", bookInfo.Itinerary.FareId);

                if (isLion)
                {
                    request.AddQueryParameter("lioncaptcha", lionCaptcha);
                    request.AddQueryParameter("lionsessionid", lionSessionId);
                }

                request.AddQueryParameter("child", bookInfo.Passengers.Count(x => x.Type == PaxType.Child).ToString());
                request.AddQueryParameter("adult", bookInfo.Passengers.Count(x => x.Type == PaxType.Adult).ToString());
                request.AddQueryParameter("infant", bookInfo.Passengers.Count(x => x.Type == PaxType.Infant).ToString());
                request.AddQueryParameter("conSalutation", TitleCd.Mnemonic(bookInfo.Contact.Title));
                request.AddQueryParameter("conFirstName", first);
                request.AddQueryParameter("conLastName", last);
                request.AddQueryParameter("conPhone", "%2B"+ bookInfo.Contact.CountryCallingCode + bookInfo.Contact.Phone);
                request.AddQueryParameter("conEmailAddress", bookInfo.Contact.Email);
                request.AddQueryParameter("output", "json");

                //TODO Need Improvement
                if (adults != null && adults.Count != 0)
                {
                    i =  1;
                    foreach (var adult in adults)
                    {
                        generateId += 1;
                        request.AddQueryParameter("firstnamea" + i, adult.FirstName);
                        request.AddQueryParameter("lastnamea" + i, adult.LastName);
                        if (bookInfo.Itinerary.RequireBirthDate)
                        {
                            request.AddQueryParameter("birthdatea" + i, adult.DateOfBirth.Value.ToString("yyyy-MM-dd"));
                        }
                        
                        request.AddQueryParameter("titlea" + i, GetTitle(adult.Title));
                        request.AddQueryParameter("ida" + i, generateId.ToString());
                        //if (bookInfo.Itinerary.RequireNationality)
                        //{
                        //    request.AddQueryParameter("passportnationalitya" + i, adult.Nationality);
                        //}
                        if (!string.IsNullOrEmpty(bookInfo.Itinerary.Trips[0].Segments[0].BaggageCapacity))
                        {
                            request.AddQueryParameter("dcheckinbaggagea1" + i, bookInfo.Itinerary.Trips[0].Segments[0].BaggageCapacity);
                        }
                        if (bookInfo.Itinerary.RequirePassport)
                        {
                            request.AddQueryParameter("passportnoa" + i, adult.PassportNumber);
                            request.AddQueryParameter("passportExpiryDatea" + i, adult.PassportExpiryDate.Value.ToString("yyyy-MM-dd"));
                            var issueDate = adult.PassportExpiryDate.Value.AddYears(-5).AddDays(1);
                            request.AddQueryParameter("passportnationalitya" + i, adult.PassportCountry);
                            request.AddQueryParameter("passportissueddatea" + i, issueDate.ToString("yyyy-MM-dd"));
                            request.AddQueryParameter("passportissuinga" + i, adult.PassportCountry);
                        }
                        i++;
                    }
                }

                if ( childs != null && childs.Count != 0)
                {
                    i = 1;
                    foreach (var child in childs)
                    {
                        generateId += 1;
                        request.AddQueryParameter("firstnamec" + i, child.FirstName);
                        request.AddQueryParameter("lastnamec" + i, child.LastName);
                        request.AddQueryParameter("idc" + i, generateId.ToString());
                        request.AddQueryParameter("titlec" + i, GetTitle(child.Title));
                        request.AddQueryParameter("birthdatec" + i, child.DateOfBirth.Value.ToString("yyyy-MM-dd"));
                        //if (bookInfo.Itinerary.RequireNationality)
                        //{
                        //    request.AddQueryParameter("passportnationalityc" + i, child.Nationality);
                        //}
                        if (!string.IsNullOrEmpty(bookInfo.Itinerary.Trips[0].Segments[0].BaggageCapacity))
                        {
                            request.AddQueryParameter("dcheckinbaggagec1" + i, bookInfo.Itinerary.Trips[0].Segments[0].BaggageCapacity);
                        }
                        if (bookInfo.Itinerary.RequirePassport)
                        {
                            request.AddQueryParameter("passportnoc" + i, child.PassportNumber);
                            request.AddQueryParameter("passportExpiryDatec" + i, child.PassportExpiryDate.Value.ToString("yyyy-MM-dd"));
                            var issueDate = child.PassportExpiryDate.Value.AddYears(-5).AddDays(1);
                            request.AddQueryParameter("passportnationalityc" + i, child.PassportCountry);
                            request.AddQueryParameter("passportissueddatec" + i, issueDate.ToString("yyyy-MM-dd"));
                            request.AddQueryParameter("passportissuingc" + i, child.PassportCountry);
                            
                        }

                        i++;
                    }
                }

                if (infants != null && infants.Count != 0)
                {
                    i = 1;
                    foreach (var infant in infants)
                    {
                        generateId += 1;
                        request.AddQueryParameter("firstnamei" + i, infant.FirstName);
                        request.AddQueryParameter("lastnamei" + i, infant.LastName);
                        request.AddQueryParameter("titlei" + i, GetTitle(infant.Title));
                        request.AddQueryParameter("parenti" + i, adults.First().FirstName);
                        request.AddQueryParameter("idi" + i, generateId.ToString());
                        request.AddQueryParameter("birthdatei" + i, infant.DateOfBirth.Value.ToString("yyyy-MM-dd"));
                        if (!string.IsNullOrEmpty(bookInfo.Itinerary.Trips[0].Segments[0].BaggageCapacity))
                        {
                            request.AddQueryParameter("dcheckinbaggagei1" + i, bookInfo.Itinerary.Trips[0].Segments[0].BaggageCapacity);
                        }
                        //if (bookInfo.Itinerary.RequireNationality)
                        //{
                        //    request.AddQueryParameter("passportnationalityi" + i, infant.Nationality);
                        //}
                        if (bookInfo.Itinerary.RequirePassport)
                        {
                            request.AddQueryParameter("passportnoi" + i, infant.PassportNumber);
                            request.AddQueryParameter("passportExpiryDatei" + i, infant.PassportExpiryDate.Value.ToString("yyyy-MM-dd"));
                            var issueDate = infant.PassportExpiryDate.Value.AddYears(-5).AddDays(1);
                            request.AddQueryParameter("passportnationalityi" + i, infant.PassportCountry);
                            request.AddQueryParameter("passportissueddatei" + i, DateTime.Now.ToString("yyyy-MM-dd"));
                            request.AddQueryParameter("passportissuingi" + i, infant.PassportCountry);
                        }
                        i++;
                    }
                }

                var response = client.Execute(request);

                var addOrderResponse = JsonExtension.Deserialize<TiketBaseResponse>(response.Content);
                
                if (addOrderResponse == null && (addOrderResponse.Diagnostic.Status != "200"))
                    return null;

                return addOrderResponse;

            }

            internal OrderResponse Order(string token)
            {
                var client = CreateTiketClient();
                var url = "/order?token=" + token + "&output=json";
                var request = new RestRequest(url, Method.GET);
                var response = client.Execute(request);
                var orderResponse = JsonExtension.Deserialize<OrderResponse>(response.Content);
                if (orderResponse == null && orderResponse.Diagnostic.Status != "200")
                    return null;
                return orderResponse;
            }

            internal string GetTitle(Title title)
            {
                var titleTostring = "";
                switch (title)
                {
                    case Title.Miss:
                        titleTostring = "Miss";
                        break;
                    case Title.Mister:
                        titleTostring = "Mr";
                        break;
                    case Title.Mistress:
                        titleTostring = "Mrs";
                        break;
                }
                return titleTostring;
            }

            //step 1
            internal TiketBaseResponse CheckoutPage(string _token, string orderId)
            {
                var client = CreateTiketClient();
                var url = "order/checkout/" + orderId + "/IDR?token=" + _token + "&output=json";
                var request = new RestRequest(url, Method.GET);
                var response = client.Execute(request);
                var flightData = JsonExtension.Deserialize<TiketBaseResponse>(response.Content);
                if (flightData == null && flightData.Diagnostic.Status != "200")
                    return null;
                return flightData;
            }

            //step2
            internal TiketBaseResponse CheckoutPageCustomer(string _token)
            {
                var client = CreateTiketClient();
                var url = "/checkout/checkout_customer";
                var request = new RestRequest(url, Method.GET);
                request.AddQueryParameter("token", _token);
                request.AddQueryParameter("salutation", "Mr");
                request.AddQueryParameter("firstName", "Suheri");
                request.AddQueryParameter("lastName", "Marpaung");
                request.AddQueryParameter("emailAddress", "suheri@travelmadezy.com");
                request.AddQueryParameter("phone", "%2B85360343300");
                request.AddQueryParameter("saveContinue", "2");
                request.AddQueryParameter("output", "json");
                var response = client.Execute(request);
                var flightData = JsonExtension.Deserialize<TiketBaseResponse>(response.Content);
                if (flightData == null && flightData.Diagnostic.Status != "200")
                    return null;
                return flightData;
            }
        }

        internal override RevalidateFareResult RevalidateFare(RevalidateConditions conditions)
        {
            return new RevalidateFareResult();
        }


        internal override Currency CurrencyGetter(string currency)
        {
            return new Currency(currency, Supplier.Sriwijaya);
        }

        internal override List<BookingStatusInfo> GetBookingStatus()
        {
            throw new NotImplementedException();
        }

    }
}
