// variables
var Imlek = {};


// card animation
Imlek.CardAnim = function(state) {

    $('.imlek-wrapper .card').each(function () {

        var max = 1500;
        var min = 500;
        var speed = (Math.random() * (max - min)) + min;

        $(this).css({
            transformOrigin: '50% 0'
        }).transition({
            rotate: '15deg'
        }, speed, 'linear').transition({
            rotate: '-15deg'
        }, speed, 'linear');
    });

};

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
    $(this).closest('.card').fadeOut().remove();
    $('.imlek-wrapper .tree').children().show();
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


