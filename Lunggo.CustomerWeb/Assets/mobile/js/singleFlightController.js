app.controller('SingleFlightController', ['$http', '$scope', '$rootScope', '$interval', function($http, $scope, $rootScope, $interval) {

    // **********
    // on document ready
    angular.element(document).ready(function () {
        $scope.FlightFunctions.GetFlight();
    });

    // ********************
    // variables
    //$.datepicker.setDefaults(
    //        $.extend(
    //        { 'dateFormat': 'dd-mm-yy' },
    //        $.datepicker.regional['id']
    //        )
    //    );
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
                Requests: [],
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
                Value: 'TotalFare',
                Invert: false,
                Set : function (sortBy, invert) {
                    $scope.FlightConfig[0].FlightSort.Label = sortBy;
                    $scope.FlightConfig[0].FlightSort.Invert = invert;
                    switch (sortBy) {
                        case 'price':
                            $scope.FlightConfig[0].FlightSort.Value = 'TotalFare';
                            break;
                        case 'duration':
                            $scope.FlightConfig[0].FlightSort.Value = 'Trips[0].TotalDuration';
                            break;
                        case 'airline':
                            $scope.FlightConfig[0].FlightSort.Value = 'Trips[0].Airlines[0].Name';
                            break;
                        case 'departure':
                            $scope.FlightConfig[0].FlightSort.Value  = 'Trips[0].Segments[0].DepartureTime';
                            break;
                        case 'arrival':
                            $scope.FlightConfig[0].FlightSort.Value  = 'Trips[0].Segments[(Trips[0].Segments.length-1)].ArrivalTime';
                            break;
                    }
                    $scope.SetOverlay('');
                }
            },
            FlightExpiry: {
                expired: false,
                time: '',
                start : function() {}
            }
        }
    ];
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
    $scope.FlightFunctions.GetFlight = function() {

        $scope.PageConfig.Busy = true;
        
        if ($scope.FlightConfig[0].FlightRequest.Progress < 100) {

            console.log('request : ' + $scope.FlightConfig[0].FlightRequest.Requests);

            // **********
            // ajax
            $http.get(FlightSearchConfig.Url, {
                params: {
                    request: $scope.FlightConfig[0].FlightRequest
                }
            }).success(function (returnData) {

                // set searchID
                RevalidateConfig.SearchId = returnData.SearchId;
                $scope.FlightConfig[0].FlightRequest.SearchId = returnData.SearchId;

                // set flight request if pristine
                if ($scope.FlightConfig[0].FlightRequest.Pristine == true) {
                    $scope.FlightConfig[0].FlightRequest.Pristine = false;
                    for (var i = 0; i < returnData.MaxRequest; i++) {
                        $scope.FlightConfig[0].FlightRequest.Requests.push(i + 1);
                    }
                }

                // if granted request is not null
                if (returnData.GrantedRequests.length) {
                    console.log('Granted request  : ' + returnData.GrantedRequests);
                    for (var i = 0; i < returnData.GrantedRequests.length; i++) {
                        // add to completed
                        if ($scope.FlightConfig[0].FlightRequest.Completed.indexOf(returnData.GrantedRequests[i] < 0)) {
                            $scope.FlightConfig[0].FlightRequest.Completed.push(returnData.GrantedRequests[i]);
                        }
                        // check current request. Remove if completed
                        if ($scope.FlightConfig[0].FlightRequest.Requests.indexOf(returnData.GrantedRequests[i] < 0)) {
                            $scope.FlightConfig[0].FlightRequest.Requests.splice($scope.FlightConfig[0].FlightRequest.Requests.indexOf(returnData.GrantedRequests[i]), 1);
                        }

                    }

                    // update total progress
                    $scope.FlightConfig[0].FlightRequest.Progress = ((returnData.MaxRequest - $scope.FlightConfig[0].FlightRequest.Requests.length) / returnData.MaxRequest) * 100;
                    console.log('Progress : ' + $scope.FlightConfig[0].FlightRequest.Progress + ' %');
                    console.log(returnData);

                    // generate flight
                    if (returnData.FlightList.length) {
                        $scope.FlightFunctions.GenerateFlightList(returnData.FlightList);
                    }
                    //$scope.FlightFunctions.PostRequest('departure');
                    // set expiry if progress == 100
                    if ($scope.FlightConfig[0].FlightRequest.Progress == 100) {
                        $scope.FlightConfig[0].FlightExpiry.time = returnData.ExpiryTime;
                        $scope.PageConfig.ExpiryDate.Time = returnData.ExpiryTime;
                        $scope.PageConfig.ExpiryDate.Start();
                        $scope.FlightFunctions.PostRequest('departure');
                        for (var x = 0; x < $scope.FlightConfig[0].FlightList.length; x++) {
                            $scope.priceFilterParam.prices.push($scope.FlightConfig[0].FlightList[x].TotalFare);
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
                    } else {
                        $scope.FlightConfig[0].FlightRequest.FinalProgress = $scope.FlightConfig[0].FlightRequest.Progress;
                    }

                }

                // loop the function
                setTimeout(function () {
                    $scope.FlightFunctions.GetFlight();
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
            for (var x = 0; x < data[i].Trips[0].Airlines.length; x++) {
                data[i].Trips[0].Airlines[x].Checked = true;
            }
            targetScope.FlightList.push(data[i]);
        }
    }// generate flight list end

    // after flight request complete		
    $scope.FlightFunctions.PostRequest = function (targetFlight) {
        console.log('----------------------------------------');
        console.log('Post request function :');
        var targetScope = (targetFlight == 'departure' ? $scope.FlightConfig[0] : $scope.FlightConfig[1]);
        // generate airline for flight filtering		
        for (var i = 0; i < targetScope.FlightList.length; i++) {
            targetScope.FlightList[i].AirlinesTag = [];
            for (var x = 0; x < targetScope.FlightList[i].Trips[0].Airlines.length; x++) {
                targetScope.FlightList[i].AirlinesTag.push(targetScope.FlightList[i].Trips[0].Airlines[x].Code);
                targetScope.FlightFilter.Airline.push(targetScope.FlightList[i].Trips[0].Airlines[x]);
            }
        }
        // remove duplicate from airline filter		
        var dupes = {};
        var Airlines = [];
        $.each(targetScope.FlightFilter.Airline, function (i, el) {
            if (!dupes[el.Code]) {
                dupes[el.Code] = true;
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

            // revalidate flight
            $http.get(RevalidateConfig.Url, {
                params: {
                    SearchId: RevalidateConfig.SearchId,
                    ItinIndex: $scope.FlightConfig[0].FlightList[indexNo].RegisterNumber,
                    SecureCode: $scope.FlightConfig[0].FlightRequest.SecureCode
                }
            }).success(function (returnData) {
                RevalidateConfig.Validated = true;
                console.log(indexNo);
                if (returnData.IsValid == true) {
                    console.log('departure flight available');
                    RevalidateConfig.Available = true;
                    RevalidateConfig.Token = returnData.Token;

                    $('.push-token input').val(RevalidateConfig.Token);
                    $('.push-token').submit();

                } else if (returnData.IsValid == false) {
                    RevalidateConfig.Available = false;
                    $scope.PageConfig.Validating = false;
                    $scope.FlightConfig[0].ActiveFlightAvailable = false;

                    if (returnData.IsOtherFareAvailable == true) {
                        console.log('departure flight has new price');
                        $scope.FlightConfig[0].ActiveFlightNewPrice = returnData.NewFare;
                        RevalidateConfig.NewFare = true;
                        RevalidateConfig.Token = returnData.Token;
                        // update price
                        $scope.FlightConfig[0].FlightList[indexNo].TotalFare = returnData.NewFare;
                        $('.push-token input').val(RevalidateConfig.Token);
                    } else if (returnData.IsOtherFareAvailable == false) {
                        console.log('departure flight is gone');
                        $scope.FlightConfig[0].ActiveFlightNewPrice = -1;
                        RevalidateConfig.NewFare = false;
                        $scope.FlightConfig[0].FlightList[indexNo].Available = false;
                    }
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


    }// revalidate flight end

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
        current: [-1, -1],
        prices: []
    };

    $scope.FlightFiltering.PriceFilter = function () {
        
        return function(flight) {
            if (flight.TotalFare >= $scope.priceFilterParam.current[0] && flight.TotalFare <= $scope.priceFilterParam.current[1]) {
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
                    if (flight.Trips[0].TotalTransit == 0) {
                        return flight;
                    }
                }

                if (targetScope.FlightFilter.Transit[1]) {
                    if (flight.Trips[0].TotalTransit == 1) {
                        return flight;
                    }
                }
                if (targetScope.FlightFilter.Transit[2]) {
                    if (flight.Trips[0].TotalTransit > 1) {
                        return flight;
                    }
                }
            }
            //if (targetScope.FlightFilter.Transit[0]) {
            //    if (flight.Trips[0].TotalTransit == 0) {
            //        return flight;
            //    }
            //}
            //if (targetScope.FlightFilter.Transit[1]) {
            //    if (flight.Trips[0].TotalTransit == 1) {
            //        return flight;
            //    }
            //}
            //if (targetScope.FlightFilter.Transit[2]) {
            //    if (flight.Trips[0].TotalTransit > 1) {
            //        return flight;
            //    }
            //}
        }
    }
    // departure time filter		
    $scope.FlightFiltering.DepartureTimeFilter = function (targetFlight) {
        var targetScope = (targetFlight == 'departure' ? $scope.FlightConfig[0] : $scope.FlightConfig[1]);
        return function (flight) {

            if (!targetScope.FlightFilter.DepartureTime[0] && !targetScope.FlightFilter.DepartureTime[1] &&
                !targetScope.FlightFilter.DepartureTime[2] && !targetScope.FlightFilter.DepartureTime[3]) {
                return flight;
            } else {
                if (targetScope.FlightFilter.DepartureTime[0]) {
                        if ($scope.getHour(flight.Trips[0].Segments[0].DepartureTime) >= 0400 && $scope.getHour(flight.Trips[0].Segments[0].DepartureTime) <= 1100) {
                            return flight;
                        }
                }

                if (targetScope.FlightFilter.DepartureTime[1]) {
                        if ($scope.getHour(flight.Trips[0].Segments[0].DepartureTime) >= 1100 && $scope.getHour(flight.Trips[0].Segments[0].DepartureTime) <= 1500) {
                            return flight;
                        }
                }

                if (targetScope.FlightFilter.DepartureTime[2]) {
                    if ($scope.getHour(flight.Trips[0].Segments[0].DepartureTime) >= 1500 && $scope.getHour(flight.Trips[0].Segments[0].DepartureTime) <= 1900) {
                        return flight;
                    }
                }

                if (targetScope.FlightFilter.DepartureTime[3]) {
                    if ($scope.getHour(flight.Trips[0].Segments[0].DepartureTime) >= 1900 || $scope.getHour(flight.Trips[0].Segments[0].DepartureTime) <= 0400) {
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
                    if ($scope.getHour(flight.Trips[0].Segments[flight.Trips[0].Segments.length - 1].ArrivalTime) >= 0400 && $scope.getHour(flight.Trips[0].Segments[flight.Trips[0].Segments.length - 1].ArrivalTime) <= 1100) {
                        return flight;
                    }
                }
                if (targetScope.FlightFilter.ArrivalTime[1]) {
                    if ($scope.getHour(flight.Trips[0].Segments[flight.Trips[0].Segments.length - 1].ArrivalTime) >= 1100 && $scope.getHour(flight.Trips[0].Segments[flight.Trips[0].Segments.length - 1].ArrivalTime) <= 1500) {
                        return flight;
                    }
                }
                if (targetScope.FlightFilter.ArrivalTime[2]) {
                    if ($scope.getHour(flight.Trips[0].Segments[flight.Trips[0].Segments.length - 1].ArrivalTime) >= 1500 && $scope.getHour(flight.Trips[0].Segments[flight.Trips[0].Segments.length - 1].ArrivalTime) <= 1900) {
                        return flight;
                    }
                }
                if (targetScope.FlightFilter.ArrivalTime[3]) {
                    if ($scope.getHour(flight.Trips[0].Segments[flight.Trips[0].Segments.length - 1].ArrivalTime) >= 1900 || $scope.getHour(flight.Trips[0].Segments[flight.Trips[0].Segments.length - 1].ArrivalTime) <= 0400) {
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
                targetScope.FlightFilter.AirlineSelected.push(targetScope.FlightFilter.Airline[i].Code);
            }
        }

        if (targetScope.FlightFilter.AirlineSelected.length == 0) {
            for (var x = 0;x < targetScope.FlightFilter.Airline.length; x++) {
                targetScope.FlightFilter.AirlineSelected.push(targetScope.FlightFilter.Airline[x].Code);
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