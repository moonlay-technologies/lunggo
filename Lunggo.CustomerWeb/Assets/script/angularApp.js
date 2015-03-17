// ************************
// variables

var SearchHotelConfig = {
    Url: 'http://travorama-apidev.cloudapp.net/api/v1/hotels',
    ResultCount: '10',
    CurrentPage: '1'
}

var SearchRoomConfig = {
    Url: 'http://travorama-apidev.cloudapp.net/api/v1/rooms'
}

// ************************
// functions

// generate SearchParams
function GenerateSearchParams() {

    // location
    
    // date

    // price

    // rating

}

// ************************
// ready function
$(document).ready(function () {

});


// ************************x
// angular app
(function () {

    var app = angular.module('travorama', ['ngRoute']);

    // Room Controller
    app.controller('RoomController', ['$http', '$scope', function($http, $scope) {

        // room list
        var room_list = this;
        room_list.rooms = [];

        // room search params
        $scope.RoomSearchParams = {};

        // load room list function
        $scope.load_room_list = function() {
            
            console.log('Loading...');

            // http request
            $http.get(SearchRoomConfig.Url, {
                params: {
                    //StayDate: $scope.RoomSearchParams.StayDate,
                    //StayLength: $scope.RoomSearchParams.StayLength,
                    //RoomCount: $scope.RoomSearchParams.RoomCount,
                    //SearchId: $scope.HotelSearchParams.SearchId
                    HotelId: '456789',
                    StayDate: '2015-10-10',
                    StayLength: '1',
                    RoomCount: '2'
                }
            }).success(function (data) {
                console.log(data);
                console.log('...Loaded');
            }).error(function () {
                console.log('REQUEST ERROR');
            });

        };

    }])

    // Hotel Controller
    app.controller('HotelController', ['$http', '$scope', function ($http, $scope) {

        // hotel list
        var hotel_list = this;
        hotel_list.hotels = [];

        // hotel search params
        $scope.HotelSearchParams = {};

        // load hotel list function
        $scope.load_hotel_list = function () {

            console.log('Loading...');

            // generate StarRating
            $scope.HotelSearchParams.StarRating = [$scope.HotelSearchParams.star0, $scope.HotelSearchParams.star1, $scope.HotelSearchParams.star2, $scope.HotelSearchParams.star3, $scope.HotelSearchParams.star4, $scope.HotelSearchParams.star5].join('');
            if ($scope.HotelSearchParams.StarRating.length == 0) {
                $scope.HotelSearchParams.StarRating = '-1,1,2,3,4,5';
            }

            // http request
            $http.get(SearchHotelConfig.Url, {
                params: {
                    LocationId: $scope.HotelSearchParams.LocationId,
                    StayDate: $scope.HotelSearchParams.StayDate,
                    StayLength: $scope.HotelSearchParams.StayLength,
                    SearchId: $scope.HotelSearchParams.SearchId,
                    SortBy: $scope.HotelSearchParams.SortBy,
                    StartIndex: $scope.HotelSearchParams.StartIndex,
                    ResultCount: SearchHotelConfig.ResultCount,
                    MinPrice: $scope.HotelSearchParams.MinPrice,
                    MaxPrice: $scope.HotelSearchParams.MaxPrice,
                    StarRating: $scope.HotelSearchParams.StarRating
                }
            }).success(function (data) {
                console.log(data);
                hotel_list.hotels = data.HotelList;
                hotel_list.SearchId = data.SearchId;
                hotel_list.hotelsTotal = data.TotalFilteredCount;
                console.log('...Loaded');
            }).error(function () {
                console.log('REQUEST ERROR');
            });

        };


    }]);

})();
