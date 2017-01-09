app.controller('ThankyouController', ['$http', '$scope', '$location', '$log', function ($http, $scope, $log) {

    
    angular.element(document).ready(function () {
        $scope.rsvNo = window.location.search.toString().split('=')[1];
    });
    $scope.hide = false;
    $scope.returnUrl = window.location.origin;
    $scope.roomService = 0;
    $scope.returnUrl = window.location.origin;
    $scope.pageLoaded = true;

    $scope.seeDetail = function () {
        $scope.hide = true;
    }

    $scope.msToTime = function (duration) {

        var minutes = parseInt((duration / (1000 * 60)) % 60),
                hours = parseInt((duration / (1000 * 60 * 60)));
        hours = hours;
        minutes = minutes;
        return hours + "j " + minutes + "m";
    }

    $scope.refresh = function () {
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
                            if (returnData.data.flight != null) {
                                if (returnData.data.flight.payment.status != '2') {
                                    window.location.reload(1);
                                    $log.debug(returnData.data.flight.payment.status);
                                }
                            } else if (returnData.data.hotel != null) {
                                if (returnData.data.hotel.payment.status != '2') {
                                    window.location.reload(1);
                                    $log.debug(returnData.data.hotel.payment.status);
                                }
                            }
                            
                            else {
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
                $log.debug('Not Authorized');
            }
        }, 15000);
    }

    $scope.capitalizeFirstLetter = function (sentence) {
        var words = sentence.split(" ");
        var text = "";
        for (var i = 0; i < words.length; i++) {
            text += words[i].substring(0, 1) + words[i].substring(1, words[i].length).toLowerCase() + " ";
        }
        return text;
    }
}]);