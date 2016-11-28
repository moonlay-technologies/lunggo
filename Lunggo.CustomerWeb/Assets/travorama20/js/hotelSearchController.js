// home controller
app.controller('hotelSearchController', ['$scope', '$log', '$http', '$resource', '$timeout', 'hotelSearchSvc', function ($scope, $log, $http, $resource, $timeout, hotelSearchSvc) {

    $scope.destinationName = "";
    $scope.model = {};
    $scope.hotels = [];
    $scope.totalActualHotel = '';
    $scope.hotelFilterDisplayInfo = undefined;

    $scope.filterDisabled = true;
    var isFirstload = true;

    //@Url.Action("Search", "Hotel")?zzz={{departureDate}}" method="POST"
    //=============== hotel start ======================
    //$scope.hotel = {};
    ////$scope.hotel.searchHotelType = { "location": 'Location', searchId: 'SearchId'};
    //$scope.hotel.searchId = null;
    ////$scope.hotel.searchId = "fde8cec5-c267-4e6f-88a2-d6cf1219e14c";
    //$scope.hotel.location = "BALI";
    //$scope.hotel.checkinDate = "12/10/2016";
    //$scope.hotel.checkoutDate = "12/11/2016";
    //$scope.hotel.adultCount = 3;
    //$scope.hotel.childCount = 1;
    //$scope.hotel.nightCount = 1;
    //$scope.hotel.roomCount = 2;
    //$scope.hotel.childrenAges = [];
    //$scope.hotel.searchParam = '';

    //new { checkinDate
    //:
    //"2016-12-03T00:00:00"
    //checkoutDate
    //:
    //"2016-12-05"
    //hotelFilter
    //:
    //{priceFilter: {minPrice: 200000, maxPrice: 1800000}, zoneFilter: [], starFilter: null,…}
    //facilityFilter
    //:
    //null
    //nameFilter
    //:
    //""
    //priceFilter
    //:
    //{minPrice: 200000, maxPrice: 1800000}
    //maxPrice
    //:
    //1800000
    //minPrice
    //:
    //200000
    //starFilter
    //:
    //null
    //zoneFilter
    //:
    //[]
    //hotelSorting
    //:
    //"ASCENDINGPRICE"
    //location
    //:
    //"16162"
    //nightCount
    //:
    //2
    //occupancies
    //:
    //[{adultCount: 1, childCount: 0, roomCount: 1, childrenAges: ["0", "0", "0", "0"]}]
    //0
    //:
    //{adultCount: 1, childCount: 0, roomCount: 1, childrenAges: ["0", "0", "0", "0"]}
    //adultCount
    //:
    //1
    //childCount
    //:
    //0
    //childrenAges
    //:
    //["0", "0", "0", "0"]
    //0
    //:
    //"0"
    //1
    //:
    //"0"
    //2
    //:
    //"0"
    //3
    //:
    //"0"
    //roomCount
    //:
    //1
    //page
    //:
    //1
    //perPage
    //:
    //20
    //regsId
    //:
    //null
    //searchHotelType
    //:
    //"SearchId"
    //searchId
    //:
    //"5ef8e473-31eb-457b-a138-ab17c288620b"




    $scope.searchDone = false;
    $scope.finishLoad = false;

    $scope.researching = false;
    $scope.filter = {};
    $scope.filter.nameFilter = "";
    $scope.filter.minPrice = 0;
    $scope.filter.maxPrice = 0;
    $scope.filter.zones = [];
    $scope.filter.stars = null;
    $scope.filter.facilities = null;
    $scope.sortByType = { "ascendingPrice": "ASCENDINGPRICE", "descendingPrice": "DESCENDINGPRICE"};
    $scope.sortBy = $scope.sortByType.ascendingPrice;

    $scope.page = 1;
    $scope.perPage = 20;
    $scope.pageCount = 1;
    $scope.totalHotelCount = 0;
    $scope.searchHeader = {};
    $scope.loc = loc;
    $scope.init = function (model) {
        $scope.model = model;
        $log.debug($scope.model);
        //$scope.hotel.searchId = $scope.model.searchId;
        $scope.hotelSearch.location = $scope.model.searchParamObject.location;
        $scope.hotelSearch.checkinDate = $scope.model.searchParamObject.checkinDate;
        $scope.hotelSearch.checkoutDate = $scope.model.searchParamObject.checkoutDate;
        $scope.hotelSearch.destinationCheckinDate = $scope.model.searchParamObject.checkinDate;
        $scope.hotelSearch.destinationCheckoutDate = $scope.model.searchParamObject.checkoutDate;
        $scope.hotelSearch.adultCount = $scope.model.searchParamObject.adultCount;
        $scope.hotelSearch.childCount = $scope.model.searchParamObject.childCount;
        $scope.hotelSearch.nightCount = $scope.model.searchParamObject.nightCount;//new Date($scope.hotelSearch.checkoutDate).getDate() - new Date($scope.hotelSearch.checkinDate).getDate();
        $scope.hotelSearch.destinationNightCount = $scope.model.searchParamObject.nightCount;
        $scope.hotelSearch.roomCount = $scope.model.searchParamObject.roomCount;
        $scope.hotelSearch.childrenAges = $scope.model.searchParamObject.childrenAges;
        $scope.hotelSearch.searchParamObject = $scope.model.searchParamObject;
        //$scope.hotelSearch.searchParam = $scope.model.searchParam;
        $scope.searchParam = model.searchParam;

        $scope.searchHotel();
    }

    var searchPromise = function () {
        var pagination = { 'sortBy': $scope.sortBy, 'page': $scope.page, 'perPage' : $scope.perPage };
        return hotelSearchSvc.searchHotel($scope.hotelSearch, $scope.filter, pagination).$promise.then(function (data) {
            return data;
        });
    }

    hotelSearchSvc.initializeSearchForm($scope);

    $scope.searchHotel = function (filter, sort) {
        $scope.searchDone = false;
        $scope.pageCount = 0;

        $timeout(function () { }, 1);
        searchPromise().then(function (data) {
            var flag = false;
            if (validateResponse(data) == false) {
                return false;
            } else $scope.researching = false;

            if (data.searchId !== undefined) $scope.hotelSearch.searchId = data.searchId;

            $scope.hotelSearch.destinationName = data.destinationName;
            $scope.hotelSearch.locationDisplay = data.destinationName;
            $scope.hotels = data.hotels;
            $scope.totalActualHotel = data.returnedHotelCount;
            $scope.returnedHotelCount = data.returnedHotelCount;
            $scope.filteredHotelCount = data.filteredHotelCount;
            
            $scope.page = data.page;
            $scope.perPage = data.perPage;
            $scope.pageCount = data.pageCount;
            $scope.totalHotelCount = data.totalHotelCount;

            if (isFirstload) {
                $scope.filter.minPrice = data.minPrice;
                $scope.filter.maxPrice = data.maxPrice;
            initiatePriceSlider();

                $scope.hotelFilterDisplayInfo = data.hotelFilterDisplayInfo;
                isFirstload = false;
            };

            //$timeout(function () { customCheckbox(); }, 0);
            $log.debug(data);
        }, function (error) {
            $log.error("error: " + error);
        }).finally(function () {
            if ($scope.researching == false) {
                $scope.searchDone = true;
                $scope.finishLoad = true;
            }

        });
    };

    var validateResponse = function (data) {
        if (data.error != undefined || data.error != null) {
            if (data.error == "ERHSEA02") {
                $log.debug('search id expired. researching...');
                $scope.hotelSearch.searchId = null;
                $scope.researching = true;
                $scope.searchHotel();
                return false;
            }
        }

        return true;
    };

    $scope.GotoDetailHotel = function (hotelCd) {
        $log.debug('redirect to detail hotel with hotelCd: ' + hotelCd);
        location.href = '/id/Hotel/DetailHotel/?' +
            "searchId=" + $scope.hotelSearch.searchId + "&" +
            "hotelCd=" + hotelCd + "&" + 
            "searchParam=" + $scope.searchParam;
    }

    $scope.changeFilter = function (filterType, value) {
        if (filterType == 'star') {
            if ($scope.filter.stars == null) {
                $scope.filter.stars = [value];
            }
            else {
                var starIndex = $scope.filter.stars.indexOf(value);
                if (starIndex < 0) {
                    $scope.filter.stars.push(value);
                }
                else {
                    $scope.filter.stars.splice(starIndex, 1);
                }
            }
        }
        else if (filterType == 'zone') {
            if ($scope.filter.zones == null) {
                $scope.filter.zones = [value.code];
            }
            else {
                var zoneCodeIndex = $scope.filter.zones.indexOf(value.code);
                if (zoneCodeIndex < 0) {
                    $scope.filter.zones.push(value.code);
                }
                else {
                    $scope.filter.zones.splice(zoneCodeIndex, 1);
                }
            }
        }
        else if (filterType == 'facility') {
            if ($scope.filter.facilities == null) {
                $scope.filter.facilities = [value.code];
            }
            else {
                var facilityCodeIndex = $scope.filter.facilities.indexOf(value.code);
                if (facilityCodeIndex < 0) {
                    $scope.filter.facilities.push(value.code);
                }
                else {
                    $scope.filter.facilities.splice(facilityCodeIndex, 1);
                }
            }
        }
    }

    var initiatePriceSlider = function (minPrice, maxPrice) {
        $scope.filter.minPrice = $scope.filter.minPrice - ($scope.filter.minPrice % 100000);
        if ($scope.filter.maxPrice % 100000 == 0) {
            $scope.filter.maxPrice = $scope.filter.maxPrice;
        } else $scope.filter.maxPrice = $scope.filter.maxPrice + (100000 - $scope.filter.maxPrice % 100000);
        // activate price filter
        $('.slider-range').slider({
            range: true,
            min: $scope.filter.minPrice,
            max: $scope.filter.maxPrice,
            step: 100000,
            values: [$scope.filter.minPrice, $scope.filter.maxPrice],
            create: function (event, ui) {
                $('.slider-range-min').val($scope.filter.minPrice);
                $('.slider-range-min').trigger('input');
                $('.slider-range-max').val($scope.filter.maxPrice);
                $('.slider-range-max').trigger('input');
            },
            slide: function (event, ui) {
                //$('.slider-range-min').val(ui.values[0]);
                $scope.filter.minPrice = ui.values[0];
                $('.slider-range-min').trigger('input');
                $scope.filter.maxPrice = ui.values[1];
                $('.slider-range-max').trigger('input');
                $scope.$apply();
            }
        });
    }
    
    //$scope.$watch('hotel.locationDisplay', function (newValue, oldValue) {
    //    if (newValue.length >= 3) {
    //        hotelAutocompleteSvc.resource.get({ prefix: newValue }).$promise.then(function (data) {
    //            $timeout(function () {
    //                $scope.hotel.hotelAutocomplete = data.hotelAutocomplete;
    //                $log.debug($scope.hotel.hotelAutocomplete);
    //            }, 0);
    //        });
    //    }
    //});

    $scope.prevPage = function () {
        $scope.page--;
        $scope.searchHotel();
    };

    $scope.nextPage = function () {
        $scope.page++;
        $scope.searchHotel();
    };

    $scope.changePage = function (pageNumber) {
        $scope.page = pageNumber;
        $scope.searchHotel();
    };

    $scope.changeSorting = function (sortBy) {
        if ($scope.searchDone && $scope.sortBy != sortBy) {
            $scope.sortBy = sortBy;
            $scope.searchHotel();
        }
    };

    $scope.filterHotels = function () {
        $scope.page = 1;
        $scope.searchHotel();

    };

    var filterHotels = function () {
        if ($scope.finishLoad) {
            $scope.page = 1;
            $scope.searchHotel();
        }
    }

    $scope.$watch('filter.nameFilter', function (newValue, oldValue, scope) {
        filterHotels();
    }, true);

    var watchGroup = ['filter.minPrice', 'filter.maxPrice']
    $scope.$watchGroup(watchGroup, function (newValue, oldValue, scope) {
        filterHotels();
    }, true);

    $scope.$watch('filter.zones', function (newValue, oldValue, scope) {
        filterHotels();
    }, true);

    $scope.$watch('filter.stars', function (newValue, oldValue, scope) {
        filterHotels();
    }, true);

    $scope.$watch('filter.facilities', function (newValue, oldValue, scope) {
        filterHotels();
    }, true);

    $scope.toTitleCase = function(str)
    {
        return str.replace(/\w\S*/g, function(txt){return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase();});
    }
}]);
