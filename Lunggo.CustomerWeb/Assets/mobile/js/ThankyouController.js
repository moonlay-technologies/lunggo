// checkout controller
//var app = angular.module('myApp', ['ngRoute']);
app.controller('ThankyouController', ['$http', '$scope', '$location', function ($http, $scope) {

    
    angular.element(document).ready(function () {

        $scope.rsvNo = window.location.search.toString().split('=')[1];
    });
    $scope.hide = false;
    $scope.returnUrl = window.location.origin;
    $scope.seeDetail = function() {
        $scope.hide = true;
    }
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

    $scope.returnUrl = window.location.origin;
    $scope.pageLoaded = true;
    $scope.msToTime = function (duration) {

        var milliseconds = parseInt((duration % 1000) / 100),
                seconds = parseInt((duration / 1000) % 60),
                minutes = parseInt((duration / (1000 * 60)) % 60),
                hours = parseInt((duration / (1000 * 60 * 60)));
        hours = hours;
        minutes = minutes;
        seconds = seconds;
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
                        //console.log(returnData);
                        if (returnData.data.status != "500") {
                            if (returnData.data.flight.payment.status != '2') {
                                window.location.reload(1);
                                //window.location.replace("https://local.travorama.com/id/Flight/Thankyou?rsvNo=160226537584#/%23page-3");
                                console.log(returnData.data.flight.payment.status);
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
                console.log('Not Authorized');
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

    
    $scope.totalRoom = totalRoom;
    $scope.checkin = checkin;
    $scope.checkout = checkout;
    $scope.nights = nights;

    $scope.roomService = 0;
    $scope.netFare = netFare;
    
}]);