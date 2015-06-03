// on document ready
$(document).ready(function() {
    siteHeaderFunctions();
});

// site header functions
function siteHeaderFunctions() {
    siteMenuFunctions();
    userMenuFunctions();

    // site menu functions
    function siteMenuFunctions() {
        $('.site-menu').click(function () {
            $('.user-menu').removeClass('active');
            $('.user-menu-content').hide();
            $(this).toggleClass('active');
            $('.site-menu-content').toggle();
        });
    }

    // user menu functions
    function userMenuFunctions() {
        $('.user-menu').click(function () {
            $('.site-menu').removeClass('active');
            $('.site-menu-content').hide();
            $(this).toggleClass('active');
            $('.user-menu-content').toggle();
        });
    }

}

