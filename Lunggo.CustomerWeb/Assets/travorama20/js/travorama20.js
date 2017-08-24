//// currently unsuppported feature from javascript ES6
// import {translateMonth} from 'services/dateTimeService';

$(function () {
    $('[data-toggle="tooltip"]').tooltip();
});

if (typeof (angular) == 'object') {
    var app = angular.module('travorama', ['ngResource']);

    app.service('flightParam', function () {
        var param = {
            oriCity: '',
            oriArpt: '',
            destCity: '',
            destArpt: ''
        };
        return {
            setPropertyOri: function (oriCity, oriArpt) {
                param.oriCity = oriCity;
                
            }
        };
    });

    app.factory('DataSource', ['$http', function ($http) {
        return {
            get: function (file, callback, transform) {
                $http.get(
                    file,
                    { transformResponse: transform }
                ).
                success(function (data, status) {
                    // console.log("Request succeeded");
                    callback(data);
                }).
                error(function (data, status) {
                    // console.log("Request failed " + status);
                });
            }
        };
    }]);

    var SOURCE_FILE = "/Config/application.properties";

    xmlTransform = function (data) {
        // console.log("transform data");a
        var x2js = new X2JS();
        var json = x2js.xml_str2json(data);
        return json.guitars.guitar;
    };

    setData = function (data) {
        $scope.dataSet = data;
    };
};

// variables
var currentDate = new Date();
var trial = 0;

// site header function
$('html').click(function () {
    $('.dropdown-content').hide();
    $('.form-flight-class').siblings('.option').hide();
});
$('[data-trigger="dropdown"]').click(function (evt) {
    evt.stopPropagation();
    evt.preventDefault();
    $(this).siblings('.dropdown-content').toggle();
});

// general functions

