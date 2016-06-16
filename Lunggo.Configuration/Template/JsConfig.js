//******************************************
// Variables
var SystemConfig = {
    SystemBusy: false
};

var SearchHotelConfig = {
    Url: '$apiUrl$$hotelPath$',
    ResultCount: 24
};

var SearchRoomConfig = {
    Url: '$apiUrl$$roomPath$'
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
                url = departureParam + '.' + returnParam + '-' + passengerParam;
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

var SubscribeConfig = {
    Url: '$apiUrl$$subscribePath$'
};

var LoginConfig = {
    Url: '$apiUrl$$loginPath$'
};

var GetProfileConfig = {
    Url: '$apiUrl$$getProfilePath$'
};

var RegisterConfig = {
    Url: '$apiUrl$$registerPath$'
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

var TransferConfig = {
    Url: '$apiUrl$$transferPaymentPath$'
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

function setCookie(cname, cvalue, expTime) {
    var d = new Date(expTime);
    //d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + "; " + expires+ "; path=/";
}

function setRefreshCookie(cname, cvalue)
{
    var d = new Date();
    d.setTime(d.getTime() + (9999 * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + "; " + expires + "; path=/";
}

function isValid()
{
    var token = getCookie('accesstoken');
    var refreshToken = getCookie('refreshtoken');
    if (token) {
        return true;
    }
    else
    {
        console.log('Test');
        //Here to get Token
        if (refreshToken != null) {
            var xhttp = new XMLHttpRequest();
            xhttp.open("POST", LoginConfig.Url, true);
            xhttp.send("refreshtoken:"+getCookie('refreshtoken'));
            if (xhttp.status == "200") {
                return true;
            }
            else
            {
                return false;
            } 
        }
        else
        {
            return true;
        }
    }
}

function isLogin()
{
    var token = getCookie('accesstoken');
    var refreshToken = getCookie('refreshtoken');
    if (token != null || token != '') {
        return true;
    }
    else
    {
        if (refreshToken != null || refreshToken != '')
        {
            
        }
        //do some logic here if refresh token is not null
        return false;
    }
}


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
    setCookie(name, "", -1);
}

function deleteCookie(name, path) {
    // If the cookie exists
    if (getCookie(name))
        setCookie(name, "", -1, path);
}


