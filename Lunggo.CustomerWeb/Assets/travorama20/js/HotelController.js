jQuery(document).ready(function ($) {
    $('body .checkbox').on('click touchstart', function () {
        var id = $(this).parent().find('.check');
        if ($(id).is(':checked')) {
            id.checked = false;
            $(this).removeClass('active');
        } else {
            id.checked = true;
            $(this).addClass('active');
        }
    });

    $('body .radio').on('click touchstart', function () {
        var id = $(this).parent().find('.check-radio');
        $('body .radio').checked = false;
        $('body .radio').removeClass('active');
        id.checked = true;
        $(this).addClass('active');
    });

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
    });

    // Dropdown room
    $('body .change-room').on('click', function () {
        $(this).toggleClass('active');
    });

    // Big Slider Detail Hotel
    $(window).load(function () {
        $('.flexslider').flexslider({
            animation: "slide",
            controlNav: "thumbnails",
            start: function (slider) {
                $('body').removeClass('loading');
            }
        });
    });

    // Open Room Detail
    $('body .info-room').on('click', function () {
        var parent1 = $(this).closest('.room-list').find('.room-left');
        var parent2 = parent1.closest('li').find('.hotel-detail');
        parent2.toggleClass('active');

        // Slick Slider Detail Hotel
        $('body .hd-slider').slick({
            autoplay: false,
            autoplaySpeed: 2500,
            dots: false
        });
    });
});