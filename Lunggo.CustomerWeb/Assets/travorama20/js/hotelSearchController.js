// home controller
app.controller('hotelSearchController', ['$scope', '$log', '$http', '$resource', function ($scope, $log, $http, $resource) {

    $scope.model = {};
    $scope.hotels = [];
    $scope.totalActualHotel = '';
    $scope.hotelFilterDisplayInfo = undefined;

    $scope.filterDisabled = true;


    //@Url.Action("Search", "Hotel")?zzz={{departureDate}}" method="POST"
    //=============== hotel start ======================
    $scope.hotel = {};
    $scope.hotel.searchId = null;
    $scope.hotel.location = "BALI";
    $scope.hotel.checkinDate = "12/10/2016";
    $scope.hotel.checkoutDate = "12/11/2016";
    $scope.hotel.adultCount = 3;
    $scope.hotel.childCount = 1;
    $scope.hotel.nightCount = 1;
    $scope.hotel.roomCount = 2;

    $scope.filter = {};
    $scope.filter.minPrice = 0;
    $scope.filter.maxPrice = 0;
    $scope.filter.zones = null;
    $scope.filter.stars = [];
    $scope.sorting = '';

    $scope.searchHeader = {};


    var resource = $resource('//api.local.travorama.com/v1/hotel/search',
            {},
            {
                query: {
                    method: 'POST',
                    params: {},
                    isArray: false
                }
            }
        );

    $scope.init = function (model) {
        $scope.model = model;
        $log.debug($scope.model);

        $scope.hotel.location = $scope.model.location;
        $scope.hotel.checkinDate = $scope.model.checkinDate;
        $scope.hotel.checkoutDate =$scope.model.checkoutDate;
        $scope.hotel.adultCount = $scope.model.adultCount;
        $scope.hotel.childCount = $scope.model.childCount;
        $scope.hotel.nightCount = $scope.model.nightCount
        $scope.hotel.roomCount = $scope.model.roomCount;

        $scope.searchHotel();
    }

    $scope.searchHotel = function (filter, sort) {
        $log.debug('searching hotel');

        resource.query({}, {
            "searchId": $scope.hotel.searchId,

            "location": $scope.hotel.location,
            "checkinDate": $scope.hotel.checkinDate,
            "checkoutDate": $scope.hotel.checkoutDate,
            "adultCount": $scope.hotel.adultCount,
            "childCount": $scope.hotel.childCount,
            "nightCount": $scope.hotel.nightCount,
            "roomCount": $scope.hotel.roomCount,
            "from": "1",
            "to": "10",

            "hotelFilter": 
                {
                    "priceFilter":
                        {
                            "minPrice": $scope.filter.minPrice,
                            "maxPrice": $scope.filter.maxPrice
                        },
                    "zoneFilter":
                        {
                            "zones": $scope.filter.zones
                        },
                    "starFilter":
                        {
                            "stars": $scope.filter.stars
                        }
                },
            "hotelSorting": $scope.sorting
            //"hotelSorting": "ASCENDINGPRICE"
        }).$promise.then(function (data) {

            $scope.filter.minPrice = data.minPrice;
            $scope.filter.maxPrice = data.maxPrice;
            $scope.hotel.searchId = data.searchId;
            $scope.hotels = data.hotels;
            totalActualHotel = data.totalActualHotel;
            $scope.hotelFilterDisplayInfo = data.hotelFilterDisplayInfo;

            initiatePriceSlider();
            $log.debug(data);
        })

        //if (filter != undefined || sort != undefined) {
        //    location.href = '/id/Hotel/Search/' + $scope.hotel.filterParam();
        //}
        //else {
        //    location.href = '/id/Hotel/Search/' + $scope.hotel.searchParam();
        //}
    };

    $scope.hotel.searchParam = function () {
        return ("?info=" +
            [$scope.hotel.location,
             $scope.hotel.checkinDate,
             $scope.hotel.checkoutDate,
             $scope.hotel.adultCount,
             $scope.hotel.childCount,
             $scope.hotel.nightCount,
             $scope.hotel.roomCount].join('.')
        )
    }

    $scope.GotoDetailHotel = function (hotelCd) {
        $log.debug('redirect to detail hotel with hotelCd: ' + hotelCd);
        location.href = '/id/Hotel/DetailHotel/?' +
            "searchId=" + $scope.hotel.searchId + "&" +
            "hotelCd=" + hotelCd;
    }

    $scope.changeFilter = function (star) {
        if ($scope.filter.stars == null) {
            $scope.filter.stars = [star];
        }
        else {
            var starIndex = $scope.filter.stars.indexOf(star);
            if (starIndex < 0) {
                $scope.filter.stars.push(star);
            }
            else {
                $scope.filter.stars.splice(starIndex, 1);
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

}]);
