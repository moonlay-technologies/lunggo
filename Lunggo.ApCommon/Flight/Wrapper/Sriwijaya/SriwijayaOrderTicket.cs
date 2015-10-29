using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsQuery;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Encoder;
using Lunggo.Framework.Web;

namespace Lunggo.ApCommon.Flight.Wrapper.Sriwijaya
{
    internal partial class SriwijayaWrapper
    {
        internal override OrderTicketResult OrderTicket(string bookingId, bool canHold)
        {
            return Client.OrderTicket(bookingId, canHold);
        }

        private partial class SriwijayaClientHandler
        {
            internal OrderTicketResult OrderTicket(string bookingId, bool canHold)
            {
                var client = new ExtendedWebClient();
                var untukEncode = "ticketing:" + bookingId + ":STEP2";
                var encode = untukEncode.Base64Encode();
                var encode2 = encode.Base64Encode();

                Client.CreateSession(client);
                try
                {
                    client.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                    //client.Headers["Accept-Encoding"] = "gzip, deflate";
                    client.Headers["Content-Type"] = "application/x-www-form-urlencoded";
                    client.Headers["Accept-Language"] = "en-GB,en-US;q=0.8,en;q=0.6";
                    client.Headers["Upgrade-Insecure-Requests"] = "1";
                    client.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                    client.Headers["Referer"] = "http://agent.sriwijayaair.co.id/SJ-Eticket/application/?action=CheckBCode&reffNo=" + bookingId + "";
                    client.Headers["Host"] = "agent.sriwijayaair.co.id";
                    client.Headers["Origin"] = "https://www.sriwijayaair.co.id";
                    client.AutoRedirect = true;
                    client.Expect100Continue = false;

                    var issueparams =
                        "Submit=Issue" +
                        "&action=ticketing" +
                        "&reffNo="+ encode2 +"%3D";

                    client.UploadString("http://agent.sriwijayaair.co.id/SJ-Eticket/application/?action=CheckBCode&reffNo="+ bookingId +"", issueparams);
        
                    client.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                    //client.Headers["Accept-Encoding"] = "gzip, deflate";
                    client.Headers["Content-Type"] = "application/x-www-form-urlencoded";
                    client.Headers["Accept-Language"] = "en-GB,en-US;q=0.8,en;q=0.6";
                    client.Headers["Upgrade-Insecure-Requests"] = "1";
                    client.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                    client.Headers["Referer"] = "http://agent.sriwijayaair.co.id/SJ-Eticket/application/?action=CheckBCode&reffNo=" + bookingId + "";
                    client.Headers["Host"] = "agent.sriwijayaair.co.id";
                    client.Headers["Origin"] = "https://www.sriwijayaair.co.id";
                    client.AutoRedirect = true;

                    var cekparams =
                        "reffNo=" + bookingId +
                        "&action=CheckBCode" +
                        "&step=STEP2";

                    var cekresult = client.UploadString("http://agent.sriwijayaair.co.id/SJ-Eticket/application/?", cekparams);
                   
                    //var cekresult = System.IO.File.ReadAllText(@"C:\Users\User\Documents\Kerja\Crawl\REPONSEissue.txt");
                    CQ ambilTimeLimit = (CQ)cekresult;

                    var tunjukClassTarget = ambilTimeLimit["#pagewrapper>#pnr .reservationDetail .bookingCodeWrap:nth-child(3)>.bookingCode"];
                    var tunjukStatusBook = tunjukClassTarget.MakeRoot()[".bookingCode"];
                    var statusBook = tunjukClassTarget.Select(x => x.Cq().Text()).FirstOrDefault();

                    var hasil = new OrderTicketResult();
                    if ((statusBook == "Confirm") || (statusBook == "Confirmed"))
                    {
                        hasil.BookingId = bookingId;
                        hasil.IsSuccess = true;
                        hasil.IsInstantIssuance = true;
                    }
                    else
                    {
                        hasil.IsSuccess = false;
                        hasil.Errors = new List<FlightError> { FlightError.FailedOnSupplier };
                    }
                    
                    Client.LogoutSession(client);
                    return hasil;
                }
                catch (Exception)
                {
                    var isIssued = IsIssued(bookingId);
                    switch (isIssued)
                    {
                        case IssueEnum.IssueSuccess:
                            Client.LogoutSession(client);
                            return new OrderTicketResult
                            {
                                IsSuccess = true,
                                BookingId = bookingId
                            };
                        case IssueEnum.NotIssued:
                            Client.LogoutSession(client);
                            return new OrderTicketResult
                            {
                                IsSuccess = false
                            };
                        case IssueEnum.CheckingError:
                            Client.LogoutSession(client);
                            return new OrderTicketResult
                            {
                                IsSuccess = false,
                                Errors = new List<FlightError> { FlightError.TechnicalError },
                                ErrorMessages = new List<string> { "Failed to check whether deposit cut or not! Manual checking advised!" }
                            };
                        default:
                            Client.LogoutSession(client);
                            return new OrderTicketResult
                            {
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