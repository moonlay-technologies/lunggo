//******************************************
// Variables
var SystemConfig = {
    SystemBusy: false
};

var SearchHotelConfig = {
    Url: 'https://dv2-api.azurewebsites.net/api/v1/hotels',
    ResultCount: 24
};

var SearchRoomConfig = {
    Url: 'https://dv2-api.azurewebsites.net/api/v1/rooms'
};

var FlightSearchConfig = {
    Url: 'https://dv2-api.azurewebsites.net/api/v1/flights',
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
            var passenger = [ params.adult, (params.child || 0), (params.infant || 0) ];
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

var RevalidateConfig = {
    Url: 'https://dv2-api.azurewebsites.net/api/v1/flights/revalidate',
    working: false
};

var FlightBookConfig = {
    Url: 'https://dv2-api.azurewebsites.net/api/v1/flights/book',
    working: false
};

var FlightPayConfig = {
    Url: 'https://dv2-api.azurewebsites.net/api/v1/flights/pay',
    working: false
};

var GetRulesConfig = {
    Url: 'https://dv2-api.azurewebsites.net/api/v1/flights/rules',
    working: false
};

var HotelAutocompleteConfig = {
    Url: 'https://dv2-api.azurewebsites.net/api/v1/autocomplete/hotellocation/'
};

var FlightAutocompleteConfig = {
    Url: 'https://dv2-api.azurewebsites.net/api/v1/autocomplete/airport/'
};

var AirlineAutocompleteConfig = {
    Url: 'https://dv2-api.azurewebsites.net/api/v1/autocomplete/airline/'
};

var CheckVoucherConfig = {
    Url: 'https://dv2-api.azurewebsites.net/api/v1/voucher/check'
};

var SubscribeConfig = {
    Url: 'https://dv2-api.azurewebsites.net/api/v1/newsletter/subscribe'
};

var RegisterConfig = {
    Url: 'http://dv2-cw.azurewbsites.net/id/ApiAccount/Register'
};

var ResetPasswordConfig = {
    Url: 'http://dv2-cw.azurewbsites.net/id/ApiAccount/ResetPassword'
};

var ForgotPasswordConfig = {
    Url: 'http://dv2-cw.azurewbsites.net/id/ApiAccount/ForgotPassword'
};

var ChangePasswordConfig = {
    Url: 'http://dv2-cw.azurewbsites.net/id/ApiAccount/ChangePassword'
};

var ChangeProfileConfig = {
    Url: 'http://dv2-cw.azurewbsites.net/id/ApiAccount/ChangeProfile'
};

var ResendConfirmationEmailConfig = {
    Url: 'http://dv2-cw.azurewbsites.net/id/ApiAccount/ResendConfirmationEmail'
};

var VeritransTokenConfig = {
    Url: 'https://api.sandbox.veritrans.co.id/v2/token',
    ClientKey: 'VT-client-J8i9AzRyIU49D_v3'
};

var TransferConfig = {
    Url: 'https://dv2-api.azurewebsites.net/api/v1/transferidentifier'
};