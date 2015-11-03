//******************************************
// Variables
var SystemConfig = {
    SystemBusy: false
};

var SearchHotelConfig = {
    Url: 'http://localhost:1147/api/v1/hotels',
    ResultCount: 24
};

var SearchRoomConfig = {
    Url: 'http://localhost:1147/api/v1/rooms'
};

var FlightSearchConfig = {
    Url: 'http://localhost:1147/api/v1/flights'
};

var RevalidateConfig = {
    Url: 'http://localhost:1147/api/v1/flights/revalidate',
    working: false
};

var FlightBookConfig = {
    Url: 'http://localhost:1147/api/v1/flights/book',
    working: false
};

var GetRulesConfig = {
    Url: 'http://localhost:1147/api/v1/flights/rules',
    working: false
};

var HotelAutocompleteConfig = {
    Url: 'http://localhost:1147/api/v1/autocomplete/hotellocation/'
};

var FlightAutocompleteConfig = {
    Url: 'http://localhost:1147/api/v1/autocomplete/airport/'
};

var AirlineAutocompleteConfig = {
    Url: 'http://localhost:1147/api/v1/autocomplete/airline/'
};

var CheckVoucherConfig = {
    Url: 'http://localhost:1147/api/v1/voucher/check'
};

var SubscribeConfig = {
    Url: 'http://localhost:1147/api/v1/newsletter/subscribe'
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