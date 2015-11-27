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
    Url: 'http://qa.travorama.com/id/ApiAccount/Register'
};

var ResetPasswordConfig = {
    Url: 'http://qa.travorama.com/id/ApiAccount/ResetPassword'
};

var ForgotPasswordConfig = {
    Url: 'http://qa.travorama.com/id/ApiAccount/ForgotPassword'
};

var ChangePasswordConfig = {
    Url: 'http://qa.travorama.com/id/ApiAccount/ChangePassword'
};

var ChangeProfileConfig = {
    Url: 'http://qa.travorama.com/id/ApiAccount/ChangeProfile'
};

var ResendConfirmationEmailConfig = {
    Url: 'http://qa.travorama.com/id/ApiAccount/ResendConfirmationEmail'
};