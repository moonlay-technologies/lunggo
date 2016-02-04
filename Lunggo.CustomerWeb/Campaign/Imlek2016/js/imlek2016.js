// variables
var Imlek = {};

// email
Imlek.Email = '';

// start or end the overlay
Imlek.State = function(state) {

    if (state == 'start') {
        $('.imlek-wrapper').fadeIn();
        $('body').addClass('no-scroll');
    } else if (state == 'end') {
        $('.imlek-wrapper').fadeOut();
        $('body').removeClass('no-scroll');
    } else if (state == 'init') {
        $('.imlek-wrapper .tree').fadeIn();
        $('.imlek-wrapper .email').fadeOut();
    }

};

// Add email
Imlek.AddEmail = function(emailAddress) {
    Imlek.Email = emailAddress;
}

// submit email
$('#imlek__email__submit').click(function() {
    Imlek.AddEmail($('#imlek__email__address').val());
    Imlek.State('init');
});

// card on hover
$('.imlek-wrapper .card').hover(function() {

    // on mouse in

}, function() {
    
    // on mouse out

});

// card on click
$('.imlek-wrapper .card').click(function(evt) {
    evt.preventDefault();

    if ($(this).hasClass('active')) {
        return false;
    }

    // add active class
    $(this).addClass('active');

    var speed = 500;

    $('.imlek-wrapper .tree').children().hide();
    $(this).show();

    // animate
    $(this).animate({
        left: '50%',
        width: '250px',
        marginLeft: '-125px',
        zIndex: 9999,
        top: 0
    }, speed, animateSlide(this));

    // animate slide
    function animateSlide(target) {

        $(target).find('.prize__wrapper').show();
        $(target).children('.envelope').delay(speed).animate({
            top: 150
        },500);
    }

});

$('.imlek-wrapper .card .prize__close').click(function(evt) {
    $(this).closest('.card').fadeOut(700, function() { $(this).remove(); });
    $('.imlek-wrapper .tree').children().show();
});

// body no scroll
$(document).ready(function() {
    Imlek.State('start');
});

// imlek close
$('.imlek-wrapper__close').click(function() {
    Imlek.State('end');
});


// monkey animation
Imlek.MonkeyAnimate = function(){
    $('.imlek-wrapper .monkey').css({
        transformOrigin: '50% 0'
    }).transition({
        rotate: '15deg'
    }, 1200, 'linear').transition({
        rotate: '-15deg'
    }, 1200, 'linear', Imlek.MonkeyAnimate);
};
Imlek.MonkeyAnimate();


