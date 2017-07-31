using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Hotel.Constant;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.common;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.helpers;
using Lunggo.ApCommon.Product.Model;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds
{
    public class HotelBedsIssue
    {
        public HotelIssueTicketResult IssueHotel(HotelIssueInfo hotelIssueInfo)
        {
            var revalidateResult = RecheckReservation(hotelIssueInfo.Hotel, hotelIssueInfo.SearchId, hotelIssueInfo.State);
            if (!revalidateResult.IsSuccess)
            {
                return new HotelIssueTicketResult
                {
                    ErrorMessages = revalidateResult.ErrorMessages,
                    IsSuccess = false,
                    Status = "FAILED",
                    IsPriceChanged = revalidateResult.IsPriceChanged
                };
            }
            var client = new HotelApiClient(HotelApiType.BookingApi);
            var booking = new Booking();

            var firstname = hotelIssueInfo.Pax[0].FirstName;
            var lastname = hotelIssueInfo.Pax[0].LastName;
            string first, last;
            var splittedName = hotelIssueInfo.Contact.Name.Trim().Split(' ');
            if (splittedName.Length == 1)
            {
                first = hotelIssueInfo.Contact.Name;
                last = hotelIssueInfo.Contact.Name;
            }
            else
            {
                first = hotelIssueInfo.Contact.Name.Substring(0, hotelIssueInfo.Contact.Name.LastIndexOf(' '));
                last = splittedName[splittedName.Length - 1];
            }
            booking.createHolder(first, last);
            foreach (var room in hotelIssueInfo.Hotel.Rooms)
            {
                foreach (var rate in room.Rates)
                {
                    var confirmRoom = new ConfirmRoom {details = new List<RoomDetail>()};
                    var ratekey = rate.RateKey;
                    var data = ratekey.Split('|')[9];
                    var totalRoomForThisRate = Convert.ToInt32(data.Split('~')[0]);
                    var totalAdultperRate = Convert.ToInt32(data.Split('~')[1]);
                    var totalChildrenperRate = Convert.ToInt32(data.Split('~')[2]);

                    for (var z = 1; z <= totalRoomForThisRate; z++)
                    {
                        for (var i = 0; i < totalAdultperRate; i++)
                        {
                            confirmRoom.detailed(RoomDetail.GuestType.ADULT, 30, firstname, lastname, z);
                        }
                        for (var i = 0; i < totalChildrenperRate; i++)
                        {
                            confirmRoom.detailed(RoomDetail.GuestType.CHILD, 8, firstname, lastname, z);
                        }
                    }
                    
                    booking.addRoom(rate.RateKey, confirmRoom);
                }
            }

            booking.clientReference = hotelIssueInfo.RsvNo;
            if (hotelIssueInfo.Hotel.SpecialRequest != null)
            {
                booking.remark = hotelIssueInfo.Hotel.SpecialRequest;
            }

            var bookingRq = booking.toBookingRQ();
            if (bookingRq == null)
                return new HotelIssueTicketResult
                {
                    IsInstantIssuance = false,
                    Status = "FAILED"
                };
            var responseBooking = client.confirm(bookingRq);
            if (responseBooking == null)
                return new HotelIssueTicketResult
                {
                    IsSuccess = false,
                    Status = "FAILED"
                };
            if (responseBooking.error != null)
                return new HotelIssueTicketResult
                {
                    IsSuccess = false,
                    Status = "FAILED: " + responseBooking.error.message,
                };
            if (responseBooking.booking == null)
                return new HotelIssueTicketResult
                {
                    IsSuccess = false,
                    Status = "FAILED"
                };
            if (responseBooking.booking.status == SimpleTypes.BookingStatus.CONFIRMED)
            {
                return new HotelIssueTicketResult
                {
                    IsSuccess = true,
                    SupplierName = responseBooking.booking.hotel.supplier.name,
                    SupplierVat = responseBooking.booking.hotel.supplier.vatNumber,
                    RsvNo = hotelIssueInfo.RsvNo,
                    Status = "CONFIRMED",
                    BookingId = hotelIssueInfo.Hotel.Rooms.SelectMany(r => r.Rates.Select(t => t.RateKey)).ToList(),
                    ClientReference = responseBooking.booking.clientReference,
                    BookingReference = responseBooking.booking.reference
                };
            }

            return new HotelIssueTicketResult
            {
                IsSuccess = false,
                Status = "FAILED",
            };

        }

        public RevalidateHotelResult RecheckReservation(HotelDetail hotelDetail, string searchId, ReservationState state)
        {
            var oldPrice = hotelDetail.Rooms.Sum(room => room.Rates.Sum(rate => rate.Price.Supplier));
            var occupancies = new List<Occupancy>();

            foreach (var room in hotelDetail.Rooms)
            {
                occupancies.AddRange(room.Rates.Select(rate => new Occupancy
                {
                    RoomCount = rate.RateCount,
                    AdultCount = rate.AdultCount,
                    ChildCount = rate.ChildCount,
                    ChildrenAges = rate.ChildrenAges
                }));
            }

            occupancies = occupancies.Distinct().ToList();
            var rateFound = Enumerable.Repeat(false, occupancies.Count).ToList();

            var request = new SearchHotelCondition
            {
                Occupancies = occupancies,
                CheckIn = hotelDetail.CheckInDate,
                Checkout = hotelDetail.CheckOutDate,
                HotelCode = hotelDetail.HotelCode,
                SearchId = searchId
            };

            var hotelbeds = new HotelBedsSearchHotel();
            var searchResult = hotelbeds.SearchHotel(request);
            HotelService.GetInstance().AddPriceMargin(searchResult.HotelDetails, state.Currency);

            if (searchResult.HotelDetails == null || searchResult.HotelDetails.Count == 0)
            {
                Console.WriteLine("When refresh ratekeys, no hotel result");
                return new RevalidateHotelResult
                {
                    ErrorMessages = new List<string> { "When refresh ratekeys, no hotel result" },
                    IsSuccess = false
                };
            }

            if (searchResult.HotelDetails.Any(hotel => hotel.Rooms == null || hotel.Rooms.Count == 0))
            {
                Console.WriteLine("When refresh ratekeys, there is at least a hotel without Rooms");
                return new RevalidateHotelResult
                {
                    ErrorMessages = new List<string> { "When refresh ratekeys, there is at least a hotel without Rooms" },
                    IsSuccess = false
                };
            }

            if (searchResult.HotelDetails.Any(hotel => hotel.Rooms.Any(room => room.Rates == null || room.Rates.Count == 0)))
            {
                Console.WriteLine("When refresh ratekeys, there is at least a room without Rates");
                return new RevalidateHotelResult
                {
                    ErrorMessages = new List<string> { "When refresh ratekeys, there is at least a room without Rates" },
                    IsSuccess = false
                };
            }

            foreach (var room in hotelDetail.Rooms)
            {
                foreach (var rate in room.Rates)
                {
                    if (rate.TimeLimit >= DateTime.UtcNow) continue;
                    var sampleRatekey = rate.RateKey.Split('|');
                    var roomCd = sampleRatekey[5];
                    var someData = sampleRatekey[6];
                    var board = sampleRatekey[7];
                    var roomCount = rate.RateCount;
                    var adultCount = rate.AdultCount;
                    var childCount = rate.ChildCount;
                    var childrenAges = sampleRatekey[10];
                    var index = room.Rates.IndexOf(rate);

                    foreach (var rooma in searchResult.HotelDetails[0].Rooms)
                    {
                        foreach (var ratea in rooma.Rates)
                        {
                            var rateKey = ratea.RateKey.Split('|');
                            if (rateKey[5] == roomCd && rateKey[6] == someData && rateKey[7] == board &&
                                Convert.ToInt32(rateKey[9].Split('~')[0]) == roomCount &&
                                Convert.ToInt32(rateKey[9].Split('~')[1]) == adultCount &&
                                Convert.ToInt32(rateKey[9].Split('~')[2]) == childCount &&
                                rateKey[10] == childrenAges)
                            {
                                if (BookingStatusCd.Mnemonic(rate.Type) == CheckRateStatus.Recheck)
                                {
                                    var revalidateResult = HotelService.GetInstance().CheckRate(ratea.RegsId.Split(',')[2], ratea.Price.Supplier);
                                    if (revalidateResult.IsPriceChanged)
                                    {
                                        rate.Price.SetSupplier(revalidateResult.NewPrice.GetValueOrDefault(),
                                            rate.Price.SupplierCurrency);
                                    }
                                    rate.RateKey = revalidateResult.RateKey;
                                    rate.Price.SetSupplier(revalidateResult.NewPrice.GetValueOrDefault(),
                                        rate.Price.SupplierCurrency);
                                    rate.RateCommentsId = ratea.RateCommentsId;

                                }
                                else
                                {
                                    rate.RateKey = ratea.RateKey;
                                    rate.Price.SetSupplier(ratea.Price.OriginalIdr, rate.Price.SupplierCurrency);
                                    rate.RateCommentsId = ratea.RateCommentsId;
                                    rateFound[index] = true;
                                }
                            }
                        }
                    }
                }
            }

            if (rateFound.Any(r => r == false))
            {
                Console.WriteLine("At least one rate can not be found in refresh ratekey");
                return new RevalidateHotelResult
                {
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "At least one rate can not be found in refresh ratekey" }
                };
            }

            hotelDetail.Rooms.ForEach(ro => ro.Rates = HotelService.GetInstance().BundleRates(ro.Rates));

            var newPrice = hotelDetail.Rooms.Sum(room => room.Rates.Sum(rate => rate.Price.Supplier));
            if (newPrice != oldPrice)
            {
                return new RevalidateHotelResult
                {
                    IsPriceChanged = true,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "Price Changed (" + oldPrice + " -> " + newPrice + ")" },
                };
                
            }
            return new RevalidateHotelResult
            {
                IsPriceChanged = false,
                IsSuccess = true
            };
        }
    }
}
