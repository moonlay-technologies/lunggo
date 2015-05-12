//******************************************
// Variables
var SystemConfig = {
    SystemBusy: false
};

var SearchHotelConfig = {
    Url: 'http://travorama-apidev.azurewebsites.net/api/v1/hotels',
    ResultCount: 24
};

var SearchRoomConfig = {
    Url: 'http://travorama-apidev.azurewebsites.net/api/v1/rooms'
};

var FlightSearchConfig = {
    Url: 'http://travorama-apidev.azurewebsites.net/api/v1/flights'
};

var RevalidateConfig = {
    Url: 'http://travorama-apidev.azurewebsites.net/api/v1/flights/revalidate',
    working: false
};

var HotelAutocompleteConfig = {
    Url: 'http://travorama-apidev.azurewebsites.net/api/v1/autocomplete/hotellocation'
};

var FlightAutocompleteConfig = {
    Url: 'http://travorama-apidev.azurewebsites.net/api/v1/autocomplete/airport'
};