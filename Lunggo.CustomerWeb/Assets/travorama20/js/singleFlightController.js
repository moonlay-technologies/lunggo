// travorama angular app - Flight Controller

app.controller('singleFlightController', [
    '$http', '$scope', function ($http, $scope) {

        // **********
        // on document ready
        angular.element(document).ready(function () {
            $scope.getFlight();
        });

        // **********
        // general variables
        $scope.busy = false;
        $scope.loading = false;
        $scope.loadingFlight = false;
        $scope.flightList = [];
        $scope.expiryTime = '';
        $scope.flightRequest = {
            CabinClass: FlightSearchConfig.flightForm.cabin,
            AdultCount: FlightSearchConfig.flightForm.passenger.adult,
            ChildCount: FlightSearchConfig.flightForm.passenger.child,
            InfantCount: FlightSearchConfig.flightForm.passenger.infant,
            TripType: FlightSearchConfig.flightForm.type,
            Trips: FlightSearchConfig.flightForm.trips,
        };

        // **********
        // general functions
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

        // milisecond to hour
        $scope.msToTime = function(duration) {
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
        $scope.getDate = function(dateTime) {
            dateTime = new Date(dateTime);
            dateTime = dateTime.getDate();
            return dateTime;
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
        $scope.availableFilter = function(flight) {
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
        $scope.priceFilter = function(flight) {
            if ( flight.TotalFare >= $scope.priceFilterParam.current[0] && flight.TotalFare <= $scope.priceFilterParam.current[1] ) {
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
            if ($scope.airlineFilterParam.pure == true) {
                return flight;
            } else {
                for (var i in flight.AirlinesTag) {
                    if ($scope.airlineFilterParam.selected.indexOf(flight.AirlinesTag[i]) != -1) {
                        return flight;
                    }
                }
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
            newFare: false
        };
        $scope.revalidateFlight = function (indexNo) {

            $('body').addClass('no-scroll');

            console.log('---------------------');
            console.log('Validating Flight no : '+indexNo);

            $scope.revalidateFlightParam.validating = true;
            $scope.loading = true;

            // revalidate flight
            $http.get(RevalidateConfig.Url, {
                params: {
                    SearchId: RevalidateConfig.SearchId,
                    ItinIndex: indexNo
                }
            }).success(function (returnData) {
                $scope.revalidateFlightParam.validating = false;
                $scope.revalidateFlightParam.validated = true;

                if (returnData.IsValid == true) {
                    console.log('departure flight available');
                    $scope.revalidateFlightParam.available = true;
                    $scope.revalidateFlightParam.token = returnData.Token;

                    $('.push-token input').val($scope.revalidateFlightParam.token);
                    $('.push-token').submit();

                } else if (returnData.IsValid == false) {
                    $scope.revalidateFlightParam.available = false;

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
                        $scope.flightList[indexNo].available = false;

                    }
                }
            }).error(function (returnData) {
                $scope.departureFlightConfig.validatingFlight = false;
                console.log('ERROR Validating Flight');
                console.log(returnData);
                console.log('--------------------');
            });
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

            console.log('----------');
            console.log('Getting flight list with parameter');
            console.log(FlightSearchConfig.flightForm);

            // ajax
            $http.get(FlightSearchConfig.Url, {
                params: {
                    request: $scope.flightRequest
                }
            }).success(function(returnData) {
                $scope.busy = false;
                $scope.loading = false;
                $scope.loadingFlight = false;
                console.log('Success getting flight list');
                // console log the return data
                console.log(returnData);

                // set expiry time
                $scope.expiryTime = new Date(returnData.ExpiryTime);

                // set searchID
                RevalidateConfig.SearchId = returnData.SearchId;

                // generate flight list
                $scope.generateFlightList(returnData.FlightList);

                console.log('Finished getting flight');
                console.log('----------');


            }).error(function(returnData) {
                console.log('Failed to get flight list');
                console.log(returnData);
                console.log('Finished getting flight list');
                console.log('----------');

            });

        }

        // generate flight list
        $scope.generateFlightList = function(data) {

            // add data to flight list
            $scope.flightList = data;

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
                // populate prices and set min-price and max-price
                $scope.priceFilterParam.prices.push( data[i].TotalFare );
                function sortNumber(a, b) {
                    return a - b;
                }
                $scope.priceFilterParam.prices.sort(sortNumber);
                $scope.priceFilterParam.initial[0] = Math.floor($scope.priceFilterParam.prices[0]);
                $scope.priceFilterParam.initial[1] = Math.round($scope.priceFilterParam.prices[$scope.priceFilterParam.prices.length - 1]);

                // *****
                // populate airline tag
                $scope.flightList[i].AirlinesTag = [];
                for (var x = 0 ; x < $scope.flightList[i].Trips[0].Airlines.length; x++) {
                    $scope.airlineFilterParam.airlinesList.push($scope.flightList[i].Trips[0].Airlines[x]);
                    $scope.flightList[i].AirlinesTag.push($scope.flightList[i].Trips[0].Airlines[x].Code);
                }

            }

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

            console.log($scope);

        }

    }
]);// flight controller

