//******************************************
// on document ready
$(document).ready(function(){

	toggle_filter();
	hotelSearch();
	hotelDetail();
	flightSearch();
    switchSearchForm();
    hotelSearchFormFunctions();
    flightSearchFormFunctions();
    paymentOptionForm();

});

//******************************************
// FUNCTIONS

//******************************************
// payment option form
function paymentOptionForm() {

    $('.payment-form .payment-option label').click(function () {
        var activeClass = $(this).attr('for');
        $('.payment-form .payment-option label').removeClass('active');
        $(this).addClass('active');
        $('.payment-form .payment-detail section').removeClass('active');
        $('.payment-form .payment-detail section.'+activeClass+'-detail').addClass('active');
    });

}

//******************************************
// toggle search form
function switchSearchForm() {

    var scrollTarget;
    var buttonPosition;

    $('#site-header .search-switch-button').click(function () {
        // get scroll value
        if ($(this).hasClass('flight')) {
            buttonPosition = {left : '0', marginLeft: '5%'};
            scrollTarget = $('#site-header .header-form-wrapper .flight-form').position().left;
            $(this).removeClass('flight').addClass('hotel');
            $(this).children('img').attr('src', '/Assets/images/search-form-switch-hotel.png');
        } else {
            buttonPosition = {left : '100%', marginLeft: '-10%'};
            scrollTarget = 0;
            $(this).removeClass('hotel').addClass('flight');
            $(this).children('img').attr('src','/Assets/images/search-form-switch-flight.png');
        }

        // scrollLeft the element
        $('#site-header .header-form-wrapper').stop().animate({
            scrollLeft: scrollTarget
        }, {
            duration: 1000
        });

        // animate button
        $(this).stop().animate(buttonPosition, 1000);
    });

    

}

