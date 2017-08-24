angular.module('travorama')
    .directive('priceDetailDirective', [
        function() {
            return {
                restrict: 'E',
                scope: {
                    flight: '='
                },
                templateUrl: "/Assets/travorama20/angularDirectory/priceDetail/priceDetailTemplate.html",
                controller: [
                    '$scope', function($scope) {
                        // console.log('masup dir controller');
                    }
                ]
            };
        }
    ]);
