app.controller('b2bAuthController', [
    '$scope', '$log', '$http', '$location', '$resource', '$timeout', function ($scope, $log, $http, $location, $resource, $timeout){
        $scope.pageLoaded = true;
        $scope.trial = 0;
        $scope.errorMessage = '';
        
        $scope.form = {
            email: '',
            password: '',
            submitting: false,
            isLogin: false,
            submitted: false,
            success: false
        };
        $scope.loginFailed = false;
        $scope.form.submit = function () {
            if ($scope.trial > 3) {
                $scope.trial = 0;
            }
            $scope.loginFailed = false;
            $scope.errorMessage = '';
            $scope.form.submitting = true;
            var authAccess = getAuthAccess();
            if (authAccess == 2 || authAccess == 1) {
                $http({
                    method: 'POST',
                    url: LoginConfig.Url,
                    data: {
                        userName: $scope.form.email,
                        password: $scope.form.password,
                        clientId: 'V2toa2VrOXFSWFZOUXpSM1QycEZlRTlIVlhwYWFrVjVUVVJrYlZsVVp6Vk5WRlp0VGtSR2FrOUhSWGhhYWsweFRucGpNRTE2U1RCT2VtTjNXbTFKZDFwcVFUMD0=',
                        clientSecret: 'V2tkS2FFOUVhek5QUjFsNFRucFpNVmt5UlRST2JVWnNXVmRKTTA1dFVtaFBSMDVyV1dwQk5WcEhTWGxPZWtwcVRVUkpNVTFCUFQwPQ=='
                    },
                    headers: {
                        'Authorization': 'Bearer ' + getCookie('accesstoken')
                    }
                }).then(function (returnData) {
                    $scope.form.submitting = false;
                    $scope.form.submitted = true;
                    $scope.loginFailed = false;
                    if (returnData.data.status == '200') {
                        setCookie("accesstoken", returnData.data.accessToken, returnData.data.expTime);
                        setCookie("refreshtoken", returnData.data.refreshToken, returnData.data.expTime);
                        setCookie("authkey", returnData.data.accessToken, returnData.data.expTime);
                        $scope.form.success = true;
                        window.location.href = "/id/B2BTemplate/index";
                    }
                    else {
                        $scope.loginFailed = true;
                        if (returnData.data.error == 'ERALOG01') {
                            $scope.errorMessage = 'RefreshNeeded';
                            $scope.errorMessage = 'Login Gagal';
                        }
                        else if (returnData.data.error == 'ERALOG02') {
                            $scope.errorMessage = 'E-mail/Password Tidak Tepat';
                        }
                        else if (returnData.data.error == 'ERALOG03') {
                            $scope.errorMessage = 'E-mail sudah terdaftar tapi belum terkonfirmasi';
                        }
                        else if (returnData.data.error == 'ERALOG04') {
                            $scope.errorMessage = 'Failed';
                            $scope.errorMessage = 'Login Gagal';
                        }
                        else if (returnData.data.error == 'ERALOG05') {
                            $scope.errorMessage = 'E-mail Anda belum terdaftar';

                        }
                        else {
                            $scope.errorMessage = 'Failed';
                            $scope.errorMessage = 'Login Gagal';
                        }
                        $log.debug('Error : ' + returnData.data.error);
                    }
                }).catch(function (returnData) {
                    $scope.trial++;
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.form.submit();
                    }
                    else {
                        $scope.overlay = true;
                        $scope.errorMessage = 'Failed';
                        $scope.errorMessage = 'Login Gagal';
                        $log.debug('Failed to Login');
                        $scope.form.submitting = false;
                        $scope.form.submitted = false;
                        $scope.form.isLogin = false;
                    }
                });
            }
            else {
                $scope.form.submitting = false;
                $scope.form.submitted = false;
                $scope.form.isLogin = false;
            }

        }

    }
]);// auth controller end

