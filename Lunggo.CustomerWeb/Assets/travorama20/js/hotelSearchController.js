// home controller
app.controller('hotelSearchController', ['$scope', '$log', '$http', '$resource', function ($scope, $log, $http, $resource) {

    $scope.model = {};
    $scope.init = function (model) {
        $scope.model = model;

        var Job = $resource('//api.local.travorama.com/v1/hotel/search',
            {},
            {
                query: { method: 'POST', params: {}, isArray: false }
            }
        );

        Job.query().$promise.then(function (data) {
            var wrapped = angular.fromJson(data);
            angular.forEach(wrapped.items, function (item, idx) {
                wrapped.items[idx] = new Job(item);
            });
            return wrapped;
        })
    }
    





    //@Url.Action("Search", "Hotel")?zzz={{departureDate}}" method="POST"
    //=============== hotel start ======================

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
    //=============== hotel end ======================

}]);
