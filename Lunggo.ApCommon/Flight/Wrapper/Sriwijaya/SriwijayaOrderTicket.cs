using System;
using System.Collections.Generic;
using System.Linq;
using CsQuery;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Config;
using Lunggo.Framework.Encoder;
using Lunggo.Framework.Web;
using RestSharp;

namespace Lunggo.ApCommon.Flight.Wrapper.Sriwijaya
{
    internal partial class SriwijayaWrapper
    {
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
                var clientx = CreateAgentClient();
                var untukEncode = "ticketing:" + bookingId + ":STEP2";
                var encode = untukEncode.Base64Encode();
                var encode2 = encode.Base64Encode();

                Login(clientx);
                try
                {
                    var url = "SJ-Eticket/application/?action=CheckBCode&reffNo=" + bookingId;
                    var submitRequest = new RestRequest(url, Method.POST);
                    var postData =
                        "Submit=Issue" +
                        "&action=ticketing" +
                        "&reffNo="+ encode2 +"%3D";
                    submitRequest.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                    var submitResponse = clientx.Execute(submitRequest);

                    url = "SJ-Eticket/application/?";
                    var checkRequest = new RestRequest(url, Method.POST);
                    postData =
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

                    var hasil = new IssueTicketResult();
                    if ((statusBook == "Confirm") || (statusBook == "Confirmed"))
                    {
                        hasil.BookingId = bookingId;
                        hasil.IsSuccess = true;
                        hasil.IsInstantIssuance = true;
                    }
                    else
                    {
                        hasil.CurrentBalance = GetCurrentBalance();
                        hasil.IsSuccess = false;
                        hasil.Errors = new List<FlightError> { FlightError.FailedOnSupplier };
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
                            return new IssueTicketResult
                            {
                                CurrentBalance = GetCurrentBalance(),
                                IsSuccess = false
                            };
                        case IssueEnum.CheckingError:
                            Logout(clientx);
                            return new IssueTicketResult
                            {
                                CurrentBalance = GetCurrentBalance(),
                                IsSuccess = false,
                                Errors = new List<FlightError> { FlightError.TechnicalError },
                                ErrorMessages = new List<string> { "Failed to check whether deposit cut or not! Manual checking advised!" }
                            };
                        default:
                            Logout(clientx);
                            return new IssueTicketResult
                            {
                                CurrentBalance = GetCurrentBalance(),
                                IsSuccess = false,
                                Errors = new List<FlightError> { FlightError.TechnicalError },
                                ErrorMessages = new List<string> { "Failed to check whether deposit cut or not! Manual checking advised!" }
                            };
                    }
                }
            }
        }
    }
}