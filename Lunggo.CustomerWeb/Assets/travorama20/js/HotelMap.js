app.controller('hotelMapController', [
    '$scope', '$log', '$window', '$http', '$resource', '$timeout', '$interval', 'hotelSearchSvc',
    function ($scope, $log, $window, $http, $resource, $timeout, $interval, hotelSearchSvc) {

        $scope.model = {};
        $scope.hotels = [];
        $scope.totalActualHotel = '';
        $scope.hotelFilterDisplayInfo = undefined;
        $scope.totalHotelCount = 0;
        $scope.isDestination = isDestination;
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
        if ($scope.isDestination == true) {
            $(".filter-trigger > div").css("width", "25%");
        } else {
            $(".filter-trigger > div").css("width", "33.333333%");
        }
        $(".hotel-sort-filter-action > a").css("width", "50%");
        var isFirstload = true;
        $scope.bottomPage = false;
        $scope.searchDone = false;
        $scope.finishLoad = false;
        $scope.page = 1;
        $scope.perPage = 10000;
        $scope.pageCount = 1;
        $scope.researching = false;
        $scope.returnUrl = "/";
        $scope.expired = false;
        $scope.showPopularDestinations = false;
        $scope.view = {
            showHotelSearch: false
        }
        $scope.init = function (model) {
            $scope.model = model;
            $log.debug($scope.model);
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

            $scope.searchParam = model.searchParam;
            $scope.searchHotel();

        }

        hotelSearchSvc.initializeSearchForm($scope);

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
                if ($scope.returnedHotelCount == 0) {
                    $scope.hotels = [];
                }
                $scope.filteredHotelCount = data.filteredHotelCount;

                if (data.page > $scope.page) {
                    $scope.page = data.page;
                }

                if (data.hotels && data.hotels.length > 0) {
                    gotoMap(data.hotels);
                }
                $scope.perPage = data.perPage;
                $scope.pageCount = data.pageCount;
                $scope.totalHotelCount = data.totalHotelCount;

                if (isFirstload) {
                    $scope.filter.minPrice = data.minPrice;
                    $scope.filter.maxPrice = data.maxPrice;
                    $scope.minPrice = data.minPrice;
                    $scope.maxPrice = data.maxPrice;
                    $scope.hotelFilterDisplayInfo = data.hotelFilterDisplayInfo;
                    isFirstload = false;

                };
                initiatePriceSlider();
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

        var searchPromise = function () {
            var pagination = { 'sortBy': $scope.sortBy, 'page': $scope.page, 'perPage': $scope.perPage };
            return hotelSearchSvc.searchHotel($scope.hotelSearch, $scope.filter, pagination).$promise.then(function (data) {
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

                    //$scope.searchHotel();
                    return false;
                }
            }

            return true;
        };
        $scope.markers = [];
        $.each($scope.markers, function (i, marker) {
            marker.click(function () {
                alert(marker);
            });
        });
        $scope.getUrlHotelDetail = function (hotel) {
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

        $scope.GotoDetailHotel = function (hotel) {
            var url = $scope.getUrlHotelDetail(hotel);
            $window.open(url);
        }

        $scope.toTitleCase = function (str) {
            return str.replace(/\w\S*/g, function (txt)
            { return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase(); });
        }

        function gotoMap(hotels) {
            $scope.markers = [];
            var map = new google.maps.Map(document.getElementById('map'), {
                zoom: 12,
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
            var locations = [];
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
                locations.push({ lat: hotels[j].latitude, lng: hotels[j].longitude });
            }

            for (var i = 0, feature; feature = features[i]; i++) {
                if (i == 0) {
                    addMarker(feature, i);
                } else {
                    addMarker(feature, i);
                }
            }
           
            // Add a marker clusterer to manage the markers.
            var markerCluster = new MarkerClusterer(map, $scope.markers,
                {imagePath: 'https://developers.google.com/maps/documentation/javascript/examples/markerclusterer/m'});
        
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
                    '<div class="map-container" id ="hotel-'+ order +'">' +
                    '<div class="hotel-round" hotel-list-image ' + 'style="' + 'background-image: url(' + feature.image + ')"></div>' +
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

                marker.addListener('click', function () {
                    infoWindow.open(map, marker);
                    var iwOuter = $('.gm-style-iw').addClass('custom');
                    var p = $(this).closest('.gm-style-iw');
                    var q = p.siblings();
                    //$('.map-container').hide();
                    if ($('#hotel-' + order).hasClass("showThisHotel")) {
                        $('#hotel-' + order).removeClass("showThisHotel");
                        $('#hotel-' + order).hide();
                    } else {
                        $('#hotel-' + order).addClass("showThisHotel");
                        $('#hotel-' + order).show();
                    }
                    
                    $('.map-container').not('#hotel-' + order).hide();
                    $('.map-container').not('#hotel-' + order).removeClass("showThisHotel");
                    p.find('.map-container').show();
                    p.find('.map-price').hide();
                    q.find('.map-container').hide();
                    q.find('.map-price').show();

                    var iwBackground = iwOuter.prev();
                    iwBackground.children().css({ 'display': 'none' });

                    var iwCloseBtn = iwOuter.next();
                    iwCloseBtn.css({ display: 'none' });
                });

                
            }
            google.maps.event.trigger(map, 'resize');
            //google.maps.event.addListenerOnce(map, 'idle', function () {
            //    google.maps.event.trigger(map, 'resize');
            //    map.setCenter(location);
            //});
        }

        $scope.refreshPage = function () {
            location.reload();
        }

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
            $('#slider-range').slider({
                range: true,
                min: $scope.minPrice,
                max: $scope.maxPrice,
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

        $scope.resetPrice = function() {
            $scope.filter.minPrice = $scope.minPrice - ($scope.minPrice % 100000);
            $scope.filter.maxPrice = $scope.maxPrice + (100000 - $scope.maxPrice % 100000);
            $('.slider-range').slider({
                values: [$scope.filter.minPrice, $scope.filter.maxPrice]
            });
        }
        $scope.selectAll = function (type) {
            if (type == 'zone') {
                for (var x = 0; x < $scope.hotelFilterDisplayInfo.zoneFilter.length; x++) {
                    if ($scope.filter.zones.indexOf($scope.hotelFilterDisplayInfo.zoneFilter[x].code) <= -1) {
                        $scope.filter.zones.push($scope.hotelFilterDisplayInfo.zoneFilter[x].code);
                    }
                }
            }

            else if (type == 'facility') {
                for (var x = 0; x < $scope.hotelFilterDisplayInfo.facilityFilter.length; x++) {
                    if ($scope.filter.facilities.indexOf($scope.hotelFilterDisplayInfo.facilityFilter[x].code) <= -1) {
                        $scope.filter.facilities.push($scope.hotelFilterDisplayInfo.facilityFilter[x].code);
                    }
                }
            }

            else if (type == 'star') {
                for (var x = 0; x < $scope.hotelFilterDisplayInfo.starFilter.length; x++) {
                    if ($scope.filter.stars.indexOf($scope.hotelFilterDisplayInfo.starFilter[x].code) <= -1) {
                        $scope.filter.stars.push($scope.hotelFilterDisplayInfo.starFilter[x].code);
                    }
                }
            }

        }
    }]);

jQuery(document).ready(function ($) {
    // Search List Image
    $(function () {
        $("body .img-list").each(function (i, elem) {
            var img = $(elem);
            var div = $("<div />").css({
                background: "url(" + img.attr("src") + ") no-repeat",
                width: "100%",
                height: "125px",
                "background-size": "cover",
                "background-position": "center"
            });
            img.replaceWith(div);
        });
    });

    $('body .dropdown').click(function () {
        $('body .option').toggle();
        $('.hotel-sort').text();
        $('.form-hotel-sort .option > span').click(function () {
            $('.hotel-sort').text(this.innerHTML);
        });
    });

    $('.zoom-map').click(function () {
        var point = $(this).closest('.search-list-result').find('.row-content');
        point.children('.search-list').hide();
        point.children('.search-map').removeClass('col-half');
        google.maps.event.trigger(map, "resize");
        //$("#map").css("width", "885px");
    });

    $('.list-map').click(function () {
        var point = $(this).closest('.search-list-result').find('.row-content');
        point.children('.search-list').show();
        point.children('.search-map').addClass('col-half');
        google.maps.event.trigger(map, "resize");
        //$("#map").css("width", "433px");
    });

    //Show tab filter
    $('body .switch-filter').click(function () {
        var trig = $('.filter-trigger');
        var filter = $('.filter-content');

        if ($(this).is('#filter-star')) {
            trig.find('.filter-star').addClass('active');
            trig.find('.filter-star').siblings().removeClass('active');

            filter.find('.filter-star-content').addClass('active');
            filter.find('.filter-star-content').siblings().removeClass('active');
        } else if ($(this).is('#filter-price')) {
            trig.find('.filter-price').addClass('active');
            trig.find('.filter-price').siblings().removeClass('active');

            filter.find('.filter-price-content').addClass('active');
            filter.find('.filter-price-content').siblings().removeClass('active');
        } else if ($(this).is('#filter-area')) {
            trig.find('.filter-area').addClass('active');
            trig.find('.filter-area').siblings().removeClass('active');

            filter.find('.filter-area-content').addClass('active');
            filter.find('.filter-area-content').siblings().removeClass('active');
        } else if ($(this).is('#filter-facility')) {
            trig.find('.filter-facility').addClass('active');
            trig.find('.filter-facility').siblings().removeClass('active');

            filter.find('.filter-facility-content').addClass('active');
            filter.find('.filter-facility-content').siblings().removeClass('active');
        }
    });

    //Close tab filter
    $('body .close-filter').click(function () {
        $('.filter-trigger div').removeClass('active');
        $('.fc-wrapper').removeClass('active');
    });

    $('body .select-none').on('click', function () {
        var p = $(this).closest('.fc-title').parent().find('.tab-detail');
        var c = p.find('.sqr');
        var i = c.find('.check');

        i.prop('checked', false);
        c.removeClass('active');
    });

    $('.map-container').click(function () {
        ('.map-container').hide();
    });

    $('.open-txt').click(function () {
        $(this).toggleClass('active');
        $(this).parent().find('.short-txt').toggleClass('open');
    });
});