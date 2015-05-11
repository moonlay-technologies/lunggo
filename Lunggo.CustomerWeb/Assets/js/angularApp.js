// ************************
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

            // get room list
            var flightList = this;
            flightList.list = [];
            $scope.FlightSearchParams = {};
            $scope.loaded = false;
            $scope.FlightSearchFilter = {};
            $scope.FlightSearchFilter.Airlines = [];
            $scope.FlightSearchFilter.Prices = [];
            $scope.FlightSearchFilter.Stops = [];
            $scope.FlightSearchFilter.AirlinesList = [];

            // add class on click
            $scope.selectedItem = -1;
            $scope.clickedItem = function ($index) {
                if ($index == $scope.selectedItem) {
                    $scope.selectedItem = -1;
                } else if ($index != $scope.selectedItem) {
                    $scope.selectedItem = $index;
                }

            }

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

                    // generate flight list for search filtering
                    for (var i = 0; i < flightList.list.length ; i++) {
                        $scope.FlightSearchFilter.Prices.push(flightList.list[i].TotalFare);
                        $scope.FlightSearchFilter.Stops.push(flightList.list[i].FlightTrips[0].TotalTransit);
                        $scope.FlightSearchFilter.AirlinesList.push(flightList.list[i].FlightTrips[0].Airlines[0]);
                        if (flightList.list[i].TripType == 2) {
                            $scope.FlightSearchFilter.Stops.push(flightList.list[i].FlightTrips[1].TotalTransit);
                            $scope.FlightSearchFilter.AirlinesList.push(flightList.list[i].FlightTrips[1].Airlines[0]);
                        }
                    }

                    // generate filter data

                    $scope.FlightSearchFilter.Prices = $scope.FlightSearchFilter.Prices.sort();
                    $scope.FlightSearchFilter.Stops = $scope.FlightSearchFilter.Stops.sort();
                    $scope.FlightSearchFilter.Stops = $scope.FlightSearchFilter.Stops[$scope.FlightSearchFilter.Stops.length - 1];

                    var dupes = {};
                    $.each($scope.FlightSearchFilter.AirlinesList, function (i, el) {
                        if (!dupes[el.Code]) {
                            dupes[el.Code] = true;
                            $scope.FlightSearchFilter.Airlines.push(el);
                        }
                    });
                    
                    console.log('LOADED');
                    console.log('--------------------------------');

                    console.log($scope.FlightSearchFilter);

                    $scope.loaded = true;

                }).error(function () {
                    console.log('REQUEST ERROR');
                    console.log('--------------------------------');
                });

            }

            // revalidate flight itinerary
            $scope.revalidate = function (indexNo) {
                var searchId = $scope.FlightSearchParams.SearchId;

                loading_overlay('show');

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
                            var userConfirmation = confirm("The price for the flight has been updated. The new price is : " + returnData.NewFare + ". Do you want to continue ?");
                            if (userConfirmation) {
                                loading_overlay('hide');
                                window.location.assign(location.origin + '/id/flight/Checkout?token=' + returnData.HashKey);
                            }
                        } else if (returnData.IsValid == false && returnData.IsOtherFareAvailable == false) {
                            loading_overlay('hide');
                            alert("Sorry, the flight is no longer valid. Please check another flight.");
                        }

                    }).error(function (data) {
                        console.log(data)
                    });
                } else {
                    console.log('Sistem busy, please wait');
                }

            }


        }
    ]);


})();


