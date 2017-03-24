
$(function () {
    $('[data-toggle="tooltip"]').tooltip();
});

if (typeof (angular) == 'object') {
    var app = angular.module('travorama', ['ngRoute', 'ngResource']);

    app.service('flightParam', function () {
        var param = {
            oriCity: '',
            oriArpt: '',
            destCity: '',
            destArpt:''
        };
        return {
            setPropertyOri: function(oriCity, oriArpt) {
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
                    console.log("Request succeeded");
                    callback(data);
                }).
                error(function (data, status) {
                    console.log("Request failed " + status);
                });
            }
        };
    }]);

    var SOURCE_FILE = "/Config/application.properties";

    xmlTransform = function (data) {
        console.log("transform data");
        var x2js = new X2JS();
        var json = x2js.xml_str2json(data);
        return json.guitars.guitar;
    };

    setData = function (data) {
        $scope.dataSet = data;
    };
};

//********************
// variables
var currentDate = new Date();
var trial = 0;

//********************
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

//********************
// general functions

// get parameter
function getParam(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

// translate month
function translateMonth(month) {
    switch (month) {
        case 0:
            month = 'Jan';
            break;
        case 1:
            month = 'Feb';
            break;
        case 2:
            month = 'Mar';
            break;
        case 3:
            month = 'Apr';
            break;
        case 4:
            month = 'Mei';
            break;
        case 5:
            month = 'Jun';
            break;
        case 6:
            month = 'Jul';
            break;
        case 7:
            month = 'Agu';
            break;
        case 8:
            month = 'Sep';
            break;
        case 9:
            month = 'Okt';
            break;
        case 10:
            month = 'Nov';
            break;
        case 11:
            month = 'Des';
            break;
    }
    return month;
}


//********************
// subscribe form functions
function validateEmail(email) {
    var re = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;
    return re.test(email);
}
function subscribeFormFunctions() {
    SubscribeConfig.email = '';
    SubscribeConfig.name = '';

    $('form.subscribe-form input[type="submit"]').click(function (evt) {
        evt.preventDefault();
        validateForm();
    });
    function validateForm() {
        $('form.subscribe-form input[type="submit"]').prop('disabled', true);
        $('form.subscribe-form input[type="submit"]').val('LOADING');
        SubscribeConfig.email = $('form.subscribe-form input.subscribe-email').val();
        SubscribeConfig.name = $('form.subscribe-form input.subscribe-name').val();

        if ($('form.subscribe-form input.subscribe-email').val()) {
            var emailValue = $('form.subscribe-form input.subscribe-email').val();
            console.log('email validation : ' + validateEmail(emailValue));
            if (validateEmail(emailValue)) {
                SubscribeConfig.email = emailValue;
            } else {
                SubscribeConfig.email = '';
                alert('Alamat email tidak valid');
            }
        } else {
            $('form.subscribe-form input.subscribe-email').attr('placeholder', 'Mohon masukan Alamat Email Anda');
            $('form.subscribe-form input.subscribe-email').parent().addClass('has-error');
        }

        if ($('form.subscribe-form input.subscribe-name').val()) {
            SubscribeConfig.name = $('form.subscribe-form input.subscribe-name').val();
        } else {
            $('form.subscribe-form input.subscribe-name').attr('placeholder', 'Mohon masukan Nama Anda');
            $('form.subscribe-form input.subscribe-name').parent().addClass('has-error');
        }

        if (SubscribeConfig.name && SubscribeConfig.email) {
            submitForm();
        } else {
            recheckForm();
        }

    }

    function recheckForm() {
        SubscribeConfig.email = '';
        SubscribeConfig.name = '';
        $('form.subscribe-form input[type="submit"]').removeProp('disabled');
        $('form.subscribe-form input[type="submit"]').val('DAFTAR');
    }

    function submitForm() {
        if (trial > 3) {
            trial = 0;
        }
        $('form.subscribe-form .subscribe-email, form.subscribe-form .subscribe-name').prop('disabled', true);
        $.ajax({
            url: SubscribeConfig.Url,
            method: 'POST',
            data: JSON.stringify({ "email": SubscribeConfig.email, "name": SubscribeConfig.name }),
            headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
        }).done(function (returnData) {

            $('.subscribe-before').hide();
            $('.subscribe-after').show();

        }).error(function (returnData) {
            trial++;
            if (refreshAuthAccess() && trial < 4) //refresh cookie
            {
                submitForm();
            }
        });
    }

}


function newsletterFormFunctions() {
    console.log('Saving data');
}
$(document).ready(function () {
    $('form.form-newsletter input[type="submit"]').click(function (evt) {
        validateNewsletterForm();
    });

    function validateNewsletterForm() {
        $('form.form-newsletter input[type="submit"]').prop('disabled', true);
        //$('form.form-newsletter input[type="submit"]').val('LOADING');
        email = $('form.form-newsletter input.input-type').val();
        console.log('Masuk Sini');
        //console.log('email' + NewsletterConfig.email);

        if ($('form.form-newsletter input.input-type').val()) {
            var emailValue = $('form.form-newsletter input.input-type').val();
            console.log('email validation : ' + validateEmail(emailValue));
            if (validateEmail(emailValue)) {
                email = emailValue;
                submitNewsletterForm();
            } else {
                email = '';
                alert('Alamat email tidak valid');
                recheckForm();
            }
        } else {
            $('form.form-newsletter input.input-type').attr('placeholder', 'Mohon masukan Alamat Email Anda');
            $('form.form-newsletter input.input-type').parent().addClass('has-error');
            recheckForm();
        }
    }

    function recheckForm() {
        $('form.form-newsletter input[type="submit"]').removeProp('disabled');
        $('form.form-newsletter input[type="submit"]').val('DAFTAR');
    }

    function submitNewsletterForm() {
        if (trial > 3) {
            trial = 0;
        }
        $('form.form-newsletter .input-type').prop('disabled', true);
        email = $('form.form-newsletter input.input-type').val();
        var div = document.getElementById("thankyou");
        /*if (div.style.display !== "none") {
            div.style.display = "none";
        }*/
        subscriberName = 'subscriber';
        $.ajax({
            url: SubscribeConfig.Url,
            method: 'POST',
            data: JSON.stringify({ "email": email, "name": subscriberName }),
            headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
        }).done(function (returnData) {
            console.log('done');
            console.log(returnData);
            if (returnData.IsSuccess) {
                $('.page-newsletter').hide();
                //$('.thankyou-popup').show();
                //$('.thankyou-popup').modal('show');
                //$('#myModal').modal('show');
                div.style.display = "block";
                $('.close-popup').click(function (e) {
                    e.preventDefault();
                    //$('.thankyou-popup').hide();
                    //$('#myModal').modal('hide');
                    div.style.display = "none";
                });
            }
            else {
                var normalflow = document.getElementById("normalflow");
                var memberexist = document.getElementById("memberexist");
                if (returnData.IsMemberExist) {
                    normalflow.style.display = "none";
                    memberexist.style.display = "block";
                    var close = document.getElementById("newsletter");
                    $('.close-member').click(function (e) {
                        e.preventDefault();
                        close.style.display = "none";
                    });
                }
                else {
                    console.log('failed');
                }
            }

        }).error(function (returnData) {
            trial++;
            if (refreshAuthAccess() && trial < 4) //refresh cookie
            {
                submitNewsletterForm();
            }
        });
    }
});

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
            if (getCookie('accesstoken')) {
                status = 1;
            }
            else {
                status = 0;
            }
        }
        else {
            status = 0;
        }
    });
    return status;
}