//******************************************
// hotel search form
function hotelSearchFormFunctions() {

    // activate functions
    $(document).ready(function() {
        locationAutocomplete();
        datePicker();
        stayLength();
        roomCount();
        validateForm();
    });

    // ******************************
    // validate form
    function validateForm() {
        
    }

    // ******************************
    // location auto complete
    var locationAutocomplete = function () {

        // settings
        var elInput = 'input.input-location';
        var ajaxUrl = 'http://travorama-apidev.azurewebsites.net/api/v1/autocomplete/hotellocation/';
        var minLength = 3;
        var listWrapper = '.location-autocomplete';

        // generate popup element
        $('body').append('<div class="location-autocomplete"></div>');
        $('.location-autocomplete').css({
            position: 'absolute',
            zIndex: 9999,
            top: $(elInput).offset().top + $(elInput).outerHeight(),
            left: $(elInput).offset().left,
            width: $(elInput).outerWidth()
        });

        // run function on keyup
        $(elInput).keyup(function () {
            var reqVal = $(this).val();
            verifyInput(reqVal);
        });

        $(elInput).focus(function () {
            var reqVal = $(this).val();
            verifyInput(reqVal);
        });

        // function verified input
        function verifyInput(input_val) {
            if (input_val.length >= minLength) {
                $(listWrapper).empty();
                $(listWrapper).append('<li> Loading </li>');
                $(listWrapper).wrapInner('<ul></ul>');
                $(listWrapper).show();
                getResult(input_val);
            } else {
                $(listWrapper).hide();
            }
        }

        // get result
        function getResult(inputVal) {
            $.ajax({
                method: "GET",
                dataType: "json",
                url: ajaxUrl + inputVal
            })
            .done(function (result) {
                showResult(result);
            });
        }

        // show result
        function showResult(data) {
            if (data.length > 0) {
                $(listWrapper).empty();
                for (i = 0; i < data.length; i++) {
                    $(listWrapper).append('<li data-location-id="' + data[i].LocationId + '"><span>' + data[i].LocationName + ',' + data[i].RegionName + ',' + data[i].CountryName + '</span></li>');
                }
                $(listWrapper).wrapInner('<ul></ul>');
                $(listWrapper).show().attr('data-active', 'true');
            } else {
                $(listWrapper).empty();
                $(listWrapper).append('<li class="text-center">Lokasi tidak ditemukan</li>');
                $(listWrapper).wrapInner('<ul></ul>');
                $(listWrapper).show();
            }
        }

        // on click
        selectResult();
        function selectResult() {
            $(listWrapper).on('click', 'li', function () {
                $(elInput).val($(this).text());
                $(listWrapper).hide();
                $(elInput).attr('data-location-id', $(this).attr('data-location-id'));
                $('.search-hotel-form .search-hotel-value.location').val($(this).attr('data-location-id'));
            });
        }

        // hide when click another element
        $('html').click(function () {
            $(listWrapper).hide();
        });
        $("input.input-location , .location-autocomplete").on('click', function (evt) {
            evt.stopPropagation();
        });


    };

    // ******************************
    // date picker
    var datePicker = function() {
        $('.input-checkin.select-date').pickmeup_twitter_bootstrap({
           calendars: 3,
            format: 'Y-m-d',
            hide_on_select: true,
            select_month: false,
            select_year: false,
            separator: '-',
            min: new Date,
            change: function () {
                datePickerCheckout($(this).pickmeup('get_date'));
                $('.search-hotel-value.staydate').val( $(this).val() );
            }
        });
    }

    function datePickerCheckout(the_date) {

        var selectedDate = the_date || new Date;

        $('.input-checkout.select-date').pickmeup_twitter_bootstrap('destroy');
        $('.input-checkout.select-date').pickmeup_twitter_bootstrap({
            calendars: 3,
            format: 'Y-m-d',
            hide_on_select: true,
            select_month: false,
            select_year: false,
            separator: '-',
            min: selectedDate,
            default_date: selectedDate,
            change: function() {
                calculateDate( $(this).pickmeup('get_date') );
            }
        });
        $('.input-checkout.select-date').pickmeup_twitter_bootstrap('update');

    }

    function calculateDate(checkoutDate) {
        var checkinDate = new Date($('.search-hotel-value.staydate').val());
        var checkoutDate = new Date( checkoutDate);
        var stayLength = Math.abs(checkoutDate - checkinDate);
        var stayLengthValue = Math.ceil(stayLength/ (1000 * 3600 * 24));
        $('.search-hotel-value.staylength').val(stayLengthValue);
    }

    // ******************************
    // stay length
    var stayLength = function() {
        
    }

    // ******************************
    // room count
    var roomCount = function (room) {
        var maxRoom = room || 4;
        var roomOption = '.input-room.select-room';
        var roomSelection = '.room-count-selection';

        // generate location autocomplete
        $('body').append('<div class="'+roomSelection.replace('.','')+'"></div>');
        $(roomSelection).css({
            top: $(roomOption).offset().top + $(roomOption).outerHeight(),
            left: $(roomOption).offset().left,
            width: $(roomOption).outerWidth()
        });
        for (var i = 1; i <= maxRoom; i++) {
            var room;
            if (i > 1) {
                room = ' rooms'
            } else {
                room = ' room'
            }
            $(roomSelection).append('<li data-value="' + i + '">' + i + room + ' </li>');
        }
        $(roomSelection).wrapInner('<ul></ul>');

        // show
        $(roomOption).focus(function () {
            $(roomSelection).show();
        });

        // select room option based on keyboard key press
        $(roomOption).on('keydown', function(evt) {
            var keyValue = evt.which;
            var roomValue;
            if (keyValue >= 49 && keyValue<= 57) {
                switch (keyValue) {
                    case 49:
                        roomValue = 1;
                        break;
                    case 50:
                        roomValue = 2;
                        break;
                    case 51:
                        roomValue = 3;
                        break;
                    case 52:
                        roomValue = 4;
                        break;
                    default:
                        roomValue = 4;
                        break;
                }
                $(roomSelection).children('ul').children('li:nth-child('+roomValue+')').click();
                return false;
            } else {
                return false;
            }
        });

        // select room
        $(roomSelection).on('click', 'li', function () {
            $(roomOption).val($(this).html());
            $(roomOption).attr('data-current-room', $(this).attr('data-value'));
            $(roomOption).blur();
            $(roomSelection).hide();
        });


        // hide room option
        $('html').click(function () {
            $(roomSelection).hide();
        });
        $(roomOption).on('click', function (evt) {
            evt.stopPropagation();
        });

    }

}

