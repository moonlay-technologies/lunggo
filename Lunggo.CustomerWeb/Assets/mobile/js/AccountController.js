// User Account Controller
app.controller('UserAccountController', ['$http', '$scope', '$rootScope', '$location',function ($http, $scope, $rootScope, $location) {

    $scope.PageConfig = $rootScope.PageConfig;
    $scope.Countries = $rootScope.Countries;
    // change page
    $scope.PageConfig.ActivePage = 'menu';
    $scope.PageConfig.ActivePageChanged = false;
    $scope.PageConfig.ChangePage = function(page) {
        $scope.PageConfig.ActivePage = page;
        $scope.PageConfig.ActivePageChanged = true;
        $location.hash(page);
    }
    angular.element(document).ready(function () {
        $location.hash('menu');
    });
    $scope.$watch(function () {
        return location.hash;
    }, function (value) {
        if (!$scope.PageConfig.ActivePageChanged) {
            if (value != '') {
                $scope.PageConfig.ChangePage('menu');
            }
        } else {
            if (value == '' || value == '##menu') {
                $scope.PageConfig.ActivePage = 'menu';
            } else if (value == '##history' || value == 'history') {
                $scope.PageConfig.ActivePage = 'history';
            } else if (value == '##profile' || value == 'profile') {
                $scope.PageConfig.ActivePage = 'profile';
            }
        }
    });
    // user profile
    $scope.UserProfile = userProfile;
    $scope.UserProfile.Editing = false;
    $scope.UserProfile.Edit = function () {
        if ($scope.UserProfile.Editing == false) {
            $scope.UserProfile.Editing = true;
        } else {
            $scope.UserProfile.Editing = false;
        }
    }
    $scope.EditProfile = editProfile;
    $scope.EditProfile.Updating = false;
    $scope.EditProfile.Failed = false;
    $scope.EditProfile.Submit = function() {
        
    }
    // user password
    $scope.UserPassword = {
        CurrentPassword: '',
        NewPassword: '',
        ConfirmPassword: '',
        Editing: false,
        Edit: function() {
            if ($scope.UserPassword.Editing == false) {
                $scope.UserPassword.Editing = true;
            } else {
                $scope.UserPassword.Editing = false;
            }
        },
        Updating: false,
        Failed: false,
        Submit: function () {
            $scope.UserPassword.Updating = true;
            $http({
                url: ChangePasswordConfig.Url,
                method: 'POST',
                data: {
                    NewPassword: $scope.UserPassword.NewPassword,
                    OldPassword: $scope.UserPassword.CurrentPassword,
                    ConfirmPassword: $scope.UserPassword.ConfirmPassword
                }
            }).then(function (returnData) {
                $scope.UserPassword.NewPassword = '';
                $scope.UserPassword.CurrentPassword = '';
                $scope.UserPassword.ConfirmPassword = '';
                if (returnData.data.Status == 'Success') {
                    console.log('Success requesting reset password');
                    console.log(returnData);
                    $scope.UserPassword.Updating = false;
                    $scope.UserPassword.Editing = false;
                }
                else {
                    console.log(returnData.data.Description);
                    console.log(returnData);
                    $scope.UserPassword.Updating = false;
                    $scope.UserPassword.Editing = false;
                }
            }, function (returnData) {
                console.log('Failed requesting reset password');
                console.log(returnData);
                $scope.UserPassword.Editing = false;
                $scope.UserPassword.Updating = false;
            });
        }
    };

}]);// User Account Controller

// Contact Controller
app.controller('ContactController', ['$http', '$scope', '$rootScope', function ($http, $scope, $rootScope) {

    $scope.PageConfig = $rootScope.PageConfig;

}]);// Contact Controller

// Order Detail Controller
app.controller('OrderDetailController', ['$http', '$scope', '$rootScope', function ($http, $scope, $rootScope) {

    $scope.PageConfig = $rootScope.PageConfig;

}]);// Order Detail Controller

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