function getAnonymousAccessByRefreshToken(refreshToken) {
    var status = 0;
    $.ajax({
        url: LoginConfig.Url,
        method: 'POST',
        async: false,
        data: JSON.stringify(
            {
                "refreshtoken": refreshToken,
                "clientId": "V2toa2VrOXFSWFZOUXpSM1QycEZlRTlIVlhwYWFrVjVUVVJrYlZsVVp6Vk5WRlp0VGtSR2FrOUhSWGhhYWsweFRucGpNRTE2U1RCT2VtTjNXbTFKZDFwcVFUMD0=",
                "clientSecret": "V2tkS2FFOUVhek5QUjFsNFRucFpNVmt5UlRST2JVWnNXVmRKTTA1dFVtaFBSMDVyV1dwQk5WcEhTWGxPZWtwcVRVUkpNVTFCUFQwPQ=="
            }),
        contentType: 'application/json',
    }).done(function (returnData) {
        if (returnData.status == '200') {
            setCookie("accesstoken", returnData.accessToken, returnData.expTime);
            setCookie("refreshtoken", returnData.refreshToken, returnData.expTime);
            if (getCookie('accesstoken')) {
                status = 1;
            }
            else {
                status = 0;
            }
        }
        else {
            status = 0;
        }
    });
    return status;
}


