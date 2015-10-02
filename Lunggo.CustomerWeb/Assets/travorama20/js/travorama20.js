$(function () {
    $('[data-toggle="tooltip"]').tooltip()
})

//********************
// variables
var indexPageConfig;

//********************
// site header function
$('html').click(function() {
    $('.dropdown-content').hide();
});
$('[data-trigger="dropdown"]').click(function (evt) {
    evt.stopPropagation();
    evt.preventDefault();
    $(this).siblings('.dropdown-content').toggle();
});



//********************
// index page functions
function indexPageFunctions() {
    //*****
    // index page config
    indexPageConfig = {
        autocomplete: {
            loading: false,
            keyword: {},
            result: {},
            cache: {}
        },
        flightForm: {
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
        }
    };

    //*****
    // select flight type
    $('.form-flight-type').click(function() {
        indexPageConfig.flightForm.type = $('input[name="flightType"]:checked').val();

        if (indexPageConfig.flightForm.type == 'return'){
            $('.form-flight-oneway').hide();
            $('.form-flight-return').show();
        } else {
            $('.form-flight-oneway').show();
            $('.form-flight-return').hide();
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
    // on switch target
    $('.switch-destination').click(function() {
        var prevOrigin = $('.form-flight-origin').val();
        var prevOriginCode = indexPageConfig.flightForm.origin;
        var prevDestination = $('.form-flight-destination').val();
        var prevDestinationCode = indexPageConfig.flightForm.destination;

        $('.form-flight-origin').val(prevDestination);
        $('.form-flight-destination').val(prevOrigin);
        indexPageConfig.flightForm.origin = prevDestinationCode;
        indexPageConfig.flightForm.destination = prevOriginCode;
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
        indexPageConfig.autocomplete.loading = true;
        $.ajax({
            url: FlightAutocompleteConfig.Url + keyword
        }).done(function (returnData) {
            $('.autocomplete-pre').addClass('hidden');
            indexPageConfig.autocomplete.loading = false;
            indexPageConfig.autocomplete.result = returnData;
            // development start
            console.log(returnData);
            // development end
            generateSearchResult(indexPageConfig.autocomplete.result);
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
            indexPageConfig.flightForm.origin = locationCode;
            $('.form-flight-origin').val( $(this).text() );
        } else {
            indexPageConfig.flightForm.destination = locationCode;
            $('.form-flight-destination').val($(this).text());
        }
        hideLocation();
    });
    // on keypress on form flight search
    $('.form-flight-location').keyup(function () {
        indexPageConfig.autocomplete.keyword = $(this).val();
        getLocation(indexPageConfig.autocomplete.keyword);
    });

    //*****
    // date selector
    $('.form-flight-departure').click(function() {
        showCalendar('departure');
        $('.date-picker').datepicker('option','minDate', new Date());
    });
    $('.form-flight-return').click(function() {
        if (indexPageConfig.flightForm.departureDate) {
            $('.date-picker').datepicker('option', 'minDate', new Date(indexPageConfig.flightForm.departureDate));
            showCalendar('return');
        } else {
            $('.form-flight-departure').click();
        }
    });
    // embed date picker into page
    $('.date-picker').datepicker({
        numberOfMonths: 2,
        onSelect: function (data) {
            var target;
            var chosenDate = new Date(data);
            if ( $('.search-calendar').attr('data-date') == 'departure' ) {
                indexPageConfig.flightForm.departureDate = new Date(data);
                target = '.form-flight-departure';
            } else {
                indexPageConfig.flightForm.returnDate = new Date(data);
                target = '.form-flight-return';
            }
            $(target + ' .date').html(chosenDate.getDate());
            $(target + ' .month').html( translateMonth(chosenDate.getMonth()) );
            $(target + ' .year').html(chosenDate.getFullYear());
            $('.search-calendar').hide();
        }
    });

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

    // set current date as default
    $(document).ready(function() {
        var currentDate = new Date();
        // indexPageConfig.flightForm.departureDate = new Date();
        // indexPageConfig.flightForm.returnDate = new Date();
        $('.form-flight-departure .date, .form-flight-return .date').html(currentDate.getDate());
        $('.form-flight-departure .month, .form-flight-return .month').html(translateMonth(currentDate.getMonth()));
        $('.form-flight-departure .year, .form-flight-return .year').html(currentDate.getFullYear());
    });

    //*****
    // select cabin
    $('.form-flight-class').click(function() {
        $(this).siblings('.option').show();
    });
    $('.form-group.flight-class .option span').click(function() {
        $('.form-flight-class').html($(this).html());
        indexPageConfig.flightForm.cabin = $(this).attr('data-value');
        $('.form-group.flight-class .option').hide();
    });

    //*****
    // show and hide search location
    function showLocation(place) {
        place = place || $('.search-location').attr('data-place');
        if (place == 'origin') {
            $('.search-location .location-header .origin').removeClass('hidden');
            $('.search-location .location-header .destination').addClass('hidden');
        } else {
            $('.search-location .location-header .origin').addClass('hidden');
            $('.search-location .location-header .destination').removeClass('hidden');
        }
        $('.search-location').attr('data-place',place);
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
        target = target || $('.search-location').attr('data-date');
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
    $('.passenger-input').keydown(function(e) {
        $(this).val('');
        if ($.inArray(e.keyCode, [46, 8, 27, 13, 110, 190]) !== -1 ||
            (e.keyCode == 65 && (e.ctrlKey === true || e.metaKey === true)) || (e.keyCode >= 35 && e.keyCode <= 40)) {
            return;
        }
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });
    $('.passenger-input').keyup(function() {
        console.log( $(this).val() );
        console.log(indexPageConfig.flightForm.passenger);
        if ($(this).hasClass('adult')) {
            if ($(this).val() < 1) {
                $(this).val(1);
            } else {
                if ($(this).val() > (indexPageConfig.flightForm.maxPassenger - (parseInt($('.passenger-input.child').val()) + parseInt($('.passenger-input.infant').val())))) {
                    $(this).val((indexPageConfig.flightForm.maxPassenger - (parseInt($('.passenger-input.child').val()) + parseInt($('.passenger-input.infant').val()))));
                }
            }
            if ($('.passenger-input.infant').val() > $(this).val()) {
                $('.passenger-input.infant').val($(this).val());
            }
        } else if ($(this).hasClass('child')) {
            if ($(this).val() > (indexPageConfig.flightForm.maxPassenger - (parseInt($('.passenger-input.adult').val()) + parseInt($('.passenger-input.infant').val())))) {
                $(this).val((indexPageConfig.flightForm.maxPassenger - (parseInt($('.passenger-input.adult').val()) + parseInt($('.passenger-input.infant').val()))));
            }
        } else if ($(this).hasClass('infant')) {
            if ( $(this).val() > $('.passenger-input.adult').val() ) {
                $(this).val( $('.passenger-input.adult').val() );
            } else {
                if ($(this).val() > (indexPageConfig.flightForm.maxPassenger - (parseInt($('.passenger-input.child').val()) + parseInt($('.passenger-input.adult').val())))) {
                    $(this).val((indexPageConfig.flightForm.maxPassenger - (parseInt($('.passenger-input.child').val()) + parseInt($('.passenger-input.adult').val()))));
                }
            }
        }
        indexPageConfig.flightForm.passenger.adult = $('.passenger-input.adult').val();
        indexPageConfig.flightForm.passenger.child = $('.passenger-input.child').val();
        indexPageConfig.flightForm.passenger.infant = $('.passenger-input.infant').val();
    });

    //*****
    // submit validation
    $('.flight-submit-button').on('click',function (evt) {
        evt.preventDefault();
        validateFlightForm();
    });

    function validateFlightForm() {
        if (!indexPageConfig.flightForm.departureDate) {
            indexPageConfig.flightForm.departureDate = new Date();
        }
        if (!indexPageConfig.flightForm.returnDate) {
            indexPageConfig.flightForm.returnDate = new Date();
        }
        if (indexPageConfig.flightForm.origin && indexPageConfig.flightForm.destination) {
            generateFlightSearchParam();
        } else {
            if (!indexPageConfig.flightForm.origin) {
                alert('Please select your origin airport');
            }
            if (!indexPageConfig.flightForm.destination) {
                alert('Please select your destination airpot');
            }
        }
    }

    function generateFlightSearchParam() {
        var departureDate = (('0' + indexPageConfig.flightForm.departureDate.getDate()).slice(-2) + ('0' + (indexPageConfig.flightForm.departureDate.getMonth() + 1)).slice(-2) + indexPageConfig.flightForm.departureDate.getFullYear().toString().substr(2,2) );
        var returnDate = (('0' + indexPageConfig.flightForm.returnDate.getDate()).slice(-2) + ('0' + (indexPageConfig.flightForm.returnDate.getMonth() + 1)).slice(-2) + indexPageConfig.flightForm.returnDate.getFullYear().toString().substr(2, 2));
        var departureParam = indexPageConfig.flightForm.origin + indexPageConfig.flightForm.destination + departureDate ;
        var returnParam = indexPageConfig.flightForm.destination + indexPageConfig.flightForm.origin + returnDate;
        var passengerParam = indexPageConfig.flightForm.passenger.adult.toString() + indexPageConfig.flightForm.passenger.child.toString() + indexPageConfig.flightForm.passenger.infant.toString() + indexPageConfig.flightForm.cabin;
        var flightSearchParam;
        // generate flight search param
        if (indexPageConfig.flightForm.type == 'return') {
            flightSearchParam = departureParam + '.' + returnParam + '-' + passengerParam;
        } else {
            flightSearchParam = departureParam + '-' + passengerParam ;
        }
        $('.form-flight input[name="info"]').val(flightSearchParam);
        //console.log(flightSearchParam);
        $('.form-flight').submit();
    }

}