/* Functions for Subscription Form (Email Newsletter) */

SubscribeConfig.email = '';
SubscribeConfig.name = '';


/* validate email with regex
 * return true or false
 */
function validateEmail (email) {
    var re = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;
    return re.test(email);
}


$('form.subscribe-form input[type="submit"]').click(function (evt) {
    evt.preventDefault();
    validateForm();
});

function validateForm() {
    $('form.subscribe-form input[type="submit"]').prop('disabled', true);
    $('form.subscribe-form input[type="submit"]').val('LOADING');
    SubscribeConfig.email = $('form.subscribe-form input.subscribe-email').val();
    SubscribeConfig.name = $('form.subscribe-form input.subscribe-name').val();

    if ($('form.subscribe-form input.subscribe-email').val()) {
        var emailValue = $('form.subscribe-form input.subscribe-email').val();
        if (validateEmail(emailValue)) {
            SubscribeConfig.email = emailValue;
        } else {
            SubscribeConfig.email = '';
            alert('Alamat email tidak valid');
        }
    } else {
        $('form.subscribe-form input.subscribe-email').attr('placeholder', 'Mohon masukan Alamat Email Anda');
        $('form.subscribe-form input.subscribe-email').parent().addClass('has-error');
    }

    if ($('form.subscribe-form input.subscribe-name').val()) {
        SubscribeConfig.name = $('form.subscribe-form input.subscribe-name').val();
    } else {
        $('form.subscribe-form input.subscribe-name').attr('placeholder', 'Mohon masukan Nama Anda');
        $('form.subscribe-form input.subscribe-name').parent().addClass('has-error');
    }

    if (SubscribeConfig.name && SubscribeConfig.email) {
        submitForm();
    } else {
        recheckForm();
    }
}

function recheckForm() {
    SubscribeConfig.email = '';
    SubscribeConfig.name = '';
    $('form.subscribe-form input[type="submit"]').removeProp('disabled');
    // $('form.subscribe-form input[type="submit"]').val('DAFTAR');
}

var subscriptionTrial = 0;
function submitForm() {
    if (subscriptionTrial > 3) {
        subscriptionTrial = 0;
    }
    $('form.subscribe-form .subscribe-email, form.subscribe-form .subscribe-name').prop('disabled', true);
    $.ajax({
        url: SubscribeConfig.Url,
        method: 'POST',
        data: JSON.stringify({ "email": SubscribeConfig.email, "name": SubscribeConfig.name }),
        headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
    }).done(function (returnData) {
        $('.subscribe-before').hide();
        $('.subscribe-after').show();
    }).fail(function (returnData) {
        subscriptionTrial++;
        if (refreshAuthAccess() && subscriptionTrial < 4) //refresh cookie
            submitForm();
    });
}