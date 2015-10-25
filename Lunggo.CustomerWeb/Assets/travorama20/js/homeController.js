// home controller
app.controller('homeController', [
    '$scope', function($scope) {

        $scope.departureDate = departureDate;
        $scope.topDestinations = TopDestinations;
        $scope.flightDestination = {
            name: indexPageDestination,
            code: indexPageDestinationsCode
        };

    }
]);