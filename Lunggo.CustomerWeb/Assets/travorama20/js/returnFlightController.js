// travorama angular app - Flight Controller

app.controller('returnFlightController', [
    '$http', '$scope', '$interval', '$timeout', function($http, $scope, $interval, $timeout) {

        // ******************************
        // on document ready
        angular.element(document).ready(function() {
            // $scope.getDepartureFlight();
            // $scope.getReturnFlight();
            $scope.getFlight('return');

            //$scope.ProgressAnimation('departure');
            //$scope.ProgressAnimation('return');
        });

        $scope.ProgressAnimation = function (targetScope) {
            targetScope = 'departure' ? $scope.departureFlightConfig : $scope.returnFlightConfig;
            $interval(function () {
                if (targetScope.flightSearchParams.FinalProgress < targetScope.flightSearchParams.MaxProgress) {
                    targetScope.flightSearchParams.FinalProgress = targetScope.flightSearchParams.FinalProgress + 1;
                }
            }, $scope.ProgressDuration);
        };

        $scope.ProgressAnimation = function (targetScope, delayTime) {
            targetScope = 'departure' ? $scope.departureFlightConfig : $scope.returnFlightConfig;
            delayTime = delayTime || 1000;
            var randomTime = (Math.random()) * 3000;
            $timeout(function () {
                //console.log(delayTime);
                if (targetScope.flightSearchParams.FinalProgress < (targetScope.flightSearchParams.MaxProgress - 1) && targetScope.flightSearchParams.FinalProgress <= 100) {
                    targetScope.flightSearchParams.FinalProgress = targetScope.flightSearchParams.FinalProgress + 1;
                    $scope.ProgressAnimation(targetScope.name, randomTime);
                }
            }, delayTime);
        };

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
                value: 'TotalFare',
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
                value: 'TotalFare',
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
                departureDate = Date.UTC(departureDate.getUTCFullYear(), (departureDate.getUTCMonth() + 1), departureDate.getUTCDate());
                arrivalDate = Date.UTC(arrivalDate.getUTCFullYear(), (arrivalDate.getUTCMonth() + 1), arrivalDate.getUTCDate());
                var overday = Math.floor((arrivalDate - departureDate) / (1000 * 3600 * 24));
                if (overday > 0) {
                    overday = '+' + overday;
                }
                return overday;
            }
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
        $scope.selectFlight = function (departureFlightIndexNo, returnFlightIndexNo) {

            if ($scope.pageConfig.flightsValidated) {

                console.log('flight available. Will be redirected within');
                $scope.pageConfig.redirectingPage = true;
                var fareToken = $scope.departureFlightConfig.validateToken + '.' + $scope.returnFlightConfig.validateToken;
                $scope.pageConfig.fareToken = fareToken;
                console.log($scope.pageConfig.fareToken);
                $('.pushToken .fareToken').val(fareToken);
                $('.pushToken').submit();

            } else {

                // select flight
                console.log('validating departure flight no : ' + departureFlightIndexNo + ',return flight no : ' + returnFlightIndexNo + 'SearchId : ' + $scope.returnFlightConfig.searchId);
                //console.log('Dep Req : ' + $scope.departureFlightConfig.flightList[departureFlightIndexNo].reg + ':::: Ret Req :' + $scope.returnFlightConfig.flightList[returnFlightIndexNo].reg);
                $scope.departureFlightConfig.validating = true;
                $scope.returnFlightConfig.validating = true;
                $http.post(SelectConfig.Url, {
                    searchId: $scope.returnFlightConfig.searchId,
                    regs: [$scope.departureFlightConfig.flightList[departureFlightIndexNo].reg, $scope.returnFlightConfig.flightList[returnFlightIndexNo].reg],
                }).success(function (returnData) {
                    $scope.departureFlightConfig.validating = false;
                    $scope.returnFlightConfig.validating = false;
                    if ((returnData.token != "" || returnData.token != null) && returnData.status == "200") {
                        console.log('flight available');
                        $scope.departureFlightConfig.validateToken = returnData.token;
                        $scope.departureFlightConfig.validateAvailable = true;
                        $scope.returnFlightConfig.validateToken = returnData.token;
                        $scope.returnFlightConfig.validateAvailable = true;
                        if ($scope.returnFlightConfig.validating == false) {
                            flightsValidated();
                        }
                    }
                    else
                    {
                        console.log('flight unavailable');
                        $scope.departureFlightConfig.validateAvailable = false;
                        $scope.returnFlightConfig.validateAvailable = false;
                    }
                }).error(function (data) {
                    $scope.departureFlightConfig.validating = false;
                    $scope.returnFlightConfig.validating = false;
                    console.log('ERROR Validating Flight :');
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
                        var fareToken = $scope.departureFlightConfig.validateToken;;
                        console.log('Token : '+fareToken);
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
                    targetFlight.flightSort.value = 'fare';
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
                if (!targetScope.loading && !targetScope.loadingFlight) {
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
                } else {
                    return flight;
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
                if (!targetScope.loading && !targetScope.loadingFlight) {
                    if (flight.fare >= targetScope.flightFilter.price.current[0] && flight.fare <= targetScope.flightFilter.price.current[1]) {
                        return flight;
                    }
                } else {
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
                    targetScope.flightFilter.airline.value.push(targetScope.flightFilter.airline.list[i].code);
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
            return function (flight) {
                if (!targetScope.loading && !targetScope.loadingFlight) {
                    if ($scope.getHour(flight.trips[0].segments[0].departureTime) >= parseInt(targetScope.flightFilter.time.departure[0])
                        && $scope.getHour(flight.trips[0].segments[0].departureTime) <= parseInt(targetScope.flightFilter.time.departure[1])
                        && $scope.getHour(flight.trips[0].segments[flight.trips[0].segments.length - 1].arrivalTime) >= parseInt(targetScope.flightFilter.time.arrival[0])
                        && $scope.getHour(flight.trips[0].segments[flight.trips[0].segments.length - 1].arrivalTime) <= parseInt(targetScope.flightFilter.time.arrival[1]))
                    {
                        return flight;
                    }
                } else {
                    return flight;
                }
            }
        }

        // ******************************
        // get flights
        $scope.getFlight = function (targetScope) {

            targetScope = $scope.returnFlightConfig;
            $scope.departureFlightConfig.loading = true;
            $scope.departureFlightConfig.loadingFlight = true;
            targetScope.loading = true;
            targetScope.loadingFlight = true;

            // get flight
            console.log('Getting flight for : ' + '/' + targetScope.flightSearchParams + '/' + targetScope.progress);
            if (targetScope.progress < 100) {
                $http.get(FlightSearchConfig.Url + '/' + targetScope.flightSearchParams + '/' + targetScope.progress, {

                }).success(function (returnData) {

                    // set search ID
                    //targetScope.flightSearchParams.SearchId = targetScope.flightSearchParams;
                    targetScope.searchId = targetScope.flightSearchParams;

                    if (!$scope.pageConfig.expiry.time) {
                        $scope.pageConfig.expiry.time = returnData.expTime;
                        $scope.departureFlightConfig.finalProgress = targetScope.progress;
                    }

                    if (targetScope.progress < 100) {
                        targetScope.finalProgress = targetScope.progress; // change this
                        $scope.departureFlightConfig.finalProgress = targetScope.progress;
                    }
                    
                    targetScope.progress = returnData.progress;
                    $scope.departureFlightConfig.progress = returnData.progress;

                    // generate flight
                    if (returnData.flights.length)
                    {
                        if (returnData.flights[0].options.length)
                        {
                            $scope.arrangeFlightData('departure', returnData.flights[0].options); // For Departure Flight
                        }
                        if (returnData.flights[1].options.length)
                        {
                            $scope.arrangeFlightData('return', returnData.flights[1].options); // For Return Flight
                        }
                    }
                    

                    console.log(returnData);

                    // loop the function
                    setTimeout(function () {
                        $scope.getFlight('return');
                    }, 1000);

                    // if error     
                }).error(function (returnData) {
                    console.log('Failed to get ' + targetScope.name + ' flight list');
                    console.log('ERROR :');
                    console.log(returnData);
                    targetScope.progress = 100;
                    targetScope.finalProgress = 100;
                });
            } else {
                console.log('complete getting flight for '+targetScope.name);
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

            if (targetScope.progress == 100) {

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

                    // populate prices
                    targetScope.flightFilter.price.prices.push(targetScope.flightList[i].fare);

                    // populate airline code
                    targetScope.flightList[i].AirlinesTag = [];
                    for (var x = 0; x < targetScope.flightList[i].trips[0].airlines.length; x++) {
                        targetScope.flightFilter.airline.airlines.push(targetScope.flightList[i].trips[0].airlines[x]);
                        targetScope.flightList[i].AirlinesTag.push(targetScope.flightList[i].trips[0].airlines[x].code);
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
                if (!dupes[el.code]) {
                    dupes[el.code] = true;
                    targetScope.flightFilter.airline.list.push(el);
                }
            });
            targetScope.flightFilter.airline.airlines = [];

            // activate price filter
            $('.'+targetFlight+'-price-slider').slider({
                range: true,
                min: targetScope.flightFilter.price.initial[0],
                max: targetScope.flightFilter.price.initial[1],
                step: 100,
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

            targetScope.progress = 100;

            console.log('Completed setting flight filtering for : ' + targetScope.name);
            console.log(targetScope);

        }

    }
]);// flight controller

