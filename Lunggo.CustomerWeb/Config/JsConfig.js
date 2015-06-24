//******************************************
// Variables
var SystemConfig = {
    SystemBusy: false
};

var SearchHotelConfig = {
    Url: 'http://prod-api.azurewebsites.net/api/v1/hotels',
    ResultCount: 24
};

var SearchRoomConfig = {
    Url: 'http://prod-api.azurewebsites.net/api/v1/rooms'
};

var FlightSearchConfig = {
    Url: 'http://prod-api.azurewebsites.net/api/v1/flights'
};

var RevalidateConfig = {
    Url: 'http://prod-api.azurewebsites.net/api/v1/flights/revalidate',
    working: false
};

var GetRulesConfig = {
    Url: 'http://prod-api.azurewebsites.net/api/v1/flights/rules',
    working: false
};

var HotelAutocompleteConfig = {
    Url: 'http://prod-api.azurewebsites.net/api/v1/autocomplete/hotellocation/'
};

var FlightAutocompleteConfig = {
    Url: 'http://prod-api.azurewebsites.net/api/v1/autocomplete/airport/'
};