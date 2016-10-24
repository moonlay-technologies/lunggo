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
        if ($(id).is(':checked')) {
            id.checked = false;
            $(this).removeClass('active');
        } else {
            id.checked = true;
            $(this).addClass('active');
        }
    });

    $("body .img-list").each(function (i, elem) {
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