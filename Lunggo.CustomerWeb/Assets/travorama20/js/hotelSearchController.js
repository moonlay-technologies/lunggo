// home controller
app.controller('hotelSearchController', ['$scope', '$log', '$http', '$resource', '$timeout', function ($scope, $log, $http, $resource, $timeout) {

    $scope.model = {};
    $scope.hotels = [];
    $scope.totalActualHotel = '';
    $scope.hotelFilterDisplayInfo = undefined;

    $scope.filterDisabled = true;
    var isFirstload = true;

    //@Url.Action("Search", "Hotel")?zzz={{departureDate}}" method="POST"
    //=============== hotel start ======================
    $scope.hotel = {};
    $scope.hotel.searchHotelType = { "location": 'Location', searchId: 'SearchId'};
    $scope.hotel.searchId = null;
    //$scope.hotel.searchId = "d3eaa926-7bb1-4a08-bfed-824b14968a50";
    $scope.hotel.location = "BALI";
    $scope.hotel.checkinDate = "12/10/2016";
    $scope.hotel.checkoutDate = "12/11/2016";
    $scope.hotel.adultCount = 3;
    $scope.hotel.childCount = 1;
    $scope.hotel.nightCount = 1;
    $scope.hotel.roomCount = 2;
    $scope.hotel.childrenAges = [];
    $scope.hotel.searchParam = '';

    $scope.searchDone = false;
    $scope.researching = false;
    $scope.filter = {};
    $scope.filter.nameFilter = "";
    $scope.filter.minPrice = 0;
    $scope.filter.maxPrice = 0;
    $scope.filter.zones = null;
    $scope.filter.stars = null;
    $scope.filter.facilities = null;
    $scope.sortByType = { "ascendingPrice": "ASCENDINGPRICE", "descendingPrice": "DESCENDINGPRICE"};
    $scope.sortBy = $scope.sortByType.ascendingPrice;

    $scope.page = 1;
    $scope.perPage = 20;
    $scope.pageCount = 0;
    $scope.totalHotelCount = 0;
    $scope.searchHeader = {};


    var resource = $resource(HotelSearchConfig.Url,
            {},
            {
                query: {
                    method: 'POST',
                    params: $scope.Hotel,
                    isArray: false
                }
            }
        );

    $scope.init = function (model) {
        $scope.model = model;
        $log.debug($scope.model);
        //$scope.hotel.searchId = $scope.model.searchId;
        $scope.hotel.location = $scope.model.location;
        $scope.hotel.checkinDate = $scope.model.checkinDate;
        $scope.hotel.checkoutDate = $scope.model.checkoutDate;
        $scope.hotel.adultCount = $scope.model.adultCount;
        $scope.hotel.childCount = $scope.model.childCount;
        $scope.hotel.nightCount = (new Date($scope.hotel.checkoutDate) - new Date($scope.hotel.checkinDate)) /(3600 * 24 * 1000);
        $scope.hotel.roomCount = $scope.model.roomCount;
        $scope.hotel.childrenAges = $scope.model.childrenAges;
        $scope.hotel.searchParam = $scope.model.searchParam;

        $scope.searchHotel();
    }

    $scope.searchHotel = function (filter, sort) {
        $log.debug('searching hotel: ');
        $scope.searchDone = false;
        $scope.pageCount = 0;

        resource.query( {}, {
            "searchHotelType": $scope.hotel.searchId == null ? $scope.hotel.searchHotelType.location : $scope.hotel.searchHotelType.searchId,
            "searchId": $scope.hotel.searchId,

            "location": $scope.hotel.location,
            "checkinDate": $scope.hotel.checkinDate,
            "checkoutDate": moment($scope.model.checkinDate).add($scope.hotel.nightCount, 'days').format("YYYY-MM-DD"),
            "nightCount": $scope.hotel.nightCount,
            "occupancies":
                [{
                    "adultCount": $scope.hotel.adultCount,
                    "childCount": $scope.hotel.childCount,
                    "roomCount": $scope.hotel.roomCount,
                    "childrenAges": $scope.hotel.childrenAges
                }], 
            "hotelFilter":
            {
                "priceFilter":
                {
                    "minPrice": $scope.filter.minPrice,
                    "maxPrice": $scope.filter.maxPrice
                },
                "zoneFilter": $scope.filter.zones,
                "starFilter": $scope.filter.stars,
                "facilityFilter": $scope.filter.facilities,
                "nameFilter": $scope.filter.nameFilter
            },
            "hotelSorting": $scope.sortBy,
            "page": $scope.page,
            "perPage": $scope.perPage,
            "regsId": $scope.regsId
        }).$promise.then(function (data) {
            var flag = false;
            if (validateResponse(data) == false) {
                return false;
            } else $scope.researching = false;

            if (data.searchId !== undefined) $scope.hotel.searchId = data.searchId;

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
            }

            //$timeout(function () { customCheckbox(); }, 0);
            $log.debug(data);
        }, function (error) {
            $log.error("error: " + error);
        }).finally(function () {
            if ($scope.researching == false) {
                $scope.searchDone = true;
            }
        });
    };

    var validateResponse = function (data) {
        if (data.error != undefined || data.error != null) {
            if (data.error == "ERHSEA02") {
                $log.debug('search id expired. researching...');
                $scope.hotel.searchId = null;
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
            "searchId=" + $scope.hotel.searchId + "&" +
            "hotelCd=" + hotelCd + "&" + 
            "searchParam=" + $scope.hotel.searchParam;
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
    
    //=============== hotel end ======================
    //$scope.$watch('hotels', function (newValue, oldValue, ccc) {
    //    $timeout(function () {
    //        $("body .col-left-hotel .img-list").each(function (i, elem) {
    //            var img = $(elem);
    //            var div = $("<div />").css({
    //                background: "url(" + img.attr("src") + ") no-repeat",
    //                width: "143px",
    //                height: "180px",
    //                "background-size": "cover",
    //                "background-position": "center"
    //            });
    //            img.replaceWith(div);
    //        });
    //    }, 0);
    //});

    //$scope.resizeImages = function () {
    //    $timeout(function () {
    //        $("body .col-left-hotel .img-list").each(function (i, elem) {
    //            var img = $(elem);
    //            var div = $("<div />").css({
    //                background: "url(" + img.attr("src") + ") no-repeat",
    //                width: "143px",
    //                height: "180px",
    //                "background-size": "cover",
    //                "background-position": "center"
    //            });
    //            img.replaceWith(div);
    //        });
    //    }, 0);
    //}

    $scope.prevPage = function () {
        $scope.page--;
        $scope.searchHotel();
    }

    $scope.nextPage = function () {
        $scope.page++;
        $scope.searchHotel();
    }

    $scope.changePage = function (pageNumber) {
        $scope.page = pageNumber;
        $scope.searchHotel();
    }

    $scope.changeSorting = function (sortBy) {
        if ($scope.searchDone && $scope.sortBy != sortBy) {
            $scope.sortBy = sortBy;
            $scope.searchHotel();
        }
    }

    $scope.filterHotels = function () {
        $scope.page = 1;
        $scope.searchHotel();

    }

}]);
