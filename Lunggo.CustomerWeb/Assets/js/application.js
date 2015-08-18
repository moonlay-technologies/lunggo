//******************************************
// on document ready
$(document).ready(function () {

    toggleLanguageSelect();
    homePageFunctions();
    toggleFilter();
    hotelSearch();
    hotelDetail();
    flightSearchFunctions();
    hotelSearchFormFunctions();
    flightSearchFormFunctions();
    paymentOptionForm();
    checkoutPageFunctions();
    modalFunctions();
    toggleFilterMobile();

});

//******************************************
// subscribe form
function subscribeForm() {

    $('.subscribe-form input[type="text"]').keyup(function() {
        $('.subscribe-form input[type="text"]').css('color','#333333');
    });

    $('.subscribe-form .user-name').focus(function () {
        if ($(this).val() == 'Name cannot be blank') {
            $(this).val('');
        }
    });

    $('.subscribe-form .user-email').focus(function() {
        if ($(this).val() == 'Email cannot be blank') {
            $(this).val('');
        }
    });

    $('.subscribe-form .submit-button').click(function (evt) {
        if ($('.subscribe-form .user-name').val().length == 0) {
            $('.subscribe-form .user-name').val('Name cannot be blank');
            $('.subscribe-form .user-name').css('color', '#a94442');
        } else {
            if ($('.subscribe-form .user-email').val().length == 0) {
                $('.subscribe-form .user-email').val('Email cannot be blank');
                $('.subscribe-form .user-email').css('color', '#a94442');
            } else {
                $(this).prop('disabled', true);
                var userEmail = $('.subscribe-form .user-email').val();
                $.ajax({
                    url: "/id/home/newsletter?address=" + userEmail
                }).done(function () {

                    $('.subscribe-form').hide();
                    $('.subscribe-complete').show();

                });
            }
        }
    });

}


//******************************************
// format number in thousand
function formatNumber(num) {
    return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.");
}

//******************************************
// validate checkout form
function validateCheckoutForm() {

    $('.date-picker').datepicker({
        changeMonth: true,
        changeyear: true,
        dateFormat: 'yy-mm-dd'
    });

    var inputTotal = $('input[class~="control-check"]').length;
    var inputValid = 0;

    $('input[class~="control-check"]').keypress(function () {
        checkInput();
    });
    $('input[class~="control-check"]').change(function () {
        checkInput();
    });

    function checkInput() {
        inputValid = 0;
        $('input[class~="control-check"]').each(function () {
            if ($(this).val().length > 0) {
                inputValid = inputValid + 1;
            }
        });
        if (inputValid == inputTotal) {
            $('.send-form').attr('disabled', false);
        } else {
            $('.send-form').attr('disabled', true);
        }
    }

}

//******************************************
// toggle language select
function toggleLanguageSelect() {
    $('[data-toggle="dropdown"]').click(function (evt) {
        evt.stopPropagation();
        $(this).siblings('.dropdown-menu').toggle();
    });
    $('body').click(function() {
        $('.dropdown-menu').hide();
    });
}


//******************************************
// toggle filter mobile
function toggleFilterMobile() {
    $('.flight-filter-mobile .filter-trigger').click(function() {
        console.log('JEMPING');
        $('.flight-filter-mobile-content').toggleClass('hidden');
        $('.flight-filter-mobile .filter-trigger .toggle').toggle();
    });
}


//******************************************
// payment option form
function paymentOptionForm() {

    $('.payment-form .payment-option label').click(function () {
        var activeClass = $(this).attr('for');
        $('.payment-form .payment-option label').removeClass('active');
        $(this).addClass('active');
        $('.payment-form .payment-detail section').removeClass('active');
        $('.payment-form .payment-detail section.' + activeClass + '-detail').addClass('active');
        console.log('');
    });

}

