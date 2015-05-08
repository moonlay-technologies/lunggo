//******************************************
// on document ready
$(document).ready(function () {

    homePageFunctions();
    toggleFilter();
    hotelSearch();
    hotelDetail();
    flightSearchFunctions();
    hotelSearchFormFunctions();
    flightSearchFormFunctions();
    paymentOptionForm();

});

//******************************************
// payment option form
function paymentOptionForm() {

    $('.payment-form .payment-option label').click(function () {
        var activeClass = $(this).attr('for');
        $('.payment-form .payment-option label').removeClass('active');
        $(this).addClass('active');
        $('.payment-form .payment-detail section').removeClass('active');
        $('.payment-form .payment-detail section.' + activeClass + '-detail').addClass('active');
    });

}

//******************************************
// Home page functions
function homePageFunctions() {

    switchForm();

    //**********
    function switchForm() {
        $('.form-switch .switch label').click(function() {
            $(this).siblings().removeClass('active');
            $(this).addClass('active');

            var targetForm = $(this).attr('data-target');

            $('.form-main-content > div').removeClass('active');
            $('.form-main-content .form-'+targetForm+'-search').addClass('active');

        });
    }

}


//******************************************
// hotel search form
function hotelSearchFormFunctions() {

    // ******************************
    // location auto complete
    var hotelAutocompleteCache = {};
    $('.hotel-location.autocomplete').autocomplete({
        minLength: 3,
        source: function (request, response) {
            var term = request.term;
            if (term in hotelAutocompleteCache) {
                response(hotelAutocompleteCache[term]);
                return;
            }
            $.getJSON(HotelAutocompleteConfig.Url+request.term, function (data, status, xhr) {
                var cleanData = [];
                for (var i = 0; i < data.length; i++) {
                    var newData = {
                        label: data[i].LocationName + ' ,'+data[i].RegionName+' ,'+data[i].CountryName,
                        value: data[i].LocationId
                    };
                    cleanData.push(newData);
                }
                hotelAutocompleteCache[term] = cleanData;
                response(cleanData);
            });
        },
        select: function (event, ui) {
            var userInputValue = $('.hotel-location.autocomplete').val();
            $('.hotel-location.autocomplete').attr('data-userInputValue', userInputValue);
            event.preventDefault();
            $('.hotel-location.autocomplete').val(ui.item.label);
            $('#hotel-search-location').val(ui.item.value);
            $('.hotel-checkin').focus();
        }
    });
    $('.hotel-location.autocomplete').focus(function() {
        var currentValue = $(this).val();
        var userInputValue = $(this).attr('data-userInputValue');
        $(this).val(userInputValue);
    });

    // ******************************
    // date picker
    $('.hotel-checkin.date-picker').datepicker({
        minDate: 0,
        numberOfMonths: 3,
        firstDay: 1,
        dateFormat: "dd-MM-yy",
        altField: '#hotel-search-checkin',
        altFormat: 'yy-mm-dd',
        onClose: function (selectedDate) {
            // set checkout minDate to checkin date + 1
            var checkoutMinDate = new Date(selectedDate);
            var checkoutMaxDate = new Date(selectedDate);
            checkoutMinDate.setDate(checkoutMinDate.getDate() + 1);
            checkoutMaxDate.setDate(checkoutMaxDate.getDate() + 5);
            $('.hotel-checkout.date-picker').datepicker('option', 'minDate', checkoutMinDate);
            $('.hotel-checkout.date-picker').datepicker('option', 'maxDate', checkoutMaxDate);
            $('.hotel-checkout.date-picker').focus();
            calculateDate();
        }
    });
    $('.hotel-checkout.date-picker').datepicker({
        numberOfMonths: 3,
        firstDay: 1,
        dateFormat:"dd-MM-yy",
        onClose: function() {
            calculateDate();
        }
    });

    function calculateDate() {
        var checkinDate = new Date( $('.hotel-checkin.date-picker').val() );
        var checkoutDate = new Date( $('.hotel-checkout.date-picker').val() );
        var stayLength = Math.abs(checkoutDate - checkinDate);
        var stayLengthValue = Math.ceil(stayLength / (1000 * 3600 * 24));
        $('.hotel-room').val(stayLengthValue);
    }

    // ******************************
    // submit form
    $('.form-hotel-search .submit-button').click(function() {
        $('.form-hotel-search form').submit();
    });

}

