app.controller('SingleFlightController', ['$http', '$scope', '$interval', function($http, $scope, $interval) {

    // ********************
    // general variables

    $scope.PageConfig = {
        loaded: true,
        ActiveSection: 'departure',
        ActiveOverlay: ''
    };

    $scope.SearchConfig = {}

    $scope.FlightConfig = [
        {
            Name: 'departure',
            FlightList: [],
            FlightFilter: {},
            FlightSort: {}
        },
        {
            Name : 'return'
        }
    ];


    // ********************
    // general functions

    // set overlay
    $scope.SetOverlay = function(overlay) {
        if (!overlay) {
            $scope.PageConfig.ActiveOverlay = '';
        } else {
            $scope.PageConfig.ActiveOverlay = overlay;
        }
    }


}]);