//******************************************
// flight search form
function flightSearchFormFunctions() {

    // activate functions
    $(document).ready(function() {
        airportAutocomplete();
        validateForm();
        switchFlightType();
        datePicker();
    });

    // ******************************
    // validate form
    function validateForm() {
        
    }

    // ******************************
    // flight autocomplete
    function airportAutocomplete() {

        var autocompleteWrapper = '.airport-autocomplete';
        var ajaxUrl = 'http://travorama-apidev.azurewebsites.net/api/v1/autocomplete/airport/';
        var minLength = 3;
        var elInput = '.airport-autocomplete-input';
        var elPosition = {};

        // generate autocomplete wrapper
        $('body').append('<div class="'+autocompleteWrapper.replace('.','')+'"></div>');

        // run function on keyup
        $(elInput).each(function() {
            $(elInput).keyup(function () {
                var reqVal = $(this).val();
                verifyInput(reqVal);
            });
        });

        $(elInput).each(function () {
            var previousValue;
            $(this).focus(function () {
                previousValue = $(this).val();
                $(this).val('');
                $(autocompleteWrapper).empty();
                $('.autocomplete-current').removeClass('autocomplete-current');
                $(this).addClass('autocomplete-current');
                elPosition.top = $(this).offset().top + $(this).outerHeight();
                elPosition.left = $(this).offset().left;
                $(autocompleteWrapper).css({
                    top: elPosition.top,
                    left: elPosition.left
                });
                $(autocompleteWrapper).attr('data-airport-for', $(this).attr('data-airport-for') );
            });
        });
        
        // verify input
        function verifyInput(reqVal) {
            if (reqVal.length >= minLength) {
                getResult(reqVal);
                $(autocompleteWrapper).show();
            } else {
                $(autocompleteWrapper).hide();
            }
        }

        // get Ajax
        function getResult(locationQuery) {
            $.ajax({
                method: "GET",
                dataType: "json",
                url: ajaxUrl + locationQuery
            })
            .done(function (result) {
                showResult(result);
                console.log(result);
            });
        }
        
        // show result
        function showResult(data) {
            if (data.length > 0) {
                $(autocompleteWrapper).empty();
                for (i = 0; i < data.length; i++) {
                    $(autocompleteWrapper).append('<li data-code="' + data[i].Code + '"><span>' + data[i].Name + ' ,' + data[i].City + '</span></li>');
                }
                $(autocompleteWrapper).wrapInner('<ul></ul>');
                $(autocompleteWrapper).show().attr('data-active', 'true');
            } else {
                $(autocompleteWrapper).empty();
                $(autocompleteWrapper).append('<li class="text-center">Lokasi tidak ditemukan</li>');
                $(autocompleteWrapper).wrapInner('<ul></ul>');
                $(autocompleteWrapper).show();
            }
        }

        // select airport
        $(autocompleteWrapper).on('click', 'li', function () {
            $('.autocomplete-current').val($(this).text());
            $(elInput).blur();
            $(autocompleteWrapper).hide();
            $('.autocomplete-current').removeClass('autocomplete-current');
            var airportTarget = $(autocompleteWrapper).attr('data-airport-for');
            if (airportTarget == 'Ori') {
                $('.search-flight-form #flight-origin').val( $(this).attr('data-Code') );
            } else if (airportTarget == 'Dest') {
                $('.search-flight-form #flight-destination').val($(this).attr('data-Code'));
            }
        });

        // hide airport autocomplete
        $('html').click(function () {
            $(autocompleteWrapper).hide();
        });
        $(autocompleteWrapper).on('click', function (evt) {
            evt.stopPropagation();
        });
        

    }

    // ******************************
    // flight type
    function switchFlightType() {
        $('form.search-flight-form #return-flight').change(function () {
            var checked = $(this).prop('checked');
            var inputTarget = 'form.search-flight-form .flight-return-date';
            if (checked) {
                $(inputTarget).prop('disabled', false);
                $(inputTarget).attr('placeholder', 'return date');
                $(inputTarget).val('');
                $('.search-flight-form .flight-form-value#flight-type').val('RET');
            } else {
                $(inputTarget).prop('disabled', true);
                $(inputTarget).attr('placeholder', '');
                $(inputTarget).val('one way');
                $('.search-flight-form .flight-form-value#flight-type').val('ONE');
            }
        });
    }

    // ******************************
    // date picker
    var datePicker = function () {
        $('.flight-date.select-date').pickmeup_twitter_bootstrap({
            calendars: 3,
            format: 'Y-m-d',
            hide_on_select: true,
            select_month: false,
            select_year: false,
            separator: '-',
            min: new Date,
            change: function () {
                datePickerReturn($(this).pickmeup('get_date'));
                $('.search-flight-form .flight-form-value#flight-date').val($(this).val());
            }
        });
        
    }

    function datePickerReturn(theDate) {

        var selectedDate = theDate || new Date;

        $('.search-flight-form .flight-return-date.select-date').pickmeup_twitter_bootstrap('destroy');
        $('.search-flight-form .flight-return-date.select-date').pickmeup_twitter_bootstrap({
            calendars: 3,
            format: 'Y-m-d',
            hide_on_select: true,
            select_month: false,
            select_year: false,
            separator: '-',
            min: selectedDate,
            default_date: selectedDate,
            change: function () {
                $('.search-flight-form .flight-form-value#flight-return-date').val($(this).val());
            }
        });
        $('.search-flight-form .flight-return-date.select-date').pickmeup_twitter_bootstrap('update');

    }

}

