// travorama angular app - checkout controller
app.controller('checkoutController', [
    '$http', '$scope', function($http, $scope) {

        //********************
        // variables
        $scope.currentPage = 1;
        $scope.loginShown = false;
        $scope.stepClass = '';
        $scope.titles = [
            { name: 'Mr', value: 'mr' },
            { name: 'Mrs', value: 'mrs' },
            { name: 'Ms', value: 'ms' }
        ];
        $scope.infantTitles = [
            { name: 'Mr', value: 'mr' },
            { name: 'Ms', value: 'ms' }
        ];

        //********************
        // general functions
        // change page
        $scope.changePage = function (page) {
            // change current page variable
            $scope.currentPage = page;
            // change step class
            $scope.stepClass = 'active-' + page;
        }
        // change page after login
        $scope.changePage(currentPage);

        // toggle Travorama Login
        $scope.toggleLogin = function() {
            if ($scope.loginShown == false) {
                $scope.loginShown = true;
            } else {
                $scope.loginShown = false;
            }
        }

        // test validate form
        $scope.testForm = function (formSelect) {
            if (formSelect == "buyerForm") {
                console.log($scope.buyerForm);
            }
        }

    }
]);// checkout controller