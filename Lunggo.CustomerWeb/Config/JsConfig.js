//******************************************
// Variables
var SystemConfig = {
    SystemBusy: false
};

var SearchHotelConfig = {
    Url: 'http://dv1-api.azurewebsites.net/api/v1/hotels',
    ResultCount: 24
};

var SearchRoomConfig = {
    Url: 'http://dv1-api.azurewebsites.net/api/v1/rooms'
};

var FlightSearchConfig = {
    Url: 'http://dv1-api.azurewebsites.net/api/v1/flights'
};

var RevalidateConfig = {
    Url: 'http://dv1-api.azurewebsites.net/api/v1/flights/revalidate',
    working: false
};

var HotelAutocompleteConfig = {
    Url: 'http://dv1-api.azurewebsites.net/api/v1/autocomplete/hotellocation'
};

var FlightAutocompleteConfig = {
    Url: 'http://dv1-api.azurewebsites.net/api/v1/autocomplete/airport'
};