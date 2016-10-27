app.controller('HotelController', ['$http', '$scope', '$rootScope', function ($http, $scope, $rootScope) {


    // **********
    // variables
    $scope.PageConfig = $rootScope.PageConfig;
    $scope.FlightSearchForm = $rootScope.FlightSearchForm;

    $scope.PageConfig.Loaded = true;

    // set overlay
    $scope.SetOverlay = function (overlay) {
        console.log('changing overlay to : ' + overlay);
        if (!overlay) {
            $scope.PageConfig.ActiveOverlay = '';
            $scope.PageConfig.BodyNoScroll = false;
        } else {
            $scope.PageConfig.ActiveOverlay = overlay;
            $scope.PageConfig.BodyNoScroll = true;
        }
    }

}]);