function getLoginAccessByRefreshToken(refreshToken) {
    var status = 0;
    $.ajax({
        url: LoginConfig.Url,
        method: 'POST',
        async: false,
        data: JSON.stringify(
            {
                "refreshtoken": refreshToken,
                "clientId": "V2toa2VrOXFSWFZOUXpSM1QycEZlRTlIVlhwYWFrVjVUVVJrYlZsVVp6Vk5WRlp0VGtSR2FrOUhSWGhhYWsweFRucGpNRTE2U1RCT2VtTjNXbTFKZDFwcVFUMD0=",
                "clientSecret": "V2tkS2FFOUVhek5QUjFsNFRucFpNVmt5UlRST2JVWnNXVmRKTTA1dFVtaFBSMDVyV1dwQk5WcEhTWGxPZWtwcVRVUkpNVTFCUFQwPQ=="
            }),
        contentType: 'application/json',
    }).done(function (returnData) {
        if (returnData.status == '200') {
            setCookie("accesstoken", returnData.accessToken, returnData.expTime);
            setCookie("refreshtoken", returnData.refreshToken, returnData.expTime);
            setCookie("authkey", returnData.accessToken, returnData.expTime);
            if (getCookie('accesstoken')) {
                status = 2;
            }
            else {
                status = 0;
            }
        }
        else {
            status = 0;
        }
    });
    return status;
}

function getAuthAccess() {
    var token = getCookie('accesstoken');
    var refreshToken = getCookie('refreshtoken');
    var authKey = getCookie('authkey');
    var status = 0;

    if (authKey) {
        if (token) {
            return 2;
        }
        else {
            if (refreshToken) {
                status = getLoginAccessByRefreshToken(refreshToken);
                if (status == 0) {
                    status = getAnonymousFirstAccess();
                }
            }
            else {
                return 0; //harusnya gak pernah masuk sini
            }
        }
    }
    else {
        if (token) {
            return 1;
        }
        else {
            //Get Anonymous Token By Refresh Token
            if (refreshToken) {
                status = getAnonymousAccessByRefreshToken(refreshToken);
                if (status == 0) {
                    status = getAnonymousFirstAccess();
                }
            }
            else {
                //For Anynomius at first
                status = getAnonymousFirstAccess();
            }
        }
    }
    return status;
}


function refreshAuthAccess() {
    /*
    * If failed to get Authorization, but accesstoken is still exist
    */
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

            if (status == 2) {
                return true;
            }
            else {
                return false;
            }
        }
        else {
            status = getAnonymousAccessByRefreshToken(refreshToken);
            if (status == 0) {
                status = getAnonymousFirstAccess();
            }

            if (status == 1 || status == 2) {
                return true;
            }
            else {
                return false;
            }
        }
    }
    else {
        getAnonymousFirstAccess();
        return true;
    }
}


