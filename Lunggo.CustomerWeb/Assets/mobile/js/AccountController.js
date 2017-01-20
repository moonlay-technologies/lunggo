// User Account Controller

app.controller('siteHeaderController', [
    '$http', '$scope', '$log', function ($http, $scope, $log) {
        $scope.profileloaded = false;
        $scope.email = '';
        $scope.name = '';
        $scope.trial = 0;
        $scope.returnUrl = document.referrer;
        //$scope.authProfile = {}

        $scope.ProfileConfig = {
            getProfile: function () {
                if ($scope.trial > 3) {
                    $scope.trial = 0;
                }
                $scope.name = '';
                $http({
                    method: 'GET',
                    url: GetProfileConfig.Url,
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).then(function (returnData) {
                    if (returnData.data.status == "200") {
                        $scope.pageLoaded = true;
                        $scope.isLogin = true;
                        $log.debug('Success getting Profile');
                        $scope.email = returnData.data.email;
                        $scope.name = returnData.data.name;
                        $scope.profileloaded = true;
                        $log.debug(returnData);
                        $scope.name = returnData.data.name;
                        //$scope.lastName = returnData.last;
                    }
                    else {
                        $log.debug('There is an error');
                        $log.debug('Error : ' + returnData.data.error);
                        $scope.pageLoaded = true;
                        $scope.isLogin = false;
                        $scope.profileloaded = true;
                        $log.debug(returnData);
                    }
                }).catch(function (returnData) {
                    $scope.trial++;
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.ProfileConfig.getProfile();
                    }
                    else {
                        $log.debug('Failed to Login');
                        $scope.pageLoaded = true;
                        $scope.isLogin = false;
                        $scope.profileloaded = true;
                    }
                });
            }
        }
        //Set Authorization to get Profile
        $scope.authAccess = getAuthAccess();
        if ($scope.authAccess == 2) {
            $scope.ProfileConfig.getProfile();
        }
        else {
            $scope.isLogin = false;
            $scope.pageLoaded = true;
        }
        
        $scope.logout = {
            isLogout: false,
            getLogout: function () {
                eraseCookie('accesstoken');
                eraseCookie('refreshtoken');
                eraseCookie('authkey');
                $scope.isLogin = false;
                $scope.logout.isLogout =  true;
                window.location.href = "/";
            }
        }
    }]
);

