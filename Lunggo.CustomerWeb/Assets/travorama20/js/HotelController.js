jQuery(document).ready(function ($) {
        
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