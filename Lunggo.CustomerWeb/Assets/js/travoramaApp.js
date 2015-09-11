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
        '$http', '$scope', '$interval', function ($http, $scope, $interval) {
            
            // ********************
            // on document ready
            angular.element(document).ready(function () {
                console.log('document ready');
                $scope.getFlight();
            });

            // ********************
            // general variables

            $scope.flightSearchParams = jQuery.parseJSON($('.flight-search-page').attr('data-flight-search-params'));
            $scope.flightSearchResult = {};
            $scope.loaded = false;
            $scope.busy = false;
            $scope.flightDetailActive = false;
            $scope.flightCurrent = -1;
            $scope.flightCurrentDetail = {};
            $scope.sort = {
                label: 'price',
                value: 'TotalFare'
            }
            $scope.reverse = false;
            $scope.order = function (sort, reverseState) {
                $scope.reverse = reverseState || false;
                $scope.sort.label = sort;
                $scope.selectedItem = -1;
                $scope.selectedRules = -1;
                switch (sort) {
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
                // flight filtering variables
                $scope.flightSearchFilter = {};
                $scope.flightSearchFilter.airlines = [];
                $scope.flightSearchFilter.airlinesList = [];
                $scope.flightSearchFilter.airlinesFilter = [];
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
                return $scope;
            }


            // ********************
            // search expiry time
            $scope.pageExpired = false;
            $scope.pageExpiryDate = '';
            $scope.pageExpiryStarted = false;
            $scope.pageExpiryStart = function(expiryDate) {
                $scope.pageExpiryDate = new Date(expiryDate);
                if ($scope.pageExpiryStarted == false) {
                    $scope.pageExpiryStarted = true;
                    $interval(function() {
                        if (new Date() >= $scope.pageExpiryDate) {
                            $scope.pageExpired = true;
                        }
                    }, 1000);
                }
            }
            $scope.refreshPage = function () {
                console.log('JEMPING REFRESH');
                location.reload();
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
                if ( flight.TotalFare >= $scope.flightSearchFilter.priceFilter[0] && flight.TotalFare <= $scope.flightSearchFilter.priceFilter[1] ) {
                    return flight;
                }
            }

            // stop filter
            $scope.stopFilterOnward = function (flight) {
                if ($scope.flightSearchFilter.stop.onward[0] == true && $scope.flightSearchFilter.stop.onward[1] == true && $scope.flightSearchFilter.stop.onward[2] == true) {
                    return flight;
                } else {
                    if ($scope.flightSearchFilter.stop.onward[0] == true && $scope.flightSearchFilter.stop.onward[1] == false && $scope.flightSearchFilter.stop.onward[2] == false) {
                        if (flight.FlightTrips[0].TotalTransit < 1) {
                            return flight;
                        }
                    }
                    if ($scope.flightSearchFilter.stop.onward[0] == true && $scope.flightSearchFilter.stop.onward[1] == true && $scope.flightSearchFilter.stop.onward[2] == false) {
                        if (flight.FlightTrips[0].TotalTransit < 2) {
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
            }
            // stop filter on return flight
            $scope.stopFilterReturn = function(flight) {
                if ($scope.flightSearchParams.TripType == 'Return') {
                    if ($scope.flightSearchFilter.stop.return[0] == true && $scope.flightSearchFilter.stop.return[1] == true && $scope.flightSearchFilter.stop.return[2] == true) {
                        return flight;
                    } else {
                        if ($scope.flightSearchFilter.stop.return[0] == true && $scope.flightSearchFilter.stop.return[1] == false && $scope.flightSearchFilter.stop.return[2] == false) {
                            if (flight.FlightTrips[1].TotalTransit < 1) {
                                return flight;
                            }
                        }
                        if ($scope.flightSearchFilter.stop.return[0] == true && $scope.flightSearchFilter.stop.return[1] == true && $scope.flightSearchFilter.stop.return[2] == false) {
                            if (flight.FlightTrips[1].TotalTransit < 2) {
                                return flight;
                            }
                        }
                        if ($scope.flightSearchFilter.stop.return[0] == false && $scope.flightSearchFilter.stop.return[1] == true && $scope.flightSearchFilter.stop.return[2] == false) {
                            if (flight.FlightTrips[1].TotalTransit == 1) {
                                return flight;
                            }
                        }
                        if ($scope.flightSearchFilter.stop.return[0] == false && $scope.flightSearchFilter.stop.return[1] == true && $scope.flightSearchFilter.stop.return[2] == true) {
                            if (flight.FlightTrips[1].TotalTransit > 0) {
                                return flight;
                            }
                        }
                        if ($scope.flightSearchFilter.stop.return[0] == false && $scope.flightSearchFilter.stop.return[1] == false && $scope.flightSearchFilter.stop.return[2] == true) {
                            if (flight.FlightTrips[1].TotalTransit > 1) {
                                return flight;
                            }
                        }
                        if ($scope.flightSearchFilter.stop.return[0] == true && $scope.flightSearchFilter.stop.return[1] == false && $scope.flightSearchFilter.stop.return[2] == true) {
                            if (flight.FlightTrips[1].TotalTransit < 1 || flight.FlightTrips[0].TotalTransit > 1) {
                                return flight;
                            }
                        }
                        if ($scope.flightSearchFilter.stop.return[0] == false && $scope.flightSearchFilter.stop.return[1] == false && $scope.flightSearchFilter.stop.return[2] == false) {
                            return flight;
                        }
                    }
                } else {
                    return flight;
                }
            }

            // generate airline filter
            $scope.checkAirline = function () {
                console.log($scope.flightSearchFilter);
                $scope.flightSearchFilter.airlinesFilter = [];
                for (var i = 0; i < $scope.flightSearchFilter.airlines.length; i++) {
                    if ($scope.flightSearchFilter.airlines[i].checked == true) {
                        $scope.flightSearchFilter.airlinesFilter.push($scope.flightSearchFilter.airlines[i].Code);
                    }
                }
            }

            // airline filter
            $scope.airlineFilter = function(flight) {
                if ($scope.flightSearchFilter.airlinesFilter.length > 0) {
                    for (var i in flight.AirlinesTag) {
                        if ($scope.flightSearchFilter.airlinesFilter.indexOf(flight.AirlinesTag[i]) != -1) {
                            return flight;
                        }
                    }
                    return false;
                } else {
                    return flight;
                }
            }

            // time filter
            $scope.timeFilterOnward = function (flight) {
                if ($scope.getHour(flight.FlightTrips[0].FlightSegments[0].DepartureTime) >= $scope.flightSearchFilter.onwardDeparture[0]
                    && $scope.getHour(flight.FlightTrips[0].FlightSegments[0].DepartureTime) <= $scope.flightSearchFilter.onwardDeparture[1]
                    && $scope.getHour(flight.FlightTrips[0].FlightSegments[flight.FlightTrips[0].FlightSegments.length - 1].ArrivalTime) >= $scope.flightSearchFilter.onwardArrival[0]
                    && $scope.getHour(flight.FlightTrips[0].FlightSegments[flight.FlightTrips[0].FlightSegments.length - 1].ArrivalTime) <= $scope.flightSearchFilter.onwardArrival[1]
                    )
                {
                    return flight;
                }
            }
            $scope.timeFilterReturn = function (flight) {
                if ($scope.flightSearchParams.TripType == 'Return') {
                    if ($scope.getHour(flight.FlightTrips[1].FlightSegments[0].DepartureTime) >= $scope.flightSearchFilter.returnDeparture[0]
                        && $scope.getHour(flight.FlightTrips[1].FlightSegments[0].DepartureTime) <= $scope.flightSearchFilter.returnDeparture[1]
                        && $scope.getHour(flight.FlightTrips[1].FlightSegments[flight.FlightTrips[1].FlightSegments.length - 1].ArrivalTime) >= $scope.flightSearchFilter.returnArrival[0]
                        && $scope.getHour(flight.FlightTrips[1].FlightSegments[flight.FlightTrips[1].FlightSegments.length - 1].ArrivalTime) <= $scope.flightSearchFilter.returnArrival[1]
                        ) {
                            return flight;
                        }
                } else {
                    return flight;
                }
            }
            $scope.getHour = function (datetime) {
                datetime = datetime.substr(11, 2);
                return parseInt(datetime);
            }

            // ********************
            // revalidate flight itinerary
            $scope.revalidate = function (resultNo) {
                var searchId = $scope.flightSearchParams.SearchId;

                $scope.validating = true;
                loadingOverlay(true);
                console.log('validating :');

                if (RevalidateConfig.working == false) {
                    RevalidateConfig.working = true;

                    $http.get(RevalidateConfig.Url, {
                        params: {
                            SearchId: searchId,
                            ItinIndex: resultNo
                        }
                    }).success(function (returnData) {
                        RevalidateConfig.working = false;

                        console.log(returnData);

                        RevalidateConfig.value = returnData;

                        if (returnData.IsValid == true) {
                            window.location.assign(location.origin + '/id/flight/Checkout?token=' + returnData.HashKey);
                        } else if (returnData.IsValid == false && returnData.IsOtherFareAvailable == true) {
                            $scope.flightSearchResult.FlightList[resultNo].TotalFare = returnData.NewFare;
                            var userConfirmation = confirm("The price for the flight has been updated. The new price is : " + returnData.NewFare + ". Do you want to continue ?");
                            if (userConfirmation) {
                                $scope.validating = false;
                                loadingOverlay(false);
                                window.location.assign(location.origin + '/id/flight/Checkout?token=' + returnData.HashKey);
                            } else {
                                $scope.validating = false;
                                loadingOverlay(false);
                            }
                        } else if (returnData.IsValid == false && returnData.IsOtherFareAvailable == false) {
                            $scope.validating = false;
                            loadingOverlay(false);
                            alert("Sorry, the flight is no longer valid. Please check another flight.");
                            $scope.flightDetailActive = false;
                            $scope.flightSearchResult.FlightList[resultNo].hidden = true;
                        }

                    }).error(function (data) {
                        console.log(data);
                    });
                } else {
                    console.log('Sistem busy, please wait');
                }

            }

            // ********************
            // get flight list
            $scope.getFlight = function() {

                // get the flight function if system is not busy
                if ($scope.busy == false) {
                    console.log('Fetching flight list with param :');
                    console.log($scope.flightSearchParams);
                    $scope.busy = true;
                    loadingOverlay(true);
                    // AJAX
                    $http.get(FlightSearchConfig.Url, {
                        params: {
                            request: $scope.flightSearchParams
                        }
                    }).success(function (data) {
                        $scope.pageExpiryStart(data.ExpiryTime);
                        loadingOverlay(false);
                        $scope.flightSearchResult = data;
                        FlightSearchConfig.SearchId = data.SearchId;
                        $scope.flightSearchParams.SearchId = data.SearchId;
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
                                $scope.flightSearchFilter.airlinesList.push($scope.flightSearchResult.FlightList[i].FlightTrips[0].Airlines[x]);
                                $scope.flightSearchResult.FlightList[i].AirlinesTag.push($scope.flightSearchResult.FlightList[i].FlightTrips[0].Airlines[x].Code);
                            }
                            if ($scope.flightSearchResult.FlightList[i].TripType == 2) {
                                for (var x = 0 ; x < $scope.flightSearchResult.FlightList[i].FlightTrips[1].Airlines.length; x++) {
                                    $scope.flightSearchFilter.airlinesList.push($scope.flightSearchResult.FlightList[i].FlightTrips[1].Airlines[x]);
                                    $scope.flightSearchResult.FlightList[i].AirlinesTag.push($scope.flightSearchResult.FlightList[i].FlightTrips[1].Airlines[x].Code);
                                }
                            }
                            $scope.flightSearchResult.FlightList[i].FareLoaded = false;
                            $scope.flightSearchResult.FlightList.FareRules = '';
                        }
                        var dupes = {};
                        $.each($scope.flightSearchFilter.airlinesList, function (i, el) {
                            if (!dupes[el.Code]) {
                                dupes[el.Code] = true;
                                $scope.flightSearchFilter.airlines.push(el);
                            }
                        });
                        $scope.flightSearchFilter.airlinesList = {};

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
                        $('.price-slider').slider({
                            range: true,
                            min: $scope.flightSearchFilter.priceFilter[0],
                            max: $scope.flightSearchFilter.priceFilter[1],
                            step: 100000,
                            values: [$scope.flightSearchFilter.priceFilter[0], $scope.flightSearchFilter.priceFilter[1]],
                            create: function (event, ui) {
                                $('.price-filter-min').val($scope.flightSearchFilter.priceFilter[0]);
                                $('.price-filter-min').trigger('input');
                                $('.price-filter-max').val($scope.flightSearchFilter.priceFilter[1]);
                                $('.price-filter-max').trigger('input');
                            },
                            slide: function (event, ui) {
                                $scope.flightSearchFilter.priceFilterDisplay[0] = ui.values[0];
                                $scope.flightSearchFilter.priceFilterDisplay[1] = ui.values[1];
                            },
                            stop: function (event, ui) {
                                $('.price-filter-min').val(ui.values[0]);
                                $('.price-filter-min').trigger('input');
                                $('.price-filter-max').val(ui.values[1]);
                                $('.price-filter-max').trigger('input');
                            }
                        });
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
                        loadingOverlay(false);
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
            // validate flight
            $scope.validateFlight = function(flightNo) {
                console.log('validating flight no : '+ flightNo);

            }// validateFlight()

        }
    ]);// flight controller

    // ******************************
    // checkout controller
    app.controller('checkoutController', [
        '$scope', function ($scope) {

            $scope.flightPassengers = [];
            $scope.passengersValid = false;
            $scope.passengersNumber = -1;
            $scope.validTotal = -1;


            // passengers validation
            $scope.passengersValidation = function() {


                $scope.validTotal = 0;

                for (var i = 0; i < $scope.passengersNumber; i++) {
                    if ($scope.flightPassengers[i].firstname) {
                        $scope.flightPassengers[i].valid = true;
                        $scope.validTotal++;
                    } else {
                        $scope.flightPassengers[i].valid = false;
                    }
                }

            }

        }
    ]);// checkout controller


})();