// get parameter
function getParam(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

function getAnonymousFirstAccess() {
    var status = 0;
    $.ajax({
        url: LoginConfig.Url,
        method: 'POST',
        async: false,
        data: JSON.stringify({
            "clientId": "V2toa2VrOXFSWFZOUXpSM1QycEZlRTlIVlhwYWFrVjVUVVJrYlZsVVp6Vk5WRlp0VGtSR2FrOUhSWGhhYWsweFRucGpNRTE2U1RCT2VtTjNXbTFKZDFwcVFUMD0=",
            "clientSecret": "V2tkS2FFOUVhek5QUjFsNFRucFpNVmt5UlRST2JVWnNXVmRKTTA1dFVtaFBSMDVyV1dwQk5WcEhTWGxPZWtwcVRVUkpNVTFCUFQwPQ=="
        }),
        contentType: 'application/json',
    }).done(function (returnData) {
        if (returnData.status == '200') {
            setCookie("accesstoken", returnData.accessToken, returnData.expTime);
            setCookie("refreshtoken", returnData.refreshToken, returnData.expTime);
            if (getCookie('accesstoken')) status = 1;
            else status = 0;
            }
        else status = 0;
    });
    return status;
}

function getAnonymousAccessByRefreshToken(refreshToken) {
    var status = 0;
    $.ajax({
        url: LoginConfig.Url,
        method: 'POST',
        async: false,
        data: JSON.stringify({ "refreshtoken": refreshToken, "clientId": "Jajal", "clientSecret": "Standar" }),
        contentType: 'application/json',
    }).done(function (returnData) {
        if (returnData.status == '200') {
            setCookie("accesstoken", returnData.accessToken, returnData.expTime);
            setCookie("refreshtoken", returnData.refreshToken, returnData.expTime);
            if (getCookie('accesstoken')) status = 1;
            else status = 0;
            }
        else status = 0;
    });
    return status;
}

function getLoginAccessByRefreshToken(refreshToken) {
    var status = 0;
    $.ajax({
        url: LoginConfig.Url,
        method: 'POST',
        async: false,
        data: JSON.stringify({ "refreshtoken": refreshToken, "clientId": "Jajal", "clientSecret": "Standar" }),
        contentType: 'application/json',
    }).done(function (returnData) {
        if (returnData.status == '200') {
            setCookie("accesstoken", returnData.accessToken, returnData.expTime);
            setCookie("refreshtoken", returnData.refreshToken, returnData.expTime);
            setCookie("authkey", returnData.accessToken, returnData.expTime);
            if (getCookie('accesstoken')) status = 2;
            else status = 0;
            }
        else status = 0;
    });
    return status;
}

function getAuthAccess() {
    var token = getCookie('accesstoken');
    var refreshToken = getCookie('refreshtoken');
    var authKey = getCookie('authkey');
    var status = 0;

    if (authKey) {
        if (token) return 2;
        else {
            if (refreshToken) {
                status = getLoginAccessByRefreshToken(refreshToken);
                if (status == 0) {
                    status = getAnonymousFirstAccess();
                }
            } else return 0;
            }
            }
    else {
        if (token) return 1;
        else {
            //Get Anonymous Token By Refresh Token
            if (refreshToken) {
                status = getAnonymousAccessByRefreshToken(refreshToken);
                if (status == 0) {
                    status = getAnonymousFirstAccess();
                }
            } else {
                //For Anynomius at first
                status = getAnonymousFirstAccess();
            }
        }
    }
    return status;
}

function refreshAuthAccess() {
    /* If failed to get Authorization, but accesstoken is still exist     */
    var token = getCookie('accesstoken');
    var refreshToken = getCookie('refreshtoken');
    var authKey = getCookie('authkey');
    var status = 0;
    if (refreshToken) {
        if (authKey) {
            status = getLoginAccessByRefreshToken(refreshToken);
            if (status == 0) {
                status = getAnonymousFirstAccess();
                eraseCookie('authkey');
            }
            if (status == 2) return true;
            else return false;
            }
            else {
            status = getAnonymousAccessByRefreshToken(refreshToken);
            if (status == 0) status = getAnonymousFirstAccess();

            if (status == 1 || status == 2) return true;
            else return false;
            }
            }
    else {
        getAnonymousFirstAccess();
        return true;
    }
}


// flight page functions
// Used in Search Result Flight Page
function flightPageFunctions() {

    //***Select one of this***
    flightFormSearchFunctions(); 

    // toggle search form
    $('.search-result-form-trigger').click(function () {
        $('.change-flight').stop().slideToggle();
        $('.change-flight').toggleClass('active');
        if (!$('.change-flight').hasClass('active')) {
            $('.search-location, .search-calendar').hide();
        }
    });
    // toggle filter
    $('.search-result-filter .filter-trigger span').click(function () {
        $(this).parent().siblings().removeClass('active');
        $(this).parent().addClass('active');
        var targetFilter = $(this).attr('data-target');
        if ($('.search-result-filter .filter-content>div#' + targetFilter).hasClass("active")) {
            $('.search-result-filter .close-filter').click();
            ct += 1;
        } else {
            $('.search-result-filter .filter-content>div').removeClass('active');
            $('.search-result-filter .filter-content>div#' + targetFilter).addClass('active');
            ct += 1;
        }
    });
    // close filter
    $('.search-result-filter .close-filter').click(function () {
        $(this).parent().removeClass('active');
        $(this).parent().parent().siblings('.filter-trigger').children().removeClass('active');
        ct += 1;
    });
}

var ct = 0;
var flightPageSearchFormParam = {
    type: 'return',
    origin: '',
    originCity: '',
    destination: '',
    destinationCity: '',
    cabin: 'y',
    departureDate: '',
    returnDate: '',
    maxPassenger: 9,
    passenger: {
        adult: 1,
        child: 0,
        infant: 0
    }
};

//********************
// static page functions
function staticPageFunctions() {
    // *****
    // toggle FAQ question
    $('.toggle-all').on('click', function () {
        if ($(this).hasClass('active')) {
            $(this).closest('li').children('ol').children('li').removeClass('active');
        } else {
            $(this).closest('li').children('ol').children('li').addClass('active');
        }
        checkQuestion();
    });
    $('.accordion-wrapper ol li ol li header').on('click', function () {
        $(this).closest('li').toggleClass('active');
        checkQuestion();
    });
    // check faqs
    var checkQuestion = function () {
        $('.accordion-wrapper>ol>li').each(function () {
            if ($(this).children('ol').find('.active').length > 0) {
                $(this).find('.toggle-all').addClass('active').text('Hide All');
            } else {
                $(this).find('.toggle-all').removeClass('active').text('Show All');
            }
        });
    }

    $('.toggle-all').on('click', function () {
        if ($(this).hasClass('active')) {
            $(this).closest('li').children('ol').children('li').removeClass('active');
        } else {
            $(this).closest('li').children('ol').children('li').addClass('active');
        }
        checkQuestion();
    });
}

// flight form search function
// used in Index Page 
function flightFormSearchFunctions() {

    $(window).bind("pageshchange-flight-classow", function (event) {
        if (event.originalEvent.persisted) {
            $('.flight-submit-button').removeProp('disabled');
            window.location.reload();
        }
    });

    // index page config
    FlightSearchConfig.autocomplete = {
        loading: false,
        keyword: {},
        result: {},
        cache: {}
    };
    FlightSearchConfig.flightForm = {
        maxPassenger: 9,
        currentSelection: '',
        origin: '',
        originCity: '',
        destination: '',
        destinationCity: '',
        type: 'oneway',
        departureDate: '',
        returnDate: '',
        cabin: 'y',
        passenger: {
            adult: 1,
            child: 0,
            infant: 0
        }
    };

    // select flight type
    $('.form-flight-type').click(function () {
        FlightSearchConfig.flightForm.type = $('input[name="flightType"]:checked').val();
        $('.change-flight-class .option, .form-flight-passenger .option , .search-location, .search-calendar').hide();
        if (FlightSearchConfig.flightForm.type == 'return') $('.form-flight-return').removeClass('disabled');
        else $('.form-flight-return').addClass('disabled');
    });

    // on flight origin focus
    $('.form-flight-origin').click(function () {
        $(this).select();
        showLocation('origin');
    });
    $('.form-flight-destination').click(function () {
        $(this).select();
        showLocation('destination');
    });

    // flight recommendation
    $('.section-search .search-location .location-recommend .nav-click.prev').click(function (evt) {
        evt.preventDefault();
        if (parseInt($('.section-search .search-location .location-recommend .tab-header nav ul').css('margin-left')) < 0) {
            $('.section-search .search-location .location-recommend .tab-header nav ul').css('margin-left', '+=135px');
        }
    });
    $('.section-search .search-location .location-recommend .nav-click.next').click(function (evt) {
        evt.preventDefault();
        if (parseInt($('.section-search .search-location .location-recommend .tab-header nav ul').css('margin-left')) > -(135 * ($('.search-location .location-recommend .tab-header nav ul li').length - 4))) {
            $('.section-search .search-location .location-recommend .tab-header nav ul').css('margin-left', '-=135px');
        }
    });

    // The user clicked the category-tab ("Destinasi Populer" || "Indonesia" || etc.)
    $('.section-search .search-location .location-recommend nav ul li ').click(function () {
        var showClass = $(this).attr('data-show');
        $(this).addClass('active');
        $(this).siblings().removeClass('active');
        $('.section-search .search-location .location-recommend .tab-content>div').removeClass('active');
        $('.section-search .search-location .location-recommend .tab-content>div.' + showClass).addClass('active');
    });

    // The user choose one of recommendation in location lists
    $('.section-search .search-location .location-recommend .tab-content a').click(function (evt, sharedProperties) {
        evt.preventDefault();
        var dataPlace = $('.section-search .search-location').attr('data-place')
        var locationCode = $(this).attr('data-code');
        var locationCity = $(this).text();
        updateLocation( dataPlace, locationCode, locationCity);
    });

    // on swap target
    $('.switch-destination').click(function () {
        var prevOrigin = $('.form-flight-origin').val();
        var prevOriginCode = FlightSearchConfig.flightForm.origin;
        var prevOriginCity = FlightSearchConfig.flightForm.originCity;
        var prevDestination = $('.form-flight-destination').val();
        var prevDestinationCode = FlightSearchConfig.flightForm.destination;
        var prevDestinationCity = FlightSearchConfig.flightForm.destinationCity;

        $('.form-flight-origin').val(prevDestination);
        $('.form-flight-destination').val(prevOrigin);
        FlightSearchConfig.flightForm.origin = prevDestinationCode;
        FlightSearchConfig.flightForm.destination = prevOriginCode;
        FlightSearchConfig.flightForm.originCity = prevDestinationCity;
        FlightSearchConfig.flightForm.destinationCity = prevOriginCity;
    });

    $('.form-flight').on('keyup keypress', function (e) {
        var code = e.keyCode || e.which;
        if (code == 13) {
            e.preventDefault();
            return false;
        }
    });

    // autocomplete function
    function getLocation(keyword) {
        if (trial > 3) {
            trial = 0;
        }
        FlightSearchConfig.autocomplete.loading = true;
        $('.section-search autocomplete-pre .text-pre').hide();
        $('.section-search autocomplete-pre .text-loading').show();
        if (typeof (FlightSearchConfig.autocomplete.cache[keyword]) != "undefined") {
            FlightSearchConfig.autocomplete.result = FlightSearchConfig.autocomplete.cache[keyword];
            generateSearchResult(FlightSearchConfig.autocomplete.result);
            if (FlightSearchConfig.autocomplete.result.length > 0) {
                $('.section-search .autocomplete-no-result').hide();
                $('.section-search .autocomplete-pre .text-loading').hide();
                $('.section-search .autocomplete-result').show();
            } else {
                $('.section-search .autocomplete-pre .text-loading').hide();
                $('.section-search .autocomplete-result').hide();
                $('.section-search .autocomplete-no-result').show();
            }
        } else {
            $.ajax({
                url: FlightAutocompleteConfig.Url + keyword,
                headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
            }).done(function (returnData) {
                $('.section-search .autocomplete-pre .text-pre').hide();
                $('.section-search .autocomplete-pre .text-loading').hide();
                FlightSearchConfig.autocomplete.loading = false;
                FlightSearchConfig.autocomplete.result = returnData.airports;
                FlightSearchConfig.autocomplete.cache[keyword] = returnData.airports;
                generateSearchResult(FlightSearchConfig.autocomplete.result);
                if (returnData.airports.length > 0) {
                    $('.section-search .autocomplete-no-result').hide();
                    $('.section-search .autocomplete-pre .text-loading').hide();
                    $('.section-search .autocomplete-result').show();
                } else {
                    $('.section-search .autocomplete-pre .text-loading').hide();
                    $('.section-search .autocomplete-result').hide();
                    $('.section-search .autocomplete-no-result').show();
                }
            }).fail(function (returnData) {
                trial++;
                if (refreshAuthAccess() && trial < 4) //refresh cookie
                {
                    getLocation(keyword);
                }
            });
        }
    }
    function generateSearchResult(list) {
        $('.section-search .autocomplete-result ul').empty();
        for (var i = 0 ; i < list.length; i++) {
            $('.section-search .autocomplete-result ul').append(
                '<li data-code="' + list[i].code + '" data-city="' + list[i].city + '">'
                + list[i].city + ' (' + list[i].code + '), ' + list[i].name + ', ' + list[i].country
                + '</li>'
            );
        }
    }

    //// REDUNDANT#1 same function used in homeController. consider to move this function
    //// to be accessed globally accross Controllers / js.
    /* Update location text in form textbox and attributes in FlightSearchConfig.flightForm
     * dataPlace: "origin" || "";   validates the location type: origin or destination
     */
    function updateLocation( dataPlace, locationCode, locationCity){
        if (dataPlace == 'origin') {
            FlightSearchConfig.flightForm.origin = locationCode;
            FlightSearchConfig.flightForm.originCity = locationCity;
            $('.form-flight-origin').val(locationCity + ' (' + locationCode + ')');
        } else {
            FlightSearchConfig.flightForm.destination = locationCode;
            FlightSearchConfig.flightForm.destinationCity = locationCity;
            $('.form-flight-destination').val(locationCity + ' (' + locationCode + ')');
        }
        $('.flight-submit-button').removeClass('disabled');
        hideLocation();
    }
    
    // The user select one location from search result
    $('.section-search .autocomplete-result ul').on('click', 'li', function () {
        var dataPlace = $('.section-search .search-location').attr('data-place');
        var locationCode = $(this).attr('data-code');
        var locationCity = $(this).attr('data-city');
        updateLocation( dataPlace, locationCode, locationCity);
    });

    // on keypress on form flight search
    $('.form-flight-location').keyup(function (evt) {
        if (evt.keyCode == 27) {
            hideLocation();
        } else {
            if ($(this).val().length >= 3) {
                $('.section-search .search-location .location-recommend').hide();
                $('.section-search .search-location .location-search').show();
                FlightSearchConfig.autocomplete.keyword = $(this).val();
                getLocation(FlightSearchConfig.autocomplete.keyword);
            } else {
                $('.section-search .search-location .location-recommend').hide();
                $('.section-search .search-location .location-search').show();
                $('.section-search .search-location .location-search .autocomplete-pre .text-pre').show();
                $('.section-search .search-location .location-search .autocomplete-result').hide();
                $('.section-search .search-location .location-search .autocomplete-no-result').hide();
            }
        }
    });
    $('.form-flight-location').keydown(function (evt) {
        if (evt.keyCode == 9 || evt.which == 9) {
            evt.preventDefault();
        }
    });
    $('.form-flight-location').focusout(function (evt) {
        if ($(this).hasClass('form-flight-origin')) {
            $(this).val((FlightSearchConfig.flightForm.originCity + " (" + FlightSearchConfig.flightForm.origin + ")"));
        } else {
            $(this).val((FlightSearchConfig.flightForm.destinationCity + " (" + FlightSearchConfig.flightForm.destination + ")"));
        }
    });

    // date selector
    $('.form-flight-departure').click(function () {
        showCalendar('departure');
        $('.date-picker').datepicker('option', 'minDate', new Date());
    });
    $('.form-flight-return').click(function () {
        if (!$(this).hasClass('disabled')) {
            if (FlightSearchConfig.flightForm.departureDate) {
                $('.date-picker').datepicker('option', 'minDate', new Date(FlightSearchConfig.flightForm.departureDate));
            } else {
                $('.date-picker').datepicker('option', 'minDate', new Date());
            }
            showCalendar('return');
        }
    });
    var holidays = [];
    var holidayNames = [];

    var authAccess = getAuthAccess();
    if (authAccess == 1 || authAccess == 2) {
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
                        holidays.push(holidayDate);
                        holidayNames.push(holidayName);
                    }
                }
            }
        })
        // .fail(function (returnData) {});
    }

    function highlightDays(date) {
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
    // embed date picker into page
    $('.date-picker').datepicker({
        numberOfMonths: 2,
        onSelect: function (data) {
            data = data.substring(3, 5) + "/" + data.substring(0, 2) + "/" + data.substring(6, 10);
            var target;
            var chosenDate = new Date(data);
            if ($('.search-calendar').attr('data-date') == 'departure') {
                FlightSearchConfig.flightForm.departureDate = new Date(data);
                if (FlightSearchConfig.flightForm.departureDate > FlightSearchConfig.flightForm.returnDate) {
                    FlightSearchConfig.flightForm.returnDate = new Date(data);
                    $('.form-flight-return .date').html(('0' + chosenDate.getDate()).slice(-2));
                    $('.form-flight-return .month').html(translateMonth(chosenDate.getMonth()));
                    $('.form-flight-return .year').html(chosenDate.getFullYear());
                    $('.search-calendar').hide();
                }
                target = '.form-flight-departure';
            } else {
                FlightSearchConfig.flightForm.returnDate = new Date(data);
                target = '.form-flight-return';
            }
            $(target + ' .date').html(('0' + chosenDate.getDate()).slice(-2));
            $(target + ' .month').html(translateMonth(chosenDate.getMonth()));
            $(target + ' .year').html(chosenDate.getFullYear());
            $('.search-calendar').hide();
        },
        beforeShowDay: highlightDays
    });

    // set current date as default
    $(document).ready(function () {
        // set default date for departure flight
        var departureDate = '';
        if (Cookies.get('departure')) {
            var cookieDateDeparture = new Date(Cookies.get('departure'));
            var nowDate = new Date();
            if (cookieDateDeparture > nowDate) {
                departureDate = new Date(Cookies.get('departure'));
            } else {
                departureDate = new Date();
                departureDate = departureDate.setDate(departureDate.getDate() + 1);
                departureDate = new Date(departureDate);
            }
        } else {
            departureDate = new Date();
            departureDate = departureDate.setDate(departureDate.getDate() + 1);
            departureDate = new Date(departureDate);
        }
        $('.form-flight-departure .date').html(('0' + departureDate.getDate()).slice(-2));
        $('.form-flight-departure .month').html(translateMonth(departureDate.getMonth()));
        $('.form-flight-departure .year').html(departureDate.getFullYear());
        FlightSearchConfig.flightForm.departureDate = departureDate;
        // set default date for return flight
        var returnDate = '';
        if (Cookies.get('return')) {
            var cookieDateReturn = new Date(Cookies.get('return'));
            var tomorrowDate = new Date();
            tomorrowDate.setDate(tomorrowDate.getDate() + 1);
            if (cookieDateReturn > nowDate) {
                returnDate = new Date(Cookies.get('return'));
            } else {
                returnDate = new Date();
                returnDate = returnDate.setDate(returnDate.getDate() + 2);
                returnDate = new Date(returnDate);
            }
        } else {
            returnDate = new Date();
            returnDate = returnDate.setDate(returnDate.getDate() + 2);
            returnDate = new Date(returnDate);
        }
        $('.form-flight-return .date').html(('0' + (returnDate.getDate())).slice(-2));
        $('.form-flight-return .month').html(translateMonth(returnDate.getMonth()));
        $('.form-flight-return .year').html(returnDate.getFullYear());
        FlightSearchConfig.flightForm.returnDate = returnDate;

        // set default flight and return flight
        if (Cookies.get('origin')) {
            $('.form-flight-origin').val(Cookies.get('originCity') + ' (' + Cookies.get('origin') + ')');
            FlightSearchConfig.flightForm.origin = Cookies.get('origin');
            FlightSearchConfig.flightForm.originCity = Cookies.get('originCity');
        } else {
            $('.form-flight-origin').val('Jakarta (CGK)');
            FlightSearchConfig.flightForm.origin = 'CGK';
            FlightSearchConfig.flightForm.originCity = 'Jakarta';
        }
        if (Cookies.get('destination')) {
            $('.form-flight-destination').val(Cookies.get('destinationCity') + ' (' + Cookies.get('destination') + ')');
            FlightSearchConfig.flightForm.destination = Cookies.get('destination');
            FlightSearchConfig.flightForm.destinationCity = Cookies.get('destinationCity');
        } else {
            $('.form-flight-destination').val('Denpasar (DPS)');
            FlightSearchConfig.flightForm.destination = 'DPS';
            FlightSearchConfig.flightForm.destinationCity = 'Denpasar';
        }

        // flight passenger
        if (Cookies.get('adult')) {
            $('.passenger-input.adult').text(Cookies.get('adult'));
            FlightSearchConfig.flightForm.adult = Cookies.get('adult');
            FlightSearchConfig.flightForm.passenger.adult = parseInt(Cookies.get('adult'));
        } else {
            $('.passenger-input.adult').text('1');
        }
        if (Cookies.get('child')) {
            $('.passenger-input.child').text(Cookies.get('child'));
            FlightSearchConfig.flightForm.child = Cookies.get('child');
            FlightSearchConfig.flightForm.passenger.child = parseInt(Cookies.get('child'));
        } else {
            $('.passenger-input.child').text('0');
        }
        if (Cookies.get('infant')) {
            $('.passenger-input.infant').text(Cookies.get('infant'));
            FlightSearchConfig.flightForm.infant = Cookies.get('infant');
            FlightSearchConfig.flightForm.passenger.infant = parseInt(Cookies.get('infant'));
        } else {
            $('.passenger-input.infant').text('0');
        }

        // flight cabin
        if (Cookies.get('cabin')) {
            switch (Cookies.get('cabin')) {
                case 'y':
                    FlightSearchConfig.flightForm.cabin = 'y';
                    $('.form-flight-class>span').text($('.form-flight-class .option .economy').text());
                    break;
                case 'c':
                    FlightSearchConfig.flightForm.cabin = 'c';
                    $('.form-flight-class>span').text($('.form-flight-class .option .business').text());
                    break;
                case 'f':
                    FlightSearchConfig.flightForm.cabin = 'f';
                    $('.form-flight-class>span').text($('.form-flight-class .option .first').text());
                    break;
            }
        }

        // navigation
        $('.home-nav .nav-prev').click(function () {
            $('.ui-datepicker-prev').click();
        });
        $('.home-nav .nav-next').click(function () {
            $('.ui-datepicker-next').click();
        });

        // flight type
        if (Cookies.get('type')) {
            var x = Cookies.get('type').toLowerCase();
            if (Cookies.get('type').toLowerCase() == 'return') {
                $('.form-flight-type[data-value="return"]').click();
            } else {
                $('.form-flight-type[data-value="oneway"]').click();
            }
        }

    });

    $(window).on('load', function () {
        // flight type
        if (Cookies.get('type')) {
            if (Cookies.get('type').toLowerCase() == 'return') {
                $('.form-flight-type[data-value="return"]').click();
            } else {
                $('.form-flight-type[data-value="oneway"]').click();
            }
        }
    });

    // select cabin
    $('.form-flight-class').click(function (evt) {
        evt.stopPropagation();
        $(this).find('.option').toggle();
        $('.form-flight-passenger .option, .search-calendar, .search-location').hide();
    });
    $('.form-flight-class .option span').click(function () {
        $('.form-flight-class>span').html($(this).html());
        FlightSearchConfig.flightForm.cabin = $(this).attr('data-value');
        $('.form-group.flight-class').click();
    });

    // show and hide search location
    function showLocation(place) {
        place = place || $('.search-location').attr('data-place');
        $('.section-search .search-location .location-recommend').show();
        $('.section-search .search-location .location-search').hide();
        if (place == 'origin') {
            $('.section-search .search-location .location-header .origin').removeClass('hidden');
            $('.section-search .search-location .location-header .destination').addClass('hidden');
        } else {
            $('.section-search .search-location .location-header .origin').addClass('hidden');
            $('.section-search .search-location .location-header .destination').removeClass('hidden');
        }
        $('.section-search .search-location').attr('data-place', place);
        $('.section-search .search-location').attr('id', place);
        $('.section-search .search-location').show();
        $('.search-calendar').hide();
    }

    function hideLocation() {
        $('.section-search .search-location').hide();
    }
    
    $('.section-search .close-location').click( function () {
        hideLocation();
    });

    // show and hide search calendar
    function showCalendar(target) {
        target = target || $('.search-calendar').attr('data-date');
        $('.search-calendar').attr('id', target);
        if (target == 'departure') {
            $('.search-calendar .calendar-header .departure').removeClass('hidden');
            $('.search-calendar .calendar-header .return').addClass('hidden');
        } else {
            $('.search-calendar .calendar-header .departure').addClass('hidden');
            $('.search-calendar .calendar-header .return').removeClass('hidden');
        }
        $('.search-calendar').attr('data-date', target);
        $('.search-calendar').show();
        hideLocation();
    }
    
    $('.close-calendar').click(function () {
        $('.search-calendar').hide();
    });

    // validate passenger
    $('.form-flight-passenger').click(function (evt) {
        evt.stopPropagation();
        $('.change-flight-class .option').hide();
        $(this).parent().siblings().children('div').children('.option').hide();
        $(this).children('.option').toggle();
        $('.search-location, .search-calendar').hide();
    });
    $('.form-flight-passenger .option span').click(function (evt) {
        var alertText = {
            over: "Jumlah total penumpang tidak boleh lebih dari 9 orang",
            infant: "Jumlah penumpang bayi tidak boleh lebih dari penumpang dewasa"
        };
        evt.preventDefault();
        evt.stopPropagation();
        var parentClass = $(this).closest('.form-flight-passenger');
        var optionValue = parseInt($(this).text());
        if (parentClass.hasClass('adult')) {
            if (optionValue > (FlightSearchConfig.flightForm.maxPassenger - (FlightSearchConfig.flightForm.passenger.child + FlightSearchConfig.flightForm.passenger.infant))) {
                alert(alertText.over);
                optionValue = (FlightSearchConfig.flightForm.maxPassenger - (FlightSearchConfig.flightForm.passenger.child + FlightSearchConfig.flightForm.passenger.infant));
            }
            if (FlightSearchConfig.flightForm.passenger.infant > optionValue) {
                alert(alertText.infant);
                FlightSearchConfig.flightForm.passenger.infant = optionValue;
                $('.passenger-input.infant').text(optionValue);
            }
            FlightSearchConfig.flightForm.passenger.adult = optionValue;
            $('.passenger-input.adult').text(optionValue);
        } else if (parentClass.hasClass('child')) {
            if (optionValue > (FlightSearchConfig.flightForm.maxPassenger - (FlightSearchConfig.flightForm.passenger.adult + FlightSearchConfig.flightForm.passenger.infant))) {
                alert(alertText.over);
                optionValue = (FlightSearchConfig.flightForm.maxPassenger - (FlightSearchConfig.flightForm.passenger.adult + FlightSearchConfig.flightForm.passenger.infant));
            }
            FlightSearchConfig.flightForm.passenger.child = optionValue;
            $('.passenger-input.child').text(optionValue);
        } else if (parentClass.hasClass('infant')) {
            if (optionValue > FlightSearchConfig.flightForm.passenger.adult) {
                alert(alertText.infant);
                optionValue = FlightSearchConfig.flightForm.passenger.adult;
            } else {
                if (optionValue > (FlightSearchConfig.flightForm.maxPassenger - (FlightSearchConfig.flightForm.passenger.child + FlightSearchConfig.flightForm.passenger.adult))) {
                    alert(alertText.over);
                    optionValue = (FlightSearchConfig.flightForm.maxPassenger - (FlightSearchConfig.flightForm.passenger.child + FlightSearchConfig.flightForm.passenger.adult));
                }
            }
            FlightSearchConfig.flightForm.passenger.infant = optionValue;
            $('.passenger-input.infant').text(optionValue);
        }
        $(this).parent().hide();
    });

    $('.passenger-input').keyup(function () {
        if ($(this).hasClass('adult')) {
            if ($(this).val() < 1) {
                $(this).val(1);
            } else {
                if ($(this).val() > (FlightSearchConfig.flightForm.maxPassenger - (parseInt($('.passenger-input.child').val()) + parseInt($('.passenger-input.infant').val())))) {
                    $(this).val((FlightSearchConfig.flightForm.maxPassenger - (parseInt($('.passenger-input.child').val()) + parseInt($('.passenger-input.infant').val()))));
                }
            }
            if ($('.passenger-input.infant').val() > $(this).val()) {
                $('.passenger-input.infant').val($(this).val());
            }
        }
    });

    // submit validation
    $('.flight-submit-button').on('click', function (evt) {
        evt.preventDefault();
        validateFlightForm( function(success, message){
            if (success){
                $(this).prop('disabled', true);
                setCookie();
                generateFlightSearchParam();
            } else {
                alert(message);
                // $('.flight-submit-button').removeProp('disabled');
            }
        });
    });

    // validate Flight Form and return the validation result to callback function
    function validateFlightForm ( callback ) {
        var flightForm = FlightSearchConfig.flightForm;
        if (flightForm.origin && flightForm.destination) {
            if (flightForm.origin == flightForm.destination) {
                return callback(false, "Kota Asal dan Tujuan Tidak Boleh Sama");
            }
            if (!flightForm.departureDate) {
                flightForm.departureDate = new Date();
            }
            if (!flightForm.returnDate) {
                flightForm.returnDate = new Date();
            }
            return callback(true,'');
        } else {
            if (!flightForm.origin) {
                return callback(false, "Silakan pilih bandara asal Anda");
            }
            if (!flightForm.destination) {
                return callback(false, "Silakan pilih bandara tujuan Anda");
            }
        }
    }

    // set cookie
    function setCookie() {
        Cookies.set('origin', FlightSearchConfig.flightForm.origin, { expires: 9999 });
        Cookies.set('originCity', FlightSearchConfig.flightForm.originCity, { expires: 9999 });
        Cookies.set('destination', FlightSearchConfig.flightForm.destination, { expires: 9999 });
        Cookies.set('destinationCity', FlightSearchConfig.flightForm.destinationCity, { expires: 9999 });
        Cookies.set('departure', FlightSearchConfig.flightForm.departureDate, { expires: 9999 });
        Cookies.set('return', FlightSearchConfig.flightForm.returnDate, { expires: 9999 });
        Cookies.set('type', FlightSearchConfig.flightForm.type, { expires: 9999 });
        Cookies.set('adult', FlightSearchConfig.flightForm.passenger.adult, { expires: 9999 });
        Cookies.set('child', FlightSearchConfig.flightForm.passenger.child, { expires: 9999 });
        Cookies.set('infant', FlightSearchConfig.flightForm.passenger.infant, { expires: 9999 });
        Cookies.set('cabin', FlightSearchConfig.flightForm.cabin, { expires: 9999 });
    }

    function generateFlightSearchParam() {
        var flightForm = FlightSearchConfig.flightForm;
        var departureDate = (('0' + flightForm.departureDate.getDate()).slice(-2) + ('0' + (flightForm.departureDate.getMonth() + 1)).slice(-2) + flightForm.departureDate.getFullYear().toString().substr(2, 2));
        var returnDate = (('0' + flightForm.returnDate.getDate()).slice(-2) + ('0' + (flightForm.returnDate.getMonth() + 1)).slice(-2) + flightForm.returnDate.getFullYear().toString().substr(2, 2));
        var departureParam = flightForm.origin + flightForm.destination + departureDate;
        var returnParam = flightForm.destination + flightForm.origin + returnDate;
        var passengerParam = flightForm.passenger.adult.toString() + flightForm.passenger.child.toString() + flightForm.passenger.infant.toString() + flightForm.cabin;
        var flightSearchParam;
        // generate flight search param
        if (flightForm.type == 'return') {
            flightSearchParam = departureParam + '~' + returnParam + '-' + passengerParam;
        } else {
            flightSearchParam = departureParam + '-' + passengerParam;
        }
        $('.form-flight input[name="info"]').val(flightSearchParam);
        FlightSearchConfig.flightForm.originCity = FlightSearchConfig.flightForm.originCity.replace(/\s+/g, '-');
        FlightSearchConfig.flightForm.originCity = FlightSearchConfig.flightForm.originCity.replace(/[^0-9a-zA-Z-]/gi, '');
        FlightSearchConfig.flightForm.destinationCity = FlightSearchConfig.flightForm.destinationCity.replace(/\s+/g, '-');
        FlightSearchConfig.flightForm.destinationCity = FlightSearchConfig.flightForm.destinationCity.replace(/[^0-9a-zA-Z-]/gi, '');
        var urlLink = FlightSearchConfig.flightForm.originCity + '-' + FlightSearchConfig.flightForm.destinationCity + '-' +
            FlightSearchConfig.flightForm.origin + '-' + FlightSearchConfig.flightForm.destination + '/' + flightSearchParam;
        $('.form-flight').attr("action", "id/tiket-pesawat/cari/" + urlLink);
        gotoFlightSearch(urlLink);
        //$('.form-flight').submit();
    }

    function gotoFlightSearch(url) {
        window.location = window.location.origin + '/id/tiket-pesawat/cari/' + url;
    }
}

