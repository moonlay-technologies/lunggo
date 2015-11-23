app.controller('SingleFlightController', ['$http', '$scope', '$interval', function($http, $scope, $interval) {

    // **********
    // on document ready
    angular.element(document).ready(function () {
        $scope.FlightFunctions.GetFlight();
    });

    // ********************
    // general variables

    $scope.PageConfig = {
        Loaded: true,
        BodyNoScroll: false,
        ActiveSection: 'departure',
        ActiveOverlay: '',
        Busy: false,
        Loading: 0
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
                Trips: FlightSearchConfig.flightForm.trips,
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
                start : function() {}
            }
        }
    ];
    $scope.FlightFunctions = {};


    // ********************
    // general functions

    // set overlay
    $scope.SetOverlay = function(overlay) {
        if (!overlay) {
            $scope.PageConfig.ActiveOverlay = '';
            $scope.PageConfig.NoScroll = false;
        } else {
            $scope.PageConfig.ActiveOverlay = overlay;
            $scope.PageConfig.NoScroll = true;
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

                    // set expiry if progress == 100
                    if ($scope.FlightConfig[0].FlightRequest.Progress == 100) {
                        $scope.FlightConfig[0].expiry.time = returnData.ExpiryTime;
                    } else {
                        $scope.FlightConfig[0].FlightRequest.FinalProgress = $scope.FlightConfig[0].FlightRequest.Progress;
                    }

                    console.log('Progress : ' + $scope.FlightConfig[0].FlightRequest.Progress + ' %');
                    console.log(returnData);

                    // generate flight
                    $scope.FlightFunctions.GenerateFlightList(returnData.FlightList);
                }

                // loop the function
                setTimeout(function () {
                    $scope.FlightFunctions.GetFlight();
                }, 1000);

            }).error(function (returnData) {
                console.log('Failed to get flight list');
                console.log(returnData);
            });

        } else {
            console.log('Finished getting flight list !');
            $scope.PageConfig.Busy = false;
        }

    }

    // generate flight list
    $scope.FlightFunctions.GenerateFlightList = function(data) {
        
        var startNo = $scope.FlightConfig[0].FlightList.length;
        for (var i = 0; i < data.length; i++) {
            data[i].Available = true;
            data[i].IndexNo = (startNo + i);
            $scope.FlightConfig[0].FlightList.push(data[i]);
        }

    }

    // revalidate flight
    $scope.FlightFunctions.Revalidate = function() {
        
    }

    // set active flight
    $scope.FlightFunctions.SetActiveFlight = function (FlightNumber) {
        console.log(FlightNumber);
        if (FlightNumber >= 0) {
            $scope.FlightConfig[0].ActiveFlight = FlightNumber;
            $scope.SetOverlay('flight-detail');
        } else {
            $scope.FlightConfig[0].ActiveFlight = -1;
            $scope.SetOverlay();
        }
    }

    // ms to time
    $scope.FlightFunctions.msToTime = function (duration) {
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


}]);