// User Account Controller

app.controller('siteHeaderController', [
    '$http', '$scope', function ($http, $scope) {
        $scope.profileloaded = false;
        $scope.email = '';
        $scope.authProfile = {}
        $scope.ProfileConfig = {
            getProfile: function () {
                $scope.firstName = '';
                $scope.lastName = '';
                $http.get(GetProfileConfig.Url, {
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).success(function (returnData) {
                    if (returnData.status == "200") {
                        $scope.pageLoaded = true;
                        $scope.isLogin = true;
                        console.log('Success getting Profile');
                        $scope.email = returnData.email;
                        $scope.profileloaded = true;
                        console.log(returnData);
                        $scope.firstName = returnData.first;
                        $scope.lastName = returnData.last;
                    }
                    else {
                        console.log('There is an error');
                        console.log('Error : ' + returnData.error);
                        $scope.pageLoaded = true;
                        $scope.isLogin = false;
                        $scope.profileloaded = true;
                        console.log(returnData);
                    }
                }, function (returnData) {
                    console.log('Failed to Login');
                    $scope.pageLoaded = true;
                    $scope.isLogin = false;
                    $scope.profileloaded = true;
                    console.log(returnData);
                });
            }
        }
        //Set Authorization to get Profile
        $scope.authAccess = getAuthAccess();
        if ($scope.authAccess == 1) {
            $scope.ProfileConfig.getProfile();
        }
        else {
            $scope.isLogin = false;
            $scope.pageLoaded = true;
        }
        $scope.returnUrl = document.referrer;
        $scope.logout = {
            isLogout: false,
            getLogout: function () {
                eraseCookie('accesstoken');
                eraseCookie('refreshtoken');
                $scope.isLogin = false;
                $scope.logout.isLogout =  true;
                window.location.href = "/";
            }
        }

    }]);


app.controller('UserAccountController', ['$http', '$scope', '$rootScope', '$location',function ($http, $scope, $rootScope, $location) {

    $scope.PageConfig = $rootScope.PageConfig;
    $scope.Countries = $rootScope.Countries;
    //$scope.Order;
    $scope.pageLoaded = true;
    $scope.formMessage = '';
    $scope.NotifBox = false;
    //if (order.length) {
    //    $scope.Order = JSON.parse(order);
    //}
    $scope.getdata = function() {
        $http.get(GetProfileConfig.Url, {
            headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
        }).success(function (returnData) {
            if (returnData.status == "200") {
                $scope.pageLoaded = true;
                $scope.isLogin = true;
                console.log('Success getting Profile');
                $scope.email = returnData.email;
                $scope.profileloaded = true;
                console.log(returnData);
                $scope.UserProfile.firstname = returnData.first;
                $scope.UserProfile.lastname = returnData.last;
                $scope.UserProfile.phone =returnData.phone;
                $scope.UserProfile.country = returnData.countryCallCd;
            }
            else {
                console.log('There is an error');
                console.log('Error : ' + returnData.error);
                $scope.pageLoaded = true;
                $scope.isLogin = false;
                $scope.profileloaded = true;
                console.log(returnData);
            }
        }, function (returnData) {
            console.log('Failed to Login');
            $scope.pageLoaded = true;
            $scope.isLogin = false;
            $scope.profileloaded = true;
            console.log(returnData);
        });
    }
    $scope.getdata();
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
            case 0:
                text = 'OneWay';
                break;
            case 1:
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
            var authAccess = getAuthAccess();
            if (authAccess == 1) {
                $scope.getFlightHeader = 'Bearer ' + getCookie('accesstoken');
            }
            else {
                $scope.getFlightHeader = null;
            }

            // submit form to URL
            $http({
                url: ChangeProfileConfig.Url,
                method: 'PATCH',
                data: {
                    first: editProfile.firstname,
                    last: editProfile.lastname,
                    phone: editProfile.phone,
                    countryCallCd: editProfile.country
                },
                headers: { 'Authorization': $scope.getFlightHeader }
            }).then(function (returnData) {
                console.log(returnData);
                if (returnData.data.status == '200') {
                    console.log('Success requesting change profile');
                    //$scope.UserProfile.address = editProfile.address;
                    $scope.UserProfile.firstname = editProfile.firstname;
                    $scope.UserProfile.lastname = editProfile.lastname;

                    $scope.UserProfile.phone = editProfile.phone;
                    $scope.UserProfile.country = editProfile.country;
                    $scope.UserProfile.edit = false;
                    $scope.UserProfile.updating = false;
                    $scope.UserProfile.updated = true;
                    $scope.formMessage = 'Profil Anda Telah Berhasil Diperbaharui';
                    $scope.NotifBox = true;
                }
                else {
                    console.log(returnData.data.error);
                    $scope.UserProfile.edit = true;
                    $scope.UserProfile.updating = false;
                    $scope.formMessage = 'Profil Anda Belum Diperbaharui';
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
            authAccess = getAuthAccess();
            if (authAccess == 1) {
                $scope.getFlightHeader = 'Bearer ' + getCookie('accesstoken');
            }
            else {
                $scope.getFlightHeader = null;
            }
            // submit form to URL
            $http({
                url: ChangePasswordConfig.Url,
                method: 'POST',
                data: {
                    newPassword: $scope.passwordForm.newPassword,
                    oldPassword: $scope.passwordForm.currentPassword
                },
                headers: { 'Authorization': $scope.getFlightHeader }
            }).then(function (returnData) {
                $scope.passwordForm.newPassword = '';
                $scope.passwordForm.currentPassword = '';
                $scope.passwordForm.confirmationPassword = '';
                if (returnData.data.status == '200') {
                    console.log('Success requesting reset password');
                    console.log(returnData);
                    $scope.password.edit = false;
                    $scope.password.updating = false;
                    
                }
                else {
                    console.log(returnData.data.error);
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
            var authAccess = getAuthAccess();
            if (authAccess == 1) {
                $scope.getFlightHeader = 'Bearer ' + getCookie('accesstoken');
            }
            else {
                $scope.getFlightHeader = null;
            }

            $http({
                url: ChangePasswordConfig.Url,
                method: 'POST',
                data: {
                    newPassword: $scope.UserPassword.NewPassword,
                    oldPassword: $scope.UserPassword.CurrentPassword
                },
                headers: { 'Authorization': $scope.getFlightHeader }
            }).then(function (returnData) {
                $scope.UserPassword.NewPassword = '';
                $scope.UserPassword.CurrentPassword = '';
                $scope.UserPassword.ConfirmPassword = '';
                if (returnData.data.status == '200') {
                    console.log('Success requesting reset password');
                    console.log(returnData);
                    $scope.UserPassword.Updating = false;
                    $scope.UserPassword.Editing = false;
                    $scope.formMessage = 'Password Anda Telah Berhasil Diperbaharui';
                    $scope.NotifBox = true;
                }
                else {
                    //console.log(returnData.data.Description);
                    console.log(returnData);
                    $scope.UserPassword.Updating = false;
                    $scope.UserPassword.Editing = false;
                    //if (returnData.data.Description == 'ModelInvalid') {
                    //    $scope.formMessage = 'Password Baru yang Anda Masukkan Tidak Cocok dengan yang Dikonfimasi';
                    //} else {
                    $scope.formMessage = 'Password Lama yang Anda Masukkan Tidak Tepat';
                    //}
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

    //Get Transaction History

    $scope.trxHistory = {
        getTrxHistory: function () {
            //Check Authorization
            var authAccess = getAuthAccess();
            if (authAccess == 1) {
                $scope.getFlightHeader = 'Bearer ' + getCookie('accesstoken');
            }
            else {
                $scope.getFlightHeader = null;
            }
            $http.get(TrxHistoryConfig.Url, {
                headers: { 'Authorization': $scope.getFlightHeader }
            }).success(function (returnData) {
                if (returnData.status == "200") {
                    console.log('Success getting Transaction');
                    console.log(returnData);
                    $scope.flightlist = returnData.flights;
                    var flights = [];

                    for (var i = 0; i < $scope.flightlist.length; i++) {
                        if ($scope.flightlist[i] !== null) {
                            flights.push($scope.flightlist[i]);
                        }
                    }
                    $scope.flightlist = flights;
                    console.log($scope.flightlist);
                }
                else {
                    console.log('There is an error');
                    console.log('Error : ' + returnData.error);
                    console.log(returnData);
                }
            }, function (returnData) {
                console.log('Failed to Get Transaction History');
                console.log(returnData);
            });
        }
    }

    $scope.trxHistory.getTrxHistory();
    
    $scope.returnUrl = "/";
    $scope.logout = {
        isLogout: false,
        getLogout: function () {
            eraseCookie('accesstoken');
            eraseCookie('refreshtoken');
            //$scope.isLogin = false;
            $scope.logout.isLogout = true;
            window.location.href = "/";
        }
    }

}]);// User Account Controller

// Contact Controller
app.controller('ContactController', ['$http', '$scope', '$rootScope', function ($http, $scope, $rootScope) {

    $scope.PageConfig = $rootScope.PageConfig;

}]);// Contact Controller

// Order Detail Controller
app.controller('OrderDetailController', ['$http', '$scope', '$rootScope', function ($http, $scope, $rootScope) {

    $scope.PageConfig = $rootScope.PageConfig;
    $scope.orderDate = new Date(orderDate);
    $scope.rsvNo = rsvNo;
    $scope.var = 3;
    $scope.flight = [];
    $scope.title = function(title) {
        if (title == '1')
            return 'Tn.';
        else if (title == '2')
            return 'Ny.';
        else if (title == '3')
            return 'Nn.';
    }
    $scope.paxtype = function(types) {
        if (types == 1)
            return 'Dewasa';
        else if (types == 2)
            return 'Anak';
        else if (types == 3)
            return 'Bayi';
    }

    $scope.methodpayment = function(types) {
        if (types == 1)
            return 'Kartu Kredit';
        else if (types == 2)
            return 'Transfer Antar Bank';
        else if (types == 3)
            return 'Mandiri Clickpay';
        else if (types == 4)
            return 'Cimb Clicks';
        else if (types == 5)
            return 'Virtual Account';
        else if (types == 6)
            return 'BCA Klikpay';
        else if (types == 7)
            return 'Epay BRI ';
        else if (types == 8)
            return 'Telkomsel T-Cash';
        else if (types == 9)
            return 'XL Tunai';
        else if (types == 10)
            return 'BBM Money';
        else if (types == 11)
            return 'Indosat Dompetku';
        else if (types == 12)
            return 'Mandiri E-Cash';
        else if (types == 13)
            return 'Mandiri Bill Payment';
        else if (types == 14)
            return 'Indomaret';
        else if (types == 15)
            return 'Credit';
        else if (types == 16)
            return 'Deposit';
    }

    $scope.paymentstatus = function(types) {
        if (types == 3)
            return 'Lunas';
        else if (types == 1)
            return 'Dibatalkan';
        else if (types == 2)
            return 'Menunggu Pembayaran';
        else if (types == 4)
            return 'Ditolak';
        else if (types == 5)
            return 'Kadaluarsa';
        else if (types == 6)
            return 'Memverifikasi Pembayaran';
        else if (types == 7)
            return 'Butuh Verifikasi Ulang';
        else if (types == 8)
            return 'Gagal';
    }

    $scope.getRsv = function() {
        $http.get(GetReservationConfig.Url + $scope.rsvNo, {
            headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') },
            rsvNo: $scope.rsvNo
        }).success(function (returnData) {
            if (returnData.status == "200") {
                $scope.flight = returnData.flight;
                $scope.datafailed = false;
            }
            else {
                console.log('There is an error');
                console.log('Error : ' + returnData.error);
                $scope.datafailed = true;
                console.log(returnData);
                window.location.href = "/";
            }
        }, function (returnData) {
            console.log('Failed to Get Detail');
            $scope.datafailed = true;
            console.log(returnData);
        });
    }

    $scope.getRsv();

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
        $scope.returnUrl = document.referrer;
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
                $scope.User.Sending = true;
                $http.post(LoginConfig.Url, {
                    email: $scope.User.Email,
                    password: $scope.User.Password,
                    clientId: 'Jajal',
                    clientSecret: 'Standar'
                }).then(function (returnData) {
                    $scope.User.Resubmitting = false;
                    if (returnData.data.status == '200') {
                        setCookie('accesstoken', returnData.data.accessToken, returnData.data.expTime);
                        setRefreshCookie('refreshtoken', returnData.data.FlightConfigrefreshToken);
                        $scope.User.Email = '';
                        $scope.User.Resubmitted = true;
                        window.location.href = "/";
                    }
                    else {
                        $scope.overlay = true;
                        if (returnData.data.error == 'ERALOG01') { $scope.User.Message = 'RefreshNeeded'; }
                        else if (returnData.data.error == 'ERALOG02') { $scope.User.Message = 'InvalidInputData'; } //
                        else if (returnData.data.error == 'ERALOG03') {
                            $scope.User.Message = 'AlreadyRegisteredButUnconfirmed';
                            $scope.User.Resubmitted = false;
                        } //
                        else if (returnData.data.error == 'ERALOG04') { $scope.User.Message = 'Failed'; } //
                        else if (returnData.data.error == 'ERALOG05') { $scope.User.Message = 'NotRegistered'; } //
                        else { $scope.errorMessage = 'Failed'; }
                        console.log('Error : ' + returnData.error);
                        $scope.User.Sending = false;
                    }
                }, function (returnData) {
                    console.log('Failed requesting confirmation e-mail');
                    console.log(returnData);
                    $scope.User.Message = 'Failed';
                });
            },

            Reconfirm: function () {
                $scope.User.Resubmitting = true;
                $http({
                    url: ResendConfirmationEmailConfig.Url,
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
        Invalid : false,
        Error: false,
        Registered: false,
        Newsletter: false,
        EmailSent: false,
        EmailConfirmed: false,
        Sending: false,
        Sent: false,
        Reconfirm: function () {
            $scope.User.Resubmitting = true;
            $http({
                url: ResendConfirmationEmailConfig.Url,
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
             
            $http.post(RegisterConfig.Url, {
                email: $scope.User.Email,
                
            }).success(function(returnData) {
                $scope.User.Sending = false;
                $scope.User.Sent = true;
                if (returnData.status == '200') {
                    $scope.User.Registered = false;
                    $scope.User.EmailSent = false;
                    $scope.User.EmailConfirmed = false;
                    $scope.User.Email = '';
                }
                else {
                    switch (returnData.error) {
                        case "ERAREG02":
                            $scope.User.Registered = true;
                            $scope.User.EmailSent = true;
                            $scope.User.EmailConfirmed = false;
                            break;
                        case "ERAREG03":
                            $scope.User.Registered = true;
                            $scope.User.EmailSent = false;
                            $scope.User.EmailConfirmed = true;
                            $scope.User.Email = '';
                            break;
                        case "ERRGEN99":
                            $scope.User.Error = true;
                            break;
                        case "ERAREG01":
                            $scope.User.Invalid = true;
                            break;
                    }
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
        Email1: "",
        Resubmit: false,
        SentReconfirm: false,
        Sending: false,
        Sent: false,
        ReturnData : {
            Found: false,
            EmailConfirmed: false,
            Error: false
        },
        SendForm: function () {
            $scope.EmailForm.Sending = true;
            // send form
            // submit form to URL
            var authAccess = getAuthAccess();
            if (authAccess == 1) {
                $scope.getFlightHeader = 'Bearer ' + getCookie('accesstoken');
            }
            else {
                $scope.getFlightHeader = null;
            }
            $http({
                url: ForgotPasswordConfig.Url,
                method: 'POST',
                data: {
                    email: $scope.EmailForm.Email
                },
                headers: { 'Authorization': $scope.getFlightHeader }
            }).then(function (returnData) {
                $scope.EmailForm.Sending = false;
                $scope.EmailForm.Sent = true;
                console.log(returnData);
                if (returnData.data.status == '200') {
                    $scope.EmailForm.ReturnData.Found = true;
                    $scope.EmailForm.ReturnData.EmailConfirmed = true;
                }
                else {
                    switch (returnData.data.error) {
                        case "ERAFPW01":
                            $scope.EmailForm.ReturnData.Found = false;
                            $scope.EmailForm.ReturnData.EmailConfirmed = false;
                            break;
                        case "ERAFPW02":
                            $scope.EmailForm.ReturnData.Found = false;
                            $scope.EmailForm.ReturnData.EmailConfirmed = true;
                            break;
                        case "ERAFPW03":
                            $scope.EmailForm.ReturnData.Found = true;
                            $scope.EmailForm.ReturnData.EmailConfirmed = false;
                            $scope.EmailForm.Email1 = $scope.EmailForm.Email;
                            break;
                        case "ERRGEN99":
                            $scope.EmailForm.ReturnData.Error = true;
                            break;
                    }
                }
                
            }, function (returnData) {
                console.log('Failed requesting reset password');
                console.log(returnData);
                $scope.EmailForm.Sending = false;
                $scope.EmailForm.ReturnData.Error = true;
            });
        },
        Reconfirm: function() {
                $scope.EmailForm.Resubmit = true;
                $http({
                    url: ResendConfirmationEmailConfig.Url,
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
            var authAccess = getAuthAccess();
            if (authAccess == 1) {
                $scope.getFlightHeader = 'Bearer ' + getCookie('accesstoken');
            }
            else {
                $scope.getFlightHeader = null;
            }

            $http({
                url: ResetPasswordMobileConfig.Url,
                method: 'POST',
                data: {
                    Password: $scope.form.password,
                    ConfirmPassword: $scope.form.password,
                    Email: $scope.form.userEmail,
                    Code: $scope.form.code
                },
                headers: { 'Authorization': $scope.getFlightHeader }
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