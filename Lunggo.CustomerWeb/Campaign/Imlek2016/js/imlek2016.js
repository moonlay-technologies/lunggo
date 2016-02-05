// variables
var Imlek = {};

// email
Imlek.Email = '';
Imlek.Url = '';
Imlek.Eligible = true;

// start or end the overlay
Imlek.State = function(state) {

    if (state == 'start') {
        $('.imlek-wrapper').fadeIn();
        $('body').addClass('no-scroll');
    } else if (state == 'end') {
        $('.imlek-wrapper').fadeOut();
        $('body').removeClass('no-scroll');
    } else if (state == 'init') {
        $('.imlek-wrapper .tree').css('opacity', '1');
        $('.imlek-wrapper .email').fadeOut();
    }

};

// Add email
Imlek.AddEmail = function (emailAddress) {
    Imlek.Email = emailAddress;
    Imlek.State('init');
}

// submit email
$('#imlek__email__submit').click(function () {

    var email = $('#imlek__email__address').val();
    var re = /[A-Z0-9._%+-]+@[A-Z0-9.-]+.[A-Z]{2,4}/igm;

    if ( email == '' || !re.test( email ) ) {
        $(this).parent().addClass('has-error');
        $(this).siblings('.help-block').show();
    } else {
        Imlek.Url = $(this).attr('data-url');
        Imlek.AddEmail(email);
    }

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

    // console.log(Imlek);

    if (Imlek.Email.length == 0) {
        return false;
    }

    if ($(this).hasClass('active')) {
        return false;
    }

    // API call
    $.post((Imlek.Url + '/v1/promo/imlek' ), { email: Imlek.Email })
    .done(function (data) {

        // text target
        var textTarget = $('.imlek-wrapper .card.active').children('.prize__wrapper').children('.prize__body');

        var text = {};
        text.noPrize = '<h3><b>Silahkan Coba Lagi</b></h3><p>Maaf, Anda belum beruntung. Silahkan coba lagi.</p>';
        text.noEligible = '<h3><b>Sudah Habis</b></h3><p>Kesempatan Anda hari ini sudah habis. Silahkan coba lagi esok hari.</p>';

        if (data.c < 0) {
            textTarget.addClass('no-eligible');
            textTarget.html(text.noEligible);
            Imlek.Eligible = false;
        } else if (data.c == 0) {
            textTarget.addClass('no-prize');
            textTarget.html(text.noPrize);
        } else if (data.c > 0) {
            textTarget.addClass('prize');
            //data.v = "TRAVOR4M4";
            switch (data.c) {
                case 1:
                    textTarget.addClass('prize-50');
                    break;
                case 2:
                    textTarget.addClass('prize-100');
                    break;
                case 3:
                    textTarget.addClass('prize-150');
                    break;
                case 4:
                    textTarget.addClass('prize-200');
                    break;
            }
            textTarget.html('<p class="text-info">Masukkan kode voucher ini ketika melakukan pembayaran.</p><p class="voucher-code">'+data.v+'</p>');
        }

    });

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
    if (!Imlek.Eligible) {
        Imlek.State('end');
    }
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


