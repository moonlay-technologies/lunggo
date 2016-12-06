// home controller
app.controller('homeController', ['$scope', '$log', '$http', '$location', '$resource', '$timeout', 'hotelSearchSvc', function ($scope, $log, $http, $location, $resource, $timeout, hotelSearchSvc) {

    // =========================== FLIGHT =========================================
    // **********
    // variables
    //$scope.PageConfig = $rootScope.PageConfig;
    //$scope.FlightSearchForm = $rootScope.FlightSearchForm;

    //$scope.PageConfig.Loaded = true;

    //jQuery(document).ready(function ($) {
    //    function showAgeChild() {
    //        var val = $('body .form-child select').val();
    //        val = parseInt(val);
    //        $('body .age-child').hide();

    //        if (val > 0) {
    //            $('body .age-child').show();
    //        }
    //    }
    //    $('body .form-child select').change(showAgeChild);
    //    showAgeChild();
    //});
    // =========================== FLIGHT =========================================

    $scope.departureDate = moment().add(1, 'day').format('DDMMYY');

    //(DateTime.Now.AddDays(1).ToString("ddMMyy")
    //if (!angular.isUndefined(topDestinations)) {
    //    $scope.topDestinations = topDestinations;
    //}

    //if (!angular.isUndefined(indexPageDestination) && !angular.isUndefined(indexPageDestinationsCode)) {
    //    $scope.flightDestination = {
    //        name: indexPageDestination,
    //        code: indexPageDestinationsCode
    //    };
    //}

    $scope.hotelCalendar = {};
    $scope.hotelCalendar.show = true;
    $scope.changeTab = function (tab) {
        if (tab == 'hotel') {
            $('.search-location').hide();
            $('.search-calendar').hide();
        } else if (tab == 'flight') {
            $scope.view.showHotelSearch = false;
            $('.search-calendar-hotel').hide();
        }
    }
    
    $scope.placeholderFilter = function (qqq) {
        if ($scope.hotelSearch.locationDisplay == "") {
            $scope.hotelSearch.locationDisplay = "Kota, Wilayah atau Nama Hotel";
        }
    }

    //=============== hotel start ======================
    hotelSearchSvc.initializeSearchForm($scope);

    $scope.hotel = {};
    $scope.myarray = [];
    $scope.getNumber = function (num) {
        $scope.myarrays = [];

        for (var i = 0; i <= num - 1; i++) {
            $scope.myarrays.push(i);
        }
        return $scope.myarrays;
    }
    $scope.view = {}
    $scope.view.showHotelSearch = false;
    $scope.searchlocation = false;
    $scope.wrongParam = false;

    $scope.init = function (model) {
        $log.debug(model);
    }

    $scope.hotel.searchHotel = function () {
        hotelSearchSvc.gotoHotelSearch($scope.hotelSearch);
    };

    $scope.HotelSearchForm = {
        AutoComplete: {
            Keyword: '',
            MinLength: 3,
            GetLocation: function () {
                hotelSearchSvc.getLocation($scope.HotelSearchForm.AutoComplete.Keyword);
            },

        },
    }

    $('.form-hotel-location').click(function () {
        $(this).select();
    });

    $('.hotel-date-picker').datepicker({
        numberOfMonths: 2,
        onSelect: function (date) {
            date = date.substring(3, 5) + "/" + date.substring(0, 2) + "/" + date.substring(6, 10);
            //console.log(data);
            //$scope.setCheckinDate(data);

            var scope = angular.element($('.hotel-date-picker')).scope();
            $scope.setCheckinDate(scope, date);

            $log.debug("checkinDate = " + date);
            var target;
            var chosenDate = new Date(date);
            $(target + ' .date').html(('0' + chosenDate.getDate()).slice(-2));
            $(target + ' .month').html(translateMonth(chosenDate.getMonth()));
            $(target + ' .year').html(chosenDate.getFullYear());
            $('.search-calendar-hotel').hide();
            var cd = new Date(date);
            var checkoutDate = new Date(cd.setDate(cd.getDate() + $scope.hotel.nightCount));
            var dd = checkoutDate.getDate();
            var mm = checkoutDate.getMonth() + 1;
            var yyyy = checkoutDate.getFullYear();
            var d = yyyy + '-' + mm + '-' + dd;
            $scope.hotel.checkoutDate = moment(checkoutDate, "MM-DD-YYYY");
            $log.debug("checkout date = " + $scope.hotel.checkoutDate);
        }
    });


    $scope.initOnclick = function () {

        // Select Age Children
        $('body .select-age').on('click', function (evt) {
            evt.stopPropagation();
            $(this).parent().siblings().children('div').children('.option').hide();
            $(this).children('.option').toggle();
            $('.search-calendar-hotel, .search-hotel, .child .option, .adult .option').hide();
        });

        //$('body .form-hotel-night').on('click', function () {
        //    $(this).children('.option').toggle();
        //    $('.search-calendar-hotel, .search-hotel, .form-hotel-room .option, .form-child-age').hide();
        //});

        //$('body .form-hotel-room').on('click', function () {
        //    $(this).children('.option').toggle();
        //    $('.search-calendar-hotel, .search-hotel, .form-hotel-night .option').hide();
        //});

        $('body .adult').on('click', function () {
            $(this).children('.option').toggle();
            $('.child .option, .select-age .option').hide();
        });

        $('body .child').on('click', function () {
            $(this).children('.option').toggle();
            $('.adult .option, .select-age .option').hide();
        });

        $('body .form-child-age').hide();
        $('body .form-hotel-room span').on('click', function () {
            $('body .form-child-age').show();
        });

        $('body input[name="FormAgeSubmit"]').on('click', function () {
            $('body .form-child-age').hide();
        });

    }
    //=============== hotel end ======================
}]);

