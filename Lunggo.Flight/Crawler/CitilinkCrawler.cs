using CsQuery;
using Lunggo.Flight.Model;
using Lunggo.Framework.Web;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace Lunggo.Flight.Crawler
{
    public class CitilinkCrawler : ICrawler
    {
        string CitilinkBaseUrl = "https://book.citilink.co.id/";
        string AirLineName = "Citilink";
        int AirLineCode = 3;
        int percentageChildPrice = 75;
        int InfantPrice = 200000;
        public List<FlightTicket> Search(TicketSearch SearchParam)
        {
            try
            {
                CQ TableResult = CsQueryGetTbodyFromTableFlightIdCitilink(SearchParam);

                List<FlightTicket> ListFlightTicket = new List<FlightTicket>();
                List<List<string>> ResultListOfTable = TableResult.First().Find("tr").Select(tr => tr.Cq().Find("td").Select(td => td.InnerHTML).ToList()).Where(tr => tr.Count() == 5).ToList();
                ListFlightTicket.AddRange(ConvertHTMLTableToFlightTicketClass(ResultListOfTable, SearchParam, false));

                if (TableResult.Count() > 1)
                {
                    List<List<string>> ResultListOfTable2 = TableResult.Last().Find("tr").Select(tr => tr.Cq().Find("td").Select(td => td.InnerHTML).ToList()).Where(tr=>tr.Count()==5).ToList();
                    ListFlightTicket.AddRange(ConvertHTMLTableToFlightTicketClass(ResultListOfTable, SearchParam, true));
                }
                return ListFlightTicket;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public class Citilink : ExtendedWebClient
        {
            public bool Login()
            {
                string URI = "https://book.citilink.co.id/LoginAgent.aspx";
                string myParameters =  @"ControlGroupLoginAgentView$AgentLoginView$ButtonLogIn=Log+In" + 
                     @"&ControlGroupLoginAgentView$AgentLoginView$PasswordFieldPassword=Standar1234" +
                     @"&ControlGroupLoginAgentView$AgentLoginView$TextBoxUserID=Travelmadezy" +
                     @"&__EVENTARGUMENT=" +
                     @"&__EVENTTARGET=" +
                     //@"&__VIEWSTATE=/wEPDwUBMGRkBsrCYiDYbQKCOcoq/UTudEf14vk=" +
                     @"&pageToken" ;

                    Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    Headers[HttpRequestHeader.Accept] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                    Headers[HttpRequestHeader.Host] = "book.citilink.co.id";
                    Headers[HttpRequestHeader.Referer] = "https://book.citilink.co.id/LoginAgent.aspx?culture=id-ID";
                    Headers[HttpRequestHeader.AcceptLanguage] = "en-GB,en-US;q=0.8,en;q=0.6";
                    Headers["Origin"] = "https://book.citilink.co.id";
                    //Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
                    Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                    Headers["Upgrade-Insecure-Requests"] = "1";

                    string htmlResult = UploadString(URI,"POST",myParameters);
                    var c = htmlResult.Contains("Search.aspx");
                    if (!c)
                    {
                        bool respone = true;
                        return respone;
                    }
                    else
                    {
                        bool respone = false;
                        return respone;
                    }
                    
                    
                }

                 public string search()
                 {
                        Login();
                        string searchURI = @"https://book.citilink.co.id/BookingListTravelAgent.aspx";
                        Headers[HttpRequestHeader.Host] = "book.citilink.co.id";
                        Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:41.0) Gecko/20100101 Firefox/41.0";
                        Headers[HttpRequestHeader.Accept] = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                        Headers[HttpRequestHeader.AcceptLanguage] = "en-GB,en-US;q=0.8,en;q=0.6";
                        Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
                        Headers[HttpRequestHeader.Referer] = "https://book.citilink.co.id/BookingListTravelAgent.aspx";
                        Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                        Headers["Origin"] = "https://book.citilink.co.id";
                        Headers["Upgrade-Insecure-Requests"] = "1";
                        
                        

                     string searchParamaters =
                         @"AvailabilitySearchInputBookingListTravelAgentView$ButtonSubmit=Find+Flights" +
                         @"&AvailabilitySearchInputBookingListTravelAgentView$DropDownListMarketDay1=19" +
                         @"&AvailabilitySearchInputBookingListTravelAgentView$DropDownListMarketDay2=12" +
                         @"&AvailabilitySearchInputBookingListTravelAgentView$DropDownListMarketMonth1=2015-12" +
                         @"&AvailabilitySearchInputBookingListTravelAgentView$DropDownListMarketMonth2=2015-10" +
                         @"&AvailabilitySearchInputBookingListTravelAgentView$DropDownListPassengerType_ADT=1" +
                         @"&AvailabilitySearchInputBookingListTravelAgentView$DropDownListPassengerType_CHD=0" +
                         @"&AvailabilitySearchInputBookingListTravelAgentView$DropDownListPassengerType_INFANT=0" +
                         @"&AvailabilitySearchInputBookingListTravelAgentView$DropDownListSearchBy=columnView" +
                         @"&AvailabilitySearchInputBookingListTravelAgentView$RadioButtonMarketStructure=OneWay" +
                         @"&AvailabilitySearchInputBookingListTravelAgentView$TextBoxMarketDestination1=SOC" +
                         @"&AvailabilitySearchInputBookingListTravelAgentView$TextBoxMarketDestination2=" +
                         @"&AvailabilitySearchInputBookingListTravelAgentView$TextBoxMarketOrigin1=HLP" +
                         @"&AvailabilitySearchInputBookingListTravelAgentView$TextBoxMarketOrigin2=" +
                         @"&AvailabilitySearchInputBookingListTravelAgentViewdestinationStation1=SOC" +
                         @"&AvailabilitySearchInputBookingListTravelAgentViewdestinationStation2=" +
                         @"&AvailabilitySearchInputBookingListTravelAgentVieworiginStation1=HLP" +
                         @"&AvailabilitySearchInputBookingListTravelAgentVieworiginStation2=" +
                         @"&ControlGroupBookingListTravelAgentView$BookingListBookingListTravelAgentView$DropDownListTypeOfSearch=0" +
                         @"&ControlGroupBookingListTravelAgentView$BookingListBookingListTravelAgentView$Search=ForAgent" +
                         @"&ControlGroupBookingListTravelAgentView$BookingListBookingListTravelAgentView$TextBoxKeyword=" +
                         @"&__EVENTARGUMENT=" +
                         @"&__EVENTTARGET=" +
                         //@"&__VIEWSTATE=/wEPDwUBMGQYAQUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgMFWkNvbnRyb2xHcm91cEJvb2tpbmdMaXN0VHJhdmVsQWdlbnRWaWV3JEJvb2tpbmdMaXN0Qm9va2luZ0xpc3RUcmF2ZWxBZ2VudFZpZXckUmFkaW9Gb3JBZ2VudAVbQ29udHJvbEdyb3VwQm9va2luZ0xpc3RUcmF2ZWxBZ2VudFZpZXckQm9va2luZ0xpc3RCb29raW5nTGlzdFRyYXZlbEFnZW50VmlldyRSYWRpb0ZvckFnZW5jeQVbQ29udHJvbEdyb3VwQm9va2luZ0xpc3RUcmF2ZWxBZ2VudFZpZXckQm9va2luZ0xpc3RCb29raW5nTGlzdFRyYXZlbEFnZW50VmlldyRSYWRpb0ZvckFnZW5jeTXhy2ltZry/VwOL4DGYiD+r/S9H" +
                         @"&date_picker=2015-10-09" +
                         @"&date_picker=2015-10-12" +
                         @"&pageToken=";

                     string searchResult = UploadString(searchURI,searchParamaters);

                     return searchResult;
                    }

            public void Wait()
            {
                string waitURI = "https://book.citilink.co.id/Wait.aspx";
               
                Headers[HttpRequestHeader.Host] = "book.citilink.co.id";
                Headers[HttpRequestHeader.UserAgent] ="Mozilla/5.0 (Windows NT 6.3; WOW64; rv:41.0) Gecko/20100101 Firefox/41.0";
                Headers[HttpRequestHeader.Accept] = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                Headers[HttpRequestHeader.AcceptLanguage] = "en-GB,en-US;q=0.8,en;q=0.6";
                Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
                Headers[HttpRequestHeader.Referer] = "https://book.citilink.co.id/Wait.aspx";
                Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                Headers["Origin"] = "https://book.citilink.co.id";
                Headers["Upgrade-Insecure-Requests"] = "1";

                DownloadString(waitURI);
                var cek = ResponseUri.AbsolutePath;
                while (cek.Contains("Wait"))
                {
                    string waitResult = DownloadString(waitURI);
                }
                
            }

            public string Select()
            {
                string selectURI = @"https://book.citilink.co.id/ScheduleSelect.aspx";
                Headers[HttpRequestHeader.Host] = "book.citilink.co.id";
                Headers[HttpRequestHeader.UserAgent] ="Mozilla/5.0 (Windows NT 6.3; WOW64; rv:41.0) Gecko/20100101 Firefox/41.0";
                Headers[HttpRequestHeader.Accept] = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                Headers[HttpRequestHeader.AcceptLanguage] = "en-GB,en-US;q=0.8,en;q=0.6";
                Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
                Headers[HttpRequestHeader.Referer] = "https://book.citilink.co.id/ScheduleSelect.aspx";
                Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                Headers["Origin"] = "https://book.citilink.co.id";
                Headers["Upgrade-Insecure-Requests"] = "1";

                string selectParameters =
                    @"AvailabilitySearchInputScheduleSelectView$DdlCurrencyDynamic=IDR" +
                    @"&AvailabilitySearchInputScheduleSelectView$DropDownListMarketDay1=19" +
                    @"&AvailabilitySearchInputScheduleSelectView$DropDownListMarketDay2=26" +
                    @"&AvailabilitySearchInputScheduleSelectView$DropDownListMarketMonth1=2015-12" +
                    @"&AvailabilitySearchInputScheduleSelectView$DropDownListMarketMonth2=2015-12" +
                    @"&AvailabilitySearchInputScheduleSelectView$DropDownListPassengerType_ADT=1" +
                    @"&AvailabilitySearchInputScheduleSelectView$DropDownListPassengerType_CHD=0" +
                    @"&AvailabilitySearchInputScheduleSelectView$DropDownListPassengerType_INFANT=0" +
                    @"&AvailabilitySearchInputScheduleSelectView$DropDownListSearchBy=columnView" +
                    @"&AvailabilitySearchInputScheduleSelectView$RadioButtonMarketStructure=OneWay" +
                    @"&AvailabilitySearchInputScheduleSelectView$TextBoxMarketDestination1=SOC" +
                    @"&AvailabilitySearchInputScheduleSelectView$TextBoxMarketDestination2=" +
                    @"&AvailabilitySearchInputScheduleSelectView$TextBoxMarketOrigin1=HLP" +
                    @"&AvailabilitySearchInputScheduleSelectView$TextBoxMarketOrigin2=" +
                    @"&AvailabilitySearchInputScheduleSelectViewdestinationStation1=SOC" +
                    @"&AvailabilitySearchInputScheduleSelectViewdestinationStation2=" +
                    @"&AvailabilitySearchInputScheduleSelectVieworiginStation1=HLP" +
                    @"&AvailabilitySearchInputScheduleSelectVieworiginStation2=" +
                    @"&ControlGroupScheduleSelectView$AvailabilityInputScheduleSelectView$HiddenFieldTabIndex1=4" +
                    @"&ControlGroupScheduleSelectView$AvailabilityInputScheduleSelectView$market1=0~E~~E~RGFR~~1~X|QG~ 150~ ~~HLP~12/19/2015 13:55~SOC~12/19/2015 15:10~" +
                    @"&ControlGroupScheduleSelectView$ButtonSubmit=Lanjutkan" +
                    @"&__EVENTARGUMENT=" +
                    @"&__EVENTTARGET=" +
                    //@"&__VIEWSTATE=/wEPDwUBMGRkBsrCYiDYbQKCOcoq/UTudEf14vk=" +
                    @"&date_picker=2015-12-19" +
                    @"&date_picker=2015-12-26" +
                    @"&pageToken=";

                string searchResult = UploadString(selectURI, selectParameters);
                return searchResult;
            }

            public string Passenger()
            {
                string passURI = @"https://book.citilink.co.id/Passenger.aspx";
                Headers[HttpRequestHeader.Host] = "book.citilink.co.id";
                Headers[HttpRequestHeader.UserAgent] ="Mozilla/5.0 (Windows NT 6.3; WOW64; rv:41.0) Gecko/20100101 Firefox/41.0";
                Headers[HttpRequestHeader.Accept] = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                Headers[HttpRequestHeader.AcceptLanguage] = "en-GB,en-US;q=0.8,en;q=0.6";
                Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
                Headers[HttpRequestHeader.Referer] = "https://book.citilink.co.id/Passenger.aspx";
                Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                Headers["Origin"] = "https://book.citilink.co.id";
                Headers["Upgrade-Insecure-Requests"] = "1";

                string passParameters =
                    @"CONTROLGROUPPASSENGER$ButtonSubmit=Continue" +
                    @"&CONTROLGROUPPASSENGER$ContactInputPassengerView$DropDownListCountry=" +
                    @"&CONTROLGROUPPASSENGER$ContactInputPassengerView$DropDownListStateProvince=" +
                    @"&CONTROLGROUPPASSENGER$ContactInputPassengerView$DropDownListTitle=MR" +
                    @"&CONTROLGROUPPASSENGER$ContactInputPassengerView$TextBoxAddressLine1=Jl. Jend Sudirman Kav. 52-53" +
                    @"&CONTROLGROUPPASSENGER$ContactInputPassengerView$TextBoxAddressLine2=Equity Tower, 25th Floor" +
                    @"&CONTROLGROUPPASSENGER$ContactInputPassengerView$TextBoxAddressLine3=SCBD Lot 9" +
                    @"&CONTROLGROUPPASSENGER$ContactInputPassengerView$TextBoxCity=Jakarta Selatan" +
                    @"&CONTROLGROUPPASSENGER$ContactInputPassengerView$TextBoxEmailAddress=dwi.agustina@travelmadezy.com" +
                    @"&CONTROLGROUPPASSENGER$ContactInputPassengerView$TextBoxFax=021-29035099" +
                    @"&CONTROLGROUPPASSENGER$ContactInputPassengerView$TextBoxFirstName=Dwi" +
                    @"&CONTROLGROUPPASSENGER$ContactInputPassengerView$TextBoxHomePhone=019217126" +
                    @"&CONTROLGROUPPASSENGER$ContactInputPassengerView$TextBoxLastName=Agustina" +
                    @"&CONTROLGROUPPASSENGER$ContactInputPassengerView$TextBoxMiddleName=middle" +
                    @"&CONTROLGROUPPASSENGER$ContactInputPassengerView$TextBoxPostalCode=zip/postal" +
                    @"&CONTROLGROUPPASSENGER$ContactInputPassengerView$TextBoxWorkPhone=0811351793" +
                    @"&CONTROLGROUPPASSENGER$ItineraryDistributionInputPassengerView$Distribution=2" +
                    @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListBirthDateDay_0=1" +
                    @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListBirthDateMonth_0=1" +
                    @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListBirthDateYear_0=1970" +
                    @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListDocumentCountry0_0=" +
                    @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListDocumentDateDay0_0=1" +
                    @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListDocumentDateMonth0_0=1" +
                    @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListDocumentDateYear0_0=2015" +
                    @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListDocumentType0_0=" +
                    @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListNationality_0=" +
                    @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListResidentCountry_0=" +
                    @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListTitle_0=MR" +
                    @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$RadioButtonInsurance=No" +
                    @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$TextBoxDocumentNumber0_0=" +
                    @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$TextBoxFirstName_0=Rahmat" +
                    @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$TextBoxLastName_0=Pratama" +
                    @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$TextBoxMiddleName_0=middle" +
                    @"&__EVENTARGUMENT=" +
                    @"&__EVENTTARGET=" +
                    //@"&__VIEWSTATE=/wEPDwUBMGQYAQUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgIFR0NPTlRST0xHUk9VUFBBU1NFTkdFUiRQYXNzZW5nZXJJbnB1dFZpZXdQYXNzZW5nZXJWaWV3JENoZWNrQm94SW5zdXJhbmNlBUFDT05UUk9MR1JPVVBQQVNTRU5HRVIkUGFzc2VuZ2VySW5wdXRWaWV3UGFzc2VuZ2VyVmlldyRDaGVja0JveFBNSXZkWh6Cdtm1mad5oP+7VGz2nQKe
                    @"&pageToken=";

                string passResult = UploadString(passURI, passParameters);
                return passResult;
            }

            public string Kursi()
            {
                string kursiURI = @"https://book.citilink.co.id/SeatMap.aspx";
                Headers[HttpRequestHeader.Host] = "book.citilink.co.id";
                Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:41.0) Gecko/20100101 Firefox/41.0";
                Headers[HttpRequestHeader.Accept] = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                Headers[HttpRequestHeader.AcceptLanguage] = "en-GB,en-US;q=0.8,en;q=0.6";
                Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
                Headers[HttpRequestHeader.Referer] = "https://book.citilink.co.id/SeatMap.aspx";
                Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                Headers["Origin"] = "https://book.citilink.co.id";
                Headers["Upgrade-Insecure-Requests"] = "1";

                string kursiParameter =
                    @"ControlGroupUnitMapView$UnitMapViewControl$EquipmentConfiguration_0_PassengerNumber_0=" +
                    @"&ControlGroupUnitMapView$UnitMapViewControl$HiddenEquipmentConfiguration_0_PassengerNumber_0=" +
                    @"&ControlGroupUnitMapView$UnitMapViewControl$compartmentDesignatorInput=" +
                    @"&ControlGroupUnitMapView$UnitMapViewControl$deckDesignatorInput=1" +
                    @"&ControlGroupUnitMapView$UnitMapViewControl$passengerInput=0" +
                    @"&ControlGroupUnitMapView$UnitMapViewControl$tripInput=0" +
                    @"&__EVENTARGUMENT=" +
                    @"&__EVENTTARGET=ControlGroupUnitMapView$UnitMapViewControl$LinkButtonAssignUnit" +
                    //"&__VIEWSTATE=/wEPDwUBMGQYAQUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgEFN0NvbnRyb2xHcm91cFVuaXRNYXBWaWV3JFVuaXRNYXBWaWV3Q29udHJvbCRDaGVja0JveFNlYXRC9WoNScpqWuAJVhj4Iqw3MUfIjw=="
                    @"&pageToken=";
                string kursiResult = UploadString(kursiURI, kursiParameter);
                return kursiResult;
            }

            public string Payment()
            {
                string paymentURI = @"https://book.citilink.co.id/Payment.aspx";
                Headers[HttpRequestHeader.Host] = "book.citilink.co.id";
                Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:41.0) Gecko/20100101 Firefox/41.0";
                Headers[HttpRequestHeader.Accept] = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                Headers[HttpRequestHeader.AcceptLanguage] = "en-GB,en-US;q=0.8,en;q=0.6";
                Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
                Headers[HttpRequestHeader.Referer] = "https://book.citilink.co.id/Payment.aspx";
                Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                Headers["Origin"] = "https://book.citilink.co.id";
                Headers["Upgrade-Insecure-Requests"] = "1";

                string payParameter =
                    @"__EVENTTARGET=" +
                    @"&__EVENTARGUMENT=" +
                    //@"&__VIEWSTATE=%2FwEPDwUBMGRkBsrCYiDYbQKCOcoq%2FUTudEf14vk%3D" +
                    @"&pageToken=" +
                    @"&DropDownListPaymentMethodCode=AgencyAccount%3AAG" +
                    @"&DropDownListPaymentMethodCode=PrePaid%3AKB" +
                    @"&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24TxtCardNumber=" +
                    @"&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24TxtVcc=" +
                    @"&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24DdlExpMonth=1 " +
                    @"&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24DdlExpYear=2015" +
                    @"&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24TxtCardHolderName=Dwi+Agustina" +
                    @"&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24TxtCardHolderAddress=Jl.+Jend+Sudirman+Kav.+52-53" +
                    @"&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24TxtCardHolderCity=Jakarta+Selatan" +
                    @"&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24TxtCardHolderProvince=" +
                    @"&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24TxtCardHolderZipCode=" +
                    @"&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24DdlCardHolderCountry=AD" +
                    @"&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24TxtCardHolderEmail=dwi.agustina%40travelmadezy.com" +
                    @"&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24TxtCardHolderPhone=7439843" +
                    @"&DropDownListPaymentMethodCode=ExternalAccount%3AMC" +
                    @"&DropDownListPaymentMethodCode=Voucher%3AVO" +
                    @"&TextBoxVoucherAccount_VO_ACCTNO=" +
                    @"&AMOUNT=" +
                    @"&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24DropDownListPaymentMethodCode=PrePaid%3AHOLD" +
                    @"&DropDownListPaymentMethodCode=PrePaid%3AHOLD" +
                    @"&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24AgreementPaymentInputViewPaymentView%24CheckBoxAgreement=on" +
                    @"&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24storedPaymentId=" +
                    @"&CONTROLGROUPPAYMENTBOTTOM%24ButtonSubmit=Lanjutkan";

                string payResult = UploadString(paymentURI, payParameter);
                return payResult;
            }
        }

       


        CQ CsQueryGetTbodyFromTableFlightIdCitilink(TicketSearch SearchParam)
        {
            CQ CsQueryContentPencarianCitilink = GetCsQueryHTMLContentPencarianCitilink(SearchParam);
            //CQ CsQueryTable = CsQueryContentPencarianCitilink["div>.w99.availabilityTable>tbody"];
            CQ CsQueryTable = CsQueryContentPencarianCitilink["div>.w99.availabilityTable>tbody"];
            return CsQueryTable;
        }
        CQ GetCsQueryHTMLContentPencarianCitilink(TicketSearch SearchParam)
        {
            string HTMLContentPencarianCitilink = GetStringHTMLContentPencarianCitilink(SearchParam);
            CQ CsQueryContentPencarianCitilink = CQ.Create(HTMLContentPencarianCitilink);
            return CsQueryContentPencarianCitilink;
        }
        string GetStringHTMLContentPencarianCitilink(TicketSearch SearchParam)
        {
            RestClient RestClientCitilink = GetRestClientForCitilink();
            RestRequest RestRequestCitilink = GetRestRequestForSearchPage();
            IRestResponse ResponseSearchPage = RestClientCitilink.Execute(RestRequestCitilink);
            string ViewStateFromSearchPage = GetViewStateFromSearchPage(ResponseSearchPage);
            IRestResponse ResponsePencarianTicket = RestResponseCitilinkPencarianTicket(RestClientCitilink, SearchParam, ViewStateFromSearchPage);
            return ResponsePencarianTicket.Content;
        }
        RestClient GetRestClientForCitilink()
        {
            RestClient RestClientCitilink = new RestClient();
            RestClientCitilink.BaseUrl = this.CitilinkBaseUrl;
            RestClientCitilink.CookieContainer = new CookieContainer();
            return RestClientCitilink;
        }
        RestRequest GetRestRequestForSearchPage()
        {
            RestRequest RestRequestForSearchPage = new RestRequest(Method.GET);
            RestRequestForSearchPage.Resource = "SearchOnly.aspx";
            return RestRequestForSearchPage;
        }
        string GetViewStateFromSearchPage(IRestResponse ResponseSearchPage)
        {
            CQ CsQueryContentSearchPage = CQ.Create(ResponseSearchPage.Content);
            CQ CsQueryIDViewState = CsQueryContentSearchPage["#viewState"];
            return CsQueryIDViewState.Val();
        }
        IRestResponse RestResponseCitilinkPencarianTicket(RestClient RestClientCitilink, TicketSearch SearchParam, string ViewStateFromSearchPage)
        {
            try
            {
                RestRequest RestRequestForPencarianTicket = ConvertRestRequestForPencarianTicket(SearchParam, ViewStateFromSearchPage);
                IRestResponse ResponsePencarianTicket = RestClientCitilink.Execute(RestRequestForPencarianTicket);
                return ResponsePencarianTicket;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        RestRequest ConvertRestRequestForPencarianTicket(TicketSearch SearchParam, string ViewStateFromSearchPage)
        {
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.Resource = "SearchOnly.aspx";
            request.AddParameter("__VIEWSTATE", ViewStateFromSearchPage);
            request.AddParameter("AvailabilitySearchInputSearchOnlyView$DdlCurrencyDynamic", "IDR");
            request.AddParameter("AvailabilitySearchInputSearchOnlyVieworiginStation1", SearchParam.DepartFromCode);
            request.AddParameter("AvailabilitySearchInputSearchOnlyView$TextBoxMarketOrigin1", SearchParam.DepartFromCode);
            request.AddParameter("AvailabilitySearchInputSearchOnlyViewdestinationStation1", SearchParam.DepartToCode);
            request.AddParameter("AvailabilitySearchInputSearchOnlyView$TextBoxMarketDestination1", SearchParam.DepartToCode);
            request.AddParameter("date_picker", SearchParam.DepartDate.ToString("yyyy-MM-dd"));
            request.AddParameter("AvailabilitySearchInputSearchOnlyView$DropDownListMarketDay1", SearchParam.DepartDate.ToString("dd"));
            request.AddParameter("AvailabilitySearchInputSearchOnlyView$DropDownListMarketMonth1", SearchParam.DepartDate.ToString("yyyy-MM"));
            if (SearchParam.IsReturn)
            {
                request.AddParameter("date_picker", SearchParam.ReturnDate.ToString("yyyy-MM-dd"));
                request.AddParameter("AvailabilitySearchInputSearchOnlyView$DropDownListMarketDay2", SearchParam.ReturnDate.ToString("dd"));
                request.AddParameter("AvailabilitySearchInputSearchOnlyView$DropDownListMarketMonth2", SearchParam.ReturnDate.ToString("yyyy-MM"));
                request.AddParameter("AvailabilitySearchInputSearchOnlyView$RadioButtonMarketStructure", "RoundTrip");
            }
            else
            {
                request.AddParameter("AvailabilitySearchInputSearchOnlyView$RadioButtonMarketStructure", "OneWay");
            }
            request.AddParameter("AvailabilitySearchInputSearchOnlyView$DropDownListPassengerType_ADT", SearchParam.Adult);
            request.AddParameter("AvailabilitySearchInputSearchOnlyView$DropDownListPassengerType_CHD", SearchParam.Child);
            request.AddParameter("AvailabilitySearchInputSearchOnlyView$DropDownListPassengerType_INFANT", SearchParam.Infant);
            request.AddParameter("AvailabilitySearchInputSearchOnlyView$DropDownListSearchBy", "columnView");
            request.AddParameter("AvailabilitySearchInputSearchOnlyView$ButtonSubmit", "Find Flights");
            return request;

        }
        List<FlightTicket> ConvertHTMLTableToFlightTicketClass(List<List<string>> ResultListOfTable, TicketSearch SearchParam, bool returning)
        {
            try
            {
                List<FlightTicket> ListFlightTicket = new List<FlightTicket>();
                for (int TableRow = 0; TableRow < ResultListOfTable.Count(); TableRow++)
                {
                    if (isRowHeaderThatDoesntContainAnyData(ResultListOfTable[TableRow]))
                        continue;
                    FlightTicket Ticket = new FlightTicket();
                    for (int TableColumn = 0; TableColumn < ResultListOfTable[TableRow].Count(); TableColumn++)
                    {

                        if (!string.IsNullOrEmpty(ResultListOfTable[TableRow][TableColumn]))
                            ResultListOfTable[TableRow][TableColumn] = ReplaceUnnecessaryHTMLTag(ResultListOfTable[TableRow][TableColumn]);
                        else
                            ResultListOfTable[TableRow][TableColumn] = "";
                        if (TableColumn == 0)
                        {
                            List<DepartDetail> listDepartDetail = new List<DepartDetail>();
                            if (isHaveMultipleFlightTime(ResultListOfTable[TableRow][TableColumn]))
                            {
                                string[] ListTimeDetail = ResultListOfTable[TableRow][TableColumn].Replace("<br><br>", "§").Split('§');
                                string[] ListLocationDetail = ResultListOfTable[TableRow][TableColumn + 1].Replace("<br><br>", "§").Split('§');
                                string[] ListFlightCodeDetail = ResultListOfTable[TableRow][TableColumn + 2].Split('/');
                                for (int i = 0; i < ListTimeDetail.Count(); i++)
                                {
                                    DepartDetail departDetail = new DepartDetail();
                                    string[] splittedTime = ListTimeDetail[i].Replace("<br>", "§").Split('§');
                                    departDetail.DepartTime = ConvertStringTimeToTimeSpan(splittedTime[0].Trim());
                                    departDetail.ArrivedTime = ConvertStringTimeToTimeSpan(splittedTime[1].Trim());

                                    string[] splittedLocation = ListLocationDetail[i].Replace("<br>", "§").Split('§');
                                    departDetail.DepartFrom = splittedLocation[0].Trim();
                                    departDetail.ArrivedAt = splittedLocation[1].Trim();

                                    departDetail.FlightCode = GetFlightCode(ListFlightCodeDetail[i].Trim());

                                    listDepartDetail.Add(departDetail);
                                }
                            }
                            else
                            {
                                
                                DepartDetail departDetail = new DepartDetail();
                                string[] splittedTime = ResultListOfTable[TableRow][TableColumn].Replace("<br>", "§").Split('§');
                                departDetail.DepartTime = ConvertStringTimeToTimeSpan(splittedTime[0].Trim());
                                departDetail.ArrivedTime = ConvertStringTimeToTimeSpan(splittedTime[1].Trim());

                                string[] splittedLocation = ResultListOfTable[TableRow][TableColumn+1].Replace("<br>", "§").Split('§');
                                departDetail.DepartFrom = splittedLocation[0].Trim();
                                departDetail.ArrivedAt = splittedLocation[1].Trim();

                                departDetail.FlightCode = GetFlightCode(ResultListOfTable[TableRow][TableColumn + 2].Trim());
                                listDepartDetail.Add(departDetail);
                            }
                            Ticket.ListDepartDetail.AddRange(listDepartDetail);
                            TableColumn = TableColumn + 2;
                        }
                        else if (TableColumn == 3)
                        {
                            Ticket.AdultTicket.EconomicPrice = Ticket.AdultTicket.PromoPrice = GetPrice(ResultListOfTable[TableRow][TableColumn]);

                        }
                        else if (TableColumn == 4)
                        {
                            Ticket.AdultTicket.BusinessPrice = GetPrice(ResultListOfTable[TableRow][TableColumn]);
                            //if(SearchParam.Child>0)
                            //    Ticket.ChildTicket.BusinessPrice = Ticket.AdultTicket.BusinessPrice * this.percentageChildPrice / 100;
                            //if(SearchParam.Infant > 0)
                            //    Ticket.InfantTicket.BusinessPrice = this.InfantPrice;
                        }
                    }
                    Ticket.AirlineName = this.AirLineName;
                    Ticket.AirlineCode = this.AirLineCode;
                    Ticket.returning = returning;
                    ListFlightTicket.Add(Ticket);
                }
                return ListFlightTicket;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        List<DepartDetail> GetDepartDetailFromArrayValue(string[] FirstColumnValue)
        {
            List<DepartDetail> ListDepartDetail = new List<DepartDetail>();
            DepartDetail FlightDetail = new DepartDetail();
            for (int i = 0; i < FirstColumnValue.Count(); i++)
            {
                if (i % 4 > 1)
                {
                    if (i % 4 == 2)
                    {
                        ListDepartDetail.Add(FlightDetail);
                        FlightDetail = new DepartDetail();
                    }
                    continue;
                }
                if (i % 4 == 0)
                {
                    FlightDetail.FlightCode = FirstColumnValue[i];
                }
                else if (i % 4 == 1)
                {
                    string[] SplitDepart = FirstColumnValue[i].Split('-');
                    FlightDetail.DepartTime = ConvertStringTimeToTimeSpan(SplitDepart[0].Substring(0, 4));
                    FlightDetail.DepartFrom = SplitDepart[0].Substring(5, 3);
                    FlightDetail.ArrivedTime = ConvertStringTimeToTimeSpan(SplitDepart[1].Substring(1, 4));
                    FlightDetail.ArrivedAt = SplitDepart[1].Substring(6, 3);
                }

            }
            return ListDepartDetail;
        }
        bool isRowHeaderThatDoesntContainAnyData(List<string> ListColumn)
        {
            return ListColumn.Count() < 1 ? true : false;
        }
        bool isHaveMultipleFlightTime(string column)
        {
            return column.Contains("<br><br>") ? true : false;
        }
        string[] ReplaceUnnecessaryHTMLTagForFirstColumnAndSplit(string StringHTML)
        {
            StringHTML = StringHTML.Replace("<b>", "");
            StringHTML = StringHTML.Replace("</b>", "");
            return StringHTML.Split('§');
        }
        string ReplaceUnnecessaryHTMLTag(string StringHTML)
        {
            StringHTML = StringHTML.Replace("\n", "");
            StringHTML = StringHTML.Replace("/n", "");
            StringHTML = StringHTML.Replace("\t", "");
            StringHTML = StringHTML.Replace("/t", "");
            return StringHTML.ToString().Trim();
        }
        bool isPromo(CQ temp)
        {
            string ClassNameForPromo = ".classofservice";
            var x = temp.Find(ClassNameForPromo).Select(tr => tr.InnerHTML).ToList();
            if (x.Count() > 0)
                return true;
            else
                return false;
        }
        string GetFlightCode(string RawFlightCode)
        {
            string[] splitRawCode = RawFlightCode.Replace("&nbsp;\n", "§").Split('§');
            return string.Format("{0} {1}", splitRawCode[0].Trim(), splitRawCode[1].Trim());
        }
        int? GetPrice(string StringHTML)
        {
            StringHTML = StringHTML.Replace("<p>", "");
            StringHTML = StringHTML.Replace("</p>", "");
            if (string.IsNullOrEmpty(StringHTML))
                return null;
            CQ temp = CQ.Create(StringHTML);
            string HargaTicket = temp[1].ToString().Trim();
            HargaTicket = Regex.Replace(HargaTicket, "[^0-9]", "");
            return Convert.ToInt32(HargaTicket);
        }
        TimeSpan ConvertStringTimeToTimeSpan(string stringTime)
        {
            return TimeSpan.ParseExact(stringTime, @"h\:mm", CultureInfo.InvariantCulture);
        }
    }
}
