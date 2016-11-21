angular.module('travorama')
app.factory('searchService', ['$log', function ($log) {

    var factory = {};

    factory.gotoHotelSearch = function (hotel) {
        $log.debug('using search service. searching...');
        var param = searchParam(hotel);
        location.href = '/id/Hotel/Search/' + param;
    };

    var searchParam = function (hotel) {
        if (hotel.location == null || hotel.location.length == 0) {
            //$scope.wrongParam = true;
            alert("Silakan pilih lokasi atau hotel dari daftar yang tersedia");
        }

        return "?info=" + [
            hotel.searchHotelType,
            hotel.location,
            hotel.checkinDate.format("YYYY-MM-DD"),
            hotel.checkoutDate.format("YYYY-MM-DD"),
            hotel.adultCount,
            hotel.childCount,
            hotel.nightCount,
            hotel.roomCount,
            hotel.childrenAges
        ].join('.');
    };

    return factory;
}]);

