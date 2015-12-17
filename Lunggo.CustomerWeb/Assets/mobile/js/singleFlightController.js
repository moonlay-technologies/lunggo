app.controller('SingleFlightController', ['$http', '$scope', '$rootScope', '$interval', function($http, $scope, $rootScope, $interval) {

    // **********
    // on document ready
    angular.element(document).ready(function () {
        $scope.FlightFunctions.GetFlight();
    });

    // ********************
    // variables

    $scope.PageConfig = $rootScope.PageConfig;

    // set additional variables for Flight Page
    $scope.PageConfig.ActiveSection = 'departure';
    $scope.PageConfig.ActiveOverlay = '';
    $scope.PageConfig.Loading = 0;
    $scope.PageConfig.Validating = false;
    $scope.PageConfig.ExpiryDate = {
        Expired: false,
        Time: '',
        Start: function() {
            var ExpiryTime = new Date($scope.PageConfig.ExpiryDate.Time);
            if ($scope.PageConfig.ExpiryDate.Expired || $scope.PageConfig.ExpiryDate.Starting) return;
            $interval(function() {
                $scope.PageConfig.ExpiryDate.Starting = true;
                var NowTime = new Date();
                if (NowTime > ExpiryTime) {
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
    // functions

    // set overlay
    $scope.SetOverlay = function(overlay) {
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
                        $scope.FlightConfig[0].FlightExpiry.time = returnData.ExpiryTime;
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
    $scope.FlightFunctions.GenerateFlightList = function(data) {
        var startNo = $scope.FlightConfig[0].FlightList.length;
        for (var i = 0; i < data.length; i++) {
            data[i].Available = true;
            data[i].IndexNo = (startNo + i);
            $scope.FlightConfig[0].FlightList.push(data[i]);
        }
    }// generate flight list end

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
            console.log('SKIP TO BOOK');

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


}]);