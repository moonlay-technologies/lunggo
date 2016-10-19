// home controller
app.controller('homeController', ['$scope', '$log', '$http', '$location', function($scope, $log, $http, $location) {

    $scope.departureDate = departureDate;
    $scope.topDestinations = topDestinations;
    $scope.flightDestination = {
        name: indexPageDestination,
        code: indexPageDestinationsCode
    };

    $scope.hotelCalendar = {};
    $scope.hotelCalendar.show = true;
    $scope.changeTab = function (){
    }

    //@Url.Action("Search", "Hotel")?zzz={{departureDate}}" method="POST"
    //=============== hotel start ======================

    $scope.hotel = {};
    $scope.hotel.checkinDate = "28-12-2016";
    $scope.hotel.checkoutDate = "30-12-2016";
    $scope.hotel.nightCount = 1;
    $scope.hotel.roomCount = 2;
    $scope.hotel.adultCount = 3;
    $scope.hotel.childCount = 1;


    $scope.hotel.searchHotel = function (){
        $log.debug('searching hotel');
        location.href = '/id/Hotel/Search/' + $scope.hotel.searchParam();
        //$http({
        //   url: "/id/Hotel/Search", 
        //   method: "GET",
        //   params: {aaa : "kode123", bbb : "silubab" }
        //   })
    };

    $scope.hotel.searchParam = function (){
        return ("?info=" + 
            [$scope.hotel.checkinDate,
             $scope.hotel.checkoutDate,
             $scope.hotel.nightCount,
             $scope.hotel.roomCount,
             $scope.hotel.adultCount,
             $scope.hotel.childCount].join('.')
        )
    }
    //=============== hotel end ======================
    
}]);

// Calendar 2016 Controller
app.controller('campaignController', [
    '$scope', function ($scope) {
        
    }
]);