// Calendar 2016 Controller
app.controller('campaignController', [
    '$scope', function ($scope) {

    }
]);

//********************
// hotel form search function
jQuery(document).ready(function ($) {
    //Show hotel
    $('.form-hotel-location').click(function (evt) {
        evt.stopPropagation();
        $('.search-hotel').show();
        $('.search-calendar-hotel, .select-age .option, .form-hotel-night .option, .form-hotel-room .option, .form-child-age').hide();
    });

    //hideHotel hotel
    function hideHotel() {
        $('.search-hotel').hide();
    }

    //close hotel
    $('.close-hotel').click(function () { hideHotel(); });

    $('.search-hotel .location-recommend nav ul li ').click(function () {
        var showClass = $(this).attr('data-show');
        $(this).addClass('active');
        $(this).siblings().removeClass('active');
        $('.search-hotel .location-recommend .tab-content>div').removeClass('active');
        $('.search-hotel .location-recommend .tab-content>div.' + showClass).addClass('active');
    });

    //*****
    // show and hide search calendar
    function showCalendar() {
        $('.search-calendar-hotel').show();
    }

    function hideCalendar() {
        $('.search-calendar-hotel').hide();
    }
    $('.close-calendar-hotel').click(function () { hideCalendar(); });

    //*****
    // date selector
    $('.form-hotel-checkin, .form-hotel-checkout').click(function (evt) {
        $('.search-calendar-hotel').show();
        showCalendar();
        $('.hotel-date-picker').datepicker('option', 'minDate', new Date());
        evt.stopPropagation();
        $('.search-hotel').show();
        $('.search-hotel, .select-age .option, .form-hotel-night .option, .form-hotel-room .option, .form-child-age').hide();
    });

    // Select Age Children
    $('body .select-age').on('click', function (evt) {
        evt.stopPropagation();
        $(this).parent().siblings().children('div').children('.option').hide();
        $(this).children('.option').toggle();
        $('.search-calendar-hotel, .search-hotel, .child .option, .adult .option').hide();
    });

    $('body .form-hotel-night').on('click', function () {
        $(this).children('.option').toggle();
        $('.search-calendar-hotel, .search-hotel, .form-hotel-room .option, .form-child-age').hide();
    });

    $('body .form-hotel-room').on('click', function () {
        $(this).children('.option').toggle();
        $('.search-calendar-hotel, .search-hotel, .form-hotel-night .option').hide();
    });

    $('body .adult').on('click', function () {
        $(this).children('.option').toggle();
        $('.child .option, .select-age .option').hide();
    });

    $('body .child').on('click', function () {
        $(this).children('.option').toggle();
        $('.adult .option, .select-age .option').hide();
    });

    $('body .form-child-age').hide();
    $('body .form-hotel-room span').on('click', function () {
        $('body .form-child-age').show();
    });

    $('body input[name="FormAgeSubmit"]').on('click', function () {
        $('body .form-child-age').hide();
    });

    //Mobile Home Page
    //$('select[name="roomclass"]').on('change', function () {
    //    var toNumb = parseInt($('select[name="roomclass"]').val());
    //    var a = $(this).closest('.form__room').parent();
    //    var b = a.parent().children('.form__visitor');

    //    var oldItem = b.children('.age-child').find('.ag-list');
    //    var oldItem2 = b.children('.age-child').find('.ag-list-2');

    //    var counter = 1;

    //    if (oldItem2.html() == '') {
    //        var itemc = oldItem.find('.ag-wrap').clone();
    //        oldItem2.html(itemc);
    //    }
    //    var newitem = oldItem2.find('.ag-wrap').clone();
    //    oldItem.html('');

    //    for (i = 0; i < toNumb; i++) {
    //        newitem.attr('id', 'item-' + counter);

    //        var item = newitem.clone();

    //        oldItem.append(item);
    //        counter++;
    //    };
    //});
});
 
