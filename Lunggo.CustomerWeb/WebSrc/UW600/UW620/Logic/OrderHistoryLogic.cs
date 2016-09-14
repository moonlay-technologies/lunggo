using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.CustomerWeb.WebSrc.UW600.UW620.Model;
using Lunggo.CustomerWeb.WebSrc.UW600.UW620.Object;
using Lunggo.Framework.Database;
using Lunggo.ApCommon.Hotel.Logic;
using Lunggo.ApCommon.Hotel.Logic.Search;
using Lunggo.ApCommon.Hotel.Object;
using Lunggo.CustomerWeb.WebSrc.UW600.UW620.Query;


namespace Lunggo.CustomerWeb.WebSrc.UW600.UW620.Logic
{
    public class OrderHistoryLogic
    {

        public static IEnumerable<Uw620CompleteHistory> SendHistory(Uw620OrderHistoryRespone request)
        {            
            var HotelRespone = GetHotelDetail(request.IdMember,request);
            var FlightRespone = GetFlightDetail(request.IdMember, request);


            return MergeHotelFlight(HotelRespone, FlightRespone);
        }

        public static IEnumerable<Uw620HotelHistory> GetHotelDetail(String idMember, Uw620OrderHistoryRespone request)
        {
            var connOpen = DbService.GetInstance().GetOpenConnection();
            
            var query = GetHotelHistory.GetInstance();
            var dataCapture = query.Execute(connOpen, new
            {
                idMember
            });

            IEnumerable<Uw620HotelHistory> HotelMapping = null;
            //dataCapture.Select(selectedItem => new Uw620HotelHistory
            //{
            //    IdMember = idMember,
            //    HotelName = getHotelName(request, selectedItem.HotelNo),
            //    CheckInDateTime = (DateTime)selectedItem.CheckInDate,
            //    CheckOutDateTime = (DateTime)selectedItem.CheckOutDate,
            //    OrderId = selectedItem.RsvNo,
            //    StatusPayment = getStatusPayment(selectedItem.PaymentStatusCd),
            //    Address = getAddress(request, selectedItem.HotelNo),
            //    Type = "Hotel"
            //});
            
            return HotelMapping;
        }

        public static string getHotelName(Uw620OrderHistoryRespone request,String hotelNo)
        {
            request.HotelId = hotelNo;
            var searchServiceRequest = PreProcessHotelDetailRequest(request);
            var hotelDetail = HotelsSearchService.GetHotelDetail(searchServiceRequest.HotelId);

            var hotName = hotelDetail.HotelName;

            if (hotName == null)
            {
                hotName = hotelNo;
            }

            return hotName;
        }

        public static string getAddress(Uw620OrderHistoryRespone request, String hotelNo)
        {
            request.HotelId = hotelNo;
            var searchServiceRequest = PreProcessHotelDetailRequest(request);
            var hotelDetail = HotelsSearchService.GetHotelDetail(searchServiceRequest.HotelId);


            var hotAddress = hotelDetail.Address;

            if (hotAddress == null)
            {
                hotAddress = "Unknown";
            }

            return hotAddress;
        }

        public static string getStatusPayment(String statusCode)
        {
            String statusPayment;

            if (statusCode == "01")
            {
                statusPayment = "Pending";
            }
            else
            {
                statusPayment = "Done";
            }

            return statusPayment;
        }


        public static IEnumerable<Uw620FlightHistory> GetFlightDetail(String idMember, Uw620OrderHistoryRespone request)
        {
            var connOpen = DbService.GetInstance().GetOpenConnection();

            var query = GetFlightHistory.GetInstance();
            var dataCapture = query.Execute(connOpen, new
            {
                idMember
            });

            var FlightMapping = dataCapture.Select(selectedItem => new Uw620FlightHistory
            {
                IdMember = idMember,
                OrderId = selectedItem.RsvNo,
                DepartureTime = (DateTime) selectedItem.DepartureDate,
                TypeWay = selectedItem.OverallTripTypeCd,
                Airline = getFlightAirline(selectedItem.AirlineCd),//selectedItem.AirlineCd,
                StatusPayment = getStatusPayment(selectedItem.PaymentStatusCd),
                Origin = getAirportName(selectedItem.OriginAirportCd),//selectedItem.OriginAirportCd,
                Destination = getAirportName(selectedItem.DestinationAirportCd),//selectedItem.DestinationAirportCd,
                Type = "Flight"
            });

            return FlightMapping;
        }

        public static IEnumerable<Uw620CompleteHistory> MergeHotelFlight(IEnumerable<Uw620HotelHistory> hotelHistory,
            IEnumerable<Uw620FlightHistory> flightHistory)
        {
            var allMap = new List<Uw620CompleteHistory>();

            foreach (var selectedItem in hotelHistory)
            {
                allMap.Add(new Uw620CompleteHistory()
                {
                    IdMember = selectedItem.IdMember,
                    HotelName = selectedItem.HotelName,
                    CheckInDateTime = selectedItem.CheckInDateTime,
                    CheckOutDateTime = selectedItem.CheckOutDateTime,
                    OrderId = selectedItem.OrderId,
                    StatusPayment = selectedItem.StatusPayment,
                    Address = selectedItem.Address,
                    Type = "Hotel"
                });                
            }

            foreach (var selectedItem in flightHistory)
            {
                allMap.Add(new Uw620CompleteHistory()
                {
                    IdMember = selectedItem.IdMember,
                    OrderId = selectedItem.OrderId,
                    DepartureTime = selectedItem.DepartureTime,
                    Origin = selectedItem.Origin,
                    Destination = selectedItem.Destination,
                    TypeWay = selectedItem.TypeWay,
                    Airline = selectedItem.Airline,
                    StatusPayment = selectedItem.StatusPayment,
                    Type = "Flight" 
                });
            }
            
            return allMap;
        } 

        private static HotelRoomsSearchServiceRequest PreProcessHotelDetailRequest(Uw620OrderHistoryRespone request)
        {
            var searchServiceRequest = ParameterPreProcessor.InitializeHotelRoomsSearchServiceRequest(request);
            ParameterPreProcessor.PreProcessLangParam(searchServiceRequest, request);
            ParameterPreProcessor.PreProcessStayLengthParam(searchServiceRequest, request);
            ParameterPreProcessor.PreProcessStayDateParam(searchServiceRequest, request);
            ParameterPreProcessor.PreProcessRoomCountParam(searchServiceRequest, request);
            return searchServiceRequest;
        }

        public static string getFlightAirline(string airLineCode)
        {
            var airlineName = dictAirline(airLineCode);

            return airlineName;
        }

        public static string dictAirline(string theAirlineCode)
        {
            var airlineName = "";
            
            if (theAirlineCode == "MH")
            {
                airlineName = "Malaysia Airlines";
            }else if (theAirlineCode == "JT")
            {
                airlineName = "Lion Air";
            }

            return airlineName;;
        }

        public static string getAirportName(string airportCode)
        {
            var airportName = dictDestination(airportCode);

            return airportName;
        }

        public static string dictDestination(string theAirportCode)
        {
            var airportName = "";

            if (theAirportCode == "CGK")
            {
                airportName = "Jakarta";
            }
            else if (theAirportCode == "TKG")
            {
                airportName = "Lampung";
            }
            else if (theAirportCode == "JUN")
            {
                airportName = "Surabaya";
            }

            return airportName; 
        }


    }
}