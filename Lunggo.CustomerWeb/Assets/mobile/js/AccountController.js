// Check Order Controller
app.controller('CheckOrderController', ['$http', '$scope', '$rootScope', function($http, $scope, $rootScope) {
    
    $scope.PageConfig = $rootScope.PageConfig;

    $scope.Order = {
        Number: '',
        Name : ''
    };

}]);// Check Order Controller

// Login Controller
app.controller('LoginController', ['$http', '$scope', '$rootScope', function($http, $scope, $rootScope) {

    $scope.PageConfig = $rootScope.PageConfig;

    $scope.User = {
        Email: '',
        Password: '',
        Message: LoginMessage,
        Sending: false,
        Sent: false,
        Send: function() {
            $scope.User.Sending = true;
            $('.login-form').submit();
        }
    };

    console.log($scope.User.Message);

}]);// Login Controller

// Register Controller
app.controller('RegisterController', ['$http', '$scope', '$rootScope', function($http, $scope, $rootScope) {

    $scope.PageConfig = $rootScope.PageConfig;

    $scope.User = {
        Email: '',
        Message: '',
        Registered: false,
        EmailSent: false,
        EmailConfirmed: false,
        Sending: false,
        Sent : false,
        Send : function() {
            $scope.User.Sending = true;

            $http({
                url: RegisterConfig.Url,
                method: 'POST',
                data: {
                    Email: $scope.User.Email,
                }
            }).then(function (returnData) {
                $scope.form.Sending = false;
                $scope.form.Sent = true;

                switch (returnData.data.Status) {
                    case "Success":
                        $scope.User.Registered = false;
                        $scope.User.EmailSent = false;
                        $scope.User.EmailConfirmed = false;
                        $scope.User.Email= '';
                        break;
                    case "AlreadyRegistered":
                        $scope.User.Registered = true;
                        $scope.User.EmailSent = true;
                        $scope.User.EmailConfirmed = true;
                        $scope.User.Email = '';
                        break;
                    case "AlreadyRegisteredButUnconfirmed":
                        $scope.User.Registered = true;
                        $scope.User.EmailSent = true;
                        $scope.User.EmailConfirmed = false;
                        break;
                    case "InvalidInputData":
                        $scope.User.Email = '';
                        break;
                }

            }, function (returnData) {
                console.log('Failed requesting reset password');
                console.log(returnData);
                $scope.User.Sending = false;
                $scope.User.Sent = false;
            });

        }

    }

}]);// Register Controller

// Forgot Password Controller
app.controller('ForgotpasswordController', ['$http', '$scope', '$rootScope', function ($http, $scope, $rootScope) {

    $scope.PageConfig = $rootScope.PageConfig;

    $scope.EmailForm = {
        Email: "",
        Sending: false,
        Sent: false,
        ReturnData : {
            Found: false,
            EmailConfirmed: false
        },
        SendForm: function () {
            $scope.EmailForm.Sending = true;
            // send form
            // submit form to URL
            $http({
                url: ForgotPasswordConfig.Url,
                method: 'POST',
                data: {
                    email: $scope.EmailForm.Email
                }
            }).then(function (returnData) {
                $scope.EmailForm.Sending = false;
                $scope.EmailForm.Sent = true;
                console.log(returnData);

                switch (returnData.data.Status) {
                    case "Success":
                        $scope.EmailForm.ReturnData.Found = true;
                        $scope.EmailForm.ReturnData.EmailConfirmed = true;
                        break;
                    case "NotRegistered":
                        $scope.EmailForm.ReturnData.Found = false;
                        break;
                    case "AlreadyRegisteredButUnconfirmed":
                        $scope.EmailForm.ReturnData.Found = true;
                        $scope.EmailForm.ReturnData.EmailConfirmed = false;
                        break;
                    case "InvalidInputData":
                        $scope.EmailForm.ReturnData.Found = false;
                        break;
                }
            }, function (returnData) {
                console.log('Failed requesting reset password');
                console.log(returnData);
                $scope.EmailForm.Sending = false;
            });
        }
    };

}]);// ForgotPasswordController