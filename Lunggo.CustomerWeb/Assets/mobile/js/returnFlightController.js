app.controller('ReturnFlightController', ['$http', '$scope', '$interval', function($http, $scope, $interval) {

    // **********
    // on document ready
    angular.element(document).ready(function () {
        $scope.FlightFunctions.GetFlight('departure');
        $scope.FlightFunctions.GetFlight('return');
    });


    // **********
    // variables
    $scope.PageConfig = {
        Loaded: true,
        BodyNoScroll: false,
        ActiveSection: 'departure',
        ActiveOverlay: '',
        Busy: false,
        Loading: 0,
        Validating: false,
        FlightList: [],
        ExpiryDate: {
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
    };

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
            FlightFilter: {},
            FlightSort: {
                label: '',
                value: ''
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
            FlightFilter: {},
            FlightSort: {
                label: '',
                value: ''
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
        return hours + "h " + minutes + "m";
    }


    // **********
    // get  flight
    $scope.FlightFunctions.GetFlight = function(targetScope) {
        $scope.PageConfig.Busy = true;
        if (targetScope == "departure") {
            targetScope = $scope.FlightConfig[0];
        } else {
            targetScope = $scope.FlightConfig[1];
        }
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

                    // generate flight
                    $scope.FlightFunctions.GenerateFlightList(targetScope.Name, returnData.FlightList);

                    // update total progress
                    targetScope.FlightRequest.Progress = ((returnData.MaxRequest - targetScope.FlightRequest.Requests.length) / returnData.MaxRequest) * 100;

                    console.log('Progress : ' + targetScope.FlightRequest.Progress + ' %');
                    console.log(returnData);

                    // set expiry if progress == 100
                    if (targetScope.FlightRequest.Progress == 100) {

                        $scope.PageConfig.ExpiryDate.Time = returnData.ExpiryTime;
                        console.log($scope.FlightConfig);

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
        if (targetScope == 'departure') {
            targetScope = $scope.FlightConfig[0];
        } else {
            targetScope = $scope.FlightConfig[1];
        }

        var startNo = targetScope.FlightList.length;
        for (var i = 0; i < data.length; i++) {
            data[i].Available = true;
            data[i].IndexNo = (startNo + i);
            targetScope.FlightList.push(data[i]);
        }

    }

    // run if departure and return flight search has completed
    $scope.FlightFunctions.CompleteGetFlight = function() {
        
    }

    // set active flight
    $scope.FlightFunctions.SetActiveFlight = function (targetScope, flightNumber) {
        if (targetScope) {
            targetScope = targetScope == 'departure' ? $scope.FlightConfig[0] : $scope.FlightConfig[1];
            if (flightNumber >= 0) {
                targetScope.ActiveFlight = flightNumber;

                if ($scope.FlightConfig.ActiveFlight != -1 && $scope.FlightConfig[1].ActiveFlight != -1) {
                    $scope.SetOverlay('summary');
                }

            } else {
                targetScope.ActiveFlight = -1;
            }
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

        var flightValidated = [false,false];

        // **********
        // validate flight function
        var validateFlight = function(targetFlight, indexNo) {
            targetFlight = targetFlight == 'departure' ? flightValidated[0] : flightValidated[1];
            var secureCode = targetFlight == 'departure' ? $scope.FlightConfig[0].FlightRequest.SecureCode : $scope.FlightConfig[1].FlightRequest.SecureCode;
            var targetScope = targetFlight == 'departure' ? $scope.FlightConfig[0] : $scope.FlightConfig[1];

            $http.get(RevalidateConfig.Url, {
                params: {
                    SearchId: RevalidateConfig.SearchId,
                    ItinIndex: indexNo,
                    SecureCode: secureCode
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

        }
        // after validate function

        // **********
        // start validate
        validateFlight('departure', departureIndexNo);
        validateFlight('return', returnIndexNo);


    }


}]);