// User Account Controller
app.controller('UserAccountController', ['$http', '$scope', '$rootScope', '$location',function ($http, $scope, $rootScope, $location) {

    $scope.PageConfig = $rootScope.PageConfig;
    $scope.Countries = $rootScope.Countries;
    //$scope.Order;
    $scope.pageLoaded = true;
    $scope.formMessage = '';
    $scope.NotifBox = false;
    if (order.length) {
        $scope.Order = JSON.parse(order);
    }

    // order status
    $scope.OrderStatus = function(num) {
        var text = '';
        switch (num) {
            case 1:
                text = 'Cancel';
                break;
            case 2:
                text = 'Pending';
                break;
            case 3:
                text = 'Settled';
                break;
            case 4:
                text = 'Denied';
                break;
            case 5:
                text = 'Expired';
                break;
            case 6:
                text = 'Veryfing';
                break;
            case 7:
                text = 'Challanged';
                break;
        }
        return text;
    }
    // flight type
    $scope.FlightType = function(num) {
        var text = '';
        switch (num) {
            case 1:
                text = 'OneWay';
                break;
            case 2:
                text = 'Return';
                break;
        }
        return text;
    }

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


    $scope.$watch('PageConfig.ActivePage', function() {
        $scope.NotifBox = false;

        //if ($scope.PageConfig.ActivePage == 'menu') {
        //}
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
    $scope.EditProfile.Submit = function (name) {
        // edit profile form
        if (name == 'profile') {
            $scope.UserProfile.edit = !($scope.UserProfile.edit);
            $scope.UserProfile.updated = false;
        }
        else if (name == 'profileSave') {
            console.log('submitting form');
            $scope.UserProfile.updating = true;
            // submit form to URL
            $http({
                url: ChangeProfileMobileConfig.Url,
                method: 'POST',
                data: {
                    Address: editProfile.address,
                    FirstName: editProfile.firstname,
                    LastName: editProfile.lastname,
                    PhoneNumber: editProfile.phone,
                    CountryCd: editProfile.country
                }
            }).then(function (returnData) {
                console.log(returnData);
                if (returnData.data.Status == 'Success') {
                    console.log('Success requesting change profile');
                    $scope.UserProfile.address = editProfile.address;
                    $scope.UserProfile.firstname = editProfile.firstname;
                    $scope.UserProfile.lastname = editProfile.lastname;
                    $scope.UserProfile.phone = editProfile.phone;
                    $scope.UserProfile.country = editProfile.country;
                    $scope.UserProfile.edit = false;
                    $scope.UserProfile.updating = false;
                    $scope.UserProfile.updated = true;
                    $scope.formMessage = 'Profile Anda Telah Berhasil Diperbaharui';
                    $scope.NotifBox = true;
                }
                else {
                    console.log(returnData.data.Description);
                    $scope.UserProfile.edit = true;
                    $scope.UserProfile.updating = false;
                    $scope.formMessage = 'Profile Anda Belum Diperbaharui';
                    $scope.NotifBox = true;
                }
            }, function (returnData) {
                console.log('Failed requesting change profile');
                console.log(returnData);
                $scope.profileForm.edit = true;
                $scope.UserProfile.updating = false;
                $scope.formMessage = 'Profile Anda Belum Diperbaharui';
                $scope.NotifBox = true;
            });
        }
        if (name == 'password') {
            $scope.password.edit = !($scope.password.edit);
            $scope.password.failed = false;
        }
        else if (name == 'passwordSave') {

            $scope.password.updating = true;
            $scope.password.failed = false;

            console.log('submitting form');
            // submit form to URL
            $http({
                url: ChangePasswordConfig.Url,
                method: 'POST',
                data: {
                    NewPassword: $scope.passwordForm.newPassword,
                    OldPassword: $scope.passwordForm.currentPassword,
                    ConfirmPassword: $scope.passwordForm.confirmationPassword
                }
            }).then(function (returnData) {
                $scope.passwordForm.newPassword = '';
                $scope.passwordForm.currentPassword = '';
                $scope.passwordForm.confirmationPassword = '';
                if (returnData.data.Status == 'Success') {
                    console.log('Success requesting reset password');
                    console.log(returnData);
                    $scope.password.edit = false;
                    $scope.password.updating = false;
                    
                }
                else {
                    console.log(returnData.data.Description);
                    console.log(returnData);
                    $scope.password.updating = false;
                    $scope.password.failed = true;
                }
            }, function (returnData) {
                console.log('Failed requesting reset password');
                console.log(returnData);
                $scope.password.edit = true;
                $scope.password.updating = false;
            });
        }
        
    }
    // user password
    $scope.UserPassword = {
        CurrentPassword: '',
        NewPassword: '',
        ConfirmPassword: '',
        Editing: false,
        Message: '',
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
                url: ChangePasswordMobileConfig.Url,
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
                    $scope.formMessage = 'Password Anda Telah Berhasil Diperbaharui';
                    $scope.NotifBox = true;
                }
                else {
                    console.log(returnData.data.Description);
                    console.log(returnData);
                    $scope.UserPassword.Updating = false;
                    $scope.UserPassword.Editing = false;
                    if (returnData.data.Description == 'ModelInvalid') {
                        $scope.formMessage = 'Password Baru yang Anda Masukkan Tidak Cocok dengan yang Dikonfimasi';
                    } else {
                        $scope.formMessage = 'Password yang Anda Masukkan Tidak Tepat';
                    }
                    $scope.NotifBox = true;

                }
            }, function (returnData) {
                console.log('Failed requesting reset password');
                console.log(returnData);
                $scope.UserPassword.Editing = false;
                $scope.UserPassword.Updating = false;
                $scope.formMessage = 'Permohonan ganti password Anda gagal';
                $scope.NotifBox = true;
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
    $scope.orderDate = new Date(orderDate);

}]);// Order Detail Controller

// Check Order Controller
app.controller('CheckOrderController', ['$http', '$scope', '$rootScope', function($http, $scope, $rootScope) {
    
    $scope.PageConfig = $rootScope.PageConfig;
    $scope.pageLoaded = true;
    $scope.Order = {
        Number: '',
        Name : ''
    };

}]);// Check Order Controller

// Login Controller
app.controller('LoginController', ['$http', '$scope', '$rootScope', function($http, $scope, $rootScope) {

        $scope.PageConfig = $rootScope.PageConfig;
        $scope.pageLoaded = true;
        $scope.overlay = false;

        //if ($scope.User.Message) {
        //    $scope.overlay = true;
        //} else {
        //    $scope.overlay = false;
        //}
        $scope.closeOverlay = function() {
            $scope.overlay = false;
            $scope.User.Message = '';
        }

    $scope.User = {
        Email: '',
        newEmail: '',
            Resubmitting: false,
            Resubmitted: false,
            Password: '',
            Message: '',
            Sending: false,
            Sent: false,
            Send: function() {
                $scope.User.Message = '';
                $scope.User.Sending = true;
                $http({
                    url: LoginMobileConfig.Url,
                    method: 'POST',
                    data: {
                        Email: $scope.User.Email,
                        Password: $scope.User.Password,
                    }
                }).then(function (returnData) {
                    $scope.User.Resubmitting = false;

                    if (returnData.data.Status == "Success") {
                        $scope.User.Email = '';
                        $scope.User.Resubmitted = true;
                    }

                }, function (returnData) {
                    console.log('Failed requesting confirmation e-mail');
                    console.log(returnData);
                    $scope.submitting = false;
                    $scope.submitted = false;
                });

            },
            Reconfirm: function () {
                $scope.User.Resubmitting = true;
                $http({
                    url: ResendConfirmationEmailMobileConfig.Url,
                    method: 'POST',
                    data: {
                        Email: $scope.User.newEmail,
                    }
                }).then(function (returnData) {
                    $scope.User.Resubmitting = false;
                    
                    if (returnData.data.Status == "Success") {
                        $scope.User.Email = '';
                        $scope.User.Resubmitted = true;
                    }

                }, function (returnData) {
                    console.log('Failed requesting confirmation e-mail');
                    console.log(returnData);
                    $scope.submitting = false;
                    $scope.submitted = false;
                });
            }
        }

    $scope.User.Message = LoginMessage;

}]);// Login Controller

// Register Controller
app.controller('RegisterController', ['$http', '$scope', '$rootScope', function($http, $scope, $rootScope) {

    $scope.PageConfig = $rootScope.PageConfig;
    $scope.pageLoaded = true;
    $scope.overlay = false;
    $scope.closeOverlay = function () {
        $scope.overlay = false;
        $scope.User.Sending = false;
        $scope.User.Sent = false;
    }
    $scope.validateEmail = function (email) {
        var re = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;
        return re.test(email);
    }
    $scope.User = {
        Resubmitted: false,
        Resubmitting: false,
        resubmitok: false,
        Email: '',
        Message: '',
        Registered: false,
        Newsletter: false,
        EmailSent: false,
        EmailConfirmed: false,
        Sending: false,
        Sent: false,
        Reconfirm: function () {
            $scope.User.Resubmitting = true;
            $http({
                url: ResendConfirmationEmailMobileConfig.Url,
                method: 'POST',
                data: {
                    Email: $scope.User.Email,
                }
            }).then(function (returnData) {
                $scope.User.Resubmitting = false;
                

                if (returnData.data.Status == "Success") {
                    $scope.User.Email = '';
                    $scope.User.Resubmitted = true;
                }

            }, function (returnData) {
                console.log('Failed requesting confirmation e-mail');
                console.log(returnData);
                $scope.submitting = false;
                $scope.submitted = false;
            });
        },
        Send : function() {
            if (!($scope.validateEmail($scope.User.Email))) {
                $scope.User.Email = '';
                alert('Alamat E-mail Tidak Valid');
            } else {
                if ($scope.User.Email) {
                    $scope.User.Submit();
                } else {
                    alert('Masukkan Alamat E-mail dengan Format name@domain.com');
                }
            }
        },
        Submit: function() {
            $scope.User.Sending = true;
            $http({
                url: RegisterMobileConfig.Url,
                method: 'POST',
                data: {
                    'Email': $scope.User.Email,
                    'Name': $scope.User.Name,
                }
            }).success(function(returnData) {
                $scope.User.Sending = false;
                $scope.User.Sent = true;
                switch (returnData.Status) {
                    case "Success":
                        $scope.User.Registered = false;
                        $scope.User.EmailSent = false;
                        $scope.User.EmailConfirmed = false;
                        $scope.User.Email = '';
                        break;
                    case "AlreadyRegistered":
                        $scope.User.Registered = true;
                        $scope.User.EmailSent = false;
                        $scope.User.EmailConfirmed = true;
                        $scope.User.Email = '';
                        break;
                    case "AlreadyRegisteredButUnconfirmed":
                        $scope.User.Registered = true;
                        $scope.User.EmailSent = true;
                        $scope.User.EmailConfirmed = false;

                        break;
                    case "InvalidInputData":
                        break;
                }
                //
                if ($scope.Subscribe) {
                    $scope.User.Newsletter = true;
                } else {
                    $scope.User.Newsletter = false;
                }
                
            })
                .error(function (returnData) {
                    console.log('Failed');
                    console.log(returnData);
                    $scope.User.Sending = false;
                    $scope.User.Sent = false;
                });
        },
        ReconfirmSent: function() {
            $scope.User.Resubmitted = false;
        }
    }

    $scope.Subscribe = function () {
        var temp = false;
        $http({
            url: SubscribeConfig.Url,
            method: 'POST',
            data: {
                'address': $scope.User.Email,
            }
        }).success(function (returnData) {
            $scope.User.showBox = true;
            console.log('Succeed');
            $scope.User.Sending = false;
            if (returnData == true) {
                temp = true;
            } else {
                temp = false;
            }
            //$scope.User.Sent = true;
        })
            .error(function (returnData) {
                console.log(returnData);
                $scope.User.showBox = true;
                $scope.User.Sending = false;
                $scope.User.Sent = false;
                temp = false;
            });

        return temp;
    }

}]);// Register Controller

// Forgot Password Controller
app.controller('ForgotpasswordController', ['$http', '$scope', '$rootScope', function ($http, $scope, $rootScope) {

    $scope.PageConfig = $rootScope.PageConfig;
    $scope.pageLoaded = true;
    $scope.overlay = false;
    $scope.closeOverlay = function() {
        $scope.overlay = false;
        $scope.EmailForm.Sending = false;
        $scope.EmailForm.Sent = false;
    }

    $scope.EmailForm = {
        Email: "",
        Resubmit: false,
        SentReconfirm: false,
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
                url: ForgotPasswordMobileConfig.Url,
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
                        $scope.EmailForm.ReturnData.EmailConfirmed = true;
                        break;
                    case "AlreadyRegisteredButUnconfirmed":
                        $scope.EmailForm.ReturnData.Found = true;
                        $scope.EmailForm.ReturnData.EmailConfirmed = false;
                        break;
                    case "InvalidInputData":
                        $scope.EmailForm.ReturnData.Found = false;
                        $scope.EmailForm.ReturnData.EmailConfirmed = false;
                        break;
                }
            }, function (returnData) {
                console.log('Failed requesting reset password');
                console.log(returnData);
                $scope.EmailForm.Sending = false;
            });
        },
        Reconfirm: function() {
                $scope.EmailForm.Resubmit = true;
                $http({
                    url: ResendConfirmationEmailMobileConfig.Url,
                    method: 'POST',
                    data: {
                        Email: $scope.EmailForm.Email,
                    }
                }).then(function (returnData) {
                    $scope.EmailForm.Resubmit = false;
                    $scope.EmailForm.SentReconfirm = true;

                    if (returnData.data.Status == "Success") {
                        $scope.EmailForm.Email = '';
                    }

                }, function (returnData) {
                    console.log('Failed requesting reset password');
                    console.log(returnData);
                    $scope.EmailForm.Resubmit = false;
                    $scope.EmailForm.SentReconfirm = false;
                });
            
        }
    };

}]);// ForgotPasswordController

