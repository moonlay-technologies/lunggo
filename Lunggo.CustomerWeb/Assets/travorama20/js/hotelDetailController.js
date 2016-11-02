﻿// home controller
app.controller('hotelDetailController', ['$scope', '$log', '$http', '$resource', '$timeout', function ($scope, $log, $http, $resource, $timeout) {

    $scope.init = function (model) {
        $log.debug(model);
        $scope.model = model;
        $scope.searchId = $scope.model.searchId;
        var maxImages = 6;

        var resource = $resource('//api.local.travorama.com/v1/hotel/GetHotelDetail/:searchId/:hotelCd',
            {},
            {
                query: {
                    method: 'GET',
                    params: { searchId: model.searchId, hotelCd: model.hotelCd },
                    isArray: false
                }
            }
        );

        resource.query({}, {
            "location": "16152",
            "checkinDate": "03/01/2017",
            "checkoutDate": "03/04/2017",
            "adultCount": "1",
            "childCount": "0",
            "nightCount": "3",
            "roomCount": "1",
            "from": "1",
            "to": "9"
        }).$promise.then(function (data) {
            $scope.hotel = data.hotelDetails;

            var loadedImages = 0;
            var tempHotelImages = [];
            $.each($scope.hotel.images, function (key, value) {
                tempHotelImages.push("http://photos.hotelbeds.com/giata/bigger/" + value);

                loadedImages++;
                if (loadedImages == maxImages) {
                    return false;
                }
            });
            $scope.hotel.images = tempHotelImages;
            $log.debug($scope.hotel);

            accordionFunctions();
        })
    }

    $scope.model = {};
    $scope.hotels = [];
    $scope.totalActualHotel = '';
    $scope.hotelFilterDisplayInfo = undefined;
    $scope.filterDisabled = true;

    //@Url.Action("Search", "Hotel")?zzz={{departureDate}}" method="POST"
    //=============== hotel start ======================

    $scope.searchId = '';
    $scope.hotel = {};
    $scope.hotel.location = "BALI";
    $scope.hotel.checkinDate = "28-12-2016";
    $scope.hotel.checkoutDate = "30-12-2016";
    $scope.hotel.adultCount = 3;
    $scope.hotel.childCount = 1;
    $scope.hotel.nightCount = 1;
    $scope.hotel.roomCount = 2;

    $scope.hotel.searchHotel = function () {
        $log.debug('searching hotel');
        location.href = '/id/Hotel/Search/' + $scope.hotel.searchParam();
        //$http({
        //   url: "/id/Hotel/Search", 
        //   method: "GET",
        //   params: {aaa : "kode123", bbb : "silubab" }
        //   })
    };

    $scope.hotel.searchParam = function () {
        return ("?info=" +
            [$scope.hotel.location,
             $scope.hotel.checkinDate,
             $scope.hotel.checkoutDate,
             $scope.hotel.adultCount,
             $scope.hotel.childCount,
             $scope.hotel.nightCount,
             $scope.hotel.roomCount].join('.')
        )
    }

    var accordionFunctions = function() {
        //Accordion Help Section by W3School
        $timeout(function () {
            var acc = document.getElementsByClassName("accordion");
            var i;

            for (i = 0; i < acc.length; i++) {
                acc[i].onclick = function () {
                    this.classList.toggle("active");
                    this.nextElementSibling.classList.toggle("show");
                }
            }
        }, 0)
    }
    //=============== hotel end ======================

}]);
