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
    Url: '$apiUrl$$flightPath$'
};

var RevalidateConfig = {
    Url: '$apiUrl$$flightRevalidatePath$',
    working: false
};

var FlightBookConfig = {
    Url: '$apiUrl$$flightBookPath$',
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

var ResetPasswordConfig = {
    Url: 'http://localhost:23321/id/ApiAccount/ResetPassword'
};

var ForgotPasswordConfig = {
    Url: 'http://localhost:23321/id/ApiAccount/ForgotPassword'
};

var ChangePasswordConfig = {
    Url: 'http://localhost:23321/id/ApiAccount/ChangePassword'
};

var ChangeProfileConfig = {
    Url: 'http://localhost:23321/id/ApiAccount/ChangeProfile'
};