app.factory('hotelSearchSvc', ['$log', '$resource', '$timeout', function ($log, $resource, $timeout) {
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
        var searchtype = '';
        if (hotelSearch.searchId == null) {
            if (hotelSearch.locationType == 'Hotel') {
                searchtype = hotelSearch.searchHotelType.hotel;
            } else {
                searchtype = hotelSearch.searchHotelType.location;
            }
        } else {
            searchtype = hotelSearch.searchHotelType.searchId;
        }
        return factory.resource.post({}, {
            "searchHotelType": hotelSearch.searchHotelType.location,
            "searchId": hotelSearch.searchId,

            "location": hotelSearch.location,
            //"hotelCd": hotelSearch.locationType == "Hotel" ? hotelSearch.location : 0,
            "checkinDate": hotelSearch.checkinDate,
            "checkoutDate": moment(hotelSearch.checkinDate).add(hotelSearch.nightCount, 'days').format("YYYY-MM-DD"),
            "nightCount": hotelSearch.nightCount,
            "occupancies": hotelSearch.occupancies,
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
        if (hotelSearch.locationType == 'Hotel') {
            hotelSearch.searchinghotel = true;
            var filter = {
                nameFilter: "",
                minPrice: 0,
                maxPrice: 0,
                zones: [],
                stars: null,
                facilities: null,
            };
             var sortByType = {
                "ascendingPrice": "ASCENDINGPRICE", "descendingPrice": "DESCENDINGPRICE",
                "ascendingStar": "ASCENDINGSTAR", "descendingStar": "DESCENDINGSTAR"
            };
            var sortBy = sortByType.ascendingPrice;

            var page = 1;
            var perPage = 20;
            var pagination = { 'sortBy': sortBy, 'page': page, 'perPage': perPage };
            hotelSearch.occupancies = hotelSearch.occupancies.splice(0, hotelSearch.roomCount);
            for (var x = 0; x < hotelSearch.occupancies.length; x++) {
                if (hotelSearch.occupancies[x].childCount == 0) {
                    hotelSearch.occupancies[x].childrenAges = [0, 0, 0, 0];
                } else {
                    hotelSearch.occupancies[x].childrenAges.splice(0, hotelSearch.occupancies[x].childCount);
                    
                }
                hotelSearch.occupancies[x].roomCount = 1;
            }
            factory.searchHotel(hotelSearch, filter, pagination).$promise.then(function (data) {
                hotelSearch.searchinghotel = false;
                if (data == null) {
                    alert('Mohon maaf. Hotel ini telah terisi penuh.');
                } else {
                    if (data.hotels == null || data.hotels.length == 0) {
                        alert('Mohon maaf. Hotel ini telah terisi penuh.');
                    } else {
                       
                        $log.debug('redirect to detail hotel with hotelCd: ' + data.hotels[0].hotelCd);
                        window.location.href ='/id/Hotel/DetailHotel?' +
                            "searchId=" + data.searchId + "&" +
                            "hotelCd=" + data.hotels[0].hotelCd + "&" +
                            "searchParam=" + searchParam(hotelSearch);
                        
                    }
                }
            });

            
        } else {
            var param = searchParam(hotelSearch);
            $log.debug(param);
            if (param != false) {
                location.href = '/id/Hotel/Search/' + param;
            }
        }
        
    };

    //factory.myarray = [];
    //factory.getNumber = function (num) {
    //    factory.myarray = [];

    //    for (var i = 0; i <= num - 1; i++) {
    //        factory.myarray.push(i);
    //    }

    //}
    factory.initializeSearchForm = function (scope, searchParamObject) {
        scope.hotelSearch = {};
        scope.changeSearch = {
            occupancies : []
        };
        scope.hotelSearch.searchHotelType = { "location": 'Location', searchId: 'SearchId' , hotel:'HotelCode'};
        scope.hotelSearch.searchinghotel = false;
        scope.filter = {};
        scope.filter.nameFilter = "";
        scope.filter.minPrice = 0;
        scope.filter.maxPrice = 0;
        scope.filter.zones = null;
        scope.filter.stars = null;
        scope.filter.facilities = null;

        scope.hotelSearch.sortByType = {
            "ascendingPrice": "ASCENDINGPRICE", "descendingPrice": "DESCENDINGPRICE",
            "ascendingStar": "ASCENDINGSTAR", "descendingStar": "DESCENDINGSTAR"
        };
        scope.hotelSearch.sortBy = scope.hotelSearch.sortByType.ascendingPrice;
        scope.hotelSearch.page = 1;
        scope.hotelSearch.perPage = 20;
        scope.hotelSearch.occupancies = [];
        scope.hotelSearch.locationType = '';
        var defaultValue = {
            locationCode: 16084,
            locationDisplay: "Bali, Indonesia",
            checkinDate: moment().locale("id").add(5, 'days'),
            checkoutDate: moment().locale("id").add(7, 'days'),

            adultCount :1,
            childCount: 0,
            nightCount: 2,
            destinationNightCount: 2,
            roomCount: 1,
            childrenAges : [0,0,0,0]
        }
        
        if (searchParamObject != null && searchParamObject !== undefined) {
            scope.hotelSearch.location = searchParamObject.location;
            scope.hotelSearch.locationDisplay = "";

            $log.debug("scope.hotelSearch.checkinDate = " + scope.hotelSearch.checkinDate);
            scope.hotelSearch.checkinDate = searchParamObject.checkinDate;
            $log.debug("scope.hotelSearch.checkinDate = " + scope.hotelSearch.checkinDate);
            scope.hotelSearch.checkoutDate = searchParamObject.checkoutDate;
            scope.hotelSearch.checkinDateDisplay = moment(searchParamObject.checkinDate).locale("id").format('LL');
            scope.hotelSearch.checkoutDateDisplay = moment(searchParamObject.checkoutDate).locale("id").format('LL');

            scope.hotelSearch.destinationCheckinDate = moment(searchParamObject.checkinDate).locale("id").format('LL');
            scope.hotelSearch.destinationCheckoutDate = moment(searchParamObject.checkoutDate).locale("id").format('LL');

            scope.hotelSearch.nightCount = searchParamObject.nightCount != null ? searchParamObject.nightCount : 2;
            scope.hotelSearch.destinationNightCount = searchParamObject.nightCount != null ? searchParamObject.nightCount : 2;

            scope.hotelSearch.roomCount = searchParamObject.roomCount != null ? searchParamObject.roomCount : 2;

            scope.hotelSearch.childrenAges = defaultValue.childrenAges;
            scope.hotelSearch.occupancies = searchParamObject.occupancies;
        }
        else {
            scope.hotelSearch.location = defaultValue.locationCode;
            scope.hotelSearch.locationDisplay = defaultValue.locationDisplay;
            $log.debug("scope.hotelSearch.checkinDate = " + scope.hotelSearch.checkinDate);

            scope.hotelSearch.checkinDate = defaultValue.checkinDate;
            $log.debug("scope.hotelSearch.checkinDate = " + scope.hotelSearch.checkinDate);
            scope.hotelSearch.checkoutDate = defaultValue.checkoutDate;
            scope.hotelSearch.destinationCheckinDate = scope.hotelSearch.checkinDate.locale("id").format('LL');
            scope.hotelSearch.destinationCheckoutDate = scope.hotelSearch.checkoutDate.locale("id").format('LL');
            
            scope.hotelSearch.checkinDateDisplay = scope.hotelSearch.checkinDate.locale("id").format('LL');
            scope.hotelSearch.checkoutDateDisplay = scope.hotelSearch.checkoutDate.locale("id").format('LL');

            scope.hotelSearch.adultCount = defaultValue.adultCount;
            scope.hotelSearch.childCount = defaultValue.childCount;
            scope.hotelSearch.nightCount = defaultValue.nightCount;
            scope.hotelSearch.destinationNightCount = defaultValue.nightCount;
            scope.hotelSearch.roomCount = defaultValue.roomCount;
            scope.hotelSearch.childrenAges = defaultValue.childrenAges;
            for (var i = 0; i <= 7; i++) {
                scope.hotelSearch.occupancies.push({
                    adultCount: 1,
                    childCount: 0,
                    childrenAges: [0, 0, 0, 0]
                });
            }
            
        }
        //scope.changeSearch = scope.hotelSearch;
        
        scope.hotelSearch.adultCountMin = 1;
        scope.hotelSearch.adultCountMax = 5;
        scope.hotelSearch.adultrange = [1, 2, 3, 4, 5];
        scope.hotelSearch.childrange = [0, 1, 2, 3, 4];
        scope.hotelSearch.roomrange = [1,2,3,4,5,6,7,8];
        scope.hotelSearch.nightrange = [1, 2, 3, 4, 5, 6, 7];
        scope.hotelSearch.childagerange = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11];
        scope.hotelSearch.childCountMin = 0;
        scope.hotelSearch.childCountMax = 4;

        scope.hotelSearch.nightCountMin = 1;
        scope.hotelSearch.nightCountMax = 7;

        scope.hotelSearch.roomCountMin = 1;
        scope.hotelSearch.roomCountMax = 8;

        scope.hotelSearch.totalAdult = 0;
        scope.hotelSearch.totalChildren = 0;
        scope.hotelSearch.getHotels = function (param) {
            factory.gotoHotelSearch(this);

            //var pagination = { 'sortBy': this.sortBy, 'page': this.page, 'perPage' : this.perPage };
            //factory.searchHotel(this, this.filter, pagination);
        }

        
        scope.selectLocation = function (location) {
            scope.hotelSearch.location = location.id;
            scope.hotelSearch.locationDisplay = location.name;
            scope.view.showHotelSearch = false;
            scope.hotelSearch.locationType = location.type;
        }

        factory.selectLocation = function (location) {
            scope.hotelSearch.location = location.id;
            scope.hotelSearch.locationDisplay = location.name;
            scope.view.showHotelSearch = false;
            scope.hotelSearch.locationType = location.type;
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

       factory.getLocation = function (newValue) {
            if (newValue.length >= 3) {
                scope.hotelSearch.autocompleteResource.get({ prefix: newValue }).$promise.then(function (data) {
                    $timeout(function () {
                        scope.hotelSearch.hotelAutocomplete = data.hotelAutocomplete;
                        $log.debug(scope.hotelSearch.hotelAutocomplete);
                    }, 0);
                });
            };
        };

        scope.$watch('hotelSearch.nightCount', function (newValue, oldValue) {
            if (oldValue != newValue) {
                var cod = moment(scope.hotelSearch.checkinDate);
                scope.hotelSearch.checkoutDate = moment(cod).add(scope.hotelSearch.nightCount, 'days');
                scope.hotelSearch.checkoutDateDisplay = moment(scope.hotelSearch.checkoutDate).locale("id").format('LL');
            }

        });

        scope.$watch('hotelSearch.roomCount', function (newValue) {
            scope.hotelSearch.totalAdult = 0;
            scope.hotelSearch.totalChildren = 0;
            for (var i = 0; i < newValue; i++) {
                scope.hotelSearch.totalAdult += scope.hotelSearch.occupancies[i].adultCount;
                scope.hotelSearch.totalChildren += scope.hotelSearch.occupancies[i].childCount;
            }

        });

        for (var x = 0; x < scope.hotelSearch.occupancies.length; x++) {
            scope.$watch("hotelSearch.occupancies['" + x + "'].adultCount", function (val, oldVal) {
                if (val != null) {
                    scope.hotelSearch.totalAdult = scope.hotelSearch.totalAdult - oldVal + val;
                }
                
            });
            scope.$watch("hotelSearch.occupancies['" + x + "'].childCount", function (val, oldVal) {
                if (val != null) {
                    scope.hotelSearch.totalChildren = scope.hotelSearch.totalChildren - oldVal + val;
                }
                
            });
        }
        
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

        scope.getLocationCode = function () {
            var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            var locationCode = hashes[2].split('.')[1];
            return locationCode;
        }
        scope.setCheckinDate = function (scopeElement, date) {
            scopeElement.$apply(function () {
                $log.debug("scopeElement.hotelSearch.checkinDate = " + scopeElement.hotelSearch.checkinDate);
                scopeElement.hotelSearch.checkinDate = moment(date);
                $log.debug("scopeElement.hotelSearch.checkinDate = " + scopeElement.hotelSearch.checkinDate);
                scopeElement.hotelSearch.checkoutDate = moment(date).add(scopeElement.hotelSearch.nightCount, 'days');
                scopeElement.hotelSearch.checkinDateDisplay = scopeElement.hotelSearch.checkinDate.locale("id").format('LL');
                scopeElement.hotelSearch.checkoutDateDisplay = scopeElement.hotelSearch.checkoutDate.locale("id").format('LL');
            });
        }

        //factory.setCheckinDate = function (scopeElement, date) {
        //    scopeElement.$apply(function () {
        //        $log.debug("scopeElement.hotelSearch.checkinDate = " + scopeElement.hotelSearch.checkinDate);
        //        scopeElement.hotelSearch.checkinDate = moment(date).toISOString();
        //        $log.debug("scopeElement.hotelSearch.checkinDate = " + scopeElement.hotelSearch.checkinDate);
        //        scopeElement.hotelSearch.checkoutDate = moment(date).add(scopeElement.hotelSearch.nightCount, 'days').toISOString();
        //        scope.hotelSearch.checkinDateDisplay = moment(scopeElement.hotelSearch.checkinDate).locale("id").format('LL');
        //        scope.hotelSearch.checkoutDateDisplay = moment(scopeElement.hotelSearch.checkoutDate).locale("id").format('LL');
        //    });
        //}

        $('.hotel-date-picker').datepicker({
            numberOfMonths: 2,
            onSelect: function (date) {
                date = date.substring(3, 5) + "/" + date.substring(0, 2) + "/" + date.substring(6, 10);
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
                $log.debug("checkout date = " + scope.hotelSearch.checkoutDate);
                scope.hotelSearch.checkinDate = moment(scope.hotelSearch.checkinDate).locale("id");
                scope.hotelSearch.checkoutDate = moment(scope.hotelSearch.checkoutDate).locale("id");
                //scope.hotelSearch.checkinDateDisplay = scope.hotelSearch.checkinDate.format('LL');
                //scope.hotelSearch.checkoutDateDisplay = scope.hotelSearch.checkoutDate.format('LL');
            }
        });

        scope.initChildrenAges = function (n) {
            //if (scope.hotelSearch == null) {
            //    scope.hotelSearch = {}
            //    for (var h = 0; h < 8; h++) {
            //        scope.hotelSearch.occupancies.push({
            //            adultCount: 1,
            //            childCount: 0,
            //            childrenAges: [0,0,0,0]
            //        });
            //    }
            //}
            if (n < scope.hotelSearch.roomCount) {
                for (var c = 0; c < scope.hotelSearch.occupancies[n].childCount; c++) {
                    scope.hotelSearch.occupancies[n].childrenAges[c] = hotelSearch.occupancies[n].childrenAges[c];
                }
                for (var y = 0; y < 4 - scope.hotelSearch.occupancies[n].childCount; y++) {
                    scope.hotelSearch.occupancies[n].childrenAges.push(0);
                }
            } else {
                scope.hotelSearch.occupancies[n].childrenAges = [0, 0, 0, 0];
            } 
        }

    }

    var searchParam = function (hotelSearch) {
        if (hotelSearch.location == null || hotelSearch.location.length == 0) {
            //$scope.wrongParam = true;
            alert("Silakan pilih lokasi atau hotel dari daftar yang tersedia");
            return false;
        } else {
            var occupancyQuery = [];
            for (var r = 0; r < hotelSearch.roomCount; r++) {
                var q1 = [hotelSearch.occupancies[r].adultCount, hotelSearch.occupancies[r].childCount].join('~');
                var age = '';
                if (hotelSearch.occupancies[r].childCount > 0) {
                    var realAge = hotelSearch.occupancies[r].childrenAges.slice(0, hotelSearch.occupancies[r].childCount);
                    age = realAge.join(',');
                    q1 = [q1, age].join('~');
                }

                occupancyQuery.push(q1);
            }

            var occquery = occupancyQuery.join('|');

            var completequery = "?info=" + [
            hotelSearch.searchHotelType.location,
            hotelSearch.location,
            moment(hotelSearch.checkinDate).format("YYYY-MM-DD"),
            moment(hotelSearch.checkoutDate).format("YYYY-MM-DD"),
            hotelSearch.nightCount,
            hotelSearch.roomCount,
            occquery
            ].join('.');
            return completequery;
        }     
    };
   
    return factory;
}]);

