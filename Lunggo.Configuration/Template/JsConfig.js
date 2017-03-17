//******************************************
// Variables
var SystemConfig = {
    SystemBusy: false
};

var FlightSearchConfig = {
    Url: '$apiUrl$$flightPath$',
    // generate search URL
    GenerateSearchParam: function (params) {
        if (typeof (params) == 'object') {
            // set search result variable
            var url = '';
            var departureParam = '';
            var returnParam = '';
            var passengerParam = '';
            // get variables
            var trip = params.trip || false;
            var departureDate = new Date(params.departureDate) || '';
            var returnDate = new Date(params.returnDate) || '';
            var origin = params.origin;
            var destination = params.destination;
            var passenger = [ params.adult, (params.children || 0), (params.infant || 0) ];
            var cabin = params.cabin.toLowerCase();
            // generate departure param
            departureParam = ( origin + destination ) + ( (('0' + departureDate.getDate()).slice(-2)) + (('0' + (departureDate.getMonth()+1)).slice(-2) ) + (departureDate.getFullYear().toString().substr(2,2)) );
            // generate return param
            if (trip == true) {
                returnParam = (destination + origin) + ((('0' + returnDate.getDate()).slice(-2)) + (('0' + (returnDate.getMonth() + 1)).slice(-2)) + (returnDate.getFullYear().toString().substr(2, 2)));
            }
            // generate passenger param
            if (cabin != 'y' || cabin != 'c' || cabin != 'f') {
                switch (cabin) {
                    case 'economy':
                        cabin = 'y';
                        break;
                    case 'business':
                        cabin = 'c';
                        break;
                    case 'first':
                        cabin = 'f';
                        break;
                }
            }
            passengerParam = passenger[0] + '' + passenger[1] + '' + passenger[2] + '' + cabin ;
            // generate search url
            if (trip == false) {
                url = departureParam + '-' + passengerParam;
            } else {
                url = departureParam + '~' + returnParam + '-' + passengerParam;
            }
            // return the search url
            return url;
        } else {
            console.log('Cannot generate Search URL. Parameter type should be in object. Sample : ');
            console.log('{ trip : true , departureDate : "10-January-2016", returnDate: "11-january-2016", origin : "CGK", destination : "DPS", adult : 1, children : 1, infant : 1 }');
        }
    }
};

var SelectConfig = {
    Url: '$apiUrl$$flightSelectPath$',
    working: false
};

var RevalidateConfig = {
    Url: '$apiUrl$$flightRevalidatePath$',
    working: false
};

var FlightBookConfig = {
    Url: '$apiUrl$$flightBookPath$',
    working: false
};

var FlightPayConfig = {
    Url: '$apiUrl$$flightPayPath$',
    working: false
};

var GetRulesConfig = {
    Url: '$apiUrl$$flightRulesPath$',
    working: false
};

var HotelAutocompleteConfig = {
    Url: '$apiUrl$$autocompleteHotelLocationPath$'
};

var FlightAutocompleteConfig = {
    Url: '$apiUrl$$autocompleteAirportPath$'
};

var AirlineAutocompleteConfig = {
    Url: '$apiUrl$$autocompleteAirlinePath$'
};

var CheckVoucherConfig = {
    Url: '$apiUrl$$checkVoucherPath$'
};

var CheckBinDiscountConfig = {
    Url: '$apiUrl$$checkBinDiscountPath$'
};

var SubscribeConfig = {
    Url: '$apiUrl$$subscribePath$'
};

var LoginConfig = {
    Url: '$apiUrl$$loginPath$'
};

var B2BLoginPathConfig = {
    Url: '$apiUrl$$b2bLoginPath$'
};

var GetProfileConfig = {
    Url: '$apiUrl$$getProfilePath$'
};

var RegisterConfig = {
    Url: '$apiUrl$$registerPath$'
};

var B2BRegisterConfig = {
    Url: '$apiUrl$$b2bRegisterPath$'
};

var GetUserConfig = {
    Url: '$apiUrl$$getUserPath$'
};

var AddUserConfig = {
    Url: '$apiUrl$$addUserPath$'
};

var UpdateRoleConfig = {
    Url: '$apiUrl$$updateRolePath$'
};

var UpdateUserConfig = {
    Url: '$apiUrl$$updateUserPath$'
};
var UpdateUserLockConfig = {
    Url: '$apiUrl$$updateUserLockPath$'
};

var DeleteUserConfig = {
    Url: '$apiUrl$$deleteUserPath$'
};

