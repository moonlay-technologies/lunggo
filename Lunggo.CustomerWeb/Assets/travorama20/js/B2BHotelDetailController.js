﻿// home controller
app.controller('B2BHotelDetailController', ['$scope', '$log', '$http', '$resource', '$timeout', '$interval', 'hotelSearchSvc', 'imageSvc',
    function ($scope, $log, $http, $resource, $timeout, $interval, hotelSearchSvc, imageSvc) {

        $scope.hotel = {};
        $scope.searchId = '';
        $scope.searchParam = '';
        $scope.hotel.location = '';
        $scope.hotel.checkinDate = "";
        $scope.hotel.checkoutDate = "";
        $scope.hotel.adultCount = 3;
        $scope.hotel.childCount = 1;
        $scope.hotel.nightCount = "";
        $scope.hotel.roomCount = 2;
        $scope.availableRateId = '';
        $scope.returnUrl = "/";
        $scope.roomCount = 3;
        $scope.minRoomCount = 3;
        $scope.maxRoomCount = 100;
        $scope.pageLoaded = true;
        $scope.searchDone = false;

        $scope.hotelImages = hotelImages;
        $scope.selectedRoom = '';
        $scope.selectFailed = false;
        $scope.booking = false;
        $scope.expired = false;
        $scope.noResults = true;
        $scope.showPopularDestinations = false;
        $scope.hideRoomDetail = true;
        $scope.singleRoom = [];
        $scope.hotelCode = '';
        $scope.lastSearch = {
            nightcount: '',
            checkIn: '',
            checkOut: '',
            occupancies: '',
            totalOcc: ''
        }

        $scope.lat = lat;
        $scope.lng = lng;
        $scope.hotelName = hotelName;
        $scope.hotelAddress = hotelAddress;

        var promise;
        $('#inputLocationHotel').on('click', function () {
            $scope.showPopularDestinations = true;
        });

        $scope.init = function (model) {
            $log.debug(model);

            //$scope.searchId = model.searchId;
            $scope.searchParam = model.searchParam;
            var mydata = $scope.searchParam.split('.');
            var cekin = mydata[2];
            var cekout = mydata[3];
            var nightcount = mydata[4];
            var roomcount = mydata[5];
            var occupancies = mydata[6].split('|');
            var totalOcc = occupancies.length;
            $scope.hotelCode = model.hotelCd;

            var searchParamObject = {
                nightCount: nightcount,
                roomCount: roomcount,
                checkinDate: moment.utc(cekin, "YYYY-MM-DD"),
                checkoutDate: moment.utc(cekout, "YYYY-MM-DD"),
                occupancies: []
            }

            for (var x = 0; x < occupancies.length; x++) {
                var occdata = occupancies[x].split('~');
                var ages = [];
                if (occdata.length == 3) {
                    var agedata = occdata[2].split(',');
                    for (var a = 0; a < agedata.length; a++) {
                        ages.push(parseInt(agedata[a]));
                    }
                    for (var b = 0; b < 4 - agedata.length; b++) {
                        ages.push(0);
                    }
                } else {
                    ages = [0, 0, 0, 0];
                }
                searchParamObject.occupancies.push({
                    adultCount: occdata[0],
                    childCount: occdata[1],
                    childrenAges: occdata[1] == "0" ? [0, 0, 0, 0] : ages,
                    roomCount: 1
                });
            }

            for (var m = 0; m < 8 - roomcount; m++) {
                searchParamObject.occupancies.push({
                    adultCount: 1,
                    childCount: 0,
                    childrenAges: [0, 0, 0, 0],
                    roomCount: 1
                });
            }
            hotelSearchSvc.initializeSearchForm($scope, searchParamObject);
            hotelSearchSvc.getHolidays();
            $('.hotel-date-picker').datepicker('option', 'beforeShowDay', hotelSearchSvc.highlightDays);

            $scope.hotelSearch.destinationCheckinDate = moment(cekin, "YYYY-MM-DD").locale("id").format('dddd, DD MMMM YYYY');
            $scope.hotelSearch.destinationCheckoutDate = moment(cekout, "YYYY-MM-DD").locale("id").format('dddd, DD MMMM YYYY');

            var loadedImages = 0;
            var maxImages = 6;
            var tempHotelImages = [];

            //Remove broken images
            //$scope.hotelImages = JSON.parse($scope.hotelImages);
            //    var imageCount = $scope.hotelImages.length;
            //    var imageIndex = 0;
            //    var finishedSlider = 0;
            //    $.each($scope.hotelImages, function (key, value) {
            //        imageSvc.isImage(value).then(function () {
            //            if (loadedImages < maxImages) {
            //                loadedImages++;
            //                tempHotelImages.push(value);
            //                $scope.hotelImages = tempHotelImages;
            //            }
            //            else return false;
            //        }, function () {
            //            return; //equivalent of continue
            //        }).finally(function () {
            //            imageIndex++;
            //            if (!finishedSlider && loadedImages == 1 || (imageIndex + 1 == imageCount && loadedImages < maxImages)) {
            //                $timeout(function () {}, 0);
            //                finishedSlider = true;
            //            }
            //        });
            //    });
            $timeout(function () { initiateMap(); }, 0);
            $timeout(function () {
                // **********
                // Search Detail Tab
                $('body .detail-tab').each(function (index, item) {
                    if (index > 0) {
                        $(item).hide();
                    }
                });
                $('body .hotel-detail-menu-action a').on('click touchstart', function () {
                    var id = $(this).attr('attr-link');
                    $('body .detail-tab').hide();
                    $(id).show();
                });


                //Shorten Area

                //Search Detail
                $('body .sh-desc a').on('click touchstart', function () {
                    $('body .sh-desc a').toggleClass('active');
                    $('body .sh-txt').toggleClass('opened');
                });



                //**********
                //Slick Slider Detail Hotel
                $('.dh-slider').slick({
                    autoplay: true,
                    autoplaySpeed: 2500,
                    dots: false
                });


                $('.mh-list a').click(function () {
                    $(window).on("scroll", function () {
                        if ($(window).scrollTop() > 1800) {
                            $('.site-header').fadeOut();
                        } else {
                            $('.site-header').fadeIn();
                        }
                    });
                    var val = $(this).data('value');
                    $('html, body').animate({
                        scrollTop: $("#" + val).offset().top
                    }, 700);
                });
                //Hotel Detail
                //$('.mh-list a').click(function () {
                //    var link = $(this).attr('id');
                //    var header;
                //    if (link == '#menu-facility') {
                //        $('html, body').animate({
                //            scrollTop: $(".info-hotel").offset().top
                //        }, 1000);
                //            }
                //    else if (link == '#menu-desc') {
                //        $('html, body').animate({
                //            scrollTop: $("#menu-desc").offset().top - 80
                //        },1000);

                //    } else if (link == '#menu-tnc') {
                //        $('html, body').animate({
                //            scrollTop: $("#menu-tnc").offset().top - 80
                //        }, 1000);
                //    }
                //        });

                // Hotel Detail Slider
                $('#image-gallery').lightSlider({
                    gallery: true,
                    item: 1,
                    thumbItem: 6,
                    slideMargin: 0,
                    loop: true,
                    keyPress: true,
                    onSliderLoad: function () {
                        $('#image-gallery').removeClass('cS-hidden');
                    }
                });
            }, 0);

            $timeout(function () {
                $scope.availableRates(nightcount, searchParamObject.checkinDate, searchParamObject.checkoutDate,
                    searchParamObject.occupancies, totalOcc);
            }, 0);
            $scope.hotelSearch.location = $scope.getLocationCode();
        }

        // ********************** DISPLAY HOTEL DETAILS *********************************

        var initiateMap = function () {
            var myLatLng = { lat: $scope.lat, lng: $scope.lng };

            var map = new google.maps.Map(document.getElementById('map'), {
                zoom: 15,
                center: myLatLng,
                mapTypeControl: false,
                streetViewControl: false,
                fullscreenControl: false
            });

            var marker = new google.maps.Marker({
                position: myLatLng,
                map: map,
                title: $scope.hotelName,
                attribution: {
                    source: 'AIzaSyCRAmMz6GPXsXi1pZAl5QUsjNTcY0ZfqVA',
                    webUrl: 'https://developers.google.com/maps/'
                }
            });

            var infoDesc = '<div class="map-content">' +
                '<div class="hotel-title bold-txt blue-txt">' + $scope.hotelName + '</div>' +
                '<div class="hotel-address regular-txt">' + $scope.hotelAddress + '</div>' +
                '</div>';

            var infoWindow = new google.maps.InfoWindow({
                content: infoDesc
            });

            infoWindow.open(map, marker);

            marker.addListener('click', function () {
                infoWindow.open(map, marker);
            });
        }

        var resource = $resource(HotelAvailableRatesConfig.Url,
            {},
            {
                query: {
                    method: 'POST',
                    isArray: false,
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }
            }
        );

        $scope.setDescriptionDisplay = function (descriptions) {
            if (descriptions != null && descriptions.length > 0) {
                var description = [];
                var descriptionArray = descriptions.split('.');
                var tempDescription = '';
                $.each(descriptionArray, function (key, value) {
                    value = value + '.';
                    tempDescription = tempDescription + value;
                    if (key % 3 == 0) {
                        description.push(tempDescription);
                        tempDescription = '';
                    }
                });
                return description;
            }
        }

        var validateResponse = function (data) {
            //searchId expired
            if (data.error == "ERHGHD02") {
                $log.debug('searchId is expired. (' + $scope.searchId + ') \n redirecting to search with ' + $scope.searchParam);
                $scope.expired = true;
                hotelSearchSvc.gotoHotelSearch($scope.hotelSearch);
            }
        }

        $scope.seeRoomDetail = function (room) {
            $scope.selectedRoom = room;
        }

        var hotelDetailFunctions = function () {
            // Open Room Details
            $('body .room-rl').on('click', function () {
                var parent1 = $(this).closest('.room-list').find('.room-left');
                var parent2 = parent1.closest('.room-list-container li').find('.hotel-detail');
                parent2.slideToggle('.4s');
                $(this).closest('.room-list-container li').siblings().find('.hotel-detail, .option').hide();

                $(this).each(function () {
                    $(this).closest('.room-list-container li').find('.room-gallery').lightSlider({
                        gallery: true,
                        item: 1,
                        thumbItem: 6,
                        slideMargin: 0,
                        loop: true,
                        keyPress: true,
                        onSliderLoad: function () {
                            $('.room-gallery').removeClass('cS-hidden');
                        }
                    });
                });

                var list = $(this).closest('.room-list-container li');

                list.find('.room-list').toggleClass('active');
                list.siblings().find('.room-list').removeClass('active');

                list.toggleClass('active');
                list.siblings().removeClass('active');


            });

            $("body .change-room").click(function () {
                var parent1 = $(this).closest('.room-list').find('.room-left');
                var parent2 = parent1.closest('.room-list-container li');
                parent2.find('.option').toggle();
                parent2.siblings().find('.hotel-detail, .option').hide();
            });
        }

        var setSingleRoom = function (rooms) {
            var singleRoom = [];
            for (var i = 0; i < rooms.length; i++) {
                for (var j = 0; j < rooms[i].rates.length; j++) {
                    singleRoom.push({
                        "roomCode": rooms[i].roomCode,
                        "roomName": rooms[i].roomName,
                        "Type": rooms[i].Type,
                        "TypeName": rooms[i].TypeName,
                        "roomImages": rooms[i].roomImages,
                        "facilityCode": rooms[i].facilityCode,
                        "characteristic": rooms[i].characteristic,
                        "rate": rooms[i].rates[j]
                    });
                }
            }

            return singleRoom;
        }

        $scope.togleDisplay = function () {
            if ($scope.roomCount == $scope.maxRoomCount) {
                $scope.roomCount = $scope.minRoomCount;
            } else {
                $scope.roomCount = $scope.maxRoomCount;
            }
        }

        //TBD??
        //var initiateSlider = function () {
        //    $('#image-gallery').lightSlider({
        //        gallery: true,
        //        item: 1,
        //        thumbItem: 6,
        //        slideMargin: 0,
        //        loop: true,
        //        keyPress: true,
        //        onSliderLoad: function () {
        //            $('#image-gallery').removeClass('cS-hidden');
        //        }
        //    });

        //    $timeout(function () {
        //        var altImagePath = document.location.origin + '/Assets/travorama20/images/Hotel/no-hotel.png';
        //        var elements = angular.element(document.querySelectorAll('#imgtum'));

        //        //angular.each(elements, function(key, value) {
        //        //    elements[key].removeAttr('src');
        //        //    elements[key].attr('src', altImagePath); // set default image
        //        //});
        //    }, 0);

        //}

        //TBD??
        $scope.isFreeRefund = function (room, index) {
            if (room.rate.isRefundable) {
                if (room.rate.cancellation !== undefined && room.rate.cancellation.length > 0) {
                    var cancellation = room.rate.cancellation;

                    var cancelTRUE = moment(cancellation[0].startTime).isAfter(moment(), 'day');


                    if (true) {

                    }
                    return cancelTRUE;
                }
                else {
                    $log.debug('room ' + index + ' is refundable but no cancellation rate');
                    return false;
                }
            }
            else {
                return false;
            }
        };

        $scope.toTitleCase = function (str) {
            return str.replace(/\w\S*/g, function (txt) { return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase(); });
        }

        $scope.setHoursToWIB = function (date) {
            var newDate = new Date(date);
            return newDate;
        }


        // ********************************* END ****************************************

        // ******************************* SELECT ROOM **********************************



        var selectService = $resource(HotelSelectConfig.Url,
              {},
              {
                  query: {
                      method: 'POST',
                      params: {},
                      isArray: false,
                      headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                  }
              }
          );


        $scope.bookRoom = function (room) {
            $scope.booking = true;
            var authAccess = getAuthAccess();
            if (authAccess == 1 || authAccess == 2) {
                selectService.query({}, {
                    "searchId": $scope.availableRateId,
                    "regs": [
                          {
                              "regsId": room.regsId,
                              "rateCount": room.breakdowns[0].rateCount,
                              "adultCount": room.breakdowns[0].adultCount,
                              "childCount": room.breakdowns[0].childCount,
                              "childrenAges": room.breakdowns[0].childrenAges
                          }
                    ]
                }).$promise.then(function (data) {
                    $scope.booking = false;
                    if (data.status != 200 || data.token == null) {
                        $scope.selectFailed = true;
                    } else {
                        $scope.token = data.token;
                        $scope.tokenLimit = data.timeLimit;
                        $log.debug('selected, token = ' + $scope.selectToken);

                        $log.debug('going to checkout');

                        location.href = '/id/B2BHotel/Checkout/?token=' + $scope.token;
                    }

                });
            } else {
                $scope.booking = false;
                $log.debug('Unauthorised');
            }
        }

        // ********************************* END ****************************************

        // ********************************** OTHERS ************************************

        $scope.HotelSearchForm = {
            AutoComplete: {
                Keyword: '',
                MinLength: 3,
                GetLocation: function () {
                    $scope.getLocation($scope.HotelSearchForm.AutoComplete.Keyword);
                    // function in hotelSearchService.js
                },
            },
        }

        var accordionFunctions = function () {
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
            }, 0);
        }

        $('.change-hotel.form-control').click(function () {
            $(this).select();
        });

        $scope.availableRates = function (nightCount, checkIn, checkOut, occupancies, totalOcc) {
            $scope.hideRoomDetail = true;
            $scope.searchDone = false;
            $scope.noResults = false;
            var x = checkIn.format('YYYY');
            var y = checkIn.format('MM');
            var z = checkIn.format('DD');
            var ci = x + "-" + y + "-" + z;
            var a = checkOut.format('YYYY');
            var b = checkOut.format('MM');
            var c = checkOut.format('DD');
            var co = a + "-" + b + "-" + c;

            var newcheckIn = new Date(ci);
            var newcheckOut = new Date(co);

            $scope.lastSearch.checkIn = checkIn;
            $scope.lastSearch.checkOut = checkOut;
            $scope.lastSearch.nightcount = nightCount;
            $scope.lastSearch.occupancies = occupancies;
            $scope.lastSearch.totalOcc = totalOcc;
            var authAccess = getAuthAccess();
            if (authAccess == 1 || authAccess == 2) {
                resource.query({}, {
                    "hotelCode": $scope.hotelCode,
                    "nights": nightCount,
                    "checkIn": newcheckIn,
                    "checkout": newcheckOut,
                    "occupancies": occupancies.slice(0, totalOcc)
                }).$promise.then(function (data) {
                    $scope.searchDone = true;
                    $scope.hideRoomDetail = false;
                    if (data != null) {
                        if (data.rooms != null && data.rooms.length > 0) {
                            $scope.hotel.rooms = data.rooms;
                            $scope.expiryDate = new Date(data.expTime);
                            $scope.expiryDate = $scope.expiryDate.setMinutes($scope.expiryDate.getMinutes() + 1);

                            promise = $interval(function () {
                                var nowTime = new Date();
                                if (nowTime > $scope.expiryDate) {
                                    $scope.expired = true;
                                }
                            }, 1000);
                            $scope.availableRateId = data.id;
                            $scope.singleRoom = setSingleRoom(data.rooms);
                            $log.debug($scope.singleRoom);
                            $.each($scope.hotel.rooms, function (roomKey, room) {
                                $.each(room.roomImages, function (imageKey, roomImage) {
                                    $scope.hotel.rooms[roomKey].roomImages[imageKey] = roomImage;
                                });
                            });

                            var cekin = $scope.hotel.rooms[0].rates[0].regsId.split(',')[2].split('|')[0];
                            var cekout = $scope.hotel.rooms[0].rates[0].regsId.split(',')[2].split('|')[1];
                            $scope.hotel.checkinDate = new Date(parseInt(cekin.substring(0, 4)), parseInt(cekin.substring(4, 6)) - 1, parseInt(cekin.substring(6, 8)));
                            $scope.hotel.checkoutDate = new Date(parseInt(cekout.substring(0, 4)), parseInt(cekout.substring(4, 6)) - 1, parseInt(cekout.substring(6, 8)));
                            $scope.hotel.nightCount = (new Date($scope.hotel.checkoutDate) - new Date($scope.hotel.checkinDate)) / (3600 * 24 * 1000);


                            $timeout(function () { hotelDetailFunctions(); }, 0);
                            $timeout(function () { accordionFunctions(); }, 0);
                            //**********
                            //Open Detail Room in Mobile
                            $timeout(function () {
                                $('body .dh-list').on('click', function () {
                                    var id = $(this).parent();

                                    id.toggleClass('active');
                                    id.siblings().removeClass('active');

                                    id.find('.dh-list-detail').toggleClass('active');
                                    id.siblings().find('.dh-list-detail').removeClass('active');
                                });
                            }, 0);

                            $scope.singleRoom.sort(function (a, b) {
                                return a.rate.breakdowns[0].netFare - b.rate.breakdowns[0].netFare;
                            });
                        }
                    } else {
                        $scope.noResults = true;
                        $scope.searchDone = false;
                    }
                });
            } else {
                $scope.searchDone = true;
            }
        }

        $scope.refreshPage = function () {
            $interval.cancel(promise);
            $scope.expired = false;
            $scope.availableRates($scope.lastSearch.nightcount, $scope.lastSearch.checkIn, $scope.lastSearch.checkOut,
                $scope.lastSearch.occupancies, $scope.lastSearch.totalOcc);
        }

        $scope.slideToResults = function (platform) {
            if (platform == 'Desktop') {
                $('html, body').animate({
                    scrollTop: $('.menu-hotel').offset().top - 80
                }, 700);
            } else {
                $('html, body').animate({
                    scrollTop: $('.result-title').offset().top
                }, 700);
            }
        }
        // ********************************* END ****************************************

    }]);