//******************************************
// generate hotel star
function generate_star() {
    $('.generate-star').each(function () {
        var star_rating = $(this).attr('data-star');
        $(this).html('');
        for (var i = 1; i <= star_rating; i++) {
            $(this).append('<span class="fa fa-star></span>');
        }
        for (var i = 5; i > star_rating; i--) {
            $(this).append('<span class="fa fa-star-o></span>');
        }
    });
}

//******************************************
// news hover
$('.page.home-page section.news .news-wrapper').hover(
    function () {
        $(this).children('.news-link').children('.news-content').stop(true).animate({
            height: '100%',
            padding: '70px 30px 0'
        });
    }
    , function () {
        $(this).children('.news-link').children('.news-content').stop(true).animate({
            height: '0',
            padding: '0 30px'
        });
    }
);

//******************************************
// slider
$('.page.home-page section.slider .slide').each(function () {
    var slide_bg = $(this).children('.slide-bg').attr('src');
    var slide_bg_color = $(this).children('.slide-bg').attr('data-color');
    $(this).children('.slide-bg').remove();
    $(this).css({
        background: 'url(' + slide_bg + ') top center no-repeat ' + slide_bg_color,
        backgroundSize: 'cover'
    });
});

//******************************************
// toggle filter functions
function toggle_filter() {
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
            $('.hotel-detail-page .hotel-image .hotel-main-image').css('background-image', 'url('+selected_image+')');
        });
    }

}

