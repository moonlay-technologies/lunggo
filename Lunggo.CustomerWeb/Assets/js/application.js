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
        onClose: function(selectedDate) {
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
    function airportAutocomplete() {};

    // ******************************
    // flight type
    $('.flight-type-wrapper label').click(function() {
        
    });

    // ******************************
    // date picker
    $('.flight-departure-date.date-picker').datepicker({
        numberOfMonths: 3,
        minDate: 0,
        dateFormat: 'dd-MM-yy',
        onClose: function(selectedDate) {
            $('.flight-return-date.date-picker').datepicker('option','minDate',selectedDate);
        }
    });
    $('.flight-return-date.date-picker').datepicker({
        numberOfMonths: 3,
        dateFormat: 'dd-MM-yy',
        onClose: function(selectedDate) {}
    });

};

//******************************************
// toggle filter functions
function toggleFilter() {
    $('aside.filter .filter-button').click(function (evt) {
        evt.preventDefault();
        var filter_el = 'aside.filter';
        var state = $(filter_el).attr('data-active');

        if (state == 'true') {
            $(filter_el).attr('data-active', 'false');
            $(filter_el).stop(true).animate({
                left: '-200px'
            });
            $(this).children('.arrow').html('&GT;');
            $(this).children('.text').html('FILTER OFF');
        } else if (state == 'false') {
            $(filter_el).attr('data-active', 'true');
            $(filter_el).stop(true).animate({
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

    toggle_view();

    // hotel search view
    function toggle_view() {
        $('.hotel-search-page .display-option a').click(function (evt) {
            evt.preventDefault();
            $(this).siblings().removeClass('active');
            $(this).addClass('active');
            rearrange();
        });

        function rearrange() {
            var view_type = $('.hotel-search-page .display-option a.active span').attr('class');
            if (view_type == 'fa fa-list') {
                $('section.hotel-list').removeClass('square').addClass('horizontal');
            } else if (view_type == 'fa fa-th') {
                $('section.hotel-list').removeClass('horizontal').addClass('square');
            }
        }

    }

}

//******************************************
// hotel detail functions
function hotelDetail() {

    hotel_image();

    // hotel image
    function hotel_image() {
        var main_image = '.hotel-detail-page .hotel-image .hotel-main-image img';
        $('.hotel-detail-page .hotel-image .hotel-thumb a').click(function (evt) {
            evt.preventDefault();
            var selected_image = $(this).children('img').attr('data-image-url');
            $('.hotel-detail-page .hotel-image .hotel-main-image').css('background-image', 'url(' + selected_image + ')');
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
