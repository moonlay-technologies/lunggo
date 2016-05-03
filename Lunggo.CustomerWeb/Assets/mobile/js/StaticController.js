// FAQS Controller
app.controller('FaqsController', ['$http', '$scope', '$rootScope', function ($http, $scope, $rootScope) {

    $scope.PageConfig = $rootScope.PageConfig;

}]);// FAQS Controller

// Privacy Controller
app.controller('PrivacyController', ['$http', '$scope', '$rootScope', function ($http, $scope, $rootScope) {

    $scope.PageConfig = $rootScope.PageConfig;

}]);// Privacy Controller

// Terms Controller
app.controller('TermsController', ['$http', '$scope', '$rootScope', function ($http, $scope, $rootScope) {

    $scope.PageConfig = $rootScope.PageConfig;

}]);// Terms Controller

// how to order controller
app.controller('HowToOrderController', ['$http', '$scope', '$rootScope', function ($http, $scope, $rootScope) {

    $scope.PageConfig = $rootScope.PageConfig;

}]);// Terms Controller

// promo controller
// how to order controller
app.controller('PromoController', ['$http', '$scope', '$rootScope', function ($http, $scope, $rootScope) {

    $scope.PageConfig = $rootScope.PageConfig;

}]);// promo controller

app.controller('NewsletterController', ['$http', '$scope', '$rootScope', function ($http, $scope, $rootScope) {

    
    $scope.PageConfig = $rootScope.PageConfig;
    $scope.pageLoaded = true;
    $scope.overlay = false;
    $scope.closeOverlay = function () {
        $scope.overlay = false;
        $scope.User.Sending = false;
        $scope.User.Sent = false;
        $scope.User.showBox = false;
    }
    $scope.form = {
        //submitting: false,
        userEmail: '',
        name: ''
    };
    $scope.validateEmail = function (email){
        var re = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;
        return re.test(email);
    }

    $scope.User = {
        isEmailCorrect: false,
        Email: '',
        Registered: false,
        Sending: false,
        Sent: false,
        showBox: false,
        Send: function () {
            if ($scope.validateEmail($scope.form.userEmail)) {
                $scope.User.Email = $scope.form.userEmail;
            } else {
                $scope.User.Email = '';
                alert('Alamat e-mail tidak valid');
            }

            $scope.User.Name = $scope.form.name;
            if ($scope.User.Email && $scope.User.Name) {
                $scope.User.SubmitForm();
            } else {
                alert('Masukkan alamat e-mail dan nama');
            }
        },
        SubmitForm: function() {
            $scope.User.Sending = true;
            $http({
                url: SubscribeConfig.Url,
                method: 'POST',
                data: {
                    'address': $scope.User.Email,
                    'name': $scope.User.Name,
                }
            }).success(function (returnData) {
                $scope.User.showBox = true;
                console.log('Succeed');
                $scope.User.Sending = false;
                if (returnData == true) {
                    $scope.User.Registered = true;
                } else {
                    $scope.User.Registered = false;
                }
                    $scope.User.Sent = true;
                })
                .error(function (returnData) {
                    console.log('Failed requesting reset password');
                    console.log(returnData);
                    $scope.User.showBox = true;
                    $scope.User.Sending = false;
                    $scope.User.Sent = false;
                });
        }

    }

}]);