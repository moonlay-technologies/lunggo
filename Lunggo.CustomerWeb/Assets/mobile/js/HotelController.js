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
            var id = $(this).attr('attr-link');
            $('body .detail-tab').hide();
            $(id).show();
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

        //// **********
        //// Custom Checkbox
        //$('body .switch').on('click touchstart', function () {
        //    var id = $(this).parent().find('.check');
        //    if ($(id).is(':checked')) {
        //        id.checked = false;
        //        $(this).removeClass('active');
        //    } else {
        //        id.checked = true;
        //        $(this).addClass('active');
        //    }
        //});

        //// **********
        //// Custom Radio
        //$('body .radio').on('click touchstart', function () {
        //    var id = $(this).parent().find('.check-radio');
        //    $('body .radio').checked = false;
        //    $('body .radio').removeClass('active');
        //    id.checked = true;
        //    $(this).addClass('active');
        //});
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

        // **********
        // Counter Detail Room
        $('body .btn-min').on('click', function(){
            var value = $(this).parent().find('.result-room').val();
            value = calculate(value, 'min');

            $(this).parent().find('.result-room').val(value);
        });

        $('body .btn-plus').on('click', function(){
            var value = $(this).parent().find('.result-room').val();
            var limit = $(this).attr('attr-limit');
            value = calculate(value, 'plus', limit);

            $(this).parent().find('.result-room').val(value);
    });

        function calculate(value, type, limit){
            value = parseInt(value);
            limit = parseInt(limit);

            if(value < 0){
                value = 0;
            }

            if (type == 'min') {
                if(value > 0){
                    value = value - 1;
                }
            } else if (type == 'plus') {
                value = value + 1;

                if(value > limit){
                    value = limit;
                }
    }

}]);