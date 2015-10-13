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
            validating: false,
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

        // set page active section
        $scope.setPageActiveSection = function (target) {
            if (target == 'departure') {
                if ($scope.departureFlightConfig.chosenFlight < 0) {
                    $scope.pageConfig.activeFlightSection = 'departure';
                }
            } else if (target == 'return') {
                if ($scope.returnFlightConfig.chosenFlight < 0) {
                    $scope.pageConfig.activeFlightSection = 'return';
                }
            }
        }

        // ******************************
        // flight overview
        // close flight overview
        $scope.closeOverview = function () {
            $scope.pageConfig.overviewDetailShown = false;
            $('body').removeClass('no-scroll');
            if ($scope.pageConfig.flightsValidated == true) {
                $scope.pageConfig.flightsValidated = false;
                if ($scope.departureFlightConfig.validateAvailable == true && $scope.returnFlightConfig.validateAvailable == false) {
                    $scope.pageConfig.activeFlightSection = 'return';
                    $scope.returnFlightConfig.chosenFlight = -1;
                } else if ($scope.departureFlightConfig.validateAvailable== false && $scope.returnFlightConfig.validateAvailable == true) {
                    $scope.pageConfig.activeFlightSection = 'departure';
                    $scope.departureFlightConfig.chosenFlight = -1;
                } else if ($scope.departureFlightConfig.validateAvailable == false && $scope.returnFlightConfig.validateAvailable == false) {
                    if ($scope.departureFlightConfig.validateNewfare == false && $scope.returnFlightConfig.validateNewfare == false) {
                        $scope.pageConfig.activeFlightSection = 'departure';
                        $scope.departureFlightConfig.chosenFlight = -1;
                        $scope.returnFlightConfig.chosenFlight = -1;
                    } else if ($scope.departureFlightConfig.validateNewfare == false && $scope.returnFlightConfig.validateNewfare == true) {
                        $scope.pageConfig.activeFlightSection = 'departure';
                        $scope.departureFlightConfig.chosenFlight = -1;
                    } else if ($scope.departureFlightConfig.validateNewfare == true && $scope.returnFlightConfig.validateNewfare == false) {
                        $scope.pageConfig.activeFlightSection = 'return';
                        $scope.returnFlightConfig.chosenFlight = -1;
                    }
                }
            } else {
                if ($scope.pageConfig.activeFlightSection == 'return') {
                    $scope.returnFlightConfig.chosenFlight = -1;
                } else if ($scope.pageConfig.activeFlightSection == 'departure') {
                    $scope.departureFlightConfig.chosenFlight = -1;
                }
            }
        }

        // toggle overview detail
        $scope.toggleOverviewDetail = function () {
            if ($scope.pageConfig.overviewDetailShown == true) {
                $scope.pageConfig.overviewDetailShown = false;
            } else {
                $scope.pageConfig.overviewDetailShown = true;
            }
        }

        // ******************************
        // revalidate flights
        $scope.revalidateFlight = function (departureFlightIndexNo, returnFlightIndexNo) {

            if ($scope.pageConfig.flightsValidated) {

                console.log('flight available. Will be redirected within');
                $scope.pageConfig.redirectingPage = true;
                var fareToken = $scope.departureFlightConfig.validateToken + '.' + $scope.returnFlightConfig.validateToken;
                $scope.pageConfig.fareToken = fareToken;
                console.log($scope.pageConfig.fareToken);
                $('.pushToken .fareToken').val(fareToken);
                $('.pushToken').submit();

            } else {

                // revalidate departure flight
                console.log('validating departure flight');
                $scope.departureFlightConfig.validating = true;
                $http.get(RevalidateConfig.Url, {
                    params: {
                        SearchId: $scope.departureFlightConfig.searchId,
                        ItinIndex: departureFlightIndexNo
                    }
                }).success(function (returnData) {
                    $scope.departureFlightConfig.validating = false;
                    if (returnData.IsValid == true) {
                        console.log('departure flight available');
                        $scope.departureFlightConfig.validateToken = returnData.Token;
                        $scope.departureFlightConfig.validateAvailable = true;
                        if ($scope.returnFlightConfig.validating == false) {
                            flightsValidated();
                        }
                    } else if (returnData.IsValid == false) {
                        console.log('departure flight unavailable');
                        $scope.departureFlightConfig.validateAvailable = false;
                        if (returnData.IsOtherFareAvailable == true) {
                            $scope.departureFlightConfig.validateToken = returnData.Token;
                            console.log('departure flight has new price');
                            $scope.departureFlightConfig.validateNewfare = true;
                            $scope.departureFlightConfig.flightList[departureFlightIndexNo].TotalFare = returnData.NewFare;
                            if ($scope.returnFlightConfig.validating == false) {
                                flightsValidated();
                            }
                        } else if (returnData.IsOtherFareAvailable == false) {
                            console.log('departure flight is gone');
                            $scope.departureFlightConfig.validateNewfare = false;
                            $scope.departureFlightConfig.flightList[departureFlightIndexNo].available = false;
                            if ($scope.returnFlightConfig.validating == false) {
                                flightsValidated();
                            }
                        }
                    }
                }).error(function (data) {
                    $scope.departureFlightConfig.validating = false;
                    console.log('ERROR Validating Departure Flight :');
                    console.log(data);
                    console.log('--------------------');
                });

                // revalidate return flight
                console.log('validating return flight');
                $scope.returnFlightConfig.validating = true;
                $http.get(RevalidateConfig.Url, {
                    params: {
                        SearchId: $scope.returnFlightConfig.searchId,
                        ItinIndex: returnFlightIndexNo
                    }
                }).success(function (returnData) {
                    $scope.returnFlightConfig.validating = false;
                    if (returnData.IsValid == true) {
                        console.log('return flight available');
                        $scope.returnFlightConfig.validateToken = returnData.Token;
                        $scope.returnFlightConfig.validateAvailable = true;
                        if ($scope.departureFlightConfig.validating == false) {
                            flightsValidated();
                        }
                    } else if (returnData.IsValid == false) {
                        console.log('return flight unavailable');
                        $scope.returnFlightConfig.validateAvailable = false;
                        if (returnData.IsOtherFareAvailable == true) {
                            $scope.returnFlightConfig.validateToken = returnData.Token;
                            console.log('return flight has new price');
                            $scope.returnFlightConfig.validateNewfare = true;
                            $scope.returnFlightConfig.flightList[returnFlightIndexNo].TotalFare = returnData.NewFare;
                            if ($scope.departureFlightConfig.validating == false) {
                                flightsValidated();
                            }
                        } else if (returnData.IsOtherFareAvailable == false) {
                            console.log('return flight is gone');
                            $scope.returnFlightConfig.validateNewfare = false;
                            $scope.returnFlightConfig.flightList[returnFlightIndexNo].Available = false;
                            if ($scope.departureFlightConfig.validating == false) {
                                flightsValidated();
                            }
                        }
                    }
                }).error(function (data) {
                    $scope.returnFlightConfig.validatingFlight = false;
                    console.log('ERROR Validating Departure Flight :');
                    console.log(data);
                    console.log('--------------------');
                });

                /*-----------*/
                // run after both flight validated
                var flightsValidated = function () {
                    $scope.pageConfig.flightsValidated = true;
                    console.log('flights validated');
                    // if flight is valid
                    if ($scope.departureFlightConfig.validateAvailable == true && $scope.returnFlightConfig.validateAvailable == true) {
                        console.log('flight available. Will be redirected.');
                        $scope.pageConfig.redirectingPage = true;
                        var fareToken = $scope.departureFlightConfig.validateToken + '.' + $scope.returnFlightConfig.validateToken;
                        console.log('Token : '+fareToken);
                        $('.pushToken .fareToken').val(fareToken);
                        $('.pushToken').submit();
                    }
                }

            }


        }

        // ******************************
        // flight sorting
        $scope.setSort = function (targetFlight, label) {
            if (targetFlight == 'departure') {
                targetFlight = $scope.departureFlightConfig;
            } else {
                targetFlight = $scope.returnFlightConfig;
            }
            targetFlight.flightSort.reverse = (targetFlight.flightSort.label === label) ? !targetFlight.flightSort.reverse : false;
            targetFlight.flightSort.label = label;
            switch (label) {
                case 'airline':
                    targetFlight.flightSort.value = 'Trips[0].Airlines[0].Name';
                    break;
                case 'departure':
                    targetFlight.flightSort.value = 'Trips[0].Segments[0].DepartureTime';
                    break;
                case 'arrival':
                    targetFlight.flightSort.value = 'Trips[0].Segments[(Trips[0].Segments.length-1)].ArrivalTime';
                    break;
                case 'duration':
                    targetFlight.flightSort.value = 'Trips[0].TotalDuration';
                    break;
                case 'price':
                    targetFlight.flightSort.value = 'TotalFare';
                    break;
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
            targetScope.searchId = data.SearchId;

        }// $scope.arrangeFlight()


    }
]);// flight controller

