//******************************************
// Variables
var SystemConfig = {
    SystemBusy: false
};

var SearchHotelConfig = {
    Url: 'https://api.local.travorama.com/api/v1/hotels',
    ResultCount: 24
};

var SearchRoomConfig = {
    Url: 'https://api.local.travorama.com/api/v1/rooms'
};

var FlightSearchConfig = {
    Url: 'https://api.local.travorama.com/api/v1/flights'
};

var RevalidateConfig = {
    Url: 'https://api.local.travorama.com/api/v1/flights/revalidate',
    working: false
};

var FlightBookConfig = {
    Url: 'https://api.local.travorama.com/api/v1/flights/book',
    working: false
};

var GetRulesConfig = {
    Url: 'https://api.local.travorama.com/api/v1/flights/rules',
    working: false
};

var HotelAutocompleteConfig = {
    Url: 'https://api.local.travorama.com/api/v1/autocomplete/hotellocation/'
};

var FlightAutocompleteConfig = {
    Url: 'https://api.local.travorama.com/api/v1/autocomplete/airport/'
};

var AirlineAutocompleteConfig = {
    Url: 'https://api.local.travorama.com/api/v1/autocomplete/airline/'
};

var CheckVoucherConfig = {
    Url: 'https://api.local.travorama.com/api/v1/voucher/check'
};

var SubscribeConfig = {
    Url: 'https://api.local.travorama.com/api/v1/newsletter/subscribe'
};

var RegisterConfig = {
    Url: 'http://local.travorama.com/id/ApiAccount/Register'
};

var ResetPasswordConfig = {
    Url: 'http://local.travorama.com/id/ApiAccount/ResetPassword'
};

var ForgotPasswordConfig = {
    Url: 'http://local.travorama.com/id/ApiAccount/ForgotPassword'
};

var ChangePasswordConfig = {
    Url: 'http://local.travorama.com/id/ApiAccount/ChangePassword'
};

var ChangeProfileConfig = {
    Url: 'http://local.travorama.com/id/ApiAccount/ChangeProfile'
};

var ResendConfirmationEmailConfig = {
    Url: 'http://local.travorama.com/id/ApiAccount/ResendConfirmationEmail'
};