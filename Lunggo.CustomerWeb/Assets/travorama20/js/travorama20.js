$(function() {
    $('[data-toggle="tooltip"]').tooltip();
});

if (typeof(angular) == 'object') {
    var app = angular.module('travorama', ['ngRoute']);
}

//********************
// variables
var currentDate = new Date();

//********************
// site header function
$('html').click(function() {
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
            month = 'May';
            break;
        case 5:
            month = 'Jun';
            break;
        case 6:
            month = 'Jul';
            break;
        case 7:
            month = 'Aug';
            break;
        case 8:
            month = 'Sep';
            break;
        case 9:
            month = 'Oct';
            break;
        case 10:
            month = 'Nov';
            break;
        case 11:
            month = 'Dec';
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

    $('form.subscribe-form input[type="submit"]').click(function(evt) {
        evt.preventDefault();
        validateForm();
    });
    function validateForm() {
        $('form.subscribe-form input[type="submit"]').prop('disabled',true);
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
            $('form.subscribe-form input.subscribe-name').attr('placeholder', 'Mohon masukan Alamat Nama Anda');
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
        $.ajax({
            url: SubscribeConfig.Url,
            method: 'POST',
            data: { address : SubscribeConfig.email, name : SubscribeConfig.name }
        }).done(function (returnData) {

            $('form.subscribe-form').hide();
            $('.subscribe-form.subscribed').show();

        });
    }

}

//********************
// flight page functions
function flightPageFunctions() {
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
    $('.search-result-filter .filter-trigger span').click(function() {
        var targetFilter = $(this).attr('data-target');
        $('.search-result-filter .filter-content>div').removeClass('active');
        $('.search-result-filter .filter-content>div#'+targetFilter).addClass('active');
    });
}

//********************
// index page functions
function indexPageFunctions() {
    flightFormSearchFunctions();

    $(document).ready(function() {
        changeTheme(indexPageDestination);
    });
    /*
    $('.section-popular .destination a').click(function(evt) {
        evt.preventDefault();
        var target = $(this).attr('data-target');
        changeTheme(target);
    });
    */
    // change header background
    function changeTheme(location) {
        location = location.toLowerCase();
        var backgroundImage = "";
        var locationCode = '';
        if ( location.length > 0 ) {
            switch (location) {
                case "jakarta":
                    backgroundImage = '/Assets/images/campaign/jakarta.jpg';
                    locationCode = 'CGK';
                    break;
                case "bandung":
                    backgroundImage = '/Assets/images/campaign/bandung.jpg';
                    locationCode = 'BDO';
                    break;
                case "surabaya":
                    backgroundImage = '/Assets/images/campaign/surabaya.jpg';
                    locationCode = 'SUB';
                    break;
                case "yogyakarta":
                    backgroundImage = '/Assets/images/campaign/yogyakarta.jpg';
                    locationCode = 'JOG';
                    break;
                case "bali":
                    backgroundImage = '/Assets/images/campaign/bali.jpg';
                    locationCode = 'DPS';
                    break;
                case "singapore":
                    backgroundImage = '/Assets/images/campaign/singapore.jpg';
                    locationCode = 'SIN';
                    break;
                case "malaysia":
                    backgroundImage = '/Assets/images/campaign/malaysia.jpg';
                    locationCode = 'KUL';
                    break;
                case "hongkong":
                    backgroundImage = '/Assets/images/campaign/hongkong.jpg';
                    locationCode = 'HKG';
                    break;
            }

            // change value on HTML
            $('.form-flight-destination').val(location+' ('+locationCode+')');
            $('.slider').css('background-image','url('+backgroundImage+')');
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
    $('.toggle-all').click(function() {
        $(this).closest('li').children('ol').children('li').removeClass('active');


        
    });
    $('.question-wrapper ol li ol li header').click(function() {
        $(this).closest('li').toggleClass('active');
    });


}

//********************
// flight form search function
function flightFormSearchFunctions() {
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
        destination: '',
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
    $('.form-flight-type').click(function() {
        FlightSearchConfig.flightForm.type = $('input[name="flightType"]:checked').val();

        $('.change-flight-class .option, .form-flight-passenger .option , .search-location, .search-calendar').hide();

        if (FlightSearchConfig.flightForm.type == 'return') {
            $('.form-flight-oneway').hide();
            $('.form-flight-return').show();
            $('.form-flight-return').removeClass('disabled');
        } else {
            $('.form-flight-oneway').show();
            $('.form-flight-return').hide();
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
        if ( parseInt($('.search-location .location-recommend .tab-header nav ul').css('margin-left')) < 0 ) {
            $('.search-location .location-recommend .tab-header nav ul').css('margin-left','+=135px');
        }
    });
    $('.search-location .location-recommend .nav-click.next').click(function (evt) {
        evt.preventDefault();
        if (parseInt($('.search-location .location-recommend .tab-header nav ul').css('margin-left')) > -(135 * ($('.search-location .location-recommend .tab-header nav ul li').length - 4))) {
            $('.search-location .location-recommend .tab-header nav ul').css('margin-left', '-=135px');
        }
    });
    $('.search-location .location-recommend nav ul li ').click(function() {
        var showClass = $(this).attr('data-show');
        $(this).addClass('active');
        $(this).siblings().removeClass('active');
        $('.search-location .location-recommend .tab-content>div').removeClass('active');
        $('.search-location .location-recommend .tab-content>div.'+showClass).addClass('active');
    });
    $('.search-location .location-recommend .tab-content a').click(function (evt) {
        evt.preventDefault();
        var locationCode = $(this).attr('data-code');
        if ($('.search-location').attr('data-place') == 'origin') {
            FlightSearchConfig.flightForm.origin = locationCode;
            $('.form-flight-origin').val($(this).text() + ' ('+locationCode+')' );
        } else {
            FlightSearchConfig.flightForm.destination = locationCode;
            $('.form-flight-destination').val($(this).text() + ' (' + locationCode + ')');
        }
        hideLocation();
    });

    //*****
    // on switch target
    $('.switch-destination').click(function() {
        var prevOrigin = $('.form-flight-origin').val();
        var prevOriginCode = FlightSearchConfig.flightForm.origin;
        var prevDestination = $('.form-flight-destination').val();
        var prevDestinationCode = FlightSearchConfig.flightForm.destination;

        $('.form-flight-origin').val(prevDestination);
        $('.form-flight-destination').val(prevOrigin);
        FlightSearchConfig.flightForm.origin = prevDestinationCode;
        FlightSearchConfig.flightForm.destination = prevOriginCode;
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
        FlightSearchConfig.autocomplete.loading = true;
        $('autocomplete-pre .text-pre').addClass('hidden');
        $('autocomplete-pre .text-loading').removeClass('hidden');
        $.ajax({
            url: FlightAutocompleteConfig.Url + keyword
        }).done(function (returnData) {
            $('.autocomplete-pre').addClass('hidden');
            FlightSearchConfig.autocomplete.loading = false;
            FlightSearchConfig.autocomplete.result = returnData;
            // development start
            console.log(returnData);
            // development end
            generateSearchResult(FlightSearchConfig.autocomplete.result);
            if (returnData.length > 0) {
                $('.autocomplete-no-result').addClass('hidden');
            } else {
                $('.autocomplete-no-result').removeClass('hidden');
            }
        });
    }
    function generateSearchResult(list) {
        $('.autocomplete-result ul').empty();
        for (var i = 0 ; i < list.length; i++) {
            $('.autocomplete-result ul').append('<li data-code="'+ list[i].Code +'">'+list[i].City+' ('+list[i].Code+'), '+list[i].Name+', '+list[i].Country+'</li>');
        }
    }
    // select search result
    $('.autocomplete-result ul').on('click','li',function() {
        var locationCode = $(this).attr('data-code');
        if ( $('.search-location').attr('data-place') == 'origin') {
            FlightSearchConfig.flightForm.origin = locationCode;
            $('.form-flight-origin').val( $(this).text() );
        } else {
            FlightSearchConfig.flightForm.destination = locationCode;
            $('.form-flight-destination').val($(this).text());
        }
        hideLocation();
    });
    // on keypress on form flight search
    $('.form-flight-location').keyup(function (evt) {
        if (evt.keyCode == 27) {
            hideLocation();
        } else {
            if ($(this).val().length > 0) {
                $('.search-location .location-recommend').hide();
                $('.search-location .location-search').show();
                FlightSearchConfig.autocomplete.keyword = $(this).val();
                getLocation(FlightSearchConfig.autocomplete.keyword);
            } else {
                $('.search-location .location-recommend').show();
                $('.search-location .location-search').hide();
            }
        }
    });
    $('.form-flight-location').keydown(function(evt) {
        if (evt.keyCode == 9 || evt.which == 9) {
            evt.preventDefault();
        }
    });

    //*****
    // date selector
    $('.form-flight-departure').click(function() {
        showCalendar('departure');
        $('.date-picker').datepicker('option','minDate', new Date());
    });
    $('.form-flight-return').click(function() {
        if (FlightSearchConfig.flightForm.departureDate) {
            $('.date-picker').datepicker('option', 'minDate', new Date(FlightSearchConfig.flightForm.departureDate));
        } else {
            $('.date-picker').datepicker('option', 'minDate', new Date());
        }
        showCalendar('return');
    });
    // embed date picker into page
    $('.date-picker').datepicker({
        numberOfMonths: 2,
        onSelect: function (data) {
            var target;
            var chosenDate = new Date(data);
            if ( $('.search-calendar').attr('data-date') == 'departure' ) {
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
            $(target + ' .month').html( translateMonth(chosenDate.getMonth()) );
            $(target + ' .year').html(chosenDate.getFullYear());
            $('.search-calendar').hide();
        }
    });

    // set current date as default
    $(document).ready(function() {
        var currentDate = new Date();
        $('.form-flight-departure .date, .form-flight-return .date').html(('0' + currentDate.getDate()).slice(-2));
        $('.form-flight-departure .month, .form-flight-return .month').html(translateMonth(currentDate.getMonth()));
        $('.form-flight-departure .year, .form-flight-return .year').html(currentDate.getFullYear());
    });

    //*****
    // select cabin
    $('.form-flight-class').click(function (evt) {
        evt.stopPropagation();
        $(this).find('.option').toggle();
        $('.form-flight-passenger .option, .search-calendar, .search-location').hide();
    });
    $('.form-flight-class .option span').click(function() {
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
    $('.close-location').click(function() { hideLocation(); });

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
    $('.close-calendar').click(function() { hideCalendar(); });

    //*****
    // validate passenger
    $('.form-flight-passenger').click(function (evt) {
        evt.stopPropagation();
        $('.change-flight-class .option').hide();
        $(this).siblings().children('.option').hide();
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
        if ( parentClass.hasClass('adult') ) {
            if (optionValue > (FlightSearchConfig.flightForm.maxPassenger - (FlightSearchConfig.flightForm.passenger.child + FlightSearchConfig.flightForm.passenger.infant))) {
                alert(alertText.over);
                optionValue = (FlightSearchConfig.flightForm.maxPassenger - (FlightSearchConfig.flightForm.passenger.child + FlightSearchConfig.flightForm.passenger.infant));
            }
            if (FlightSearchConfig.flightForm.passenger.infant > optionValue) {
                alert(alertText.over);
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
            $('.passenger-input.child').text( optionValue );
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
    $('.passenger-input').keyup(function() {
        console.log( $(this).val() );
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
        $(this).prop('disabled',true);
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

    function generateFlightSearchParam() {
        var departureDate = (('0' + FlightSearchConfig.flightForm.departureDate.getDate()).slice(-2) + ('0' + (FlightSearchConfig.flightForm.departureDate.getMonth() + 1)).slice(-2) + FlightSearchConfig.flightForm.departureDate.getFullYear().toString().substr(2, 2));
        var returnDate = (('0' + FlightSearchConfig.flightForm.returnDate.getDate()).slice(-2) + ('0' + (FlightSearchConfig.flightForm.returnDate.getMonth() + 1)).slice(-2) + FlightSearchConfig.flightForm.returnDate.getFullYear().toString().substr(2, 2));
        var departureParam = FlightSearchConfig.flightForm.origin + FlightSearchConfig.flightForm.destination + departureDate;
        var returnParam = FlightSearchConfig.flightForm.destination + FlightSearchConfig.flightForm.origin + returnDate;
        var passengerParam = FlightSearchConfig.flightForm.passenger.adult.toString() + FlightSearchConfig.flightForm.passenger.child.toString() + FlightSearchConfig.flightForm.passenger.infant.toString() + FlightSearchConfig.flightForm.cabin;
        var flightSearchParam;
        // generate flight search param
        if (FlightSearchConfig.flightForm.type == 'return') {
            flightSearchParam = departureParam + '.' + returnParam + '-' + passengerParam;
        } else {
            flightSearchParam = departureParam + '-' + passengerParam ;
        }
        $('.form-flight input[name="info"]').val(flightSearchParam);
        //console.log(flightSearchParam);
        $('.form-flight').submit();
    }

}