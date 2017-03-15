// home controller
app.controller('hotelSearchController', ['$scope', '$log', '$window', '$http', '$resource', '$timeout', '$interval','hotelSearchSvc',
function ($scope, $log, $window, $http, $resource, $timeout, $interval, hotelSearchSvc) {

    $(document).ready(function () {

        if (Cookies.get('hotelLocationDisplay')) {
            $scope.hotelSearch.locationDisplay = Cookies.get('hotelLocationDisplay');
            if (Cookies.get('hotelLocation')) {
                $scope.hotelSearch.location = Cookies.get('hotelLocation');
            } else {
                $scope.hotelSearch.location = 1316553;
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
        } else {
            $scope.hotelSearch.locationDisplay = 'Bali, Indonesia';
            $scope.hotelSearch.urlData.country = 'Indonesia';
            $scope.hotelSearch.urlData.destination = 'Bali';
            $scope.hotelSearch.urlData.zone = null;
            $scope.hotelSearch.urlData.area = null;
            $scope.hotelSearch.urlData.type = 'Destination';
        }

        if (Cookies.get('hotelSearchCheckInDate')) {
            if (new Date(Cookies.get('hotelSearchCheckInDate')) < new Date()) {
                $scope.hotelSearch.checkinDate = moment().locale("id").add(5, 'days');
                $scope.hotelSearch.checkinDateDisplay = moment($scope.hotelSearch.checkinDate).locale("id").format('LL');
            } else {
                $scope.hotelSearch.checkinDate = new Date(Cookies.get('hotelSearchCheckInDate'));
                $scope.hotelSearch.checkinDateDisplay = moment($scope.hotelSearch.checkinDate).locale("id").format('LL');
                $('.ui-datepicker.checkindate').datepicker("setDate", new Date($scope.hotelSearch.checkinDateDisplay));
            }
        } else {
            $scope.hotelSearch.checkinDate = moment().locale("id").add(5, 'days');
            $scope.hotelSearch.checkinDateDisplay = moment($scope.hotelSearch.checkinDate).locale("id").format('LL');
        }

        if (Cookies.get('hotelSearchCheckOutDate')) {
            if (new Date(Cookies.get('hotelSearchCheckOutDate')) < new Date(new Date().getTime() + 24 * 60 * 60 * 1000)) {
                $scope.hotelSearch.checkoutDate = moment().locale("id").add(7, 'days');
                $scope.hotelSearch.checkoutDateDisplay = moment($scope.hotelSearch.checkoutDate).locale("id").format('LL');
            } else {
                $scope.hotelSearch.checkoutDate = new Date(Cookies.get('hotelSearchCheckOutDate'));
                $scope.hotelSearch.checkoutDateDisplay = moment($scope.hotelSearch.checkoutDate).locale("id").format('LL');
            }
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
   
    $scope.gtmContentType = 'hotel';
    $scope.gtmCity = gtmCity;
    $scope.gtmRegion = gtmRegion;
    $scope.gtmCountry = gtmCountry;
    $scope.gtmCheckinDate = gtmCheckinDate;
    $scope.gtmCheckoutDate = gtmCheckoutDate;
    $scope.gtmNumAdults = gtmNumAdults;
    $scope.gtmNumChildren = gtmNumChildren;
    $scope.gtmPurchaseCurrency = gtmPurchaseCurrency;
    $scope.gtmPageValue = null;
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

    $scope.checkHotel = function(hotels) {
        var allContained = true;
        for (var i = 0; i < hotels.length; i++) {
            var hotelName = hotels[i].hotelName.toLowerCase();
            if ($scope.filter.nameFilter != null && $scope.filter.nameFilter !== '') {
                if (hotelName.indexOf($scope.filter.nameFilter.toLowerCase()) == -1) {
                    allContained = false;
                    break;
                }
            }        
        }

        return allContained;
    }
    $scope.searchHotel = function (filter, sort, isMobile) {
        $('#perRoom').addClass("active");
        $('#total').removeClass("active");
        $scope.searchDone = false;
        $scope.pageCount = 0;

        $timeout(function () { }, 1);
        searchPromise().then(function (data) {
            $scope.expired = false;
            if (validateResponse(data) == false) {
                return false;
            } else $scope.researching = false;
            $scope.hotels = [];
            if (data.hotels != null) {
                if ($scope.checkHotel(data.hotels)) {
            if (data.searchId !== undefined) {

                $scope.hotelSearch.searchId = data.searchId;
                $scope.changeSearch.searchId = data.searchId;
            }

            $scope.expiryDate = new Date(data.expTime);
                    $interval(function() {
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

                        var hotelId = [];
                        if (data.totalHotelCount >= 10) {
                            for (var i = 0; i < 10; i++) {
                                hotelId.push(data.hotels[i].hotelCd.toString());
                            }
                        }else if (data.totalHotelCount > 0 && data.totalHotelCount < 10) {
                            for (var i = 0; i < data.totalHotelCount; i++) {
                                hotelId.push(data.hotels[i].hotelCd.toString());
                            }
                        }
                        
                        !function (f, b, e, v, n, t, s) {
                            if (f.fbq) return; n = f.fbq = function () {
                                n.callMethod ?
                                n.callMethod.apply(n, arguments) : n.queue.push(arguments)
                            }; if (!f._fbq) f._fbq = n;
                            n.push = n; n.loaded = !0; n.version = '2.0'; n.queue = []; t = b.createElement(e); t.async = !0;
                            t.src = v; s = b.getElementsByTagName(e)[0]; s.parentNode.insertBefore(t, s)
                        }(window,document, 'script', '//connect.facebook.net/en_US/fbevents.js');

                        //fbq('init', '<FB_PIXEL_ID>');

                        fbq('track', 'Search', {
                            content_type: 'hotel',
                            content_ids: hotelId,
                            checkin_date: $scope.gtmCheckinDate,
                            checkout_date: $scope.gtmCheckoutDate,
                            city: $scope.gtmCity,
                            region: $scope.gtmRegion,
                            country: $scope.gtmCountry,
                            num_adults: $scope.gtmNumAdults,
                            num_children: $scope.gtmNumChildren,
                            purchase_value: data.minPrice,
                            purchase_currency: $scope.gtmPurchaseCurrency,
                        });
            };
                    $scope.searchDone = true;
                    $scope.finishLoad = true;
                } else {
                    $scope.searchDone = false;
                }
            } else {
                if ($scope.researching == false) {
                    $scope.searchDone = true;
                    $scope.finishLoad = true;
                } else {
                    $scope.searchDone = false;
                    $scope.finishLoad =false;
                }
                
            }

            $log.debug(data);
        }, function (error) {
            $log.error("error: " + error);
        }).finally(function () {
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
    
    //setup before functions
    var typingTimer;                //timer identifier
    var doneTypingInterval = 500;  //time in ms (5 seconds)

    $scope.$watch('filter.nameFilter', function (newValue, oldValue, scope) {
        clearTimeout(typingTimer);
        typingTimer = setTimeout(filterHotels, doneTypingInterval);
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