//********************
// flight page functions
// Used in Search Result Flight Page
function flightPageFunctions() {

    //***Select one of this***
    flightFormSearchFunctions(); // SELECTED
    //*********END************

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

        //if (ct % 2 == 0) {
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
// index page functions
function indexPageFunctions() {
    flightFormSearchFunctions();

    $(document).ready(function () {
        changeTheme(indexPageDestination);
    });
    // change header background
    function changeTheme(location) {
        location = location.toLowerCase();
        var backgroundImage = "";
        var locationCode = '';
        if (location.length > 0) {
            switch (location) {
                case "jakarta":
                    backgroundImage = '/Assets/images/campaign/jakarta.jpg';
                    location = 'Jakarta';
                    locationCode = 'CGK';
                    break;
                case "bandung":
                    backgroundImage = '/Assets/images/campaign/bandung.jpg';
                    location = 'Bandung';
                    locationCode = 'BDO';
                    break;
                case "surabaya":
                    backgroundImage = '/Assets/images/campaign/surabaya.jpg';
                    location = 'Surabaya';
                    locationCode = 'SUB';
                    break;
                case "yogyakarta":
                    backgroundImage = '/Assets/images/campaign/yogyakarta.jpg';
                    location = 'Yogyakarta';
                    locationCode = 'JOG';
                    break;
                case "bali":
                    backgroundImage = '/Assets/images/campaign/bali.jpg';
                    location = 'Denpasar';
                    locationCode = 'DPS';
                    break;
                case "singapore":
                    backgroundImage = '/Assets/images/campaign/singapore.jpg';
                    location = 'Singapore';
                    locationCode = 'SIN';
                    break;
                case "malaysia":
                    backgroundImage = '/Assets/images/campaign/malaysia.jpg';
                    location = 'Malaysia';
                    locationCode = 'KUL';
                    break;
                case "hong kong":
                    backgroundImage = '/Assets/images/campaign/hongkong.jpg';
                    location = 'Hong Kong';
                    locationCode = 'HKG';
                    break;
            }

            // change value on HTML
            $('.form-flight-destination').val(location + ' (' + locationCode + ')');
            $('.slider').css('background-image', 'url(' + backgroundImage + ')');
            FlightSearchConfig.flightForm.destination = locationCode;
            $('html,  body').stop().animate({
                scrollTop: 0
            });
        }
    }

}


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

//********************
// flight form search function
// used in Index Page 
function flightFormSearchFunctions() {

    $(window).bind("pageshow", function (event) {
        if (event.originalEvent.persisted) {
            $('.flight-submit-button').removeProp('disabled');
            window.location.reload();
        }
    });


    //*****
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
        type: 'return',
        departureDate: '',
        returnDate: '',
        cabin: 'y',
        passenger: {
            adult: 1,
            child: 0,
            infant: 0
        }
    };

    //*****
    // select flight type
    $('.form-flight-type').click(function () {
        FlightSearchConfig.flightForm.type = $('input[name="flightType"]:checked').val();

        $('.change-flight-class .option, .form-flight-passenger .option , .search-location, .search-calendar').hide();

        if (FlightSearchConfig.flightForm.type == 'return') {
            $('.form-flight-return').removeClass('disabled');
        } else {
            $('.form-flight-return').addClass('disabled');
        }

    });

    //*****
    // on flight origin focus
    $('.form-flight-origin').click(function () {
        $(this).select();
        showLocation('origin');
    });
    $('.form-flight-destination').click(function () {
        $(this).select();
        showLocation('destination');
    });

    //*****
    // flight recommendation
    $('.search-location .location-recommend .nav-click.prev').click(function (evt) {
        evt.preventDefault();
        if (parseInt($('.search-location .location-recommend .tab-header nav ul').css('margin-left')) < 0) {
            $('.search-location .location-recommend .tab-header nav ul').css('margin-left', '+=135px');
        }
    });
    $('.search-location .location-recommend .nav-click.next').click(function (evt) {
        evt.preventDefault();
        if (parseInt($('.search-location .location-recommend .tab-header nav ul').css('margin-left')) > -(135 * ($('.search-location .location-recommend .tab-header nav ul li').length - 4))) {
            $('.search-location .location-recommend .tab-header nav ul').css('margin-left', '-=135px');
        }
    });
    $('.search-location .location-recommend nav ul li ').click(function () {
        var showClass = $(this).attr('data-show');
        $(this).addClass('active');
        $(this).siblings().removeClass('active');
        $('.search-location .location-recommend .tab-content>div').removeClass('active');
        $('.search-location .location-recommend .tab-content>div.' + showClass).addClass('active');
    });
    $('.search-location .location-recommend .tab-content a').click(function (evt, sharedProperties) {
        evt.preventDefault();
        var locationCode = $(this).attr('data-code');
        var locationCity = $(this).text();
        if ($('.search-location').attr('data-place') == 'origin') {
            if (locationCity != FlightSearchConfig.flightForm.destinationCity) {
                FlightSearchConfig.flightForm.origin = locationCode;
                FlightSearchConfig.flightForm.originCity = locationCity;
                $('.form-flight-origin').val($(this).text() + ' (' + locationCode + ')');
                $('.flight-submit-button').removeClass('disabled');
            } else {
                $('.form-flight-origin').val($(this).text() + ' (' + locationCode + ')');
                alert('Kota Asal dan Tujuan Tidak Boleh Sama');
                $('.flight-submit-button').addClass('disabled');
            }

        } else {
            if (locationCity != FlightSearchConfig.flightForm.originCity) {
                FlightSearchConfig.flightForm.destination = locationCode;
                FlightSearchConfig.flightForm.destinationCity = locationCity;
                $('.form-flight-destination').val($(this).text() + ' (' + locationCode + ')');
                $('.flight-submit-button').removeClass('disabled');
            } else {
                $('.form-flight-destination').val($(this).text() + ' (' + locationCode + ')');
                alert('Kota Asal dan Tujuan Tidak Boleh Sama');
                $('.flight-submit-button').addClass('disabled');
            }

        }
        hideLocation();
    });

    //*****
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

    //*****
    // autocomplete function
    function getLocation(keyword) {
        if (trial > 3) {
            trial = 0;
        }
        FlightSearchConfig.autocomplete.loading = true;
        $('autocomplete-pre .text-pre').hide();
        $('autocomplete-pre .text-loading').show();
        if (typeof (FlightSearchConfig.autocomplete.cache[keyword]) != "undefined") {
            FlightSearchConfig.autocomplete.result = FlightSearchConfig.autocomplete.cache[keyword];
            console.log('from cache : ');
            console.log(FlightSearchConfig.autocomplete.result);
            generateSearchResult(FlightSearchConfig.autocomplete.result);
            if (FlightSearchConfig.autocomplete.result.length > 0) {
                $('.autocomplete-no-result').hide();
                $('.autocomplete-pre .text-loading').hide();
                $('.autocomplete-result').show();
            } else {
                $('.autocomplete-pre .text-loading').hide();
                $('.autocomplete-result').hide();
                $('.autocomplete-no-result').show();
            }
        } else {
            $.ajax({
                url: FlightAutocompleteConfig.Url + keyword,
                headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
            }).done(function (returnData) {
                $('.autocomplete-pre .text-pre').hide();
                $('.autocomplete-pre .text-loading').hide();
                FlightSearchConfig.autocomplete.loading = false;
                FlightSearchConfig.autocomplete.result = returnData.airports;
                FlightSearchConfig.autocomplete.cache[keyword] = returnData.airports;
                console.log(returnData);
                generateSearchResult(FlightSearchConfig.autocomplete.result);
                if (returnData.airports.length > 0) {
                    $('.autocomplete-no-result').hide();
                    $('.autocomplete-pre .text-loading').hide();
                    $('.autocomplete-result').show();
                } else {
                    $('.autocomplete-pre .text-loading').hide();
                    $('.autocomplete-result').hide();
                    $('.autocomplete-no-result').show();
                }
            }).error(function (returnData) {
                trial++;
                if (refreshAuthAccess() && trial < 4) //refresh cookie
                {
                    getLocation(keyword);
                }
            });
        }
    }
    function generateSearchResult(list) {
        $('.autocomplete-result ul').empty();
        for (var i = 0 ; i < list.length; i++) {
            $('.autocomplete-result ul').append('<li data-code="' + list[i].code + '" data-city="' + list[i].city + '">' + list[i].city + ' (' + list[i].code + '), ' + list[i].name + ', ' + list[i].country + '</li>');
        }
    }
    // select search result
    $('.autocomplete-result ul').on('click', 'li', function () {
        var locationCode = $(this).attr('data-code');
        var locationCity = $(this).attr('data-city');
        if ($('.search-location').attr('data-place') == 'origin') {
            if (locationCity != FlightSearchConfig.flightForm.destinationCity) {
                FlightSearchConfig.flightForm.origin = locationCode;
                FlightSearchConfig.flightForm.originCity = locationCity;
                $('.form-flight-origin').val(locationCity + ' (' + locationCode + ')');
                $('.flight-submit-button').removeClass('disabled');
            } else {
                $('.form-flight-origin').val($(this).text() + ' (' + locationCode + ')');
                alert('Kota Asal dan Tujuan Tidak Boleh Sama');
                $('.flight-submit-button').addClass('disabled');
            }
        } else {
            if (locationCity != FlightSearchConfig.flightForm.originCity) {
                FlightSearchConfig.flightForm.destination = locationCode;
                FlightSearchConfig.flightForm.destinationCity = locationCity;
                $('.form-flight-destination').val(locationCity + ' (' + locationCode + ')');
                $('.flight-submit-button').removeClass('disabled');
            } else {
                $('.form-flight-destination').val($(this).text() + ' (' + locationCode + ')');
                alert('Kota Asal dan Tujuan Tidak Boleh Sama');
                $('.flight-submit-button').addClass('disabled');
            }
        }
        hideLocation();
        console.log("BERHASIL");
    });
    // on keypress on form flight search
    $('.form-flight-location').keyup(function (evt) {
        if (evt.keyCode == 27) {
            hideLocation();
        } else {
            if ($(this).val().length >= 3) {
                $('.search-location .location-recommend').hide();
                $('.search-location .location-search').show();
                FlightSearchConfig.autocomplete.keyword = $(this).val();
                getLocation(FlightSearchConfig.autocomplete.keyword);
            } else {
                $('.search-location .location-recommend').hide();
                $('.search-location .location-search').show();
                $('.search-location .location-search .autocomplete-pre .text-pre').show();
                $('.search-location .location-search .autocomplete-result').hide();
                $('.search-location .location-search .autocomplete-no-result').hide();
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

    //*****
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

        }).error(function (returnData) {

        });
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
            console.log(data);
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
            // FlightSearchConfig.flightForm.cabin = Cookies.get('cabin');
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
                $('.form-flight-type[value="return"]').click();
            } else {
                $('.form-flight-type[value="oneway"]').click();
            }
        }

    });

    $(window).load(function () {
        // flight type
        if (Cookies.get('type')) {
            if (Cookies.get('type').toLowerCase() == 'return') {
                $('.form-flight-type[value="return"]').click();
            } else {
                $('.form-flight-type[value="oneway"]').click();
            }
        }
    });

    //*****
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

    //*****
    // show and hide search location
    function showLocation(place) {
        place = place || $('.search-location').attr('data-place');
        $('.search-location .location-recommend').show();
        $('.search-location .location-search').hide();
        if (place == 'origin') {
            $('.search-location .location-header .origin').removeClass('hidden');
            $('.search-location .location-header .destination').addClass('hidden');
        } else {
            $('.search-location .location-header .origin').addClass('hidden');
            $('.search-location .location-header .destination').removeClass('hidden');
        }
        $('.search-location').attr('data-place', place);
        $('.search-location').attr('id', place);
        $('.search-location').show();
        hideCalendar();
    }

    function hideLocation() {
        $('.search-location').hide();
    }
    $('.close-location').click(function () { hideLocation(); });

    //*****
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
    function hideCalendar() {
        $('.search-calendar').hide();
    }
    $('.close-calendar').click(function () { hideCalendar(); });

    //*****
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
        console.log($(this).val());
        console.log(FlightSearchConfig.flightForm.passenger);
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

    //*****
    // submit validation
    $('.flight-submit-button').on('click', function (evt) {
        $(this).prop('disabled', true);
        evt.preventDefault();
        validateFlightForm();
    });

    function validateFlightForm() {
        if (!FlightSearchConfig.flightForm.departureDate) {
            FlightSearchConfig.flightForm.departureDate = new Date();
        }
        if (!FlightSearchConfig.flightForm.returnDate) {
            FlightSearchConfig.flightForm.returnDate = new Date();
        }
        if (FlightSearchConfig.flightForm.origin && FlightSearchConfig.flightForm.destination) {
            setCookie();
            generateFlightSearchParam();
        } else {
            if (!FlightSearchConfig.flightForm.origin) {
                alert('Please select your origin airport');
                $('.flight-submit-button').removeProp('disabled');
            }
            if (!FlightSearchConfig.flightForm.destination) {
                alert('Please select your destination airpot');
                $('.flight-submit-button').removeProp('disabled');
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
        var departureDate = (('0' + FlightSearchConfig.flightForm.departureDate.getDate()).slice(-2) + ('0' + (FlightSearchConfig.flightForm.departureDate.getMonth() + 1)).slice(-2) + FlightSearchConfig.flightForm.departureDate.getFullYear().toString().substr(2, 2));
        var returnDate = (('0' + FlightSearchConfig.flightForm.returnDate.getDate()).slice(-2) + ('0' + (FlightSearchConfig.flightForm.returnDate.getMonth() + 1)).slice(-2) + FlightSearchConfig.flightForm.returnDate.getFullYear().toString().substr(2, 2));
        var departureParam = FlightSearchConfig.flightForm.origin + FlightSearchConfig.flightForm.destination + departureDate;
        var returnParam = FlightSearchConfig.flightForm.destination + FlightSearchConfig.flightForm.origin + returnDate;
        var passengerParam = FlightSearchConfig.flightForm.passenger.adult.toString() + FlightSearchConfig.flightForm.passenger.child.toString() + FlightSearchConfig.flightForm.passenger.infant.toString() + FlightSearchConfig.flightForm.cabin;
        var flightSearchParam;
        // generate flight search param
        if (FlightSearchConfig.flightForm.type == 'return') {
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
        //console.log(flightSearchParam);
        //$('.form-flight').submit();
    }

    function gotoFlightSearch(url) {
        window.location = window.location.origin + '/id/tiket-pesawat/cari/' + url;
    }
}

//********************
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