// accordion functions
function accordionFunctions() {
    //Accordion Help Section by W3School
    var acc = document.getElementsByClassName("accordion");
    var i;

    for (i = 0; i < acc.length; i++) {
        acc[i].onclick = function () {
            this.classList.toggle("active");
            this.nextElementSibling.classList.toggle("show");
        }
    }
}

function backToTop() {
    jQuery(document).ready(function ($) {
        if ($('#back-to-top').length) {
            var scrollTrigger = 700, // px
                backToTop = function () {
                    var scrollTop = $(window).scrollTop();
                    if (scrollTop > scrollTrigger) {
                        $('#back-to-top').addClass('show');
                    } else {
                        $('#back-to-top').removeClass('show');
                    }
                };
            backToTop();
            $(window).on('scroll', function () {
                backToTop();
            });
            $('#back-to-top').on('click', function (e) {
                e.preventDefault();
                $('html,body').animate({
                    scrollTop: 0
                }, 700);
            });
        }
    });
}

function goTop() {
    $('.go-top').on('click', function () {
        $('html,body').animate({ scrollTop: 0 }, 700);
    });
}


//// ===============================================
//// currently unimplemented codes
//// ===============================================

//function changeMainTab() {
//    jQuery(document).ready(function ($) {
//        $('body .menu-main li').click(function() {
//            if ($(this).is('#header-flight')) {
//                var itemF = $(this).closest('.site-header').parent();

