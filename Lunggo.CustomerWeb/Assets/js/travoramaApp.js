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

            $scope.dummyParamsReturn = { "TripType": "Return", "TripInfos": [{ "OriginAirport": "CGK", "DestinationAirport": "HND", "DepartureDate": "2015-10-25T00:00:00" }, { "OriginAirport": "HND", "DestinationAirport": "CGK", "DepartureDate": "2015-10-28T00:00:00" }], "AdultCount": "1", "ChildCount": "0", "InfantCount": "0", "CabinClass": "Economy", "Currency": "IDR" };
            $scope.dummyParamsOneway = { "TripType": "OneWay", "TripInfos": [{ "OriginAirport": "CGK", "DestinationAirport": "HND", "DepartureDate": "2015-10-25T00:00:00" }], "AdultCount": "1", "ChildCount": "0", "InfantCount": "0", "CabinClass": "Economy", "Currency": "IDR" };

            $scope.flightSearchParams = $scope.dummyParamsReturn;
            $scope.flightSearchResult = {};
            $scope.loaded = false;
            $scope.busy = false;
            $scope.flightDetailActive = false;
            $scope.flightCurrent = -1;
            $scope.flightCurrentDetail = {};
                // flight sorting
                $scope.flightSort = {
                    label: '',
                    value: ''
                };
                // flight filtering variables
                $scope.flightSearchFilter = {};
                $scope.flightSearchFilter.airlines = [];
                $scope.flightSearchFilter.airlineList = [];
                $scope.flightSearchFilter.prices = [];
                // stop filter
                $scope.flightSearchFilter.stop = {};
                $scope.flightSearchFilter.stop.onward = [true, true, true];
                $scope.flightSearchFilter.stop.return = [true, true, true];
                // price filter
                $scope.flightSearchFilter.priceFilter = [-1, -1];
                $scope.flightSearchFilter.priceFilterDisplay = [-1, -1];
                // flight time
                $scope.flightSearchFilter.onwardDeparture = [0, 24];
                $scope.flightSearchFilter.onwardDepartureDisplay = [0,24];
                $scope.flightSearchFilter.onwardArrival = [0, 24];
                $scope.flightSearchFilter.onwardArrivalDisplay = [0, 24];
                $scope.flightSearchFilter.returnDeparture = [0, 24];
                $scope.flightSearchFilter.returnDepartureDisplay = [0, 24];
                $scope.flightSearchFilter.returnArrival = [0, 24];
                $scope.flightSearchFilter.returnArrivalDisplay = [0, 24];

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
            // get date time function
            $scope.getDateTime = function (dateTime) {
                return new Date(dateTime);
            }

            // ********************
            // Filtering functions
            // price filter
            $scope.priceFilter = function(flight) {
                return flight;
            }

            // stop filter
            $scope.stopFilter = function (flight) {
                // if flight oneway
                if ($scope.flightSearchParams.TripType == 'OneWay') {
                    if ($scope.flightSearchFilter.stop.onward[0] == true && $scope.flightSearchFilter.stop.onward[1] == true && $scope.flightSearchFilter.stop.onward[2] == true) {
                        return flight;
                    } else {

                        if ($scope.flightSearchFilter.stop.onward[0] == true && $scope.flightSearchFilter.stop.onward[1] == false && $scope.flightSearchFilter.stop.onward[2] == false) {
                            if (flight.FlightTrips[0].TotalTransit < 1) {
                                return flight;
                            }
                        }
                        if ($scope.flightSearchFilter.stop.onward[0] == true && $scope.flightSearchFilter.stop.onward[1] == true && $scope.flightSearchFilter.stop.onward[2] == false) {
                            if (flight.FlightTrips[0].TotalTransit < 2 ) {
                                return flight;
                            }
                        }
                        if ($scope.flightSearchFilter.stop.onward[0] == false && $scope.flightSearchFilter.stop.onward[1] == true && $scope.flightSearchFilter.stop.onward[2] == false) {
                            if (flight.FlightTrips[0].TotalTransit == 1) {
                                return flight;
                            }
                        }
                        if ($scope.flightSearchFilter.stop.onward[0] == false && $scope.flightSearchFilter.stop.onward[1] == true && $scope.flightSearchFilter.stop.onward[2] == true) {
                            if (flight.FlightTrips[0].TotalTransit > 0) {
                                return flight;
                            }
                        }
                        if ($scope.flightSearchFilter.stop.onward[0] == false && $scope.flightSearchFilter.stop.onward[1] == false && $scope.flightSearchFilter.stop.onward[2] == true) {
                            if (flight.FlightTrips[0].TotalTransit > 1) {
                                return flight;
                            }
                        }
                        if ($scope.flightSearchFilter.stop.onward[0] == true && $scope.flightSearchFilter.stop.onward[1] == false && $scope.flightSearchFilter.stop.onward[2] == true) {
                            if (flight.FlightTrips[0].TotalTransit < 1 || flight.FlightTrips[0].TotalTransit > 1) {
                                return flight;
                            }
                        }
                        if ($scope.flightSearchFilter.stop.onward[0] == false && $scope.flightSearchFilter.stop.onward[1] == false && $scope.flightSearchFilter.stop.onward[2] == false) {
                            return null;
                        }

                    }
                // if flight return
                } else {
                    return flight;
                }
            }
            
            // airline filter
            $scope.airlineFilter = function(flight) {
                return flight;
            }

            // time filter
            $scope.timeFilter = function(flight) {
                return flight;
            }
            $scope.getHour = function (datetime) {
                datetime = datetime.substr(11, 2);
                return parseInt(datetime);
            }


            // ********************
            // get flight list
            $scope.getFlight = function() {

                // get the flight function if system is not busy
                if ($scope.busy == false) {
                    console.log('Fetching flight list with param :');
                    console.log($scope.flightSearchParams);
                    $scope.busy = true;

                    // AJAX
                    $http.get(FlightSearchConfig.Url, {
                        params: {
                            request: $scope.flightSearchParams
                        }
                    }).success(function (data) {

                        $scope.flightSearchResult = data;
                        FlightSearchConfig.SearchId = data.SearchId;
                        // if no flight
                        if (data.TotalFlightCount == 0) {
                            $scope.noFlight = true;
                        }
                        // generate flight index for all flight
                        for (var i = 0; i < $scope.flightSearchResult.FlightList.length; i++) {
                            $scope.flightSearchResult.FlightList[i].FlightIndex = i;
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
                        // sort flight prices
                        function sortNumber(a, b) {
                            return a - b;
                        }
                        $scope.flightSearchFilter.prices.sort(sortNumber);
                        $scope.flightSearchFilter.priceFilter[0] = $scope.flightSearchFilter.prices[0];
                        $scope.flightSearchFilter.priceFilter[1] = $scope.flightSearchFilter.prices[ $scope.flightSearchFilter.prices.length - 1 ];

                        $scope.flightSearchFilter.priceFilterDisplay[0] = $scope.flightSearchFilter.prices[0];
                        $scope.flightSearchFilter.priceFilterDisplay[1] = $scope.flightSearchFilter.prices[$scope.flightSearchFilter.prices.length - 1];

                        // *****
                        console.log($scope.flightSearchResult);
                        console.log('Flight Received');
                        console.log($scope.flightSearchFilter);

                        $scope.loaded = true;
                        $scope.busy = false;

                        // show filter and sorting option
                        $('.flight-search-param, .flight-search-filter').show();

                        // *****
                        // filter function that using slider
                        $('.onward-departure-slider').slider({
                            range: true,
                            min: 0, max: 24, step: 1, values: [0, 24],
                            create: function (event, ui) {
                                $('.onward-departure-min').val(0);
                                $('.onward-departure-min').trigger('input');
                                $('.onward-departure-max').val(24);
                                $('.onward-departure-max').trigger('input');
                                $('.onward-departure-min-display').html(0);
                                $('.onward-departure-max-display').html(24);
                            },
                            slide: function(event, ui) {
                                $('.onward-departure-min-display').html(ui.values[0]);
                                $('.onward-departure-max-display').html(ui.values[1]);
                            },
                            stop: function (event, ui) {
                                $('.onward-departure-min').val(ui.values[0]);
                                $('.onward-departure-min').trigger('input');
                                $('.onward-departure-max').val(ui.values[1]);
                                $('.onward-departure-max').trigger('input');
                            }
                        });
                        $('.onward-arrival-slider').slider({
                            range: true,
                            min: 0, max: 24, step: 1, values: [0, 24],
                            create: function (event, ui) {
                                $('.onward-arrival-min').val(0);
                                $('.onward-arrival-min').trigger('input');
                                $('.onward-arrival-max').val(24);
                                $('.onward-arrival-max').trigger('input');
                                $('.onward-arrival-min-display').html(0);
                                $('.onward-arrival-max-display').html(24);
                            },
                            slide: function (event, ui) {
                                $('.onward-arrival-min-display').html(ui.values[0]);
                                $('.onward-arrival-max-display').html(ui.values[1]);
                            },
                            stop: function (event, ui) {
                                $('.onward-arrival-min').val(ui.values[0]);
                                $('.onward-arrival-min').trigger('input');
                                $('.onward-arrival-max').val(ui.values[1]);
                                $('.onward-arrival-max').trigger('input');
                            }
                        });
                        $('.return-departure-slider').slider({
                            range: true,
                            min: 0, max: 24, step: 1, values: [0, 24],
                            create: function (event, ui) {
                                $('.return-departure-min').val(0);
                                $('.return-departure-min').trigger('input');
                                $('.return-departure-max').val(24);
                                $('.return-departure-max').trigger('input');
                                $('.return-departure-min-display').html(0);
                                $('.return-departure-max-display').html(24);
                            },
                            slide: function (event, ui) {
                                $('.return-departure-min-display').html(ui.values[0]);
                                $('.return-departure-max-display').html(ui.values[1]);
                            },
                            stop: function (event, ui) {
                                $('.return-departure-min').val(ui.values[0]);
                                $('.return-departure-min').trigger('input');
                                $('.return-departure-max').val(ui.values[1]);
                                $('.return-departure-max').trigger('input');
                            }
                        });
                        $('.return-arrival-slider').slider({
                            range: true,
                            min: 0, max: 24, step: 1, values: [0, 24],
                            create: function (event, ui) {
                                $('.return-arrival-min').val(0);
                                $('.return-arrival-min').trigger('input');
                                $('.return-arrival-max').val(24);
                                $('.return-arrival-max').trigger('input');
                                $('.return-arrival-min-display').html(0);
                                $('.return-arrival-max-display').html(24);
                            },
                            slide: function (event, ui) {
                                $('.return-arrival-min-display').html(ui.values[0]);
                                $('.return-arrival-max-display').html(ui.values[1]);
                            },
                            stop: function (event, ui) {
                                $('.return-arrival-min').val(ui.values[0]);
                                $('.return-arrival-min').trigger('input');
                                $('.return-arrival-max').val(ui.values[1]);
                                $('.return-arrival-max').trigger('input');
                            }
                        });


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


