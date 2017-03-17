app.controller('b2bAuthController', [
    '$scope', '$log', '$http', '$location', '$resource', '$timeout', function ($scope, $log, $http){
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

        //Get Transaction History

        //$scope.getTrxHistory = function () {
        //    if ($scope.trial > 3) {
        //        $scope.trial = 0;
        //    }
        //    var authAccess = getAuthAccess();
        //    $scope.loading = true;
        //    if (authAccess == 2) {
        //        $http({
        //            method: 'GET',
        //            url: BookerTrxHistoryConfig.Url,
        //            headers: {
        //                'Authorization': 'Bearer ' + getCookie('accesstoken')
        //            }
        //        }).then(function (returnData) {
        //            $scope.loading = false;
        //            if (returnData.data.status == "200") {
        //                $log.debug('Success getting Transaction');
        //                $scope.flightlist = returnData.data.flights;
        //                $scope.hotellist = returnData.data.hotels;
        //                $scope.signature = returnData.data.signature;
        //            } else {
        //                $log.debug('There is an error');
        //                $log.debug('Error : ' + returnData.data.error);
        //                $log.debug(returnData);
        //            }
        //        }).catch(function (returnData) {
        //            $scope.trial++;
        //            if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
        //            {
        //                $scope.trxHistory.getTrxHistory();
        //            } else {
        //                $log.debug('Failed to Get Transaction History');
        //            }
        //        });
        //    } else {
        //        $log.debug('Not Authorized');
        //    }
        //}

        //$scope.getTrxHistory();

        $scope.loginFailed = false;
        $scope.form.submit = function () {
            $('.wait').modal({
                backdrop: 'static',
                keyboard: false
            });
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
                    url: B2BLoginPathConfig.Url,
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
                    $('.wait').modal("hide");
                    $scope.form.submitting = false;
                    $scope.form.submitted = true;
                    $scope.loginFailed = false;
                    if (returnData.data.status == '200') {
                        $('.loginSucceed').modal({
                            backdrop: 'static',
                            keyboard: false
                        });
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

app.controller('b2BApproverController', [
    '$http', '$scope', '$timeout', '$log', function ($http, $scope, $timeout, $log) {

        var hash = (location.hash);
        // variables

        //General Variables
        $scope.pageLoaded = true;
        $scope.email = 'Test';
        $scope.trial = 0;
        $scope.loading = false;
        $scope.currentSection = '';
        $scope.profileForm = {
            active: false
        };
        $scope.passwordForm = {
            active: false
        };
        $scope.isConfirm = false;
        $scope.close = function () {
            $scope.isConfirm = false;
        }

        $scope.userProfile = {
            email: '',
            name: '',
            countryCallCd: '',
            phone: '',
            edit: false,
            updating: false
        }

        $scope.bookerInfo = {
            name: '',
            bookerTitle: '',
            bookerMessage: '',
            edit: false,
            updating: false
        }

        $scope.updateData = {
            rsvNo: "",
            status : "",
            message:""
        }

        $scope.infoOrder = function(index) {
            $scope.bookerInfo.name = $scope.orderList[index].bookerName;
            $scope.bookerInfo.bookerTitle = $scope.orderList[index].bookerMessageTitle;
            $scope.bookerInfo.bookerMessage = $scope.orderList[index].bookerMessageDescription;
            $('.info-order').modal('show');
        }

        $scope.rejectReservation = function (rsvNo, status) {
            $scope.updateData.rsvNo = rsvNo;
            $scope.updateData.status = status;
            $('.req-reject').modal('show');
        }



        $scope.bookingstatus = function (types) {
            if (types == 12)
                return 'Approved';
            else if (types == 13)
                return 'Pending';
            else if (types == 6)
                return 'Expired';
            else if (types == 11)
                return 'Rejected';
            else if (types == 8)
                return 'Cancelled';
            else if (types == 10)
                return 'Failed';
            else if (types == 0)
                return 'Pending';
        }

        $scope.password = {
            edit: false,
            updating: false,
            failed: false
        }

        $scope.countries = [{ "name": "Afghanistan", "dial_code": "93", "code": "AF" }, { "name": "Aland Islands", "dial_code": "358", "code": "AX" }, { "name": "Albania", "dial_code": "355", "code": "AL" }, { "name": "Algeria", "dial_code": "213", "code": "DZ" }, { "name": "AmericanSamoa", "dial_code": "1 684", "code": "AS" }, { "name": "Andorra", "dial_code": "376", "code": "AD" }, { "name": "Angola", "dial_code": "244", "code": "AO" }, { "name": "Anguilla", "dial_code": "1 264", "code": "AI" }, { "name": "Antarctica", "dial_code": "672", "code": "AQ" }, { "name": "Antigua and Barbuda", "dial_code": "1268", "code": "AG" }, { "name": "Argentina", "dial_code": "54", "code": "AR" }, { "name": "Armenia", "dial_code": "374", "code": "AM" }, { "name": "Aruba", "dial_code": "297", "code": "AW" }, { "name": "Australia", "dial_code": "61", "code": "AU" }, { "name": "Austria", "dial_code": "43", "code": "AT" }, { "name": "Azerbaijan", "dial_code": "994", "code": "AZ" }, { "name": "Bahamas", "dial_code": "1 242", "code": "BS" }, { "name": "Bahrain", "dial_code": "973", "code": "BH" }, { "name": "Bangladesh", "dial_code": "880", "code": "BD" }, { "name": "Barbados", "dial_code": "1 246", "code": "BB" }, { "name": "Belarus", "dial_code": "375", "code": "BY" }, { "name": "Belgium", "dial_code": "32", "code": "BE" }, { "name": "Belize", "dial_code": "501", "code": "BZ" }, { "name": "Benin", "dial_code": "229", "code": "BJ" }, { "name": "Bermuda", "dial_code": "1 441", "code": "BM" }, { "name": "Bhutan", "dial_code": "975", "code": "BT" }, { "name": "Bolivia, Plurinational State of", "dial_code": "591", "code": "BO" }, { "name": "Bosnia and Herzegovina", "dial_code": "387", "code": "BA" }, { "name": "Botswana", "dial_code": "267", "code": "BW" }, { "name": "Brazil", "dial_code": "55", "code": "BR" }, { "name": "British Indian Ocean Territory", "dial_code": "246", "code": "IO" }, { "name": "Brunei Darussalam", "dial_code": "673", "code": "BN" }, { "name": "Bulgaria", "dial_code": "359", "code": "BG" }, { "name": "Burkina Faso", "dial_code": "226", "code": "BF" }, { "name": "Burundi", "dial_code": "257", "code": "BI" }, { "name": "Cambodia", "dial_code": "855", "code": "KH" }, { "name": "Cameroon", "dial_code": "237", "code": "CM" }, { "name": "Canada", "dial_code": "1", "code": "CA" }, { "name": "Cape Verde", "dial_code": "238", "code": "CV" }, { "name": "Cayman Islands", "dial_code": " 345", "code": "KY" }, { "name": "Central African Republic", "dial_code": "236", "code": "CF" }, { "name": "Chad", "dial_code": "235", "code": "TD" }, { "name": "Chile", "dial_code": "56", "code": "CL" }, { "name": "China", "dial_code": "86", "code": "CN" }, { "name": "Christmas Island", "dial_code": "61", "code": "CX" }, { "name": "Cocos (Keeling) Islands", "dial_code": "61", "code": "CC" }, { "name": "Colombia", "dial_code": "57", "code": "CO" }, { "name": "Comoros", "dial_code": "269", "code": "KM" }, { "name": "Congo", "dial_code": "242", "code": "CG" }, { "name": "Congo, The Democratic Republic of the Congo", "dial_code": "243", "code": "CD" }, { "name": "Cook Islands", "dial_code": "682", "code": "CK" }, { "name": "Costa Rica", "dial_code": "506", "code": "CR" }, { "name": "Cote d'Ivoire", "dial_code": "225", "code": "CI" }, { "name": "Croatia", "dial_code": "385", "code": "HR" }, { "name": "Cuba", "dial_code": "53", "code": "CU" }, { "name": "Cyprus", "dial_code": "357", "code": "CY" }, { "name": "Czech Republic", "dial_code": "420", "code": "CZ" }, { "name": "Denmark", "dial_code": "45", "code": "DK" }, { "name": "Djibouti", "dial_code": "253", "code": "DJ" }, { "name": "Dominica", "dial_code": "1 767", "code": "DM" }, { "name": "Dominican Republic", "dial_code": "1 849", "code": "DO" }, { "name": "Ecuador", "dial_code": "593", "code": "EC" }, { "name": "Egypt", "dial_code": "20", "code": "EG" }, { "name": "El Salvador", "dial_code": "503", "code": "SV" }, { "name": "Equatorial Guinea", "dial_code": "240", "code": "GQ" }, { "name": "Eritrea", "dial_code": "291", "code": "ER" }, { "name": "Estonia", "dial_code": "372", "code": "EE" }, { "name": "Ethiopia", "dial_code": "251", "code": "ET" }, { "name": "Falkland Islands (Malvinas)", "dial_code": "500", "code": "FK" }, { "name": "Faroe Islands", "dial_code": "298", "code": "FO" }, { "name": "Fiji", "dial_code": "679", "code": "FJ" }, { "name": "Finland", "dial_code": "358", "code": "FI" }, { "name": "France", "dial_code": "33", "code": "FR" }, { "name": "French Guiana", "dial_code": "594", "code": "GF" }, { "name": "French Polynesia", "dial_code": "689", "code": "PF" }, { "name": "Gabon", "dial_code": "241", "code": "GA" }, { "name": "Gambia", "dial_code": "220", "code": "GM" }, { "name": "Georgia", "dial_code": "995", "code": "GE" }, { "name": "Germany", "dial_code": "49", "code": "DE" }, { "name": "Ghana", "dial_code": "233", "code": "GH" }, { "name": "Gibraltar", "dial_code": "350", "code": "GI" }, { "name": "Greece", "dial_code": "30", "code": "GR" }, { "name": "Greenland", "dial_code": "299", "code": "GL" }, { "name": "Grenada", "dial_code": "1 473", "code": "GD" }, { "name": "Guadeloupe", "dial_code": "590", "code": "GP" }, { "name": "Guam", "dial_code": "1 671", "code": "GU" }, { "name": "Guatemala", "dial_code": "502", "code": "GT" }, { "name": "Guernsey", "dial_code": "44", "code": "GG" }, { "name": "Guinea", "dial_code": "224", "code": "GN" }, { "name": "Guinea-Bissau", "dial_code": "245", "code": "GW" }, { "name": "Guyana", "dial_code": "595", "code": "GY" }, { "name": "Haiti", "dial_code": "509", "code": "HT" }, { "name": "Holy See (Vatican City State)", "dial_code": "379", "code": "VA" }, { "name": "Honduras", "dial_code": "504", "code": "HN" }, { "name": "Hong Kong", "dial_code": "852", "code": "HK" }, { "name": "Hungary", "dial_code": "36", "code": "HU" }, { "name": "Iceland", "dial_code": "354", "code": "IS" }, { "name": "India", "dial_code": "91", "code": "IN" }, { "name": "Indonesia", "dial_code": "62", "code": "ID" }, { "name": "Iran, Islamic Republic of Persian Gulf", "dial_code": "98", "code": "IR" }, { "name": "Iraq", "dial_code": "964", "code": "IQ" }, { "name": "Ireland", "dial_code": "353", "code": "IE" }, { "name": "Isle of Man", "dial_code": "44", "code": "IM" }, { "name": "Israel", "dial_code": "972", "code": "IL" }, { "name": "Italy", "dial_code": "39", "code": "IT" }, { "name": "Jamaica", "dial_code": "1 876", "code": "JM" }, { "name": "Japan", "dial_code": "81", "code": "JP" }, { "name": "Jersey", "dial_code": "44", "code": "JE" }, { "name": "Jordan", "dial_code": "962", "code": "JO" }, { "name": "Kazakhstan", "dial_code": "7 7", "code": "KZ" }, { "name": "Kenya", "dial_code": "254", "code": "KE" }, { "name": "Kiribati", "dial_code": "686", "code": "KI" }, { "name": "Korea, Democratic People's Republic of Korea", "dial_code": "850", "code": "KP" }, { "name": "Korea, Republic of South Korea", "dial_code": "82", "code": "KR" }, { "name": "Kuwait", "dial_code": "965", "code": "KW" }, { "name": "Kyrgyzstan", "dial_code": "996", "code": "KG" }, { "name": "Laos", "dial_code": "856", "code": "LA" }, { "name": "Latvia", "dial_code": "371", "code": "LV" }, { "name": "Lebanon", "dial_code": "961", "code": "LB" }, { "name": "Lesotho", "dial_code": "266", "code": "LS" }, { "name": "Liberia", "dial_code": "231", "code": "LR" }, { "name": "Libyan Arab Jamahiriya", "dial_code": "218", "code": "LY" }, { "name": "Liechtenstein", "dial_code": "423", "code": "LI" }, { "name": "Lithuania", "dial_code": "370", "code": "LT" }, { "name": "Luxembourg", "dial_code": "352", "code": "LU" }, { "name": "Macao", "dial_code": "853", "code": "MO" }, { "name": "Macedonia", "dial_code": "389", "code": "MK" }, { "name": "Madagascar", "dial_code": "261", "code": "MG" }, { "name": "Malawi", "dial_code": "265", "code": "MW" }, { "name": "Malaysia", "dial_code": "60", "code": "MY" }, { "name": "Maldives", "dial_code": "960", "code": "MV" }, { "name": "Mali", "dial_code": "223", "code": "ML" }, { "name": "Malta", "dial_code": "356", "code": "MT" }, { "name": "Marshall Islands", "dial_code": "692", "code": "MH" }, { "name": "Martinique", "dial_code": "596", "code": "MQ" }, { "name": "Mauritania", "dial_code": "222", "code": "MR" }, { "name": "Mauritius", "dial_code": "230", "code": "MU" }, { "name": "Mayotte", "dial_code": "262", "code": "YT" }, { "name": "Mexico", "dial_code": "52", "code": "MX" }, { "name": "Micronesia, Federated States of Micronesia", "dial_code": "691", "code": "FM" }, { "name": "Moldova", "dial_code": "373", "code": "MD" }, { "name": "Monaco", "dial_code": "377", "code": "MC" }, { "name": "Mongolia", "dial_code": "976", "code": "MN" }, { "name": "Montenegro", "dial_code": "382", "code": "ME" }, { "name": "Montserrat", "dial_code": "1664", "code": "MS" }, { "name": "Morocco", "dial_code": "212", "code": "MA" }, { "name": "Mozambique", "dial_code": "258", "code": "MZ" }, { "name": "Myanmar", "dial_code": "95", "code": "MM" }, { "name": "Namibia", "dial_code": "264", "code": "NA" }, { "name": "Nauru", "dial_code": "674", "code": "NR" }, { "name": "Nepal", "dial_code": "977", "code": "NP" }, { "name": "Netherlands", "dial_code": "31", "code": "NL" }, { "name": "Netherlands Antilles", "dial_code": "599", "code": "AN" }, { "name": "New Caledonia", "dial_code": "687", "code": "NC" }, { "name": "New Zealand", "dial_code": "64", "code": "NZ" }, { "name": "Nicaragua", "dial_code": "505", "code": "NI" }, { "name": "Niger", "dial_code": "227", "code": "NE" }, { "name": "Nigeria", "dial_code": "234", "code": "NG" }, { "name": "Niue", "dial_code": "683", "code": "NU" }, { "name": "Norfolk Island", "dial_code": "672", "code": "NF" }, { "name": "Northern Mariana Islands", "dial_code": "1 670", "code": "MP" }, { "name": "Norway", "dial_code": "47", "code": "NO" }, { "name": "Oman", "dial_code": "968", "code": "OM" }, { "name": "Pakistan", "dial_code": "92", "code": "PK" }, { "name": "Palau", "dial_code": "680", "code": "PW" }, { "name": "Palestinian Territory, Occupied", "dial_code": "970", "code": "PS" }, { "name": "Panama", "dial_code": "507", "code": "PA" }, { "name": "Papua New Guinea", "dial_code": "675", "code": "PG" }, { "name": "Paraguay", "dial_code": "595", "code": "PY" }, { "name": "Peru", "dial_code": "51", "code": "PE" }, { "name": "Philippines", "dial_code": "63", "code": "PH" }, { "name": "Pitcairn", "dial_code": "872", "code": "PN" }, { "name": "Poland", "dial_code": "48", "code": "PL" }, { "name": "Portugal", "dial_code": "351", "code": "PT" }, { "name": "Puerto Rico", "dial_code": "1 939", "code": "PR" }, { "name": "Qatar", "dial_code": "974", "code": "QA" }, { "name": "Romania", "dial_code": "40", "code": "RO" }, { "name": "Russia", "dial_code": "7", "code": "RU" }, { "name": "Rwanda", "dial_code": "250", "code": "RW" }, { "name": "Reunion", "dial_code": "262", "code": "RE" }, { "name": "Saint Barthelemy", "dial_code": "590", "code": "BL" }, { "name": "Saint Helena, Ascension and Tristan Da Cunha", "dial_code": "290", "code": "SH" }, { "name": "Saint Kitts and Nevis", "dial_code": "1 869", "code": "KN" }, { "name": "Saint Lucia", "dial_code": "1 758", "code": "LC" }, { "name": "Saint Martin", "dial_code": "590", "code": "MF" }, { "name": "Saint Pierre and Miquelon", "dial_code": "508", "code": "PM" }, { "name": "Saint Vincent and the Grenadines", "dial_code": "1 784", "code": "VC" }, { "name": "Samoa", "dial_code": "685", "code": "WS" }, { "name": "San Marino", "dial_code": "378", "code": "SM" }, { "name": "Sao Tome and Principe", "dial_code": "239", "code": "ST" }, { "name": "Saudi Arabia", "dial_code": "966", "code": "SA" }, { "name": "Senegal", "dial_code": "221", "code": "SN" }, { "name": "Serbia", "dial_code": "381", "code": "RS" }, { "name": "Seychelles", "dial_code": "248", "code": "SC" }, { "name": "Sierra Leone", "dial_code": "232", "code": "SL" }, { "name": "Singapore", "dial_code": "65", "code": "SG" }, { "name": "Slovakia", "dial_code": "421", "code": "SK" }, { "name": "Slovenia", "dial_code": "386", "code": "SI" }, { "name": "Solomon Islands", "dial_code": "677", "code": "SB" }, { "name": "Somalia", "dial_code": "252", "code": "SO" }, { "name": "South Africa", "dial_code": "27", "code": "ZA" }, { "name": "South Georgia and the South Sandwich Islands", "dial_code": "500", "code": "GS" }, { "name": "Spain", "dial_code": "34", "code": "ES" }, { "name": "Sri Lanka", "dial_code": "94", "code": "LK" }, { "name": "Sudan", "dial_code": "249", "code": "SD" }, { "name": "Suriname", "dial_code": "597", "code": "SR" }, { "name": "Svalbard and Jan Mayen", "dial_code": "47", "code": "SJ" }, { "name": "Swaziland", "dial_code": "268", "code": "SZ" }, { "name": "Sweden", "dial_code": "46", "code": "SE" }, { "name": "Switzerland", "dial_code": "41", "code": "CH" }, { "name": "Syrian Arab Republic", "dial_code": "963", "code": "SY" }, { "name": "Taiwan", "dial_code": "886", "code": "TW" }, { "name": "Tajikistan", "dial_code": "992", "code": "TJ" }, { "name": "Tanzania, United Republic of Tanzania", "dial_code": "255", "code": "TZ" }, { "name": "Thailand", "dial_code": "66", "code": "TH" }, { "name": "Timor-Leste", "dial_code": "670", "code": "TL" }, { "name": "Togo", "dial_code": "228", "code": "TG" }, { "name": "Tokelau", "dial_code": "690", "code": "TK" }, { "name": "Tonga", "dial_code": "676", "code": "TO" }, { "name": "Trinidad and Tobago", "dial_code": "1 868", "code": "TT" }, { "name": "Tunisia", "dial_code": "216", "code": "TN" }, { "name": "Turkey", "dial_code": "90", "code": "TR" }, { "name": "Turkmenistan", "dial_code": "993", "code": "TM" }, { "name": "Turks and Caicos Islands", "dial_code": "1 649", "code": "TC" }, { "name": "Tuvalu", "dial_code": "688", "code": "TV" }, { "name": "Uganda", "dial_code": "256", "code": "UG" }, { "name": "Ukraine", "dial_code": "380", "code": "UA" }, { "name": "United Arab Emirates", "dial_code": "971", "code": "AE" }, { "name": "United Kingdom", "dial_code": "44", "code": "GB" }, { "name": "United States", "dial_code": "1", "code": "US" }, { "name": "Uruguay", "dial_code": "598", "code": "UY" }, { "name": "Uzbekistan", "dial_code": "998", "code": "UZ" }, { "name": "Vanuatu", "dial_code": "678", "code": "VU" }, { "name": "Venezuela, Bolivarian Republic of Venezuela", "dial_code": "58", "code": "VE" }, { "name": "Vietnam", "dial_code": "84", "code": "VN" }, { "name": "Virgin Islands, British", "dial_code": "1 284", "code": "VG" }, { "name": "Virgin Islands, U.S.", "dial_code": "1 340", "code": "VI" }, { "name": "Wallis and Futuna", "dial_code": "681", "code": "WF" }, { "name": "Yemen", "dial_code": "967", "code": "YE" }, { "name": "Zambia", "dial_code": "260", "code": "ZM" }, { "name": "Zimbabwe", "dial_code": "263", "code": "ZW" }];


        // Pass Data into MVC Controller
        $scope.submitRsvNo = function (rsvNo) {
            $('form#rsvno input#rsvno-input').val(rsvNo);
            $('form#rsvno').submit();
        }

        $scope.getCountries = function (dialCode) {
            for (var i = 0; i < $scope.countries.length; i++) {
                if ($scope.countries[i].dial_code == dialCode) {
                    return $scope.countries[i].name;
                }
            }
        }

        //Functions for Displaying Hotel Information

        $scope.totalFlightpax = function (pax) {
            return pax.length;
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

        $scope.titlePax = function(type) {
            if (type == 1)
                return 'Tuan. ';
            if (type == 2)
                return 'Nyonya. ';
            if (type == 3)
                return 'Nona. ';
        }

        $scope.paxType = function (type) {
            if (type == 1)
                return 'Dewasa';
            if (type == 2)
                return 'Anak-anak';
            if (type == 3)
                return 'Bayi';
        }

        $scope.getPhotoUrl = function (url) {
            return 'http://photos.hotelbeds.com/giata/small/' + url;
        }

        //Get Profile
        $scope.TakeProfileConfig = {
            TakeProfile: function () {
                if ($scope.trial > 3) {
                    $scope.trial = 0;
                }
                var authAccess = getAuthAccess();
                if (authAccess == 2) {
                    $http({
                        method: 'GET',
                        url: GetProfileConfig.Url,
                        async: false,
                        headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                    }).then(function (returnData) {
                        if (returnData.status == "200") {
                            $log.debug('Success getting Profile');
                            $scope.userProfile = {
                                email: returnData.data.email,
                                name: returnData.data.name,
                                countryCallCd: parseInt(returnData.data.countryCallCd),
                                phone: parseInt(returnData.data.phone)
                            };
                            $scope.editProfile = $scope.userProfile;
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
                    $log.debug('Not Authorized');
                }
            }
        }

        //Get Transaction History

        $scope.trxHistory = {
            firstload: true,
            getTrxHistory: function (filter) {
                if ($scope.trial > 3) {
                    $scope.trial = 0;
                }
                var authAccess = getAuthAccess();
                $scope.loading = true;
                if (authAccess == 2) {
                    $http({
                        method: 'POST',
                        url: ApproverOrderListPathConfig.Url,
                        headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') },
                        data: {
                            filter: filter
                        }
                    }).then(function (returnData) {
                        $scope.loading = false;
                        if (returnData.data.status == "200") {
                            $log.debug('Success getting Transaction');
                            $scope.orderList = returnData.data.reservations;
                            if ($scope.trxHistory.firstload == true) {
                                $timeout(function () {
                                    $('body .ol-item-hide').hide();
                                    $('body .ol-item').click(function () {
                                        $(this).parent().find('.ol-item-hide').toggle();
                                        $(this).parent().find('.ol-item').toggleClass('show');
                                        $(this).parent().siblings().find('.ol-item-hide').hide();
                                        $(this).parent().siblings().find('.ol-item').removeClass('show');
                                    });
                                }, 0);
                                $scope.trxHistory.firstload = false;
                            }
                            
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
                            $scope.trxHistory.getTrxHistory(filter);
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

        $scope.updateReservation = {
            rsvUpdated : false,
            isConfirm: false,
            isFailed:false,
            rsvNo : "",
            status: "",
            select : function(rsvNo, status) {
                $scope.updateData.rsvNo = rsvNo;
                $scope.updateData.status = status;
                $scope.updateData.isConfirm = true;
            },
            update: function (rsvNo, status) {
                $scope.updateData.rsvNo = rsvNo;
                $scope.updateData.status = status;
                $scope.updateData.isConfirm = false;
                var authAccess = getAuthAccess();
                if (authAccess == 2) {
                    //authorized
                    $http({
                        url: UpdateReservationConfig.Url,
                        method: 'POST',
                        data: {
                            rsvNo: $scope.updateData.rsvNo,
                            status: $scope.updateData.status,
                            message: $scope.updateData.message
                        },
                        headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                    }).then(function (returnData) {
                        //$log.debug(returnData);
                        if (returnData.data.status == '200') {
                            $log.debug('Success updating reservation');
                            $scope.updateReservation.rsvUpdated = true;
                            $scope.updateReservation.isFailed = false;
                            window.location.reload();
                        }
                        else {
                            $log.debug(returnData.data.error);
                            $log.debug(returnData);
                            $scope.updateReservation.rsvUpdated = false;
                            $scope.updateReservation.isFailed = true;
                            //window.location.reload();
                        }
                    }).catch(function (returnData) {
                        $scope.trial++;
                        if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                        {
                            $scope.updateReservation.update(type);
                        }
                        else {
                            $log.debug('Failed Update Reservation');
                            $log.debug(returnData);
                            $scope.updateReservation.rsvUpdated = false;
                            $scope.updateReservation.isFailed = true;
                            //window.location.reload();
                        }
                    });
                }
                else { //if not authorized
                    $scope.rsvUpdated = false;
                }
            }
        }

        //Executing Get Profile and Transaction History
        $scope.TakeProfileConfig.TakeProfile();
        $scope.trxHistory.getTrxHistory('pending');

        $scope.changeSection = function (name) {
            $scope.currentSection = name;
        }

        if (hash == '#order') {
            $scope.changeSection('order');
        } else {
            $scope.changeSection('profile');
        }

    }
]);// account controller end

app.controller('b2BBookerController', [
    '$http', '$scope', '$timeout', '$log', function ($http, $scope, $timeout, $log) {

        var hash = (location.hash);
        // variables

        //General Variables
        $scope.pageLoaded = true;
        $scope.email = 'Test';
        $scope.trial = 0;
        $scope.loading = false;
        $scope.currentSection = '';
        $scope.profileForm = {
            active: false
        };
        $scope.passwordForm = {
            active: false
        };
        $scope.isConfirm = false;
        $scope.close = function () {
            $scope.isConfirm = false;
        }

        $scope.userProfile = {
            email: '',
            name: '',
            countryCallCd: '',
            phone: '',
            edit: false,
            updating: false
        }

        $scope.bookerInfo = {
            name: '',
            bookerTitle: '',
            bookerMessage: '',
            edit: false,
            updating: false
        }

        $scope.updateData = {
            rsvNo: "",
            status: "",
            message: ""
        }

        $scope.bookingstatus = function (types) {
            if (types == 12)
                return 'Approved';
            else if (types == 13)
                return 'Pending';
            else if (types == 6)
                return 'Expired';
            else if (types == 11)
                return 'Rejected';
            else if (types == 8)
                return 'Cancelled';
            else if (types == 10)
                return 'Failed';
            else if (types == 0)
                return 'Pending';
        }

        $scope.password = {
            edit: false,
            updating: false,
            failed: false
        }

        $scope.countries = [{ "name": "Afghanistan", "dial_code": "93", "code": "AF" }, { "name": "Aland Islands", "dial_code": "358", "code": "AX" }, { "name": "Albania", "dial_code": "355", "code": "AL" }, { "name": "Algeria", "dial_code": "213", "code": "DZ" }, { "name": "AmericanSamoa", "dial_code": "1 684", "code": "AS" }, { "name": "Andorra", "dial_code": "376", "code": "AD" }, { "name": "Angola", "dial_code": "244", "code": "AO" }, { "name": "Anguilla", "dial_code": "1 264", "code": "AI" }, { "name": "Antarctica", "dial_code": "672", "code": "AQ" }, { "name": "Antigua and Barbuda", "dial_code": "1268", "code": "AG" }, { "name": "Argentina", "dial_code": "54", "code": "AR" }, { "name": "Armenia", "dial_code": "374", "code": "AM" }, { "name": "Aruba", "dial_code": "297", "code": "AW" }, { "name": "Australia", "dial_code": "61", "code": "AU" }, { "name": "Austria", "dial_code": "43", "code": "AT" }, { "name": "Azerbaijan", "dial_code": "994", "code": "AZ" }, { "name": "Bahamas", "dial_code": "1 242", "code": "BS" }, { "name": "Bahrain", "dial_code": "973", "code": "BH" }, { "name": "Bangladesh", "dial_code": "880", "code": "BD" }, { "name": "Barbados", "dial_code": "1 246", "code": "BB" }, { "name": "Belarus", "dial_code": "375", "code": "BY" }, { "name": "Belgium", "dial_code": "32", "code": "BE" }, { "name": "Belize", "dial_code": "501", "code": "BZ" }, { "name": "Benin", "dial_code": "229", "code": "BJ" }, { "name": "Bermuda", "dial_code": "1 441", "code": "BM" }, { "name": "Bhutan", "dial_code": "975", "code": "BT" }, { "name": "Bolivia, Plurinational State of", "dial_code": "591", "code": "BO" }, { "name": "Bosnia and Herzegovina", "dial_code": "387", "code": "BA" }, { "name": "Botswana", "dial_code": "267", "code": "BW" }, { "name": "Brazil", "dial_code": "55", "code": "BR" }, { "name": "British Indian Ocean Territory", "dial_code": "246", "code": "IO" }, { "name": "Brunei Darussalam", "dial_code": "673", "code": "BN" }, { "name": "Bulgaria", "dial_code": "359", "code": "BG" }, { "name": "Burkina Faso", "dial_code": "226", "code": "BF" }, { "name": "Burundi", "dial_code": "257", "code": "BI" }, { "name": "Cambodia", "dial_code": "855", "code": "KH" }, { "name": "Cameroon", "dial_code": "237", "code": "CM" }, { "name": "Canada", "dial_code": "1", "code": "CA" }, { "name": "Cape Verde", "dial_code": "238", "code": "CV" }, { "name": "Cayman Islands", "dial_code": " 345", "code": "KY" }, { "name": "Central African Republic", "dial_code": "236", "code": "CF" }, { "name": "Chad", "dial_code": "235", "code": "TD" }, { "name": "Chile", "dial_code": "56", "code": "CL" }, { "name": "China", "dial_code": "86", "code": "CN" }, { "name": "Christmas Island", "dial_code": "61", "code": "CX" }, { "name": "Cocos (Keeling) Islands", "dial_code": "61", "code": "CC" }, { "name": "Colombia", "dial_code": "57", "code": "CO" }, { "name": "Comoros", "dial_code": "269", "code": "KM" }, { "name": "Congo", "dial_code": "242", "code": "CG" }, { "name": "Congo, The Democratic Republic of the Congo", "dial_code": "243", "code": "CD" }, { "name": "Cook Islands", "dial_code": "682", "code": "CK" }, { "name": "Costa Rica", "dial_code": "506", "code": "CR" }, { "name": "Cote d'Ivoire", "dial_code": "225", "code": "CI" }, { "name": "Croatia", "dial_code": "385", "code": "HR" }, { "name": "Cuba", "dial_code": "53", "code": "CU" }, { "name": "Cyprus", "dial_code": "357", "code": "CY" }, { "name": "Czech Republic", "dial_code": "420", "code": "CZ" }, { "name": "Denmark", "dial_code": "45", "code": "DK" }, { "name": "Djibouti", "dial_code": "253", "code": "DJ" }, { "name": "Dominica", "dial_code": "1 767", "code": "DM" }, { "name": "Dominican Republic", "dial_code": "1 849", "code": "DO" }, { "name": "Ecuador", "dial_code": "593", "code": "EC" }, { "name": "Egypt", "dial_code": "20", "code": "EG" }, { "name": "El Salvador", "dial_code": "503", "code": "SV" }, { "name": "Equatorial Guinea", "dial_code": "240", "code": "GQ" }, { "name": "Eritrea", "dial_code": "291", "code": "ER" }, { "name": "Estonia", "dial_code": "372", "code": "EE" }, { "name": "Ethiopia", "dial_code": "251", "code": "ET" }, { "name": "Falkland Islands (Malvinas)", "dial_code": "500", "code": "FK" }, { "name": "Faroe Islands", "dial_code": "298", "code": "FO" }, { "name": "Fiji", "dial_code": "679", "code": "FJ" }, { "name": "Finland", "dial_code": "358", "code": "FI" }, { "name": "France", "dial_code": "33", "code": "FR" }, { "name": "French Guiana", "dial_code": "594", "code": "GF" }, { "name": "French Polynesia", "dial_code": "689", "code": "PF" }, { "name": "Gabon", "dial_code": "241", "code": "GA" }, { "name": "Gambia", "dial_code": "220", "code": "GM" }, { "name": "Georgia", "dial_code": "995", "code": "GE" }, { "name": "Germany", "dial_code": "49", "code": "DE" }, { "name": "Ghana", "dial_code": "233", "code": "GH" }, { "name": "Gibraltar", "dial_code": "350", "code": "GI" }, { "name": "Greece", "dial_code": "30", "code": "GR" }, { "name": "Greenland", "dial_code": "299", "code": "GL" }, { "name": "Grenada", "dial_code": "1 473", "code": "GD" }, { "name": "Guadeloupe", "dial_code": "590", "code": "GP" }, { "name": "Guam", "dial_code": "1 671", "code": "GU" }, { "name": "Guatemala", "dial_code": "502", "code": "GT" }, { "name": "Guernsey", "dial_code": "44", "code": "GG" }, { "name": "Guinea", "dial_code": "224", "code": "GN" }, { "name": "Guinea-Bissau", "dial_code": "245", "code": "GW" }, { "name": "Guyana", "dial_code": "595", "code": "GY" }, { "name": "Haiti", "dial_code": "509", "code": "HT" }, { "name": "Holy See (Vatican City State)", "dial_code": "379", "code": "VA" }, { "name": "Honduras", "dial_code": "504", "code": "HN" }, { "name": "Hong Kong", "dial_code": "852", "code": "HK" }, { "name": "Hungary", "dial_code": "36", "code": "HU" }, { "name": "Iceland", "dial_code": "354", "code": "IS" }, { "name": "India", "dial_code": "91", "code": "IN" }, { "name": "Indonesia", "dial_code": "62", "code": "ID" }, { "name": "Iran, Islamic Republic of Persian Gulf", "dial_code": "98", "code": "IR" }, { "name": "Iraq", "dial_code": "964", "code": "IQ" }, { "name": "Ireland", "dial_code": "353", "code": "IE" }, { "name": "Isle of Man", "dial_code": "44", "code": "IM" }, { "name": "Israel", "dial_code": "972", "code": "IL" }, { "name": "Italy", "dial_code": "39", "code": "IT" }, { "name": "Jamaica", "dial_code": "1 876", "code": "JM" }, { "name": "Japan", "dial_code": "81", "code": "JP" }, { "name": "Jersey", "dial_code": "44", "code": "JE" }, { "name": "Jordan", "dial_code": "962", "code": "JO" }, { "name": "Kazakhstan", "dial_code": "7 7", "code": "KZ" }, { "name": "Kenya", "dial_code": "254", "code": "KE" }, { "name": "Kiribati", "dial_code": "686", "code": "KI" }, { "name": "Korea, Democratic People's Republic of Korea", "dial_code": "850", "code": "KP" }, { "name": "Korea, Republic of South Korea", "dial_code": "82", "code": "KR" }, { "name": "Kuwait", "dial_code": "965", "code": "KW" }, { "name": "Kyrgyzstan", "dial_code": "996", "code": "KG" }, { "name": "Laos", "dial_code": "856", "code": "LA" }, { "name": "Latvia", "dial_code": "371", "code": "LV" }, { "name": "Lebanon", "dial_code": "961", "code": "LB" }, { "name": "Lesotho", "dial_code": "266", "code": "LS" }, { "name": "Liberia", "dial_code": "231", "code": "LR" }, { "name": "Libyan Arab Jamahiriya", "dial_code": "218", "code": "LY" }, { "name": "Liechtenstein", "dial_code": "423", "code": "LI" }, { "name": "Lithuania", "dial_code": "370", "code": "LT" }, { "name": "Luxembourg", "dial_code": "352", "code": "LU" }, { "name": "Macao", "dial_code": "853", "code": "MO" }, { "name": "Macedonia", "dial_code": "389", "code": "MK" }, { "name": "Madagascar", "dial_code": "261", "code": "MG" }, { "name": "Malawi", "dial_code": "265", "code": "MW" }, { "name": "Malaysia", "dial_code": "60", "code": "MY" }, { "name": "Maldives", "dial_code": "960", "code": "MV" }, { "name": "Mali", "dial_code": "223", "code": "ML" }, { "name": "Malta", "dial_code": "356", "code": "MT" }, { "name": "Marshall Islands", "dial_code": "692", "code": "MH" }, { "name": "Martinique", "dial_code": "596", "code": "MQ" }, { "name": "Mauritania", "dial_code": "222", "code": "MR" }, { "name": "Mauritius", "dial_code": "230", "code": "MU" }, { "name": "Mayotte", "dial_code": "262", "code": "YT" }, { "name": "Mexico", "dial_code": "52", "code": "MX" }, { "name": "Micronesia, Federated States of Micronesia", "dial_code": "691", "code": "FM" }, { "name": "Moldova", "dial_code": "373", "code": "MD" }, { "name": "Monaco", "dial_code": "377", "code": "MC" }, { "name": "Mongolia", "dial_code": "976", "code": "MN" }, { "name": "Montenegro", "dial_code": "382", "code": "ME" }, { "name": "Montserrat", "dial_code": "1664", "code": "MS" }, { "name": "Morocco", "dial_code": "212", "code": "MA" }, { "name": "Mozambique", "dial_code": "258", "code": "MZ" }, { "name": "Myanmar", "dial_code": "95", "code": "MM" }, { "name": "Namibia", "dial_code": "264", "code": "NA" }, { "name": "Nauru", "dial_code": "674", "code": "NR" }, { "name": "Nepal", "dial_code": "977", "code": "NP" }, { "name": "Netherlands", "dial_code": "31", "code": "NL" }, { "name": "Netherlands Antilles", "dial_code": "599", "code": "AN" }, { "name": "New Caledonia", "dial_code": "687", "code": "NC" }, { "name": "New Zealand", "dial_code": "64", "code": "NZ" }, { "name": "Nicaragua", "dial_code": "505", "code": "NI" }, { "name": "Niger", "dial_code": "227", "code": "NE" }, { "name": "Nigeria", "dial_code": "234", "code": "NG" }, { "name": "Niue", "dial_code": "683", "code": "NU" }, { "name": "Norfolk Island", "dial_code": "672", "code": "NF" }, { "name": "Northern Mariana Islands", "dial_code": "1 670", "code": "MP" }, { "name": "Norway", "dial_code": "47", "code": "NO" }, { "name": "Oman", "dial_code": "968", "code": "OM" }, { "name": "Pakistan", "dial_code": "92", "code": "PK" }, { "name": "Palau", "dial_code": "680", "code": "PW" }, { "name": "Palestinian Territory, Occupied", "dial_code": "970", "code": "PS" }, { "name": "Panama", "dial_code": "507", "code": "PA" }, { "name": "Papua New Guinea", "dial_code": "675", "code": "PG" }, { "name": "Paraguay", "dial_code": "595", "code": "PY" }, { "name": "Peru", "dial_code": "51", "code": "PE" }, { "name": "Philippines", "dial_code": "63", "code": "PH" }, { "name": "Pitcairn", "dial_code": "872", "code": "PN" }, { "name": "Poland", "dial_code": "48", "code": "PL" }, { "name": "Portugal", "dial_code": "351", "code": "PT" }, { "name": "Puerto Rico", "dial_code": "1 939", "code": "PR" }, { "name": "Qatar", "dial_code": "974", "code": "QA" }, { "name": "Romania", "dial_code": "40", "code": "RO" }, { "name": "Russia", "dial_code": "7", "code": "RU" }, { "name": "Rwanda", "dial_code": "250", "code": "RW" }, { "name": "Reunion", "dial_code": "262", "code": "RE" }, { "name": "Saint Barthelemy", "dial_code": "590", "code": "BL" }, { "name": "Saint Helena, Ascension and Tristan Da Cunha", "dial_code": "290", "code": "SH" }, { "name": "Saint Kitts and Nevis", "dial_code": "1 869", "code": "KN" }, { "name": "Saint Lucia", "dial_code": "1 758", "code": "LC" }, { "name": "Saint Martin", "dial_code": "590", "code": "MF" }, { "name": "Saint Pierre and Miquelon", "dial_code": "508", "code": "PM" }, { "name": "Saint Vincent and the Grenadines", "dial_code": "1 784", "code": "VC" }, { "name": "Samoa", "dial_code": "685", "code": "WS" }, { "name": "San Marino", "dial_code": "378", "code": "SM" }, { "name": "Sao Tome and Principe", "dial_code": "239", "code": "ST" }, { "name": "Saudi Arabia", "dial_code": "966", "code": "SA" }, { "name": "Senegal", "dial_code": "221", "code": "SN" }, { "name": "Serbia", "dial_code": "381", "code": "RS" }, { "name": "Seychelles", "dial_code": "248", "code": "SC" }, { "name": "Sierra Leone", "dial_code": "232", "code": "SL" }, { "name": "Singapore", "dial_code": "65", "code": "SG" }, { "name": "Slovakia", "dial_code": "421", "code": "SK" }, { "name": "Slovenia", "dial_code": "386", "code": "SI" }, { "name": "Solomon Islands", "dial_code": "677", "code": "SB" }, { "name": "Somalia", "dial_code": "252", "code": "SO" }, { "name": "South Africa", "dial_code": "27", "code": "ZA" }, { "name": "South Georgia and the South Sandwich Islands", "dial_code": "500", "code": "GS" }, { "name": "Spain", "dial_code": "34", "code": "ES" }, { "name": "Sri Lanka", "dial_code": "94", "code": "LK" }, { "name": "Sudan", "dial_code": "249", "code": "SD" }, { "name": "Suriname", "dial_code": "597", "code": "SR" }, { "name": "Svalbard and Jan Mayen", "dial_code": "47", "code": "SJ" }, { "name": "Swaziland", "dial_code": "268", "code": "SZ" }, { "name": "Sweden", "dial_code": "46", "code": "SE" }, { "name": "Switzerland", "dial_code": "41", "code": "CH" }, { "name": "Syrian Arab Republic", "dial_code": "963", "code": "SY" }, { "name": "Taiwan", "dial_code": "886", "code": "TW" }, { "name": "Tajikistan", "dial_code": "992", "code": "TJ" }, { "name": "Tanzania, United Republic of Tanzania", "dial_code": "255", "code": "TZ" }, { "name": "Thailand", "dial_code": "66", "code": "TH" }, { "name": "Timor-Leste", "dial_code": "670", "code": "TL" }, { "name": "Togo", "dial_code": "228", "code": "TG" }, { "name": "Tokelau", "dial_code": "690", "code": "TK" }, { "name": "Tonga", "dial_code": "676", "code": "TO" }, { "name": "Trinidad and Tobago", "dial_code": "1 868", "code": "TT" }, { "name": "Tunisia", "dial_code": "216", "code": "TN" }, { "name": "Turkey", "dial_code": "90", "code": "TR" }, { "name": "Turkmenistan", "dial_code": "993", "code": "TM" }, { "name": "Turks and Caicos Islands", "dial_code": "1 649", "code": "TC" }, { "name": "Tuvalu", "dial_code": "688", "code": "TV" }, { "name": "Uganda", "dial_code": "256", "code": "UG" }, { "name": "Ukraine", "dial_code": "380", "code": "UA" }, { "name": "United Arab Emirates", "dial_code": "971", "code": "AE" }, { "name": "United Kingdom", "dial_code": "44", "code": "GB" }, { "name": "United States", "dial_code": "1", "code": "US" }, { "name": "Uruguay", "dial_code": "598", "code": "UY" }, { "name": "Uzbekistan", "dial_code": "998", "code": "UZ" }, { "name": "Vanuatu", "dial_code": "678", "code": "VU" }, { "name": "Venezuela, Bolivarian Republic of Venezuela", "dial_code": "58", "code": "VE" }, { "name": "Vietnam", "dial_code": "84", "code": "VN" }, { "name": "Virgin Islands, British", "dial_code": "1 284", "code": "VG" }, { "name": "Virgin Islands, U.S.", "dial_code": "1 340", "code": "VI" }, { "name": "Wallis and Futuna", "dial_code": "681", "code": "WF" }, { "name": "Yemen", "dial_code": "967", "code": "YE" }, { "name": "Zambia", "dial_code": "260", "code": "ZM" }, { "name": "Zimbabwe", "dial_code": "263", "code": "ZW" }];


        // Pass Data into MVC Controller
        $scope.submitRsvNo = function (rsvNo) {
            $('form#rsvno input#rsvno-input').val(rsvNo);
            $('form#rsvno').submit();
        }

        $scope.getCountries = function (dialCode) {
            for (var i = 0; i < $scope.countries.length; i++) {
                if ($scope.countries[i].dial_code == dialCode) {
                    return $scope.countries[i].name;
                }
            }
        }

        $scope.infoOrder = function (index) {
            $scope.bookerInfo.name = $scope.orderList[index].bookerName;
            $scope.bookerInfo.bookerTitle = $scope.orderList[index].bookerMessageTitle;
            $scope.bookerInfo.bookerMessage = $scope.orderList[index].bookerMessageDescription;
            $('.info-order').modal('show');
        }

        //Functions for Displaying Hotel Information

        $scope.totalFlightpax = function (pax) {
            return pax.length;
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

        $scope.titlePax = function (type) {
            if (type == 1)
                return 'Tuan. ';
            if (type == 2)
                return 'Nyonya. ';
            if (type == 3)
                return 'Nona. ';
        }

        $scope.paxType = function (type) {
            if (type == 1)
                return 'Dewasa';
            if (type == 2)
                return 'Anak-anak';
            if (type == 3)
                return 'Bayi';
        }

        $scope.getPhotoUrl = function (url) {
            return 'http://photos.hotelbeds.com/giata/small/' + url;
        }

        //Get Profile
        $scope.TakeProfileConfig = {
            TakeProfile: function () {
                if ($scope.trial > 3) {
                    $scope.trial = 0;
                }
                var authAccess = getAuthAccess();
                if (authAccess == 2) {
                    $http({
                        method: 'GET',
                        url: GetProfileConfig.Url,
                        async: false,
                        headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                    }).then(function (returnData) {
                        if (returnData.status == "200") {
                            $log.debug('Success getting Profile');
                            $scope.userProfile = {
                                email: returnData.data.email,
                                name: returnData.data.name,
                                countryCallCd: parseInt(returnData.data.countryCallCd),
                                phone: parseInt(returnData.data.phone)
                            };
                            $scope.editProfile = $scope.userProfile;
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
                    $log.debug('Not Authorized');
                }
            }
        }

        //Get Transaction History

        $scope.trxHistory = {
            firstload: true,
            getTrxHistory: function (filter) {
                if ($scope.trial > 3) {
                    $scope.trial = 0;
                }
                var authAccess = getAuthAccess();
                $scope.loading = true;
                if (authAccess == 2) {
                    $http({
                        method: 'POST',
                        url: BookerOrderListPathConfig.Url,
                        headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') },
                        data: {
                            filter: filter
                        }
                    }).then(function (returnData) {
                        $scope.loading = false;
                        if (returnData.data.status == "200") {
                            $log.debug('Success getting Transaction');
                            $scope.orderList = returnData.data.reservations;
                            if ($scope.trxHistory.firstload == true) {
                                $timeout(function () {
                                    $('body .ol-item-hide').hide();
                                    $('body .ol-item').click(function () {
                                        $(this).parent().find('.ol-item-hide').toggle();
                                        $(this).parent().find('.ol-item').toggleClass('show');
                                        $(this).parent().siblings().find('.ol-item-hide').hide();
                                        $(this).parent().siblings().find('.ol-item').removeClass('show');
                                    });
                                }, 0);
                                $scope.trxHistory.firstload = false;
                            }

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
                            $scope.trxHistory.getTrxHistory(filter);
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

        //Executing Get Profile and Transaction History
        $scope.TakeProfileConfig.TakeProfile();
        $scope.trxHistory.getTrxHistory('pending');

        $scope.changeSection = function (name) {
            $scope.currentSection = name;
        }

        if (hash == '#order') {
            $scope.changeSection('order');
        } else {
            $scope.changeSection('profile');
        }

    }
]);// account controller end