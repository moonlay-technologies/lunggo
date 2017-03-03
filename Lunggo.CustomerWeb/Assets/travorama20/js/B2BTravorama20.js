// check if angular exist
if (typeof (angular) == 'object') {

    var app = angular.module('travoramaB2B', ['ngRoute', 'ngResource']);

    // root scope
    app.run(function ($rootScope) {
        $rootScope.travoramaModuleName = 'travoramaB2B';
        $rootScope.profileloaded = false;
        $rootScope.email = '';
        $rootScope.name = '';
        $rootScope.trial = 0;
        $rootScope.roles = [];
        $rootScope.isAdmin = false;
        $rootScope.isFinance = false;
        $rootScope.returnUrl = document.referrer;
        $rootScope.authProfile = {} //tbd
        $rootScope.windowlocation = window.location.pathname;

        $rootScope.trial = 0;
        $rootScope.getProfile = function() {
            
            if ($rootScope.trial > 3) {
                $rootScope.trial = 0;
            }
            $.ajax({
                method: 'GET',
                url: GetProfileConfig.Url,
                headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') },
                async: false
            }).done(function (returnData) {
                if (returnData.data.status == "200") {
                    $rootScope.pageLoaded = true;
                    $rootScope.isLogin = true;
                    //$log.debug('Success getting Profile');
                    $rootScope.email = returnData.data.email;
                    $rootScope.name = returnData.data.name;
                    $rootScope.roles = returnData.data.roles;
                    $rootScope.getRole();
                    $rootScope.profileloaded = true;
                } else {
                    //$log.debug('There is an error');
                    //$log.debug('Error : ' + returnData.data.error);
                    $rootScope.pageLoaded = true;
                    $rootScope.isLogin = false;
                    $rootScope.profileloaded = true;
                    //$log.debug(returnData);
                }
            }).error(function () {
                $rootScope.trial++;
                if (refreshAuthAccess() && $rootScope.trial < 4) //refresh cookie
                {
                    $rootScope.getProfile();
                } else {
                    //$log.debug('Failed to Login');
                    $rootScope.pageLoaded = true;
                    $rootScope.isLogin = false;
                    $rootScope.profileloaded = true;
                }
            });
        }
        //$rootScope.getProfile();
        $rootScope.getRole = function () {
            for (var i = 0; i < $rootScope.roles.length; i++) {
                if ($rootScope.roles[i] === "Admin") {
                    $rootScope.isAdmin = true;
                    $('.paymentmgmt').hide();
                }
                if ($rootScope.roles[i] === "Finance") {
                    $rootScope.isFinance = true;
                }
            }
        };
    });//app.run
}

function getAnonymousFirstAccess() {
    var status = 0;
    $.ajax({
        url: LoginConfig.Url,
        method: 'POST',
        async: false,
        data: JSON.stringify(
            {
                "clientId": "WWxoa2VrOXFSWFZOUXpSM1QycEpORTB5U1RWT1IxcHNXVlJOTTFsWFZYaE5hbVJwVFVSSk5FOUVTbWxOUkVVMFRrUlNhVmxxVlhwT01sbDNUbXBvYkUxNlJUMD0=",
                "clientSecret": "VFVSTk1sbHFiR3hhVjAweFdXMUdhbHBYVVhwYVIxRXpUWHBCTlUxRVRtdGFhbHBvV1ZSU2FFMUhSbXhOUkdob1dtcEpkMDVSUFQwPQ=="
            }),
        contentType: 'application/json',
    }).done(function (returnData) {
        if (returnData.status == '200') {
            setCookie("accesstoken", returnData.accessToken, returnData.expTime);
            setCookie("refreshtoken", returnData.refreshToken, returnData.expTime);
            if (getCookie('accesstoken')) {
                status = 1;
            }
            else {
                status = 0;
            }
        }
        else {
            status = 0;
        }
    });
    return status;
}

function getAnonymousAccessByRefreshToken(refreshToken) {
    var status = 0;
    $.ajax({
        url: LoginConfig.Url,
        method: 'POST',
        async: false,
        data: JSON.stringify({ "refreshtoken": refreshToken, "clientId": "Jajal", "clientSecret": "Standar" }),
        contentType: 'application/json',
    }).done(function (returnData) {
        if (returnData.status == '200') {
            setCookie("accesstoken", returnData.accessToken, returnData.expTime);
            setCookie("refreshtoken", returnData.refreshToken, returnData.expTime);
            if (getCookie('accesstoken')) {
                status = 1;
            }
            else {
                status = 0;
            }
        }
        else {
            status = 0;
        }
    });
    return status;
}


function getLoginAccessByRefreshToken(refreshToken) {
    var status = 0;
    $.ajax({
        url: LoginConfig.Url,
        method: 'POST',
        async: false,
        data: JSON.stringify({ "refreshtoken": refreshToken, "clientId": "Jajal", "clientSecret": "Standar" }),
        contentType: 'application/json',
    }).done(function (returnData) {
        if (returnData.status == '200') {
            setCookie("accesstoken", returnData.accessToken, returnData.expTime);
            setCookie("refreshtoken", returnData.refreshToken, returnData.expTime);
            setCookie("authkey", returnData.accessToken, returnData.expTime);
            if (getCookie('accesstoken')) {
                status = 2;
            }
            else {
                status = 0;
            }
        }
        else {
            status = 0;
        }
    });
    return status;
}

function getAuthAccess() {
    var token = getCookie('accesstoken');
    var refreshToken = getCookie('refreshtoken');
    var authKey = getCookie('authkey');
    var status = 0;

    if (authKey) {
        if (token) {
            return 2;
        }
        else {
            if (refreshToken) {
                status = getLoginAccessByRefreshToken(refreshToken);
                if (status == 0) {
                    status = getAnonymousFirstAccess();
                }
            }
            else {
                return 0; //harusnya gak pernah masuk sini
            }
        }
    }
    else {
        if (token) {
            return 1;
        }
        else {
            //Get Anonymous Token By Refresh Token
            if (refreshToken) {
                status = getAnonymousAccessByRefreshToken(refreshToken);
                if (status == 0) {
                    status = getAnonymousFirstAccess();
                }
            }
            else {
                //For Anynomius at first
                status = getAnonymousFirstAccess();
            }
        }
    }
    return status;
}


function refreshAuthAccess() {
    /*
    * If failed to get Authorization, but accesstoken is still exist
    */
    var token = getCookie('accesstoken');
    var refreshToken = getCookie('refreshtoken');
    var authKey = getCookie('authkey');
    var status = 0;
    if (refreshToken) {
        if (authKey) {
            status = getLoginAccessByRefreshToken(refreshToken);
            if (status == 0) {
                status = getAnonymousFirstAccess();
                eraseCookie('authkey');
            }

            if (status == 2) {
                return true;
            }
            else {
                return false;
            }
        }
        else {
            status = getAnonymousAccessByRefreshToken(refreshToken);
            if (status == 0) {
                status = getAnonymousFirstAccess();
            }

            if (status == 1 || status == 2) {
                return true;
            }
            else {
                return false;
            }
        }
    }
    else {
        getAnonymousFirstAccess();
        return true;
    }
}