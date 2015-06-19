﻿// ************************
// Angular app
(function () {

    var app = angular.module('travorama', ['ngRoute']);

    // hotel controller
    app.controller('HotelController', [
        '$http', '$scope', function ($http, $scope) {

            // run hotel search function on document ready
            angular.element(document).ready(function () {
                $scope.loadHotelList();
            });

            $scope.loaded = false;

            $scope.star = {
                star1: true,
                star2: true,
                star3: true,
                star4: true,
                star5: true
            }

            // hotel list
            var hotel_list = this;
            hotel_list.hotels = [];

            // hotel search params
            $scope.HotelSearchParams = {};

            // *******************************
            // default value
            $scope.HotelSearchParams.StayDate = $('.search-page.hotel-search-page').attr('data-search-StayDate');
            $scope.HotelSearchParams.StayLength = $('.search-page.hotel-search-page').attr('data-search-StayLength');
            $scope.HotelSearchParams.LocationId = $('.search-page.hotel-search-page').attr('data-search-LocationId');
            $scope.HotelSearchParams.ResultCount = $('.search-page.hotel-search-page').attr('data-search-ResultCount');
            $scope.HotelSearchParams.RoomCount = $('.search-page.hotel-search-page').attr('data-search-RoomCount');
            // *******************************

            $scope.getStar = function (starRating) {
                return new Array(starRating);
            }

            $scope.getStarO = function (starRating) {
                starRating = 5 - starRating;
                return new Array(starRating);
            }

            // load hotel list function
            $scope.loadHotelList = function (page) {

                $scope.loaded = false;

                // set default page
                $scope.CurrentPage = page || 1;

                // set startIndex
                $scope.CurrentPage = $scope.CurrentPage - 1;
                $scope.HotelSearchParams.StartIndex = (SearchHotelConfig.ResultCount * $scope.CurrentPage);

                $scope.hotels = [];

                // generate StarRating
                var tempArr = [];
                $scope.HotelSearchParams.StarRating;
                ($scope.star.star1) ? tempArr.push('1') : null;
                ($scope.star.star2) ? tempArr.push('2') : null;
                ($scope.star.star3) ? tempArr.push('3') : null;
                ($scope.star.star4) ? tempArr.push('4') : null;
                ($scope.star.star5) ? tempArr.push('5') : null;
                $scope.HotelSearchParams.StarRating = tempArr.join(',');

                // generate minimum and maximum price
                var priceRange = $('#price-range').val().split(",");
                $scope.HotelSearchParams.MinPrice = priceRange[0];
                $scope.HotelSearchParams.MaxPrice = priceRange[1];

                console.log('--------------------------------');
                console.log('Searching for hotel with params:');
                console.log($scope.HotelSearchParams);

                // http request
                $http.get(SearchHotelConfig.Url, {
                    params: {
                        LocationId: $scope.HotelSearchParams.LocationId,
                        StayDate: $scope.HotelSearchParams.StayDate,
                        StayLength: $scope.HotelSearchParams.StayLength,
                        SearchId: $scope.HotelSearchParams.SearchId,
                        SortBy: $scope.HotelSearchParams.SortBy,
                        StartIndex: $scope.HotelSearchParams.StartIndex,
                        ResultCount: SearchHotelConfig.ResultCount,
                        MinPrice: $scope.HotelSearchParams.MinPrice,
                        MaxPrice: $scope.HotelSearchParams.MaxPrice,
                        StarRating: $scope.HotelSearchParams.StarRating
                    }
                    // if success
                }).success(function (data) {
                    // console data
                    console.log('Hotel search result:');
                    console.log(data);

                    // add data to $scope
                    hotel_list.hotels = data.HotelList;
                    $scope.HotelSearchParams.SearchId = data.SearchId;

                    // pagination
                    $scope.MaxPage = Math.ceil(data.TotalFilteredCount / SearchHotelConfig.ResultCount);
                    $scope.show_page($scope.MaxPage);

                    console.log('loaded');
                    console.log('--------------------------------');

                    $scope.loaded = true;

                    // if error
                }).error(function () {
                    console.log('REQUEST ERROR');
                    console.log('--------------------------------');

                    $scope.loaded = true;

                });

            };

            // paging function
            $scope.show_page = function (max_page) {
                $('footer .pagination').html('');
                for (n = 1; n <= max_page; n++) {
                    $('footer .pagination').append('<li><a href="#" data-page="' + n + '">' + n + '</a></li>');
                }
                $('footer .pagination a').click(function () {
                    var page = $(this).attr('data-page');
                    $scope.loadHotelList(page);
                });
            }


        }
    ]);

    // room controller
    app.controller('RoomController', [
        '$http', '$scope', function ($http, $scope) {

            // run hotel search function on document ready
            angular.element(document).ready(function () {
                $scope.getRoomlist();
            });

            // get room list
            var room_list = this;
            room_list.rooms = [];
            $scope.RoomSearchParams = {};
            $scope.loaded = false;

            // default value
            $scope.RoomSearchParams.HotelId = $('#form-room').attr('data-hotelId');
            $scope.RoomSearchParams.StayDate = $('#form-room-checkin').val() || '2015-05-05';
            $scope.RoomSearchParams.StayLength = $('#form-room-length').val() || 1;
            $scope.RoomSearchParams.RoomCount = $('#form-room-qty').val() || 1;
            $scope.RoomSearchParams.SearchId = '';

            $scope.getRoomlist = function () {

                console.log('--------------------------------');
                console.log('Searching for Room with params:');
                console.log($scope.RoomSearchParams);

                $scope.loaded = false;

                $http.get(SearchRoomConfig.Url, {
                    params: {
                        HotelId: $scope.RoomSearchParams.HotelId,
                        StayDate: $scope.RoomSearchParams.StayDate,
                        StayLength: $scope.RoomSearchParams.StayLength,
                        RoomCount: $scope.RoomSearchParams.RoomCount,
                        SearchId: $scope.RoomSearchParams.SearchId,
                    }
                }).success(function (data) {
                    console.log(data);

                    room_list.rooms = data.PackageList;

                    console.log('LOADED');
                    console.log('--------------------------------');

                    $scope.loaded = true;

                }).error(function () {
                    console.log('REQUEST ERROR');
                    console.log('--------------------------------');
                });

            }

        }
    ]);

    // fight controller
    app.controller('FlightController', [
        '$http', '$scope', function ($http, $scope) {

            FlightSearchConfig.params = jQuery.parseJSON($('.flight-search-page').attr('data-flight-search-params'));

            // run hotel search function on document ready
            angular.element(document).ready(function () {
                $scope.getFlightList();
            });

            // flight list            
            var flightList = this;
            flightList.list = [];

            $scope.noFlight = false;

            // flight sorting
            $scope.sort = {
                label: '',
                value: ''
            }
            $scope.reverse = true;
            $scope.order = function(sort) {
                $scope.reverse = ($scope.sort.label === sort) ? !$scope.reverse : false;
                $scope.sort.label = sort;
                $scope.selectedItem = -1;
                switch(sort)
                {
                    case 'airline':
                        $scope.sort.value = 'FlightTrips[0].Airlines[0].Name';
                        break;
                    case 'departure':
                        $scope.sort.value = 'FlightTrips[0].FlightSegments[0].DepartureTime';
                        break;
                    case 'duration':
                        $scope.sort.value = 'FlightTrips[0].TotalDuration';
                        break;
                    case 'arrival':
                        $scope.sort.value = 'FlightTrips[0].FlightSegments[(FlightTrips[0].FlightSegments.length-1)].ArrivalTime';
                        break;
                    case 'price':
                        $scope.sort.value = 'TotalFare';
                        break;
                    // return flight
                    case 'departAirline':
                        $scope.sort.value = 'FlightTrips[0].Airlines[0].Name';
                        break;
                    case 'departDeparture':
                        $scope.sort.value = 'FlightTrips[0].FlightSegments[0].DepartureTime';
                        break;
                    case 'departDuration':
                        $scope.sort.value = 'FlightTrips[0].TotalDuration';
                        break;
                    case 'departArrival':
                        $scope.sort.value = 'FlightTrips[0].FlightSegments[(FlightTrips[0].FlightSegments.length-1)].ArrivalTime';
                        break;
                    case 'returnAirline':
                        $scope.sort.value = 'FlightTrips[1].Airlines[0].Name';
                        break;
                    case 'returnDeparture':
                        $scope.sort.value = 'FlightTrips[1].FlightSegments[0].DepartureTime';
                        break;
                    case 'returnDuration':
                        $scope.sort.value = 'FlightTrips[1].TotalDuration';
                        break;
                    case 'returnArrival':
                        $scope.sort.value = 'FlightTrips[1].FlightSegments[(FlightTrips[1].FlightSegments.length-1)].ArrivalTime';
                        break;
                }
            }

            // generate flight filter
            $scope.FlightSearchParams = FlightSearchConfig.params;
            $scope.loaded = false;
            $scope.validating = false;
            $scope.FlightSearchFilter = {};
            $scope.FlightSearchFilter.Airlines = [];
            $scope.FlightSearchFilter.Prices = [];
            $scope.FlightSearchFilter.StopFilter = {};
            $scope.FlightSearchFilter.StopFilter.direct = true;
            $scope.FlightSearchFilter.StopFilter.one = true;
            $scope.FlightSearchFilter.StopFilter.two = true;
            $scope.FlightSearchFilter.AirlinesList = [];
            $scope.FlightSearchFilter.AirlinesFilter = [];
            $scope.FlightSearchFilter.PriceFilter = {};

            // log on click
            $scope.consoleLog = function (dataToLog) {
                console.log(dataToLog);
            }

            // generate Airlines Filter
            $scope.checkAirline = function () {
                $scope.FlightSearchFilter.AirlinesFilter = [];
                for (var i =0; i < $scope.FlightSearchFilter.Airlines.length; i++) {
                    if ($scope.FlightSearchFilter.Airlines[i].checked == true) {
                        $scope.FlightSearchFilter.AirlinesFilter.push($scope.FlightSearchFilter.Airlines[i].Code);
                    }
                }
            };

            // filtering
            $scope.priceFilter = function (flight) {
                if (flight.TotalFare >= $scope.FlightSearchFilter.PriceFilter.currentMin && flight.TotalFare <= $scope.FlightSearchFilter.PriceFilter.currentMax) {
                    return flight;
                }
            }

            $scope.stopFilter = function (flight) {
                // return (flight.FightTrips[0].TotalTransit);

                if ($scope.FlightSearchFilter.StopFilter.direct == false && $scope.FlightSearchFilter.StopFilter.one == false && $scope.FlightSearchFilter.StopFilter.two == false) {
                    return flight;
                }

                if ($scope.FlightSearchFilter.StopFilter.direct) {
                    if ( flight.FlightTrips[0].TotalTransit == 0 ) {
                        return flight;
                    }
                }
                if ( $scope.FlightSearchFilter.StopFilter.one ) {
                    if (flight.FlightTrips[0].TotalTransit == 1) {
                        return flight;
                    }
                }
                if ($scope.FlightSearchFilter.StopFilter.two) {
                    if (flight.FlightTrips[0].TotalTransit > 1) {
                        return flight;
                    }
                }


            }

            $scope.airlineFilter = function (flight) {
                if ( $scope.FlightSearchFilter.AirlinesFilter.length > 0 ) {
                    for (var i in flight.AirlinesTag) {
                        if ($scope.FlightSearchFilter.AirlinesFilter.indexOf(flight.AirlinesTag[i]) != -1) {
                            return flight;
                        }
                    }
                    return false;
                } else {
                    return flight;
                }
            }

            // add class on click
            $scope.selectedItem = -1;
            $scope.clickedItem = function ($index) {
                $scope.selectedRules = -1;
                if ($index == $scope.selectedItem) {
                    $scope.selectedItem = -1;
                } else if ($index != $scope.selectedItem) {
                    $scope.selectedItem = $index;
                }
            }
            $scope.selectedRules = -1;

            $scope.getDateTime = function (dateTime) {
                return new Date(dateTime);
            }

            // get  flight list
            $scope.getFlightList = function () {

                console.log('--------------------------------');
                console.log('Searching for Flights with params:');
                console.log($scope.FlightSearchParams);

                $scope.loaded = false;

                $http.get(FlightSearchConfig.Url, {
                    params: {
                        request: $('.flight-search-page').attr('data-flight-search-params')
                    }
                }).success(function (data) {

                    console.log(data);

                    flightList.list = data.FlightList;
                    $scope.FlightSearchParams.SearchId = data.SearchId;
                    if (flightList.list.length == 0) {
                        $scope.noFlight = true;
                    }

                    // *****
                    // generate flight list for search filtering
                    for (var i = 0; i < flightList.list.length ; i++) {
                        flightList.list[i].AirlinesTag = [];
                        $scope.FlightSearchFilter.Prices.push(flightList.list[i].TotalFare);
                        for (var x = 0 ; x < flightList.list[i].FlightTrips[0].Airlines.length; x++) {
                            $scope.FlightSearchFilter.AirlinesList.push(flightList.list[i].FlightTrips[0].Airlines[x]);
                            flightList.list[i].AirlinesTag.push(flightList.list[i].FlightTrips[0].Airlines[x].Code);
                        }
                        if (flightList.list[i].TripType == 2) {
                            for (var x = 0 ; x < flightList.list[i].FlightTrips[1].Airlines.length; x++) {
                                $scope.FlightSearchFilter.AirlinesList.push(flightList.list[i].FlightTrips[1].Airlines[x]);
                                flightList.list[i].AirlinesTag.push(flightList.list[i].FlightTrips[1].Airlines[x].Code);
                            }
                        }
                        flightList.list[i].FareLoaded = false;
                        flightList.list[i].FareRules = '';
                    }

                    // set prices min and max value for filtering
                    function sortNumber(a, b) {
                        return a - b;
                    }
                    $scope.FlightSearchFilter.Prices.sort(sortNumber);
                    $scope.FlightSearchFilter.PriceFilter.min = Math.floor($scope.FlightSearchFilter.Prices[0]);
                    $scope.FlightSearchFilter.PriceFilter.max = Math.round($scope.FlightSearchFilter.Prices[ $scope.FlightSearchFilter.Prices.length-1 ]);

                    var dupes = {};
                    $.each($scope.FlightSearchFilter.AirlinesList, function (i, el) {
                        if (!dupes[el.Code]) {
                            dupes[el.Code] = true;
                            $scope.FlightSearchFilter.Airlines.push(el);
                        }
                    });
                    $scope.FlightSearchFilter.AirlinesList = {};

                    // *****
                    console.log('LOADED');
                    console.log('--------------------------------');

                    $scope.loaded = true;
                    FlightSearchConfig.loaded = true;

                    // show filter
                    $('.flight-search-page .flight-search-filter').show();
                    
                    // generate price slider
                    $('.price-slider').slider({
                        range: true,
                        min: $scope.FlightSearchFilter.PriceFilter.min,
                        max: $scope.FlightSearchFilter.PriceFilter.max,
                        values: [$scope.FlightSearchFilter.PriceFilter.min, $scope.FlightSearchFilter.PriceFilter.max],
                        create: function(event, ui) {
                            $('.price-slider-min').val( $scope.FlightSearchFilter.PriceFilter.min );
                            $('.price-slider-min').trigger('input');
                            $('.price-slider-max').val( $scope.FlightSearchFilter.PriceFilter.max );
                            $('.price-slider-max').trigger('input');
                        },
                        slide: function(event, ui) {
                            $('.price-slider-min').val(ui.values[0]);
                            $('.price-slider-min').trigger('input');
                            $('.price-slider-max').val(ui.values[1]);
                            $('.price-slider-max').trigger('input');
                        }
                    });

                }).error(function () {

                    $scope.noFlight = true;
                    $scope.loaded = true;

                    console.log('REQUEST ERROR');
                    console.log('--------------------------------');
                });

            }

            // revalidate flight itinerary
            $scope.revalidate = function (indexNo) {
                var searchId = $scope.FlightSearchParams.SearchId;

                $scope.validating = true;

                console.log('validating :');

                if (RevalidateConfig.working == false) {
                    RevalidateConfig.working = true;

                    $http.get(RevalidateConfig.Url, {
                        params: {
                            SearchId: searchId,
                            ItinIndex: indexNo
                        }
                    }).success(function (returnData) {
                        RevalidateConfig.working = false;

                        console.log(returnData);

                        RevalidateConfig.value = returnData;

                        if (returnData.IsValid == true) {
                            window.location.assign(location.origin + '/id/flight/Checkout?token=' + returnData.HashKey);
                        } else if (returnData.IsValid == false && returnData.IsOtherFareAvailable == true) {
                            flightList.list[indexNo].TotalFare = returnData.NewFare;
                            var userConfirmation = confirm("The price for the flight has been updated. The new price is : " + returnData.NewFare + ". Do you want to continue ?");
                            if (userConfirmation) {
                                $scope.validating = false;
                                window.location.assign(location.origin + '/id/flight/Checkout?token=' + returnData.HashKey);
                            } else {
                                $scope.validating = false;
                            }
                        } else if (returnData.IsValid == false && returnData.IsOtherFareAvailable == false) {
                            $scope.validating = false;
                            alert("Sorry, the flight is no longer valid. Please check another flight.");
                            flightList.list[indexNo].hidden = true;
                        }

                    }).error(function (data) {
                        console.log(data)
                    });
                } else {
                    console.log('Sistem busy, please wait');
                }

            }

            // get flight rules
            $scope.getRules = function (sequenceNo) {
                var searchId = $scope.FlightSearchParams.SearchId;

                // switch flight rules
                $scope.selectedItem = -1;
                if ($scope.selectedRules == sequenceNo) {
                    $scope.selectedRules = -1;
                } else {
                    $scope.selectedRules = sequenceNo;
                }
                
                // get flight rules
                if ( flightList.list[sequenceNo].FareLoaded == false ) {

                    console.log('getting rules for '+sequenceNo+'... ');

                    $http.get(GetRulesConfig.Url, {
                        params: {
                            SearchId: searchId,
                            ItinIndex: sequenceNo
                        }
                    }).success(function(returnData) {
                        GetRulesConfig.working = false;

                        GetRulesConfig.value = returnData;

                        console.log(returnData);

                        flightList.list[sequenceNo].FareLoaded = true;
                        flightList.list[sequenceNo].FareRules = returnData;

                        console.log('...Fare rules '+sequenceNo+' loaded');

                    }).error(function(returnData) {
                        console.log(returnData)
                    });

                }

                
            }

        }
    ]);

})();


