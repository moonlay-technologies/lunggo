// home controller
app.controller('hotelSearchController', ['$scope', '$log', '$http', '$resource', '$timeout', function ($scope, $log, $http, $resource, $timeout) {

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
    $scope.hotel.checkinDate = "12/10/2016";
    $scope.hotel.checkoutDate = "12/11/2016";
    $scope.hotel.adultCount = 3;
    $scope.hotel.childCount = 1;
    $scope.hotel.nightCount = 1;
    $scope.hotel.roomCount = 2;

    $scope.init = function (model) {
        $scope.model = model;

        var resource = $resource('//api.local.travorama.com/v1/hotel/search',
            {},
            {
                query: {
                    method: 'POST',
                    params: {},
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
            $scope.searchId = data.searchId;
            $scope.hotels = data.hotels;
            $scope.searchId = data.searchId;
            totalActualHotel = data.totalActualHotel;
            $scope.hotelFilterDisplayInfo = data.hotelFilterDisplayInfo;

            $log.debug(data);
        })
    }

    $scope.hotel.searchHotel = function () {
        $log.debug('searching hotel');
        location.href = '/id/Hotel/Search/' + $scope.hotel.searchParam();
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

    $scope.GotoDetailHotel = function (hotelCd) {
        $log.debug('redirect to detail hotel with hotelCd: ' + hotelCd);
        location.href = '/id/Hotel/DetailHotel/?' +
            "searchId=" + $scope.searchId + "&" +
            "hotelCd=" + hotelCd;
    }
    //=============== hotel end ======================


    $scope.$watch('hotels', function (newValue, oldValue, ccc) {
        $timeout(function () {
            $("body .col-left-hotel .img-list").each(function (i, elem) {
                var img = $(elem);
                var div = $("<div />").css({
                    background: "url(" + img.attr("src") + ") no-repeat",
                    width: "143px",
                    height: "180px",
                    "background-size": "cover",
                    "background-position": "center"
                });
                img.replaceWith(div);
            });
        }, 0);
    });
}]);
