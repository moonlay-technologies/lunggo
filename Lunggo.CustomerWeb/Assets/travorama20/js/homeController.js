// home controller
app.controller('homeController', ['$scope', '$log', '$http', '$location', function($scope, $log, $http, $location) {

    $scope.departureDate = departureDate;
    $scope.topDestinations = topDestinations;
    $scope.flightDestination = {
        name: indexPageDestination,
        code: indexPageDestinationsCode
    };

    //@Url.Action("Search", "Hotel")?zzz={{departureDate}}" method="POST"
    //=============== hotel start ======================

    $scope.hotel = {};
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
        return "?info=aaabbbccc"} 
    }
    //=============== hotel end ======================
    
}]);

// Calendar 2016 Controller
app.controller('campaignController', [
    '$scope', function ($scope) {
        
    }
]);