// Travorama reset controller
app.controller('b2bForgotPasswordController', [
    '$http', '$scope', '$log', function ($http, $scope, $log) {

        $scope.pageLoaded = true;
        $scope.trial = 0;
        $scope.form = {
            submitted: false,
            submitting: false,
            email: '',
            found: false,
            registered: false,
            emailConfirmed: false
        };

        $('.email').click(function () {
            $(this).select();
        });

        $scope.form.submit = function () {
            $('.buttonSubmit').attr("disabled", true);
            if ($scope.trial > 3) {
                $scope.trial = 0;
            }
            $scope.form.submitting = true;
            $log.debug('submitting form');
            //Check Authorization
            var authAccess = getAuthAccess();
            if (authAccess == 1 || authAccess == 2) {
                $scope.getFlightHeader = 'Bearer ' + getCookie('accesstoken');
                // submit form to URL
                $http({
                    url: ForgotPasswordConfig.Url,
                    method: 'POST',
                    data: {
                        userName: $scope.form.email
                    },
                    headers: { 'Authorization': $scope.getFlightHeader }
                }).then(function (returnData) {
                    $('.buttonSubmit').attr("disabled", false);
                    $scope.form.submitting = false;
                    $scope.form.submitted = true;
                    if (returnData.data.status == '200') {
                        $scope.form.found = true;
                        $scope.form.emailConfirmed = true;
                        $('body .reset-password').hide();
                    }
                    else {
                        switch (returnData.data.error) {
                            case "ERAFPW01":
                                $scope.form.found = false;
                                break;
                            case "ERAFPW02":
                                $scope.form.found = false;
                                break;
                            case "ERAFPW03":
                                $scope.form.found = true;
                                $scope.form.emailConfirmed = false;
                                break;
                            case "ERRGEN99":
                                $scope.form.found = false;
                                break;
                        }
                    }

                }).catch(function () {
                    $('.buttonSubmit').attr("disabled", false);
                    $scope.trial++;
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.form.submit();
                    }
                    else {
                        $log.debug('Failed requesting reset password');
                        $scope.form.submitting = false;
                    }
                });
            }
            else {
                $('.buttonSubmit').attr("disabled", false);
                $scope.form.submitting = false;
            }
        }
    }
]);// reset controller end

app.controller('b2bResetPasswordController', [
    '$http', '$scope', '$log', function ($http, $scope, $log) {

        $scope.pageLoaded = true;
        $scope.trial = 0;
        $scope.form = {
            submitted: false,
            submitting: false,
            isSuccess: false,
            userEmail: userEmail,
            code: code,
            resubmitting: false,
            reconfirm: false,
        };
        $scope.form.submit = function () {
            if ($scope.trial > 3) {
                $scope.trial = 0;
            }
            $scope.form.submitting = true;
            //Check Authorization
            var authAccess = getAuthAccess();
            if (authAccess == 2 || authAccess == 1) {
                $http({
                    url: ResetPasswordConfig.Url,
                    method: 'POST',
                    data: {
                        Password: $scope.form.password,
                        UserName: $scope.form.userEmail,
                        Code: $scope.form.code
                    },
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).then(function (returnData) {
                    if (returnData.data.status == '200') {
                        $log.debug('Success requesting reset password');
                        //$log.debug(returnData);
                        $scope.form.submitting = false;
                        $scope.form.submitted = true;
                        $scope.form.isSuccess = true;
                    }
                    else {
                        $log.debug(returnData.data.Description);
                        switch (returnData.data.error) {
                            case 'ERARST01':
                                $scope.errorLog = 'Invalid Request';
                                $scope.errorMessage = 'Pastikan Anda telah mendaftar di travorama untuk melakukan reset password ini';
                                break;
                            case 'ERARST02':
                                $scope.errorLog = 'User Not Found';
                                $scope.errorMessage = 'User ini tidak ditemukan';
                                break;
                            case 'ERARST03':
                                $scope.errorLog = 'Failed to reset password';
                                $scope.errorMessage = 'Pastikan Anda telah mendaftar di travorama untuk melakukan reset password ini';
                                break;
                            case 'ERRGEN98':
                                $scope.errorLog = 'Invalid JSON Format';
                                $scope.errorMessage = 'Pastikan Anda telah mendaftar di travorama untuk melakukan reset password ini';
                                break;
                            case 'ERRGEN99':
                                $scope.errorLog = 'There is a problem on the server';
                                $scope.errorMessage = 'Pastikan Anda telah mendaftar di travorama untuk melakukan reset password ini';
                                break;
                        }

                        $log.debug($scope.errorLog);
                        $scope.form.submitting = false;
                        $scope.form.submitted = true;
                        $scope.form.isSuccess = false;
                    }
                }).catch(function (returnData) {
                    $scope.trial++;
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.form.submit();
                    }
                    else {
                        $log.debug('Failed requesting reset password');
                        $scope.form.submitting = false;
                        $scope.form.submitted = true;
                        $scope.form.isSuccess = false;
                    }
                });
            }
            else {
                $log.debug('Not Authorized');
                $scope.form.submitting = false;
                $scope.form.submitted = true;
                $scope.form.isSuccess = false;
            }

        }
    }
]);// reset controller end