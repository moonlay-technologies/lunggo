// home controller
app.controller('hotelSearchController', ['$scope', '$log', '$window', '$http', '$resource', '$timeout', 'hotelSearchSvc',
function ($scope, $log, $window, $http, $resource, $timeout, hotelSearchSvc) {

    // **************************GENERAL VARIABLES*****************************
   
    // Hotel Data
    $scope.model = {};
    $scope.hotels = [];
    $scope.totalActualHotel = '';
    $scope.hotelFilterDisplayInfo = undefined;
    $scope.totalHotelCount = 0;

    // Filter and Sort Variables

    $scope.filter = {
        nameFilter: "",
        minPrice: 0,
        maxPrice: 0,
        zones: [],
        stars: null,
        facilities: null
    };

    $scope.sortByType = {
        "ascendingPrice": "ASCENDINGPRICE", "descendingPrice": "DESCENDINGPRICE",
        "ascendingStar": "ASCENDINGSTAR", "descendingStar": "DESCENDINGSTAR"
    };
    $scope.sortBy = $scope.sortByType.ascendingPrice;
    $scope.minPrice = '';
    $scope.maxPrice = '';

    // Below is object for filtering and sort
    $scope.changeSearch = {
        occupancies: [],
        location: location,
    }

    // Page Load And Display

    var isFirstload = true;
    $scope.loc = loc;
    $scope.bottomPage = false;
    $scope.searchDone = false;
    $scope.finishLoad = false;
    $scope.page = 1;
    $scope.perPage = 20;
    $scope.pageCount = 1;
    $scope.researching = false;
    $scope.returnUrl = "/";
    $scope.expired = false;
   
    // ***************************************END*******************************

    // ****************************** INITS ************************************
    $scope.init = function (model) {
        $scope.model = model;
        $log.debug($scope.model);
        $scope.hotelSearch.location = $scope.model.searchParamObject.location;
        $scope.hotelSearch.checkinDate = $scope.model.searchParamObject.checkinDate;
        $scope.hotelSearch.checkinDateDisplay = moment($scope.hotelSearch.checkinDate).locale("id").format('LL');
        $scope.hotelSearch.checkoutDate = $scope.model.searchParamObject.checkoutDate;
        $scope.hotelSearch.checkoutDateDisplay = moment($scope.hotelSearch.checkoutDate).locale("id").format('LL');

        $scope.hotelSearch.destinationCheckinDate = $scope.model.searchParamObject.checkinDate;
        $scope.hotelSearch.destinationCheckoutDate = $scope.model.searchParamObject.checkoutDate;
        $scope.hotelSearch.nightCount = $scope.model.searchParamObject.nightCount;
        $scope.hotelSearch.destinationNightCount = $scope.model.searchParamObject.nightCount;
        $scope.hotelSearch.roomCount = $scope.model.searchParamObject.roomCount;
        $scope.hotelSearch.occupancies = $scope.model.searchParamObject.occupancies;
        $scope.hotelSearch.searchParamObject = $scope.model.searchParamObject;
        for (var x = 0; x < 7; x++) {
            if (x < $scope.hotelSearch.occupancies.length) {
                $scope.changeSearch.occupancies[x] = {
                    adultCount: $scope.hotelSearch.occupancies[x].adultCount,
                    childCount: $scope.hotelSearch.occupancies[x].childCount,
                    childrenAges: [0, 0, 0, 0],
                    roomCount: 1,
                }
                for (var y = 0; y < $scope.hotelSearch.occupancies[x].childCount; y++) {
                    $scope.changeSearch.occupancies[x].childrenAges[y] = $scope.hotelSearch.occupancies[x].childrenAges[y];
                }
            }
        }

        $scope.changeSearch.checkinDate = $scope.model.searchParamObject.checkinDate;
        $scope.changeSearch.checkoutDate = $scope.model.searchParamObject.checkoutDate;
        $scope.changeSearch.checkinDateDisplay = $scope.hotelSearch.checkinDateDisplay;
        $scope.changeSearch.checkoutDateDisplay = $scope.hotelSearch.checkoutDateDisplay;
        $scope.changeSearch.roomCount = $scope.hotelSearch.roomCount;
        $scope.changeSearch.nightCount = $scope.hotelSearch.nightCount;
        $scope.changeSearch.location = $scope.hotelSearch.location;
        $scope.changeSearch.locationDisplay = loc;
        $scope.changeSearch.adultrange = [1, 2, 3, 4, 5];
        $scope.changeSearch.childrange = [0, 1, 2, 3, 4];
        $scope.changeSearch.roomrange = [1, 2, 3, 4, 5, 6, 7, 8];
        $scope.changeSearch.nightrange = [1, 2, 3, 4, 5, 6, 7];
        $scope.changeSearch.childagerange = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11];
        $scope.changeSearch.searchHotelType = {
            location: $scope.hotelSearch.searchHotelType.location,
            searchId: $scope.hotelSearch.searchHotelType.searchId,
        }
        $scope.searchParam = model.searchParam;
        $scope.searchHotel();
    }
    hotelSearchSvc.initializeSearchForm($scope);

    // ******************************* END *************************************

    // ************************** SEARCH FUNCTIONS *****************************
    var searchPromise = function () {
        var pagination = { 'sortBy': $scope.sortBy, 'page': $scope.page, 'perPage': $scope.perPage };
        return hotelSearchSvc.searchHotel($scope.changeSearch, $scope.filter, pagination).$promise.then(function (data) {
            return data;
        });
    }

    var validateResponse = function (data) {
        if (data.error != undefined || data.error != null) {
            if (data.error == "ERHSEA02") {
                $log.debug('search id expired. researching...');
                $scope.expired = true;
                $scope.hotelSearch.searchId = null;
                $scope.researching = true;
                $scope.searchHotel();
                return false;
            }
        }

        return true;
    };
    $scope.searchHotel = function (filter, sort, isMobile) {
        $scope.searchDone = false;
        $scope.pageCount = 0;

        $timeout(function () { }, 1);

        searchPromise().then(function (data) {

            if (validateResponse(data) == false) {
                return false;
            } else $scope.researching = false;

            if (data.searchId !== undefined) {

                $scope.hotelSearch.searchId = data.searchId;
                $scope.changeSearch.searchId = data.searchId;
            }

            $scope.hotelSearch.destinationName = data.destinationName;
            $scope.hotelSearch.locationDisplay = data.destinationName;
            if (isMobile) {
                $scope.hotels.push.apply($scope.hotels, data.hotels);
                $scope.bottomPage = false;
            } else {
                $scope.hotels = data.hotels;
            }
            
            $scope.totalActualHotel = data.returnedHotelCount;
            $scope.returnedHotelCount = data.returnedHotelCount;
            $scope.filteredHotelCount = data.filteredHotelCount;
            
            if (data.page > $scope.page) {
                $scope.page = data.page;
            }
            
            $scope.perPage = data.perPage;
            $scope.pageCount = data.pageCount;
            $scope.totalHotelCount = data.totalHotelCount;

            if (isFirstload) {
                $scope.filter.minPrice = data.minPrice;
                $scope.filter.maxPrice = data.maxPrice;
                $scope.minPrice = data.minPrice;
                $scope.maxPrice = data.maxPrice;
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

    $scope.hotel = {
        searchHotel: function () {
            hotelSearchSvc.gotoHotelSearch($scope.hotelSearch);
        }
    }

    // ****************************** END **************************************

    // *************************** GO TO DETAIL HOTEL **************************

    $scope.GotoDetailHotel = function (hotelCd) {
        $log.debug('redirect to detail hotel with hotelCd: ' + hotelCd);
        $window.open('/id/Hotel/DetailHotel?' +
            "searchId=" + $scope.hotelSearch.searchId + "&" +
            "hotelCd=" + hotelCd + "&" +
            "searchParam=" + $scope.searchParam, '_blank');
    }

    // ****************************** END **************************************

    // ****************************** PAGING ***********************************

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
    
    $scope.getPageNumberList = function (pageCount, currentPage) {

        var maxPage = 10;

        var returnValue = [];
        if (pageCount <= maxPage) {
            for (var p = 1; p <= pageCount; p++)
                returnValue.push(p);
        } else {
            if (currentPage < maxPage) {
                for (var p = 1; p <= maxPage; p++)
                    returnValue.push(p);
            } else if (currentPage >= maxPage && currentPage < pageCount - maxPage) {
                for (var p = currentPage - 5; p <= currentPage + 4; p++)
                    returnValue.push(p);
            } else if (currentPage >= pageCount - maxPage) {
                for (var p = pageCount - maxPage; p <= pageCount; p++)
                    returnValue.push(p);
            }
        }

        return returnValue;
    };

    // ****************************** END **************************************

    // ************************* FILTER & SORTS ********************************

    var filterHotels = function () {
        if ($scope.finishLoad) {
            $scope.page = 1;
            $scope.searchHotel();
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
    
    $scope.$watch('filter.nameFilter', function (newValue, oldValue, scope) {
        filterHotels();
    }, true);

    $scope.$watch('filter.minPrice', function (newValue, oldValue, scope) {
        if (oldValue != 0 && oldValue !== undefined) {
            filterHotels();
        }
    }, true);

    $scope.$watch('filter.maxPrice', function (newValue, oldValue, scope) {
        if (oldValue != 0 && oldValue !== undefined) {
            filterHotels();
        }
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

    $scope.resetButtonFilter = function () {
        $scope.filter.stars = [];
        $scope.filter.zones = [];
        $scope.filter.facilities = [];
        $scope.filter.minPrice = $scope.minPrice - ($scope.minPrice % 100000);
        $scope.filter.maxPrice = $scope.maxPrice + (100000 - $scope.maxPrice % 100000);
        $('.slider-range').slider({
            values: [$scope.filter.minPrice, $scope.filter.maxPrice]
        });
    }

    $('.open-txt').click(function () {
        $(this).toggleClass('active');
        $(this).parent().find('.short-txt').toggleClass('open');
    });

    // ****************************** END **************************************

    // ****************************** OTHER ************************************

    $('.change-hotel.form-control').click(function () {
        $(this).select();
    });

    $scope.toTitleCase = function (str) {
        return str.replace(/\w\S*/g, function (txt)
        { return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase(); });
    }

    $scope.HotelSearchForm = {
        AutoComplete: {
            Keyword: '',
            MinLength: 3,
            GetLocation: function () {
                $scope.getLocation($scope.HotelSearchForm.AutoComplete.Keyword);
            },
        },
    }

    // ****************************** END **************************************
}]);

//$('.overlay .filter-group--facility a').on('click', function () { //click or click touchstart
//    $('.overlay .filter-group--facility a').toggleClass('active');
//    $('.overlay .sh-list').toggleClass('opened');
//});

//$('.overlay .filter-group--area').on('click', function () {
//    $('.overlay .filter-group--facility a').toggleClass('active');
//    $('.overlay .sh-list').toggleClass('opened');
//});