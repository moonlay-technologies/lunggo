// ************************
// Angular app
(function () {

    var app = angular.module('travorama', ['ngRoute']);

    // ******************************
    // hotel controller
    app.controller('hotelController', [
        '$http', '$scope', function ($http, $scope) {

            // ********************
            // genereal variables
            $scope.HotelSearchParams = {};
            $scope.HotelSearchResult = {};
            $scope.PageConfig = {
                Loaded: false,
                busy: false,
                TotalPage: 1,
                CurrentPage: 1
            };

            // ********************
            // get hotel list
            $scope.GetHotel = function() {
                
            }// GetHotel()

            // ********************
            // show hotel
            $scope.ShowHotelList = function(pageNumber) {
                
            }// ShowHotelList();


        }
    ]);// hotel controller

    // ******************************
    // room controller
    app.controller('roomController', [
        '$http', '$scope', function ($http, $scope) {

            

        }
    ]);// room controller

    // ******************************
    // fight controller
    app.controller('flightController', [
        '$http', '$scope', function ($http, $scope) {
            
            // ********************
            // on document ready
            angular.element(document).ready(function () {
                console.log('document ready');
                $scope.getFlight();
            });

            // ********************
            // general variables

            $scope.dummyParams = { "TripType": "Return", "TripInfos": [{ "OriginAirport": "CGK", "DestinationAirport": "HND", "DepartureDate": "2015-10-25T00:00:00" }, { "OriginAirport": "HND", "DestinationAirport": "CGK", "DepartureDate": "2015-10-28T00:00:00" }], "AdultCount": "1", "ChildCount": "0", "InfantCount": "0", "CabinClass": "Economy", "Currency": "IDR" };

            $scope.flightSearchParams = $scope.dummyParams;
            $scope.flightSearchResult = {};
            $scope.loaded = false;
            $scope.busy = false;
            $scope.flightDetailActive = false;
            $scope.flightCurrent = -1;
            $scope.flightCurrentDetail = {};
            $scope.flightSearchFilter = {};
            $scope.flightSearchFilter.airlines = [];
            $scope.flightSearchFilter.prices = [];
            $scope.flightSearchFilter.airlineList = [];

            $scope.getCtrlScope = function() {
                return $scope
            }

            // ********************
            // return cabin class name
            $scope.cabinClassName = function(cabin) {
                switch (cabin) {
                    case 'y':
                        return 'Economy';
                        break;
                    case 'c':
                        return 'Business';
                        break;
                    case 'f':
                        return 'First Class';
                        break;
                }
            }

            // ********************
            // update current active flight
            $scope.updateActiveFlight = function() {
                $scope.flightCurrentDetail = $scope.flightSearchResult.FlightList[$scope.flightCurrent];
                console.log($scope.flightCurrentDetail);
                $('body').addClass('modal-active');
            }
            $scope.hideDetailActive = function () {
                $('body').removeClass('modal-active');
            }

            // ********************
            // get date time
            $scope.getDateTime = function (dateTime) {
                return new Date(dateTime);
            }


            // ********************
            // get flight list
            $scope.getFlight = function() {

                // get the flight function if system is not busy
                if ($scope.busy == false) {
                    console.log('Fetching flight list with param :');
                    console.log($scope.dummyParams);
                    $scope.busy = true;

                    // AJAX
                    $http.get(FlightSearchConfig.Url, {
                        params: {
                            request: $scope.dummyParams
                        }
                    }).success(function (data) {

                        $scope.flightSearchResult = data;

                        FlightSearchConfig.SearchId = data.SearchId;

                        for ( var i=0; i< $scope.flightSearchResult.FlightList.length; i++ ) {
                            $scope.flightSearchResult.FlightList[i].FlightIndex = i;
                        }

                        if (data.TotalFlightCount == 0) {
                            $scope.noFlight = true;
                        }

                        // *****
                        // generate airline list for search filtering
                        for (var i = 0; i < $scope.flightSearchResult.FlightList.length ; i++) {
                            $scope.flightSearchResult.FlightList[i].AirlinesTag = []; // make airline tag
                            $scope.flightSearchFilter.prices.push($scope.flightSearchResult.FlightList[i].TotalFare); // push price
                            for (var x = 0 ; x < $scope.flightSearchResult.FlightList[i].FlightTrips[0].Airlines.length; x++) {
                                $scope.flightSearchFilter.airlines.push($scope.flightSearchResult.FlightList[i].FlightTrips[0].Airlines[x]);
                                $scope.flightSearchResult.FlightList[i].AirlinesTag.push($scope.flightSearchResult.FlightList[i].FlightTrips[0].Airlines[x].Code);
                            }
                            if ($scope.flightSearchResult.FlightList[i].TripType == 2) {
                                for (var x = 0 ; x < $scope.flightSearchResult.FlightList[i].FlightTrips[1].Airlines.length; x++) {
                                    $scope.flightSearchFilter.airlines.push($scope.flightSearchResult.FlightList[i].FlightTrips[1].Airlines[x]);
                                    $scope.flightSearchResult.FlightList[i].AirlinesTag.push($scope.flightSearchResult.FlightList[i].FlightTrips[1].Airlines[x].Code);
                                }
                            }
                            $scope.flightSearchResult.FlightList[i].FareLoaded = false;
                            $scope.flightSearchResult.FlightList.FareRules = '';
                        }
                        var dupes = {};
                        $.each($scope.flightSearchFilter.airlines, function (i, el) {
                            if (!dupes[el.Code]) {
                                dupes[el.Code] = true;
                                $scope.flightSearchFilter.airlineList.push(el);
                            }
                        });
                        $scope.flightSearchFilter.airlines = {};

                        // *****
                        // sort price
                        function sortNumber(a, b) {
                            return a - b;
                        }
                        $scope.flightSearchFilter.prices.sort(sortNumber);

                        // *****
                        console.log($scope.flightSearchResult);
                        console.log('Flight Fetched');
                        console.log($scope.flightSearchFilter);

                        $scope.loaded = true;
                        $scope.busy = false;

                        // show filter and sorting option
                        $('.flight-search-param, .flight-search-filter').show();

                    }).error(function () {

                        $scope.noFlight = true;
                        $scope.loaded = true;
                        $scope.busy = false;

                        console.log('REQUEST ERROR');
                    });

                } else {
                    console.log('System Busy, please wait.');
                }

            }// getFlight()

            // ********************
            // pagination
            $scope.showPage = function(pageNumber) {
                
            }// showPage()

            // ********************
            // flight sorting
            $scope.sortFlight = function() {
                
            }// sortFlight()

            // ********************
            // get flight detail
            $scope.getFlightDetail = function(flightNo) {
                console.log('getting flight detail for : '+ flightNo);

            }// getFlightdetail()

            // ********************
            // validate flight
            $scope.validateFlight = function(flightNo) {
                console.log('validating flight no : '+ flightNo);

            }// validateFlight()

        }
    ]);// flight controller

})();


