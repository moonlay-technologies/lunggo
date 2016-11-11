// home controller
app.controller('hotelDetailController', ['$scope', '$log', '$http', '$resource', '$timeout', function ($scope, $log, $http, $resource, $timeout) {

    $scope.hotel = {};
    $scope.searchId = '';
    $scope.hotel.location = "";
    $scope.hotel.checkinDate = "";
    $scope.hotel.checkoutDate = "";
    $scope.hotel.adultCount = 3;
    $scope.hotel.childCount = 1;
    $scope.hotel.nightCount = "";
    $scope.hotel.roomCount = 2;


    $scope.init = function (model) {
        $log.debug(model);
        $scope.model = model;
        $scope.searchId = model.searchId;

        var maxImages = 6;

        var resource = $resource(HotelDetailsConfig.Url + '/:searchId/:hotelCd',
            {},
            {
                query: {
                    method: 'GET',
                    params: { searchId: model.searchId, hotelCd: model.hotelCd },
                    isArray: false
                }
            }
        );

        resource.query({}, {}).$promise.then(function (data) {
            $scope.hotel = data.hotelDetails;

            var loadedImages = 0;
            var tempHotelImages = [];
            $.each($scope.hotel.images, function (key, value) {
                
                tempHotelImages.push("http://photos.hotelbeds.com/giata/bigger/" + value);

                loadedImages++;
                if (loadedImages == maxImages) {
                    return false;
                }
            });
            $scope.hotel.images = tempHotelImages;
            $log.debug($scope.hotel);
            $scope.loc = $scope.hotel.city + ', ' + $scope.hotel.country;
            var cekin = $scope.hotel.room[0].rate.regsId.split(',')[2].split('|')[0];
            var cekout = $scope.hotel.room[0].rate.regsId.split(',')[2].split('|')[1];
            $scope.hotel.checkinDate = new Date(parseInt(cekin.substring(0, 4)), parseInt(cekin.substring(4, 6)) - 1, parseInt(cekin.substring(6, 8)));
            $scope.hotel.checkoutDate = new Date(parseInt(cekout.substring(0, 4)), parseInt(cekout.substring(4, 6)) - 1, parseInt(cekout.substring(6, 8)));
            $scope.hotel.nightCount = new Date($scope.hotel.checkoutDate).getDate() - new Date($scope.hotel.checkinDate).getDate();
            
            accordionFunctions();
            $timeout(function () { hotelDetailFunctions(); }, 0);

        })
    }

    $scope.model = {};
    $scope.hotels = [];
    $scope.totalActualHotel = '';
    $scope.hotelFilterDisplayInfo = undefined;
    $scope.filterDisabled = true;

    //@Url.Action("Search", "Hotel")?zzz={{departureDate}}" method="POST"
    //=============== hotel start ======================

  

    $scope.hotel.searchHotel = function () {
        $log.debug('searching hotel');
        //$http({
        //   url: "/id/Hotel/Search", 
        //   method: "GET",
        //   params: {aaa : "kode123", bbb : "silubab" }
        //   })
    };

    $scope.hotel.searchParam = function () {
        return ("?info=" +
            [$scope.hotel.location,
             $scope.hotel.checkinDate,
             $scope.hotel.checkoutDate,
             $scope.hotel.adultCount,
             $scope.hotel.childCount,
             $scope.hotel.nightCount,
             $scope.hotel.roomCount].join('.')
        )
    }

    var selectService = $resource(HotelSelectConfig.Url,
          {},
          {
              query: {
                  method: 'POST',
                  params: { },
                  isArray: false
              }
          }
      );
    $scope.bookRoom = function (room) {
        var childrenages = room.rate.regsId.split(',')[2].split('|')[10];
        var ages = [];
        if (childrenages != null && childrenages.length != 0) {
            var age = childrenages.split('~');
            for (var i = 0; i < age.length; i++) {
                ages.push(parseInt(age[i]));
            }
        }

        selectService.query({}, {
            "searchId": $scope.searchId,
            "regs": [
                  {
                      "regsId": room.rate.regsId,
                      "rateCount": room.rate.roomCount,
                      "adultCount": room.rate.adultCount,
                      "childCount": room.rate.childCount,
                      "childrenAges": ages
                  }
            ]
        }).$promise.then(function (data) {
            $scope.token = data.token;
            $scope.tokenLimit = data.timeLimit;
            $log.debug('selected, token = ' + $scope.selectToken);

            $log.debug('going to checkout');

            location.href = '/id/Hotel/Checkout/?token=' + $scope.token;
        })
    }

    var accordionFunctions = function() {
        //Accordion Help Section by W3School
        $timeout(function () {
            var acc = document.getElementsByClassName("accordion");
            var i;

            for (i = 0; i < acc.length; i++) {
                acc[i].onclick = function () {
                    this.classList.toggle("active");
                    this.nextElementSibling.classList.toggle("show");
                }
            }
        }, 0)
    }

    var hotelDetailFunctions = function(parameters) {
        // Open Room Detail
        $('body .info-room').on('click', function () {
            var parent1 = $(this).closest('.room-list').find('.room-left');
            var parent2 = parent1.closest('li').find('.hotel-detail');
            parent2.toggleClass('active');

            // Slick Slider Detail Hotel
            //$('body .hd-slider').slick({
            //    autoplay: false,
            //    autoplaySpeed: 2500,
            //    dots: false
            //});
        });

        // Dropdown room
        $('body .change-room').on('click', function () {
            $(this).toggleClass('active');
        });
    }
    //=============== hotel end ======================

}]);
