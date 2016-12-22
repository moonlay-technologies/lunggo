// home controller
app.controller('homeController', ['$scope', '$log', '$http', '$location', '$resource', '$timeout', 'hotelSearchSvc', function ($scope, $log, $http, $location, $resource, $timeout, hotelSearchSvc) {

   // ================= FLIGHT ========================

    $scope.departureDate = moment().add(1, 'day').format('DDMMYY');

    $scope.changeTab = function (tab) {
        if (tab == 'hotel') {
            $('.search-location').hide();
            $('.search-calendar').hide();
        } else if (tab == 'flight') {
            $scope.view.showHotelSearch = false;
            $('.search-calendar-hotel').hide();
            $('.form-child-age').hide();
        }
    }

    //=============== hotel start ======================
    hotelSearchSvc.initializeSearchForm($scope);

    $scope.hotel = {};
    $scope.view = {
        showHotelSearch : false
    }

    $scope.init = function (model) {
        $log.debug(model);
    }

    $scope.hotel.searchHotel = function () {
        hotelSearchSvc.gotoHotelSearch($scope.hotelSearch);
    };

    $scope.HotelSearchForm = {
        AutoComplete: {
            Keyword: '',
            MinLength: 3,
            GetLocation: function () {
                $scope.getLocation($scope.HotelSearchForm.AutoComplete.Keyword);
            },
        },
    }

    $('.form-hotel-location').click(function () {
        $(this).select();
    });


    //=============== hotel end ======================
}]);

//********************
// hotel form search function
jQuery(document).ready(function ($) {
    //Show hotel
    $('.form-hotel-location').click(function (evt) {
        evt.stopPropagation();
        $('.search-hotel').show();
        $('.search-calendar-hotel, .select-age .option, .form-hotel-night .option, .form-hotel-room .option, .form-child-age').hide();
    });

    //hideHotel hotel
    function hideHotel() {
        $('.search-hotel').hide();
    }

    //close hotel
    $('.close-hotel').click(function () { hideHotel(); });

    $('.search-hotel .location-recommend nav ul li ').click(function () {
        var showClass = $(this).attr('data-show');
        $(this).addClass('active');
        $(this).siblings().removeClass('active');
        $('.search-hotel .location-recommend .tab-content>div').removeClass('active');
        $('.search-hotel .location-recommend .tab-content>div.' + showClass).addClass('active');
    });

    //*****
    // show and hide search calendar
    function showCalendar() {
        $('.search-calendar-hotel').show();
    }

    function hideCalendar() {
        $('.search-calendar-hotel').hide();
    }
    $('.close-calendar-hotel').click(function () { hideCalendar(); });

    //*****
    // date selector
    $('.form-hotel-checkin, .form-hotel-checkout').click(function (evt) {
        $('.search-calendar-hotel').show();
        showCalendar();
        $('.hotel-date-picker').datepicker('option', 'minDate', new Date());
        evt.stopPropagation();
        $('.search-hotel').show();
        $('.search-hotel, .select-age .option, .form-hotel-night .option, .form-hotel-room .option, .form-child-age').hide();
    });

    // Select Age Children
    $('body .select-age').on('click', function (evt) {
        evt.stopPropagation();
        $(this).parent().siblings().children('div').children('.option').hide();
        $(this).children('.option').toggle();
        $('.search-calendar-hotel, .search-hotel, .form-hotel-visitor.child .option, .form-hotel-visitor.adult .option').hide();
    });

    $('body .form-hotel-night').on('click', function () {
        $(this).children('.option').toggle();
        $('.search-calendar-hotel, .search-hotel, .form-hotel-room .option, .form-child-age').hide();
    });

    $('body .form-hotel-room').on('click', function () {
        $(this).children('.option').toggle();
        $('.search-calendar-hotel, .search-hotel, .form-hotel-night .option').hide();
    });

    $('body .form-hotel-visitor.adult').on('click', function () {
        $(this).children('.option').toggle();
        $('.form-hotel-visitor.child .option, .select-age .option').hide();
    });

    $('body .form-hotel-visitor.child').on('click', function () {
        $(this).children('.option').toggle();
        $('.form-hotel-visitor.adult .option, .select-age .option').hide();
    });

    $('body .form-child-age').hide();
    $('body .form-hotel-room span').on('click', function () {
        $('body .form-child-age').show();
    });

    $('body input[name="FormAgeSubmit"]').on('click', function () {
        $('body .form-child-age').hide();
    });

    //Home Page Search Form Hotel
    $('body .menu-main li').click(function () {
        if ($(this).is('#header-flight')) {
            var itemF = $(this).closest('.site-header').parent();

            itemF.parent().find('.tab-header').find('.flight').addClass('active');
            itemF.parent().find('.tab-header').find('.flight').siblings().removeClass('active');
            itemF.parent().find('#plane').addClass('active');
            itemF.parent().find('#plane').siblings().removeClass('active');

            var linkF = $(this).find('a').attr('href', '#plane');

            linkF.parent().addClass('active');
            linkF.parent().siblings().removeClass('active');

        } else if ($(this).is('#header-hotel')) {
            var item = $(this).closest('.site-header').parent();

            item.parent().find('.tab-header').find('.hotel').addClass('active');
            item.parent().find('.tab-header').find('.hotel').siblings().removeClass('active');
            item.parent().find('#hotel').addClass('active');
            item.parent().find('#hotel').siblings().removeClass('active');

            var link = $(this).find('a').attr('href', '#hotel');

            link.parent().addClass('active');
            link.parent().siblings().removeClass('active');

        }
    });

    $('body .tab-header li').click(function () {
        if ($(this).hasClass('flight')) {
            $(this).closest('.site-content').parent().find('.menu-main').find('#header-flight').addClass('active');
            $(this).closest('.site-content').parent().find('.menu-main').find('#header-flight').siblings().removeClass('active');
        } else if ($(this).hasClass('hotel')) {
            $(this).closest('.site-content').parent().find('.menu-main').find('#header-hotel').addClass('active');
            $(this).closest('.site-content').parent().find('.menu-main').find('#header-hotel').siblings().removeClass('active');
        }
    });

    // Slider Home Page Desktop
    $('.page-slider .slider a').each(function () {
        var bgImage = $(this).children('img').attr('src');
        $(this).css({
            'background-image': 'url(' + bgImage + ')'
        });
        $(this).children('img').remove();
    });
    $('.slider-wrapper').slick({
        autoplay: true,
        autoplaySpeed: 4000,
        dots: true,
        prevArrow: '<button type="button" class="slick-prev hidden">Back</button>',
        nextArrow: '<button type="button" class="slick-next hidden">Next</button>'
    });

    // Slider Home Page Mobile
    $('.carousel-inner').slick({
        autoplay: true,
        autoplaySpeed: 2800,
        dots: true,
        prevArrow: '<button type="button" class="slick-prev hidden">Back</button>',
        nextArrow: '<button type="button" class="slick-next hidden">Next</button>'
    });
});