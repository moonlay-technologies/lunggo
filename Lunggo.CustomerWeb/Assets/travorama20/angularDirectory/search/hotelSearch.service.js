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
        $log.debug('using search service. going to hotel search...');
        var param = searchParam(hotelSearch);
        if (param != false) {
            location.href = '/id/Hotel/Search/' + param;
        }
    };

    factory.initializeSearchForm = function (scope, searchParamObject) {
        scope.hotelSearch = {};
        scope.hotelSearch.searchHotelType = { "location": 'Location', searchId: 'SearchId' };

        scope.hotelSearch.filter = {};
        scope.hotelSearch.filter.nameFilter = "";
        scope.hotelSearch.filter.minPrice = 0;
        scope.hotelSearch.filter.maxPrice = 0;
        scope.hotelSearch.filter.zones = null;
        scope.hotelSearch.filter.stars = null;
        scope.hotelSearch.filter.facilities = null;

        scope.hotelSearch.sortByType = { "ascendingPrice": "ASCENDINGPRICE", "descendingPrice": "DESCENDINGPRICE" };
        scope.hotelSearch.sortBy = scope.hotelSearch.sortByType.ascendingPrice;
        scope.hotelSearch.page = 1;
        scope.hotelSearch.perPage = 20;


        if (searchParamObject != null && searchParamObject !== undefined) {
            scope.hotelSearch.location = searchParamObject.location;
            scope.hotelSearch.locationDisplay = "";

            scope.hotelSearch.checkinDate = searchParamObject.checkinDate;
            scope.hotelSearch.checkoutDate = searchParamObject.checkoutDate;
            scope.hotelSearch.destinationCheckinDate = searchParamObject.checkinDate;
            scope.hotelSearch.destinationCheckoutDate = searchParamObject.checkoutDate;

            scope.hotelSearch.adultCount = searchParamObject.adultCount != null ? searchParamObject.adultCount : 1;
            scope.hotelSearch.childCount = searchParamObject.childCount != null ? searchParamObject.childCount : 0;

            scope.hotelSearch.nightCount = searchParamObject.nightCount != null ? searchParamObject.nightCount : 1;
            scope.hotelSearch.destinationNightCount = searchParamObject.nightCount != null ? searchParamObject.nightCount : 1;

            scope.hotelSearch.roomCount = searchParamObject.roomCount != null ? searchParamObject.roomCount : 2;
            scope.hotelSearch.childrenAges = searchParamObject.childrenAges != null ? searchParamObject.childrenAges : [0, 0, 0, 0];
        }
        else {
            scope.hotelSearch.location = null;
            scope.hotelSearch.locationDisplay = "";
            scope.hotelSearch.checkinDate = moment().locale("id").add(5, 'days');
            scope.hotelSearch.checkoutDate = moment().locale("id").add(7, 'days');
            scope.hotelSearch.destinationCheckinDate = scope.hotelSearch.checkinDate;
            scope.hotelSearch.destinationCheckoutDate = scope.hotelSearch.checkoutDate;
            scope.hotelSearch.checkinDateDisplay = scope.hotelSearch.checkinDate.locale("id").format('LL');

            scope.hotelSearch.adultCount = 1;
            scope.hotelSearch.childCount = 0;

            scope.hotelSearch.nightCount = 2;
            scope.hotelSearch.destinationNightCount = 2;
            scope.hotelSearch.roomCount = 1;
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
        
        scope.hotelSearch.getHotels = function (param) {
            factory.gotoHotelSearch(this);

            //var pagination = { 'sortBy': this.sortBy, 'page': this.page, 'perPage' : this.perPage };
            //factory.searchHotel(this, this.filter, pagination);
        }


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

        scope.setCheckinDate = function (scopeElement, date) {
            scopeElement.$apply(function () {
                scopeElement.hotelSearch.checkinDate = moment(date).toISOString();
                scopeElement.hotelSearch.checkoutDate = moment(date).add(scopeElement.hotelSearch.nightCount, 'days').toISOString();
                //scopeElement.hotelSearch.checkinDateDisplay = scope.hotelSearch.checkinDate.locale("id").format('LL');
            });
        }
        $('.hotel-date-picker').datepicker({
            numberOfMonths: 2,
            onSelect: function (date) {
                date = date.substring(3, 5) + "/" + date.substring(0, 2) + "/" + date.substring(6, 10);
                //console.log(data);
                //$scope.setCheckinDate(data);

                var scopeElement = angular.element($('.hotel-date-picker')).scope();
                scope.setCheckinDate(scopeElement, date);

                $log.debug("checkinDate = " + date);
                var target;
                var chosenDate = new Date(date);
                $(target + ' .date').html(('0' + chosenDate.getDate()).slice(-2));
                $(target + ' .month').html(translateMonth(chosenDate.getMonth()));
                $(target + ' .year').html(chosenDate.getFullYear());
                $('.search-calendar-hotel').hide();
                var cd = new Date(date);
                var checkoutDate = new Date(cd.setDate(cd.getDate() + scope.hotelSearch.nightCount));
                var dd = checkoutDate.getDate();
                var mm = checkoutDate.getMonth() + 1;
                var yyyy = checkoutDate.getFullYear();
                var d = yyyy + '-' + mm + '-' + dd;
                scope.hotelSearch.checkoutDate = moment(checkoutDate, "MM-DD-YYYY");
                $log.debug("checkout date = " + scope.hotelSearch.checkoutDate);
            }
        });


    }

    var searchParam = function (hotelSearch) {
        if (hotelSearch.location == null || hotelSearch.location.length == 0) {
            //$scope.wrongParam = true;
            alert("Silakan pilih lokasi atau hotel dari daftar yang tersedia");
            //return false;
        }


        return "?info=" + [
            hotelSearch.searchHotelType.location,
            //hotelSearch.location,
            16162,
            moment(hotelSearch.checkinDate).format("YYYY-MM-DD"),
            moment(hotelSearch.checkoutDate).format("YYYY-MM-DD"),
            hotelSearch.adultCount,
            hotelSearch.childCount,
            hotelSearch.nightCount,
            hotelSearch.roomCount,
            hotelSearch.childrenAges
        ].join('.');
    };

    return factory;
}]);