//******************************************
// flight search form
function flightSearchFormFunctions() {

    // activate functions
    $(document).ready(function () {});

    // ******************************
    // validate form
    function validateForm() {

    };

    // ******************************
    // flight autocomplete
    var flightAutocompleteCache = {};
    $('.flight-origin.autocomplete, .flight-destination.autocomplete').autocomplete({
        minLength: 3,
        source: function (request, response) {
            var term = request.term;
            if (term in flightAutocompleteCache) {
                response(flightAutocompleteCache[term]);
                return;
            }
            $.getJSON(FlightAutocompleteConfig.Url + request.term, function (data, status, xhr) {
                var cleanData = [];
                for (var i = 0; i < data.length; i++) {
                    var newData = {
                        label: data[i].Name + ' ,' + data[i].City + ' ,' + data[i].Country,
                        value: data[i].Code
                    };
                    cleanData.push(newData);
                }
                flightAutocompleteCache[term] = cleanData;
                response(cleanData);
            });
        },
        select: function (event, ui) {
            event.preventDefault();
            $(event.target).attr('data-userInputValue', $(event.target).val());
            $(event.target).val(ui.item.label);
            $(event.target).attr('data-airportCode', ui.item.value);
            if ($(event.target).hasClass('flight-origin')) {
                if ($('.form-flight-search form').attr('data-flightType') == 'round-trip') {
                    $('.flight-destination').focus();
                }
            } else if ($(event.target).hasClass('flight-destination')) {
                $('.flight-departure-date').focus();
            }
        }
    });
    $('.flight-origin.autocomplete, .flight-destination.autocomplete').each(function() {
        $(this).focus(function() {
            var userInputValue = $(this).attr('data-userInputValue');
            $(this).val(userInputValue);
        });
    });

    // ******************************
    // flight type
    $('.flight-type-wrapper label').click(function() {
        var flightType = $(this).attr('for');
        $('.form-flight-search form').attr('data-flightType', flightType);
        if (flightType == 'one-way') {
            $('.flight-return-date').hide();
            $('.flight-return-disable').show();
        } else if (flightType == 'round-trip') {
            $('.flight-return-date').show();
            $('.flight-return-disable').hide();
        }
    });

    // ******************************
    // date picker
    $('.flight-departure-date.date-picker').datepicker({
        altField: '.form-flight-search .flight-departure-date-real',
        altFormat: 'dd-mm-yy',
        numberOfMonths: 3,
        minDate: 0,
        dateFormat: 'dd-MM-yy',
        onClose: function(selectedDate) {
            $('.flight-return-date.date-picker').datepicker('option', 'minDate', selectedDate);
            $('.flight-return-date.date-picker').focus();
        }
    });
    $('.flight-return-date.date-picker').datepicker({
        altField: '.form-flight-search .flight-return-date-real',
        altFormat: 'dd-mm-yy',
        numberOfMonths: 3,
        dateFormat: 'dd-MM-yy',
        onClose: function() {}
    });

    // ******************************
    // generate
    $('.flight-submit-button').click(function(evt) {
        evt.preventDefault();
        var flightSearchData = {};
        flightSearchData.flightType = $('.form-flight-search form').attr('data-flightType');
        flightSearchData.originAirport = $('.form-flight-search .flight-origin').attr('data-airportCode');
        flightSearchData.destinationAirport = $('.form-flight-search .flight-destination').attr('data-airportCode');
        flightSearchData.departureDateTemp = $('.form-flight-search .flight-departure-date-real').val();
        flightSearchData.departureDate = flightSearchData.departureDateTemp.substring(0, 2) + flightSearchData.departureDateTemp.substring(3, 5) + flightSearchData.departureDateTemp.substring(8, 10);
        flightSearchData.returnDateTemp = $('.form-flight-search .flight-return-date-real').val();
        flightSearchData.returnDate = flightSearchData.returnDateTemp.substring(0,2) + flightSearchData.returnDateTemp.substring(3,5) + flightSearchData.returnDateTemp.substring(8,10) ;
        flightSearchData.adult = $('.form-flight-search .flight-adult').val();
        flightSearchData.child = $('.form-flight-search .flight-child').val();
        flightSearchData.infant= $('.form-flight-search .flight-infant').val();
        flightSearchData.cabin = $('.form-flight-search .flight-cabin').val();

        flightSearchData.departInfo = flightSearchData.originAirport + flightSearchData.destinationAirport + flightSearchData.departureDate;
        flightSearchData.returnInfo = flightSearchData.destinationAirport + flightSearchData.originAirport + flightSearchData.returnDate;

        if (flightSearchData.flightType == 'one-way') {
            flightSearchData.info = flightSearchData.departInfo;
        } else if (flightSearchData.flightType == 'round-trip') {
            flightSearchData.info = flightSearchData.departInfo + '.' + flightSearchData.returnInfo;
        }
        flightSearchData.info = flightSearchData.info + '-' + flightSearchData.adult + flightSearchData.child + flightSearchData.infant + flightSearchData.cabin;

        $('#flight-data-info').val(flightSearchData.info);

        console.log(flightSearchData.info);

        $('.form-flight-search form').submit();

    });

};

