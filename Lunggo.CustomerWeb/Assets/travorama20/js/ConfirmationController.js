app.controller('confirmationController', [
    '$http', '$scope', '$interval', function ($http, $scope, $interval) {

        $scope.pageLoaded = true;
        $scope.paymentTimeout = paymentTimeout;
        $scope.expired = false;
        $interval(function () {
            var nowTime = new Date();
            if (nowTime > $scope.paymentTimeout) {
                $scope.expired = true;
            }
        }, 1000);
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

    }
]);