//$scope.hotelSearch = {};
//$scope.hotelSearch.searchHotelType = 'Location';
//$scope.hotelSearch.location = "";
//$scope.hotelSearch.locationDisplay = "";
//$scope.hotelSearch.checkinDate = moment().locale("id").add(5, 'days');
//$scope.hotelSearch.checkinDateDisplay = $scope.hotelSearch.checkinDate.locale("id").format('LL');
//$scope.hotelSearch.nightCount = 2;
//$scope.hotelSearch.checkoutDate = moment().locale("id").add(7, 'days');
//$scope.hotelSearch.adultCount = 1;
//$scope.hotelSearch.childCount = 0;
//$scope.hotelSearch.childrenAges = [0, 0, 0, 0];

//$scope.hotelSearch.childrenAgeList = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11];

//$scope.hotelSearch.roomCount = 1;

//$scope.hotel.adultCountMin = 1;
//$scope.hotel.adultCountMax = 5;

//$scope.hotel.childCountMin = 0;
//$scope.hotel.childCountMax = 4;

//$scope.hotel.nightCountMin = 1;
//$scope.hotel.nightCountMax = 7;

//$scope.hotel.roomCountMin = 1;
//$scope.hotel.roomCountMax = 8;

//$scope.setCheckinDate = function (scope, date) {
//    scope.$apply(function () {
//        scope.hotelSearch.checkinDate = moment(date, "MM-DD-YYYY");
//        scope.hotelSearch.checkinDateDisplay = $scope.hotelSearch.checkinDate.locale("id").format('LL');
//    });
//}

//$scope.$watch('hotel.nightCount', function (newValue, oldValue) {
//    //var scope = angular.element($('.hotel-date-picker')).scope();
//    //$scope.setCheckinDate(scope, $scope.hotel.checkinDate);
//    //$scope.hotel.checkoutDate = $scope.hotel.checkinDate;
//    if (oldValue != newValue) {
//        var cod = moment($scope.hotel.checkinDate);
//        $scope.hotel.checkoutDate = moment(cod).add($scope.hotel.nightCount, 'days');
//    }

//});