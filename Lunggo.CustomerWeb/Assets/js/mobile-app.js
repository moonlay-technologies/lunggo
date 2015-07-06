//------------------------------
// on document ready
$(document).ready(function() {
    siteHeaderFunctions();
    searchFormFunctions();
    toggleElement();
    tabFunction();
});

var GeneralLib = {
    monthName : ["January","February","March","April","May","June","July","August","September","October","November","December"]
};

//------------------------------
// General function

// tab function
function tabFunction() {

    $('[data-function="tab"]').click(function (evt) {
        $('[data-function="tab"].active').removeClass('active');
        $(this).addClass('active');
        evt.preventDefault();
        $('.tab.active').removeClass('active');
        $('.tab'+$(this).attr('data-target')).addClass('active');
    });

}

// toggleElement on click
function toggleElement() {
    
    $(document).ready(function() {
        $('[data-function="toggle"]').each(function () {
            $(this).attr('data-active','false');
        });
    });

    // toggle element on click
    $('[data-function="toggle"]').click(function (evt) {
        evt.preventDefault();
        if ($(this).attr('data-active') == 'false') {
            // hide all current active element
            $( $('[data-function="toggle"][data-active="true"]').attr('data-target') ).hide();
            $('[data-function="toggle"][data-active="true"]').attr('data-active', 'false');
            // show clicked element
            $(this).attr('data-active', 'true');
            $($(this).attr('data-target')).show();
        } else {
            $(this).attr('data-active', 'false');
            $( $(this).attr('data-target') ).hide();
        }
    });

}// toggleElement()

// image slider
function imageSlider() {
    $('.image-slider');
}


//------------------------------
// site header functions
function siteHeaderFunctions() {
    siteMenuFunctions();
    userMenuFunctions();
    //------------------------------
    // site menu functions
    function siteMenuFunctions() {
        $('.site-menu').click(function () {
            $('.user-menu').removeClass('active');
            $('.user-menu-content').hide();
            $(this).toggleClass('active');
            $('.site-menu-content').toggle();
        });
    }// siteMenuFunctions()
    //------------------------------
    // user menu functions
    function userMenuFunctions() {
        $('.user-menu').click(function () {
            $('.site-menu').removeClass('active');
            $('.site-menu-content').hide();
            $(this).toggleClass('active');
            $('.user-menu-content').toggle();
        });
    }// userMenuFunctions()

}

//------------------------------
// home page functions
function homePageFunctions() {
    
    //------------------------------
    // detect form in selection
    $(document).ready(function() {
        if (window.location.hash) {
            if (window.location.hash == '#search-flight') {
                showSearchForm('#search-flight');
            } else if (window.location.hash == '#search-hotel') {
                showSearchForm('#search-hotel');
            }
        }
    });
    // switch to search form section
    $('.section-search a[data-trigger="showSearchForm"]').click(function() {
        showSearchForm( $(this).attr('href') );
    });
    //------------------------------
    // show and switch search form
    function showSearchForm(formSelection) {
        // hide selection section
        if ( $('.home-page .section-search').hasClass('active') ) {
            $('.home-page .section-search').slideUp().removeClass('active');
            $('.home-page .home-search').slideDown().addClass('active');
        }
        // show form section
        $('.home-search .home-search-header button.active').stop().removeClass('active');
        $('.home-search .home-search-content>.active').stop().removeClass('active');
        if (formSelection == '#search-hotel' || formSelection == 'hotel' || formSelection == 'searchHotel' || formSelection == 'search-hotel') {
            $('.home-search-header .search-hotel').addClass('active');
            $('.home-search .home-search-content>#search-hotel').addClass('active');
        } else if (formSelection == '#search-flight' || formSelection == 'flight' || formSelection == 'searchFlight' || formSelection == 'search-flight') {
            $('.home-search-header .search-flight').addClass('active');
            $('.home-search .home-search-content>#search-flight').addClass('active');
        }

    }
    // switch search form
    $('.home-search .home-search-header button').click(function (evt) {
        evt.preventDefault();
        showSearchForm( $(this).attr('data-target') );
    });

}// homePageFunction()

