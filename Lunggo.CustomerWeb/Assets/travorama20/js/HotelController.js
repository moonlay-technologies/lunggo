jQuery(document).ready(function ($) {
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
    });
});

function customCheckbox() {
    // **********
    // Custom Checkbox
//    $('body .sqr').on('click', function () {
//        var id = $(this).find('.checkbox');
//        if ($(id).is(':checked')) {
//            id.checked = true;
//            $(this).addClass('active');
//        } else {
//            id.checked = false;
//            $(this).removeClass('active');
//        }
//});
}