//******************************************
// flight search functions
function flightSearch() {

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
// Show loading
function loading_overlay(state) {

    if (state == 'show') {

        if ( $('body').has('.loading-overlay').length == 0 ) {
            create_loading();
            $('body').children('.loading-overlay').show();
        } else {
            $('body').children('.loading-overlay').show();
        }

    } else if (state == 'hide') {
        $('body').children('.loading-overlay').hide();
    }

    // create loading screen
    function create_loading() {
        $('body').append('<div class="loading-overlay"> <div class="loading-content"><img src="/assets/images/loading-image.gif" /></div> </div>');
    }

}

// ************************
// angular app
// ************************
// variables
var SearchHotelConfig = {
    Url: 'http://travorama-apidev.azurewebsites.net/api/v1/hotels',
    ResultCount: 24
};

var SearchRoomConfig = {
    Url: 'http://travorama-apidev.azurewebsites.net/api/v1/rooms'
};

var FlightSearchConfig = {
    Url: 'http://travorama-apidev.azurewebsites.net/api/v1/flights',
    // Params: jQuery.parseJSON( $('.flight-search-page').attr('data-flight-search-params') )
};

// ************************
// Angular app
(function() {

    var app = angular.module('travorama', ['ngRoute']);

    // hotel controller
    app.controller('HotelController', [
        '$http', '$scope', function($http, $scope) {

            // run hotel search function on document ready
            angular.element(document).ready(function() {
                $scope.load_hotel_list();
            });

            // hotel list
            var hotel_list = this;
            hotel_list.hotels = [];

            // hotel search params
            $scope.HotelSearchParams = {};

            // *******************************
            // default value
            $scope.HotelSearchParams.StayDate = $('.search-page.hotel-search-page').attr('data-search-StayDate');
            $scope.HotelSearchParams.StayLength = $('.search-page.hotel-search-page').attr('data-search-StayLength');
            $scope.HotelSearchParams.LocationId = $('.search-page.hotel-search-page').attr('data-search-LocationId');
            $scope.HotelSearchParams.ResultCount = $('.search-page.hotel-search-page').attr('data-search-ResultCount');
            $scope.HotelSearchParams.RoomCount = $('.search-page.hotel-search-page').attr('data-search-RoomCount');
            // *******************************

            $scope.getStar = function(starRating) {
                return new Array(starRating);
            }

            $scope.getStarO = function (starRating) {
                starRating = 5 - starRating;
                return new Array(starRating);
            }

            // load hotel list function
            $scope.load_hotel_list = function(page) {

                loading_overlay('show');

                console.log('--------------------------------');
                console.log('Searching for hotel with params:');
                console.log($scope.HotelSearchParams);

                // set default page
                $scope.CurrentPage = page || 1;

                // set startIndex
                $scope.CurrentPage = $scope.CurrentPage - 1;
                $scope.HotelSearchParams.StartIndex = (SearchHotelConfig.ResultCount * $scope.CurrentPage);

                $scope.hotels = [];

                // generate StarRating
                $scope.HotelSearchParams.StarRating = [$scope.HotelSearchParams.star0, $scope.HotelSearchParams.star1, $scope.HotelSearchParams.star2, $scope.HotelSearchParams.star3, $scope.HotelSearchParams.star4, $scope.HotelSearchParams.star5].join('');
                if ($scope.HotelSearchParams.StarRating.length == 0) {
                    $scope.HotelSearchParams.StarRating = '-1,1,2,3,4,5';
                }

                // http request
                $http.get(SearchHotelConfig.Url, {
                    params: {
                        LocationId: $scope.HotelSearchParams.LocationId,
                        StayDate: $scope.HotelSearchParams.StayDate,
                        StayLength: $scope.HotelSearchParams.StayLength,
                        SearchId: $scope.HotelSearchParams.SearchId,
                        SortBy: $scope.HotelSearchParams.SortBy,
                        StartIndex: $scope.HotelSearchParams.StartIndex,
                        ResultCount: SearchHotelConfig.ResultCount,
                        MinPrice: $scope.HotelSearchParams.MinPrice,
                        MaxPrice: $scope.HotelSearchParams.MaxPrice,
                        StarRating: $scope.HotelSearchParams.StarRating
                    }
                    // if success
                }).success(function(data) {
                    // console data
                    console.log('Hotel search result:');
                    console.log(data);

                    // add data to $scope
                    hotel_list.hotels = data.HotelList;
                    $scope.HotelSearchParams.SearchId = data.SearchId;

                    // pagination
                    $scope.MaxPage = Math.ceil(data.TotalFilteredCount / SearchHotelConfig.ResultCount);
                    $scope.show_page($scope.MaxPage);

                    console.log('loaded');
                    console.log('--------------------------------');

                    loading_overlay('hide');

                    // if error
                }).error(function() {
                    console.log('REQUEST ERROR');
                    console.log('--------------------------------');

                    loading_overlay('hide');

                });

            };

            // paging function
            $scope.show_page = function(max_page) {
                $('footer .pagination').html('');
                for (n = 1; n <= max_page; n++) {
                    $('footer .pagination').append('<li><a href="#" data-page="' + n + '">' + n + '</a></li>');
                }
                $('footer .pagination a').click(function() {
                    var page = $(this).attr('data-page');
                    $scope.load_hotel_list(page);
                });
            }


        }
    ]);

    // room controller
    app.controller('RoomController', [
        '$http', '$scope', function($http, $scope) {

            // run hotel search function on document ready
            angular.element(document).ready(function() {
                $scope.getRoomlist();
            });

            // get room list
            var room_list = this;
            room_list.rooms = [];
            $scope.RoomSearchParams = {};
            $scope.loaded = false;

            // default value
            $scope.RoomSearchParams.HotelId = $('#form-room').attr('data-hotelId');
            $scope.RoomSearchParams.StayDate = $('#form-room-checkin').val() || '2015-05-05';
            $scope.RoomSearchParams.StayLength = $('#form-room-length').val() || 1;
            $scope.RoomSearchParams.RoomCount = $('#form-room-qty').val() || 1;
            $scope.RoomSearchParams.SearchId = '';

            $scope.getRoomlist = function() {

                console.log('--------------------------------');
                console.log('Searching for Room with params:');
                console.log($scope.RoomSearchParams);

                $scope.loaded = false;

                $http.get(SearchRoomConfig.Url, {
                    params: {
                        HotelId: $scope.RoomSearchParams.HotelId,
                        StayDate: $scope.RoomSearchParams.StayDate,
                        StayLength: $scope.RoomSearchParams.StayLength,
                        RoomCount: $scope.RoomSearchParams.RoomCount,
                        SearchId: $scope.RoomSearchParams.SearchId,
                    }
                }).success(function(data) {
                    console.log(data);

                    room_list.rooms = data.PackageList;

                    console.log('LOADED');
                    console.log('--------------------------------');

                    $scope.loaded = true;

                }).error(function() {
                    console.log('REQUEST ERROR');
                    console.log('--------------------------------');
                });

            }

        }
    ]);

    // fight controller
    app.controller('FlightController', [
        '$http', '$scope', function ($http, $scope) {

            FlightSearchConfig.params = jQuery.parseJSON($('.flight-search-page').attr('data-flight-search-params'));

            // run hotel search function on document ready
            angular.element(document).ready(function () {
                $scope.getFlightList();
            });

            // get room list
            var flightList = this;
            flightList.list = [];
            $scope.FlightSearchParams = {};
            $scope.loaded = false;

            // add class on click
            $scope.selectedItem = -1;
            $scope.clickedItem = function($index) {
                $scope.selectedItem = $index;
            }

            // default value
            $scope.FlightSearchParams = {
                Ori: FlightSearchConfig.Params.Ori,
                Dest: FlightSearchConfig.Params.Dest,
                Date: FlightSearchConfig.Params.Date,
                Cabin: 'e',
                Type: FlightSearchConfig.Params.Type,
                Adult: FlightSearchConfig.Params.Adult,
                Child: FlightSearchConfig.Params.Child,
                Infant: FlightSearchConfig.Params.Infant
            }

            $scope.getFlightList = function () {

                console.log('--------------------------------');
                console.log('Searching for Flights with params:');
                console.log($scope.FlightSearchParams);

                $scope.loaded = false;

                $http.get(FlightSearchConfig.Url, {
                    params: {
                        Ori: $scope.FlightSearchParams.Ori,
                        Dest: $scope.FlightSearchParams.Dest,
                        Date: $scope.FlightSearchParams.Date,
                        Cabin: $scope.FlightSearchParams.Cabin,
                        Type: $scope.FlightSearchParams.Type,
                        Adult: $scope.FlightSearchParams.Adult,
                        Child: $scope.FlightSearchParams.Child,
                        Infant: $scope.FlightSearchParams.Infant
                    }
                }).success(function (data) {
                    console.log(data);

                    flightList.list = data.FlightList;

                    console.log('LOADED');
                    console.log('--------------------------------');

                    $scope.loaded = true;

                }).error(function () {
                    console.log('REQUEST ERROR');
                    console.log('--------------------------------');
                });

            }

        }
    ]);


})();


