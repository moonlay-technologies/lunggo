app.controller('HomePageController', ['$http', '$scope',  '$rootScope',function($http, $scope, $rootScope) {
    

    // **********
    // variables
    $scope.PageConfig = $rootScope.PageConfig;
    $scope.FlightSearchForm = $rootScope.FlightSearchForm;

    $scope.PageConfig.Loaded = true;

    jQuery(document).ready(function($) {
        function showAgeChild() {
            var val = $('body .form-child select').val();
            val = parseInt(val);
            $('body .age-child').hide();

            if (val > 0) {
                $('body .age-child').show();
            }
        }
        $('body .form-child select').change(showAgeChild);
        showAgeChild();
    });
    
}]);