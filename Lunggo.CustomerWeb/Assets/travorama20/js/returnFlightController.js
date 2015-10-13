// travorama angular app - Flight Controller

app.controller('returnFlightController', [
    '$http', '$scope', function($http, $scope) {

        // ******************************
        // on document ready
        angular.element(document).ready(function() {
            $scope.getFlights();
        });

        // ******************************
        // general variables
        $scope.pageConfig = {
            activeFlightSection: 'departure',
            showNotice: false,
            fareChanged: false,
            fareUnavailable: false,
            fareToken: '',
            overviewDetailShown: false,
            flightsValidated: false,
            redirectingPage: false
        }
        $scope.departureFlightConfig = {
            flightSearchParams: FlightSearchConfig.flightForm.departureFlightParam,
            loading: false,
            searchId: '',
            flightList: [],
            flightFilterData: {
                price: [],
                airline: []
            },
            flightFilter: {
                transit: [true, true, true],
                airline: {
                    changed: false,
                    list: [],
                    value: []
                },
                time: {
                    departure: {
                        label: [0, 24],
                        value: [0, 24]
                    },
                    arrival: {
                        label: [0, 24],
                        value: [0, 24]
                    }
                },
                price: {
                    label: [-1, -1],
                    value: [-1, -1]
                }
            },
            flightSort: {
                label: 'price',
                value: 'TotalFare',
                reverse: false
            },
            activeFlight: -1,
            chosenFlight: -1,
            chosenFlightData: {},
            validating: false,
            validated: false,
            validateToken: '',
            validateAvailable: false,
            validateValid: false,
            validateNewfare: false,
            validateActive: false,
        };
        $scope.returnFlightConfig = {
            flightSearchParams: FlightSearchConfig.flightForm.returnFlightParam,
            loading: false,
            searchId: '',
            flightList: [],
            flightFilterData: {
                price: [],
                airline: []
            },
            flightFilter: {
                transit: [true, true, true],
                airline: {
                    changed: false,
                    list: [],
                    value: []
                },
                time: {
                    departure: {
                        label: [0, 24],
                        value: [0, 24]
                    },
                    arrival: {
                        label: [0, 24],
                        value: [0, 24]
                    }
                },
                price: {
                    label: [-1, -1],
                    value: [-1, -1]
                }
            },
            flightSort: {
                label: 'price',
                value: 'TotalFare',
                reverse: false
            },
            activeFlight: -1,
            chosenFlight: -1,
            chosenFlightData: {},
            validating: false,
            validated: false,
            validateToken: '',
            validateAvailable: false,
            validateValid: false,
            validateNewfare: false,
            validateActive: false
        };

        // ******************************
        // general functions

        // milisecond to hour
        $scope.msToTime = function (duration) {
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
        $scope.getDate = function (dateTime) {
            dateTime = new Date(dateTime);
            dateTime = dateTime.getDate();
            return dateTime;
        }

        // ******************************
        // set active flight
        $scope.setActiveFlight = function (target, flightSequence) {
            if (target == 'departure') {
                if ($scope.departureFlightConfig.activeFlight == flightSequence) {
                    $scope.departureFlightConfig.activeFlight = -1;
                } else {
                    $scope.departureFlightConfig.activeFlight = flightSequence;
                }
            } else if (target == 'return') {
                if ($scope.returnFlightConfig.activeFlight == flightSequence) {
                    $scope.returnFlightConfig.activeFlight = -1;
                } else {
                    $scope.returnFlightConfig.activeFlight = flightSequence;
                }
            }
        }

        // ******************************
        // set chosen flight
        $scope.setChosenFlight = function (target, flightSequence) {
            if (target == 'departure') {
                $scope.departureFlightConfig.chosenFlight = flightSequence;
                if ($scope.returnFlightConfig.chosenFlight < 0) {
                    $scope.pageConfig.activeFlightSection = 'return';
                }
            } else if (target == 'return') {
                $scope.returnFlightConfig.chosenFlight = flightSequence;
                if ($scope.departureFlightConfig.chosenFlight < 0) {
                    $scope.pageConfig.activeFlightSection = 'departure';
                }
            }
            if ($scope.departureFlightConfig.chosenFlight >= 0 && $scope.returnFlightConfig.chosenFlight >= 0) {
                $('body').addClass('no-scroll');
            } else {
                $('body').removeClass('no-scroll');
            }
        }

        // ******************************
        // get flights
        $scope.getFlights = function () {
            console.log('Getting flights');
            // get departure flight
            $scope.departureFlightConfig.loading = true;
            $scope.departureFlightConfig.loadingFlight = true;
            console.log('----------');
            console.log('Getting departure flight list');
            console.log($scope.departureFlightConfig.flightSearchParams);
            $http.get(FlightSearchConfig.Url, {
                params: {
                    request: $scope.departureFlightConfig.flightSearchParams
                }
            }).success(function (returnData) {
                $scope.departureFlightConfig.loading = false;
                $scope.departureFlightConfig.loadingFlight = false;

                console.log('Success getting departure flight list');
                console.log(returnData);
                // arrange flight data
                $scope.arrangeFlightData($scope.departureFlightConfig, returnData);

                console.log('Finished getting departure flight list');
                console.log('----------');
            }).error(function (returnData) {
                console.log('Failed to get departure flight list');
                console.log('ERROR :' + returnData);
                console.log('Finished getting departure flight list');
                console.log('----------');
            });

            // get return flight
            $scope.returnFlightConfig.loading = true;
            $scope.returnFlightConfig.loadingFlight = true;
            console.log('----------');
            console.log('Getting return flight list');
            console.log($scope.returnFlightConfig.flightSearchParams);
            $http.get(FlightSearchConfig.Url, {
                params: {
                    request: $scope.returnFlightConfig.flightSearchParams
                }
            }).success(function (returnData) {
                $scope.returnFlightConfig.loading = false;
                $scope.returnFlightConfig.loadingFlight = false;

                console.log('Success getting return flight list');
                console.log(returnData);
                // arrange flight data
                $scope.arrangeFlightData($scope.returnFlightConfig, returnData);

                console.log('Finished getting return flight list');
                console.log('----------');
            }).error(function (returnData) {
                console.log('Failed to get return flight list');
                console.log('ERROR :' + returnData);
                console.log('Finished getting return flight list');
                console.log('----------');
            });


        }// $scope.getFlights()

        $scope.arrangeFlightData = function(targetScope, data) {
            targetScope.loading = false;
            targetScope.loadingFlight = false;

            targetScope.flightList = data.FlightList;


        }// $scope.arrangeFlight()


    }
]);// flight controller

