// checkout controller

app.controller('ContactController', ['$http', '$scope', '$location', function ($http, $scope) {

    
    angular.element(document).ready(function () {

        $scope.rsvNo = window.location.search.toString().split('=')[1];
    });

    $scope.email = '';
    $scope.name = '';
    $scope.message = '';
    $scope.send = function() {
        return $scope.email.length != 0 && $scope.name.length != 0 && $scope.message.length != 0;
    }

    
}]);