app.controller('HotelController', ['$http', '$scope', '$rootScope', '$log', function ($http, $scope, $rootScope, $log) {

    // **********
    // variables
    $scope.PageConfig = $rootScope.PageConfig;
    $scope.PageConfig.Loaded = true;

    // set overlay
    $scope.SetOverlay = function (overlay) {
        $log.debug('changing overlay to : ' + overlay);
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
            var id = $(this).attr('attr-link');
            $('body .detail-tab').hide();
            $(id).show();
        });

        // **********
        // Custom Checkbox
        $('body .checkbox').on('click', function () {
            var id = $(this).find('.check');
            if ($(id).is(':checked')) {
                id.checked = true;
                $(this).addClass('active');
            } else {
                id.checked = false;
                $(this).removeClass('active');
            }
        });

        // **********
        // Custom Radio
        $('body .round').on('click', function () {
            var id = $(this).find('.check-radio');
            $('body .round').checked = false;
            $('body .round').removeClass('active');
            id.checked = true;
            $(this).addClass('active');
        });

        // **********
        // Reset
        $('body #reset').on('click', function () {
            $('body #filter-hotel input').each(function (index, item) {
                var type = $(item).attr('type');
                var id = $(this).parent();
                if (type == 'radio' || type == 'checkbox') {
                    $(item).attr('checked', false);
                    id.removeClass('active');
                } else {
                    $(item).val('');
                }
            });
        });

        // **********
        // Open Detail Room
        $('body .dh-list').on('click', function () {
            var id = $(this).parent().find('.dh-list-detail');
            id.toggleClass('active');
            $(this).toggleClass('active');
        });
        
        //// **********
        //// Slick Slider Detail Hotel
        $('.dh-slider').slick({
            autoplay: true,
            autoplaySpeed: 2500,
            dots: false
        });

    });
}]);