// travorama angular app - Flight Controller

app.controller('singleFlightController', [
    '$http', '$scope', '$interval', '$timeout', '$log', function ($http, $scope, $interval, $timeout, $log) {

        // **********
        // on document ready
        angular.element(document).ready(function () {
            $scope.getFlight();
        });

        // **********
        // general variables
        $scope.pageLoaded = true;
        $scope.notice = true;
        $scope.trial = 0;
        $scope.busy = false;
        $scope.loading = false;
        $scope.loadingFlight = false;
        $scope.flightList = [];
        $scope.Progress = 0;
        $scope.returnUrl = "/";
        var cabin = FlightSearchConfig.flightForm.cabin;
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
        var departureDate = new Date(FlightSearchConfig.flightForm.departureDate) || '';
        var origin = FlightSearchConfig.flightForm.origin;
        var destination = FlightSearchConfig.flightForm.destination;
        var originCity = FlightData.OriginCity;
        var destinationCity = FlightData.DestinationCity;
        $scope.originCity = FlightData.OriginCity;
        $scope.destinationCity = FlightData.DestinationCity;
        var passengerParam = FlightSearchConfig.flightForm.passenger.adult + '' + FlightSearchConfig.flightForm.passenger.child + '' + FlightSearchConfig.flightForm.passenger.infant + '' + cabin;
        var departureParam = (origin + destination) + ((('0' + departureDate.getDate()).slice(-2)) + (('0' + (departureDate.getMonth() + 1)).slice(-2)) + (departureDate.getFullYear().toString().substr(2, 2)));
        $scope.flightFixRequest = departureParam + '-' + passengerParam;
        $scope.pristine = true;

        $scope.gtmContentType = gtmContentType;
        $scope.gtmDepartingDepartureDate = gtmDepartingDepartureDate;
        $scope.gtmReturningDepartureDate = gtmReturningDepartureDate;
        $scope.gtmOriginAirport = gtmOriginAirport;
        $scope.gtmDestinationAirport = gtmDestinationAirport;
        $scope.gtmDepartingArrivalDate = gtmDepartingArrivalDate;
        $scope.gtmReturningArrivalDate = gtmReturningArrivalDate;
        $scope.gtmNumAdults = gtmNumAdults;
        $scope.gtmNumChildren = gtmNumChildren;
        $scope.gtmNumInfants = gtmNumInfants;
        $scope.gtmTravelClass = gtmTravelClass;
        $scope.gtmPurchaseCurrency = gtmPurchaseCurrency;
        
        $scope.flightRequest = {
            CabinClass: FlightSearchConfig.flightForm.cabin,
            AdultCount: FlightSearchConfig.flightForm.passenger.adult,
            ChildCount: FlightSearchConfig.flightForm.passenger.child,
            InfantCount: FlightSearchConfig.flightForm.passenger.infant,
            TripType: FlightSearchConfig.flightForm.type,
            Trips: FlightSearchConfig.flightForm.trips,
            Requests: FlightSearchConfig.flightForm.Requests,
            Completed: [],
            Progress: 0,
            FinalProgress: 0,
            ProgressDuration: 1000,
            MaxProgress: 0,
            SecureCode: FlightSearchConfig.flightForm.SecureCode
        };
        $scope.expiry = {
            expired: false,
            time: '',
            start: function () {
                var expiryTime = new Date($scope.expiry.time);
                if ($scope.expiry.expired) return;
                $interval(function () {
                    var nowTime = new Date();
                    if (nowTime > expiryTime) {
                        $scope.expiry.expired = true;
                    }
                }, 1000);
            }
        }
        $scope.flightSelected = -1;
        $scope.overviewDetailShown = false;

        // **********
        // general functions

        // toggle detail
        $scope.toggleOverviewDetail = function () {
            if ($scope.overviewDetailShown == true) {
                $scope.overviewDetailShown = false;
            } else {
                $scope.overviewDetailShown = true;
            }
        }

        // close overview
        $scope.closeOverview = function () {
            $scope.flightSelected = -1;

            $scope.selectFlightParam = {
                validated: false,
                validating: false,
                available: false,
                newFare: false
            };

            $('body').removeClass('no-scroll');
        }

        // close notice
        $scope.closeNotice = function () {
            $scope.notice = false;
        }

        // milisecond to hour
        $scope.msToTime = function (duration) {

            var milliseconds = parseInt((duration % 1000) / 100),
                 seconds = parseInt((duration / 1000) % 60),
                 minutes = parseInt((duration / (1000 * 60)) % 60),
                 hours = parseInt((duration / (1000 * 60 * 60)));
            hours = hours;
            minutes = minutes;
            seconds = seconds;
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

        $scope.getOverdayDate = function (departureDate, arrivalDate) {
            if (departureDate && arrivalDate) {
                departureDate = new Date(departureDate);
                arrivalDate = new Date(arrivalDate);
                departureDate = Date.UTC(departureDate.getUTCFullYear(), (departureDate.getUTCMonth()), departureDate.getUTCDate());
                arrivalDate = Date.UTC(arrivalDate.getUTCFullYear(), (arrivalDate.getUTCMonth()), arrivalDate.getUTCDate());
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

        // **********
        // flight detail function
        $scope.priceDetailSelected = false;
        $scope.flightDetailSelected = false;
        $scope.priceActiveIndex = -1;
        $scope.flightActive = -1;

        $scope.setFlightActive = function (flightNo) {
            $scope.flightDetailSelected = true;
            $scope.priceDetailSelected = false;
            if ($scope.flightActive != flightNo) {
                $scope.flightActive = flightNo;
                $scope.priceActiveIndex = -1;
            }
            else {
                $scope.flightActive = -1;
            }
        }

        $scope.showPriceDetail = function (index) {
            $scope.flightDetailSelected = false;
            $scope.priceDetailSelected = true;
            if ($scope.priceActiveIndex != index) {
                $scope.priceActiveIndex = index;
                $scope.flightActive = -1;
            }
            else {
                $scope.priceActiveIndex = -1;
            }
        }

        // **********
        // flight sort function
        $scope.sort = {
            label: 'price',
            value: 'netAdultFare',
            reverse: false
        };
        $scope.setSort = function (sort) {
            if ($scope.sort.label == sort) {
                if ($scope.sort.reverse == false) {
                    $scope.sort.reverse = true;
                } else {
                    $scope.sort.reverse = false;
                }
            } else {
                $scope.sort.raverse = false;
            }
            $scope.sort.label = sort;
            $scope.selectedItem = -1;
            $scope.selectedRules = -1;
            switch (sort) {
                case 'airline':
                    $scope.sort.value = 'trips[0].airlines[0].name';
                    break;
                case 'departure':
                    $scope.sort.value = 'trips[0].segments[0].departureTime';
                    break;
                case 'duration':
                    $scope.sort.value = 'trips[0].totalDuration';
                    break;
                case 'arrival':
                    $scope.sort.value = 'trips[0].segments[(trips[0].segments.length-1)].arrivalTime';
                    break;
                case 'price':
                    $scope.sort.value = 'netAdultFare'; //originalAdultFare
                    break;
            }
        }

        // **********
        // flight filter function

        // available filter
        $scope.availableFilter = function (flight) {
            if (flight.Available) {
                return flight;
            }           
        }

        // stop filter
        $scope.stopFilterParam = {
            direct: true,
            one: true,
            two: true
        };

        $scope.stopFilter = function (flight) {
            if ($scope.stopFilterParam.direct) {
                if (flight.trips[0].transitCount == 0) {
                    return flight;
                }
            }
            if ($scope.stopFilterParam.one) {
                if (flight.trips[0].transitCount == 1) {
                    return flight;
                }
            }
            if ($scope.stopFilterParam.two) {
                if (flight.trips[0].transitCount > 1) {
                    return flight;
                }
            }
        }

        // price filter
        $scope.priceFilterParam = {
            initial: [-1, -1],
            current: [-1, -1],
            prices: []
        };
        $scope.priceFilter = function (flight) {
            if (flight.netAdultFare >= $scope.priceFilterParam.current[0] && flight.netAdultFare <= $scope.priceFilterParam.current[1]) {
                return flight;
            }
        }

        // airline filter
        $scope.airlineFilterParam = {
            airlinesList: [],
            airlines: [],
            selected: [],
            noneSelected: false,
            pure: true
        };
        $scope.checkAirline = function () {
            $scope.airlineFilterParam.pure = false;
            $scope.airlineFilterParam.selected = [];
            for (var i = 0; i < $scope.airlineFilterParam.airlines.length; i++) {
                if ($scope.airlineFilterParam.airlines[i].checked == true) {
                    $scope.airlineFilterParam.selected.push($scope.airlineFilterParam.airlines[i].name);
                }
            }
        }
        $scope.setAirlineFilter = function (target) {
            for (var i = 0; i < $scope.airlineFilterParam.airlines.length; i++) {
                if (target == 'all') {
                    $scope.airlineFilterParam.airlines[i].checked = true;
                } else {
                    $scope.airlineFilterParam.airlines[i].checked = false;
                }
            }
            $scope.checkAirline();
        }

        $scope.airlineFilter = function (flight) {
            if ($scope.airlineFilterParam.pure == true) {
                return flight;
            }
            else if ($scope.airlineFilterParam.selected.length < 1) {
                return flight;
            }
            else {
                for (var i = 0; i < flight.AirlinesTag.length; i++) {
                    if ($scope.airlineFilterParam.selected.indexOf(flight.AirlinesTag[i]) != -1) {
                        return flight;
                    }
                }
            }
        }

        // time filter
        $scope.getMinute = function (dateTime) {
            var hour = dateTime.substr(11, 2);
            var minute = dateTime.substr(14, 2);
            return parseInt(hour) *60 + parseInt(minute);
        }
        $scope.timeFilterParam = {
            departure: [0, 24],
            arrival: [0, 24]
        };
        $scope.timeFilter = function (flight) {
            if ($scope.getMinute(flight.trips[0].segments[0].departureTime) >= parseInt($scope.timeFilterParam.departure[0]) * 60
                && $scope.getMinute(flight.trips[0].segments[0].departureTime) <= parseInt($scope.timeFilterParam.departure[1]) * 60
                && $scope.getMinute(flight.trips[0].segments[flight.trips[0].segments.length - 1].arrivalTime) >= parseInt($scope.timeFilterParam.arrival[0]) * 60
                && $scope.getMinute(flight.trips[0].segments[flight.trips[0].segments.length - 1].arrivalTime) <= parseInt($scope.timeFilterParam.arrival[1]) * 60
            ) {
                return flight;
            }
        }


        // **********
        // Select flight
        $scope.selectFlightParam = {
            error: false,
            validated: false,
            token: '',
            validating: false,
            available: false,
            newFare: false,
            proceed: false,
            popup: false
        };
        $scope.selectFlight = function (indexNo) {
            if ($scope.trial > 3) {
                $scope.trial = 0;
            }
            $('body').addClass('no-scroll');

            $log.debug('---------------------');
            $log.debug('Selecting Flight no : ' + indexNo);
            $scope.selectFlightParam.popup = true;
            if (!$scope.selectFlightParam.validated) {

                $scope.flightSelected = indexNo;
                $scope.selectFlightParam.validating = true;

                //Check Authorization
                var authAccess = getAuthAccess();
                if (authAccess == 1 || authAccess == 2) {
                    // Select flight
                    $http({
                        method: 'POST',
                        url: SelectConfig.Url,
                        data: {
                            searchId: SelectConfig.SearchId,
                            regs: [$scope.flightList[indexNo].reg]
                        },
                        headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                    }).then(function (returnData) {
                        $scope.selectFlightParam.validated = true;
                        $log.debug(indexNo);
                        $log.debug("Response Select Flight : " + returnData);

                        if (returnData.data.token != "" || returnData.data.token != null) {
                            $log.debug('departure flight available');
                            $scope.selectFlightParam.available = true;
                            $scope.selectFlightParam.token = returnData.data.token;
                            $('.push-token input').val($scope.selectFlightParam.token);
                            $scope.selectSubmit();
                        }
                    }).catch(function (returnData) {
                        $scope.trial++;
                        if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                        {
                            $scope.selectFlight($scope.flightSelected);
                        }
                        else {
                            $scope.selectFlightParam.validatingFlight = false;
                            $log.debug('ERROR Validating Flight');
                            $log.debug('--------------------');
                            $scope.selectFlightParam.popup = false;
                            $scope.selectFlightParam.error = true;
                        }
                    });
                }
                else {
                    $scope.selectFlightParam.validatingFlight = false;
                    $log.debug('Not Authorized');
                    $scope.selectFlightParam.popup = false;
                    $scope.selectFlightParam.error = true;
                }
            } else {
                $scope.selectFlightParam.proceed = true;
                $('.push-token').submit();
            }
        }
        $scope.selectSubmit = function () {
            $scope.selectFlightParam.proceed = true;
            $('.push-token').submit();
        }
        $scope.selectCancel = function () {
            $scope.loading = false;
            $scope.selectFlightParam.validated = false;
            $scope.selectFlightParam.newFare = false;
            $scope.selectFlightParam.available = false;
            $('body').removeClass('no-scroll');
        }

        $scope.listPrices = [];
        // get flight function
        $scope.getFlight = function () {
            $scope.busy = true;
            $scope.loading = true;
            $scope.loadingFlight = true;
            if ($scope.trial > 3) {
                $scope.trial = 0;
            }

            if ($scope.Progress < 100) {
                // **********
                // ajax
                //Check Authorization
                var authAccess = getAuthAccess();
                if (authAccess == 1 || authAccess == 2) {
                    $http({
                        method: 'GET',
                        url: FlightSearchConfig.Url + $scope.flightFixRequest + '/' + $scope.Progress,
                        headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                    }).then(function (returnData) {
                        // set searchID
                        SelectConfig.SearchId = $scope.flightFixRequest;
                        $scope.flightRequest.SearchId = $scope.flightFixRequest;

                        $scope.Progress = returnData.data.progress;
                        if (returnData.data.flights.length != 0) {
                            $scope.generateFlightList(returnData.data.flights[0].options);
                            $log.debug("Ada data");
                            for (var x = 0; x < returnData.data.flights[0].options.length; x++) {
                                $scope.listPrices.push(parseInt(returnData.data.flights[0].options[x].netTotalFare));
                            }
                            $scope.generateFilterFlight();
                        }

                        if ($scope.Progress == 100) {
                            $scope.expiry.time = returnData.data.expTime;
                            $scope.flightRequest.FinalProgress = $scope.Progress;
                        } else {
                            $scope.flightRequest.FinalProgress = $scope.Progress;
                        }

                        $log.debug('Progress : ' + $scope.Progress + ' %');
                        $log.debug(returnData);

                        // loop the function
                        setTimeout(function () {
                            $scope.getFlight();
                        }, 1000);

                    }).catch(function (returnData) {
                        $log.debug('Failed to get flight list');
                        $scope.trial++;
                        if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                        {
                            $scope.getFlight();
                        }
                        else {
                            for (var i = 0; i < $scope.flightRequest.Requests.length; i++) {
                                // add to completed
                                if ($scope.flightRequest.Completed.indexOf($scope.flightRequest.Requests[i] < 0)) {
                                    $scope.flightRequest.Completed.push($scope.flightRequest.Requests[i]);
                                }
                                // check current request. Remove if completed
                                if ($scope.flightRequest.Requests.indexOf($scope.flightRequest.Requests[i] < 0)) {
                                    $scope.flightRequest.Requests.splice($scope.flightRequest.Requests.indexOf($scope.flightRequest.Requests[i]), 1);
                                }
                            }
                            $scope.flightRequest.Progress = 100;
                            $scope.flightRequest.FinalProgress = 100;
                        }
                    });
                }
                else {
                    $scope.busy = false;
                    $scope.loading = false;
                    $scope.loadingFlight = false;
                    $log.debug('Not Authorized');
                }
            } else {
                $log.debug('Finished getting flight list !');
                $scope.busy = false;
                $scope.loading = false;
                $scope.loadingFlight = false;

                !function (f, b, e, v, n, t, s) {
                    if (f.fbq) return; n = f.fbq = function () {
                        n.callMethod ?
                        n.callMethod.apply(n, arguments) : n.queue.push(arguments)
                    }; if (!f._fbq) f._fbq = n;
                    n.push = n; n.loaded = !0; n.version = '2.0'; n.queue = []; t = b.createElement(e); t.async = !0;
                    t.src = v; s = b.getElementsByTagName(e)[0]; s.parentNode.insertBefore(t, s)
                }(window, document, 'script', '//connect.facebook.net/en_US/fbevents.js');

                //fbq('init', '<FB_PIXEL_ID>');
                var lowestPrice;

                if ($scope.listPrices.length > 0) {
                    lowestPrice = Math.min.apply(Math, $scope.listPrices);
                } else {
                    lowestPrice = 0;
                }

                fbq('track', 'Search', {
                    content_type: $scope.gtmContentType,
                    departing_departure_date: $scope.gtmDepartingDepartureDate,
                    origin_airport: $scope.gtmOriginAirport,
                    destination_airport: $scope.gtmDestinationAirport,
                    num_adults: $scope.gtmNumAdults,
                    num_children: $scope.gtmNumChildren,
                    num_infants: $scope.gtmNumInfants,
                    travel_class: $scope.gtmTravelClass,
                    purchase_value: lowestPrice,
                    purchase_currency: $scope.gtmPurchaseCurrency,
                });
            }
        }

        // **********
        // generate flight list
        $scope.generateFlightList = function (data) {

            var startNo = $scope.flightList.length;

            for (var i = 0; i < data.length; i++) {
                data[i].Available = true;
                data[i].IndexNo = (startNo + i);
                $scope.flightList.push(data[i]);
            }
        }
        $scope.dupes = {};
        $scope.generateFilterFlight = function () {
            $log.debug('Generating Filter');

            // set expiry time
            $scope.expiry.start();

            // loop the data
            for (var i = 0; i < $scope.flightList.length; i++) {
                // *****
                // add available state on each flight
                $scope.flightList[i].Available = true;

                // *****
                // set default fare rules loaded and content to false
                $scope.flightList[i].FareRule = {
                    Loaded: false,
                    Content: ''
                };

                // *****
                // populate prices
                $scope.priceFilterParam.prices.push($scope.flightList[i].netAdultFare);

                // *****
                // populate airline tag
                $scope.flightList[i].AirlinesTag = [];
                for (var x = 0 ; x < $scope.flightList[i].trips[0].airlines.length; x++) {
                    $scope.airlineFilterParam.airlinesList.push($scope.flightList[i].trips[0].airlines[x]);
                    $scope.flightList[i].AirlinesTag.push($scope.flightList[i].trips[0].airlines[x].name);
                }

            }
            // sort prices and set initial price filter
            function sortNumber(a, b) {
                return a - b;
            }
            $scope.priceFilterParam.prices.sort(sortNumber);
            $scope.priceFilterParam.initial[0] = Math.floor($scope.priceFilterParam.prices[0]);
            $scope.priceFilterParam.initial[1] = Math.round($scope.priceFilterParam.prices[$scope.priceFilterParam.prices.length - 1]);
            $scope.priceFilterParam.current[0] = Math.floor($scope.priceFilterParam.prices[0]);
            $scope.priceFilterParam.current[1] = Math.round($scope.priceFilterParam.prices[$scope.priceFilterParam.prices.length - 1]);

            // remove duplicate from airlines
            
            $.each($scope.airlineFilterParam.airlinesList, function (i, el) {
                if (!$scope.dupes[el.name]) {
                    $scope.dupes[el.name] = true;
                    $scope.airlineFilterParam.airlines.push(el);
                }
            });
            $scope.airlineFilterParam.airlinesList = [];

            // activate price filter
            $('.price-slider').slider({
                range: true,
                min: $scope.priceFilterParam.initial[0],
                max: $scope.priceFilterParam.initial[1],
                step: 100,
                values: [$scope.priceFilterParam.initial[0], $scope.priceFilterParam.initial[1]],
                create: function (event, ui) {
                    $('.price-slider-min').val($scope.priceFilterParam.initial[0]);
                    $('.price-slider-min').trigger('input');
                    $('.price-slider-max').val($scope.priceFilterParam.initial[1]);
                    $('.price-slider-max').trigger('input');
                },
                slide: function (event, ui) {
                    $('.price-slider-min').val(ui.values[0]);
                    $('.price-slider-min').trigger('input');
                    $scope.priceFilterParam.current[0] = ui.values[0];
                    $('.price-slider-max').val(ui.values[1]);
                    $scope.priceFilterParam.current[1] = ui.values[1];
                    $('.price-slider-max').trigger('input');
                }
            });

            // activate time slider
            $('.departure-slider').slider({
                range: true,
                min: 0, max: 24, step: 1, values: [0, 24],
                create: function (event, ui) {
                    $('.departure-slider-min').val(0);
                    $('.departure-slider-min').trigger('input');
                    $('.departure-slider-max').val(24);
                    $('.departure-slider-max').trigger('input');
                },
                slide: function (event, ui) {
                    $('.departure-slider-min').val(ui.values[0]);
                    $scope.timeFilterParam.departure[0] = ui.values[0];
                    $('.departure-slider-min').trigger('input');
                    $('.departure-slider-max').val(ui.values[1]);
                    $scope.timeFilterParam.departure[1] = ui.values[1];
                    $('.departure-slider-max').trigger('input');
                }
            });
            $('.arrival-slider').slider({
                range: true,
                min: 0, max: 24, step: 1, values: [0, 24],
                create: function (event, ui) {
                    $('.arrival-slider-min').val(0);
                    $('.arrival-slider-min').trigger('input');
                    $('.arrival-slider-max').val(24);
                    $('.arrival-slider-max').trigger('input');
                },
                slide: function (event, ui) {
                    $('.arrival-slider-min').val(ui.values[0]);
                    $scope.timeFilterParam.arrival[0] = ui.values[0];
                    $('.arrival-slider-min').trigger('input');
                    $('.arrival-slider-max').val(ui.values[1]);
                    $scope.timeFilterParam.arrival[1] = ui.values[1];
                    $('.arrival-slider-max').trigger('input');
                }
            });

            $scope.flightRequest.FinalProgress = 100;
            $log.debug('Completed setting flight and filter');
            $log.debug($scope);
        }


        $scope.listPrices = [];
        $scope.ready = false;

        $scope.getPriceCalendar = function() {
            var todayDate = new Date();
            var startDate = ("0" + todayDate.getDate()).slice(-2)
             + ("0" + (todayDate.getMonth() + 1)).slice(-2) + todayDate.getFullYear().toString().substring(2, 4);
            var endDate = '3112' + todayDate.getFullYear().toString().substring(2, 4);

            var authAccess = getAuthAccess();
            if (authAccess == 1 || authAccess == 2) {
                $.ajax({
                    url: FlightPriceCalendarConfig.Url + '/' + origin +
                        destination + '/' + startDate
                    + '/' + endDate + '/IDR',
                    method: 'GET',
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).done(function (returnData) {
                    $scope.listPrices = returnData.listDatesAndPrices;
                    $scope.initWeek();
                }).error(function (returnData) {
                });
            }
        }
        
        $scope.getPriceCalendar();
        $scope.weeklyPrice = [];
        $scope.index = null;

        $scope.initWeek = function() {
            var stringDate = departureDate.getFullYear().toString() + '/' +
            ("0" + (departureDate.getMonth() + 1)).slice(-2) + '/' + ("0" + departureDate.getDate()).slice(-2);

            for (var x = 0; x < $scope.listPrices.length; x++) {
                if ($scope.listPrices[x].date == stringDate) {
                    $scope.index = x;
                    $scope.selectWeek($scope.index);
                    break;
                }
            }
        }

        $scope.returnMonth = function (val) {
            if (val == '1' || val == '01')
                return "Jan";
            else if (val == '2' || val == '02')
                return "Feb";
            else if (val == '3' || val == '03')
                return "Mar";
            else if (val == '4' || val == '04')
                return "Apr";
            else if (val == '5' || val == '05')
                return "Mei";
            else if (val == '6' || val == '06')
                return "Jun";
            else if (val == '7' || val == '07')
                return "Jul";
            else if (val == '8' || val == '08')
                return "Agu";
            else if (val == '9' || val == '09')
                return "Sep";
            else if (val == '10' || val == '10')
                return "Okt";
            else if (val == '11' || val == '11')
                return "Nov";
            else if (val == '12' || val == '12')
                return "Des";
        }

        $scope.returnDay = function(d) {
            if (d == 0) {
                return 'Minggu';
            }
            else if (d == 1) {
                return 'Senin';
            }
            else if (d == 2) {
                return 'Selasa';
            }
            else if (d == 3) {
                return 'Rabu';
            }
            else if (d == 4) {
                return 'Kamis';
            }
            else if (d == 5) {
                return 'Jumat';
            }
            else if (d == 6) {
                return 'Sabtu';
            }
        }

        $scope.editUrl = function () {
            originCity = originCity.replace(/\s+/g, '-');
            originCity = originCity.replace(/[^0-9a-zA-Z-]/gi, '');
            destinationCity = destinationCity.replace(/\s+/g, '-');
            destinationCity = destinationCity.replace(/[^0-9a-zA-Z-]/gi, '');
            return '/id/tiket-pesawat/cari/' + originCity + '-' + destinationCity + '-' +
                origin + '-' + destination + '/' + origin
               + destination;
        }

        $scope.selectWeek = function (index) {
            $scope.weeklyPrice = [];
            if (index < 3) {
                for (var x = 0; x < 7; x++) {
                    var date = new Date($scope.listPrices[x].date);
                    var tanggal = ("0" + date.getDate()).slice(-2) + ("0" + (date.getMonth() + 1)).slice(-2) + date.getFullYear().toString().slice(-2);
                    $scope.weeklyPrice.push({
                        price: $scope.listPrices[x].price,
                        date: $scope.listPrices[x].date.split('/')[2] + ' ' + $scope.returnMonth($scope.listPrices[x].date.split('/')[1]),
                        day: $scope.returnDay(date.getDay()),
                        url: $scope.editUrl() + tanggal + '-' + $scope.flightRequest.AdultCount.toString() + $scope.flightRequest.ChildCount.toString()
                        + $scope.flightRequest.InfantCount.toString() + cabin
                });
                }
            }
            else if (index > $scope.listPrices.length - 4) {
                for (var x = $scope.listPrices.length - 7; x < $scope.listPrices.length; x++) {
                    var date = new Date($scope.listPrices[x].date);
                    var tanggal = ("0" + date.getDate()).slice(-2) + ("0" + (date.getMonth() + 1)).slice(-2) + date.getFullYear().toString().slice(-2);
                    $scope.weeklyPrice.push({
                        price: $scope.listPrices[x].price,
                        date: $scope.listPrices[x].date.split('/')[2] + ' ' + $scope.returnMonth($scope.listPrices[x].date.split('/')[1]),
                        day: $scope.returnDay(date.getDay()),
                        url: $scope.editUrl() + tanggal + '-' + $scope.flightRequest.AdultCount.toString() + $scope.flightRequest.ChildCount.toString()
                        + $scope.flightRequest.InfantCount.toString() + cabin
                    });
                }
            } else {
                for (var x = index - 3; x < index + 4; x++) {
                    var date = new Date($scope.listPrices[x].date);
                    var tanggal = ("0" + date.getDate() ).slice(-2) + ("0" + (date.getMonth() + 1)).slice(-2) + date.getFullYear().toString().slice(-2);
                    $scope.weeklyPrice.push({
                        price: $scope.listPrices[x].price,
                        date: $scope.listPrices[x].date.split('/')[2] + ' ' + $scope.returnMonth($scope.listPrices[x].date.split('/')[1]),
                        day: $scope.returnDay(date.getDay()),
                        url: $scope.editUrl() + tanggal + '-' + $scope.flightRequest.AdultCount.toString() + $scope.flightRequest.ChildCount.toString()
                        + $scope.flightRequest.InfantCount.toString() + cabin
                    });
                }
            }
            $scope.ready = true;
        }

        $scope.next = function () {
            if ($scope.index + 7 <= $scope.listPrices.length) {
                $scope.index += 7;
            } else {
                $scope.index = $scope.listPrices.length - 4;
            }
            $scope.selectWeek($scope.index);
        }
        
        $scope.prev = function () {
            if ($scope.index - 7 >= 0) {
                $scope.index -= 7;
            } else {
                $scope.index = 3;
            }
            $scope.selectWeek($scope.index);
        }

        $scope.goto = function(link) {
            window.location.href = link;
        }
    }
]);// flight controller
