// ************************
// Angular app
(function () {

    var app = angular.module('travorama', ['ngRoute']);

    // ******************************
    // hotel controller
    app.controller('hotelController', [
        '$http', '$scope', function ($http, $scope) {

            // ********************
            // genereal variables
            $scope.HotelSearchParams = {};
            $scope.HotelSearchResult = {};
            $scope.PageConfig = {
                Loaded: false,
                busy: false,
                TotalPage: 1,
                CurrentPage: 1
            };

            // ********************
            // get hotel list
            $scope.GetHotel = function() {
                
            }// GetHotel()

            // ********************
            // show hotel
            $scope.ShowHotelList = function(pageNumber) {
                
            }// ShowHotelList();


        }
    ]);// hotel controller

    // ******************************
    // room controller
    app.controller('roomController', [
        '$http', '$scope', function ($http, $scope) {

            

        }
    ]);// room controller

    // ******************************
    // fight controller
    app.controller('flightController', [
        '$http', '$scope', function ($http, $scope) {
            
            // ********************
            // on document ready
            angular.element(document).ready(function () {
                console.log('document ready');
                $scope.getFlight();
            });

            // ********************
            // general variables

            $scope.dummyParams = { "TripType": "OneWay", "TripInfos": [{ "TripId": 0, "OriginAirport": "CGK", "DestinationAirport": "DPS", "DepartureDate": "2015-10-25T00:00:00" }], "AdultCount": "1", "ChildCount": "0", "InfantCount": "0", "CabinClass": "Economy", "Currency": "IDR" };

            $scope.flightSearchParams = $scope.dummyParams;
            $scope.flightSearchResult = {};
            $scope.loaded = false;
            $scope.busy = false;
            $scope.flightDetailActive = false;
            $scope.flightDetailCurrent = -1;

            // ********************
            // get date time
            $scope.getDateTime = function (dateTime) {
                return new Date(dateTime);
            }

            // ********************
            // current active flight
            $scope.getFlightDetail = function(flightIndex) {
                console.log( $scope.flightDetailActive );
                $scope.flightDetailActive = true;
                $scope.flightDetailCurrent = flightIndex;
                console.log($scope.flightDetailActive);
                console.log('getting detail for : ' + flightIndex);
                console.log( $scope.flightDetailCurrent );

            }


            // ********************
            // get flight list
            $scope.getFlight = function() {

                // get the flight function if system is not busy
                if ($scope.busy == false) {
                    console.log('Fetching flight list with param :');
                    console.log($scope.dummyParams);
                    $scope.busy = true;

                    // AJAX
                    $http.get(FlightSearchConfig.Url, {
                        params: {
                            request: $scope.dummyParams
                        }
                    }).success(function (data) {

                        $scope.flightSearchResult = data;

                        console.log($scope.flightSearchResult);

                        FlightSearchConfig.SearchId = data.SearchId;

                        for ( var i=0; i< $scope.flightSearchResult.FlightList.length; i++ ) {
                            $scope.flightSearchResult.FlightList[i].FlightIndex = i;
                        }

                        if (data.TotalFlightCount == 0) {
                            $scope.noFlight = true;
                        }

                        // *****
                        console.log('Flight Fetched');

                        $scope.loaded = true;
                        $scope.busy = false;

                    }).error(function () {

                        $scope.noFlight = true;
                        $scope.loaded = true;
                        $scope.busy = false;

                        console.log('REQUEST ERROR');
                    });

                } else {
                    console.log('System Busy, please wait.');
                }

            }// getFlight()

            // ********************
            // pagination
            $scope.showPage = function(pageNumber) {
                
            }// showPage()

            // ********************
            // flight sorting
            $scope.sortFlight = function() {
                
            }// sortFlight()

            // ********************
            // get flight detail
            $scope.getFlightDetail = function(flightNo) {
                console.log('getting flight detail for : '+ flightNo);

            }// getFlightdetail()

            // ********************
            // validate flight
            $scope.validateFlight = function(flightNo) {
                console.log('validating flight no : '+ flightNo);

            }// validateFlight()

        }
    ]);// flight controller

})();


