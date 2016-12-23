jQuery(document).ready(function ($) {
        $('.hotel-date-picker').datepicker({
            numberOfMonths: 2,
            onSelect: function (date) {
                date = date.substring(3, 5) + "/" + date.substring(0, 2) + "/" + date.substring(6, 10);
                //console.log(data);
                //$scope.setCheckinDate(data);

                var scope = angular.element($('.hotel-date-picker')).scope();
                $scope.setCheckinDate(scope, date);


                $log.debug("checkinDate = " + date);
                var target;
                var chosenDate = new Date(date);
                $(target + ' .date').html(('0' + chosenDate.getDate()).slice(-2));
                $(target + ' .month').html(translateMonth(chosenDate.getMonth()));
                $(target + ' .year').html(chosenDate.getFullYear());
                $('.search-calendar-hotel').hide();
                var cd = new Date(date);
                var checkoutDate = new Date(cd.setDate(cd.getDate() + $scope.hotel.nightCount));
                var dd = checkoutDate.getDate();
                var mm = checkoutDate.getMonth() + 1;
                var yyyy = checkoutDate.getFullYear();
                var d = yyyy + '-' + mm + '-' + dd;
                $scope.hotel.checkoutDate = moment(checkoutDate, "MM-DD-YYYY");
                $log.debug("checkout date = " + $scope.hotel.checkoutDate);
            }
        });
    //// **********
    //// Custom Checkbox
    //$('body .sqr').on('click', function () {
    //    var id = $(this).find('.check');
    //    if ($(id).is(':checked')) {
    //        id.checked = true;
    //        $(this).addClass('active');
    //    } else {
    //        id.checked = false;
    //        $(this).removeClass('active');
    //    }
    //});

    // **********
    // Custom Radio
    //$('body .round').on('click', function () {
    //    var id = $(this).find('.check-radio');
    //    $('body .round').checked = false;
    //    $('body .round').removeClass('active');
    //    id.checked = true;
    //    $(this).addClass('active');
    //});

    // **********
    // Search List Image
    $(function () {
        $("body .col-left-hotel .img-list").each(function (i, elem) {
        var img = $(elem);
        var div = $("<div />").css({
            background: "url(" + img.attr("src") + ") no-repeat",
            width: "143px",
            height: "180px",
            "background-size": "cover",
            "background-position": "center"
    });
        img.replaceWith(div);
    });
    });

    // Ubah Pencarian Hotel
    $('body .search-result-form-trigger').on('click', function () {
        $('body .hotel-form').slideToggle("slow");
        $('.search-hotel, .search-calendar-hotel').hide();

    });
});

function customCheckbox() {
    // **********
    // Custom Checkbox
    $('body .sqr').on('click', function () {
        var id = $(this).find('.check');
        if ($(id).is(':checked')) {
            id.checked = true;
            $(this).addClass('active');
        } else {
            id.checked = false;
            $(this).removeClass('active');
        }
});
}



//********************
// hotel form search function
jQuery(document).ready(function ($) {
    //Show hotel
    $('.change-hotel').click(function (evt) {
        evt.stopPropagation();
        $('.search-hotel').show();
        $('.search-calendar-hotel, .select-age .option').hide();
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

    $('.form-date').click(function() {
        $('.search-calendar-hotel').show();
    });

    function showCalendar() {
        $('.search-calendar-hotel').show();
    }

    function hideCalendar() {
        $('.search-calendar-hotel').hide();
}
    $('.close-calendar-hotel').click(function () { hideCalendar(); });

    //*****
    // date selector
    $('.form-date').click(function (evt) {
        $('.search-calendar-hotel').show();
        showCalendar();
        $('.hotel-date-picker').datepicker('option', 'minDate', new Date());
        evt.stopPropagation();
        $('.search-hotel').show();
        $('.search-hotel, .select-age .option, .form-room .option, .form-night .option, .form-adult .option, .form-child .option').hide();
    });

    // Select Age Childeren
    $('body .select-age').on('click', function (evt) {
        evt.stopPropagation();
        $(this).parent().siblings().children('div').children('.option').hide();
        $(this).children('.option').toggle();
        $('.search-calendar-hotel, .search-hotel, .form-room .option, .form-night .option, .form-adult .option, .form-child .option').hide();
    });

    $('body .form-night').on('click', function () {
        $(this).children('.option').toggle();
        $('.search-calendar-hotel, .search-hotel, .form-room .option, .select-age .option, .form-adult .option, .form-child .option').hide();
    });
   
    $('body .form-adult').on('click', function () {
        $(this).children('.option').toggle();
        $('.search-calendar-hotel, .search-hotel, .form-room .option, .form-night .option, .select-age .option, .form-child .option').hide();
    });

    $('body .form-child').on('click', function () {
        $(this).children('.option').toggle();
        $('.search-calendar-hotel, .search-hotel, .form-room .option, .form-night .option, .select-age .option, .form-adult .option').hide();
    });

    $('body .form-room').on('click', function () {
        $(this).children('.option').toggle();
        $('.search-calendar-hotel, .search-hotel, .form-night .option, .select-age .option, .form-adult .option, .form-child .option').hide();
    });

    $('body .room-row').hide();
    $('body .form-room span').on('click', function () {
        $('body .room-row').show();
    });

});