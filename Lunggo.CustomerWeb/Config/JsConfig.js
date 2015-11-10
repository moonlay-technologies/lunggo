//******************************************
// Variables
var SystemConfig = {
    SystemBusy: false
};

var SearchHotelConfig = {
    Url: 'http://travorama-qa-api.azurewebsites.net/api/v1/hotels',
    ResultCount: 24
};

var SearchRoomConfig = {
    Url: 'http://travorama-qa-api.azurewebsites.net/api/v1/rooms'
};

var FlightSearchConfig = {
    Url: 'http://travorama-qa-api.azurewebsites.net/api/v1/flights'
};

var RevalidateConfig = {
    Url: 'http://travorama-qa-api.azurewebsites.net/api/v1/flights/revalidate',
    working: false
};

var FlightBookConfig = {
    Url: 'http://travorama-qa-api.azurewebsites.net/api/v1/flights/book',
    working: false
};

var GetRulesConfig = {
    Url: 'http://travorama-qa-api.azurewebsites.net/api/v1/flights/rules',
    working: false
};

var HotelAutocompleteConfig = {
    Url: 'http://travorama-qa-api.azurewebsites.net/api/v1/autocomplete/hotellocation/'
};

var FlightAutocompleteConfig = {
    Url: 'http://travorama-qa-api.azurewebsites.net/api/v1/autocomplete/airport/'
};

var AirlineAutocompleteConfig = {
    Url: 'http://travorama-qa-api.azurewebsites.net/api/v1/autocomplete/airline/'
};

var CheckVoucherConfig = {
    Url: 'http://travorama-qa-api.azurewebsites.net/api/v1/voucher/check'
};

var SubscribeConfig = {
    Url: 'http://travorama-qa-api.azurewebsites.net/api/v1/newsletter/subscribe'
};

var RegisterConfig = {
    Url: 'http://travorama-qa-cw.azurewebsites.net/id/ApiAccount/Register'
};

var ResetPasswordConfig = {
    Url: 'http://travorama-qa-cw.azurewebsites.net/id/ApiAccount/ResetPassword'
};

var ForgotPasswordConfig = {
    Url: 'http://travorama-qa-cw.azurewebsites.net/id/ApiAccount/ForgotPassword'
};

var ChangePasswordConfig = {
    Url: 'http://travorama-qa-cw.azurewebsites.net/id/ApiAccount/ChangePassword'
};

var ChangeProfileConfig = {
    Url: 'http://travorama-qa-cw.azurewebsites.net/id/ApiAccount/ChangeProfile'
};

var ResendConfirmationEmailConfig = {
    Url: 'http://travorama-qa-cw.azurewebsites.net/id/ApiAccount/ResendConfirmationEmail'
};