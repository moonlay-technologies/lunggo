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

    //// change submit button's state
    var originInputValue = $(this).val();
    $(this).val('LOADING');
    $(this).prop('disabled', true);

    //// prepare form data
    var data = {};
    data.email = {
        inputDOM : $('form.subscribe-form input.subscribe-email'),
        required : "Mohon masukan Alamat Email Anda",
        invalid : "Alamat email tidak valid",
        validationRule : 'email'
    }
    data.name = {
        inputDOM : $('form.subscribe-form input.subscribe-name')
    }
    
    //// validation
    // if (SubscribeConfig.name && SubscribeConfig.email) {
    // if (SubscribeConfig.email) {
    var validation = validateForm(data); //return true || error data
    
    if (validation === true) {
        submitForm();
        console.log('submitForm')
    } else {
        recheckForm();
        $(this).prop('disabled', false);
        $(this).val(originInputValue);
        ///TODO buat liat validationnya return object required-invalid kyk apa
        console.log(validation);
    }
});

/*  Validate Form data
 *  will return (boolean) true, or (Array) errors
 */
function validateForm(data) {
    // if ($('form.subscribe-form input.subscribe-email').val()) {
    //     var emailValue = $('form.subscribe-form input.subscribe-email').val();
    //     if (validateEmail(emailValue)) {
    //         SubscribeConfig.email = emailValue;
    //     } else {
    //         SubscribeConfig.email = '';
    //         alert('Alamat email tidak valid');
    //     }
    // } else {
    //     $('form.subscribe-form input.subscribe-email').attr('placeholder', 'Mohon masukan Alamat Email Anda');
    //     $('form.subscribe-form input.subscribe-email').parent().addClass('has-error');
    // }

    var required = [], invalid = [];
    for (var key in data){
        var val = data[key].val || data[key].inputDOM.val(); // get input value. if not specified in data, get $(input).val()
        if (val) {
            switch (data[key].validationRule){
                case "email":
                    if (validateEmail(val)) {
                        //TODO GANTI
                        SubscribeConfig[key] = val;
                    } else {
                        //TODO GANTI
                        SubscribeConfig[key] = '';
                        // alert("Alamat email tidak valid");
                        alert(data[key].invalid || "invalid " + key);
                        invalid.push(key);
                        // return false;
                    }
                    break;
                // case "password": break;
                default: break;
            }
            //TODO GANTI
            SubscribeConfig[key] = val;
        } else if (data[key].required) {
            data[key].inputDOM.attr('placeholder', data[key].required || "Mohon masukan "+key+" Anda");
            data[key].inputDOM.parent().addClass('has-error');
            required.push(key);
            // return false;
        } //// else means the data is absent BUT not required (nullable); Proceed to the next data
    }
    return (required.length || invalid.length) ? {required,invalid} : true;
}

function recheckForm() {
    SubscribeConfig.email = '';
    SubscribeConfig.name = '';
}

var subscriptionTrial = 0;
function submitForm() {
    if (subscriptionTrial > 3) {
        subscriptionTrial = 0;
    }
    //TODO TO BE REMOVED
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