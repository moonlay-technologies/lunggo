$(function () {
    $('*[data-control=datetime]').each(function (index, item) {
        $(item).datepicker({
            format:'mm/dd/yyyy'
        });
    })

    $('*[data-control=spinner]').each(function (index, item) {
        var maxAttr = $(item).attr("data-max") === undefined ? 100 : $(item).attr("data-max");
        var minAttr = $(item).attr("data-min") === undefined ? 0 : $(item).attr("data-min");
        $(item).TouchSpin({
            initval: minAttr,
            min: minAttr,
            max: maxAttr
        });
    })

    $('*[data-control=stepper]').each(function (index, item) {
        $(item).stepper();
    })

    $('*[data-control=clock]').each(function (index, item) {
        $(item).clockpicker({
            autoclose: true,
        });
    })
    /*
        jquery mask
        $('.date').mask('11/11/1111');
        $('.time').mask('00:00:00');
        $('.date_time').mask('00/00/0000 00:00:00');
        $('.cep').mask('00000-000');
        $('.phone').mask('0000-0000');
        $('.phone_with_ddd').mask('(00) 0000-0000');
        $('.phone_us').mask('(000) 000-0000');
        $('.mixed').mask('AAA 000-S0S');
        $('.cpf').mask('000.000.000-00', {reverse: true});
        $('.money').mask('000.000.000.000.000,00', {reverse: true});
        $('.money2').mask("#.##0,00", {reverse: true, maxlength: false});
        $('.ip_address').mask('0ZZ.0ZZ.0ZZ.0ZZ', {translation: {'Z': {pattern: /[0-9]/, optional: true}}});
        $('.ip_address').mask('099.099.099.099');
        $('.percent').mask('##0,00%', {reverse: true});
        $('.clear-if-not-match').mask("00/00/0000", {clearIfNotMatch: true});
        $('.placeholder').mask("00/00/0000", {placeholder: "__/__/____"});
    */
});