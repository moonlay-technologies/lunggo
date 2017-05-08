using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CsQuery;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Config;
using Lunggo.Framework.Encoder;
using Lunggo.Framework.Log;
using Lunggo.Framework.Web;
using RestSharp;

namespace Lunggo.ApCommon.Flight.Wrapper.Sriwijaya
{
    internal partial class SriwijayaWrapper
    {
        public static int issueTrial = 0;	
        internal override IssueTicketResult OrderTicket(string bookingId, bool canHold)
        {
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            if (env == "production")
                return Client.OrderTicket(bookingId);
            else
                return new IssueTicketResult
                {
                    IsSuccess = true,
                    BookingId = bookingId,
                    IsInstantIssuance = true
                };
        }

        private partial class SriwijayaClientHandler
        {
            internal IssueTicketResult OrderTicket(string bookingId)
            {
                var log = LogService.GetInstance();
                var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
                var clientx = CreateAgentClient();
                var untukEncode = "ticketing:" + bookingId + ":STEP2";
                var encode = untukEncode.Base64Encode();
                var encode2 = encode.Base64Encode();
                log.Post("[Sriwijaya] Login", "#logging-issueflight");
                Login(clientx);
                try
                {

                    log.Post("[Sriwijaya] Check Booking Id. Url : SJ-Eticket/application/?action=CheckBCode", "#logging-issueflight");
                    var url = "SJ-Eticket/application/?action=CheckBCode";
                    var submitRequest1 = new RestRequest(url, Method.POST);
                    var postData = //reffNo=XBTGQQ&Submit=Go
                        "reffNo=" + bookingId +
                        "&Submit=Go";
                    submitRequest1.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                    var submitResponse1 = clientx.Execute(submitRequest1);

                    url = "SJ-Eticket/application/?action=CheckBCode";
                    var submitRequest2 = new RestRequest(url, Method.POST);
                    postData = //reffNo=XBTGQQ&action=CheckBCode&step=STEP2
                        "reffNo=" + bookingId +
                        "&action=CheckBCode" +
                        "&step=STEP2";
                    submitRequest2.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                    var submitResponse2 = clientx.Execute(submitRequest2);
                    
                    url = "SJ-Eticket/application/?action=CheckBCode";
                    var submitRequest = new RestRequest(url, Method.POST);
                    postData = //Submit=Issue&action=ticketing&reffNo=ZEdsamEyVjBhVzVuT2xoQ1ZFZFJVVHBUVkVWUU1nPT0%3D
                        "Submit=Issue" +
                        "&action=ticketing" +
                        "&reffNo="+ encode2 +"%3D";
                    submitRequest.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                    var submitResponse = clientx.Execute(submitRequest);

                    log.Post("[Sriwijaya] Post Booking Id. Url : SJ-Eticket/application/?", "#logging-issueflight");
                    url = "SJ-Eticket/application/?action=CheckBCode&reffNo=" + bookingId;
                    var checkRequest = new RestRequest(url, Method.POST);
                    postData = //reffNo=XBTGQQ&action=CheckBCode&step=STEP2
                        "reffNo=" + bookingId +
                        "&action=CheckBCode" +
                        "&step=STEP2";
                    checkRequest.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                    var checkResponse = clientx.Execute(checkRequest);
                    var cekresult = checkResponse.Content;
                   
                    //var cekresult = System.IO.File.ReadAllText(@"C:\Users\User\Documents\Kerja\Crawl\REPONSEissue.txt");
                    var ambilTimeLimit = (CQ)cekresult;

                    var tunjukClassTarget = ambilTimeLimit["#pagewrapper>#pnr .reservationDetail .bookingCodeWrap:nth-child(3)>.bookingCode"];
                    var tunjukStatusBook = tunjukClassTarget.MakeRoot()[".bookingCode"];
                    var statusBook = tunjukClassTarget.Select(x => x.Cq().Text()).FirstOrDefault();

                    log.Post("[Sriwijaya] Cek Booking Status", "#logging-issueflight");
                    var hasil = new IssueTicketResult();
                    if ((statusBook == "Confirm") || (statusBook == "Confirmed"))
                    {
                        log.Post("[Sriwijaya] Success", "#logging-issueflight");
                        hasil.BookingId = bookingId;
                        hasil.IsSuccess = true;
                        hasil.IsInstantIssuance = true;
                    }
                    else
                    {
                        log.Post("[Sriwijaya] Error because status book is not Confirm or confirmed", "#logging-issueflight");
                        hasil.CurrentBalance = GetCurrentBalance();
                        hasil.IsSuccess = false;
                        hasil.Errors = new List<FlightError> { FlightError.FailedOnSupplier };
                        hasil.ErrorMessages = new List<string> { "[Sriwijaya] Error because status book is not Confirm or confirmed" };
                    }
                    
                    Logout(clientx);
                    return hasil;
                }
                catch (Exception)
                {
                    var isIssued = IsIssued(bookingId);
                    switch (isIssued)
                    {
                        case IssueEnum.IssueSuccess:
                            Logout(clientx);
                            return new IssueTicketResult
                            {
                                IsSuccess = true,
                                BookingId = bookingId
                            };
                        case IssueEnum.NotIssued:
                            Logout(clientx);
                            if (issueTrial < 3)
                            {
                                Debug.Print("issueTrial : " + issueTrial);
                                issueTrial += 1;
                                Client.OrderTicket(bookingId);
                            }
                            return new IssueTicketResult
                            {
                                CurrentBalance = GetCurrentBalance(),
                                IsSuccess = false
                            };
                        case IssueEnum.CheckingError:
                            Logout(clientx);
                            if (issueTrial < 3)
                            {
                                Debug.Print("issueTrial : " + issueTrial);
                                issueTrial += 1;
                                Client.OrderTicket(bookingId);
                            }
                            return new IssueTicketResult
                            {
                                CurrentBalance = GetCurrentBalance(),
                                IsSuccess = false,
                                Errors = new List<FlightError> { FlightError.TechnicalError },
                                ErrorMessages = new List<string> { "[Sriwijaya] Failed to check whether deposit cut or not! Manual checking advised!" }
                            };
                        default:
                            Logout(clientx);
                            return new IssueTicketResult
                            {
                                CurrentBalance = GetCurrentBalance(),
                                IsSuccess = false,
                                Errors = new List<FlightError> { FlightError.TechnicalError },
                                ErrorMessages = new List<string> { "[Sriwijaya] Failed to check whether deposit cut or not! Manual checking advised!" }
                            };
                    }
                }
            }
        }
    }
}