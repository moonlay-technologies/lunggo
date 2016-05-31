app.controller('ReturnFlightController', ['$http', '$scope',  '$rootScope','$interval', function($http, $scope, $rootScope, $interval) {

    // **********
    // on document ready
    angular.element(document).ready(function () {
        $scope.FlightFunctions.GetFlight('departure');
        $scope.FlightFunctions.GetFlight('return');
    });
    //$.datepicker.setDefaults(
    //        $.extend(
    //        { 'dateFormat': 'dd-mm-yy' },
    //        $.datepicker.regional['id']
    //        )
    //    );

    // **********
    // variables
    $scope.PageConfig = $rootScope.PageConfig;

    $scope.PageConfig.Loaded = true;
    $scope.PageConfig.ActiveSection = 'departure';
    $scope.PageConfig.ActiveOverlay = '';
    $scope.PageConfig.Loading = 0;
    $scope.PageConfig.Validating = false;
    $scope.PageConfig.ExpiryDate = {
        Expired: false,
        Time: '',
        Start: function () {
            var expiryTime = new Date($scope.PageConfig.ExpiryDate.Time);
            if ($scope.PageConfig.ExpiryDate.Expired || $scope.PageConfig.ExpiryDate.Starting) return;
            $interval(function () {
                $scope.PageConfig.ExpiryDate.Starting = true;
                var nowTime = new Date();
                if (nowTime > expiryTime) {
                    $scope.PageConfig.ExpiryDate.Expired = true;
                }
            }, 1000);
        },
        Starting: false
    }

    $scope.FlightConfig = [
        {
            Name: 'departure',
            FlightList: [],
            ActiveFlight: -1,
            DetailFlight: -1,
            FlightRequest: {
                CabinClass: FlightSearchConfig.flightForm.cabin,
                AdultCount: FlightSearchConfig.flightForm.passenger.adult,
                ChildCount: FlightSearchConfig.flightForm.passenger.child,
                InfantCount: FlightSearchConfig.flightForm.passenger.infant,
                TripType: FlightSearchConfig.flightForm.type,
                Trips: FlightSearchConfig.flightForm.trips[0],
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
                AirlineSelected: [],
                Price: {
                    initial: [-1, -1],
                    current: [0, 1000000000],
                    prices: []
                }
            },
            FlightSort: {
                Label: 'price',
                Value: 'TotalFare',
                Invert: false,
                Set: function(sortBy, invert) {
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
                            $scope.FlightConfig[0].FlightSort.Value = 'Trips[0].Segments[0].DepartureTime';
                            break;
                        case 'arrival':
                            $scope.FlightConfig[0].FlightSort.Value = 'Trips[0].Segments[(Trips[0].Segments.length-1)].ArrivalTime';
                            break;

                    }
                    $scope.SetOverlay('');
                }
            }
        },
        {
            Name: 'return',
            FlightList: [],
            ActiveFlight: -1,
            DetailFlight: -1,
            FlightRequest: {
                CabinClass: FlightSearchConfig.flightForm.cabin,
                AdultCount: FlightSearchConfig.flightForm.passenger.adult,
                ChildCount: FlightSearchConfig.flightForm.passenger.child,
                InfantCount: FlightSearchConfig.flightForm.passenger.infant,
                TripType: FlightSearchConfig.flightForm.type,
                Trips: FlightSearchConfig.flightForm.trips[1],
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
                AirlineSelected: [],
                Price: {
                    initial: [-1, -1],
                    current: [0, 1000000000],
                    prices: []
                }
            },
            FlightSort: {
                Label: 'price',
                Value: 'TotalFare',
                Invert: false,
                Set: function (sortBy, invert) {
                    $scope.FlightConfig[1].FlightSort.Label = sortBy;
                    $scope.FlightConfig[1].FlightSort.Invert = invert;
                    switch (sortBy) {
                        case 'price':
                            $scope.FlightConfig[1].FlightSort.Value = 'TotalFare';
                            break;
                        case 'duration':
                            $scope.FlightConfig[1].FlightSort.Value = 'Trips[0].TotalDuration';
                            break;
                        case 'airline':
                            $scope.FlightConfig[1].FlightSort.Value = 'Trips[0].Airlines[0].Name';
                            break;
                        case 'departure':
                            $scope.FlightConfig[1].FlightSort.Value = 'Trips[0].Segments[0].DepartureTime';
                            break;
                        case 'arrival':
                            $scope.FlightConfig[1].FlightSort.Value = 'Trips[0].Segments[(Trips[0].Segments.length-1)].ArrivalTime';
                            break;
                    }
                    $scope.SetOverlay('');
                }
            }
        }
    ];
    $scope.FlightFunctions = {};

    // **********
    // functions

    // set overlay
    $scope.SetOverlay = function (overlay) {
        if (!overlay) {
            $scope.PageConfig.ActiveOverlay = '';
            $scope.PageConfig.BodyNoScroll = false;
        } else {
            $scope.PageConfig.ActiveOverlay = overlay;
            $scope.PageConfig.BodyNoScroll = true;
        }
    }

    // set popup
    $scope.SetPopup = function(popup) {
        if (!popup) {
            $scope.PageConfig.ActivePopup = '';
            $scope.PageConfig.BodyNoScroll = false;
        } else {
            $scope.PageConfig.ActivePopup = popup;
            $scope.PageConfig.BodyNoScroll = true;
        }
    }

    // set section
    $scope.SetSection = function(section) {
        if (section) {
            $scope.PageConfig.ActiveSection = section;
        }
        console.log('Changing section to : '+section);
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


    // **********
    // get  flight
    $scope.FlightFunctions.GetFlight = function(targetScope) {
        $scope.PageConfig.Busy = true;
        var anotherScope = targetScope == 'departure' ? $scope.FlightConfig[1] : $scope.FlightConfig[0];
        targetScope = targetScope == 'departure' ? $scope.FlightConfig[0] : $scope.FlightConfig[1];
        console.log('Getting flight for : ' + targetScope.Name + ' . Request : '+targetScope.FlightRequest.Requests);
        if (targetScope.FlightRequest.Progress < 100) {

            // **********
            // ajax
            $http.get(FlightSearchConfig.Url, {
                params: {
                    request: targetScope.FlightRequest
                }
            }).success(function (returnData) {
                
                // set searchID
                RevalidateConfig.SearchId = returnData.SearchId;
                targetScope.FlightRequest.SearchId = returnData.SearchId;

                // set flight request if pristine
                if (targetScope.FlightRequest.Pristine == true) {
                    targetScope.FlightRequest.Pristine = false;
                    for (var i = 0; i < returnData.MaxRequest; i++) {
                        targetScope.FlightRequest.Requests.push(i + 1);
                    }
                }

                // if granted request is not null
                if (returnData.GrantedRequests.length) {
                    console.log('Granted request  : ' + returnData.GrantedRequests);
                    targetScope.FlightRequest.SecureCode = returnData.OriginalRequest.SecureCode;
                    targetScope.SecureCode = returnData.OriginalRequest.SecureCode;

                    for (var i = 0; i < returnData.GrantedRequests.length; i++) {
                        // add to completed
                        if (targetScope.FlightRequest.Completed.indexOf(returnData.GrantedRequests[i] < 0)) {
                            targetScope.FlightRequest.Completed.push(returnData.GrantedRequests[i]);
                        }
                        // check current request. Remove if completed
                        if (targetScope.FlightRequest.Requests.indexOf(returnData.GrantedRequests[i] < 0)) {
                            targetScope.FlightRequest.Requests.splice(targetScope.FlightRequest.Requests.indexOf(returnData.GrantedRequests[i]), 1);
                        }

                    }

                    // update total progress
                    targetScope.FlightRequest.Progress = ((returnData.MaxRequest - targetScope.FlightRequest.Requests.length) / returnData.MaxRequest) * 100;
                    console.log('Progress : ' + targetScope.FlightRequest.Progress + ' %');
                    console.log(returnData);

                    // generate flight
                    $scope.FlightFunctions.GenerateFlightList(targetScope.Name, returnData.FlightList);

                    $scope.FlightFunctions.CompleteGetFlight(targetScope.Name);
                    // set expiry if progress == 100
                    if (targetScope.FlightRequest.Progress == 100) {

                        $scope.PageConfig.ExpiryDate.Time = returnData.ExpiryTime;
                        $scope.PageConfig.ExpiryDate.Start();
                        //$scope.FlightFunctions.CompleteGetFlight(targetScope.Name);
                        for (var x = 0; x < $scope.FlightConfig[0].FlightList.length; x++) {
                            $scope.FlightConfig[0].FlightFilter.Price.prices.push($scope.FlightConfig[0].FlightList[x].TotalFare);
                        }
                        for (var y = 0; y < $scope.FlightConfig[1].FlightList.length; y++) {
                            $scope.FlightConfig[1].FlightFilter.Price.prices.push($scope.FlightConfig[1].FlightList[y].TotalFare);
                        }
                        var a = 3;

                        function sortNumber(a, b) {
                            return a - b;
                        }
                        $scope.FlightConfig[0].FlightFilter.Price.prices.sort(sortNumber);
                        $scope.FlightConfig[0].FlightFilter.Price.initial[0] = Math.floor($scope.FlightConfig[0].FlightFilter.Price.prices[0]);
                        $scope.FlightConfig[0].FlightFilter.Price.initial[1] = Math.round($scope.FlightConfig[0].FlightFilter.Price.prices[$scope.FlightConfig[0].FlightFilter.Price.prices.length - 1]);

                        $scope.FlightConfig[0].FlightFilter.Price.current[0] = Math.floor($scope.FlightConfig[0].FlightFilter.Price.prices[0]);
                        $scope.FlightConfig[0].FlightFilter.Price.current[1] = Math.round($scope.FlightConfig[0].FlightFilter.Price.prices[$scope.FlightConfig[0].FlightFilter.Price.prices.length - 1]);

                        $scope.FlightConfig[1].FlightFilter.Price.prices.sort(sortNumber);
                        $scope.FlightConfig[1].FlightFilter.Price.initial[0] = Math.floor($scope.FlightConfig[1].FlightFilter.Price.prices[0]);
                        $scope.FlightConfig[1].FlightFilter.Price.initial[1] = Math.round($scope.FlightConfig[1].FlightFilter.Price.prices[$scope.FlightConfig[1].FlightFilter.Price.prices.length - 1]);

                        $scope.FlightConfig[1].FlightFilter.Price.current[0] = Math.floor($scope.FlightConfig[1].FlightFilter.Price.prices[0]);
                        $scope.FlightConfig[1].FlightFilter.Price.current[1] = Math.round($scope.FlightConfig[1].FlightFilter.Price.prices[$scope.FlightConfig[1].FlightFilter.Price.prices.length - 1]);

                        $('.departure-price-slider').slider({
                            range: true,
                            min: $scope.FlightConfig[0].FlightFilter.Price.initial[0],
                            max: $scope.FlightConfig[0].FlightFilter.Price.initial[1],
                            step: 100,
                            values: [$scope.FlightConfig[0].FlightFilter.Price.initial[0], $scope.FlightConfig[0].FlightFilter.Price.initial[1]],
                            create: function (event, ui) {
                                $('.departure-price-slider-min').val($scope.FlightConfig[0].FlightFilter.Price.initial[0]);
                                $('.departure-price-slider-min').trigger('input');
                                $('.departure-price-slider-max').val($scope.FlightConfig[0].FlightFilter.Price.initial[1]);
                                $('.departure-price-slider-max').trigger('input');
                            },
                            slide: function (event, ui) {
                                $('.departure-price-slider-min').val(ui.values[0]);
                                $('.departure-price-slider-min').trigger('input');
                                $('.departure-price-slider-max').val(ui.values[1]);
                                $('.departure-price-slider-max').trigger('input');
                            }
                        });

                        $('.return-price-slider').slider({
                            range: true,
                            min: $scope.FlightConfig[1].FlightFilter.Price.initial[0],
                            max: $scope.FlightConfig[1].FlightFilter.Price.initial[1],
                            step: 100,
                            values: [$scope.FlightConfig[1].FlightFilter.Price.initial[0], $scope.FlightConfig[1].FlightFilter.Price.initial[1]],
                            create: function (event, ui) {
                                $('.return-price-slider-min').val($scope.FlightConfig[1].FlightFilter.Price.initial[0]);
                                $('.return-price-slider-min').trigger('input');
                                $('.return-price-slider-max').val($scope.FlightConfig[1].FlightFilter.Price.initial[1]);
                                $('.return-price-slider-max').trigger('input');
                            },
                            slide: function (event, ui) {
                                $('.return-price-slider-min').val(ui.values[0]);
                                $('.return-price-slider-min').trigger('input');
                                $('.return-price-slider-max').val(ui.values[1]);
                                $('.return-price-slider-max').trigger('input');
                            }
                        });

                    } else {
                        targetScope.FlightRequest.FinalProgress = targetScope.FlightRequest.Progress;
                    }

                }

                // loop the function
                setTimeout(function () {
                    $scope.FlightFunctions.GetFlight(targetScope.Name);
                }, 1000);

            }).error(function (returnData) {
                console.log('Failed to get flight list');
                console.log(returnData);
                for (var i = 0; i < targetScope.FlightRequest.Requests.length; i++) {
                    // add to completed
                    if (targetScope.FlightRequest.Completed.indexOf(targetScope.FlightRequest.Requests[i] < 0)) {
                        targetScope.FlightRequest.Completed.push(targetScope.FlightRequest.Requests[i]);
                    }
                    // check current request. Remove if completed
                    if (targetScope.FlightRequest.Requests.indexOf(targetScope.FlightRequest.Requests[i] < 0)) {
                        targetScope.FlightRequest.Requests.splice(targetScope.FlightRequest.Requests.indexOf(targetScope.FlightRequest.Requests[i]), 1);
                    }
                }
                targetScope.FlightRequest.Progress = 100;
                targetScope.FlightRequest.FinalProgress = 100;

            });

        } else {
            console.log('Finished getting flight list !');
            $scope.PageConfig.Busy = false;
        }


    }

    // arrange flight
    $scope.FlightFunctions.GenerateFlightList = function(targetScope, data) {
        targetScope = targetScope == 'departure' ? $scope.FlightConfig[0] : $scope.FlightConfig[1] ;
        var startNo = targetScope.FlightList.length;
        for (var i = 0; i < data.length; i++) {
            data[i].Available = true;
            data[i].IndexNo = (startNo + i);
            // init airlines
            for (var x = 0; x < data[i].Trips[0].Airlines.length; x++) {
                data[i].Trips[0].Airlines[x].Checked = true;
            }
            targetScope.FlightList.push(data[i]);
        }

    }

    // run if departure and return flight search has completed
    $scope.FlightFunctions.CompleteGetFlight = function (targetScope) {
        console.log('/--------------------------/');
        console.log('Post get flight for : '+targetScope);
        targetScope = targetScope == 'departure' ? $scope.FlightConfig[0] : $scope.FlightConfig[1];
        // generate airline filter
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
        console.log(targetScope);

    }

    // set active flight
    $scope.FlightFunctions.SetActiveFlight = function (targetScope, flightNumber) {
        if (targetScope) {
            targetScope = targetScope == 'departure' ? $scope.FlightConfig[0] : $scope.FlightConfig[1];
            if (flightNumber >= 0) {
                targetScope.ActiveFlight = flightNumber;

                if ($scope.FlightConfig[0].ActiveFlight != -1 && $scope.FlightConfig[1].ActiveFlight != -1) {
                    $scope.SetOverlay('summary'); 
                    //$scope.PageConfig.Validating = false;
                    //$scope.FlightFunctions.Revalidate($scope.FlightConfig[0].ActiveFlight, $scope.FlightConfig[1].ActiveFlight);
                } else if ($scope.FlightConfig[0].ActiveFlight >= 0 && $scope.FlightConfig[1].ActiveFlight < 0) {
                    $scope.SetPopup('roundtrip-return');
                } else if ($scope.FlightConfig[0].ActiveFlight < 0 && $scope.FlightConfig[1].ActiveFlight >= 0) {
                    $scope.SetPopup('roundtrip-departure');
                }

            } else {
                targetScope.ActiveFlight = -1;
            }
        }
        
    }

    // swap flight
    $scope.FlightFunctions.SwapFlight = function() {
        if ($scope.PageConfig.ActiveSection == 'departure') {
            $scope.SetPopup('roundtrip-return');
        } else {
            $scope.SetPopup('roundtrip-departure');
        }
    }

    // show flight detail
    $scope.FlightFunctions.ShowDetail = function (targetScope, flightNumber) {
        
        targetScope = targetScope == 'departure' ? $scope.FlightConfig[0] : $scope.FlightConfig[1];
        // set detail flight
        targetScope.DetailFlight = flightNumber;
        $scope.SetOverlay('flight-detail');
    }

    // revalidate flight
    $scope.FlightFunctions.Revalidate = function(departureIndexNo, returnIndexNo) {

        $scope.PageConfig.Validating = true;
        console.log('Validating flight no : ' + departureIndexNo + ' & ' + returnIndexNo);

        // **********
        // validate flight function
        var validateFlight = function (targetFlight, indexNo) {
            var anotherFlight = targetFlight == 'departure' ? $scope.FlightConfig[1] : $scope.FlightConfig[0];
            targetFlight = targetFlight == 'departure' ? $scope.FlightConfig[0] : $scope.FlightConfig[1];
            var secureCode = targetFlight.SecureCode;

            targetFlight.FlightValidating = true;
            targetFlight.FlightValidated = false;

            $http.get(RevalidateConfig.Url, {
                params: {
                    SearchId: targetFlight.FlightRequest.SearchId,
                    ItinIndex: targetFlight.FlightList[indexNo].RegisterNumber,
                    SecureCode: secureCode
                }
            }).success(function (returnData) {

                targetFlight.FlightValidating = false;
                targetFlight.FlightValidated = true;

                if (returnData.IsValid == true) {
                    targetFlight.FlightAvailable = true;
                    targetFlight.Token = returnData.Token;
                    console.log(targetFlight.Name + ' flight available');

                } else if (returnData.IsValid == false) {
                    targetFlight.FlightAvailable = false;

                    if (returnData.IsOtherFareAvailable == true) {
                        targetFlight.FlightNewPrice = true;
                        targetFlight.FlightList[indexNo].TotalFare = returnData.NewFare;
                        targetFlight.Token = returnData.Token;
                        console.log(targetFlight.Name+' flight has new price');

                    } else if (returnData.IsOtherFareAvailable == false) {
                        targetFlight.FlightNewPrice = false;
                        targetFlight.FlightList[indexNo].Available = false;
                        console.log(targetFlight.Name+' flight is gone');

                    }
                }

                if (anotherFlight.FlightValidated) {
                    afterValidate();
                }

                console.log(targetFlight);
            }).error(function (returnData) {
                console.log('ERROR Validating Flight');
                console.log(returnData);
                console.log('--------------------');
            });

        }

        // **********
        // start validate
        if ($scope.PageConfig.Validated && $scope.PageConfig.ValidateConfirmation) {
            var fareToken = $scope.FlightConfig[0].Token + '.' + $scope.FlightConfig[1].Token;
            console.log('Token : ' + fareToken);
            $('.pushToken .fareToken').val(fareToken);
            $('.pushToken').submit();
        } else {
            validateFlight('departure', departureIndexNo);
            validateFlight('return', returnIndexNo);
        }

        // **********
        // after departure flight and return flight validated
        var afterValidate = function () {
            console.log('Flights validated');
            $scope.PageConfig.Validated = true;

            // if both flight available
            if ($scope.FlightConfig[0].FlightAvailable && $scope.FlightConfig[1].FlightAvailable) {
                console.log('Flights available. Will be redirected shortly');
                var fareToken = $scope.FlightConfig[0].Token + '.' + $scope.FlightConfig[1].Token;
                console.log('Token : ' + fareToken);
                $('.pushToken .fareToken').val(fareToken);
                $('.pushToken').submit();
            } else {
                if ((!$scope.FlightConfig[0].FlightAvailable && !$scope.FlightConfig[0].FlightNewPrice) || (!$scope.FlightConfig[1].FlightAvailable && !$scope.FlightConfig[1].FlightNewPrice)) {
                    $scope.PageConfig.ValidateConfirmation = false;
                } else {
                    $scope.PageConfig.ValidateConfirmation = true;
                }
                console.log($scope.PageConfig.ValidateConfirmation);
                $scope.PageConfig.Validating = false;
            }

        }


    }

    $scope.FlightFunctions.ResetValidation = function () {
        $scope.PageConfig.ValidateConfirmation = false;
        $scope.PageConfig.Validating = false;
        $scope.PageConfig.Validated = false;
        $scope.FlightConfig[0].FlightValidated = false;
        $scope.FlightConfig[1].FlightValidated = false;
    }

    // *****
    // flight filtering functions
    $scope.FlightFiltering = {};
    $scope.FlightFiltering.Touched = [false,false];
    // available filter		
    $scope.FlightFiltering.AvailableFilter = function () {
        return function (flight) {
            if (flight.Available) {
                return flight;
            }
        }
    }

    // price filter
    $scope.priceFilter = function(targetFlight) {
        return function (flight) {
            if (targetFlight == 'departure') {
                if (flight.TotalFare >= $scope.FlightConfig[0].FlightFilter.Price.current[0] && flight.TotalFare <= $scope.FlightConfig[0].FlightFilter.Price.current[1]) {
                    return flight;
                }
            } else {
                if (flight.TotalFare >= $scope.FlightConfig[1].FlightFilter.Price.current[0] && flight.TotalFare <= $scope.FlightConfig[1].FlightFilter.Price.current[1]) {
                    return flight;
                }
            }
            
        }
    }

    // transit filter		
    $scope.FlightFiltering.TransitFilter = function (targetFlight) {
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
        if ( targetScope.Name == 'departure' ) {
            $scope.FlightFiltering.Touched[0] = true;
        } else {
            $scope.FlightFiltering.Touched[1] = true;
        }
        for (var i = 0; i < targetScope.FlightFilter.Airline.length; i++) {
            if (!targetScope.FlightFilter.Airline[i].Checked) {
                targetScope.FlightFilter.AirlineSelected.push(targetScope.FlightFilter.Airline[i].Code);
            }
        }

        if (targetScope.FlightFilter.AirlineSelected.length == 0) {
            for (var x = 0; x < targetScope.FlightFilter.Airline.length; x++) {
                targetScope.FlightFilter.AirlineSelected.push(targetScope.FlightFilter.Airline[x].Code);
            }
        }
    }
    $scope.FlightFiltering.AirlineFilter = function (targetFlight) {
        var targetScope = (targetFlight == 'departure' ? $scope.FlightConfig[0] : $scope.FlightConfig[1]);
        var touched;
        return function (flight) {
            if (targetScope.Name == 'departure') {
                touched = $scope.FlightFiltering.Touched[0];
            } else {
                touched = $scope.FlightFiltering.Touched[1];
            }
            if (touched == false) {
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