app.controller('ResetpasswordController', ['$http', '$scope', '$rootScope', function ($http, $scope, $rootScope) {

        //$scope.userEmail = '';

        $scope.PageConfig = $rootScope.PageConfig;
        $scope.pageLoaded = true;
        $scope.form = {
            submitted: false,
            submitting: false,
            lala: false,
            userEmail: userEmail,
            code: code,
            password: ''
        };
        $scope.submit = function () {
            $scope.form.submitting = true;

            $http({
                url: ResetPasswordMobileConfig.Url,
                method: 'POST',
                data: {
                    Password: $scope.form.password,
                    ConfirmPassword: $scope.form.password,
                    Email: $scope.form.userEmail,
                    Code: $scope.form.code
                }
            }).then(function (returnData) {
                if (returnData.data.Status == 'Success') {
                    console.log('Success requesting reset password');
                    console.log(returnData);
                    $scope.form.submitting = false;
                    $scope.form.submitted = true;
                }
                else {
                    console.log(returnData.data.Description);
                    console.log(returnData);
                    $scope.form.lala = true;
                    $scope.form.submitted = true;
                }
            }, function (returnData) {
                console.log('Failed requesting reset password');
                console.log(returnData);
                $scope.form.submitting = false;
                $scope.form.lala = true;
            });

        }

    }
]);