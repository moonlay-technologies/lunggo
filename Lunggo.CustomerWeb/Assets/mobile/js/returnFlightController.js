app.controller('ReturnFlightController', ['$http', '$scope',  '$rootScope','$interval', function($http, $scope, $rootScope, $interval) {

    // **********
    // on document ready
    angular.element(document).ready(function () {
        var a = $scope.flightFixRequest();
        //$scope.FlightFunctions.GetFlight('departure');
        $scope.FlightFunctions.GetFlight('return');
    });

    // **********
    // variables
    $scope.trial = 0;
    $scope.PageConfig = $rootScope.PageConfig;
    $scope.PageConfig.Validated = false;
    $scope.PageConfig.ValidateConfirmation = false;
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

        return Math.min.apply(Math, listbaggage);
    }

    $scope.checkBaggageNaN = function (val) {
        return Number.isNaN(val);
    }

    $scope.checkTax = function (trip) {
        var valid = true;
        for (var x = 0; x < trip.segments.length; x++) {
            if (trip.segments[x].includedPsc) {
                valid = false;
            }
        }
        return valid;
    }

    $scope.FlightConfig = [
        {
            Token: '',
            Name: 'departure',
            FlightList: [],
            ActiveFlight: -1,
            DetailFlight: -1,
            //flightSearchParams: $scope.flightFixRequest(),
            FlightRequest: {
                CabinClass: FlightSearchConfig.flightForm.cabin,
                AdultCount: FlightSearchConfig.flightForm.passenger.adult,
                ChildCount: FlightSearchConfig.flightForm.passenger.child,
                InfantCount: FlightSearchConfig.flightForm.passenger.infant,
                TripType: FlightSearchConfig.flightForm.type,
                trips: FlightSearchConfig.flightForm.trips[0],
                Requests: [],
                Completed: [],
                Securecode: FlightSearchConfig.flightForm.SecureCode,
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
                Value: 'originalAdultFare',
                Invert: false,
                Set: function(sortBy, invert) {
                    $scope.FlightConfig[0].FlightSort.Label = sortBy;
                    $scope.FlightConfig[0].FlightSort.Invert = invert;
                    switch (sortBy) {
                        case 'price':
                            $scope.FlightConfig[0].FlightSort.Value = 'originalAdultFare';
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
            }
        },
        {
            Name: 'return',
            //flightSearchParams: $scope.flightFixRequest(),
            Token: '',
            FlightList: [],
            ActiveFlight: -1,
            DetailFlight: -1,
            FlightRequest: {
                CabinClass: FlightSearchConfig.flightForm.cabin,
                AdultCount: FlightSearchConfig.flightForm.passenger.adult,
                ChildCount: FlightSearchConfig.flightForm.passenger.child,
                InfantCount: FlightSearchConfig.flightForm.passenger.infant,
                TripType: FlightSearchConfig.flightForm.type,
                trips: FlightSearchConfig.flightForm.trips[1],
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
                Value: 'originalAdultFare',
                Invert: false,
                Set: function (sortBy, invert) {
                    $scope.FlightConfig[1].FlightSort.Label = sortBy;
                    $scope.FlightConfig[1].FlightSort.Invert = invert;
                    switch (sortBy) {
                        case 'price':
                            $scope.FlightConfig[1].FlightSort.Value = 'originalAdultFare';
                            break;
                        case 'duration':
                            $scope.FlightConfig[1].FlightSort.Value = 'trips[0].totalDuration';
                            break;
                        case 'airline':
                            $scope.FlightConfig[1].FlightSort.Value = 'trips[0].airlines[0].name';
                            break;
                        case 'departure':
                            $scope.FlightConfig[1].FlightSort.Value = 'trips[0].segments[0].departureTime';
                            break;
                        case 'arrival':
                            $scope.FlightConfig[1].FlightSort.Value = 'trips[0].segments[(trips[0].segments.length-1)].arrivalTime';
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
            departureDate = Date.UTC(departureDate.getUTCFullYear(), (departureDate.getUTCMonth() + 1), departureDate.getUTCDate());
            arrivalDate = new Date(arrivalDate);
            arrivalDate = Date.UTC(arrivalDate.getUTCFullYear(), (arrivalDate.getUTCMonth() + 1), arrivalDate.getUTCDate());
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
        var departureTemp = FlightSearchConfig.flightForm;
        var depDate = new Date(departureTemp.trips[0][0].DepartureDate) || '';
        var departureDate = (('0' + depDate.getDate()).slice(-2) + ('0' + (depDate.getMonth() + 1)).slice(-2) + depDate.getFullYear().toString().substr(2, 2));
        var departureRequest = departureTemp.trips[0][0].OriginAirport + departureTemp.trips[0][0].DestinationAirport + departureDate;
        var returnTemp = departureTemp ;
        var retDate = new Date(returnTemp.trips[1][0].DepartureDate) || '';
        var returnDate = (('0' + retDate.getDate()).slice(-2) + ('0' + (retDate.getMonth() + 1)).slice(-2) + retDate.getFullYear().toString().substr(2, 2));
        var returnRequest = returnTemp.trips[1][0].OriginAirport + returnTemp.trips[1][0].DestinationAirport + returnDate;
        
        var passenger = FlightSearchConfig.flightForm.passenger.adult + '' + FlightSearchConfig.flightForm.passenger.child + '' + FlightSearchConfig.flightForm.passenger.infant + cabin;
        return departureRequest + '~' + returnRequest + '-' + passenger;
    }
    
    // **********
    $scope.arrangeFlightData = function (targetScope, data) {
        if (targetScope == "departure" || targetScope == "Departure") {
            targetScope = $scope.FlightConfig[0];
        } else {
            targetScope = $scope.FlightConfig[1];
        }

        var startNumber = targetScope.FlightList.length;

        for (var i = 0; i < data.length; i++) {
            data[i].Available = true;
            data[i].IndexNo = (startNumber + i);
            targetScope.FlightList.push(data[i]);
            for (var x = 0; x < data[i].trips[0].airlines.length; x++) {
                data[i].trips[0].airlines[x].Checked = true;
            }
        }

        if (targetScope.FlightRequest.Progress == 100) {

            // start expiry date
            $scope.PageConfig.ExpiryDate.Start();

            // loop the result
            for (var i = 0; i < targetScope.FlightList.length; i++) {
                // populate prices
                targetScope.FlightFilter.Price.prices.push(targetScope.FlightList[i].originalAdultFare);

                // populate airline code
                targetScope.FlightList[i].AirlinesTag = [];
                for (var x = 0; x < targetScope.FlightList[i].trips[0].airlines.length; x++) {
                    targetScope.FlightFilter.Airline.push(targetScope.FlightList[i].trips[0].airlines[x]);
                    targetScope.FlightList[i].AirlinesTag.push(targetScope.FlightList[i].trips[0].airlines[x].code);
                }
            }

            function sortNumber(a, b) {
                return a - b;
            }

            targetScope.FlightFilter.Price.prices.sort(sortNumber);
            targetScope.FlightFilter.Price.initial[0] = Math.floor(targetScope.FlightFilter.Price.prices[0]);
            targetScope.FlightFilter.Price.initial[1] = Math.round(targetScope.FlightFilter.Price.prices[targetScope.FlightFilter.Price.prices.length - 1]);

            targetScope.FlightFilter.Price.current[0] = Math.floor(targetScope.FlightFilter.Price.prices[0]);
            targetScope.FlightFilter.Price.current[1] = Math.round(targetScope.FlightFilter.Price.prices[targetScope.FlightFilter.Price.prices.length - 1]);
            
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
        }
    }

    // get  flight
    $scope.FlightFunctions.GetFlight = function (targetScope) {
        if ($scope.trial > 3) {
            $scope.trial = 0;
        }
        $scope.PageConfig.Busy = true;
        var requestReturnParams = $scope.flightFixRequest();
        targetScope = $scope.FlightConfig[1];
        console.log('Getting flight for : ' + targetScope.Name + ' . Request : '+targetScope.FlightRequest.Requests);
        if (targetScope.FlightRequest.Progress < 100) {

            // **********
            // ajax
                var authAccess = getAuthAccess();
                if (authAccess == 1 || authAccess == 2)
                {
                    $http({
                        method: 'GET',
                        url: FlightSearchConfig.Url + requestReturnParams + '/' + targetScope.FlightRequest.Progress,
                        headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }

                    }).then(function (returnData) {

                        // set searchID
                        RevalidateConfig.SearchId = $scope.flightFixRequest();
                        if (!$scope.PageConfig.ExpiryDate) {
                            $scope.PageConfig.ExpiryDate.Time = returnData.data.expTime;
                            $scope.FlightConfig[0].FlightRequest.FinalProgress = targetScope.progress;
                        }

                        if (targetScope.FlightRequest.Progress < 100) {
                            targetScope.FlightRequest.FinalProgress = targetScope.FlightRequest.Progress; // change this
                            $scope.FlightConfig[0].FlightRequest.FinalProgress = targetScope.FlightRequest.Progress;
                        }

                        targetScope.FlightRequest.Progress = returnData.data.progress;
                        $scope.FlightConfig[0].FlightRequest.Progress = returnData.data.progress;

                        // if granted request is not null
                        if (returnData.data.flights) {
                            // update total progress
                            targetScope.FlightRequest.Progress = returnData.data.progress;
                            console.log('Progress : ' + targetScope.FlightRequest.Progress + ' %');
                            console.log(returnData);

                            if (returnData.data.flights.length != 0) {
                                if (returnData.data.flights[0].options.length) {
                                    $scope.arrangeFlightData('departure', returnData.data.flights[0].options); // For Departure Flight
                                }
                                if (returnData.data.flights[1].options.length) {
                                    $scope.arrangeFlightData('return', returnData.data.flights[1].options); // For Return Flight
                                }
                            }

                            // generate flight
                            //$scope.FlightFunctions.GenerateFlightList(targetScope.Name, returnData.FlightList);                   
                        }

                        // loop the function
                        setTimeout(function () {
                            $scope.FlightFunctions.GetFlight(targetScope.Name);
                        }, 1000);

                    }).catch(function (returnData) {
                        $scope.trial++;
                        if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                        {
                            $scope.FlightFunctions.GetFlight(targetScope.Name);
                        }
                        else {
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

                        }
                    });
                }
                else
                {
                    targetScope.FlightRequest.Progress = 100;
                    targetScope.FlightRequest.FinalProgress = 100;
                    console.log('Not Authorized');
                }
                

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
            for (var x = 0; x < data[i].trips[0].airlines.length; x++) {
                data[i].trips[0].airlines[x].Checked = true;
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
        var validateFlight = function () {
            if ($scope.trial > 3) {
                $scope.trial = 0;
            }
            //var anotherFlight = targetFlight == 'departure' ? $scope.FlightConfig[1] : $scope.FlightConfig[0];
            //targetFlight = targetFlight == 'departure' ? $scope.FlightConfig[0] : $scope.FlightConfig[1];
            //var securecode = targetFlight.Securecode;

            //targetFlight.FlightValidating = true;
            //targetFlight.FlightValidated = false;
            //method: 'POST',
            //url: SelectConfig.Url,
            //data: {
            //    searchId: $scope.returnFlightConfig.searchId,
            //    regs: [$scope.departureFlightConfig.flightList[departureFlightIndexNo].reg, $scope.returnFlightConfig.flightList[returnFlightIndexNo].reg],
            //    },
            //headers: { 'Authorization': $scope.getFlightHeader }
            var authAccess = getAuthAccess();
            if (authAccess == 1 || authAccess == 2)
            {
                $http({
                    method: 'POST',
                    url: SelectConfig.Url,
                    data: {
                        searchId: $scope.flightFixRequest(),
                        regs: [$scope.FlightConfig[0].FlightList[departureIndexNo].reg, $scope.FlightConfig[1].FlightList[returnIndexNo].reg],
                    },
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).then(function (returnData) {

                    $scope.FlightConfig[0].FlightValidating = false;
                    $scope.FlightConfig[1].FlightValidating = false;
                    $scope.FlightConfig[0].FlightValidated = true;
                    $scope.FlightConfig[1].FlightValidated = true;

                    if ((returnData.data.token != "" || returnData.data.token != null) && returnData.data.status == "200") {
                        console.log('flight available');
                        $scope.FlightConfig[0].FlightAvailable = true;
                        $scope.FlightConfig[0].Token = returnData.data.token;
                        $scope.FlightConfig[1].FlightAvailable = true;
                        $scope.FlightConfig[1].Token = returnData.data.token;

                    }
                    else {
                        console.log('flight unavailable');
                        $scope.FlightConfig[0].validateAvailable = false;
                        $scope.FlightConfig[1].validateAvailable = false;
                    }

                    //if (anotherFlight.FlightValidated) {
                    afterValidate();
                    //$scope.PageConfig.Validated = true;
                    //}

                    console.log(targetFlight);
                }).catch(function (returnData) {
                    $scope.trial++;
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.FlightFunctions.Revalidate(departureIndexNo, returnIndexNo);
                    }
                    else {
                        console.log('ERROR Validating Flight');
                        console.log(returnData);
                        console.log('--------------------');
                    }
                    
                });
            }
            else
            {
                $scope.FlightConfig[0].validateAvailable = false;
                $scope.FlightConfig[1].validateAvailable = false;
                console.log('Not Authorized');
            }
            
        }

        // **********
        // start validate
        if ($scope.PageConfig.Validated && $scope.PageConfig.ValidateConfirmation) {
            var fareToken = $scope.FlightConfig[0].Token + '.' + $scope.FlightConfig[1].Token;
            console.log('Token : ' + fareToken);
            $('.pushToken .fareToken').val(fareToken);
            $('.pushToken').submit();
        } else {
            validateFlight();
            //validateFlight('return', returnIndexNo);
        }

        // **********
        // after departure flight and return flight validated
        var afterValidate = function () {
            console.log('Flights validated');
            $scope.PageConfig.Validated = true;

            // if both flight available
            if ($scope.FlightConfig[0].FlightAvailable && $scope.FlightConfig[1].FlightAvailable) {
                console.log('Flights available. Will be redirected shortly');
                var fareToken = $scope.FlightConfig[0].Token;
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
                if (flight.originalAdultFare >= $scope.FlightConfig[0].FlightFilter.Price.current[0] && flight.originalAdultFare <= $scope.FlightConfig[0].FlightFilter.Price.current[1]) {
                    return flight;
                }
            } else {
                if (flight.originalAdultFare >= $scope.FlightConfig[1].FlightFilter.Price.current[0] && flight.originalAdultFare <= $scope.FlightConfig[1].FlightFilter.Price.current[1]) {
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
            //    if (flight.trips[0].TotalTransit == 0) {
            //        return flight;
            //    }
            //}
            //if (targetScope.FlightFilter.Transit[1]) {
            //    if (flight.trips[0].TotalTransit == 1) {
            //        return flight;
            //    }
            //}
            //if (targetScope.FlightFilter.Transit[2]) {
            //    if (flight.trips[0].TotalTransit > 1) {
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
                    if ($scope.getHour(flight.trips[0].segments[0].departureTime) >= 1900 || $scope.getHour(flight.trips[0].segments[0].departureTime) <= 0400) {
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
        if ( targetScope.Name == 'departure' ) {
            $scope.FlightFiltering.Touched[0] = true;
        } else {
            $scope.FlightFiltering.Touched[1] = true;
        }
        for (var i = 0; i < targetScope.FlightFilter.Airline.length; i++) {
            if (!targetScope.FlightFilter.Airline[i].Checked) {
                targetScope.FlightFilter.AirlineSelected.push(targetScope.FlightFilter.Airline[i].code);
            }
        }

        if (targetScope.FlightFilter.AirlineSelected.length == 0) {
            for (var x = 0; x < targetScope.FlightFilter.Airline.length; x++) {
                targetScope.FlightFilter.AirlineSelected.push(targetScope.FlightFilter.Airline[x].code);
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