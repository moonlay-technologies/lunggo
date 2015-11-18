// travorama angular app - Flight Controller

app.controller('singleFlightController', [
    '$http', '$scope', '$interval', function ($http, $scope, $interval) {

        // **********
        // on document ready
        angular.element(document).ready(function () {
            $scope.getFlight();
        });

        // **********
        // general variables
        $scope.pageLoaded = true;
        $scope.notice = true;
        $scope.busy = false;
        $scope.loading = false;
        $scope.loadingFlight = false;
        $scope.flightList = [];
        $scope.pristine = true;
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
                    if ( nowTime > expiryTime ) {
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
        $scope.toggleOverviewDetail = function() {
            if ($scope.overviewDetailShown == true) {
                $scope.overviewDetailShown = false;
            } else {
                $scope.overviewDetailShown = true;
            }
        }

        // close overview
        $scope.closeOverview = function() {
            $scope.flightSelected = -1;

            $scope.revalidateFlightParam = {
                validated: false,
                validating: false,
                available: false,
                newFare: false
            };

            $('body').removeClass('no-scroll');
        }

        // translate month
        $scope.translateMonth = function (month) {
            switch (month) {
                case 0:
                    month = 'Jan';
                    break;
                case 1:
                    month = 'Feb';
                    break;
                case 2:
                    month = 'Mar';
                    break;
                case 3:
                    month = 'Apr';
                    break;
                case 4:
                    month = 'May';
                    break;
                case 5:
                    month = 'Jun';
                    break;
                case 6:
                    month = 'Jul';
                    break;
                case 7:
                    month = 'Aug';
                    break;
                case 8:
                    month = 'Sep';
                    break;
                case 9:
                    month = 'Oct';
                    break;
                case 10:
                    month = 'Nov';
                    break;
                case 11:
                    month = 'Dec';
                    break;
            }
            return month;
        }

        // close notice
        $scope.closeNotice = function() {
            $scope.notice = false;
        }

        // milisecond to hour
        $scope.msToTime = function(duration) {
            var milliseconds = parseInt((duration % 1000) / 100),
                seconds = parseInt((duration / 1000) % 60),
                minutes = parseInt((duration / (1000 * 60)) % 60),
                hours = parseInt((duration / (1000 * 60 * 60)));
                // hours = parseInt((duration / (1000 * 60 * 60)) % 24);
                // days = parseInt((duration / (1000 * 60 * 60 * 24)));

            hours = hours;
            minutes = minutes;
            seconds = seconds;

            return hours + "h " + minutes + "m";
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

        // **********
        // flight detail function
        $scope.flightActive = -1;
        $scope.setFlightActive = function (flightNo) {
            if ($scope.flightActive == flightNo) {
                $scope.flightActive = -1;
            } else {
                $scope.flightActive = flightNo;
            }
        }

        // **********
        // flight sort function
        $scope.sort = {
            label: 'price',
            value: 'TotalFare',
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
                    $scope.sort.value = 'Trips[0].Airlines[0].Name';
                    break;
                case 'departure':
                    $scope.sort.value = 'Trips[0].Segments[0].DepartureTime';
                    break;
                case 'duration':
                    $scope.sort.value = 'Trips[0].TotalDuration';
                    break;
                case 'arrival':
                    $scope.sort.value = 'Trips[0].Segments[(Trips[0].Segments.length-1)].ArrivalTime';
                    break;
                case 'price':
                    $scope.sort.value = 'TotalFare';
                    break;
            }
        }

        // **********
        // flight filter function
        
        // available filter
        $scope.availableFilter = function (flight) {
            if (!$scope.loading && !$scope.loadingFlight) {
                if (flight.Available) {
                    return flight;
                }
            } else {
                return flight;
            }
        }

        // stop filter
        $scope.stopFilterParam = {
            direct: true,
            one: true,
            two: true
        };
        $scope.stopFilter = function(flight) {
            if ($scope.stopFilterParam.direct) {
                if (flight.Trips[0].TotalTransit == 0) {
                    return flight;
                }
            }
            if ($scope.stopFilterParam.one) {
                if (flight.Trips[0].TotalTransit == 1) {
                    return flight;
                }
            }
            if ($scope.stopFilterParam.two) {
                if (flight.Trips[0].TotalTransit > 1) {
                    return flight;
                }
            }
        }

        // price filter
        $scope.priceFilterParam = {
            initial : [-1,-1],
            current: [-1, -1],
            prices: []
        };
        $scope.priceFilter = function (flight) {
            if (!$scope.loading && !$scope.loadingFlight) {
                if (flight.TotalFare >= $scope.priceFilterParam.current[0] && flight.TotalFare <= $scope.priceFilterParam.current[1]) {
                    return flight;
                }
            } else {
                return flight;
            }
        }

        // airline filter
        $scope.airlineFilterParam = {
            airlinesList : [],
            airlines: [],
            selected : [],
            pure: true
        };
        $scope.checkAirline = function() {
            $scope.airlineFilterParam.pure = false;
            $scope.airlineFilterParam.selected = [];
            for (var i = 0; i < $scope.airlineFilterParam.airlines.length; i++) {
                if ($scope.airlineFilterParam.airlines[i].checked == true) {
                    $scope.airlineFilterParam.selected.push($scope.airlineFilterParam.airlines[i].Code);
                }
            }
        }
        $scope.setAirlineFilter = function(target) {
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
            if (!$scope.loading && !$scope.loadingFlight) {
                if ($scope.airlineFilterParam.pure == true) {
                    return flight;
                } else {
                    for (var i in flight.AirlinesTag) {
                        if ($scope.airlineFilterParam.selected.indexOf(flight.AirlinesTag[i]) != -1) {
                            return flight;
                        }
                    }
                }
            } else {
                return flight;
            }
        }

        // time filter
        $scope.getHour = function(dateTime) {
            dateTime = dateTime.substr(11, 2);
            return parseInt(dateTime);
        }
        $scope.timeFilterParam = {
            departure: [0, 24],
            arrival : [0,24]
        };
        $scope.timeFilter = function(flight) {
            if ($scope.getHour(flight.Trips[0].Segments[0].DepartureTime) >= parseInt($scope.timeFilterParam.departure[0])
                && $scope.getHour(flight.Trips[0].Segments[0].DepartureTime) <= parseInt($scope.timeFilterParam.departure[1])
                && $scope.getHour(flight.Trips[0].Segments[flight.Trips[0].Segments.length - 1].ArrivalTime) >= parseInt($scope.timeFilterParam.arrival[0])
                && $scope.getHour(flight.Trips[0].Segments[flight.Trips[0].Segments.length - 1].ArrivalTime) <= parseInt($scope.timeFilterParam.arrival[1])
            ) {
                return flight;
            }
        }


        // **********
        // revalidate flight
        $scope.revalidateFlightParam = {
            validated: false,
            token: '',
            validating: false,
            available: false,
            newFare: false,
            proceed: false
        };
        $scope.revalidateFlight = function (indexNo) {

            $('body').addClass('no-scroll');

            console.log('---------------------');
            console.log('Validating Flight no : '+indexNo);

            if (!$scope.revalidateFlightParam.validated) {

                $scope.flightSelected = indexNo;
                $scope.revalidateFlightParam.validating = true;

                // revalidate flight
                $http.get(RevalidateConfig.Url, {
                    params: {
                        SearchId: RevalidateConfig.SearchId,
                        ItinIndex: $scope.flightList[indexNo].RegisterNumber,
                        SecureCode: $scope.flightRequest.SecureCode
                    }
                }).success(function(returnData) {
                    $scope.revalidateFlightParam.validated = true;
                    console.log(indexNo);
                    if (returnData.IsValid == true) {
                        console.log('departure flight available');
                        $scope.revalidateFlightParam.available = true;
                        $scope.revalidateFlightParam.token = returnData.Token;

                        $('.push-token input').val($scope.revalidateFlightParam.token);
                        $('.push-token').submit();

                    } else if (returnData.IsValid == false) {
                        $scope.revalidateFlightParam.available = false;
                        $scope.revalidateFlightParam.validating = false;

                        if (returnData.IsOtherFareAvailable == true) {
                            console.log('departure flight has new price');
                            $scope.revalidateFlightParam.newFare = true;
                            $scope.revalidateFlightParam.token = returnData.Token;
                            // update price
                            $scope.revalidateFlightParam.totalFare = returnData.NewFare;
                            $scope.flightList[indexNo].TotalFare = returnData.NewFare;
                            $('.push-token input').val($scope.revalidateFlightParam.token);

                        } else if (returnData.IsOtherFareAvailable == false) {
                            console.log('departure flight is gone');
                            $scope.revalidateFlightParam.newFare = false;
                            $scope.flightList[indexNo].Available = false;

                        }
                    }
                }).error(function(returnData) {
                    $scope.revalidateFlightParam.validatingFlight = false;
                    console.log('ERROR Validating Flight');
                    console.log(returnData);
                    console.log('--------------------');
                });
            } else {
                $scope.revalidateFlightParam.proceed = true;
                $('.push-token').submit();
            }
        }
        $scope.revalidateSubmit = function() {
            $('.push-token').submit();
        }
        $scope.revalidateCancel = function () {
            $scope.loading = false;
            $scope.revalidateFlightParam.validated = false;
            $scope.revalidateFlightParam.newFare = false;
            $scope.revalidateFlightParam.available = false;
            $('body').removeClass('no-scroll');
        }


        // **********
        // get flight function
        $scope.getFlight = function() {

            $scope.busy = true;
            $scope.loading = true;
            $scope.loadingFlight = true;

            // console.log('----------');
            // console.log('Getting flight list with parameter');
            // console.log(FlightSearchConfig.flightForm);

            if ($scope.pristine || $scope.flightRequest.Requests.length) {
                console.log('request : ' + $scope.flightRequest.Requests);
            }

            if ($scope.flightRequest.Progress < 100) {
                // **********
                // ajax
                $http.get(FlightSearchConfig.Url, {
                    params: {
                        request: $scope.flightRequest
                    }
                }).success(function(returnData) {

                    // set searchID
                    RevalidateConfig.SearchId = returnData.SearchId;
                    $scope.flightRequest.SearchId = returnData.SearchId;

                    // set flight request if pristine
                    if ($scope.pristine == true) {
                        $scope.pristine = false;
                        for (var i = 0; i < returnData.MaxRequest; i++) {
                            $scope.flightRequest.Requests.push( i+1 );
                        }
                    }

                    // if granted request is not null
                    if (returnData.GrantedRequests.length) {
                        console.log('Granted request  : '+returnData.GrantedRequests);
                        for (var i = 0; i < returnData.GrantedRequests.length; i++) {
                            // add to completed
                            if ( $scope.flightRequest.Completed.indexOf( returnData.GrantedRequests[i] < 0 ) ) {
                                $scope.flightRequest.Completed.push( returnData.GrantedRequests[i] );
                            }
                            // check current request. Remove if completed
                            if ($scope.flightRequest.Requests.indexOf(returnData.GrantedRequests[i] < 0)) {
                                $scope.flightRequest.Requests.splice( $scope.flightRequest.Requests.indexOf(returnData.GrantedRequests[i]) , 1 );
                            }

                        }

                        // update total progress
                        $scope.flightRequest.Progress = ((returnData.MaxRequest - $scope.flightRequest.Requests.length) / returnData.MaxRequest) * 100;

                        // set expiry if progress == 100
                        if ($scope.flightRequest.Progress == 100) {
                            $scope.expiry.time = returnData.ExpiryTime;
                        } else {
                            $scope.flightRequest.FinalProgress = $scope.flightRequest.Progress;
                        }

                        // generate flight
                        $scope.generateFlightList(returnData.FlightList);
                        

                        console.log('Progress : '+ $scope.flightRequest.Progress +' %');
                        console.log(returnData);
                    }

                    //console.log(returnData);

                    

                    // loop the function
                    setTimeout(function () {
                        $scope.getFlight();
                    }, 1000);

                }).error(function(returnData) {
                    console.log('Failed to get flight list');
                    console.log(returnData);
                });

            } else {
                console.log('Finished getting flight list !');
                $scope.busy = false;
                $scope.loading = false;
                $scope.loadingFlight = false;
            }

        }

        // **********
        // generate flight list
        $scope.generateFlightList = function(data) {

            var startNo = $scope.flightList.length;

            for (var i = 0; i < data.length; i++) {
                data[i].Available = true;
                data[i].IndexNo = ( startNo + i);
                $scope.flightList.push(data[i]);
            }

            if ($scope.flightRequest.Progress == 100) {

                console.log('Generating Filter');

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
                    $scope.priceFilterParam.prices.push($scope.flightList[i].TotalFare);

                    // *****
                    // populate airline tag
                    $scope.flightList[i].AirlinesTag = [];
                    for (var x = 0 ; x < $scope.flightList[i].Trips[0].Airlines.length; x++) {
                        $scope.airlineFilterParam.airlinesList.push($scope.flightList[i].Trips[0].Airlines[x]);
                        $scope.flightList[i].AirlinesTag.push($scope.flightList[i].Trips[0].Airlines[x].Code);
                    }

                }
                // sort prices and set initial price filter
                function sortNumber(a, b) {
                    return a - b;
                }
                $scope.priceFilterParam.prices.sort(sortNumber);
                $scope.priceFilterParam.initial[0] = Math.floor($scope.priceFilterParam.prices[0]);
                $scope.priceFilterParam.initial[1] = Math.round($scope.priceFilterParam.prices[$scope.priceFilterParam.prices.length - 1]);

                // remove duplicatae from airlines
                var dupes = {};
                $.each($scope.airlineFilterParam.airlinesList, function (i, el) {
                    if (!dupes[el.Code]) {
                        dupes[el.Code] = true;
                        $scope.airlineFilterParam.airlines.push(el);
                    }
                });
                $scope.airlineFilterParam.airlinesList = [];

                // activate price filter
                $('.price-slider').slider({
                    range: true,
                    min: $scope.priceFilterParam.initial[0],
                    max: $scope.priceFilterParam.initial[1],
                    step: 50000,
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
                        $('.price-slider-max').val(ui.values[1]);
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
                        $('.departure-slider-min').trigger('input');
                        $('.departure-slider-max').val(ui.values[1]);
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
                        $('.arrival-slider-min').trigger('input');
                        $('.arrival-slider-max').val(ui.values[1]);
                        $('.arrival-slider-max').trigger('input');
                    }
                });

                $scope.flightRequest.FinalProgress = 100;

                console.log('Completed setting flight and filter');
                console.log($scope);
            }

        }

    }
]);// flight controller

