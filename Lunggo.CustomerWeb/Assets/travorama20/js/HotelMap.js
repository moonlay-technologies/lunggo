jQuery(document).ready(function ($) {
    // Search List Image
    $(function () {
        $("body .img-list").each(function (i, elem) {
            var img = $(elem);
            var div = $("<div />").css({
                background: "url(" + img.attr("src") + ") no-repeat",
                width: "100%",
                height: "135px",
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

var map;
function initMap() {
    map = new google.maps.Map(document.getElementById('map'), {
        zoom: 16,
        center: new google.maps.LatLng(-33.91722, 151.23064),
        mapTypeId: 'roadmap'
    });

    var iconBase = 'https://maps.google.com/mapfiles/kml/shapes/';
    var icons = {
        parking: {
            icon: iconBase + 'parking_lot_maps.png'
        },
        library: {
            icon: iconBase + 'library_maps.png'
        },
        info: {
            icon: iconBase + 'info-i_maps.png'
        }
    };

    function addMarker(feature) {
        var marker = new google.maps.Marker({
            position: feature.position,
            icon: icons[feature.type].icon,
            map: map
        });
    }

    var features = [
      {
          position: new google.maps.LatLng(-33.916988, 151.233640),
          type: 'info'
      }, {
          position: new google.maps.LatLng(-33.91662347903106, 151.22879464019775),
          type: 'parking'
      }
    ];

    for (var i = 0, feature; feature = features[i]; i++) {
        addMarker(feature);
    }
}