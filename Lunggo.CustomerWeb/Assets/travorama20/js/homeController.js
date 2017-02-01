// home controller
app.controller('homeController', ['$scope', '$log', '$http', '$location', '$resource', '$timeout', 'hotelSearchSvc', function ($scope, $log, $http, $location, $resource, $timeout, hotelSearchSvc) {

    
    $(document).ready(function() {

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
   // ================= FLIGHT ========================

    $scope.departureDate = moment().add(1, 'day').format('DDMMYY');
    $scope.changeTab = function (tab) {
        if (tab == 'hotel') {
            $('.search-location').hide();
            $('.search-calendar').hide();
        } else if (tab == 'flight') {
            $scope.view.showHotelSearch = false;
            $('.search-calendar-hotel').hide();
            $('.form-child-age').hide();
        }
    }
    
    $scope.showForm= function(tab) {
        if (tab == 'hotel') {
            $scope.isFlight = false;
        } else if (tab == 'flight') {
            $scope.isFlight = true;
        }
    }


    //=============== Price Calendar, populate cheapest price for destinations ======================


    $scope.priceFlight =
    {
        Denpasar: {
            CheapestDate: '',
            CheapestPrice: ''
        },
        Surabaya: {
            CheapestDate: '',
            CheapestPrice: ''
        },
        Jakarta: {
            CheapestDate: '',
            CheapestPrice: ''
        },
        Medan: {
            CheapestDate: '',
            CheapestPrice: ''
        }
    };

    $scope.priceHotel =
    {
        Bali: {
            CheapestDate: '',
            CheapestPrice: ''
        },
        Surabaya: {
            CheapestDate: '',
            CheapestPrice: ''
        },
        Jakarta: {
            CheapestDate: '',
            CheapestPrice: ''
        },
        Yogyakarta: {
            CheapestDate: '',
            CheapestPrice: ''
        },
        Bandung: {
            CheapestDate: '',
            CheapestPrice: ''
        }
    };

    $scope.gotoCheapestDateFlight = function (dest, depdate) {
        var date = new Date(depdate);
        var datex = ("0" + date.getDate()).slice(-2) + ("0" + (date.getMonth() + 1)).slice(-2) + date.getFullYear().toString().substr(2, 2);
        if (dest == 'DPS')
            return '/id/tiket-pesawat/cari/Jakarta-Denpasar-JKT-DPS/JKTDPS' + datex + '-100y';
        else if (dest == 'KNO')
            return '/id/tiket-pesawat/cari/Jakarta-Medan-JKT-KNO/JKTKNO' + datex + '-100y';
        else if (dest == 'SUB')
            return '/id/tiket-pesawat/cari/Jakarta-Surabaya-JKT-SUB/JKTSUB' + datex + '-100y';
        else if (dest == 'JKT')
            return '/id/tiket-pesawat/cari/Denpasar-Jakarta-DPS-JKT/DPSJKT' + datex + '-100y';
    }

    $scope.gotoCheapestDateHotel = function (dest, depdate) {
        var date = new Date();
        var datex = date.getFullYear() + '-' + ("0" + (date.getMonth() + 1)).slice(-2) + '-' + ("0" + date.getDate()).slice(-2);
        var nextdate = new Date();
        nextdate.setDate(date.getDate() + 1);
        var datey = nextdate.getFullYear() + '-' + ("0" + (nextdate.getMonth() + 1)).slice(-2) + '-' + ("0" + nextdate.getDate()).slice(-2);
        if (dest == 'JKT')
            window.location.href = '/id/hotel/cari/Indonesia/Jakarta/?info=Location.-2001512190.' + datex + '.' + datey + '.1.1.2~0';
        else if (dest == 'SUB')
            window.location.href = '/id/hotel/cari/Indonesia/Surabaya/?info=Location.681103437.' + datex + '.' + datey + '.1.1.2~0';
        else if (dest == 'JOG')
            window.location.href = '/id/hotel/cari/Indonesia/Yogyakarta/?info=Location.1439962957.' + datex + '.' + datey + '.1.1.2~0';
        else if (dest == 'BAI')
            window.location.href = '/id/hotel/cari/Indonesia/Bali/?info=Location.1890170571.' + datex + '.' + datey + '.1.1.2~0';
        else if (dest == 'MFM')
            window.location.href = '/id/hotel/cari/IMakau/Macau/?info=Location.-79525572.' + datex + '.' + datey + '.1.1.2~0';
    }
    
    $scope.getCheapestHotelPrice = function(location) {
        var authAccess = getAuthAccess();
        var date = new Date(), y = date.getFullYear(), m = date.getMonth();
        var lastDay = new Date(y, m + 1, 0);
        var startDate = ("0" + date.getDate()).slice(-2)
             + ("0" + (date.getMonth() + 1)).slice(-2) + y.toString().substring(2, 4);
        var endDate = ("0" + lastDay.getDate()).slice(-2)
             + ("0" + (lastDay.getMonth() + 1)).slice(-2) + y.toString().substring(2, 4);
        if (authAccess == 1 || authAccess == 2) {
            $.ajax({
                url: HotelPriceCalendarConfig.Url + '/' + location + '/' + startDate
                + '/' + endDate + '/IDR',
                method: 'GET',
                headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
            }).done(function (returnData) {
                var cheapestPrice = returnData.cheapestPrice;
                var cheapestDate;
                if (returnData.cheapestDate != null && returnData.cheapestDate != '') {
                    cheapestDate = new Date(returnData.cheapestDate);
                    if (location == 'BAI') {
                        $scope.priceHotel.Bali.cheapestDate = cheapestDate;
                        $scope.priceHotel.Bali.cheapestPrice = cheapestPrice;
                    } else  if (location == 'BDO') {
                        $scope.priceHotel.Bandung.cheapestDate = cheapestDate;
                        $scope.priceHotel.Bandung.cheapestPrice = cheapestPrice;
                    } else if (location == 'JOG') {
                        $scope.priceHotel.Yogyakarta.cheapestDate = cheapestDate;
                        $scope.priceHotel.Yogyakarta.cheapestPrice = cheapestPrice;
                    }
                    else if (location == 'JAV') {
                        $scope.priceHotel.Jakarta.cheapestDate = cheapestDate;
                        $scope.priceHotel.Jakarta.cheapestPrice = cheapestPrice;
                    }
                    else if (location == 'SUB') {
                        $scope.priceHotel.Surabaya.cheapestDate = cheapestDate;
                        $scope.priceHotel.Surabaya.cheapestPrice = cheapestPrice;
                    }
                } else {
                   // return [-1, ''];
                }

                //}

            }).error(function (returnData) {
               // return [-1, ''];
            });
        }
    }

    $scope.getCheapestHotelPrice('JAV');
    $scope.getCheapestHotelPrice('BDO');
    $scope.getCheapestHotelPrice('SUB');
    $scope.getCheapestHotelPrice('BAI');
    $scope.getCheapestHotelPrice('JOG');

    $scope.getCheapestFlightPrice = function(origin, destination) {
        var authAccess = getAuthAccess();
        var date = new Date(), y = date.getFullYear(); m = date.getMonth();
        var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
        //lastDay.setDate(date.getDate() + 1);
        var startDate = ("0" + date.getDate()).slice(-2)
             + ("0" + (date.getMonth() + 1)).slice(-2) + y.toString().substring(2, 4);
        var endDate = ("0" + lastDay.getDate()).slice(-2)
             + ("0" + (lastDay.getMonth() + 1)).slice(-2) + y.toString().substring(2, 4);
        
        if (authAccess == 1 || authAccess == 2) {
            $.ajax({
                url: FlightPriceCalendarConfig.Url + '/' + origin + destination + '/' + startDate
                + '/' + endDate + '/IDR',
                method: 'GET',
                headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
            }).done(function (returnData) {
                //if (returnData.status == 200) {
                var cheapestPrice = returnData.cheapestPrice;
                var cheapestDate;
                if (returnData.cheapestDate != null && returnData.cheapestDate != '') {
                    cheapestDate = new Date(returnData.cheapestDate);
                    if (origin == 'DPS' && destination == 'JKT') {
                        $scope.priceFlight.Jakarta.CheapestPrice = cheapestPrice;
                        $scope.priceFlight.Jakarta.CheapestDate = cheapestDate;
                       
                    } else if (origin == 'JKT' && destination == 'SUB') {
                        $scope.priceFlight.Surabaya.CheapestPrice = cheapestPrice;
                        $scope.priceFlight.Surabaya.CheapestDate = cheapestDate;
                    }
                    else if (origin == 'JKT' && destination == 'KNO') {
                        $scope.priceFlight.Medan.CheapestPrice = cheapestPrice;
                        $scope.priceFlight.Medan.CheapestDate = cheapestDate;
                    }
                    else if (origin == 'JKT' && destination == 'DPS') {
                        $scope.priceFlight.Denpasar.CheapestPrice = cheapestPrice;
                        $scope.priceFlight.Denpasar.CheapestDate = cheapestDate;
                    }
                    
                    $scope.listCheapestPrice = [];
                    for (var x = 0; x < returnData.listDatesAndPrices.length; x++) {
                        $scope.listCheapestPrice.push(returnData.listDatesAndPrices[x].price);
                    }
                    addCustomInformation(m, y);
                    //return [cheapestPrice, cheapestDate];
                }else {
                    //return [-1, ''];
                }

                //}

            }).error(function (returnData) {
                //return [-1, ''];
            });
        }
    }

    var listCheapestPrice = [30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30];
    $scope.selectedPopularDestination = {
        origin: '',
        destination: '',
        cheapestDate: '',
        cheapestPrice: '',
    }

    $scope.listCheapestPrice = [];
    $scope.getCheapestFlightPrice('DPS', 'JKT');
    $scope.getCheapestFlightPrice('JKT', 'DPS');
    $scope.getCheapestFlightPrice('JKT', 'SUB');
    $scope.getCheapestFlightPrice('JKT', 'KNO');
    //=============== hotel start ======================
    $scope.showPopularDestinations = false;
    hotelSearchSvc.initializeSearchForm($scope);
    hotelSearchSvc.getHolidays();
    var holidays = hotelSearchSvc.holidays;
    var holidayNames = hotelSearchSvc.holidayNames;
    function highlight(date) {
        for (var i = 0; i < holidays.length; i++) {
            var x = new Date(date);
            var xmonth = x.getMonth();
            var xday = x.getDate();
            var xyear = x.getFullYear();

            var y = new Date(holidays[i]);
            var ymonth = y.getMonth();
            var yday = y.getDate();
            var yyear = y.getFullYear();

            if (xmonth == ymonth && xday == yday && xyear == yyear) {
                return [true, 'ui-state-holiday', holidayNames[i]];
            }
        }
        return [true, ''];
        
    }
    $(function () {
        $(".ui-datepicker").datepicker({
            beforeShowDay: highlight,
        });
    });

   
    $('.hotel-date-picker').datepicker('option', 'beforeShowDay', hotelSearchSvc.highlightDays);
    $scope.hotel = {};
    $scope.view = {
        showHotelSearch : false
    }

    $scope.init = function (model) {
        $log.debug(model);
    }

    $scope.hotel.searchHotel = function () {
        for (var k = $scope.hotelSearch.roomCount; k < 8; k++) {
            $scope.hotelSearch.occupancies[k].adultCount = 1;
            $scope.hotelSearch.occupancies[k].childCount = 0;
            $scope.hotelSearch.occupancies[k].childrenAges = [0, 0, 0, 0];
        }
        setCookie();
        hotelSearchSvc.gotoHotelSearch($scope.hotelSearch);
    };

    $scope.HotelSearchForm = {
        AutoComplete: {
            Keyword: '',
            MinLength: 3,
            GetLocation: function () {
                $scope.getLocation($scope.HotelSearchForm.AutoComplete.Keyword);
            },
            PopularDestinations: hotelSearchSvc.PopularDestinations
        },
    }

    $('.form-hotel-location').click(function () {
        $(this).select();
    });

    $('#inputLocationHotel').on('click', function () {
        $scope.showPopularDestinations = true;
    });

    function setCookie() {
        Cookies.set('hotelSearchLocationDisplay', $scope.hotelSearch.locationDisplay, { expires: 9999 });
        Cookies.set('hotelSearchLocation', $scope.hotelSearch.location, { expires: 9999 });
        Cookies.set('hotelSearchCheckInDate', $scope.hotelSearch.checkinDate, { expires: 9999 });
        Cookies.set('hotelSearchCheckOutDate', $scope.hotelSearch.checkoutDate, { expires: 9999 });
        Cookies.set('hotelSearchNights', $scope.hotelSearch.nightCount, { expires: 9999 });
        Cookies.set('hotelSearchRooms', $scope.hotelSearch.roomCount, { expires: 9999 });
        Cookies.set('hotelSearchOccupancies', $scope.hotelSearch.occupancies, { expires: 9999 });
        Cookies.set('urlCountry', $scope.hotelSearch.urlData.country, { expires: 9999 });
        Cookies.set('urlDestination', $scope.hotelSearch.urlData.destination, { expires: 9999 });
        Cookies.set('urlZone', $scope.hotelSearch.urlData.zone, { expires: 9999 });
        Cookies.set('urlArea', $scope.hotelSearch.urlData.area, { expires: 9999 });
        Cookies.set('urlType', $scope.hotelSearch.urlData.type, { expires: 9999 });
    }

    //=============== hotel end ======================

    //function addCustomInformation() {
    //    setTimeout(function () {
    //        var price = $(".ui-datepicker-calendar td").filter(function () {
    //            var date = $(this).text();
    //            var tesss = /\d/.test(date);
    //            return /\d/.test(date);
    //        });

    //        price.find('a').attr('data-custom', 20);
    //        price.append('<a class="view-price btn btn-yellow sm-btn xs-txt os-bold">LIHAT</a>');

    //        $('.ui-datepicker .ui-datepicker-title').addClass('col-xs-4');
    //    }, 0);
    //}

    
    $("#pc-datepicker").datepicker({
        changeMonth: true,
        changeYear: true,
        dayNamesMin: ["MGG", "SEN", "SEL", "RAB", "KAM", "JUM", "SAB"],
        showOtherMonths: true,
        onChangeMonthYear: function (year, month) {
            addCustomInformation(month, year);
        },
        onSelect: function (date, dp) {
            var month = date.getMonth() + 1;
            var year = date.getFullYear();
            addCustomInformation(month, year);
        },
    });
    function addCustomInformation(mth, year) {
        var eventDates = {};
        eventDates = getEventDate(mth, year);

        setTimeout(function () {
            $(".ui-datepicker-calendar td").filter(function () {
                var date = $(this).text();
                //var tahun = $(this).attr('data-year');

                if (year) {
                    var month = mth;
                    if (month == 0) {
                        month = 1;
                    }
                    if (date < 10) {
                        date = '0' + date;
                    }

                    var date_format = year + '/' + '0' + month + '/' + date;
                    var highlight = eventDates[date_format];
                    if (highlight) {
                        //price.find('a').attr('data-custom', 200);
                        //price.append('<a class="view-price btn btn-yellow sm-btn xs-txt os-bold">LIHAT</a>');

                        $('.ui-datepicker .ui-datepicker-title').addClass('col-xs-4');
                        $(this).find("a").attr('data-custom', highlight);
                    }
                }

                return date;
            });
        }, 0);
    }

    function getEventDate(mth, year) {
        var eventDates = {};
        //if (year % 4 == 0) {
            if (mth == 2) {
                for (var d = 1; d <= 29; d++) {
                    var x = year.toString() + '/' + ("0" + (mth)).slice(-2).toString() + '/' + ("0" + d.toString()).slice(-2);
                    eventDates[x] = $scope.listCheapestPrice[d-1];
                }
            }else if(mth == 1 || mth ==3 || mth ==5 || mth ==7 || mth ==8 || mth == 10 || mth == 12) {
                for (var d = 1; d <= 31; d++) {
                    var x = year.toString() + '/' + ("0" + (mth)).slice(-2).toString() + '/' + ("0" + d.toString()).slice(-2);
                    eventDates[x] = $scope.listCheapestPrice[d - 1];
                }
            }else if (mth == 4 || mth == 6 || mth == 9 || mth == 11){
                for (var d = 1; d <= 30; d++) {
                    var x = year.toString() + '/' + ("0" + (mth)).slice(-2).toString() + '/' + ("0" + d.toString()).slice(-2);
                    eventDates[x] = $scope.listCheapestPrice[d - 1];
                }
            }
        
        //if (mth == 1) {
        //    eventDates['2017/02/19'] = "Rp. 120.000";
        //    eventDates['2017/02/20'] = "Rp. 90.000";
        //}
        //else {
        //    eventDates['2017/01/29'] = "Rp. 70.000";
        //    eventDates['2017/01/30'] = "Rp. 60.000";
        //    eventDates['2017/01/31'] = "Rp. 70.000";
        //}


        return eventDates
    }
}]);

//********************
// hotel form search function
jQuery(document).ready(function ($) {
    //Show hotel
    $('.form-hotel-location').click(function (evt) {
        evt.stopPropagation();
        $('.search-hotel').show();
        $('.search-calendar-hotel, .select-age .option, .form-hotel-night .option, .form-hotel-room .option, .form-child-age').hide();
    });

    //hideHotel hotel
    function hideHotel() {
        $('.search-hotel').hide();
    }

    //close hotel
    $('.close-hotel').click(function () { hideHotel(); });

    $('.search-hotel .location-recommend nav ul li ').click(function () {
        var showClass = $(this).attr('data-show');
        $(this).addClass('active');
        $(this).siblings().removeClass('active');
        $('.search-hotel .location-recommend .tab-content>div').removeClass('active');
        $('.search-hotel .location-recommend .tab-content>div.' + showClass).addClass('active');
    });

    //*****
    // show and hide search calendar
    function showCalendar() {
        $('.search-calendar-hotel').show();
    }

    function hideCalendar() {
        $('.search-calendar-hotel').hide();
    }
    $('.close-calendar-hotel').click(function () { hideCalendar(); });

    //*****
    // date selector
    $('.form-hotel-checkin, .form-hotel-checkout').click(function (evt) {
        $('.search-calendar-hotel').show();
        showCalendar();
        $('.hotel-date-picker').datepicker('option', 'minDate', new Date());
        evt.stopPropagation();
        $('.search-hotel').show();
        $('.search-hotel, .select-age .option, .form-hotel-night .option, .form-hotel-room .option, .form-child-age').hide();
    });

    // Select Age Children
    $('body .select-age').on('click', function (evt) {
        evt.stopPropagation();
        $(this).parent().siblings().children('div').children('.option').hide();
        $(this).children('.option').toggle();
        $('.search-calendar-hotel, .search-hotel, .form-hotel-visitor.child .option, .form-hotel-visitor.adult .option').hide();
    });

    $('body .form-hotel-night').on('click', function () {
        $(this).children('.option').toggle();
        $('.search-calendar-hotel, .search-hotel, .form-hotel-room .option, .form-child-age').hide();
    });

    $('body .form-hotel-room').on('click', function () {
        $(this).children('.option').toggle();
        $('.search-calendar-hotel, .search-hotel, .form-hotel-night .option').hide();
    });

    $('body .form-hotel-visitor.adult').on('click', function () {
        $(this).children('.option').toggle();
        $('.form-hotel-visitor.child .option, .select-age .option').hide();
    });

    $('body .form-hotel-visitor.child').on('click', function () {
        $(this).children('.option').toggle();
        $('.form-hotel-visitor.adult .option, .select-age .option').hide();
    });

    $('body .form-child-age').hide();
    $('body .form-hotel-room span').on('click', function () {
        $('body .form-child-age').show();
    });

    $('body input[name="FormAgeSubmit"]').on('click', function () {
        $('body .form-child-age').hide();
    });

    //Home Page Search Form Hotel
    $('body .menu-main li').click(function () {
        if ($(this).is('#header-flight')) {
            var itemF = $(this).closest('.site-header').parent();

            itemF.parent().find('.tab-header').find('.flight').addClass('active');
            itemF.parent().find('.tab-header').find('.flight').siblings().removeClass('active');
            itemF.parent().find('#plane').addClass('active');
            itemF.parent().find('#plane').siblings().removeClass('active');

            var linkF = $(this).find('a').attr('id', '#plane');

            linkF.parent().addClass('active');
            linkF.parent().siblings().removeClass('active');

        } else if ($(this).is('#header-hotel')) {
            var item = $(this).closest('.site-header').parent();

            item.parent().find('.tab-header').find('.hotel').addClass('active');
            item.parent().find('.tab-header').find('.hotel').siblings().removeClass('active');
            item.parent().find('#hotel').addClass('active');
            item.parent().find('#hotel').siblings().removeClass('active');

            var link = $(this).find('a').attr('id', '#hotel');

            link.parent().addClass('active');
            link.parent().siblings().removeClass('active');

        }
    });

    $('body .tab-header li').click(function () {
        if ($(this).hasClass('flight')) {
            $(this).closest('.site-content').parent().find('.menu-main').find('#header-flight').addClass('active');
            $(this).closest('.site-content').parent().find('.menu-main').find('#header-flight').siblings().removeClass('active');
        } else if ($(this).hasClass('hotel')) {
            $(this).closest('.site-content').parent().find('.menu-main').find('#header-hotel').addClass('active');
            $(this).closest('.site-content').parent().find('.menu-main').find('#header-hotel').siblings().removeClass('active');
        }
    });

    // Slider Home Page Desktop
    $('.page-slider .slider a').each(function () {
        var bgImage = $(this).children('img').attr('src');
        $(this).css({
            'background-image': 'url(' + bgImage + ')'
        });
        $(this).children('img').remove();
    });
    $('.slider-wrapper').slick({
        autoplay: true,
        autoplaySpeed: 4000,
        dots: true,
        prevArrow: '<button type="button" class="slick-prev hidden">Back</button>',
        nextArrow: '<button type="button" class="slick-next hidden">Next</button>'
    });

    // Slider Home Page Mobile
    $('.carousel-inner').slick({
        autoplay: true,
        autoplaySpeed: 2800,
        dots: true,
        prevArrow: '<button type="button" class="slick-prev hidden">Back</button>',
        nextArrow: '<button type="button" class="slick-next hidden">Next</button>'
    });

    // Price Calendar
    

    $('.pop-hotel').hover(function () {
        $(this).find('.view-hotel').slideToggle('fast');
    });

   
    function setPrice(date) {
        var price = $(".ui-datepicker-calendar td").filter(function () {
            var datex = $(this).text();
            return /\d/.test(datex);
        });

        var x = new Date(date);
        var listCheapestPrice = [30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30];
        var cheapestprice = listCheapestPrice[x.getDate() - 1];
        price.find('a').attr('data-custom', 200);
        price.append('<a class="view-price btn btn-yellow sm-btn xs-txt os-bold">LIHAT</a>');

        $('.ui-datepicker .ui-datepicker-title').addClass('col-xs-4');

        return [true, date.getDay() === 5 || date.getDay() === 6 ? "weekend" : "weekday"];
    }


});