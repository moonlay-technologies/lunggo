app.controller('HotelController', ['$http', '$scope', '$rootScope', function ($http, $scope, $rootScope) {


    // **********
    // variables
    $scope.PageConfig = $rootScope.PageConfig;
    $scope.PageConfig.Loaded = true;

    // set overlay
    $scope.SetOverlay = function (overlay) {
        console.log('changing overlay to : ' + overlay);
        if (!overlay) {
            $scope.PageConfig.ActiveOverlay = '';
            $scope.PageConfig.BodyNoScroll = false;
        } else {
            $scope.PageConfig.ActiveOverlay = overlay;
            $scope.PageConfig.BodyNoScroll = true;
        }
    }

    jQuery(document).ready(function ($) {
        // **********
        // Search Detail Tab
        $('body .detail-tab').each(function (index, item) {
            if (index > 0) {
                $(item).hide();
            }
        });
        $('body .hotel-detail-menu-action a').on('click touchstart', function () {
            var id = $(this).attr('href');
            $('body .detail-tab').hide();
            $(id).show();
            $(id).scrollTop(0);
        });

        // **********
        // Shorten Area

        // Search Detail
        $('body .sh-desc a').on('click touchstart', function () {
            $('body .sh-desc a').toggleClass('active');
            $('body .sh-txt').toggleClass('opened');
        });
        // Filter
        $('.overlay .filter-group--facility a').on('click touchstart', function () {
            $('.overlay .filter-group--facility a').toggleClass('active');
            $('.overlay .sh-list').toggleClass('opened');
        });

        // **********
        // Custom Checkbox
        $('body .switch').on('click touchstart', function () {
            var id = $(this).parent().find('.check');
            if ($(id).is(':checked')) {
                id.checked = false;
                $(this).removeClass('active');
            } else {
                id.checked = true;
                $(this).addClass('active');
            }
        });

        // **********
        // Custom Radio
        $('body .radio').on('click touchstart', function () {
            var id = $(this).parent().find('.check-radio');
            $('body .radio').checked = false;
            $('body .radio').removeClass('active');
            id.checked = true;
            $(this).addClass('active');
        });

        // **********
        // Slider
        $("#slider-range").slider({
            range: true,
            min: 0,
            max: 500,
            values: [75, 300],
            slide: function (event, ui) {
                $("#amount").val("$" + ui.values[0] + " - $" + ui.values[1]);
            }
        });
        $("#amount").val("$" + $("#slider-range").slider("values", 0) +
            " - $" + $("#slider-range").slider("values", 1));
    });
}]);