//******************************************
// toggle filter functions
function toggleFilter() {
    $('aside.filter .filter-button').click(function (evt) {
        evt.preventDefault();
        var filterEl = 'aside.filter';
        var state = $(filterEl).attr('data-active');

        if (state == 'true') {
            $(filterEl).attr('data-active', 'false');
            $(filterEl).stop(true).animate({
                left: '-200px'
            });
            $(this).children('.arrow').html('&GT;');
            $(this).children('.text').html('FILTER OFF');
        } else if (state == 'false') {
            $(filterEl).attr('data-active', 'true');
            $(filterEl).stop(true).animate({
                left: '0'
            });
            $(this).children('.arrow').html('&LT;');
            $(this).children('.text').html('FILTER ON');
        }

    });
}

//******************************************
// hotel search functions
function hotelSearch() {

    toggleSearchForm();
    toggleView();
    hotelSearchForm();

    // hotel search form
    function hotelSearchForm() {
        
    }

    // toggle search form
    function toggleSearchForm() {
        $('.show-hotel-search-form').click(function() {
            $('.hotel-search-form').stop().slideToggle();
        });
    }

    // hotel search view
    function toggleView() {
        $('.hotel-search-page .display-option a').click(function (evt) {
            evt.preventDefault();
            $(this).siblings().removeClass('active');
            $(this).addClass('active');
            rearrange();
        });

        function rearrange() {
            var viewType = $('.hotel-search-page .display-option a.active span').attr('class');
            if (viewType == 'fa fa-list') {
                $('section.hotel-list').removeClass('square').addClass('horizontal');
            } else if (viewType == 'fa fa-th') {
                $('section.hotel-list').removeClass('horizontal').addClass('square');
            }
        }

    }

}

//******************************************
// hotel detail functions
function hotelDetail() {

    hotelImage();

    // hotel image
    function hotelImage() {
        var mainImage = '.hotel-detail-page .hotel-image .hotel-main-image img';
        $('.hotel-detail-page .hotel-image .hotel-thumb a').click(function (evt) {
            evt.preventDefault();
            var selectedImage = $(this).children('img').attr('data-image-url');
            $('.hotel-detail-page .hotel-image .hotel-main-image').css('background-image', 'url(' + selectedImage + ')');
        });
    }

}

//******************************************
// flight search functions
function flightSearchFunctions() {

    toggleFlightDetail();

    // toggle flight detail
    function toggleFlightDetail() {
        $('.flight-list').on('click', '.flight .flight-toggle', function (evt) {
            console.log('JEMPING CLICKED');
            evt.preventDefault();
            $(this).closest('.flight').children('.flight-detail').stop().slideToggle();
            $(this).toggleClass('active');
            $(this).closest('.flight').toggleClass('active');
        });
    }

}

//******************************************
// Checkout Page functions
$('.validate-fare').click(function (evt) {
    evt.preventDefault();

    var hashKey = $(this).attr('data-hashKey');

    if (RevalidateConfig.working == false) {
        RevalidateConfig.working = true;

        loading_overlay('show');

        $.ajax({
            method: 'GET',
            url: RevalidateConfig.Url,
            data: { HashKey: hashKey }
        }).success(function (returnData) {
            RevalidateConfig.working = false;

            if (returnData.IsValid == true) {
                $('#flight-customer-form').submit();
            } else if (returnData.IsValid == false && returnData.IsOtherFareAvailable == true) {
                var userConfirmation = confirm("The price for the flight has been updated. The new price is : " + returnData.NewFare + ". Do you want to continue ?");
                if (userConfirmation) {
                    loading_overlay('hide');
                    $('#flight-customer-form').submit();
                }
            } else if (returnData.IsValid == false && returnData.IsOtherFareAvailable == false) {
                loading_overlay('hide');
                alert("Sorry, the flight is no longer valid. Please check another flight.");
            }

        });
    } else {
        console.log('system  busy. Please wait');
    }

});


//******************************************
// Show loading
function loading_overlay(state) {

    if (state == 'show') {

        if ($('body').has('.loading-overlay').length == 0) {
            createLoading();
            $('body').children('.loading-overlay').show();
        } else {
            $('body').children('.loading-overlay').show();
        }

    } else if (state == 'hide') {
        $('body').children('.loading-overlay').hide();
    }

    // create loading screen
    function createLoading() {
        $('body').append('<div class="loading-overlay"> <div class="loading-content"><img src="/assets/images/loading-image.gif" /></div> </div>');
    }

}
