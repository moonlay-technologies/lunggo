// travorama angular app - Flight Controller

app.controller('returnFlightController', [
    '$http', '$scope', '$interval', '$timeout', '$log', function($http, $scope, $interval, $timeout, $log) {

        // ******************************
        // on document ready
        angular.element(document).ready(function () {
            $scope.getFlight('return');
            $scope.staticFilter('departure');
            $scope.staticFilter('return');
        });
        // ******************************
        // general variables
        $scope.returnUrl = "/";
        $scope.pageLoaded = true;
        $scope.trial = 0;
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
            progress: 0,
            expiry: {
                expired: false,
                time: '',
                start: function() {
                    var expiryTime = new Date($scope.pageConfig.expiry.time);
                    if ($scope.pageConfig.expiry.expired || $scope.pageConfig.expiry.starting) return;
                    $interval(function () {
                        $scope.pageConfig.expiry.starting = true;
                        var nowTime = new Date();
                        if (nowTime > expiryTime) {
                            $scope.pageConfig.expiry.expired = true;
                        }
                    }, 1000);
                },
                starting : false
            }
        }

        //*****Untuk Request API****
        var departureTemp = FlightSearchConfig.flightForm.departureFlightParam;
        var depDate = new Date(departureTemp.trips[0].DepartureDate) || '';
        var departureDate = (('0' + depDate.getDate()).slice(-2) + ('0' + (depDate.getMonth() + 1)).slice(-2) + depDate.getFullYear().toString().substr(2, 2));
        var departureRequest = departureTemp.trips[0].OriginAirport + departureTemp.trips[0].DestinationAirport + departureDate;
        var returnTemp = FlightSearchConfig.flightForm.returnFlightParam;
        var retDate = new Date(returnTemp.trips[0].DepartureDate) || '';
        var returnDate = (('0' + retDate.getDate()).slice(-2) + ('0' + (retDate.getMonth() + 1)).slice(-2) + retDate.getFullYear().toString().substr(2, 2));
        var returnRequest = returnTemp.trips[0].OriginAirport + returnTemp.trips[0].DestinationAirport + returnDate;
        var cabin = FlightSearchConfig.flightForm.returnFlightParam.CabinClass;
        if (cabin != 'y' || cabin != 'c' || cabin != 'f') {
            switch (cabin) {
                case 'Economy':
                    cabin = 'y';
                    break;
                case 'Business':
                    cabin = 'c';
                    break;
                case 'First':
                    cabin = 'f';
                    break;
            }
        }

        var passenger = FlightSearchConfig.flightForm.returnFlightParam.AdultCount + '' + FlightSearchConfig.flightForm.returnFlightParam.ChildCount + '' + FlightSearchConfig.flightForm.returnFlightParam.InfantCount + cabin;
        var flightFixRequest = departureRequest + '~' + returnRequest + '-' + passenger;
        //****END****

        $scope.returnFlightConfig = {
            notice: true,
            adultCount: FlightSearchConfig.flightForm.returnFlightParam.AdultCount,
            childCount: FlightSearchConfig.flightForm.returnFlightParam.ChildCount,
            infantCount: FlightSearchConfig.flightForm.returnFlightParam.InfantCount,
            name: 'return',
            expiry: '',
            flightSearchParams: flightFixRequest,
            loading: false,
            progress:0,
            maxprogress: 0,
            finalProgress:0,
            loadingFlight: false,
            searchId: '',
            flightList: [],
            pristine: true,
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
                value: 'netAdultFare',
                reverse: false
            },
            activeFlight: -1,
            chosenFlight: -1,
            chosenFlightDetail: false,
            validating: false,
            validated: false,
            validateToken: '',
            validateAvailable: false,
            validateValid: false,
            validateNewfare: false,
            validateActive: false,
        };
        
        $scope.departureFlightConfig = {
            notice: true,
            name: 'departure',
            adultCount: FlightSearchConfig.flightForm.returnFlightParam.AdultCount,
            childCount: FlightSearchConfig.flightForm.returnFlightParam.ChildCount,
            infantCount: FlightSearchConfig.flightForm.returnFlightParam.InfantCount,
            expiry: '',
            loading: false,
            loadingFlight: false,
            searchId: '',
            flightList: [],
            pristine: true,
            progress: 0,
            maxprogress: 0,
            finalProgress: 0,
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
                value: 'netAdultFare',
                reverse: false
            },
            activeFlight: -1,
            chosenFlight: -1,
            chosenFlightDetail: false,
            validating: false,
            validated: false,
            validateToken: '',
            validateAvailable: false,
            validateValid: false,
            validateNewfare: false,
            validateActive: false,
        };
        //$scope.departureFlightConfig.flightSearchParams.MaxProgress = 0;
        $scope.returnFlightConfig.maxprogress = 0;

        // ******************************
        // general functions

        // close notice
        $scope.closeNotice = function (target) {
            if (target == 'departure') {
                $scope.departureFlightConfig.notice = false;
            } else {
                $scope.returnFlightConfig.notice = false;
            }
        }

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
            return hours + "j " + minutes + "m";
        }

        // get date
        $scope.getDate = function (dateTime) {
            dateTime = new Date(dateTime);
            dateTime = dateTime.getDate();
            return dateTime;
        }

        $scope.getFullDate = function (dateTime) {
            if (dateTime) {
                dateTime = parseInt(dateTime.substr(0, 4) + '' + dateTime.substr(5, 2) + '' + dateTime.substr(8, 2));
                return dateTime;
            }
        }
        $scope.overlapDate = function(onwardArrival, returnDeparture) {
            if (onwardArrival && returnDeparture) {
                onwardArrival = new Date(onwardArrival);
                returnDeparture = new Date(returnDeparture);
                return (returnDeparture <= onwardArrival);
            }
        }
        $scope.getOverdayDate = function (departureDate, arrivalDate) {
            if (departureDate && arrivalDate) {
                departureDate = new Date(departureDate);
                arrivalDate = new Date(arrivalDate);
                departureDate = Date.UTC(departureDate.getUTCFullYear(), (departureDate.getUTCMonth() ), departureDate.getUTCDate());
                arrivalDate = Date.UTC(arrivalDate.getUTCFullYear(), (arrivalDate.getUTCMonth() ), arrivalDate.getUTCDate());
                var overday = Math.floor((arrivalDate - departureDate) / (1000 * 3600 * 24));
                if (overday > 0) {
                    overday = '+' + overday;
                }
                return overday;
            }
        }

        //****************************SHOW MEAL, BAGGAGE, TAX ICON ****************//
        $scope.checkMeal = function (trip) {
            var available = false;
            for (var x = 0; x < trip.segments.length; x++) {
                if (trip.segments[x].hasMeal) {
                    available = true;
                }
            }

            return available;
        }

        $scope.minBaggage = function (trip) {
            var listbaggage = [];
            for (var x = 0; x < trip.segments.length; x++) {
                if (trip.segments[x].baggageCapacity != 0) {
                    listbaggage.push(trip.segments[x].baggageCapacity);
                }
            }
            var minvalue = Math.min.apply(Math, listbaggage);
            if (minvalue == 'Infinity') {
                return 0;
            }
            return Math.min.apply(Math, listbaggage);
        }

        $scope.checkBaggageNaN = function (val) {
            return Number.isNaN(val);
        }

        $scope.checkTax = function (trip) {
            var valid = true;
            for (var x = 0; x < trip.segments.length; x++) {
                if (trip.segments[x].includingPsc == false) {
                    valid = false;
                }
            }
            return valid;
        }
        //*******************************END*******************************
        
        // set active flight BUAT DETAIL PENERBANGAN
        $scope.setActiveFlight = function (target, flightSequence) {
            if (target == 'departure') {
                if ($scope.departureFlightConfig.activeFlight == flightSequence) {
                    $scope.departureFlightConfig.activeFlight = -1;
                } else {
                    $scope.departureFlightConfig.activeFlight = flightSequence;
                    $scope.departureFlightConfig.activePrice = -1;
                }
            } else if (target == 'return') {
                if ($scope.returnFlightConfig.activeFlight == flightSequence) {
                    $scope.returnFlightConfig.activeFlight = -1;
                } else {
                    $scope.returnFlightConfig.activeFlight = flightSequence;
                    $scope.returnFlightConfig.activePrice = -1;
                }
            }
        }
        $scope.showPriceDetail = function (target, flightSequence) {
            $scope.flightDetailSelected = false;
            $scope.priceDetailSelected = true;

            if (target == 'departure') {
                $scope.selectedFlight = $scope.departureFlightConfig;
            }
            else if (target == 'return') {
                $scope.selectedFlight = $scope.returnFlightConfig;
            }

            if ($scope.selectedFlight.activePrice != flightSequence) {
                $scope.selectedFlight.activePrice = flightSequence;
                $scope.selectedFlight.activeFlight = -1;
            }
            else {
                $scope.selectedFlight.activePrice = -1;
            }
        }

        // **********
        // flight detail function
        $scope.priceDetailSelected = false;
        $scope.flightDetailSelected = false;
        $scope.activePrice = -1;
        $scope.flightActive = -1;

        $scope.setFlightActive = function (flightNo) {
            $scope.flightDetailSelected = true;
            $scope.priceDetailSelected = false;
            if ($scope.flightActive != flightNo) {
                $scope.flightActive = flightNo;
                $scope.activePrice = -1;
            }
            else {
                $scope.flightActive = -1;
            }
        }
        //******end*******


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
        //******end*******

        // toggle chosen flight
        $scope.toggleChosenFlightDetail = function(targetFlight) {
            var targetScope = (targetFlight == 'departure' ? $scope.departureFlightConfig : $scope.returnFlightConfig);

            if (targetScope.chosenFlightDetail == true) {
                targetScope.chosenFlightDetail = false;
            } else {
                targetScope.chosenFlightDetail = true;
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

        $scope.pageConfig.overviewFlightDetailShowing = false;
        $scope.pageConfig.overviewPriceDetailShowing = false;

        // toggle overview detail
        $scope.toggleOverviewFlightDetail = function () {
            if ($scope.pageConfig.overviewFlightDetailShowing == false) {
                $scope.pageConfig.overviewFlightDetailShowing = true;
                $scope.pageConfig.overviewPriceDetailShowing = false;
            } else {
                $scope.pageConfig.overviewFlightDetailShowing = false;
            }
        };

        $scope.toggleOverviewPriceDetail = function () {
            if ($scope.pageConfig.overviewPriceDetailShowing == false) {
                $scope.pageConfig.overviewPriceDetailShowing = true;
                $scope.pageConfig.overviewFlightDetailShowing = false;
            } else {
                $scope.pageConfig.overviewPriceDetailShowing = false;
            }
        };

        $scope.selectError = false;
        $scope.popup = false;

        // ******************************
        // revalidate flights
        $scope.selectFlight = function (departureFlightIndexNo, returnFlightIndexNo) {
            
            if ($scope.trial > 3)
            {
                $scope.trial = 0;
            }
            $scope.popup = true;
            $scope.closeOverview();
            if ($scope.pageConfig.flightsValidated) {

                $log.debug('flight available. Will be redirected within');
                $scope.pageConfig.redirectingPage = true;
                var fareToken = $scope.departureFlightConfig.validateToken + '.' + $scope.returnFlightConfig.validateToken;
                $scope.pageConfig.fareToken = fareToken;
                $log.debug($scope.pageConfig.fareToken);
                $('.pushToken .fareToken').val(fareToken);
                $('.pushToken').submit();

            } else {

                // select flight
                $log.debug('validating departure flight no : ' + departureFlightIndexNo + ',return flight no : ' + returnFlightIndexNo + 'SearchId : ' + $scope.returnFlightConfig.searchId);
                //$log.debug('Dep Req : ' + $scope.departureFlightConfig.flightList[departureFlightIndexNo].reg + ':::: Ret Req :' + $scope.returnFlightConfig.flightList[returnFlightIndexNo].reg);
                $scope.departureFlightConfig.validating = true;
                $scope.returnFlightConfig.validating = true;

                //Check Authorization
                var authAccess = getAuthAccess();
                if (authAccess == 1 || authAccess == 2) {
                    $http({
                        method: 'POST',
                        url: SelectConfig.Url,
                        data: {
                            searchId: $scope.returnFlightConfig.searchId,
                            regs: [$scope.departureFlightConfig.flightList[departureFlightIndexNo].reg, $scope.returnFlightConfig.flightList[returnFlightIndexNo].reg]
                        },
                        headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                    }).then(function (returnData) {
                        $scope.departureFlightConfig.validating = false;
                        $scope.returnFlightConfig.validating = false;
                        if ((returnData.data.token != "" || returnData.data.token != null) && returnData.data.status == "200") {
                            $log.debug('flight available');
                            $scope.departureFlightConfig.validateToken = returnData.data.token;
                            $scope.departureFlightConfig.validateAvailable = true;
                            $scope.returnFlightConfig.validateToken = returnData.data.token;
                            $scope.returnFlightConfig.validateAvailable = true;
                            if ($scope.returnFlightConfig.validating == false) {
                                flightsValidated();
                            }
                        }
                        else {
                            $log.debug('flight unavailable');
                            $scope.departureFlightConfig.validateAvailable = false;
                            $scope.returnFlightConfig.validateAvailable = false;
                            $scope.popup = false;
                            $scope.selectError = true;
                        }
                    }).catch(function (returnData) {
                        $scope.trial++;
                        if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                        {
                            $scope.selectFlight(departureFlightIndexNo, returnFlightIndexNo);
                        }
                        else
                        {
                            $scope.departureFlightConfig.validating = false;
                            $scope.returnFlightConfig.validating = false;
                            $log.debug('ERROR Validating Flight :');
                            $log.debug('--------------------');
                            $scope.popup = false;
                            $scope.selectError = true;
                        }
                    });
                }
                else {
                    $scope.departureFlightConfig.validating = false;
                    $scope.returnFlightConfig.validating = false;
                    $log.debug('Not Authorized');
                    $scope.popup = false;
                    $scope.selectError = true;
                }

                /*-----------*/
                // run after both flight validated
                var flightsValidated = function () {
                    $scope.pageConfig.flightsValidated = true;
                    $log.debug('flights validated');
                    // if flight is valid
                    if ($scope.departureFlightConfig.validateAvailable == true && $scope.returnFlightConfig.validateAvailable == true) {
                        $log.debug('flight available. Will be redirected.');
                        $scope.pageConfig.redirectingPage = true;
                        var fareToken = $scope.departureFlightConfig.validateToken;;
                        $log.debug('Token : '+fareToken);
                        $('.pushToken .fareToken').val(fareToken);
                        $('.pushToken').submit();
                    }
                }
            }
        }

        $scope.selectSubmit = function(){}

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
                    targetFlight.flightSort.value = 'trips[0].airlines[0].name';
                    break;
                case 'departure':
                    targetFlight.flightSort.value = 'trips[0].segments[0].departureTime';
                    break;
                case 'arrival':
                    targetFlight.flightSort.value = 'trips[0].segments[(trips[0].segments.length-1)].arrivalTime';
                    break;
                case 'duration':
                    targetFlight.flightSort.value = 'trips[0].totalDuration';
                    break;
                case 'price':
                    targetFlight.flightSort.value = 'netAdultFare';
                    break;
            }
        }

        // ******************************FLIGHT FILTER *****************************
        
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
                //if (!targetScope.loading && !targetScope.loadingFlight) {
                    if (targetScope.flightFilter.transit[0]) {
                        if (flight.trips[0].transitCount == 0) {
                            return flight;
                        }
                    }
                    if (targetScope.flightFilter.transit[1]) {
                        if (flight.trips[0].transitCount == 1) {
                            return flight;
                        }
                    }
                    if (targetScope.flightFilter.transit[2]) {
                        if (flight.trips[0].transitCount > 1) {
                            return flight;
                        }
                    }
                //} else {
                //    return flight;
                //}
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
                //if (!targetScope.loading && !targetScope.loadingFlight) {
                    if (flight.netAdultFare >= targetScope.flightFilter.price.current[0] && flight.netAdultFare <= targetScope.flightFilter.price.current[1]) {
                        return flight;
                    }
                //} else {
                //    return flight;
                //}
            }
        }

        // airline filter
        $scope.checkAirline = function (targetFlight) {
            var targetScope = (targetFlight == 'departure' ? $scope.departureFlightConfig : $scope.returnFlightConfig);
            targetScope.flightFilter.airline.touched = true;
            targetScope.flightFilter.airline.value = [];
            for (var i = 0; i < targetScope.flightFilter.airline.list.length; i++) {
                if (targetScope.flightFilter.airline.list[i].checked == true) {
                    targetScope.flightFilter.airline.value.push(targetScope.flightFilter.airline.list[i].name);
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
        $scope.getMinute = function (dateTime) {
            var hour = dateTime.substr(11, 2);
            var minute = dateTime.substr(14, 2);
            return parseInt(hour) * 60 + parseInt(minute);
        }

        $scope.timeFilter = function (targetFlight) {
            var targetScope = (targetFlight == 'departure' ? $scope.departureFlightConfig : $scope.returnFlightConfig);
            return function (flight) {
                if ($scope.getMinute(flight.trips[0].segments[0].departureTime) >= parseInt(targetScope.flightFilter.time.departure[0]) * 60
                    && $scope.getMinute(flight.trips[0].segments[0].departureTime) <= parseInt(targetScope.flightFilter.time.departure[1]) * 60
                    && $scope.getMinute(flight.trips[0].segments[flight.trips[0].segments.length - 1].arrivalTime) >= parseInt(targetScope.flightFilter.time.arrival[0]) * 60
                    && $scope.getMinute(flight.trips[0].segments[flight.trips[0].segments.length - 1].arrivalTime) <= parseInt(targetScope.flightFilter.time.arrival[1]) * 60)
                {
                    return flight;
                }
            }
        }

        $scope.dupes = {
            departure: {},
            ret: {}
        }

        // initiate flight filtering
        $scope.initiateFlightFiltering = function (targetFlight) {
            var targetScope = (targetFlight == 'departure' ? $scope.departureFlightConfig : $scope.returnFlightConfig);

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

            // activate price filter
            $('.' + targetFlight + '-price-slider').slider({
                range: true,
                min: targetScope.flightFilter.price.initial[0],
                max: targetScope.flightFilter.price.initial[1],
                step: 100,
                values: [targetScope.flightFilter.price.initial[0], targetScope.flightFilter.price.initial[1]],
                create: function (event, ui) {
                    $('.' + targetFlight + '-price-slider-min').val(targetScope.flightFilter.price.initial[0]);
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



            targetScope.progress = 100;

            $log.debug('Completed setting flight filtering for : ' + targetScope.name);
            $log.debug(targetScope);
        }

        $scope.staticFilter = function (targetFlight) {
            // activate time slider
            $('.' + targetFlight + '-departure-slider').slider({
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
            $('.' + targetFlight + '-arrival-slider').slider({
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
        //Filling flight lists for filtering
        $scope.populateAirlines = function (targetScope) {
            var v = targetScope;
            if (targetScope == "departure" || targetScope == "Departure") {
                targetScope = $scope.departureFlightConfig;
            } else {
                targetScope = $scope.returnFlightConfig;
            }
            for (var i = 0; i < targetScope.flightList.length; i++) {
                // populate airline code
                targetScope.flightList[i].AirlinesTag = [];
                for (var x = 0; x < targetScope.flightList[i].trips[0].airlines.length; x++) {
                    targetScope.flightFilter.airline.airlines.push(targetScope.flightList[i].trips[0].airlines[x]);
                    targetScope.flightList[i].AirlinesTag.push(targetScope.flightList[i].trips[0].airlines[x].name);
                }

            }

            //var dupes = {};
            $.each(targetScope.flightFilter.airline.airlines, function (i, el) {
                if (v == 'departure') {
                    if (!$scope.dupes.departure[el.name]) {
                        $scope.dupes.departure[el.name] = true;
                        targetScope.flightFilter.airline.list.push(el);
                    }
                } else {
                    if (!$scope.dupes.ret[el.name]) {
                        $scope.dupes.ret[el.name] = true;
                        targetScope.flightFilter.airline.list.push(el);
                    }
                }
                
            });
            targetScope.flightFilter.airline.airlines = [];
        }

        $scope.populatePrice = function(targetFlight) {
            var targetScope = (targetFlight == 'departure' ? $scope.departureFlightConfig : $scope.returnFlightConfig);

            for (var i = 0; i < targetScope.flightList.length; i++) {
               
                // populate prices
                targetScope.flightFilter.price.prices.push(targetScope.flightList[i].netAdultFare);
            }

            function sortNumber(a, b) {
                return a - b;
            }
            targetScope.flightFilter.price.prices.sort(sortNumber);
            targetScope.flightFilter.price.initial[0] = Math.floor(targetScope.flightFilter.price.prices[0]);
            targetScope.flightFilter.price.initial[1] = Math.round(targetScope.flightFilter.price.prices[targetScope.flightFilter.price.prices.length - 1]);

            targetScope.flightFilter.price.current[0] = Math.floor(targetScope.flightFilter.price.prices[0]);
            targetScope.flightFilter.price.current[1] = Math.round(targetScope.flightFilter.price.prices[targetScope.flightFilter.price.prices.length - 1]);

            // remove duplicatae from airlines

            // activate price filter
            $('.' + targetFlight + '-price-slider').slider({
                range: true,
                min: targetScope.flightFilter.price.initial[0],
                max: targetScope.flightFilter.price.initial[1],
                step: 100,
                values: [targetScope.flightFilter.price.initial[0], targetScope.flightFilter.price.initial[1]],
                create: function (event, ui) {
                    $('.' + targetFlight + '-price-slider-min').val(targetScope.flightFilter.price.initial[0]);
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
        }

        //**********************END***********************
        
        // get flights
        $scope.getFlight = function (targetScope) {

            if ($scope.trial > 3)
            {
                $scope.trial = 0;
            }

            targetScope = $scope.returnFlightConfig;
            $scope.departureFlightConfig.loading = true;
            $scope.departureFlightConfig.loadingFlight = true;
            targetScope.loading = true;
            targetScope.loadingFlight = true;

            // get flight
            if (targetScope.progress < 100) {
                //Check Authorization
                var authAccess = getAuthAccess();
                if (authAccess == 1 || authAccess == 2) {
                    $http({
                        method: 'GET',
                        url: FlightSearchConfig.Url + targetScope.flightSearchParams + '/' + targetScope.progress,
                        headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                    }).then(function (returnData) {

                        // set search ID
                        targetScope.searchId = targetScope.flightSearchParams;

                        if (!$scope.pageConfig.expiry.time) {
                            $scope.pageConfig.expiry.time = returnData.data.expTime;
                            $scope.departureFlightConfig.finalProgress = targetScope.progress;
                        }

                        if (targetScope.progress < 100) {
                            targetScope.finalProgress = targetScope.progress; // change this
                            $scope.departureFlightConfig.finalProgress = targetScope.progress;
                        }

                        targetScope.progress = returnData.data.progress;
                        $scope.departureFlightConfig.progress = returnData.data.progress;

                        // generate flight
                        if (returnData.data.flights.length) {
                            if (returnData.data.flights[0].options.length) {
                                $scope.arrangeFlightData('departure', returnData.data.flights[0].options); // For Departure Flight
                                $scope.populateAirlines('departure');
                                $scope.populatePrice('departure');
                                $log.debug("Ada data");
                            }
                            if (returnData.data.flights[1].options.length) {
                                $scope.arrangeFlightData('return', returnData.data.flights[1].options); // For Return Flight
                                $scope.populateAirlines('return');
                                $scope.populatePrice('return');
                                $log.debug("Ada data");
                            }
                        }
                        
                        if (targetScope.progress == 100)
                        {
                            $scope.generateFlightList('departure');
                            $scope.generateFlightList('return');
                        }

                        $log.debug(returnData);

                        // loop the function
                        setTimeout(function () {
                            $scope.getFlight('return');
                        }, 1000);

                        // if error     
                    }).catch(function (returnData) {
                        $scope.trial++;
                        if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                        {
                            $scope.getFlight('return');
                        }
                        else
                        {
                            $log.debug('Failed to get ' + targetScope.name + ' flight list');
                            $log.debug('ERROR :');
                            targetScope.progress = 100;
                            targetScope.finalProgress = 100;
                        }
                    });
                }
                else {
                    targetScope.progress = 100;
                    targetScope.finalProgress = 100;
                    $log.debug('Not Authorized');
                }

                
            } else {
                $log.debug('complete getting flight for '+targetScope.name);
                targetScope.loading = false;
                targetScope.loadingFlight = false;
                $scope.departureFlightConfig.loading = false;
                $scope.departureFlightConfig.loadingFlight = false;
            }
        }

        // arrange flight
        $scope.arrangeFlightData = function (targetScope, data) {       
            if (targetScope == "departure" || targetScope == "Departure") {
                targetScope = $scope.departureFlightConfig;
            } else {
                targetScope = $scope.returnFlightConfig;
            }

            var startNumber = targetScope.flightList.length;

            for (var i = 0; i < data.length; i++) {
                data[i].Available = true;
                data[i].IndexNo = (startNumber + i);
                targetScope.flightList.push(data[i]);
            }

        }

        $scope.generateFlightList = function (targetScope)
        {
            if (targetScope == "departure" || targetScope == "Departure") {
                targetScope = $scope.departureFlightConfig;
            } else {
                targetScope = $scope.returnFlightConfig;
            }
            // start expiry date
            $scope.pageConfig.expiry.start();

            // loop the result
            for (var i = 0; i < targetScope.flightList.length; i++) {
                targetScope.flightList[i].Available = true;
                // fare rule
                targetScope.flightList[i].FareRules = {
                    Loaded: false,
                    Content: ''
                };
            }
        }
     
    }

]);// flight controller
