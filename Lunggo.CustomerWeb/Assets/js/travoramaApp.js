// ************************
// Angular app
(function () {

    var app = angular.module('travorama', ['ngRoute']);

    // ******************************
    // hotel controller
    app.controller('HotelController', [
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
    app.controller('RoomController', [
        '$http', '$scope', function ($http, $scope) {

            

        }
    ]);// room controller

    // ******************************
    // fight controller
    app.controller('FlightController', [
        '$http', '$scope', function ($http, $scope) {
            
            // ********************
            // general variables
            $scope.FlightSearchParams = {};
            $scope.FlightSearchResult = {};
            $scope.PageConfig = {
                Loaded: false,
                Busy : false,
                TotalPage : 1,
                CurrentPage : 1
            };

            // ********************
            // get flight list
            $scope.GetFlight = function() {

                // get the flight function if system is not busy
                if ($scope.PageConfig.busy == false) {
                    


                } else {
                    console.log('System Busy, please wait.');
                }

            }// GetFlight()

            // ********************
            // pagination
            $scope.ShowFlghtList = function(pageNumber) {
                
            }// Pagination()

            // ********************
            // flight sorting
            $scope.SortFlight = function() {
                
            }// SortFlight()

        }
    ]);// flight controller

})();


