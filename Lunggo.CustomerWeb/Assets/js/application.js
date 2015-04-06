//******************************************
// on document ready
$(document).ready(function(){

	toggle_filter();
	hotel_search();
	hotel_detail();
	flight_search();

    hotel_search_form_functions();

    //$('.input-checkin.select-date').pickmeup_twitter_bootstrap({
    //   calendars: 3,
    //    format: 'Y/m/d',
    //    hide_on_select: true,
    //    select_month: false,
    //    select_year: false,
    //    separator: '/',
    //    min: new Date,
    //    change: function () {
    //        date_picker_checkout( $(this).pickmeup('get_date') )
    //    }
    //});

});

//******************************************
// FUNCTIONS

//******************************************
// hotel search form
function hotel_search_form_functions() {

    // activate functions
    $(document).ready(function() {
        location_autocomplete();
        date_picker();
        stay_length();
        room_count();
    });

    // ******************************
    // location auto complete
    var location_autocomplete = function () {

        // setting
        var el_input = 'input.input-location';
        var ajax_url = 'http://travorama-apidev.azurewebsites.net/api/v1/autocomplete/hotellocation/';
        var min_char = 3;
        var list_wrapper = '.loc-auto';

        // run function on keyup
        $(el_input).keyup(function () {
            var req_val = $(this).val();
            verify_input(req_val);
        });

        $(el_input).focus(function () {
            var req_val = $(this).val();
            verify_input(req_val);
        });

        // function verified input
        function verify_input(input_val) {
            if (input_val.length >= min_char) {
                $(list_wrapper).empty();
                $(list_wrapper).append('<li class="text-center"> Loading </li>');
                $(list_wrapper).wrapInner('<ul></ul>');
                $(list_wrapper).show();
                get_result(input_val);
            } else {
                $(list_wrapper).hide();
            }
        }

        // get result
        function get_result(input_val) {
            $.ajax({
                method: "GET",
                dataType: "json",
                url: ajax_url + input_val
            })
            .done(function (result) {
                show_result(result);
            });
        }

        // show result
        function show_result(data) {
            if (data.length > 0) {
                $(list_wrapper).empty();
                for (i = 0; i < data.length; i++) {
                    $(list_wrapper).append('<li data-location-id="' + data[i].LocationId + '"><span>' + data[i].LocationName + ',' + data[i].RegionName + ',' + data[i].CountryName + '</span></li>');
                }
                $(list_wrapper).wrapInner('<ul></ul>');
                $(list_wrapper).show().attr('data-active', 'true');
            } else {
                $(list_wrapper).empty();
                $(list_wrapper).append('<li class="text-center">Lokasi tidak ditemukan</li>');
                $(list_wrapper).wrapInner('<ul></ul>');
                $(list_wrapper).show();
            }
        }

        // on click
        select_result();
        function select_result() {
            $(list_wrapper).on('click', 'li', function () {
                $(el_input).val($(this).text());
                $(list_wrapper).hide();
                $(el_input).attr('data-location-id', $(this).attr('data-location-id'));
                $('.search-hotel-form .search-hotel-value.location').val($(this).attr('data-location-id'));
            });
        }

        // hide when click another element
        $('html').click(function () {
            $(list_wrapper).hide();
        });
        $("input.input-location , .loc-auto").on('click', function (evt) {
            evt.stopPropagation();
        });



    };

    // ******************************
    // date picker
    var date_picker = function() {
        $('.input-checkin.select-date').pickmeup_twitter_bootstrap({
           calendars: 3,
            format: 'Y-m-d',
            hide_on_select: true,
            select_month: false,
            select_year: false,
            separator: '-',
            min: new Date,
            change: function () {
                date_picker_checkout($(this).pickmeup('get_date'));
                $('.search-hotel-value.staydate').val( $(this).val() );
            }
        });
    }

    function date_picker_checkout(the_date) {

        var selected_date = the_date || new Date;

        $('.input-checkout.select-date').pickmeup_twitter_bootstrap('destroy');
        $('.input-checkout.select-date').pickmeup_twitter_bootstrap({
            calendars: 3,
            format: 'Y-m-d',
            hide_on_select: true,
            select_month: false,
            select_year: false,
            separator: '-',
            min: selected_date,
            default_date: selected_date,
            change: function() {
                calculate_date( $(this).pickmeup('get_date') );
            }
        });
        $('.input-checkout.select-date').pickmeup_twitter_bootstrap('update');

    }

    function calculate_date(checkout_date) {
        var checkin_date = new Date($('.search-hotel-value.staydate').val());
        var checkout_date = new Date( checkout_date );
        var stay_length = Math.abs(checkout_date - checkin_date);
        var stay_length_value = Math.ceil(stay_length/ (1000 * 3600 * 24));
        $('.search-hotel-value.staylength').val(stay_length_value );
    }

    // ******************************
    // stay length
    var stay_length = function() {
        
    }

    // ******************************
    // room count
    var room_count = function (room) {
        var max_room = room || 4;
        var room_option = '.input-room.select-room';

        // show
        $(room_option).focus(function () {
            if ($(this).siblings('div.option').length == 0) {
                $(this).parent().addClass('option-wrapper').append('<div class="option"></div>');
                for (var i = 1; i <= max_room; i++) {
                    var room;
                    if (i > 1) {
                        room = ' rooms'
                    } else {
                        room = ' room'
                    }
                    $(this).siblings('div.option').append('<li data-value="' + i + '">' + i + room + ' </li>');
                }
                $(this).siblings('.option').wrapInner('<ul></ul>');
            }
            $(room_option).siblings('div.option').show();
        });


        $(room_option).on('keydown', function(evt) {
            var key_value = evt.which;
            var room_value;
            if (key_value >= 49 && key_value <= 57) {
                switch (key_value) {
                    case 49:
                        room_value = 1;
                        break;
                    case 50:
                        room_value = 2;
                        break;
                    case 51:
                        room_value = 3;
                        break;
                    case 52:
                        room_value = 4;
                        break;
                    default:
                        room_value = 4;
                        break;
                }
                console.log('JEMPING');
                $(this).siblings('.option').children('ul').children('li:nth-child('+room_value+')').click();
                return false;
            } else {
                return false;
            }
        });

        // select room
        $('.form-group').on('click', '.option li', function () {
            $(room_option).val($(this).html());
            $(room_option).attr('data-current-room', $(this).attr('data-value'));
            $(room_option).blur();
        });


        // hide room option
        $('html').click(function () {
            $('.option-wrapper .option').hide();
        });
        $(".input-room.select-room").on('click', function (evt) {
            evt.stopPropagation();
        });

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
function hotel_search() {

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
function hotel_detail() {

    hotel_image();

    // hotel image
    function hotel_image() {
        var main_image = '.hotel-detail-page .hotel-image .hotel-main-image img';
        $('.hotel-detail-page .hotel-image .hotel-thumb a').click(function (evt) {
            evt.preventDefault();
            var selected_image = $(this).children('img').attr('data-image-url');
            $('.hotel-detail-page .hotel-image .hotel-main-image img').attr('src', selected_image);
        });
    }

}

//******************************************
// flight search functions
function flight_search() {

    toggle_flight_detail();

    // toggle flight detail
    function toggle_flight_detail() {
        $('.flight-search-page .flight-list .flight .flight-toggle, .flight-search-page .flight-list-return .flight .flight-toggle').click(function (evt) {
            evt.preventDefault();
            $(this).closest('.flight').children('.flight-detail').stop().slideToggle();
            $(this).toggleClass('active');
            $(this).closest('.flight').toggleClass('active');
        });
    }

}

//******************************************
// Show loading
function loading_overlay(state, loc) {

    if (state == 'show') {

        if ( $(loc).has('.loading-overlay').length == 0 ) {
            create_loading();
            $(loc).children('.loading-overlay').show();
        } else {
            $(loc).children('.loading-overlay').show();
        }

    } else if (state == 'hide') {
        $(loc).children('.loading-overlay').hide();
    }

    // create loading screen
    function create_loading() {
        $(loc).css('position','relative');
        $(loc).append('<div class="loading-overlay"> <div class="loading-content"><img src="/assets/images/loading-icon.png" /></div> </div>');
    }

}

// ************************
// angular app
// ************************
// variables
var SearchHotelConfig = {
    Url: 'http://travorama-apidev.azurewebsites.net/api/v1/hotels/',
    ResultCount: 24
};


// ************************
// Angular app
(function () {

    var app = angular.module('travorama', ['ngRoute']);

    // hotel controller
    app.controller('HotelController', ['$http', '$scope', function ($http, $scope) {

        // run hotel search function on document ready
        angular.element(document).ready(function () {
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

        // load hotel list function
        $scope.load_hotel_list = function (page) {

            loading_overlay('show','body');

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
            }).success(function (data) {
                // console data
                console.log('Hotel search result:');
                console.log(data);

                //remove notif
                $('.notif').remove();

                // add data to $scope
                hotel_list.hotels = data.HotelList;
                $scope.HotelSearchParams.SearchId = data.SearchId;

                // pagination
                $scope.MaxPage = Math.ceil(data.TotalFilteredCount / SearchHotelConfig.ResultCount);
                $scope.show_page($scope.MaxPage);

                generate_star();

                console.log('loaded');
                console.log('--------------------------------');

                loading_overlay('hide', 'body');

                $scope.postProcess();

                // if error
            }).error(function () {
                console.log('REQUEST ERROR');

                loading_overlay('hide','body');

            });

        };

        // paging function
        $scope.show_page = function (max_page) {
            $('footer .pagination').html('');
            for (n = 1 ; n <= max_page ; n++) {
                $('footer .pagination').append('<li><a href="#" data-page="' + n + '">' + n + '</a></li>');
            }
            $('footer .pagination a').click(function () {
                var page = $(this).attr('data-page');
                $scope.load_hotel_list(page);
            });
        }

        // post process
        $scope.postProcess = function () {
            $('.hotel-wrapper .hotel').each(function() {
                console.log('JEMPING POST PROCESS');
            });
        }


    }]);

})();


