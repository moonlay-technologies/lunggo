// checkout controller
var app = angular.module('myApp', ['ngRoute']);
app.controller('ThankyouController', ['$http', '$scope', '$location', function ($http, $scope) {

    
    angular.element(document).ready(function () {

        $scope.rsvNo = window.location.search.toString().split('=')[1];
    });
    
    $scope.returnUrl = window.location.origin;
    $scope.refresh = function() {
        setTimeout(function () {
            //
            //Check Authorization
            var authAccess = getAuthAccess();
            if (authAccess == 1 || authAccess == 2) {
                // send form
                $http({
                    method: 'GET',
                    url: GetReservationConfig.Url + $scope.rsvNo,
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).
                    then(function (returnData) {
                        //console.log(returnData);
                    if (returnData.data.status != "500") {
                        if (returnData.data.flight.payment.status == '2') {
                            window.location.reload(1);
                            console.log(returnData.data.flight.payment.status);
                        } 
                    }
                    else {
                        $scope.refresh();
                    }
                    
                }).catch(function () {
                    console.log('error');
                });
            }
            else {
                console.log('Not Authorized');
            }
        }, 60000);
    }
}]);