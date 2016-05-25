using System.Collections.Generic;
using System.Linq;
using CsQuery;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Web;
using RestSharp;

namespace Lunggo.ApCommon.Flight.Wrapper.Sriwijaya
{
    internal partial class SriwijayaWrapper
    {
        private partial class SriwijayaClientHandler
        {
            private IssueEnum IsIssued(string bookingId)
            {
                const int maxRetryCount = 10;
                var counter = 0;
                var isIssued = (bool?)null;
                while (counter++ < maxRetryCount && isIssued == null)
                {
                    var clientx = CreateAgentClient();

                    Login(clientx);

                    var url = "SJ-Eticket/application/?action=CheckBCode&reffNo=" + bookingId;
                    var selectRequest = new RestRequest(url, Method.POST);
                    var postData =
                        "Submit=Issue" +
                        "&action=ticketing" +
                        "&reffNo=ZEdsamEyVjBhVzVuT2tsR1RFMVZSenBUVkVWUU1nPT0%3D";
                    selectRequest.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                    var selectResponse = clientx.Execute(selectRequest);
                    if (!(selectResponse.ResponseUri.AbsoluteUri.Contains("/application/?action=Check")))
                    {
                        continue;
                    }

                    url = "SJ-Eticket/application/?";
                    var checkRequest = new RestRequest(url, Method.POST);
                    postData = 
                        "reffNo=" + bookingId +
                        "&action=CheckBCode" +
                        "&step=STEP2";
                    checkRequest.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                    var checkResponse = clientx.Execute(checkRequest);
                    var cekresult = checkResponse.Content;

                    if (!(checkResponse.ResponseUri.AbsoluteUri.Contains("/application/?action=Check")))
                    {
                        continue;
                    }

                    //var cekresult = System.IO.File.ReadAllText(@"C:\Users\User\Documents\Kerja\Crawl\REPONSEissue.txt");
                    CQ ambilTimeLimit = (CQ)cekresult;

                    var tunjukClassTarget = ambilTimeLimit["#pagewrapper>#pnr .reservationDetail .bookingCodeWrap:nth-child(3)>.bookingCode"];
                    var tunjukStatusBook = tunjukClassTarget.MakeRoot()[".bookingCode"];
                    var statusBook = tunjukClassTarget.Select(x => x.Cq().Text()).FirstOrDefault();

                    var hasil = new IssueTicketResult();
                    if ((statusBook == "Confirm") || (statusBook == "Confirmed"))
                    {
                        hasil.BookingId = bookingId;
                        hasil.IsSuccess = true;
                    }
                    else
                    {
                        hasil.IsSuccess = false;
                        hasil.Errors = new List<FlightError> { FlightError.FailedOnSupplier };
                    }

                    isIssued = tunjukClassTarget.FirstElement() != null;
                }
                switch (isIssued)
                {
                    case null:
                        return IssueEnum.CheckingError;
                    case true:
                        return IssueEnum.IssueSuccess;
                    case false:
                        return IssueEnum.NotIssued;
                    default:
                        return IssueEnum.CheckingError;
                }
            }

            private enum IssueEnum
            {
                IssueSuccess,
                NotIssued,
                CheckingError
            }

        }
    }
}