app.controller('UserAccountController', ['$http', '$scope', '$rootScope', '$location', '$log',
    function ($http, $scope, $rootScope, $location, $log) {
        angular.element(document).ready(function () {
            $location.hash('menu');
        });

        $scope.PageConfig = $rootScope.PageConfig;
        $scope.Countries = $rootScope.Countries;
        $scope.trial = 0;
        $scope.pageLoaded = true;
        $scope.formMessage = '';
        $scope.NotifBox = false;
        $scope.getdata = function () {
            if ($scope.trial > 3) {
                $scope.trial = 0;
            }
            var authAccess = getAuthAccess();
            if (authAccess == 2) {
                $http({
                    method: 'GET',
                    url: GetProfileConfig.Url,
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).then(function (returnData) {
                    if (returnData.status == "200") {
                        $scope.pageLoaded = true;
                        $scope.isLogin = true;
                        $log.debug('Success getting Profile');
                        $scope.email = returnData.data.email;
                        $scope.profileloaded = true;
                        $log.debug(returnData);
                        $scope.UserProfile.name = returnData.data.name;
                        $scope.UserProfile.email = returnData.data.email;
                        $scope.UserProfile.phone = returnData.data.phone;
                        $scope.UserProfile.country = returnData.data.countryCallCd;
                    }
                    else {
                        $log.debug('There is an error');
                        $log.debug('Error : ' + returnData.data.error);
                        $scope.pageLoaded = true;
                        $scope.isLogin = false;
                        $scope.profileloaded = true;
                        $log.debug(returnData);
                    }
                }).catch(function (returnData) {
                    $scope.trial++;
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.getdata();
                    }
                    else
                    {
                        $log.debug('Failed to Login');
                        $scope.pageLoaded = true;
                        $scope.isLogin = false;
                        $scope.profileloaded = true;
                    }
                });
            }
            else
            {
                $log.debug('Not Authorized');
                $scope.pageLoaded = true;
                $scope.isLogin = false;
                $scope.profileloaded = true;
            }
        
        }
        $scope.getdata();
        $scope.loading = false;

        // change page
        $scope.PageConfig.ActivePage = 'menu';
        $scope.PageConfig.ActivePageChanged = false;
        $scope.PageConfig.ChangePage = function(page) {
            $scope.PageConfig.ActivePage = page;
            $scope.PageConfig.ActivePageChanged = true;
            $location.hash(page);
        }
    

        $scope.$watch('PageConfig.ActivePage', function() {
            $scope.NotifBox = false;
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
            if ($scope.trial > 3) {
                $scope.trial = 0;
            }
            // edit profile form
            if (name == 'profile') {
                $scope.UserProfile.edit = !($scope.UserProfile.edit);
                $scope.UserProfile.updated = false;
            }
            else if (name == 'profileSave') {
                if ($scope.EditProfile.name == null || $scope.EditProfile.name.length == 0) {
                    $scope.EditProfile.name = $scope.UserProfile.name;
                }
               
                if ($scope.EditProfile.phone == null || $scope.EditProfile.phone.length == 0) {
                    $scope.EditProfile.phone = $scope.UserProfile.phone;
                }

                if ($scope.EditProfile.country == null || $scope.EditProfile.country.length == 0) {
                    $scope.EditProfile.country = $scope.UserProfile.country;
                }
                $log.debug('submitting form');
                $scope.UserProfile.updating = true;
                var authAccess = getAuthAccess();
                if (authAccess == 2) {
                    // submit form to URL
                    $http({
                        url: ChangeProfileConfig.Url,
                        method: 'PATCH',
                        data: {
                            name: $scope.EditProfile.name,
                            phone: $scope.EditProfile.phone,
                            countryCallCd: $scope.EditProfile.country
                        },
                        headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                    }).then(function (returnData) {
                        $log.debug(returnData);
                        if (returnData.data.status == '200') {
                            $log.debug('Success requesting change profile');
                            $scope.formMessage = 'Profil Anda Telah Berhasil Diperbaharui';
                            $scope.UserProfile.name = $scope.EditProfile.name;

                            $scope.UserProfile.phone = parseInt($scope.EditProfile.phone);
                            $scope.UserProfile.country = $scope.EditProfile.country;
                            $scope.UserProfile.edit = false;
                            $scope.UserProfile.updating = false;
                            $scope.UserProfile.updated = true;
                           
                            $scope.NotifBox = true;
                        }
                        else {
                            $log.debug(returnData.data.error);
                            $scope.UserProfile.edit = true;
                            $scope.UserProfile.updating = false;
                            $scope.formMessage = 'Profil Anda Belum Diperbaharui';
                            $scope.NotifBox = true;
                        }
                    }).catch(function (returnData) {
                        $scope.trial++;
                        if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                        {
                            $scope.EditProfile.Submit(name);
                        }
                        else
                        {
                            $log.debug('Failed requesting change profile');
                            $scope.profileForm.edit = true;
                            $scope.UserProfile.updating = false;
                            $scope.formMessage = 'Profile Anda Belum Diperbaharui';
                            $scope.NotifBox = true;
                        }
                    });
                }
                else
                {
                    $log.debug('Not Authorized');
                    $scope.profileForm.edit = true;
                    $scope.UserProfile.updating = false;
                    $scope.formMessage = 'Profile Anda Belum Diperbaharui';
                    $scope.NotifBox = true;
                }
            
            }
            if (name == 'password') {
                $scope.password.edit = !($scope.password.edit);
                $scope.password.failed = false;
            }
            else if (name == 'passwordSave') {

                $scope.password.updating = true;
                $scope.password.failed = false;

                $log.debug('submitting form');
                var authAccess = getAuthAccess();
                if (authAccess == 2) {
                    // submit form to URL
                    $http({
                        url: ChangePasswordConfig.Url,
                        method: 'POST',
                        data: {
                            newPassword: $scope.passwordForm.newPassword,
                            oldPassword: $scope.passwordForm.currentPassword
                        },
                        headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                    }).then(function (returnData) {
                        $scope.passwordForm.newPassword = '';
                        $scope.passwordForm.currentPassword = '';
                        $scope.passwordForm.confirmationPassword = '';
                        if (returnData.data.status == '200') {
                            $log.debug('Success requesting reset password');
                            $log.debug(returnData);
                            $scope.password.edit = false;
                            $scope.password.updating = false;
                        }
                        else {
                            $log.debug(returnData.data.error);
                            $log.debug(returnData);
                            $scope.password.updating = false;
                            $scope.password.failed = true;
                        }
                    }).catch(function (returnData) {
                        $scope.trial++;
                        if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                        {
                            $scope.EditProfile.Submit(name);
                        }
                        else {
                            $log.debug('Failed requesting reset password');
                            $scope.password.edit = true;
                            $scope.password.updating = false;
                        }
                    });
                }
                else {
                    $log.debug('Not Authorized');
                    $scope.password.edit = true;
                    $scope.password.updating = false;
                }            
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
                if ($scope.trial > 3) {
                    $scope.trial = 0;
                }
                $scope.UserPassword.Updating = true;
                var authAccess = getAuthAccess();
                if (authAccess == 2) {
                    $http({
                        url: ChangePasswordConfig.Url,
                        method: 'POST',
                        data: {
                            newPassword: $scope.UserPassword.NewPassword,
                            oldPassword: $scope.UserPassword.CurrentPassword
                        },
                        headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                    }).then(function (returnData) {
                        $scope.UserPassword.NewPassword = '';
                        $scope.UserPassword.CurrentPassword = '';
                        $scope.UserPassword.ConfirmPassword = '';
                        if (returnData.data.status == '200') {
                            $log.debug('Success requesting reset password');
                            $log.debug(returnData);
                            $scope.UserPassword.Updating = false;
                            $scope.UserPassword.Editing = false;
                            $scope.formMessage = 'Password Anda Telah Berhasil Diperbaharui';
                            $scope.NotifBox = true;
                        }
                        else {
                            $log.debug(returnData);
                            $scope.UserPassword.Updating = false;
                            $scope.UserPassword.Editing = false;
                            $scope.formMessage = 'Password Lama yang Anda Masukkan Tidak Tepat';
                            $scope.NotifBox = true;

                        }
                    }).catch(function () {
                        $scope.trial++;
                        if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                        {
                            $scope.UserPassword.Submit();
                        }
                        else {
                            $log.debug('Failed requesting reset password');
                            $scope.UserPassword.Editing = false;
                            $scope.UserPassword.Updating = false;
                            $scope.formMessage = 'Permohonan ganti password Anda gagal';
                            $scope.NotifBox = true;
                        }
                    });
                }
                else {
                    $log.debug('Not Authorized');
                    $scope.UserPassword.Editing = false;
                    $scope.UserPassword.Updating = false;
                    $scope.formMessage = 'Permohonan ganti password Anda gagal';
                    $scope.NotifBox = true;
                }        
            }
        };

        //Get Transaction History

        // order status
        $scope.OrderStatus = function (num) {
            var text = '';
            switch (num) {
                case 1:
                    text = 'Dibatalkan';
                    break;
                case 2:
                    text = 'Menunggu Pembayaran';
                    break;
                case 3:
                    text = 'Lunas';
                    break;
                case 4:
                    text = 'Ditolak';
                    break;
                case 5:
                    text = 'Kadaluarsa';
                    break;
                case 6:
                    text = 'Memverifikasi Pembayaran';
                    break;
                case 7:
                    text = 'Menunggu Konfirmasi Ulang';
                    break;
            }
            return text;
        }

        // flight type
        $scope.FlightType = function (num) {
            var text = '';
            switch (num) {
                case 1:
                    text = '(Sejalan)';
                    break;
                case 2:
                    text = '(Pulang Pergi)';
                    break;
                default:
                    text = '';
                    break;
            }
            return text;
        }

        $scope.totalpax = function (hotel) {
            var sumAdult = 0;
            var sumChild = 0;

            for (var i = 0; i < hotel.room.length; i++) {
                for (var j = 0; j < hotel.room[i].rates.length; j++) {
                    for (var k = 0; k < hotel.room[i].rates[j].breakdowns.length; k++) {
                        sumAdult += hotel.room[i].rates[j].breakdowns[k].adultCount;
                        sumChild += hotel.room[i].rates[j].breakdowns[k].childCount;
                    }

                }
            }

            return { 'totalAdult': sumAdult, 'totalChild': sumChild };
        }

        $scope.totalRoom = function (hotel) {
            var rooms = [];

            for (var i = 0; i < hotel.room.length; i++) {
                var totalroom = 0;
                for (var j = 0; j < hotel.room[i].rates.length; j++) {
                    for (var k = 0; k < hotel.room[i].rates[j].breakdowns.length; k++) {
                        totalroom += hotel.room[i].rates[j].breakdowns[k].rateCount;
                    }
                }
                var room = {
                    'roomName': hotel.room[i].roomName,
                    'roomCount': totalroom
                };
                rooms.push(room);
            }

            var roomstc = '';
            for (var m = 0; m < rooms.length; m++) {
                roomstc += parseInt(rooms[m].roomCount) + ' ' + rooms[m].roomName + ', ';
            }
            roomstc = roomstc.substring(0, roomstc.length - 2);
            return roomstc;
        }

        $scope.sumRoom = function (hotel) {
            var sumRoom = 0;
            for (var i = 0; i < hotel.room.length; i++) {
                for (var j = 0; j < hotel.room[i].rates.length; j++) {
                    for (var k = 0; k < hotel.room[i].rates[j].breakdowns.length; k++) {
                        sumRoom += hotel.room[i].rates[j].breakdowns[k].rateCount;
                    }

                }
            }
            return sumRoom;
        }
        $scope.nightcount = function (hotel) {
            var checkin = new Date(hotel.checkIn);
            var checkout = new Date(hotel.checkOut);
            return (checkout - checkin) / (24 * 3600 * 1000);
        }

        $scope.titleCase = function (sentence) {
            return sentence.replace(/\w\S*/g, function (txt) { return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase(); });
        }

        $scope.submitRsvNo = function (rsvNo, rsvStatus) {
            $('form#rsvno input#rsvno-input').val(rsvNo);
            $('form#rsvno input#message-input').val(rsvStatus);
            $('form#rsvno').submit();
        }

        $scope.trxHistory = {
            getTrxHistory: function () {
                if ($scope.trial > 3) {
                    $scope.trial = 0;
                }
                //Check Authorization
                var authAccess = getAuthAccess();
                if (authAccess == 2) {
                    $scope.loading = true;
                    $http({
                        method: 'GET',
                        url: TrxHistoryConfig.Url,
                        headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                    }).then(function (returnData) {
                        if (returnData.data.status == "200") {
                            $scope.loading = false;
                            $log.debug('Success getting Transaction');
                            $log.debug(returnData);
                            $scope.flightlist = returnData.data.flights;
                            $scope.hotellist = returnData.data.hotels;
                            var flights = [];

                            for (var i = 0; i < $scope.flightlist.length; i++) {
                                if ($scope.flightlist[i] !== null) {
                                    flights.push($scope.flightlist[i]);
                                }
                            }
                            $scope.flightlist = flights;
                            $log.debug($scope.flightlist);
                        }
                        else {
                            $log.debug('There is an error');
                            $log.debug('Error : ' + returnData.data.error);
                            $log.debug(returnData);
                        }
                    }).catch(function (returnData) {
                        $scope.trial++;
                        if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                        {
                            $scope.trxHistory.getTrxHistory();
                        }
                        else {
                            $log.debug('Failed to Get Transaction History');
                        }
                    });
                }
                else {
                    $log.debug('Not Authorized');
                }
            
            }
        }

        $scope.trxHistory.getTrxHistory();
    
        $scope.returnUrl = "/";
        $scope.logout = {
            isLogout: false,
            getLogout: function () {
                eraseCookie('accesstoken');
                eraseCookie('refreshtoken');
                eraseCookie('authkey');
                //$scope.isLogin = false;
                $scope.logout.isLogout = true;
                window.location.href = "/";
            }
        }
}]);// User Account Controller

//// Contact Controller
//app.controller('ContactController', ['$http', '$scope', '$rootScope', function ($http, $scope, $rootScope) {

//    $scope.PageConfig = $rootScope.PageConfig;

//}]);// Contact Controller

// Order Detail Controller
app.controller('OrderDetailController', ['$http', '$scope', '$rootScope', '$log', function ($http, $scope, $rootScope, $log) {

    $scope.pageLoaded = true;
    $scope.PageConfig = $rootScope.PageConfig;
    $scope.rsvNo = rsvNo;

    // Pass Data into MVC Controller
    $scope.submitRsvNo = function (rsvNo, rsvStatus) {
        $('form#rsvno input#rsvno-input').val(rsvNo);
        $('form#rsvno input#message-input').val(rsvStatus);
        $('form#rsvno').submit();
    }

    $scope.rsvStatus = rsvStatus;
    $scope.paymentMethod = paymentMethod;
    $scope.ButtonText = function (status, method, type) {
        if (status == 1) { return 'Halaman Pembayaran'; }

        else if (status == 2 || status == 3) {
            if (method == 2 || method == 5) {
                return 'Instruksi Pembayaran';
            }
            else if (method == 4) {
                return 'Halaman Pembayaran';
            }
            else {
                return 'Lihat Detail';
            }
        }

        else if (status == 7 || status == 9 || status == 10) {
            return 'Lihat Detail';
        }

        else if (status == 4 || status == 5) {
            if (type == 'flight') {
                return 'Cetak E-tiket';
            } else {
                return 'Cetak Voucher';
            }
        }

        else if (status == 6 || status == 8) { return 'Cari Penerbangan'; }

    }

    $scope.closeOverlay = function() {
        window.location.href = "/";
    }

    $scope.TakeProfileConfig = {
        TakeProfile: function () {
            if ($scope.trial > 3) {
                $scope.trial = 0;
            }
            //Check Authorization
            var authAccess = getAuthAccess();
            if (authAccess == 2) {
                $http({
                    method: 'GET',
                    url: GetProfileConfig.Url,
                    async: false,
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).then(function (returnData) {
                    if (returnData.data.status == "200") {
                        $log.debug('Success getting Profile');
                        $scope.userProfile =
                            {
                                email: returnData.data.email,
                                name: returnData.data.name,
                                countryCallCd: returnData.data.countryCallCd,
                                phone: returnData.data.phone
                            };
                        $scope.isLogin = true;
                    }
                    else {
                        $log.debug('There is an error');
                        $log.debug('Error : ' + returnData.data.error);
                        $log.debug(returnData);
                    }
                }).catch(function (returnData) {
                    $scope.trial++;
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.TakeProfileConfig.TakeProfile();
                    }
                    else {
                        $log.debug('Failed to Get Profile');
                    }
                });
            }
            else {
                $log.debug('Not Authorized to get the profile');
            }

        }
    }
    
    $scope.TakeProfileConfig.TakeProfile();
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
app.controller('LoginController', ['$http', '$scope', '$rootScope', '$log', function ($http, $scope, $rootScope, $log) {

    $scope.PageConfig = $rootScope.PageConfig;
    $scope.trial = 0;
    $scope.pageLoaded = true;
    $scope.overlay = false;
    $scope.returnUrl = document.referrer;

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
        Send: function () {
            if ($scope.trial > 3) {
                $scope.trial = 0;
            }
            $scope.User.Sending = true;
            var authAccess = getAuthAccess();
            if (authAccess == 2 || authAccess == 1)
            {
                $http({
                    method: 'POST',
                    url: LoginConfig.Url,
                    data:{
                        userName: $scope.User.Email,
                        password: $scope.User.Password,
                        clientId: 'WWxoa2VrOXFSWFZOUXpSM1QycEpORTB5U1RWT1IxcHNXVlJOTTFsWFZYaE5hbVJwVFVSSk5FOUVTbWxOUkVVMFRrUlNhVmxxVlhwT01sbDNUbXBvYkUxNlJUMD0=',
                        clientSecret: 'VFVSTk1sbHFiR3hhVjAweFdXMUdhbHBYVVhwYVIxRXpUWHBCTlUxRVRtdGFhbHBvV1ZSU2FFMUhSbXhOUkdob1dtcEpkMDVSUFQwPQ=='
                    },
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).then(function (returnData) {
                    $scope.User.Resubmitting = false;
                    if (returnData.data.status == '200') {
                        setCookie("accesstoken", returnData.data.accessToken, returnData.data.expTime);
                        setCookie("refreshtoken", returnData.data.refreshToken, returnData.data.expTime);
                        setCookie("authkey", returnData.data.accessToken, returnData.data.expTime);
                        $scope.User.Email = '';
                        $scope.User.Resubmitted = true;
                        window.location.href = "/";
                    }
                    else {
                        $scope.overlay = true;
                        if (returnData.data.error == 'ERALOG01') {
                            $scope.User.Message = 'RefreshNeeded';
                        }
                        else if (returnData.data.error == 'ERALOG02') {
                                $scope.User.Message = 'InvalidInputData';
                        } //
                        else if (returnData.data.error == 'ERALOG03') {
                            $scope.User.Message = 'AlreadyRegisteredButUnconfirmed';
                            $scope.User.Resubmitted = false;
                        } //
                        else if (returnData.data.error == 'ERALOG04') {
                                $scope.User.Message = 'Failed';
                        } //
                        else if (returnData.data.error == 'ERALOG05') {
                            $scope.User.Message = 'NotRegistered';
                        } //
                        else {
                                $scope.errorMessage = 'Failed';
                        }
                        $log.debug('Error : ' + returnData.data.error);
                        $scope.User.Sending = false;
                    }
                }).catch(function () {
                    $scope.trial++;
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.User.Send();
                    }
                    else {
                        $log.debug('Failed requesting confirmation e-mail');
                        $scope.User.Message = 'Failed';
                    }
                });
            }
            else
            {
                $scope.User.Message = 'Failed';
            }           
        },
        Reconfirm: function () {
            if ($scope.trial > 3) {
                $scope.trial = 0;
            }
            $scope.User.Resubmitting = true;
            var authAccess = getAuthAccess();
            if (authAccess == 1 || authAccess == 2)
            {
                $http({
                    url: ResendConfirmationEmailConfig.Url,
                    method: 'POST',
                    data: {
                        email: $scope.User.Email,
                    },
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).then(function (returnData) {
                    $scope.User.Resubmitting = false;

                    if (returnData.data.status == "200") {
                        $scope.User.Email = '';
                        $scope.User.Password = '';
                        $scope.User.Resubmitted = true;
                    }

                }).catch(function () {
                    $scope.trial++;
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.User.Reconfirm();
                    }
                    else {
                        $log.debug('Failed requesting confirmation e-mail');
                        $scope.submitting = false;
                        $scope.submitted = false;
                    }
                });
            }
            else
            {
                $log.debug('Not Authorized');
                $scope.submitting = false;
                $scope.submitted = false;
            }
                
        }
    }
}]);// Login Controller

// Register Controller
app.controller('RegisterController', ['$http', '$scope', '$rootScope', '$log', function($http, $scope, $rootScope, $log) {

    $scope.PageConfig = $rootScope.PageConfig;
    $scope.pageLoaded = true;
    $scope.trial = 0;
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
            if ($scope.trial > 3) {
                $scope.trial = 0;
            }
            $scope.User.Resubmitting = true;
            $http({
                url: ResendConfirmationEmailConfig.Url,
                method: 'POST',
                data: {
                    email: $scope.User.Email,
                },
                headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
            }).then(function (returnData) {
                $scope.User.Resubmitting = false;
                

                if (returnData.data.status == "200") {
                    $scope.User.Email = '';
                    $scope.User.Resubmitted = true;
                }

            }).catch(function () {
                $scope.trial++;
                if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                {
                    $scope.User.Reconfirm();
                }
                else {
                    $log.debug('Failed requesting confirmation e-mail');
                    $scope.submitting = false;
                    $scope.submitted = false;
                }
                
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
        Submit: function () {
            if ($scope.trial > 3) {
                $scope.trial = 0;
            }
            $scope.User.Sending = true;
            var authAccess = getAuthAccess();
            if (authAccess == 1 || authAccess == 2)
            {
                $http({
                    url: RegisterConfig.Url,
                    method: 'POST',
                    data: {
                        email: $scope.User.Email,
                    },
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).then(function (returnData) {
                    $scope.User.Sending = false;
                    $scope.User.Sent = true;
                    if (returnData.data.status == '200') {
                        $scope.User.Registered = false;
                        $scope.User.EmailSent = false;
                        $scope.User.EmailConfirmed = false;
                        $scope.User.Email = '';
                    }
                    else {
                        switch (returnData.data.error) {
                            case "ERAREG02":
                                $scope.User.Registered = true;
                                $scope.User.EmailSent = false;
                                $scope.User.EmailConfirmed = true;
                                $scope.User.Email = '';
                                break;
                            case "ERAREG03":
                                $scope.User.Registered = true;
                                $scope.User.EmailSent = true;
                                $scope.User.EmailConfirmed = false;
                                break;
                            case "ERRGEN99":
                                $scope.User.Error = true;
                                break;
                            case "ERAREG01":
                                $scope.User.Invalid = true;
                                break;
                        }
                    }
                    if ($scope.Subscribe) {
                        $scope.User.Newsletter = true;
                    } else {
                        $scope.User.Newsletter = false;
                    }

                }).catch(function () {
                    $scope.trial++;
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.User.Submit();
                    }
                    else {
                        $log.debug('Failed');
                        $scope.User.Sending = false;
                        $scope.User.Sent = false;
                    }
                    
                });
            }
            else
            {
                $scope.form.submitting = false;
                $scope.form.submitted = false;
            }          
        },
        ReconfirmSent: function() {
            $scope.User.Resubmitted = false;
        }
    }

    $scope.Subscribe = function () {
        var temp = false;
        if ($scope.trial > 3) {
            $scope.trial = 0;
        }
        $http({
            url: SubscribeConfig.Url,
            method: 'POST',
            data: {
                'address': $scope.User.Email,
            },
            headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
        }).then(function (returnData) {
            $scope.User.showBox = true;
            $log.debug('Succeed');
            $scope.User.Sending = false;
            if (returnData == true) {
                temp = true;
            } else {
                temp = false;
            }

        }).catch(function () {
            $scope.trial++;
            if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
            {
                $scope.Subscribe();
            }
            else {
                $scope.User.showBox = true;
                $scope.User.Sending = false;
                $scope.User.Sent = false;
                temp = false;
            }
            
        });

        return temp;
    }
}]);// Register Controller

// Forgot Password Controller
app.controller('ForgotpasswordController', ['$http', '$scope', '$rootScope', '$log', function ($http, $scope, $rootScope, $log) {

    $scope.PageConfig = $rootScope.PageConfig;
    $scope.pageLoaded = true;
    $scope.trial = 0;
    $scope.overlay = false;
    $scope.closeOverlay = function() {
        $scope.overlay = false;
        $scope.EmailForm.Sending = false;
        $scope.EmailForm.Sent = false;
    }
    $scope.emailReconfirm = "";
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
            if ($scope.trial > 3) {
                $scope.trial = 0;
            }
            $scope.EmailForm.Sending = true;
            // send form
            // submit form to URL
            var authAccess = getAuthAccess();
            if (authAccess == 1 || authAccess == 2) {
                $scope.getFlightHeader = 'Bearer ' + getCookie('accesstoken');
                $scope.emailReconfirm = $scope.EmailForm.Email;
                //$log.debug($scope.emailReconfirm);
                $http({
                    url: ForgotPasswordConfig.Url,
                    method: 'POST',
                    data: {
                        userName: $scope.EmailForm.Email
                    },
                    headers: { 'Authorization': $scope.getFlightHeader }
                }).then(function (returnData) {
                    $scope.EmailForm.Sending = false;
                    $scope.EmailForm.Sent = true;
                    $log.debug(returnData);
                    if (returnData.data.status == '200') {
                        $scope.EmailForm.ReturnData.Found = true;
                        $scope.EmailForm.ReturnData.EmailConfirmed = true;
                    }
                    else {
                        switch (returnData.data.error) {
                            case "ERAFPW01":
                                $scope.EmailForm.ReturnData.Found = false;
                                $scope.EmailForm.ReturnData.EmailConfirmed = false;
                                $scope.EmailForm.Sending = false;
                                break;
                            case "ERAFPW02":
                                $scope.EmailForm.ReturnData.Found = false;
                                $scope.EmailForm.ReturnData.EmailConfirmed = true;
                                $scope.EmailForm.Sending = false;
                                break;
                            case "ERAFPW03":
                                $scope.EmailForm.ReturnData.Found = true;
                                $scope.EmailForm.ReturnData.EmailConfirmed = false;
                               
                                $scope.EmailForm.Sending = false;
                                break;
                            case "ERRGEN99":
                                $scope.EmailForm.ReturnData.Error = true;
                                break;
                        }
                    }

                }).catch(function (returnData) {
                    $scope.trial++;
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.EmailForm.SendForm();
                    }
                    else {
                        $log.debug('Failed requesting reset password');
                        $scope.EmailForm.Sending = false;
                        $scope.EmailForm.ReturnData.Error = true;
                    }
                    
                });
            }
            else {
                $scope.EmailForm.Sending = false;
                $scope.EmailForm.ReturnData.Error = true;
            }
            
        },
        Reconfirm: function () {
            if ($scope.trial > 3) {
                $scope.trial = 0;
            }
                $scope.EmailForm.Resubmit = true;
                var authAccess = getAuthAccess();
                //$log.debug("at reconfirm: " + $scope.emailReconfirm);
                if (authAccess == 1 || authAccess == 2)
                {
                    $http({
                        url: ResendConfirmationEmailConfig.Url,
                        method: 'POST',
                        data: {
                            email: $scope.emailReconfirm,
                        },
                        headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                    }).then(function (returnData) {
                        $scope.EmailForm.Resubmit = false;
                        $scope.EmailForm.SentReconfirm = true;

                        if (returnData.data.status == "200") {
                            $scope.EmailForm.Email = '';
                        }

                    }).catch(function () {
                        $scope.trial++;
                        if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                        {
                            $scope.EmailForm.Reconfirm();
                        }
                        else {
                            $log.debug('Failed requesting reset password');
                            $scope.EmailForm.Resubmit = false;
                            $scope.EmailForm.SentReconfirm = false;
                        }

                    });
                }
                else
                {
                    $log.debug('Not Authorized');
                    $scope.EmailForm.Resubmit = false;
                    $scope.EmailForm.SentReconfirm = false;
                }
                
            
        }
    };
}]);// ForgotPasswordController

app.controller('ResetpasswordController', ['$http', '$scope', '$rootScope', '$log', function ($http, $scope, $rootScope, $log) {

        $scope.PageConfig = $rootScope.PageConfig;
        $scope.pageLoaded = true;
        $scope.trial = 0;
        $scope.form = {
            submitted: false,
            submitting: false,
            succeed: false,
            userEmail: userEmail,
            code: code,
            password: '',
            start: true,
            resubmitted: false,
            resubmitting: false,
            reconfirm: false
        };
        $('.text').keyup(updateCount);
        $('.text').keydown(updateCount);

        function updateCount() {
            var cs = $(this).val().length;
            if (cs < 6) {
                $('#characters').text('Panjang password kurang dari 6 karakter');
            } else {
                $('#characters').text('');
            }
        }
        $scope.errorMessage = '',
        $scope.errorLog = '',
        $scope.submit = function () {
            if ($scope.trial > 3) {
                $scope.trial = 0;
            }
            $scope.form.submitting = true;
            var authAccess = getAuthAccess();
            if (authAccess == 2 || authAccess == 1)
            {
                $http({
                    url: ResetPasswordConfig.Url,
                    method: 'POST',
                    data: {
                        Username: $scope.form.userEmail,
                        Password: $scope.form.password,
                        Code: $scope.form.code
                    },
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).then(function (returnData) {
                    if (returnData.data.status == '200') {
                        $log.debug('Success requesting reset password');
                        $log.debug(returnData);
                        $scope.form.submitting = false;
                        $scope.form.submitted = true;
                        $scope.form.succeed = true;
                        $scope.form.start = false;
                    }
                    else {
                        switch (returnData.data.error) {
                            case 'ERARST01':
                                $scope.errorMessage = 'Pastikan Anda telah mendaftar di travorama untuk melakukan reset password ini';
                                break;
                            case 'ERARST02':
                                $scope.errorLog = 'User Not Found';
                                $scope.errorMessage = 'User ini tidak ditemukan';
                                break;
                            case 'ERARST03':
                                $scope.errorLog = 'Failed to reset password';
                                $scope.errorMessage = 'Password Anda tidak berhasil direset';
                                break;
                            case 'ERRGEN98':
                                $scope.errorLog = 'Invalid JSON Format';
                                $scope.errorMessage = 'Password Anda tidak berhasil direset';
                                break;
                            case 'ERRGEN99':
                                $scope.errorLog = 'There is a problem on the server';
                                $scope.errorMessage = 'Pastikan Anda telah mendaftar di travorama untuk melakukan reset password ini';
                                break;
                            default:
                                $scope.errorMessage = 'Password Anda tidak berhasil direset';
                        }
                        $log.debug(returnData.data.Description);
                        $log.debug(returnData);
                        $scope.form.succeed = false;
                        $scope.form.start = false;
                        $scope.form.submitted = true;
                        $scope.form.submitting = false;
                    }
                }).catch(function (returnData) {
                    $scope.trial++;
                    $scope.errorMessage = 'Mohon maaf. Password Anda tidak berhasil direset';
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.submit();
                    }
                    else {
                        $log.debug('Failed requesting reset password');
                        $scope.form.submitting = false;
                        $scope.form.lala = true;
                        $scope.abc = true;
                    }
                });
            }

            else
            {
                $scope.form.submitting = false;
                $scope.form.lala = true;
            }
            
        }
        $scope.reconfirm = function () {
            if ($scope.trial > 3) {
                $scope.trial = 0;
            }
            $scope.form.resubmitting = true;
            var authAccess = getAuthAccess();
            if (authAccess == 1 || authAccess == 2)
            {
                $http({
                    url: ResendConfirmationEmailConfig.Url,
                    method: 'POST',
                    data: {
                        email: $scope.form.userEmail,
                    },
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).then(function (returnData) {
                    $scope.form.resubmitting = false;
                    if (returnData.data.status == 200) {
                        $scope.form.resubmitted = true;
                    } else {
                        $scope.form.reconfirm = true;
                    }
                    

                }).catch(function (returnData) {
                    $scope.trial++;
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.EmailForm.Reconfirm();
                    }
                    else {
                        $log.debug('Failed requesting confirmation email');
                        $scope.form.resubmitting = false;
                        $scope.form.resubmitted = false;
                    }

                });
            }
            else
            {
                $log.debug('Not Authorized');
                $scope.EmailForm.Resubmit = false;
                $scope.EmailForm.SentReconfirm = false;
            }
                
            
        }
    }
]);

