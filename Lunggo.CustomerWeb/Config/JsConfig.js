//******************************************
// Variables
var SystemConfig = {
    SystemBusy: false
};

var SearchHotelConfig = {
    Url: 'https://travorama-dv2-api.azurewebsites.net/api/v1/hotels',
    ResultCount: 24
};

var SearchRoomConfig = {
    Url: 'https://travorama-dv2-api.azurewebsites.net/api/v1/rooms'
};

var FlightSearchConfig = {
    Url: 'https://travorama-dv2-api.azurewebsites.net/flight',
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
    Url: 'https://travorama-dv2-api.azurewebsites.net/flight/select',
    working: false
};

var RevalidateConfig = {
    Url: 'https://travorama-dv2-api.azurewebsites.net/api/v1/flights/revalidate',
    working: false
};

var FlightBookConfig = {
    Url: 'https://travorama-dv2-api.azurewebsites.net/flight/book',
    working: false
};

var FlightPayConfig = {
    Url: 'https://travorama-dv2-api.azurewebsites.net/api/v1/flights/pay',
    working: false
};

var GetRulesConfig = {
    Url: 'https://travorama-dv2-api.azurewebsites.net/api/v1/flights/rules',
    working: false
};

var HotelAutocompleteConfig = {
    Url: 'https://travorama-dv2-api.azurewebsites.net/api/v1/autocomplete/hotellocation/'
};

var FlightAutocompleteConfig = {
    Url: 'https://travorama-dv2-api.azurewebsites.net/api/v1/autocomplete/airport/'
};

var AirlineAutocompleteConfig = {
    Url: 'https://travorama-dv2-api.azurewebsites.net/api/v1/autocomplete/airline/'
};

var CheckVoucherConfig = {
    Url: 'https://travorama-dv2-api.azurewebsites.net/api/v1/voucher/check'
};

var SubscribeConfig = {
    Url: 'https://travorama-dv2-api.azurewebsites.net/api/v1/newsletter/subscribe'
};

var LoginConfig = {
    Url: 'https://travorama-dv2-api.azurewebsites.net/login'
};

var GetProfileConfig = {
    Url: 'https://travorama-dv2-api.azurewebsites.net/profile'
};

var RegisterConfig = {
    Url: 'http://www.dv2.travorama.com/id/ApiAccount/Register'
};

var ResetPasswordConfig = {
    Url: 'http://www.dv2.travorama.com/id/ApiAccount/ResetPassword'
};

var ForgotPasswordConfig = {
    Url: 'http://www.dv2.travorama.com/id/ApiAccount/ForgotPassword'
};

var ChangePasswordConfig = {
    Url: 'http://www.dv2.travorama.com/id/ApiAccount/ChangePassword'
};

var ChangeProfileConfig = {
    Url: 'http://www.dv2.travorama.com/id/ApiAccount/ChangeProfile'
};

var ResendConfirmationEmailConfig = {
    Url: 'http://www.dv2.travorama.com/id/ApiAccount/ResendConfirmationEmail'
};

var VeritransTokenConfig = {
    Url: 'https://api.sandbox.veritrans.co.id/v2/token',
    ClientKey: 'VT-client-J8i9AzRyIU49D_v3'
};

var TransferConfig = {
    Url: 'https://travorama-dv2-api.azurewebsites.net/api/v1/transferidentifier'
};

var LoginMobileConfig = {
    Url: 'http://m.local.travorama.com/login'
};

var RegisterMobileConfig = {
    Url: 'http://m.local.travorama.com/id/ApiAccount/Register'
};

var ResetPasswordMobileConfig = {
    Url: 'http://m.local.travorama.com/id/ApiAccount/ResetPassword'
};

var ForgotPasswordMobileConfig = {
    Url: 'http://m.local.travorama.com/id/ApiAccount/ForgotPassword'
};

var ChangePasswordMobileConfig = {
    Url: 'http://m.local.travorama.com/id/ApiAccount/ChangePassword'
};

var ChangeProfileMobileConfig = {
    Url: 'http://m.local.travorama.com/id/ApiAccount/ChangeProfile'
};

var ResendConfirmationEmailMobileConfig = {
    Url: 'http://m.local.travorama.com/id/ApiAccount/ResendConfirmationEmail'
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
    return true;
    if (token) {
        return true;
    }
    else
    {
        console.log('Test');

        if (refreshToken != null) {
            var xhttp = new XMLHttpRequest();
            xhttp.open("POST", LoginConfig.Url, true);
            //xhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
            xhttp.send("refreshtoken:"+getCookie('refreshtoken'));
            if (xhttp.status == "200") {
                return true;
            }
            else
            {
                return false;
            }
            //return true;
            
            
        }
        else
        {
            return true;
        }
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