//******************************************
// Home page functions
function homePageFunctions() {

    // disabled for Special Release
    // switchForm();

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

        var formInvalidStatus = "";
        function insertInvalidStatus(statusText) {
            $('.form-flight-search form').attr('data-invalid-status', statusText);
        }

        validateOriginAirport();

        // ***
        function validateOriginAirport() {
            if ($('.flight-origin-airport-real').val().length > 0) {
                $('.flight-origin-airport-real').attr('data-valid', 'true');
                validateDestinationAirport();
            } else {
                formInvalidStatus = "Please select your airport origin";
                insertInvalidStatus(formInvalidStatus);
            }
        }

        function validateDestinationAirport() {
            if ($('.flight-destination-airport-real').val().length > 0) {
                $('.flight-destination-airport-real').attr('data-valid', 'true');
                validateDepartDate();
            } else {
                formInvalidStatus = "Please select your airport destination";
                insertInvalidStatus(formInvalidStatus);
            }
        }

        function validateDepartDate() {
            if ($('.flight-departure-date-real').val().length > 0) {
                $('.flight-departure-date-real').attr('data-valid', 'true');
                validateReturnDate();
            } else {
                formInvalidStatus = "Please select your departure date";
                insertInvalidStatus(formInvalidStatus);
            }
        }

        function validateReturnDate() {
            if ($('.form-flight-search form').attr('data-flightType') == 'round-trip') {
                if ($('.flight-return-date-real').val().length > 0) {
                    $('.flight-return-date-real').attr('data-valid', 'true');
                    $('.form-flight-search form').attr('data-valid', 'true');
                } else {
                    formInvalidStatus = "Please select your return date";
                    insertInvalidStatus(formInvalidStatus);
                }
            } else {
                $('.form-flight-search form').attr('data-valid', 'true');
            }
        }


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
                        label: data[i].Code + ', ' + data[i].Name + ', ' + data[i].City + ', ' + data[i].Country,
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
                $('.flight-origin-airport-real').val(ui.item.value);
                $('.flight-origin-airport-real').attr('data-valid','true');
                if ($('.form-flight-search form').attr('data-flightType') == 'round-trip') {
                    $('.flight-destination').focus();
                }
            } else if ($(event.target).hasClass('flight-destination')) {
                $('.flight-destination-airport-real').val(ui.item.value);
                $('.flight-destination-airport-real').attr('data-valid','true');
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
    // airline autocomplete
    var airlineAutocompleteCache = {};
    $('.airline.autocomplete').autocomplete({
        minLength: 3,
        source: function (request, response) {
            var term = request.term;
            if (term in airlineAutocompleteCache) {
                response(airlineAutocompleteCache[term]);
                return;
            }
            $.getJSON(AirlineAutocompleteConfig.Url + request.term, function (data, status, xhr) {
                var cleanData = [];
                for (var i = 0; i < data.length; i++) {
                    var newData = {
                        label: data[i].Name + ' (' + data[i].Code + ')',
                        value: data[i].Code
                    };
                    cleanData.push(newData);
                }
                airlineAutocompleteCache[term] = cleanData;
                response(cleanData);
            });
        },
        select: function (event, ui) {
            event.preventDefault();
            $(event.target).attr('data-userInputValue', $(event.target).val());
            $(event.target).val(ui.item.label);
            $(event.target).attr('data-airportCode', ui.item.value);
        }
    });
    $('.airline.autocomplete').each(function () {
        $(this).focus(function () {
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
        onClose: function (selectedDate) {
            $('.flight-departure-date-real').attr('data-valid','true');
            $('.flight-return-date.date-picker').datepicker('option', 'minDate', selectedDate);
            $('.flight-return-date.date-picker').focus();
        }
    });
    $('.flight-return-date.date-picker').datepicker({
        altField: '.form-flight-search .flight-return-date-real',
        altFormat: 'dd-mm-yy',
        numberOfMonths: 3,
        dateFormat: 'dd-MM-yy',
        onClose: function() {
            $('.flight-return-date-real').attr('data-valid','true');
        }
    });

    // ******************************
    // validate passengers
    function validatePassengers(changedElement) {
        var maxPassenger = 9;
        var adultPassenger = parseInt($('.flight-adult').val());
        var childPassenger = parseInt($('.flight-child').val());
        var infantPassenger = parseInt($('.flight-infant').val());

        var totalPassenger = adultPassenger + childPassenger + infantPassenger;

        // if adult less than infant
        if ( infantPassenger > adultPassenger ) {
            $('.flight-infant').val( adultPassenger );
            $('.passenger-warning .max-infant').show();
        } else {

            // if total passenger more than 9
            if (totalPassenger > maxPassenger) {

                if (changedElement == 'adult') {
                    $('.flight-adult').val(9 - (childPassenger + infantPassenger));
                } else if (changedElement == 'child') {
                    $('.flight-child').val(9 - (adultPassenger + infantPassenger));
                } else if (changedElement == 'infant') {
                    $('.flight-infant').val(9 - (childPassenger + adultPassenger));
                }

                $('.passenger-warning .max-passenger').show();
            } else {
                $('.passenger-warning .max-passenger').hide();
            }

            $('.passenger-warning .max-infant').hide();
        }


    }// validatePassengers()
    $('.flight-adult').change(function() {
        validatePassengers('adult');
    });
    $('.flight-child').change(function() {
        validatePassengers('child');
    });
    $('.flight-infant').change(function () {
        validatePassengers('infant');
    });

    // ******************************
    // generate flight info data
    $('.flight-submit-button').click(function(evt) {
        evt.preventDefault();
        validateForm();
        var flightSearchData = {};
        flightSearchData.flightType = $('.form-flight-search form').attr('data-flightType');
        flightSearchData.originAirport = $('.form-flight-search .flight-origin-airport-real').val();
        flightSearchData.destinationAirport = $('.form-flight-search .flight-destination-airport-real').val();
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

        // console.log(flightSearchData.info);

        if ( $('.form-flight-search form').attr('data-valid') == 'true' ) {
            $('.form-flight-search form').submit();
        } else {
            alert( $('.form-flight-search form').attr('data-invalid-status') );
        }       


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

    // toggle search form
    function toggleSearchForm() {
        $('.show-hotel-search-form').click(function() {
            $('section.hotel-search-form').stop().slideToggle();
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

    toggleSearchForm();
    toggleFlightDetail();
    toggleFlightFilter();
    defaultFormValue();

    // default form value
    function defaultFormValue() {
        $(document).ready(function() {

            var defaultValue = jQuery.parseJSON($('.flight-search-page').attr('data-flight-search-params'));

            // if trip type == One Way
            if (defaultValue.TripType == 'OneWay') {
                $('.flight-search-form form input#one-way').click();
            }
            // passengers
            $('.flight-search-form form .flight-adult').val(defaultValue.AdultCount);
            $('.flight-search-form form .flight-child').val(defaultValue.ChildCount);
            $('.flight-search-form form .flight-infant').val(defaultValue.InfantCount);
            // cabin class
            if ( defaultValue.CabinClass == 'Economy' ) {
                defaultValue.CabinClass = 'y';
            } else if (defaultValue.CabinClass == 'Business') {
                defaultValue.CabinClass = 'c';
            } else if (defaultValue.CabinClass == 'First') {
                defaultValue.CabinClass = 'f';
            }
            $('.flight-search-form form .flight-cabin').val(defaultValue.CabinClass);

        });
    }

    toggleFlightFilterNew();
    // toggle flight filter
    function toggleFlightFilterNew() {
        $('.filter-trigger-wrapper .filter-trigger').click(function () {
            $('.page-aside .filter-close').toggleClass('active');
            $('.flight-search-page .flight-result').toggleClass('active');
        });
        // close filter
        $('.page-aside .filter-close').click(function () {
            $('.page-aside .filter-close').toggleClass('active');
            $('.flight-search-page .flight-result').toggleClass('active');
        });
    }

    // toggle flight filter
    function toggleFlightFilter() {
        $('.flight-search-filter .filter a.toggle').each(function () {
            $(this).click(function (evt) {
                evt.preventDefault();
                $('.flight-search-filter .filter .filter-container').stop().slideUp('fast');
                $(this).siblings('.filter-container').stop().slideToggle('fast');
            });
        });

        // $(window).scroll(function() {
        //     $('.flight-search-filter .filter .filter-container').stop().slideUp('fast');
        // });
    }

    // toggle search form
    function toggleSearchForm() {
        $('.show-flight-search-form').click(function() {
            $('section.flight-search-form').stop().slideToggle();
        });
    }

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
// Modal Functions
function modalFunctions() {

    // move modal wrapper
    $('.modal-wrapper').each(function () {
        $(this).appendTo('body');
    });
    
}


//******************************************
// Checkout Page functions
function checkoutPageFunctions() {

    // revalidate flight
    $('.validate-fare').click(function(evt) {
        evt.preventDefault();

        var hashKey = $(this).attr('data-hashKey');

        // show modal
        $('.flight-validate-loading').show();

        // revalidate flight
        if (RevalidateConfig.working == false) {
            RevalidateConfig.working = true;

            $.ajax({
                method: 'GET',
                url: RevalidateConfig.Url,
                data: { HashKey: hashKey }
            }).success(function(returnData) {
                RevalidateConfig.working = false;

                if (returnData.IsValid == true) {
                    $('#flight-customer-form').submit();
                } else if (returnData.IsValid == false && returnData.IsOtherFareAvailable == true) {
                    var userConfirmation = confirm("The price for the flight has been updated. The new price is : " + returnData.NewFare + ". Do you want to continue ?");
                    if (userConfirmation) {
                        $('.flight-validate-loading').hide();
                        $('#flight-customer-form').submit();
                    }
                } else if (returnData.IsValid == false && returnData.IsOtherFareAvailable == false) {
                    $('.flight-validate-loading').hide();
                    alert("Sorry, the flight is no longer valid. Please check another flight.");
                }

            });
        } else {
            console.log('system  busy. Please wait');
        }

    });
}

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
