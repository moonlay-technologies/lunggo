// home controller
app.controller('hotelDetailController', ['$scope', '$log', '$http', '$resource', '$timeout', function ($scope, $log, $http, $resource, $timeout) {

    $scope.hotel = {};
    $scope.searchId = '';
    $scope.hotel.location = "BALI";
    $scope.hotel.checkinDate = "28-12-2016";
    $scope.hotel.checkoutDate = "30-12-2016";
    $scope.hotel.adultCount = 3;
    $scope.hotel.childCount = 1;
    $scope.hotel.nightCount = 1;
    $scope.hotel.roomCount = 2;


    $scope.init = function (model) {
        $log.debug(model);
        $scope.model = model;
        $scope.searchId = model.searchId;

        var maxImages = 6;

        var resource = $resource('//api.local.travorama.com/v1/hotel/GetHotelDetail/:searchId/:hotelCd',
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

    var selectService = $resource('//api.local.travorama.com/v1/hotel/select/',
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
        selectService.query({}, {
            "searchId": $scope.searchId,
            "regs": [
                  {
                      "regsId": room.rate.regsId,
                      "rateCount": 1,
                      "adultCount": 1,
                      "childCount": 2,
                      "childrenAges": [6, 8]
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