//                itemF.parent().find('.tab-header').find('.flight').addClass('active');
//                itemF.parent().find('.tab-header').find('.flight').siblings().removeClass('active');
//                itemF.parent().find('#plane').addClass('active');
//                itemF.parent().find('#plane').siblings().removeClass('active');

//                var linkF = $(this).find('a').attr('id', '#plane');

//                linkF.parent().addClass('active');
//                linkF.parent().siblings().removeClass('active');

//            } else if ($(this).is('#header-hotel')) {
//                var item = $(this).closest('.site-header').parent();

//                item.parent().find('.tab-header').find('.hotel').addClass('active');
//                item.parent().find('.tab-header').find('.hotel').siblings().removeClass('active');
//                item.parent().find('#hotel').addClass('active');
//                item.parent().find('#hotel').siblings().removeClass('active');

//                var link = $(this).find('a').attr('id', '#hotel');

//                link.parent().addClass('active');
//                link.parent().siblings().removeClass('active');

//            }
//        });
//    });
//}

// function priceSlider() {
//     jQuery(document).ready(function ($) {
//         $('.slider-wrapper').slick({
//             autoplay: false,
//             dots: false,
//             speed: 700,
//             prevArrow: '<button type="button" class="slick-prev"></button>',
//             nextArrow: '<button type="button" class="slick-next"></button>'
//         });
//     });
// }