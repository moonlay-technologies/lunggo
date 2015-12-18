app.controller('CheckoutController', ['$http', '$scope', '$rootScope', '$interval', '$location', function($http, $scope, $rootScope, $interval, $location) {
    
    // return URL
    $scope.returnUrl = document.referrer == (window.location.origin + window.location.pathname + window.location.search) ? '/' : document.referrer;

    // set hash to page 1
    angular.element(document).ready(function () {
        //$scope.PageConfig.ActivePage = 1;
        $location.hash('page-1');
        console.log('READY');
    });

    angular.element(window).on('hashchange', function () {
        if ($location.hash() == '') {
            $scope.PageConfig.ActivePage = 1;
            $location.hash('page-1');
        }
    });
    $scope.$watch(function () {
            return location.hash;
    }, function (value) {
        if (!$scope.PageConfig.ActivePageChanged) {
            $scope.PageConfig.ChangePage(1);
            $scope.PageConfig.ActivePageChanged = true;
        } else {
            value = value.split('-');
            value = value[1];
            if (value > 0) {
                $scope.PageConfig.ChangePage(value);
            }
        }
    });

    // get hash to change page for back page

    // change page
    $scope.PageConfig.ActivePageChanged = false;
    $scope.PageConfig.ActivePage = 1;
    $scope.PageConfig.ChangePage = function(page) {
        $scope.PageConfig.ActivePage = page;
        $location.hash("page-"+page);
    }


}]);