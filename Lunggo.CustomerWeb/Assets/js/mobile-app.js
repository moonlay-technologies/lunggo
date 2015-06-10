//------------------------------
// on document ready
$(document).ready(function() {
    siteHeaderFunctions();
    searchFormFunctions();
});

//------------------------------
// site header functions
function siteHeaderFunctions() {
    siteMenuFunctions();
    userMenuFunctions();
    //------------------------------
    // site menu functions
    function siteMenuFunctions() {
        $('.site-menu').click(function () {
            $('.user-menu').removeClass('active');
            $('.user-menu-content').hide();
            $(this).toggleClass('active');
            $('.site-menu-content').toggle();
        });
    }
    //------------------------------
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

//------------------------------
// home page functions
function homePageFunctions() {
    
    //------------------------------
    // detect form in selection
    $(document).ready(function() {
        if (window.location.hash) {
            if (window.location.hash == '#search-flight') {
                showSearchForm('#search-flight');
            } else if (window.location.hash == '#search-hotel') {
                showSearchForm('#search-hotel');
            }
        }
    });
    // switch to search form section
    $('.section-search a[data-trigger="showSearchForm"]').click(function() {
        showSearchForm( $(this).attr('href') );
    });
    //------------------------------
    // show and switch search form
    function showSearchForm(formSelection) {
        // hide selection section
        if ( $('.home-page .section-search').hasClass('active') ) {
            $('.home-page .section-search').slideUp().removeClass('active');
            $('.home-page .home-search').slideDown().addClass('active');
        }
        // show form section
        $('.home-search .home-search-header button.active').stop().removeClass('active');
        $('.home-search .home-search-content>.active').stop().removeClass('active');
        if (formSelection == '#search-hotel' || formSelection == 'hotel' || formSelection == 'searchHotel' || formSelection == 'search-hotel') {
            $('.home-search-header .search-hotel').addClass('active');
            $('.home-search .home-search-content>#search-hotel').addClass('active');
        } else if (formSelection == '#search-flight' || formSelection == 'flight' || formSelection == 'searchFlight' || formSelection == 'search-flight') {
            $('.home-search-header .search-flight').addClass('active');
            $('.home-search .home-search-content>#search-flight').addClass('active');
        }

    }
    // switch search form
    $('.home-search .home-search-header button').click(function (evt) {
        evt.preventDefault();
        showSearchForm( $(this).attr('data-target') );
    });

}

//------------------------------
// search form functions
function searchFormFunctions() {
    hotelFormFunctions();
    flightFormFunctions();
    // move form set to body
    $('.form-set').appendTo('body');

    $(document).keyup(function(e) {
        if (e.keyCode == 27) {
            $('.active .close-formset').click();
        }
    });

    // toggle form set
    function toggleFormSet(formSelection) {
        
        // toggle form set
        if ($('.form-set').hasClass('active')) {
            $('.form-set').removeClass('active');
            $('.form-set').stop().animate({
                top: '100%'
            });
        } else {
            $('.form-set').addClass('active');
            $('.form-set').stop().animate({
                top: 0
            });
        }
        // show requested form
        $('.form-set .form').removeClass('active');
        $('.form-set .form'+formSelection).addClass('active');

    }
    $('.close-formset').click(function () {
        toggleFormSet();
    });
    // form on click
    $('[data-trigger="show-formset"]').click(function() {
        toggleFormSet( $(this).attr('data-target') );
    });
    

    //------------------------------
    // hotel form functions
    function hotelFormFunctions() {
        
    }

    //------------------------------
    // flight form functions
    function flightFormFunctions() {
        
    }

}