//------------------------------
// search form functions
function searchFormFunctions() {
    hotelFormFunctions();
    flightFormFunctions();
    // move form set to body
    $('.form-set').appendTo('body');
    // close 
    $(document).keyup(function(e) {
        if (e.keyCode == 27) {
            $('.active .close-formset').click();
        }
    });

    // toggle form set
    function toggleFormSet(formSelection) {
        
        // toggle form set
        if ($('.form-set').hasClass('active')) {
            $('.form-set').removeClass('active');
            $('.form-set').stop().animate({
                top: '100%'
            });
        } else {
            $('.form-set').addClass('active');
            $('.form-set').stop().animate({
                top: 0
            });
        }
        // show requested form
        $('.form-set .form').removeClass('active');
        $('.form-set .form' + formSelection).addClass('active');
        $('.form-set .form' + formSelection + ' .initialize').focus();

    }// toggleFormSet
    $('.close-formset').click(function () {
        toggleFormSet();
    });
    // form on click
    $('[data-trigger="show-formset"]').click(function() {
        toggleFormSet( $(this).attr('data-target') );
    });


    //------------------------------
    // hotel form functions
    function hotelFormFunctions() {
        hotelLocationAutocomplete();
        hotelCheckinDate();
        hotelCheckoutDate();
        hotelRoom();
        hotelValidation();

        // hotel location autocomplete
        function hotelLocationAutocomplete() {
            var hotelAutocompleteInput = '.hotel-location.autocomplete';
            var hotelAutocompleteOutput = '.hotel-location.autocomplete-result';
            var hotelAutocompleteCache = {};
            var hotelAutocompleteResult;

            $(hotelAutocompleteInput).keyup(function() {
                var keyword = $(this).val();
                if (keyword.length >= 3) {
                    $(hotelAutocompleteOutput).empty();
                    $(hotelAutocompleteOutput).append('<li>Mencari lokasi...</li>');
                    if (keyword in hotelAutocompleteCache) {
                        hotelAutocompleteResult = hotelAutocompleteCache[keyword];
                        generateResult();
                    } else {
                        $.ajax({
                            dataType: "json",
                            url: HotelAutocompleteConfig.Url + keyword,
                            success: function(result) {
                                hotelAutocompleteCache[keyword] = result;
                                hotelAutocompleteResult = result;
                                generateResult();
                            }
                        });
                    }
                } else {
                    $(hotelAutocompleteOutput).empty();
                    $(hotelAutocompleteOutput).append('<li>Keyword telalu pendek</li>');
                } // if keyword.length > 3
            });
            // function generate result
            function generateResult() {
                $(hotelAutocompleteOutput).empty();
                if (hotelAutocompleteResult.length > 0) {
                    for (var i = 0; i < hotelAutocompleteResult.length; i++) {
                        $(hotelAutocompleteOutput).append('<li data-value="' + hotelAutocompleteResult[i].LocationId + '">' + hotelAutocompleteResult[i].LocationName + ', ' + hotelAutocompleteResult[i].CountryName + '</li>');
                    }
                } else {
                    $(hotelAutocompleteOutput).append('<li>Hasil pencarian tidak bisa ditemukan</li>');
                }
            }
            // select hotel location ID
            $(hotelAutocompleteOutput).on('click','li', function() {
                if ($(this).attr('data-value').length > 0) {
                    $('.hotel-search-param [name="LocationId"]').val($(this).attr('data-value'));
                    $('.hotel-locationid[data-trigger="show-formset"]').html($(this).html());
                    $('.close-formset').click();
                    SearchHotelConfig.LocationId = $(this).attr('data-value');
                }
            });


        }// hotelLocationAutoComplete()
        // hotel checkin date
        function hotelCheckinDate() {
            $('.checkin-calendar').datepicker({
                minDate: new Date(),
                dateFormat: 'yy-mm-dd',
                onSelect: function (selectedDate) {
                    $('.close-formset').click();
                    SearchHotelConfig.checkinDate = selectedDate;
                    SearchHotelConfig.StayDate = selectedDate.split('-').join('');
                    selectedDate = new Date(selectedDate);
                    $('.hotel-checkin-date[data-trigger="show-formset"]').html('<b>Checkin : </b>' + selectedDate.getDate() + ' ' + GeneralLib.monthName[selectedDate.getMonth()] + ' ' + selectedDate.getFullYear() );
                    selectedDate.setDate( selectedDate.getDate() + 1 );
                    $('.checkout-calendar').datepicker('option', 'minDate', selectedDate);
                    $('.hotel-search-param [name="StayDate"]').val(SearchHotelConfig.StayDate);
                    $('.hotel-checkout-date[data-trigger="show-formset"]').attr('data-target','.hotel-checkout-date');
                }
            });
        }// hotelCheckinDate()
        // hotel checkout date
        function hotelCheckoutDate() {
            $('.checkout-calendar').datepicker({
                dateFormat: 'yy-mm-dd',
                onSelect: function(selectedDate) {
                    SearchHotelConfig.checkoutDate = selectedDate;
                    selectedDate = new Date(selectedDate);
                    $('.hotel-checkout-date[data-trigger="show-formset"]').html('<b>Checkout : </b>' + selectedDate.getDate() + ' ' + GeneralLib.monthName[selectedDate.getMonth()] + ' ' + selectedDate.getFullYear());
                    $('.close-formset').click();
                    calculateDate();
                }
            });
        }// hotelCheckoutDate()
        // calculate stay length
        function calculateDate() {
            var checkinDate = new Date( SearchHotelConfig.checkinDate );
            var checkoutDate = new Date( SearchHotelConfig.checkoutDate );
            var stayLength = Math.abs(checkoutDate - checkinDate);
            var stayLengthValue = Math.ceil(stayLength / (1000 * 3600 * 24));
            SearchHotelConfig.StayLength = stayLengthValue;
            $('.hotel-search-param [name="StayLength"]').val(SearchHotelConfig.StayLength);
        }
        // hotel room
        function hotelRoom() {
            SearchHotelConfig.roomCount = 1;

            $('.room-count .decrease').click(function() {
                if (SearchHotelConfig.roomCount > 1) {
                    SearchHotelConfig.roomCount = SearchHotelConfig.roomCount - 1;
                }
                refreshRoom();
            });
            $('.room-count .increase').click(function () {
                if (SearchHotelConfig.roomCount < 4) {
                    SearchHotelConfig.roomCount = SearchHotelConfig.roomCount + 1;
                }
                refreshRoom();
            });

            function refreshRoom() {
                var roomText = 'room'
                if (SearchHotelConfig.roomCount > 1) {
                    roomText = 'Rooms'
                } else {
                    roomText = 'Room'
                }
                $('.hotel-room[data-trigger="show-formset"]').html(SearchHotelConfig.roomCount + ' ' + roomText);
                $('.room-count-number').html(SearchHotelConfig.roomCount + ' ' +roomText);
            }

        }// hotelRoom()

        // hotel validation
        function hotelValidation() {
            $('.search-hotel-form input[type="submit"]').click(function(evt) {
                evt.preventDefault();

                if (SearchHotelConfig.LocationId.length > 0 && SearchHotelConfig.StayDate.length > 0 && SearchHotelConfig.StayLength > 0 && SearchHotelConfig.roomCount > 0) {
                    $('.search-hotel-form').submit();
                }

            });
        }// hotelValidateion()

    }// hotelFormFunctions()

    //------------------------------
    // flight form functions
    function flightFormFunctions() {
        airportAutocomplete();
        flightDepartureDate();
        flightReturnDate();
        flightPassenger();
        flightValidation();

        $('.flight-origin .flight-airport.autocomplete').keyup(function () {
            airportAutocomplete('origin');
        });
        $('.flight-destination .flight-airport.autocomplete').keyup(function () {
            airportAutocomplete('destination');
        });

        // flight airport origin
        function airportAutocomplete(airportTarget) {
            var flightAutocompleteKeyword;
            var flightAutocompleteInput;
            var flightAutocompleteOutput;
            var flightAutocompleteCache = {};
            var flightAutocompleteResult;

            if (airportTarget == 'origin') {
                flightAutocompleteInput = '.flight-origin .flight-airport.autocomplete';
                flightAutocompleteOutput = '.flight-origin .autocomplete-result';
            } else if (airportTarget == 'destination') {
                flightAutocompleteInput = '.flight-destination .flight-airport.autocomplete';
                flightAutocompleteOutput = '.flight-destination .autocomplete-result';
            }

            flightAutocompleteKeyword = $(flightAutocompleteInput).val();

            if (typeof flightAutocompleteKeyword != 'undefined') {
                if (flightAutocompleteKeyword.length >= 3) {   
                    $(flightAutocompleteOutput).empty();
                    $(flightAutocompleteOutput).append('<li>Mencari lokasi...</li>');
                    if (flightAutocompleteKeyword in flightAutocompleteCache) {
                        flightAutocompleteResult = flightAutocompleteCache[flightAutocompleteKeyword];
                        generateResult();
                    } else {
                        $.ajax({
                            dataType: "json",
                            url: FlightAutocompleteConfig.Url + flightAutocompleteKeyword,
                            success: function (result) {
                                flightAutocompleteCache[flightAutocompleteKeyword] = result;
                                flightAutocompleteResult = result;
                                generateResult();
                            }
                        });

                    }
                } else {
                    $(flightAutocompleteOutput).empty();
                    $(flightAutocompleteOutput).append('<li>Keyword telalu pendek</li>');
                }
            }
            // generate result
            function generateResult() {
                $(flightAutocompleteOutput).empty();
                if (flightAutocompleteResult.length > 0) {
                    for (var i = 0; i < flightAutocompleteResult.length; i++) {
                        $(flightAutocompleteOutput).append('<li data-value="' + flightAutocompleteResult[i].Code + '">' + flightAutocompleteResult[i].Code + ', ' + flightAutocompleteResult[i].Country + '</li>');
                    }
                } else {
                    $(flightAutocompleteOutput).append('<li>Hasil pencarian tidak bisa ditemukan</li>');
                }
            }
            // click on result
            $('.flight-airport.autocomplete-result').on('click', 'li', function () {
                if ( $(this).attr('data-value').length > 0 ) {
                    $('.form.active .close-formset').click();
                    var elementTarget = $(this).parent().attr('data-target');
                    $('.form-control.flight-' + elementTarget).empty();
                    if ( $(this).parent().attr('data-target') == 'origin' ) {
                        $('.form-control.flight-' + elementTarget).html('<b>Origin : </b>' + $(this).html());
                        FlightSearchConfig.origin = $(this).attr('data-value');
                    } else {
                        $('.form-control.flight-' + elementTarget).html('<b>Destination : </b>' + $(this).html());
                        FlightSearchConfig.destination = $(this).attr('data-value');
                    }
                }
            });

        }// airportOrigin()

        // flight departure date
        function flightDepartureDate() {

            $('.departure-calendar').datepicker({
                minDate: new Date(),
                dateFormat: 'yy-mm-dd',
                onSelect: function (selectedDate) {
                    FlightSearchConfig.departureDate = selectedDate;
                    $('.return-calendar').datepicker('option', 'minDate', selectedDate);
                    selectedDate = new Date(selectedDate);
                    $('.flight-departure-date[data-trigger="show-formset"]').html('<b>Departure Date : </b>' + selectedDate.getDate() + ' ' + GeneralLib.monthName[selectedDate.getMonth()] + ' ' + selectedDate.getFullYear() );
                    $('.flight-return-date[data-trigger="show-formset"]').attr('data-target','.flight-return-date');
                    $('.close-formset').click();
                }
            });

        }// flightDepartureDate()
        // flight return date
        function flightReturnDate() {

            $('.return-calendar').datepicker({
                dateFormat: 'yy-mm-dd',
                onSelect: function (selectedDate) {
                    FlightSearchConfig.returnDate = selectedDate;
                    selectedDate = new Date(selectedDate);
                    $('.flight-return-date[data-trigger="show-formset"]').html('<b>Return Date : </b>' + selectedDate.getDate() + ' ' + GeneralLib.monthName[selectedDate.getMonth()] + ' ' + selectedDate.getFullYear());
                    $('.close-formset').click();
                }
            });

        }// flightReturnDate()
        // flight passenger
        function flightPassenger() {
            FlightSearchConfig = {
                adult: 1,
                child: 0,
                infant: 0,
                flightType: 'RoundTrip'
            };

            $('div.btn.increase').click(function() {
                var dataTarget = $(this).attr('data-target');
                if (FlightSearchConfig[dataTarget] < 4) {
                    FlightSearchConfig[dataTarget] = FlightSearchConfig[dataTarget] + 1;
                }
                refreshPassenger();
            });

            $('div.btn.decrease').click(function () {
                var dataTarget = $(this).attr('data-target');
                if (FlightSearchConfig[dataTarget] > 1) {
                    FlightSearchConfig[dataTarget] = FlightSearchConfig[dataTarget] - 1;
                }
                refreshPassenger();
            });

            function refreshPassenger() {
                $('.flight-passenger-adult').html(FlightSearchConfig.adult);
                $('.flight-passenger-child').html(FlightSearchConfig.child);
                $('.flight-passenger-infant').html(FlightSearchConfig.infant);
                $('.adult-passenger span').html(FlightSearchConfig.adult + ' Adult');
                $('.child-passenger span').html(FlightSearchConfig.child + ' Child');
                $('.infant-passenger span').html(FlightSearchConfig.infant + ' Infant');
            }

        }// flightPassenger()
        
        // flight validation
        function flightValidation() {
            $('.search-flight-form input[type="submit"]').click(function(evt) {
                evt.preventDefault();
                validateForm();
            });

            $('.flighttype-select label').click(function () {
                $(this).children('input[type="radio"]').prop('checked',true);
                $(this).addClass('active');
                $(this).siblings().removeClass('active');
                FlightSearchConfig.flightType = $(this).children('input[type="radio"]').attr('value');
                // enable / disable return date
                if (FlightSearchConfig.flightType == 'RoundTrip') {
                    $('.flight-return-date').removeClass('hidden');
                    $('.flight-oneway').addClass('hidden');
                } else {
                    $('.flight-return-date').addClass('hidden');
                    $('.flight-oneway').removeClass('hidden');
                }
            });

            // validate form
            function validateForm() {
                if (typeof FlightSearchConfig.origin != 'undefined' && typeof FlightSearchConfig.destination != 'undefined' && typeof FlightSearchConfig.departureDate != 'undefined') {
                    if (FlightSearchConfig.flightType == 'OneWay') {
                        generateFlightInfo();
                    } else {

                        if (typeof FlightSearchConfig.returnDate != 'undefined') {
                            generateFlightInfo();
                        } else {
                            alert('Pilih tanggal penerbangan kembali');
                        }

                    }
                } else {

                    if (typeof FlightSearchConfig.origin == 'undefined') {
                        alert('Pilih bandara awal keberangkatan');
                    }
                    if (typeof FlightSearchConfig.destination == 'undefined') {
                        alert('Pilih bandara tujuan');
                    }
                    if (typeof FlightSearchConfig.departureDate == 'undefined') {
                        alert('Pilih tanggal keberangkatan');
                    }
                    if (typeof FlightSearchConfig.returnDate == 'undefined' && FlightSearchConfig.flightType == "RoundTrip") {
                        alert('Pilih tanggal penerbangan kembali');
                    }

                }
            }

            // generate flight info
            function generateFlightInfo() {

                FlightSearchConfig.departureInfo = FlightSearchConfig.origin + FlightSearchConfig.destination + FlightSearchConfig.departureDate.substr(8, 2) + FlightSearchConfig.departureDate.substr(5, 2) + FlightSearchConfig.departureDate.substr(2, 2);
                FlightSearchConfig.passengerInfo = FlightSearchConfig.adult + '' + FlightSearchConfig.child + '' + FlightSearchConfig.infant;
                FlightSearchConfig.cabin = 'y';

                if (FlightSearchConfig.flightType == 'OneWay') {
                    FlightSearchConfig.info = FlightSearchConfig.departureInfo;
                } else {
                    FlightSearchConfig.returnInfo = FlightSearchConfig.destination + FlightSearchConfig.origin + FlightSearchConfig.returnDate.substr(8, 2) + FlightSearchConfig.returnDate.substr(5, 2) + FlightSearchConfig.returnDate.substr(2, 2);
                    FlightSearchConfig.info = FlightSearchConfig.departureInfo + '.' + FlightSearchConfig.returnInfo ;
                }
                FlightSearchConfig.info = FlightSearchConfig.info + '-' + FlightSearchConfig.passengerInfo + FlightSearchConfig.cabin;

                $('input.flight-info').val(FlightSearchConfig.info);

                $('.search-flight-form').submit();

            }

        }// flightValidation()

    }// flightFormFunctions()

}// searchFormFunction()

//------------------------------
// Hotel search page function
function hotelSearchPageFunctions() {
}// hotelSearchPageFunctions()

