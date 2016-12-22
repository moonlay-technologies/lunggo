// checkout controller

app.controller('EticketController', ['$http', '$scope', '$location', '$log', function ($http, $scope, $log) {

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
                        if (returnData.data.status != "500") {
                            if (returnData.data.flight) {
                                if (returnData.data.flight.rsvstatus != '5') {
                                    $scope.refresh();
                                }
                            } else if (returnData.data.hotel) {
                                if (returnData.data.hotel.rsvstatus != '5') {
                                    $scope.refresh();
                                }
                            } else {
                                window.location.reload(1);
                                if (returnData.data.flight) {
                                    $log.debug(returnData.data.flight.payment.status);
                                }
                                if (returnData.data.hotel) {
                                    $log.debug(returnData.data.hotel.payment.status);
                                }

                            }
                        }
                    else {
                        $scope.refresh();
                    }
                    
                }).catch(function () {
                    $scope.refresh();
                });
            }
            else {
                $log.debug('Not Authorized');
            }
        }, 60000);
    }
}]);