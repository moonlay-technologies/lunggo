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
            "searchHotelType": searchtype,
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
                        window.location.href = '/id/Hotel/DetailHotel?' +
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
                hotelSearch.urlData.country = hotelSearch.urlData.country.replace(/\s+/g, '-');
                hotelSearch.urlData.country = hotelSearch.urlData.country.replace(/[^0-9a-zA-Z-]/gi, '');

                hotelSearch.urlData.destination = hotelSearch.urlData.destination.replace(/\s+/g, '-');
                hotelSearch.urlData.destination = hotelSearch.urlData.destination.replace(/[^0-9a-zA-Z-]/gi, '');

                if (hotelSearch.urlData.zone != null && hotelSearch.urlData.zone.length > 0) {
                    hotelSearch.urlData.zone = hotelSearch.urlData.zone.replace(/\s+/g, '-');
                    hotelSearch.urlData.zone = hotelSearch.urlData.zone.replace(/[^0-9a-zA-Z-]/gi, '');
                }
                
                if (hotelSearch.urlData.area != null && hotelSearch.urlData.area.length > 0) {
                    hotelSearch.urlData.area = hotelSearch.urlData.area.replace(/\s+/g, '-');
                    hotelSearch.urlData.area = hotelSearch.urlData.area.replace(/[^0-9a-zA-Z-]/gi, '');
                }
               
                var urlParam = '/id/hotel/cari/' + hotelSearch.urlData.country + '/' + hotelSearch.urlData.destination;

                if (hotelSearch.urlData.type == 'Zone') {
                    urlParam += '/' + hotelSearch.urlData.zone;
                } else if (hotelSearch.urlData.type == 'Area') {
                    urlParam += '/' + hotelSearch.urlData.zone + '/' + hotelSearch.urlData.area;
                }
                location.href = urlParam + '/' + param;
                
            }
        }
        
    };

    factory.holidays = [];
    factory.holidayNames = [];

    factory.getHolidays = function () {
        $.ajax({
            url: GetHolidayConfig.Url,
            method: 'GET',
            headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
        }).done(function (returnData) {
            if (returnData.status == 200) {
                if (returnData.events != null) {
                    for (var i = 0; i < returnData.events.length; i++) {
                        var holidayDate = new Date(returnData.events[i].date);
                        var holidayName = returnData.events[i].name;
                        factory.holidays.push(holidayDate);
                        factory.holidayNames.push(holidayName);
                    }
                }
            }

        }).error(function (returnData) {

        });
    };
   
    factory.highlightDays = function (date) {
        for (var i = 0; i < factory.holidays.length; i++) {
            var x = new Date(date);
            var xmonth = x.getMonth();
            var xday = x.getDate();
            var xyear = x.getFullYear();

            var y = new Date(factory.holidays[i]);
            var ymonth = y.getMonth();
            var yday = y.getDate();
            var yyear = y.getFullYear();

            if (xmonth == ymonth && xday == yday && xyear == yyear) {
                return [true, 'ui-state-holiday', factory.holidayNames[i]];
            }
        }
        return [true, ''];
    }

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
        scope.hotelSearch.urlData = {
            destination: '',
            country: '',
            type: '',
            zone: '',
            hotelName: '',
            area: ''
        };
        var defaultValue = {
            locationCode: 16173,
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

            scope.hotelSearch.destinationCheckinDate = moment(searchParamObject.checkinDate).locale("id").format('dddd, DD MMMM YYYY');
            scope.hotelSearch.destinationCheckoutDate = moment(searchParamObject.checkoutDate).locale("id").format('dddd, DD MMMM YYYY');

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
        scope.autocompleteLoading = false;
        scope.autocompleteNoResult = false;
        scope.autocompletePre = false;
        scope.hotelSearch.totalAdult = 0;
        scope.hotelSearch.totalChildren = 0;
        scope.hotelSearch.getHotels = function () {
            scope.setCookie();
            factory.gotoHotelSearch(this);
        }

        scope.getLocationCode = function () {
            var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            var locationCode = hashes[0].split('.')[0].split('=')[1];
            return locationCode;
        }

        // Autocomplete

        scope.PopularDestinations = [
           "Bali",
           "Jakarta",
           "Bandung",
           "Yogyakarta",
           "Surabaya",
           "Solo",
           "Malang",
           "Bogor",
           "Medan",
           "Palembang",
           "Singapura",
           "Kuala Lumpur",
           "Bangkok",
           "Hong Kong"
        ];

        scope.selectLocation = function (location) {
            scope.hotelSearch.location = location.id;
            scope.hotelSearch.locationDisplay = location.name;
            scope.view.showHotelSearch = false;
            scope.hotelSearch.locationType = location.type;
            scope.hotelSearch.urlData.country = location.country;
            scope.hotelSearch.urlData.destination = location.destination;
            scope.hotelSearch.urlData.zone = location.zone;
            scope.hotelSearch.urlData.area = location.area;
            scope.hotelSearch.urlData.type = location.type;
        }

        scope.$watch('hotelSearch.locationDisplay', function (newValue) {
            if (newValue != null && newValue.length >= 3) {
                scope.autocompletePre = false;
                scope.autocompleteLoading = true;
                scope.showPopularDestinations = false;
                scope.hotelSearch.autocompleteResource.get({ prefix: newValue }).$promise.then(function(data) {
                    $timeout(function() {
                        scope.autocompleteLoading = false;
                        if (data.hotelAutocomplete != null) {
                            scope.showPopularDestinations = false;
                            scope.autocompleteNoResult = false;
                        } else {
                            scope.autocompleteNoResult = true;
                        }

                        scope.hotelSearch.hotelAutocomplete = data.hotelAutocomplete;

                        $log.debug(scope.hotelSearch.hotelAutocomplete);
                    }, 0);
                });
            } else {
                if (newValue == null || newValue.length == 0) {
                    scope.autocompletePre = false;
                    scope.showPopularDestinations = true;
                } else {
                    scope.autocompletePre = true;
                }
                
                scope.autocompleteNoResult = false;
            };
        });

        scope.getLocation = function (newValue) {
            if (newValue != null && newValue.length >= 3) {
                scope.autocompletePre = false;
                scope.autocompleteLoading = true;
                scope.hotelSearch.autocompleteResource.get({ prefix: newValue }).$promise.then(function (data) {
                    $timeout(function () {
                        scope.autocompleteLoading = false;
                        if (data.hotelAutocomplete != null) {
                            scope.showPopularDestinations = false;
                            scope.autocompleteNoResult = false;
                        } else {
                            scope.autocompleteNoResult = true;
                        }

                        scope.hotelSearch.hotelAutocomplete = data.hotelAutocomplete;

                        $log.debug(scope.hotelSearch.hotelAutocomplete);
                    }, 0);
                });
            } else {
                if (newValue == null || newValue.length == 0) {
                    scope.autocompletePre = false;
                    scope.showPopularDestinations = true;
                } else {
                    scope.autocompletePre = true;
                }

                scope.autocompleteNoResult = false;
            };
        };

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

        scope.getLocationId = function(location) {
            scope.hotelSearch.autocompleteResource.get({ prefix: location }).$promise.then(function (data) {
                $timeout(function () {
                    if (data.hotelAutocomplete == null || data.hotelAutocomplete.length == 0) {
                        
                    } else {
                        scope.hotelSearch.location = data.hotelAutocomplete[0].id;
                        scope.hotelSearch.locationDisplay = data.hotelAutocomplete[0].name;
                        scope.hotelSearch.urlData.country = data.hotelAutocomplete[0].country;
                        scope.hotelSearch.urlData.destination = data.hotelAutocomplete[0].destination;
                        scope.hotelSearch.urlData.zone = data.hotelAutocomplete[0].zone;
                        scope.hotelSearch.urlData.area = data.hotelAutocomplete[0].area;
                        scope.hotelSearch.urlData.type = data.hotelAutocomplete[0].type;
                        scope.hideLocation();
                    }
                }, 0);
            });
        }

        scope.hideLocation = function() {
            $('.search-hotel').hide();
        }
        // END

        // DATE & CALENDAR

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
                $log.debug("checkout date = " + scope.hotelSearch.checkoutDate);
                scope.hotelSearch.checkinDate = moment(scope.hotelSearch.checkinDate).locale("id");
                scope.hotelSearch.checkoutDate = moment(scope.hotelSearch.checkoutDate).locale("id");
            },
            //beforeShowDay: scope.highlightDays
        });

        scope.$watch('hotelSearch.nightCount', function (newValue, oldValue) {
            if (oldValue != newValue) {
                var cod = moment(scope.hotelSearch.checkinDate);
                scope.hotelSearch.checkoutDate = moment(cod).add(scope.hotelSearch.nightCount, 'days');
                scope.hotelSearch.checkoutDateDisplay = moment(scope.hotelSearch.checkoutDate).locale("id").format('LL');
            }

        });

        // END

        // OCCUPANCIES

        //tbd
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

        //scope.$watch('hotelSearch.roomCount', function (newValue) {
        //    scope.hotelSearch.totalAdult = 0;
        //    scope.hotelSearch.totalChildren = 0;
        //    for (var i = 0; i < newValue; i++) {
        //        scope.hotelSearch.totalAdult += scope.hotelSearch.occupancies[i].adultCount;
        //        scope.hotelSearch.totalChildren += scope.hotelSearch.occupancies[i].childCount;
        //    }

        //});

        for (var x = 0; x < scope.hotelSearch.occupancies.length; x++) {
            scope.$watchGroup(['hotelSearch.roomCount', "hotelSearch.occupancies['" + x + "'].adultCount"], function (val, oldVal) {
                val[0] = parseInt(val[0]);
                val[1] = parseInt(val[1]);
                oldVal[0] = parseInt(oldVal[0]);
                oldVal[1] = parseInt(oldVal[1]);
                scope.hotelSearch.totalAdult = 0;
                scope.hotelSearch.totalChildren = 0;
                for (i = 0; i < val[0]; i++) {
                    scope.hotelSearch.totalAdult += scope.hotelSearch.occupancies[i].adultCount;
                    scope.hotelSearch.totalChildren += scope.hotelSearch.occupancies[i].childCount;
                }
                //if (val[0] != oldVal[0]) {
                //    scope.hotelSearch.totalAdult = 0;
                //    scope.hotelSearch.totalChildren = 0;
                //    for (i = 0; i < val[0]; i++) {
                //        scope.hotelSearch.totalAdult += scope.hotelSearch.occupancies[i].adultCount;
                //        scope.hotelSearch.totalChildren += scope.hotelSearch.occupancies[i].childCount;
                //    }
                //}
                //else if (val[1] != oldVal[1]) {
                    
                //    //scope.hotelSearch.totalAdult = scope.hotelSearch.totalAdult - oldVal[1] + val[1];
                //}
                
            });
            scope.$watchGroup(['hotelSearch.roomCount',"hotelSearch.occupancies['" + x + "'].childCount"], function (val, oldVal) {
                val[0] = parseInt(val[0]);
                val[1] = parseInt(val[1]);
                oldVal[0] = parseInt(oldVal[0]);
                oldVal[1] = parseInt(oldVal[1]);
                if (val[0] != oldVal[0]) {
                    scope.hotelSearch.totalAdult = 0;
                    scope.hotelSearch.totalChildren = 0;
                    for (i = 0; i < val[0]; i++) {
                        scope.hotelSearch.totalAdult += scope.hotelSearch.occupancies[i].adultCount;
                        scope.hotelSearch.totalChildren += scope.hotelSearch.occupancies[i].childCount;
                    }
                }
                else if (val[1] != oldVal[1]) {
                    //scope.hotelSearch.totalChildren = scope.hotelSearch.totalChildren - oldVal[1] + val[1];
                    scope.hotelSearch.totalAdult = 0;
                    scope.hotelSearch.totalChildren = 0;
                    for (i = 0; i < val[0]; i++) {
                        scope.hotelSearch.totalAdult += scope.hotelSearch.occupancies[i].adultCount;
                        scope.hotelSearch.totalChildren += scope.hotelSearch.occupancies[i].childCount;
                    }
                }
                
            });
        }

        //scope.$watchCollection(['hotelSearch.roomCount', 'hotelSearch.occupancies'] , function (newVal, oldVal) {
        //    scope.hotelSearch.totalAdult = 0;
        //    scope.hotelSearch.totalChildren = 0;
        //    for (i = 0; i < newVal[0]; i++) {
        //        scope.hotelSearch.totalAdult += newVal[1][i].adultCount;
        //        scope.hotelSearch.totalChildren += newVal[1][i].childCount;
        //    }

        //});
        
        scope.initChildrenAges = function (n) {

            if (n < scope.hotelSearch.roomCount) {
                for (var y = 0; y < 4 - scope.hotelSearch.occupancies[n].childCount; y++) {
                    scope.hotelSearch.occupancies[n].childrenAges.push(0);
                }
            } else {
                scope.hotelSearch.occupancies.push({
                    adultCount: 1,
                    childCount: 0,
                    childrenAges: [0, 0, 0, 0]
                });
            } 
        }

        //COOKIES
        scope.setCookie = function () {
            Cookies.set('hotelSearchLocationDisplay', scope.hotelSearch.locationDisplay, { expires: 9999 });
            Cookies.set('hotelSearchLocation', scope.hotelSearch.location, { expires: 9999 });
            Cookies.set('hotelSearchCheckInDate', scope.hotelSearch.checkinDate, { expires: 9999 });
            Cookies.set('hotelSearchCheckOutDate', scope.hotelSearch.checkoutDate, { expires: 9999 });
            Cookies.set('hotelSearchNights', scope.hotelSearch.nightCount, { expires: 9999 });
            Cookies.set('hotelSearchRooms', scope.hotelSearch.roomCount, { expires: 9999 });
            Cookies.set('hotelSearchOccupancies', scope.hotelSearch.occupancies, { expires: 9999 });
            Cookies.set('urlCountry', scope.hotelSearch.urlData.country, { expires: 9999 });
            Cookies.set('urlDestination', scope.hotelSearch.urlData.destination, { expires: 9999 });
            Cookies.set('urlZone', scope.hotelSearch.urlData.zone, { expires: 9999 });
            Cookies.set('urlArea', scope.hotelSearch.urlData.area, { expires: 9999 });
            Cookies.set('urlType', scope.hotelSearch.urlData.type, { expires: 9999 });

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

