app.controller('SingleFlightController', ['$http', '$scope', '$rootScope', '$interval', function($http, $scope, $rootScope, $interval) {

    // **********
    // on document ready
    angular.element(document).ready(function () {
        
        $scope.GetFlight('get request');
    });

    // ********************

    $scope.Progress = 0;
    $scope.FlightConfig = [
        {
            Name: 'departure',
            FlightList: [],
            ActiveFlight: -1,
            ActiveFlightAvailable: true,
            ActiveFlightNewPrice: '',
            FlightRequest: {
                CabinClass: FlightSearchConfig.flightForm.cabin,
                AdultCount: FlightSearchConfig.flightForm.passenger.adult,
                ChildCount: FlightSearchConfig.flightForm.passenger.child,
                InfantCount: FlightSearchConfig.flightForm.passenger.infant,
                TripType: FlightSearchConfig.flightForm.type,
                Trips: FlightSearchConfig.flightForm.trips,
                Requests: FlightSearchConfig.flightForm.Requests,
                Completed: [],
                SecureCode: FlightSearchConfig.flightForm.SecureCode,
                Progress: 0,
                FinalProgress: 0,
                Pristine: true
            },
            FlightFilter: {
                Transit: [false, false, false],
                DepartureTime: [false, false, false, false],
                ArrivalTime: [false, false, false, false],
                Airline: [],
                AirlineSelected: []
            },
            FlightSort: {
                Label: 'price',
                Value: 'totalFare',
                Invert: false,
                Set: function (sortBy, invert) {
                    $scope.FlightConfig[0].FlightSort.Label = sortBy;
                    $scope.FlightConfig[0].FlightSort.Invert = invert;
                    switch (sortBy) {
                        case 'price':
                            $scope.FlightConfig[0].FlightSort.Value = 'totalFare';
                            break;
                        case 'duration':
                            $scope.FlightConfig[0].FlightSort.Value = 'trips[0].totalDuration';
                            break;
                        case 'airline':
                            $scope.FlightConfig[0].FlightSort.Value = 'trips[0].airlines[0].name';
                            break;
                        case 'departure':
                            $scope.FlightConfig[0].FlightSort.Value = 'trips[0].segments[0].departureTime';
                            break;
                        case 'arrival':
                            $scope.FlightConfig[0].FlightSort.Value = 'trips[0].segments[(trips[0].segments.length-1)].arrivalTime';
                            break;
                    }
                    $scope.SetOverlay('');
                }
            },
            FlightExpiry: {
                expired: false,
                time: '',
                start: function () { }
            }
        }
    ];

    $scope.GetFlight = function (a) {

        $scope.PageConfig.Busy = true;
        console.log(a);
        if ($scope.Progress < 100) {

            console.log('request : ' + $scope.FlightConfig[0].FlightRequest.Requests);
            console.log("Request : " + FlightSearchConfig.Url + $scope.flightFixRequest() + '/' + $scope.Progress);
            //   $http.get(FlightSearchConfig.Url + '/' + $scope.flightFixRequest + '/' + $scope.Progress, {

            // **********
            // ajax
            $http.get(FlightSearchConfig.Url + '/' + $scope.flightFixRequest() + '/' + $scope.Progress, {
                params: {
                    request: $scope.FlightConfig[0].FlightRequest
                }
            }).success(function (returnData) {


                SelectConfig.SearchId = $scope.flightFixRequest();
                //$scope.flightRequest.SearchId = $scope.flightFixRequest;

                $scope.FlightConfig[0].FlightRequest.SearchId = $scope.flightFixRequest();
                console.log($scope.flightFixRequest());

                $scope.Progress = returnData.progress;
                if ($scope.Progress == 100) {
                    $scope.PageConfig.ExpiryDate.Time = returnData.expTime;
                    $scope.FlightConfig[0].FlightRequest.Progress = $scope.Progress;
                } else {
                    $scope.FlightConfig[0].FlightRequest.Progress = $scope.Progress;
                }
                if (returnData.flights.length) {
                    $scope.FlightFunctions.GenerateFlightList(returnData.flights[0].options);
                }


                console.log('Progress : ' + $scope.Progress + ' %');
                console.log(returnData);

                // loop the function
                setTimeout(function () {
                    $scope.GetFlight('1');
                }, 1000);

            }).error(function (returnData) {
                console.log('Failed to get flight list');
                console.log(returnData);
                for (var i = 0; i < $scope.FlightConfig[0].FlightRequest.Requests.length; i++) {
                    // add to completed
                    if ($scope.FlightConfig[0].FlightRequest.Completed.indexOf($scope.FlightConfig[0].FlightRequest.Requests[i] < 0)) {
                        $scope.FlightConfig[0].FlightRequest.Completed.push($scope.FlightConfig[0].FlightRequest.Requests[i]);
                    }
                    // check current request. Remove if completed
                    if ($scope.FlightConfig[0].FlightRequest.Requests.indexOf($scope.FlightConfig[0].FlightRequest.Requests[i] < 0)) {
                        $scope.FlightConfig[0].FlightRequest.Requests.splice($scope.FlightConfig[0].FlightRequest.Requests.indexOf($scope.FlightConfig[0].FlightRequest.Requests[i]), 1);
                    }
                }
                $scope.FlightConfig[0].FlightRequest.Progress = 100;
                $scope.FlightConfig[0].FlightRequest.FinalProgress = 100;
            });

        } else {
            console.log('Finished getting flight list !');
            $scope.PageConfig.Busy = false;
        }

    }// get flight end




    $scope.PageConfig = $rootScope.PageConfig;
    $scope.FlightSearchForm = $rootScope.FlightSearchForm;

    // set additional variables for Flight Page
    $scope.PageConfig.ActiveSection = 'departure';
    $scope.PageConfig.ActiveOverlay = '';
    $scope.PageConfig.Loading = 0;
    $scope.PageConfig.Validating = false;
    $scope.PageConfig.ExpiryDate = {
        Expired: false,
        Time: '',
        Start: function() {
            var expiryTime = new Date($scope.PageConfig.ExpiryDate.Time);
            if ($scope.PageConfig.ExpiryDate.Expired || $scope.PageConfig.ExpiryDate.Starting) return;
            $interval(function() {
                $scope.PageConfig.ExpiryDate.Starting = true;
                var NowTime = new Date();
                if (NowTime > expiryTime) {
                    $scope.PageConfig.ExpiryDate.Expired = true;
                }
            }, 1000);
        },
        Starting: false
    };

    $scope.flightFixRequest = function () {
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
         var passengerParam = FlightSearchConfig.flightForm.passenger.adult + '' + FlightSearchConfig.flightForm.passenger.child + '' + FlightSearchConfig.flightForm.passenger.infant + '' + cabin;
         var departureParam = (origin + destination) + ((('0' + departureDate.getDate()).slice(-2)) + (('0' + (departureDate.getMonth() + 1)).slice(-2)) + (departureDate.getFullYear().toString().substr(2, 2)));
         return departureParam + '-' + passengerParam;
    }
       
    $scope.FlightFunctions = {};


    // ********************
    // functions

    // set overlay
    $scope.SetOverlay = function (overlay) {
        console.log('changing overlay to : ' + overlay);
        if (!overlay) {
            $scope.PageConfig.ActiveOverlay = '';
            $scope.PageConfig.BodyNoScroll = false;
        } else {
            $scope.PageConfig.ActiveOverlay = overlay;
            $scope.PageConfig.BodyNoScroll = true;
        }
    }

    // get full date time
    $scope.getFullDate = function (dateTime) {
        if (dateTime) {
            dateTime = parseInt(dateTime.substr(0, 4) + '' + dateTime.substr(5, 2) + '' + dateTime.substr(8, 2));
            return dateTime;
        }
    }

    // get overday date
    $scope.getOverdayDate = function (departureDate, arrivalDate) {
        if (departureDate && arrivalDate) {
            departureDate = new Date(departureDate);
            departureDate = new Date((departureDate.getFullYear() + ' ' + (departureDate.getUTCMonth() + 1) + ' ' + departureDate.getUTCDate()));
            arrivalDate = new Date(arrivalDate);
            arrivalDate = new Date((arrivalDate.getFullYear() + ' ' + (arrivalDate.getUTCMonth() + 1) + ' ' + arrivalDate.getUTCDate()));
            var overday = arrivalDate - departureDate;
            overday = overday / 1000 / 60 / 60 / 24;
            if (overday > 0) {
                overday = '+' + overday;
            }
            return overday;
        }
    }

    // get hour and minute for time filtering		
    $scope.getHour = function (dateTime) {
        dateTime = (dateTime.substr(11, 2)) + (dateTime.substr(14, 2));
        return parseInt(dateTime);
    }

    // ms to time
    $scope.msToTime = function (duration) {
        var milliseconds = parseInt((duration % 1000) / 100),
            seconds = parseInt((duration / 1000) % 60),
            minutes = parseInt((duration / (1000 * 60)) % 60),
            hours = parseInt((duration / (1000 * 60 * 60)));
        // hours = parseInt((duration / (1000 * 60 * 60)) % 24);
        // days = parseInt((duration / (1000 * 60 * 60 * 24)));
        hours = hours;
        minutes = minutes;
        seconds = seconds;
        return hours + "j " + minutes + "m";
    }

    // ********************
    // get flight


    // generate flight list
    $scope.FlightFunctions.GenerateFlightList = function (data) {
        console.log('Generating Flight List');
        console.log(data);
        var targetScope = $scope.FlightConfig[0];
        var startNo = $scope.FlightConfig[0].FlightList.length;
        for (var i = 0; i < data.length; i++) {
            data[i].Available = true;
            data[i].IndexNo = (startNo + i);
            // init airlines
            for (var x = 0; x < data[i].trips[0].airlines.length; x++) {
                data[i].trips[0].airlines[x].Checked = true;
                }
            targetScope.FlightList.push(data[i]);
                        }

                    if ($scope.FlightConfig[0].FlightRequest.Progress == 100) {
            for (var x = 0; x < $scope.FlightConfig[0].FlightList.length; x++) {
                $scope.priceFilterParam.prices.push($scope.FlightConfig[0].FlightList[x].totalFare);
            }
            function sortNumber(a, b) {
                return a - b;
            }
            $scope.priceFilterParam.prices.sort(sortNumber);
            $scope.priceFilterParam.initial[0] = ($scope.priceFilterParam.prices[0]);
            $scope.priceFilterParam.initial[1] = ($scope.priceFilterParam.prices[$scope.priceFilterParam.prices.length - 1]);
                        
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
                    $('.price-slider-max').val(ui.values[1]);
                    $('.price-slider-max').trigger('input');
                }
            });
                    }

        // generate airline for flight filtering		
        for (var i = 0; i < targetScope.FlightList.length; i++) {
            targetScope.FlightList[i].AirlinesTag = [];
            for (var x = 0; x < targetScope.FlightList[i].trips[0].airlines.length; x++) {
                targetScope.FlightList[i].AirlinesTag.push(targetScope.FlightList[i].trips[0].airlines[x].code);
                targetScope.FlightFilter.Airline.push(targetScope.FlightList[i].trips[0].airlines[x]);
                }
                    }
        // remove duplicate from airline filter		
        var dupes = {};
        var Airlines = [];
        $.each(targetScope.FlightFilter.Airline, function (i, el) {
            if (!dupes[el.code]) {
                dupes[el.code] = true;
                Airlines.push(el);
                    }
            });
        targetScope.FlightFilter.Airline = Airlines;
        Airlines = [];

    }// generate flight list end

    // after flight request complete		
    $scope.FlightFunctions.PostRequest = function (targetFlight) {
        console.log('----------------------------------------');
        console.log('Post request function :');
        var targetScope = (targetFlight == 'departure' ? $scope.FlightConfig[0] : $scope.FlightConfig[1]);
        // generate airline for flight filtering		
        for (var i = 0; i < targetScope.FlightList.length; i++) {
            targetScope.FlightList[i].AirlinesTag = [];
            for (var x = 0; x < targetScope.FlightList[i].trips[0].airlines.length; x++) {
                targetScope.FlightList[i].AirlinesTag.push(targetScope.FlightList[i].trips[0].airlines[x].code);
                targetScope.FlightFilter.Airline.push(targetScope.FlightList[i].trips[0].airlines[x]);
            }
        }
        // remove duplicate from airline filter		
        var dupes = {};
        var Airlines = [];
        $.each(targetScope.FlightFilter.Airline, function (i, el) {
            if (!dupes[el.code]) {
                dupes[el.code] = true;
                Airlines.push(el);
            }
        });
        targetScope.FlightFilter.Airline = Airlines;
        Airlines = []; // empty the variable		
        console.log($scope.FlightConfig[0]);
    }// after flight request complete end

    // revalidate flight
    $scope.FlightFunctions.Revalidate = function(indexNo) {

        $scope.PageConfig.Validating = true;
        console.log('Validating flight no : '+indexNo);

        // AJAX revalidate
        if (!RevalidateConfig.Validated) {

            $http.post(SelectConfig.Url, {
                searchId: SelectConfig.SearchId,
                regs: [$scope.FlightConfig[0].FlightList[indexNo].reg],
            }).success(function (returnData) {
                RevalidateConfig.Validated = true;
                //$scope.selectFlightParam.validated = true;
                console.log(indexNo);
                console.log("Response Select Flight : " + returnData);
                if (returnData.token != "" || returnData.token != null) {
                    console.log('departure flight available');
                    //RevalidateConfig.Available = true;
                    //RevalidateConfig.Token = returnData.Token;

                    $('.push-token input').val(returnData.token);
                    $('.push-token').submit();
                }
            }).error(function (returnData) {
                $scope.PageConfig.Validating = false;
                console.log('ERROR Validating Flight');
                console.log(returnData);
                console.log('--------------------');
            });
        } else {
            // skip to book
            $('.push-token').submit();
        }
    }

            // revalidate flight
            //$http.get(RevalidateConfig.Url, {
            //    params: {
            //        SearchId: RevalidateConfig.SearchId,
            //        ItinIndex: $scope.FlightConfig[0].FlightList[indexNo].RegisterNumber,
            //        SecureCode: $scope.FlightConfig[0].FlightRequest.SecureCode
            //    }
            //}).success(function (returnData) {
            //    RevalidateConfig.Validated = true;
            //    console.log(indexNo);
            //    if (returnData.IsValid == true) {
            //        console.log('departure flight available');
            //        RevalidateConfig.Available = true;
            //        RevalidateConfig.Token = returnData.Token;

            //        $('.push-token input').val(RevalidateConfig.Token);
            //        $('.push-token').submit();

            //    } else if (returnData.IsValid == false) {
            //        RevalidateConfig.Available = false;
            //        $scope.PageConfig.Validating = false;
            //        $scope.FlightConfig[0].ActiveFlightAvailable = false;

            //        if (returnData.IsOtherFareAvailable == true) {
            //            console.log('departure flight has new price');
            //            $scope.FlightConfig[0].ActiveFlightNewPrice = returnData.NewFare;
            //            RevalidateConfig.NewFare = true;
            //            RevalidateConfig.Token = returnData.Token;
            //            // update price
            //            $scope.FlightConfig[0].FlightList[indexNo].TotalFare = returnData.NewFare;
            //            $('.push-token input').val(RevalidateConfig.Token);
            //        } else if (returnData.IsOtherFareAvailable == false) {
            //            console.log('departure flight is gone');
            //            $scope.FlightConfig[0].ActiveFlightNewPrice = -1;
            //            RevalidateConfig.NewFare = false;
            //            $scope.FlightConfig[0].FlightList[indexNo].Available = false;
            //        }
            //    }
            //}).error(function (returnData) {
            //    $scope.PageConfig.Validating = false;
            //    console.log('ERROR Validating Flight');
            //    console.log(returnData);
            //    console.log('--------------------');
            //});

        // revalidate flight end

    // set active flight
    $scope.FlightFunctions.SetActiveFlight = function (FlightNumber) {
        if (FlightNumber >= 0) {
            $scope.FlightConfig[0].ActiveFlight = FlightNumber;
            $scope.SetOverlay('flight-detail');
        } else {
            $scope.FlightConfig[0].ActiveFlight = -1;
            $scope.SetOverlay();
        }
    }// set active flight end

    // *****
    // flight filtering functions
    $scope.FlightFiltering = {};
    $scope.FlightFiltering.Touched = false;
    // available filter		
    $scope.FlightFiltering.AvailableFilter = function () {
        return function (flight) {
            if (flight.Available) {
                return flight;
            }
        }
    }

    // price filter
    $scope.priceFilterParam = {
        initial: [-1, -1],
        current: [0, 1000000000],
        prices: []
    };

    $scope.FlightFiltering.PriceFilter = function () {
        
        return function(flight) {
            if (flight.totalFare >= $scope.priceFilterParam.current[0] && flight.totalFare <= $scope.priceFilterParam.current[1]) {
                return flight;
                } 
        }
    }


    // transit filter		
    $scope.FlightFiltering.TransitFilter = function (targetFlight)
    {
        var targetScope = (targetFlight == 'departure' ? $scope.FlightConfig[0] : $scope.FlightConfig[1]);
        return function (flight) {

            if (!targetScope.FlightFilter.Transit[0] && !targetScope.FlightFilter.Transit[1]
                 && !targetScope.FlightFilter.Transit[2] && !targetScope.FlightFilter.Transit[3]) {
                return flight;
            } else {
                if (targetScope.FlightFilter.Transit[0]) {
                    if (flight.trips[0].transitCount == 0) {
                        return flight;
                    }
                }

                if (targetScope.FlightFilter.Transit[1]) {
                    if (flight.trips[0].transitCount == 1) {
                        return flight;
                    }
                }
                if (targetScope.FlightFilter.Transit[2]) {
                    if (flight.trips[0].transitCount > 1) {
                        return flight;
                    }
                }
            }
            //if (targetScope.FlightFilter.Transit[0]) {
            //    if (flight.Trips[0].transitCount == 0) {
            //        return flight;
            //    }
            //}
            //if (targetScope.FlightFilter.Transit[1]) {
            //    if (flight.Trips[0].transitCount == 1) {
            //        return flight;
            //    }
            //}
            //if (targetScope.FlightFilter.Transit[2]) {
            //    if (flight.Trips[0].transitCount > 1) {
            //        return flight;
            //    }
            //}
        }
    }
    // departure time filter		
    $scope.FlightFiltering.DepartureTimeFilter = function (targetFlight) {
        var targetScope = (targetFlight == 'departure' ? $scope.FlightConfig[0] : $scope.FlightConfig[1]);
        return function (flight) {
            var a = $scope.getHour(flight.trips[0].segments[0].departureTime);
            if (!targetScope.FlightFilter.DepartureTime[0] && !targetScope.FlightFilter.DepartureTime[1] &&
                !targetScope.FlightFilter.DepartureTime[2] && !targetScope.FlightFilter.DepartureTime[3]) {
                return flight;
            } else {
                if (targetScope.FlightFilter.DepartureTime[0]) {
                        if ($scope.getHour(flight.trips[0].segments[0].departureTime) >= 0400 && $scope.getHour(flight.trips[0].segments[0].departureTime) <= 1100) {
                            return flight;
                        }
                }

                if (targetScope.FlightFilter.DepartureTime[1]) {
                        if ($scope.getHour(flight.trips[0].segments[0].departureTime) >= 1100 && $scope.getHour(flight.trips[0].segments[0].departureTime) <= 1500) {
                            return flight;
                        }
                }

                if (targetScope.FlightFilter.DepartureTime[2]) {
                    if ($scope.getHour(flight.trips[0].segments[0].departureTime) >= 1500 && $scope.getHour(flight.trips[0].segments[0].departureTime) <= 1900) {
                        return flight;
                    }
                }

                if (targetScope.FlightFilter.DepartureTime[3]) {
                    if ($scope.getHour(flight.trips[0].Segments[0].departureTime) >= 1900 || $scope.getHour(flight.trips[0].Segments[0].departureTime) <= 0400) {
                        return flight;
                    }
                }
            }
        }
    }
    // arrival time filter		
    $scope.FlightFiltering.ArrivalTimeFilter = function (targetFlight) {
        var targetScope = (targetFlight == 'arrival' ? $scope.FlightConfig[0] : $scope.FlightConfig[1]);
        return function (flight) {
            if (!targetScope.FlightFilter.ArrivalTime[0] && !targetScope.FlightFilter.ArrivalTime[1]
                && !targetScope.FlightFilter.ArrivalTime[2] && !targetScope.FlightFilter.ArrivalTime[3]) {
                return flight;
            } else {
                if (targetScope.FlightFilter.ArrivalTime[0]) {
                    if ($scope.getHour(flight.trips[0].segments[flight.trips[0].segments.length - 1].arrivalTime) >= 0400 && $scope.getHour(flight.trips[0].segments[flight.trips[0].segments.length - 1].arrivalTime) <= 1100) {
                        return flight;
                    }
                }
                if (targetScope.FlightFilter.ArrivalTime[1]) {
                    if ($scope.getHour(flight.trips[0].segments[flight.trips[0].segments.length - 1].arrivalTime) >= 1100 && $scope.getHour(flight.trips[0].segments[flight.trips[0].segments.length - 1].arrivalTime) <= 1500) {
                        return flight;
                    }
                }
                if (targetScope.FlightFilter.ArrivalTime[2]) {
                    if ($scope.getHour(flight.trips[0].segments[flight.trips[0].segments.length - 1].arrivalTime) >= 1500 && $scope.getHour(flight.trips[0].segments[flight.trips[0].segments.length - 1].arrivalTime) <= 1900) {
                        return flight;
                    }
                }
                if (targetScope.FlightFilter.ArrivalTime[3]) {
                    if ($scope.getHour(flight.trips[0].segments[flight.trips[0].segments.length - 1].arrivalTime) >= 1900 || $scope.getHour(flight.trips[0].segments[flight.trips[0].segments.length - 1].arrivalTime) <= 0400) {
                        return flight;
                    }
                }
            }
        }
    }
    // airline filter		
    $scope.FlightFiltering.AirlineCheck = function (targetFlight) {
        var targetScope = (targetFlight == 'departure' ? $scope.FlightConfig[0] : $scope.FlightConfig[1]);
        targetScope.FlightFilter.AirlineSelected = [];
        $scope.FlightFiltering.Touched = true;
        
        for (var i = 0; i < targetScope.FlightFilter.Airline.length; i++) {
            if (!targetScope.FlightFilter.Airline[i].Checked) {
                targetScope.FlightFilter.AirlineSelected.push(targetScope.FlightFilter.Airline[i].code);
            }
        }

        if (targetScope.FlightFilter.AirlineSelected.length == 0) {
            for (var x = 0;x < targetScope.FlightFilter.Airline.length; x++) {
                targetScope.FlightFilter.AirlineSelected.push(targetScope.FlightFilter.Airline[x].code);
            }
        }
    }
    $scope.FlightFiltering.AirlineFilter = function (targetFlight) {
        var targetScope = (targetFlight == 'departure' ? $scope.FlightConfig[0] : $scope.FlightConfig[1]);
        return function (flight) {
            if (!$scope.FlightFiltering.Touched) {
                return flight;
            } else {
                for (var i = 0; i < flight.AirlinesTag.length; i++) {
                    if (targetScope.FlightFilter.AirlineSelected.indexOf(flight.AirlinesTag[i]) != -1) {
                        return flight;
                    }
                }
            }
        }
    }

}]);