angular.module('travorama').factory('hotelSearchSvc', ['$log', '$resource', '$timeout', function ($log, $resource, $timeout) {
    var factory = {};

    factory.resource = $resource(HotelSearchConfig.Url,
            {},
            {
                post: {
                    method: 'POST',
                    //params: $scope.Hotel,
                    isArray: false
                }
            }
        );



    factory.searchHotel = function (hotelSearch, filter, pagination) {
        $log.debug('using search service. searching...');
        return factory.resource.post({}, {
            "searchHotelType": hotelSearch.searchId == null ? hotelSearch.searchHotelType.location : hotelSearch.searchHotelType.searchId,
            "searchId": hotelSearch.searchId,

            "location": hotelSearch.location,
            "checkinDate": hotelSearch.checkinDate,
            "checkoutDate": moment(hotelSearch.checkinDate).add(hotelSearch.nightCount, 'days').format("YYYY-MM-DD"),
            "nightCount": hotelSearch.nightCount,
            "occupancies":
                [{
                    "adultCount": hotelSearch.adultCount,
                    "childCount": hotelSearch.childCount,
                    "roomCount": hotelSearch.roomCount,
                    "childrenAges": hotelSearch.childrenAges
                }],
            "hotelFilter":
            {
                "priceFilter":
                {
                    "minPrice": filter.minPrice,
                    "maxPrice": filter.maxPrice
                },
                "zoneFilter": filter.zones,
                "starFilter": filter.stars,
                "facilityFilter": filter.facilities,
                "nameFilter": filter.nameFilter
            },
            "hotelSorting": pagination.sortBy,
            "page": pagination.page,
            "perPage": pagination.perPage,
            "regsId": null
        });
    };

    factory.gotoHotelSearch = function (hotelSearch) {
        $log.debug('using search service. searching...');
        var param = searchParam(hotelSearch);
        location.href = '/id/Hotel/Search/' + param;
    };

    factory.initializeSearchForm = function (scope, searchParamObject) {
        scope.hotelSearch = {};
        scope.hotelSearch.searchHotelType = { "location": 'Location', searchId: 'SearchId' };

        if (searchParamObject != null && searchParamObject !== undefined) {
            scope.hotelSearch.location = searchParamObject.location;
            scope.hotelSearch.locationDisplay = "";
            scope.hotelSearch.checkinDate = searchParamObject.checkinDate;
            scope.hotelSearch.checkoutDate = searchParamObject.checkoutDate;

            scope.hotelSearch.adultCount = searchParamObject.adultCount != null ? searchParamObject.adultCount : 1;
            scope.hotelSearch.childCount = searchParamObject.childCount != null ? searchParamObject.childCount : 0;

            scope.hotelSearch.nightCount = searchParamObject.nightCount != null ? searchParamObject.nightCount : 1;
            scope.hotelSearch.roomCount = searchParamObject.roomCount != null ? searchParamObject.roomCount : 2;
            scope.hotelSearch.childrenAges = searchParamObject.childrenAges != null ? searchParamObject.childrenAges : [0, 0, 0, 0];
        }
        else {
            scope.hotelSearch.location = null;
            scope.hotelSearch.locationDisplay = "";
            scope.hotelSearch.checkinDate = "11-11-2017";
            scope.hotelSearch.checkoutDate = "15-15-2017";

            scope.hotelSearch.adultCount = 1;
            scope.hotelSearch.childCount = 0;

            scope.hotelSearch.nightCount = 1;
            scope.hotelSearch.roomCount = 2;
            scope.hotelSearch.childrenAges = [0, 0, 0, 0];
        }

        scope.hotelSearch.adultCountMin = 1;
        scope.hotelSearch.adultCountMax = 5;

        scope.hotelSearch.childCountMin = 0;
        scope.hotelSearch.childCountMax = 4;

        scope.hotelSearch.nightCountMin = 1;
        scope.hotelSearch.nightCountMax = 7;

        scope.hotelSearch.roomCountMin = 1;
        scope.hotelSearch.roomCountMax = 8;

        //$scope.hotel.searchId = $scope.model.searchId;
        //$scope.hotelSearch.location = model.searchParamObject.location;
        //$scope.hotelSearch.checkinDate = model.searchParamObject.checkinDate;
        //$scope.hotelSearch.checkoutDate = model.searchParamObject.checkoutDate;
        //$scope.hotelSearch.adultCount = model.searchParamObject.adultCount;
        //$scope.hotelSearch.childCount = model.searchParamObject.childCount;
        //$scope.hotelSearch.nightCount = new Date($scope.hotelSearch.checkoutDate).getDate() - new Date($scope.hotelSearch.checkinDate).getDate();
        //$scope.hotelSearch.roomCount = model.searchParamObject.roomCount;
        //$scope.hotelSearch.childrenAges = model.searchParamObject.childrenAges;
        //$scope.hotelSearch.searchParamObject = model.searchParamObject;
        //$scope.hotelSearch.searchParam = model.searchParam;


        scope.selectLocation = function (location) {
            scope.hotelSearch.location = location.id;
            scope.hotelSearch.locationDisplay = location.name;
            scope.view.showHotelSearch = false;
        }

        scope.setChildAge = function (index, age) {
            scope.hotelSearch.childrenAges[index] = age;
        }

        scope.addValue = function (variableName, amount) {
            if (variableName == 'adultCount') {
                scope.hotelSearch.adultCount = scope.hotelSearch.adultCount + amount;
                if (scope.hotelSearch.adultCount < scope.hotelSearch.adultCountMin) scope.hotelSearch.adultCount++;
                else if (scope.hotelSearch.adultCount > scope.hotelSearch.adultCountMax) scope.hotelSearch.adultCount--;
            }
            else if (variableName == 'childCount') {
                scope.hotelSearch.childCount = scope.hotelSearch.childCount + amount;
                if (scope.hotelSearch.childCount < scope.hotelSearch.childCountMin) scope.hotelSearch.childCount++;
                else if (scope.hotelSearch.childCount > scope.hotelSearch.childCountMax) scope.hotelSearch.childCount--;
            }
            else if (variableName == 'nightCount') {
                scope.hotelSearch.nightCount = scope.hotelSearch.nightCount + amount;
                if (scope.hotelSearch.nightCount < scope.hotelSearch.nightCountMin) scope.hotelSearch.nightCount++;
                else if (scope.hotelSearch.nightCount > scope.hotelSearch.nightCountMax) scope.hotelSearch.nightCount--;
            }
            else if (variableName == 'roomCount') {
                scope.hotelSearch.roomCount = scope.hotelSearch.roomCount + amount;
                if (scope.hotelSearch.roomCount < scope.hotelSearch.roomCountMin) scope.hotelSearch.roomCount++;
                else if (scope.hotelSearch.roomCount > scope.hotelSearch.roomCountMax) scope.hotelSearch.roomCount--;
            }
        }

        scope.$watch('hotelSearch.locationDisplay', function (newValue, oldValue) {
            if (newValue.length >= 3) {
                scope.hotelSearch.autocompleteResource.get({ prefix: newValue }).$promise.then(function (data) {
                    $timeout(function () {
                        scope.hotelSearch.hotelAutocomplete = data.hotelAutocomplete;
                        $log.debug(scope.hotelSearch.hotelAutocomplete);
                    }, 0);
                });
            };
        });

        scope.hotelSearch.autocompleteResource = $resource(HotelAutocompleteConfig.Url + '/:prefix',
            { prefix: '@prefix' },
            {
                get: {
                    method: 'GET',
                    params: {},
                    isArray: false
                }
            }
        );
    }

    var searchParam = function (hotelSearch) {
        if (hotelSearch.location == null || hotelSearch.location.length == 0) {
            //$scope.wrongParam = true;
            alert("Silakan pilih lokasi atau hotel dari daftar yang tersedia");
        }

        return "?info=" + [
            hotelSearch.searchHotelType,
            hotelSearch.location,
            hotelSearch.checkinDate.format("YYYY-MM-DD"),
            hotelSearch.checkoutDate.format("YYYY-MM-DD"),
            hotelSearch.adultCount,
            hotelSearch.childCount,
            hotelSearch.nightCount,
            hotelSearch.roomCount,
            hotelSearch.childrenAges
        ].join('.');
    };

    return factory;
}]);

