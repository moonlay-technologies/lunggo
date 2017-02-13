// home controller
app.controller('hotelSearchController', ['$scope', '$log', '$window', '$http', '$resource', '$timeout', '$interval','hotelSearchSvc',
function ($scope, $log, $window, $http, $resource, $timeout, $interval, hotelSearchSvc) {

    $(document).ready(function () {

        if (Cookies.get('hotelSearchLocationDisplay')) {
            $scope.hotelSearch.locationDisplay = Cookies.get('hotelSearchLocationDisplay');
        } else {
            $scope.hotelSearch.locationDisplay = 'Bali, Indonesia';
        }

        if (Cookies.get('hotelSearchLocation')) {
            $scope.hotelSearch.location = Cookies.get('hotelSearchLocation');
        } else {
            $scope.hotelSearch.location = 16173;
        }

        if (Cookies.get('urlCountry')) {
            $scope.hotelSearch.urlData.country = Cookies.get('urlCountry');
        } else {
            $scope.hotelSearch.urlData.country = 'Indonesia';
        }

        if (Cookies.get('urlDestination')) {
            $scope.hotelSearch.urlData.destination = Cookies.get('urlDestination');
        } else {
            $scope.hotelSearch.urlData.destination = 'Bali';
        }

        if (Cookies.get('urlZone')) {
            $scope.hotelSearch.urlData.zone = Cookies.get('urlZone');
        } else {
            $scope.hotelSearch.urlData.zone = null;
        }

        if (Cookies.get('urlArea')) {
            $scope.hotelSearch.urlData.area = Cookies.get('urlArea');
        } else {
            $scope.hotelSearch.urlData.area = null;
        }

        if (Cookies.get('urlType')) {
            $scope.hotelSearch.urlData.type = Cookies.get('urlType');
        } else {
            $scope.hotelSearch.urlData.type = 'Destination';
        }

        if (Cookies.get('hotelSearchCheckInDate')) {
            $scope.hotelSearch.checkinDate = new Date(Cookies.get('hotelSearchCheckInDate'));
            $scope.hotelSearch.checkinDateDisplay = moment($scope.hotelSearch.checkinDate).locale("id").format('LL');
            $('.ui-datepicker.checkindate').datepicker("setDate", new Date($scope.hotelSearch.checkinDateDisplay));
        } else {
            $scope.hotelSearch.checkinDate = moment().locale("id").add(5, 'days');
            $scope.hotelSearch.checkinDateDisplay = moment($scope.hotelSearch.checkinDate).locale("id").format('LL');
        }

        if (Cookies.get('hotelSearchCheckOutDate')) {
            $scope.hotelSearch.checkoutDate = new Date(Cookies.get('hotelSearchCheckOutDate'));
            $scope.hotelSearch.checkoutDateDisplay = moment($scope.hotelSearch.checkoutDate).locale("id").format('LL');
        } else {
            $scope.hotelSearch.checkoutDate = moment().locale("id").add(7, 'days');
            $scope.hotelSearch.checkoutDateDisplay = moment($scope.hotelSearch.checkoutDate).locale("id").format('LL');
        }

        if (Cookies.get('hotelSearchNights')) {
            $scope.hotelSearch.nightCount = Cookies.get('hotelSearchNights');
        } else {
            $scope.hotelSearch.nightCount = 2;
        }

        if (Cookies.get('hotelSearchRooms')) {
            $scope.hotelSearch.roomCount = Cookies.get('hotelSearchRooms');
        } else {
            $scope.hotelSearch.roomCount = 1;
        }
        var x = Cookies.getJSON('hotelSearchOccupancies');
        console.log(x);
        if (Cookies.getJSON('hotelSearchOccupancies')) {
            $scope.hotelSearch.occupancies = Cookies.getJSON('hotelSearchOccupancies');
        } else {
            $scope.hotelSearch.occupancies = [];
            for (var i = 0; i <= 7; i++) {
                $scope.hotelSearch.occupancies.push({
                    adultCount: 1,
                    childCount: 0,
                    childrenAges: [0, 0, 0, 0]
                });
            }
        }
    });
    // **************************GENERAL VARIABLES*****************************
   
    // Hotel Data
    $scope.model = {};
    $scope.hotels = [];
    $scope.totalActualHotel = '';
    $scope.hotelFilterDisplayInfo = undefined;
    $scope.totalHotelCount = 0;
    $scope.locFound = locFound;
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
    $scope.showPopularDestinations = false;
    $scope.view = {
        showHotelSearch : false
    }

    $scope.mapUrl = '';
    // ***************************************END*******************************

    // ****************************** INITS ************************************
    $scope.init = function (model) {
        //$log.debug("href = " + window.location.pathname);
        $scope.model = model;
        $log.debug($scope.model);
        $scope.mapUrl = window.location.pathname.replace("cari", "map") + '?info=' + model.searchParam;
        $("#mapUrl").attr("href", $scope.mapUrl);
        $("#mapUrl").attr("target", "_blank");
        $scope.hotelSearch.location = $scope.model.searchParamObject.location;
        $scope.hotelSearch.checkinDate = $scope.model.searchParamObject.checkinDate;
        $scope.hotelSearch.checkinDateDisplay = moment($scope.hotelSearch.checkinDate).locale("id").format('LL');
        $scope.hotelSearch.checkoutDate = $scope.model.searchParamObject.checkoutDate;
        $scope.hotelSearch.checkoutDateDisplay = moment($scope.hotelSearch.checkoutDate).locale("id").format('LL');

        $scope.hotelSearch.destinationCheckinDate = moment($scope.hotelSearch.checkinDate).locale("id").format('dddd, DD MMMM YYYY');
        $scope.hotelSearch.destinationCheckoutDate = moment($scope.hotelSearch.checkoutDate).locale("id").format('dddd, DD MMMM YYYY');
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
        if ($scope.locFound) {
            $scope.searchHotel();
        } else {
            $scope.searchDone = true;
            $scope.returnedHotelCount = 0;
        }
    }

    $('#inputLocationHotel').on('click', function () {
        $scope.showPopularDestinations = true;
    });
    hotelSearchSvc.initializeSearchForm($scope);
    // ******************************* END *************************************

    // ************************* HIGHLIGHT HOLIDAYS ****************************
    hotelSearchSvc.getHolidays();
    $('.hotel-date-picker').datepicker('option', 'beforeShowDay', hotelSearchSvc.highlightDays);
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
                $scope.changeSearch.searchId = null;
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
            $scope.expired = false;
            if (validateResponse(data) == false) {
                return false;
            } else $scope.researching = false;
            
            if (data.searchId !== undefined) {

                $scope.hotelSearch.searchId = data.searchId;
                $scope.changeSearch.searchId = data.searchId;
            }

            $scope.expiryDate = new Date(data.expTime);
            //$scope.expiryDate = new Date();
            //$scope.expiryDate = $scope.expiryDate.setMinutes($scope.expiryDate.getMinutes() + 1);
            $interval(function () {
                var nowTime = new Date();
                if (nowTime > $scope.expiryDate) {
                    $scope.expired = true;
                    
                }
            }, 1000);

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
                if (data.hotels && data.hotels.length > 0) {
                    gotoMap(data.hotels);
                }
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
            $scope.setCookie();
            hotelSearchSvc.gotoHotelSearch($scope.hotelSearch);
        }
    }

    $scope.refreshPage = function() {
        location.reload();
    }
    // ****************************** END **************************************

    // *************************** GO TO DETAIL HOTEL **************************

    $scope.GotoDetailHotel = function (hotel) {
        var url = $scope.getUrlHotelDetail(hotel);
        $window.open(url); 
    }

    $scope.getUrlHotelDetail = function(hotel) {
        var hotelName = hotel.hotelName;
        hotelName = hotelName.replace(/\s+/g, '-');
        hotelName = hotelName.replace(/[^0-9a-zA-Z-]/gi, '').toLowerCase();

        var destinationName = hotel.destinationName;
        destinationName = destinationName.replace(/\s+/g, '-');
        destinationName = destinationName.replace(/[^0-9a-zA-Z-]/gi, '').toLowerCase();
        //$log.debug('redirect to detail hotel with hotelCd: ' + hotel.hotelCd);
        var url = '/id/hotel/' + hotel.country + '/' + destinationName +
            '/' + hotelName + '-' + hotel.hotelCd + "/?" + $scope.searchParam; 

        return url;
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

    $scope.markers = [];
    function gotoMap(hotels) {

        var map = new google.maps.Map(document.getElementById('map'), {
            zoom: 14,
            center: { lat: hotels[hotels.length - 1].latitude, lng: hotels[hotels.length - 1].longitude },
            mapTypeControl: false,
            streetViewControl: false,
            fullscreenControl: false
        });

        var iconBase = '/Assets/images/hotel/markers-';
        var icons = {
            hotelRed: {
                icon: iconBase + 'red.png'
            }
        };

        var features = [];

        for (var j = 0; j < hotels.length; j++) {
            features.push({
                position: { lat: hotels[j].latitude, lng: hotels[j].longitude },
                type: 'hotelRed',
                hotelName: hotels[j].hotelName,
                image: hotels[j].mainImage,
                star: hotels[j].starRating,
                originalFare: hotels[j].originalCheapestFare,
                netFare: hotels[j].netCheapestFare,
                country: hotels[j].country,
                destinationName: hotels[j].destinationName,
                hotelCd: hotels[j].hotelCd
            });
        }

        for (var i = 0, feature; feature = features[i]; i++) {
            if (i == 0) {
                addMarker(feature, i);
            } else {
                addMarker(feature, i);
            }

        }

        function addMarker(feature, order) {
            var marker = new google.maps.Marker({
                position: feature.position,
                icon: icons[feature.type].icon,
                map: map
            });

            $scope.markers.push(marker);
            var star;
            if (feature.star == "1") {
                star = 'star';
            } else if (feature.star == "2") {
                star = 'star star-2';
            } else if (feature.star == "3") {
                star = 'star star-3';
            } else if (feature.star == "4") {
                star = 'star star-2';
            } else if (feature.star == "5") {
                star = 'star star-5';
            } else {
                star = 'star';
            }

            feature.originalFare = feature.originalFare.toFixed(0).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "1.");
            feature.netFare = feature.netFare.toFixed(0).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "1.");

            var url = $scope.getUrlHotelDetail(feature);
            var infoDesc =
                '<div class="map-wrapper">' +
                '<div class="map-container" id ="hotel-' + order + '">' +
                '<div class="hotel-round"' + 'style="' + 'background-image: url(' + feature.image + ')"></div>' +
                '<div class="map-content normal-txt">' +
                '<div class="hotel-title bold-txt blue-txt">' + feature.hotelName + '</div>' +
                '<div class="' + star + '"></div>' +
                '<div class="orange-txt sm-txt underline-txt">Rp ' + feature.originalFare + '</div>' +
                '<div class="orange-txt bold-txt md-txt"><sup>Rp </sup>' + feature.netFare + '</div>' +
                '</div>' +
                '<div class="map-button">' + '<a class="btn btn-yellow sm-btn"' + 'href="' + url + '"' + '>PESAN</a>' + '</div>' +
                '</div>';


            var infoWindow = new google.maps.InfoWindow({
                content: infoDesc,
                maxWidth: 350,
            });

            //if (type == 'first') {
            //infoWindow.open(map, marker);
            //}

            marker.addListener('click', function () {
                infoWindow.open(map, marker);
                var iwOuter = $('.gm-style-iw').addClass('custom');
                var p = $(this).closest('.gm-style-iw');
                var q = p.siblings();
                //$('.map-container').hide();
                $('#hotel-' + order).show();
                $('.map-container').not('#hotel-' + order).hide();
                p.find('.map-container').show();
                p.find('.map-price').hide();
                q.find('.map-container').hide();
                q.find('.map-price').show();

                var iwBackground = iwOuter.prev();
                iwBackground.children().css({ 'display': 'none' });

                var iwCloseBtn = iwOuter.next();
                iwCloseBtn.css({ display: 'none' });
            });

            //infoWindow.addListener('domready', function () {
            //    var iwOuter = $('.gm-style-iw').addClass('custom');

            //    $('.map-container').hide();

            //    $('.map-price').click(function () {
            //        var p = $(this).closest('.gm-style-iw').parent();
            //        var q = p.siblings();

            //        p.find('.map-container').show();
            //        p.find('.map-price').hide();
            //        q.find('.map-container').hide();
            //        q.find('.map-price').show();
            //    });

            //    var iwBackground = iwOuter.prev();
            //    iwBackground.children().css({ 'display': 'none' });

            //    var iwCloseBtn = iwOuter.next();
            //    iwCloseBtn.css({ display: 'none' });
            //});


        }
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