var ResetPasswordConfig = {
    Url: '$apiUrl$$resetPasswordPath$'
};

var ForgotPasswordConfig = {
    Url: '$apiUrl$$forgotPasswordPath$'
};

var ChangePasswordConfig = {
    Url: '$apiUrl$$changePasswordPath$'
};

var ChangeProfileConfig = {
    Url: '$apiUrl$$changeProfilePath$'
};

var TrxHistoryConfig = {
    Url: '$apiUrl$$trxHistoryPath$'
};

var BookerTrxHistoryConfig = {
    Url: '$apiUrl$$bookerTrxHistoryPath$'
};
var ApproverOrderListPathConfig = {
    Url: '$apiUrl$$approverorderlistPath$'
};

var BookerOrderListPathConfig = {
    Url: '$apiUrl$$bookerorderlistPath$'
};

var UpdateReservationConfig = {
    Url: '$apiUrl$$updateReservationPath$'
};

var GetReservationConfig = {
    Url: '$apiUrl$$getReservationPath$'
};

var ResendConfirmationEmailConfig = {
    Url: '$apiUrl$$resendConfirmationEmailPath$'
};

var VeritransTokenConfig = {
    Url: '$veritransTokenPath$',
    ClientKey: '$veritransClientKey$'
};

var uniqueCodePaymentConfig = {
    Url: '$apiUrl$$uniqueCodePaymentPath$'
};

var LoginMobileConfig = {
    Url: '$mobileUrl$$loginPath$'
};

var RegisterMobileConfig = {
    Url: '$mobileUrl$$registerPath$'
};

var ResetPasswordMobileConfig = {
    Url: '$mobileUrl$$resetPasswordPath$'
};

var ForgotPasswordMobileConfig = {
    Url: '$mobileUrl$$forgotPasswordPath$'
};

var ChangePasswordMobileConfig = {
    Url: '$mobileUrl$$changePasswordPath$'
};

var ChangeProfileMobileConfig = {
    Url: '$mobileUrl$$changeProfilePath$'
};

var ResendConfirmationEmailMobileConfig = {
    Url: '$mobileUrl$$resendConfirmationEmailPath$'
};

var HotelSearchConfig = {
    Url: '$apiUrl$$hotelSearchPath$'
};

var HotelDetailsConfig = {
    Url: '$apiUrl$$hotelDetailsPath$'
};

var HotelSelectConfig = {
    Url: '$apiUrl$$hotelSelectPath$'
};

var GetCreditCardConfig = {
    Url: '$apiUrl$$getCreditCardPath$'
};

var AddCreditCardConfig = {
    Url: '$apiUrl$$addCreditCardPath$'
};

var DeleteCreditCardConfig = {
    Url: '$apiUrl$$deleteCardPath$'
};

var SetPrimaryCardConfig = {
    Url: '$apiUrl$$setPrimaryCardPath$'
};

var HotelBookConfig = {
    Url: '$apiUrl$$hotelBookPath$',
    working: false
};

var HotelAvailableRatesConfig = {
    Url: '$apiUrl$$hotelAvailableRatesPath$',
    working: false
};


var GetHolidayConfig = {
    Url: '$apiUrl$$holidayListPath$',
    working: false
};
function setCookie(cname, cvalue, expTime) {

    if (cname != "accesstoken") {
        var d = new Date();
        d.setTime(d.getTime() + (9999 * 24 * 60 * 60 * 1000));
    }
    else {
        var d = new Date(expTime);
    }
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + "; " + expires + "; path=/";
}

//function setRefreshCookie(cvalue)
//{
//    var d = new Date();
//    d.setTime(d.getTime() + (9999 * 24 * 60 * 60 * 1000));
//    var expires = "expires=" + d.toUTCString();
//    document.cookie = "refreshtoken" + "=" + cvalue + "; " + expires + "; path=/";
//}

//function setAuthCookie(cvalue) {
//    var dAuth = new Date();
//    dAuth.setTime(dAuth.getTime() + (9999 * 24 * 60 * 60 * 1000));
//    var expiresAuth = "expires=" + dAuth.toUTCString();
//    document.cookie = "authKey" + "=" + cvalue + "; " + expiresAuth + "; path=/";
//}

//Get Value from Cookie
function getCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}

//Delete Specific value from Cookie
function eraseCookie(name) {
    var d = new Date(-1);
    var expires = "expires=" + d.toUTCString();
    var cvalue = "";
    document.cookie = name + "=" + cvalue + "; " + expires + "; path=/";
}