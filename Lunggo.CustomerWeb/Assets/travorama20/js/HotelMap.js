jQuery(document).ready(function ($) {
    // Search List Image
    $(function () {
        $("body .img-list").each(function (i, elem) {
            var img = $(elem);
            var div = $("<div />").css({
                background: "url(" + img.attr("src") + ") no-repeat",
                width: "100%",
                height: "125px",
                "background-size": "cover",
                "background-position": "center"
            });
            img.replaceWith(div);
        });
    });

    $('body .dropdown').click(function () {
        $('body .option').toggle();
        $('.hotel-sort').text();
        $('.form-hotel-sort .option > span').click(function () {
            $('.hotel-sort').text(this.innerHTML);
        });
    });

    $('.zoom-map').click(function () {
        var point = $(this).closest('.search-list-result').find('.row-content');
        point.children('.search-list').hide();
        point.children('.search-map').removeClass('col-half');
    });

    $('.list-map').click(function () {
        var point = $(this).closest('.search-list-result').find('.row-content');
        point.children('.search-list').show();
        point.children('.search-map').addClass('col-half');
    });

    //Show tab filter
    $('body .switch-filter').click(function () {
        var trig = $('.filter-trigger');
        var filter = $('.filter-content');

        if ($(this).is('#filter-star')) {
            trig.find('.filter-star').addClass('active');
            trig.find('.filter-star').siblings().removeClass('active');

            filter.find('.filter-star-content').addClass('active');
            filter.find('.filter-star-content').siblings().removeClass('active');
        } else if ($(this).is('#filter-price')) {
            trig.find('.filter-price').addClass('active');
            trig.find('.filter-price').siblings().removeClass('active');

            filter.find('.filter-price-content').addClass('active');
            filter.find('.filter-price-content').siblings().removeClass('active');
        } else if ($(this).is('#filter-area')) {
            trig.find('.filter-area').addClass('active');
            trig.find('.filter-area').siblings().removeClass('active');

            filter.find('.filter-area-content').addClass('active');
            filter.find('.filter-area-content').siblings().removeClass('active');
        } else if ($(this).is('#filter-facility')) {
            trig.find('.filter-facility').addClass('active');
            trig.find('.filter-facility').siblings().removeClass('active');

            filter.find('.filter-facility-content').addClass('active');
            filter.find('.filter-facility-content').siblings().removeClass('active');
        }
    });

    //Close tab filter
    $('body .close-filter').click(function () {
        $('.filter-trigger div').removeClass('active');
        $('.fc-wrapper').removeClass('active');
    });

    //Custom checkbox
    $('body .sqr').on('click', function () {
        var id = $(this).find('.check');
        if ($(id).is(':checked')) {
            id.checked = true;
            $(this).addClass('active');
        } else {
            id.checked = false;
            $(this).removeClass('active');
        }
    });

    $('body .select-all').on('click', function () {
        var p = $(this).closest('.fc-title').parent().find('.tab-detail');
        var c = p.find('.sqr');
        var i = c.find('.check');

        i.prop('checked', true);
        c.addClass('active');
    }); $('body .select-none').on('click', function () {
        var p = $(this).closest('.fc-title').parent().find('.tab-detail');
        var c = p.find('.sqr');
        var i = c.find('.check');

        i.prop('checked', false);
        c.removeClass('active');
    });
});

function initMap() {
    var currentHotel = { lat: -8.710695, lng: 115.178775 };
    var otherHotel = { lat: -8.711247, lng: 115.181347 };

    var map = new google.maps.Map(document.getElementById('map'), {
        zoom: 15,
        center: currentHotel,
        mapTypeControl: false,
        streetViewControl: false,
        fullscreenControl: false
    });

    var iconBase = '/Assets/images/hotel/markers-';
    var icons = {
        hotelBlue: {
            icon: iconBase + 'blue.png'
        }, hotelRed: {
            icon: iconBase + 'red.png'
        }
    };

    var features = [
      {
          position: currentHotel,
          type: 'hotelBlue'
      }, {
          position: otherHotel,
          type: 'hotelRed'
      }
    ];

    for (var i = 0, feature; feature = features[i]; i++) {
        addMarker(feature);
    }

    function addMarker(feature) {
        var marker = new google.maps.Marker({
            position: feature.position,
            icon: icons[feature.type].icon,
            map: map
        });

        var infoDesc = '<div class="map-container">' +
        '<div class="hotel-round"' + 'style="' + 'background-image: url(' + '/Assets/images/hotel/hotel-dummy.jpg' + ')"></div>' +
        '<div class="map-content normal-txt">' +
        '<div class="hotel-title bold-txt blue-txt">' + 'Everyday Smart' + '</div>' +
        '<div class="' + 'star star-5' + '"></div>' +
        '<small class="orange-txt sm-txt underline-txt">' + '999.999.999' + '</small>' +
        '<div class="orange-txt bold-txt">' + '999.999.999' + '</div>' +
        '</div>' +
        '</div>';

        var infoWindow = new google.maps.InfoWindow({
            content: infoDesc
        });

        infoWindow.open(map, marker);

        marker.addListener('click', function () {
            infoWindow.open(map, marker);
        });
    }
}