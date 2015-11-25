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
        FlightList: function() {
            if ($scope.PageConfig.ActiveSection == 'departure') {
                return $scope.FlightConfig[0].FlightList;
            } else {
                return $scope.FlightConfig[1].FlightList;
            }
        },
        ExpiryDate: {
            Expired: false,
            Time: '',
            Start: function () {
                var ExpiryTime = new Date($scope.PageConfig.ExpiryDate.Time);
                if ($scope.PageConfig.ExpiryDate.Expired || $scope.PageConfig.ExpiryDate.Starting) return;
                $interval(function () {
                    $scope.PageConfig.ExpiryDate.Starting = true;
                    var NowTime = new Date();
                    if (NowTime > ExpiryTime) {
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
            },
            FlightExpiry: {
                expired: false,
                time: '',
                start: function () { }
            }
        },
        {
            Name: 'return',
            FlightList: [],
            ActiveFlight: -1,
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
            },
            FlightExpiry: {
                expired: false,
                time: '',
                start: function () { }
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

    $scope.SetPopup = function(popup) {
        if (!popup) {
            $scope.PageConfig.ActivePopup = '';
            $scope.PageConfig.BodyNoScroll = false;
        } else {
            $scope.PageConfig.ActivePopup = popup;
            $scope.PageConfig.BodyNoScroll = true;
        }
    }

    $scope.SetSection = function(section) {
        if (section) {
            $scope.PageConfig.ActiveSection = section;
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
        console.log('Getting flight for : ' + targetScope.Name);
        console.log('Request : '+ targetScope.FlightRequest.Requests);
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

                    // update total progress
                    targetScope.FlightRequest.Progress = ((returnData.MaxRequest - targetScope.FlightRequest.Requests.length) / returnData.MaxRequest) * 100;

                    // set expiry if progress == 100
                    if (targetScope.FlightRequest.Progress == 100) {
                        targetScope.expiry.time = returnData.ExpiryTime;
                    } else {
                        targetScope.FlightRequest.FinalProgress = targetScope.FlightRequest.Progress;
                    }

                    console.log('Progress : ' + targetScope.FlightRequest.Progress + ' %');
                    console.log(returnData);

                    // generate flight
                    $scope.FlightFunctions.GenerateFlightList(targetScope.Name ,returnData.FlightList);
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
    $scope.FlightFunctions.SetActiveFlight = function(FlightNumber) {
        if ($scope.PageConfig.ActiveSection == 'departure') {
            $scope.FlightConfig[0].ActiveFlight = FlightNumber;
        } else {
            $scope.FlightConfig[1].ActiveFlight = FlightNumber;
        }
    }


}]);