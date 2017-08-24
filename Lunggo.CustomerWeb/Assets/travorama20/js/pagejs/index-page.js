//********************
// index page functions

flightFormSearchFunctions();

$(document).ready(function () {
    $('.fc-event-container').click(function () {
        $(this).closest('table').find('.fc-day-top').addClass('active');
    });
})

//// ===============================================
//// currently unimplemented codes
//// ===============================================


//// change header background
// function changeTheme(location) {
//     location = location.toLowerCase();
//     var backgroundImage = "";
//     var locationCode = '';
//     if (location.length > 0) {
//         switch (location) {
//             case "jakarta":
//                 backgroundImage = '/Assets/images/campaign/jakarta.jpg';
//                 location = 'Jakarta';
//                 locationCode = 'CGK';
//                 break;
//             case "bandung":
//                 backgroundImage = '/Assets/images/campaign/bandung.jpg';
//                 location = 'Bandung';
//                 locationCode = 'BDO';
//                 break;
//             case "surabaya":
//                 backgroundImage = '/Assets/images/campaign/surabaya.jpg';
//                 location = 'Surabaya';
//                 locationCode = 'SUB';
//                 break;
//             case "yogyakarta":
//                 backgroundImage = '/Assets/images/campaign/yogyakarta.jpg';
//                 location = 'Yogyakarta';
//                 locationCode = 'JOG';
//                 break;
//             case "bali":
//                 backgroundImage = '/Assets/images/campaign/bali.jpg';
//                 location = 'Denpasar';
//                 locationCode = 'DPS';
//                 break;
//             case "singapore":
//                 backgroundImage = '/Assets/images/campaign/singapore.jpg';
//                 location = 'Singapore';
//                 locationCode = 'SIN';
//                 break;
//             case "malaysia":
//                 backgroundImage = '/Assets/images/campaign/malaysia.jpg';
//                 location = 'Malaysia';
//                 locationCode = 'KUL';
//                 break;
//             case "hong kong":
//                 backgroundImage = '/Assets/images/campaign/hongkong.jpg';
//                 location = 'Hong Kong';
//                 locationCode = 'HKG';
//                 break;
//         }

//         // change value on HTML
//         $('.form-flight-destination').val(location + ' (' + locationCode + ')');
//         $('.slider').css('background-image', 'url(' + backgroundImage + ')');
//         FlightSearchConfig.flightForm.destination = locationCode;
//         $('html,  body').stop().animate({
//             scrollTop: 0
//         });
//     }
// }
