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

var RegisterConfig = {
    Url: '$rootUrl$$registerPath$'
};

var ResetPasswordConfig = {
    Url: '$rootUrl$$resetPasswordPath$'
};

var ForgotPasswordConfig = {
    Url: '$rootUrl$$forgotPasswordPath$'
};

var ChangePasswordConfig = {
    Url: '$rootUrl$$changePasswordPath$'
};

var ChangeProfileConfig = {
    Url: '$rootUrl$$changeProfilePath$'
};

var ResendConfirmationEmailConfig = {
    Url: '$rootUrl$$resendConfirmationEmailPath$'
};

var VeritransTokenConfig = {
    Url: '$veritransTokenPath$',
    ClientKey: '$veritransClientKey$'
};

var TransferConfig = {
    Url: '$apiUrl$$transferPaymentPath$'
};