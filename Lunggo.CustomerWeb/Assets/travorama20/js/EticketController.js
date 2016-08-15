// checkout controller

app.controller('EticketController', ['$http', '$scope', '$location', function ($http, $scope) {

    
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
                        if (returnData.data.flight.payment.status != '2') {
                            window.location.reload(1);
                            //window.location.replace("https://m.local.travorama.com/id/Flight/Thankyou?rsvNo=160226537584#/%23page-3");
                            console.log(returnData.data.flight.payment.status);
                        } else {
                            $scope.refresh();
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
                console.log('Not Authorized');
            }
        }, 60000);
    }
}]);