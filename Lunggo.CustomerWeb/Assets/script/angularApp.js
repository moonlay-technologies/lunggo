// ************************
// variables

var SearchHotelConfig = {
    Url: 'http://travorama-apidev.cloudapp.net/api/v1/hotels',
    ResultCount: '10',
    CurrentPage: '1'
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

    app.controller('HotelController', ['$http', '$scope', function ($http, $scope) {

        // hotel list
        var hotel_list = this;
        hotel_list.hotels = [];

        // hotel search params
        $scope.HotelSearchParams = {};

        // load hotel list function
        $scope.load_hotel_list = function () {

            console.log('Loading...');

            $('.hotel-list-content').prepend('<h1 class="preload text-center notif">LOADING</h1>');

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
