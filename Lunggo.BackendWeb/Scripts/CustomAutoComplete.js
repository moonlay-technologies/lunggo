
//attached autocomplete widget to all the autocomplete controls
$(document).ready(function () {
    BindAutoComplete();
});
function BindAutoComplete() {

    $('[data-autocomplete]').each(function (index, element) {
        var sourceurl = $(element).attr('data-sourceurl');
        var autocompletetype = $(element).attr('data-autocompletetype');
        $(element).autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: sourceurl,
                    dataType: "json",
                    data: { prefix: request.term },
                    success: function (data) {
                        response($.map(data, function (item) {
                            if (autocompletetype == 'Airport') {
                                return {
                                    label: item.City + ' (' + item.Code + ') - ' + item.Name,
                                    value: item.City + ' (' + item.Code + ')',
                                    selectedValue: item.Code
                                }
                            }
                            else if (autocompletetype == 'Airline') {
                                return {
                                    label: item.Name,
                                    value: item.Name,
                                    selectedValue: item.Code
                                }
                            }
                            else if (autocompletetype == 'Hotel') {
                                return {
                                    label: item.LocationName + ', ' + item.CountryName,
                                    value: item.LocationName + ', ' + item.CountryName,
                                    selectedValue: item.LocationId
                                }
                            }
                            else {
                                return {
                                    label: item,
                                    value: item,
                                    selectedValue: item
                                }
                            }

                        }));
                    },
                    error: function (data) {
                        alert(data);
                    },
                });
            },
            select: function (event, ui) {
                var valuetarget = $(this).attr('data-valuetarget');
                $("input:hidden[name='" + valuetarget + "']").val(ui.item.selectedValue);

                var selectfunc = $(this).attr('data-electfunction');
                if (selectfunc != null && selectfunc.length > 0) {
                    window[selectfunc](event, ui);
                    //funName();
                }
                //    selectfunc(event, ui);
            },
            change: function (event, ui) {
                var valuetarget = $(this).attr('data-valuetarget');


                $("input:hidden[name='" + valuetarget + "']").val('');
            },
        });
    });
}