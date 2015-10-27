// travorama angular app - Flight Controller

app.controller('returnFlightController', [
    '$http', '$scope', function($http, $scope) {

        // ******************************
        // on document ready
        angular.element(document).ready(function() {
            // $scope.getDepartureFlight();
            // $scope.getReturnFlight();
            $scope.getFlight('departure');
            $scope.getFlight('return');
        });

        // ******************************
        // general variables
        $scope.pageLoaded = true;
        $scope.pageConfig = {
            activeFlightSection: 'departure',
            showNotice: false,
            fareChanged: false,
            fareUnavailable: false,
            fareToken: '',
            validating: false,
            overviewDetailShown: false,
            flightsValidated: false,
            redirectingPage: false,
            progress: 0
        }
        $scope.departureFlightConfig = {
            name: 'departure',
            expiry: '',
            flightSearchParams: FlightSearchConfig.flightForm.departureFlightParam,
            loading: false,
            loadingFlight: false,
            searchId: '',
            flightList: [],
            flightFilter: {
                transit: [true, true, true],
                airline: {
                    touched: false,
                    list: [],
                    value: [],
                    airlines: []
                },
                time: {
                    departure: [0, 24],
                    arrival: [0, 24]
                },
                price: {
                    initial: [-1, -1],
                    current: [-1, -1],
                    prices: []
                }
            },
            flightSort: {
                label: 'price',
                value: 'TotalFare',
                reverse: false
            },
            activeFlight: -1,
            chosenFlight: -1,
            validating: false,
            validated: false,
            validateToken: '',
            validateAvailable: false,
            validateValid: false,
            validateNewfare: false,
            validateActive: false,
        };
        $scope.returnFlightConfig = {
            name: 'return',
            expiry: '',
            flightSearchParams: FlightSearchConfig.flightForm.returnFlightParam,
            loading: false,
            loadingFlight: false,
            searchId: '',
            flightList: [],
            flightFilter: {
                transit: [true, true, true],
                airline: {
                    touched: false,
                    list: [],
                    value: [],
                    airlines: []
                },
                time: {
                    departure: [0,24],
                    arrival: [0,24]
                },
                price: {
                    initial: [-1, -1],
                    current: [-1, -1],
                    prices: []
                }
            },
            flightSort: {
                label: 'price',
                value: 'TotalFare',
                reverse: false
            },
            activeFlight: -1,
            chosenFlight: -1,
            validating: false,
            validated: false,
            validateToken: '',
            validateAvailable: false,
            validateValid: false,
            validateNewfare: false,
            validateActive: false,
        };

        // ******************************
        // general functions

        // milisecond to hour
        $scope.msToTime = function (duration) {
            var milliseconds = parseInt((duration % 1000) / 100),
                seconds = parseInt((duration / 1000) % 60),
                minutes = parseInt((duration / (1000 * 60)) % 60),
                hours = parseInt((duration / (1000 * 60 * 60)));
            // hours = parseInt((duration / (1000 * 60 * 60)) % 24);
            // days = parseInt((duration / (1000 * 60 * 60 * 24)));

            hours = (hours < 10) ? "0" + hours : hours;
            minutes = (minutes < 10) ? "0" + minutes : minutes;
            seconds = (seconds < 10) ? "0" + seconds : seconds;

            return hours + "h " + minutes + "m";
        }

        // get date
        $scope.getDate = function (dateTime) {
            dateTime = new Date(dateTime);
            dateTime = dateTime.getDate();
            return dateTime;
        }

        // set active flight
        $scope.setActiveFlight = function (target, flightSequence) {
            if (target == 'departure') {
                if ($scope.departureFlightConfig.activeFlight == flightSequence) {
                    $scope.departureFlightConfig.activeFlight = -1;
                } else {
                    $scope.departureFlightConfig.activeFlight = flightSequence;
                }
            } else if (target == 'return') {
                if ($scope.returnFlightConfig.activeFlight == flightSequence) {
                    $scope.returnFlightConfig.activeFlight = -1;
                } else {
                    $scope.returnFlightConfig.activeFlight = flightSequence;
                }
            }
        }

        // set chosen flight
        $scope.setChosenFlight = function (target, flightSequence) {
            if (target == 'departure') {
                $scope.departureFlightConfig.chosenFlight = flightSequence;
                if ($scope.returnFlightConfig.chosenFlight < 0) {
                    $scope.pageConfig.activeFlightSection = 'return';
                }
            } else if (target == 'return') {
                $scope.returnFlightConfig.chosenFlight = flightSequence;
                if ($scope.departureFlightConfig.chosenFlight < 0) {
                    $scope.pageConfig.activeFlightSection = 'departure';
                }
            }
            if ($scope.departureFlightConfig.chosenFlight >= 0 && $scope.returnFlightConfig.chosenFlight >= 0) {
                $('body').addClass('no-scroll');
            } else {
                $('body').removeClass('no-scroll');
            }
        }

        // set page active section
        $scope.setPageActiveSection = function (target) {
            if (target == 'departure') {
                if ($scope.departureFlightConfig.chosenFlight < 0) {
                    $scope.pageConfig.activeFlightSection = 'departure';
                }
            } else if (target == 'return') {
                if ($scope.returnFlightConfig.chosenFlight < 0) {
                    $scope.pageConfig.activeFlightSection = 'return';
                }
            }
        }

        // ******************************
        // flight overview
        // close flight overview
        $scope.closeOverview = function () {
            $scope.pageConfig.overviewDetailShown = false;
            $('body').removeClass('no-scroll');
            if ($scope.pageConfig.flightsValidated == true) {
                $scope.pageConfig.flightsValidated = false;
                if ($scope.departureFlightConfig.validateAvailable == true && $scope.returnFlightConfig.validateAvailable == false) {
                    $scope.pageConfig.activeFlightSection = 'return';
                    $scope.returnFlightConfig.chosenFlight = -1;
                } else if ($scope.departureFlightConfig.validateAvailable == false && $scope.returnFlightConfig.validateAvailable == true) {
                    $scope.pageConfig.activeFlightSection = 'departure';
                    $scope.departureFlightConfig.chosenFlight = -1;
                } else if ($scope.departureFlightConfig.validateAvailable == false && $scope.returnFlightConfig.validateAvailable == false) {
                    if ($scope.departureFlightConfig.validateNewfare == false && $scope.returnFlightConfig.validateNewfare == false) {
                        $scope.pageConfig.activeFlightSection = 'departure';
                        $scope.departureFlightConfig.chosenFlight = -1;
                        $scope.returnFlightConfig.chosenFlight = -1;
                    } else if ($scope.departureFlightConfig.validateNewfare == true && $scope.returnFlightConfig.validateNewfare == true) {
                        $scope.pageConfig.activeFlightSection = 'departure';
                        $scope.departureFlightConfig.chosenFlight = -1;
                        $scope.returnFlightConfig.chosenFlight = -1;
                    } else if ($scope.departureFlightConfig.validateNewfare == false && $scope.returnFlightConfig.validateNewfare == true) {
                        $scope.pageConfig.activeFlightSection = 'departure';
                        $scope.departureFlightConfig.chosenFlight = -1;
                    } else if ($scope.departureFlightConfig.validateNewfare == true && $scope.returnFlightConfig.validateNewfare == false) {
                        $scope.pageConfig.activeFlightSection = 'return';
                        $scope.returnFlightConfig.chosenFlight = -1;
                    }
                }
            } else {
                if ($scope.pageConfig.activeFlightSection == 'return') {
                    $scope.returnFlightConfig.chosenFlight = -1;
                } else if ($scope.pageConfig.activeFlightSection == 'departure') {
                    $scope.departureFlightConfig.chosenFlight = -1;
                }
            }
        }

        // toggle overview detail
        $scope.toggleOverviewDetail = function () {
            if ($scope.pageConfig.overviewDetailShown == true) {
                $scope.pageConfig.overviewDetailShown = false;
            } else {
                $scope.pageConfig.overviewDetailShown = true;
            }
        }

        // ******************************
        // revalidate flights
        $scope.revalidateFlight = function (departureFlightIndexNo, returnFlightIndexNo) {

            if ($scope.pageConfig.flightsValidated) {

                console.log('flight available. Will be redirected within');
                $scope.pageConfig.redirectingPage = true;
                var fareToken = $scope.departureFlightConfig.validateToken + '.' + $scope.returnFlightConfig.validateToken;
                $scope.pageConfig.fareToken = fareToken;
                console.log($scope.pageConfig.fareToken);
                $('.pushToken .fareToken').val(fareToken);
                $('.pushToken').submit();

            } else {

                // revalidate departure flight
                console.log('validating departure flight');
                $scope.departureFlightConfig.validating = true;
                $http.get(RevalidateConfig.Url, {
                    params: {
                        SearchId: $scope.departureFlightConfig.searchId,
                        ItinIndex: departureFlightIndexNo
                    }
                }).success(function (returnData) {
                    $scope.departureFlightConfig.validating = false;
                    if (returnData.IsValid == true) {
                        console.log('departure flight available');
                        $scope.departureFlightConfig.validateToken = returnData.Token;
                        $scope.departureFlightConfig.validateAvailable = true;
                        if ($scope.returnFlightConfig.validating == false) {
                            flightsValidated();
                        }
                    } else if (returnData.IsValid == false) {
                        console.log('departure flight unavailable');
                        $scope.departureFlightConfig.validateAvailable = false;
                        if (returnData.IsOtherFareAvailable == true) {
                            $scope.departureFlightConfig.validateToken = returnData.Token;
                            console.log('departure flight has new price');
                            $scope.departureFlightConfig.validateNewfare = true;
                            $scope.departureFlightConfig.flightList[departureFlightIndexNo].TotalFare = returnData.NewFare;
                            if ($scope.returnFlightConfig.validating == false) {
                                flightsValidated();
                            }
                        } else if (returnData.IsOtherFareAvailable == false) {
                            console.log('departure flight is gone');
                            $scope.departureFlightConfig.validateNewfare = false;
                            $scope.departureFlightConfig.flightList[departureFlightIndexNo].available = false;
                            if ($scope.returnFlightConfig.validating == false) {
                                flightsValidated();
                            }
                        }
                    }
                }).error(function (data) {
                    $scope.departureFlightConfig.validating = false;
                    console.log('ERROR Validating Departure Flight :');
                    console.log(data);
                    console.log('--------------------');
                });

                // revalidate return flight
                console.log('validating return flight');
                $scope.returnFlightConfig.validating = true;
                $http.get(RevalidateConfig.Url, {
                    params: {
                        SearchId: $scope.returnFlightConfig.searchId,
                        ItinIndex: returnFlightIndexNo
                    }
                }).success(function (returnData) {
                    $scope.returnFlightConfig.validating = false;
                    if (returnData.IsValid == true) {
                        console.log('return flight available');
                        $scope.returnFlightConfig.validateToken = returnData.Token;
                        $scope.returnFlightConfig.validateAvailable = true;
                        if ($scope.departureFlightConfig.validating == false) {
                            flightsValidated();
                        }
                    } else if (returnData.IsValid == false) {
                        console.log('return flight unavailable');
                        $scope.returnFlightConfig.validateAvailable = false;
                        if (returnData.IsOtherFareAvailable == true) {
                            $scope.returnFlightConfig.validateToken = returnData.Token;
                            console.log('return flight has new price');
                            $scope.returnFlightConfig.validateNewfare = true;
                            $scope.returnFlightConfig.flightList[returnFlightIndexNo].TotalFare = returnData.NewFare;
                            if ($scope.departureFlightConfig.validating == false) {
                                flightsValidated();
                            }
                        } else if (returnData.IsOtherFareAvailable == false) {
                            console.log('return flight is gone');
                            $scope.returnFlightConfig.validateNewfare = false;
                            $scope.returnFlightConfig.flightList[returnFlightIndexNo].Available = false;
                            if ($scope.departureFlightConfig.validating == false) {
                                flightsValidated();
                            }
                        }
                    }
                }).error(function (data) {
                    $scope.returnFlightConfig.validatingFlight = false;
                    console.log('ERROR Validating Departure Flight :');
                    console.log(data);
                    console.log('--------------------');
                });

                /*-----------*/
                // run after both flight validated
                var flightsValidated = function () {
                    $scope.pageConfig.flightsValidated = true;
                    console.log('flights validated');
                    // if flight is valid
                    if ($scope.departureFlightConfig.validateAvailable == true && $scope.returnFlightConfig.validateAvailable == true) {
                        console.log('flight available. Will be redirected.');
                        $scope.pageConfig.redirectingPage = true;
                        var fareToken = $scope.departureFlightConfig.validateToken + '.' + $scope.returnFlightConfig.validateToken;
                        console.log('Token : '+fareToken);
                        $('.pushToken .fareToken').val(fareToken);
                        $('.pushToken').submit();
                    }
                }

            }


        }

        // ******************************
        // flight sorting
        $scope.setSort = function (targetFlight, label) {
            if (targetFlight == 'departure') {
                targetFlight = $scope.departureFlightConfig;
            } else {
                targetFlight = $scope.returnFlightConfig;
            }
            targetFlight.flightSort.reverse = (targetFlight.flightSort.label === label) ? !targetFlight.flightSort.reverse : false;
            targetFlight.flightSort.label = label;
            switch (label) {
                case 'airline':
                    targetFlight.flightSort.value = 'Trips[0].Airlines[0].Name';
                    break;
                case 'departure':
                    targetFlight.flightSort.value = 'Trips[0].Segments[0].DepartureTime';
                    break;
                case 'arrival':
                    targetFlight.flightSort.value = 'Trips[0].Segments[(Trips[0].Segments.length-1)].ArrivalTime';
                    break;
                case 'duration':
                    targetFlight.flightSort.value = 'Trips[0].TotalDuration';
                    break;
                case 'price':
                    targetFlight.flightSort.value = 'TotalFare';
                    break;
            }
        }

        // ******************************
        // flight filtering
        // available filter
        $scope.availableFilter = function() {
            return function (flight) {
                if (flight.Available) {
                    return flight;
                }
            }
        }

        // transit filter
        $scope.transitFilter = function (targetFlight) {
            var targetScope = (targetFlight == 'departure' ? $scope.departureFlightConfig : $scope.returnFlightConfig);

            return function (flight) {
                if (targetScope.flightFilter.transit[0]) {
                    if (flight.Trips[0].TotalTransit == 0) {
                        return flight;
                    }
                }
                if (targetScope.flightFilter.transit[1]) {
                    if (flight.Trips[0].TotalTransit == 1) {
                        return flight;
                    }
                }
                if (targetScope.flightFilter.transit[2]) {
                    if (flight.Trips[0].TotalTransit > 1) {
                        return flight;
                    }
                }
            }

        }
        $scope.setTransitFilter = function(targetFlight, target) {
            var targetScope = (targetFlight == 'departure' ? $scope.departureFlightConfig : $scope.returnFlightConfig);
            if (target == 'all') {
                targetScope.flightFilter.transit[0] = true;
                targetScope.flightFilter.transit[1] = true;
                targetScope.flightFilter.transit[2] = true;
            } else {
                targetScope.flightFilter.transit[0] = false;
                targetScope.flightFilter.transit[1] = false;
                targetScope.flightFilter.transit[2] = false;
            }
        }

        // price filter
        $scope.priceFilter = function (targetFlight) {
            var targetScope = (targetFlight == 'departure' ? $scope.departureFlightConfig : $scope.returnFlightConfig);
            return function (flight) {
                if (flight.TotalFare >= targetScope.flightFilter.price.current[0] && flight.TotalFare <= targetScope.flightFilter.price.current[1]) {
                    return flight;
                }
            }
        }

        // airline filter
        $scope.checkAirline = function (targetFlight) {
            var targetScope = (targetFlight == 'departure' ? $scope.departureFlightConfig : $scope.returnFlightConfig);
            targetScope.flightFilter.airline.touched = true;
            targetScope.flightFilter.airline.value = [];
            for (var i = 0; i < targetScope.flightFilter.airline.list.length; i++) {
                if (targetScope.flightFilter.airline.list[i].checked == true) {
                    targetScope.flightFilter.airline.value.push(targetScope.flightFilter.airline.list[i].Code);
                }
            }
        }
        $scope.setAirlineFilter = function (targetFlight, target) {
            var targetScope = (targetFlight == 'departure' ? $scope.departureFlightConfig : $scope.returnFlightConfig);
            for (var i = 0; i < targetScope.flightFilter.airline.list.length; i++) {
                if (target == 'all') {
                    targetScope.flightFilter.airline.list[i].checked = true;
                } else {
                    targetScope.flightFilter.airline.list[i].checked = false;
                }
            }
            $scope.checkAirline(targetFlight);
        }
        $scope.airlineFilter = function (targetFlight) {
            var targetScope = (targetFlight == 'departure' ? $scope.departureFlightConfig : $scope.returnFlightConfig);
            return function(flight) {
                if (targetScope.flightFilter.airline.touched == false) {
                    return flight;
                } else {
                    for (var i in flight.AirlinesTag) {
                        if (targetScope.flightFilter.airline.value.indexOf(flight.AirlinesTag[i]) != -1) {
                            return flight;
                        }
                    }
                }
            }
        }

        // time filter
        $scope.getHour = function (dateTime) {
            dateTime = dateTime.substr(11, 2);
            return parseInt(dateTime);
        }
        $scope.timeFilter = function (targetFlight) {
            var targetScope = (targetFlight == 'departure' ? $scope.departureFlightConfig : $scope.returnFlightConfig);
            return function(flight) {
                if ($scope.getHour(flight.Trips[0].Segments[0].DepartureTime) >= parseInt(targetScope.flightFilter.time.departure[0])
                        && $scope.getHour(flight.Trips[0].Segments[0].DepartureTime) <= parseInt(targetScope.flightFilter.time.departure[1])
                        && $scope.getHour(flight.Trips[0].Segments[flight.Trips[0].Segments.length - 1].ArrivalTime) >= parseInt(targetScope.flightFilter.time.arrival[0])
                        && $scope.getHour(flight.Trips[0].Segments[flight.Trips[0].Segments.length - 1].ArrivalTime) <= parseInt(targetScope.flightFilter.time.arrival[1])
                ) {
                    return flight;
                }
            }
        }

        // ******************************
        // get flights
        $scope.getFlight = function (targetScope) {
            // set target scope
            if (targetScope == "departure" || targetScope == "Departure") {
                targetScope = $scope.departureFlightConfig;
            } else {
                targetScope = $scope.returnFlightConfig;
            }

            targetScope.loading = true;
            targetScope.loadingFlight = true;

            // get flight
            if (targetScope.flightSearchParams.Completeness < 100) {

                 $http.get(FlightSearchConfig.Url, {
                    params: {
                        request : targetScope.flightSearchParams
                    }
                }).success(function(returnData) {
                    
                    console.log(targetScope.name+'completeness : ' +targetScope.flightSearchParams.Completeness + ' & '+targetScope.name+' completeness : ' +returnData.Completeness);

                    targetScope.flightSearchParams.SearchId = returnData.SearchId;
                    targetScope.searchId = returnData.SearchId;

                    if (targetScope.flightSearchParams.Completeness == returnData.Completeness) {
                         setTimeout(function() {
                             $scope.getFlight(targetScope.targetFlight);
                         }, 1000);
                    } else {
                         targetScope.flightSearchParams.Completeness = returnData.Completeness;

                        console.log('Success getting '+targetScope.name+' flight list');
                        console.log(returnData);

                        // pass data if FlightList.length != 0
                        if (returnData.FlightList.length) {
                            $scope.arrangeFlightData(targetScope, returnData.FlightList);
                        }

                        if (returnData.Completeness == 100) {
                            targetScope.loading = false;
                            targetScope.loadingFlight = false;
                            console.log('Finished getting ' + targetScope.name + ' flight list');
                            console.log('----------');
                        } else {
                            setTimeout(function() {
                                $scope.getFlight(targetScope.name);
                            }, 1000);
                        }
                    }
                 }).error(function(returnData) {
                    console.log('Failed to get '+targetScope.name+' flight list');
                    console.log('ERROR :' +returnData);
                });

            } else {
                targetScope.loading = false;
                targetScope.loadingFlight = false;
            }

        }

        // arrange flight
        $scope.arrangeFlightData = function(targetScope, data) {

            for (var i = 0; i < data.length; i++) {
                data[i].Available = true;
                targetScope.flightList.push(data[i]);
            }

            if (targetScope.flightSearchParams.Completeness == 100) {

                console.log(targetScope);

                // loop the result
                for (var i = 0; i < targetScope.flightList.length; i++) {
                    targetScope.flightList[i].Available = true;
                    // fare rule
                    targetScope.flightList[i].FareRules = {
                        Loaded: false,
                        Content: ''
                    };

                    // populate prices
                    targetScope.flightFilter.price.prices.push(targetScope.flightList[i].TotalFare);

                    // populate airline code
                    targetScope.flightList[i].AirlinesTag = [];
                    for (var x = 0; x < targetScope.flightList[i].Trips[0].Airlines.length; x++) {
                        targetScope.flightFilter.airline.airlines.push(targetScope.flightList[i].Trips[0].Airlines[x]);
                        targetScope.flightList[i].AirlinesTag.push(targetScope.flightList[i].Trips[0].Airlines[x].Code);
                    }

                }

                // activate flight filter
                if (targetScope.name == 'departure') {
                    $scope.initiateFlightFiltering('departure');
                } else {
                    $scope.initiateFlightFiltering('return');
                }
            }

        }// $scope.arrangeFlight()

        // initiate flight filtering
        $scope.initiateFlightFiltering = function(targetFlight) {
            var targetScope = (targetFlight == 'departure' ? $scope.departureFlightConfig : $scope.returnFlightConfig);

            console.log(targetFlight);

            // sort prices and set initial price
            function sortNumber(a, b) {
                return a - b;
            }
            targetScope.flightFilter.price.prices.sort(sortNumber);
            targetScope.flightFilter.price.initial[0] = Math.floor(targetScope.flightFilter.price.prices[0]);
            targetScope.flightFilter.price.initial[1] = Math.round(targetScope.flightFilter.price.prices[targetScope.flightFilter.price.prices.length - 1]);

            targetScope.flightFilter.price.current[0] = Math.floor(targetScope.flightFilter.price.prices[0]);
            targetScope.flightFilter.price.current[1] = Math.round(targetScope.flightFilter.price.prices[targetScope.flightFilter.price.prices.length - 1]);

            // remove duplicatae from airlines
            var dupes = {};
            $.each(targetScope.flightFilter.airline.airlines, function (i, el) {
                if (!dupes[el.Code]) {
                    dupes[el.Code] = true;
                    targetScope.flightFilter.airline.list.push(el);
                }
            });
            targetScope.flightFilter.airline.airlines = [];

            // activate price filter
            $('.'+targetFlight+'-price-slider').slider({
                range: true,
                min: targetScope.flightFilter.price.initial[0],
                max: targetScope.flightFilter.price.initial[1],
                step: 50000,
                values: [targetScope.flightFilter.price.initial[0], targetScope.flightFilter.price.initial[1]],
                create: function (event, ui) {
                    $('.'+targetFlight+'-price-slider-min').val(targetScope.flightFilter.price.initial[0]);
                    $('.' + targetFlight + '-price-slider-min').trigger('input');
                    $('.' + targetFlight + '-price-slider-max').val(targetScope.flightFilter.price.initial[1]);
                    $('.' + targetFlight + '-price-slider-max').trigger('input');
                },
                slide: function (event, ui) {
                    $('.' + targetFlight + '-price-slider-min').val(ui.values[0]);
                    $('.' + targetFlight + '-price-slider-min').trigger('input');
                    $('.' + targetFlight + '-price-slider-max').val(ui.values[1]);
                    $('.' + targetFlight + '-price-slider-max').trigger('input');
                }
            });
            
            // activate time slider
            $('.'+targetFlight+'-departure-slider').slider({
                range: true,
                min: 0, max: 24, step: 1, values: [0, 24],
                create: function (event, ui) {
                    $('.' + targetFlight + '-departure-slider-min').val(0);
                    $('.' + targetFlight + '-departure-slider-min').trigger('input');
                    $('.' + targetFlight + '-departure-slider-max').val(24);
                    $('.' + targetFlight + '-departure-slider-max').trigger('input');
                },
                slide: function (event, ui) {
                    $('.' + targetFlight + '-departure-slider-min').val(ui.values[0]);
                    $('.' + targetFlight + '-departure-slider-min').trigger('input');
                    $('.' + targetFlight + '-departure-slider-max').val(ui.values[1]);
                    $('.' + targetFlight + '-departure-slider-max').trigger('input');
                }
            });
            $('.'+targetFlight+'-arrival-slider').slider({
                range: true,
                min: 0, max: 24, step: 1, values: [0, 24],
                create: function (event, ui) {
                    $('.' + targetFlight + '-arrival-slider-min').val(0);
                    $('.' + targetFlight + '-arrival-slider-min').trigger('input');
                    $('.' + targetFlight + '-arrival-slider-max').val(24);
                    $('.' + targetFlight + '-arrival-slider-max').trigger('input');
                },
                slide: function (event, ui) {
                    $('.' + targetFlight + '-arrival-slider-min').val(ui.values[0]);
                    $('.' + targetFlight + '-arrival-slider-min').trigger('input');
                    $('.' + targetFlight + '-arrival-slider-max').val(ui.values[1]);
                    $('.' + targetFlight + '-arrival-slider-max').trigger('input');
                }
            });
            
        }



    }
]);// flight controller

