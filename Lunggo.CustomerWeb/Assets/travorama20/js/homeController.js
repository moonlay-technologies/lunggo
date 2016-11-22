// home controller
app.controller('homeController', ['$scope', '$log', '$http', '$location', '$resource', '$timeout', 'hotelSearchSvc', function ($scope, $log, $http, $location, $resource, $timeout, hotelSearchSvc) {

    $scope.departureDate = departureDate;
    $scope.topDestinations = topDestinations;
    $scope.flightDestination = {
        name: indexPageDestination,
        code: indexPageDestinationsCode
    };

    $scope.hotelCalendar = {};
    $scope.hotelCalendar.show = true;
    $scope.changeTab = function (tab) {
        if (tab == 'hotel') {
            $('.search-location').hide();
            $('.search-calendar').hide();
        }else if (tab == 'flight') {
            $scope.view.showHotelSearch = false;
            $('.search-calendar-hotel').hide();
        }
    }

    //=============== hotel start ======================
    hotelSearchSvc.initializeSearchForm($scope);

    $scope.hotel = {};
    //$scope.hotelSearch = {};
    $scope.hotelSearch.searchHotelType = 'Location';
    $scope.hotelSearch.location = "";
    $scope.hotelSearch.locationDisplay = "";
    $scope.hotelSearch.checkinDate = moment().locale("id").add(5, 'days');
    $scope.hotelSearch.checkinDateDisplay = $scope.hotelSearch.checkinDate.locale("id").format('LL');
    $scope.hotelSearch.nightCount = 2;
    $scope.hotelSearch.checkoutDate = moment().locale("id").add(7, 'days');
    $scope.hotelSearch.adultCount = 1;
    $scope.hotelSearch.childCount = 0;
    $scope.hotelSearch.childrenAges = [0, 0, 0, 0];

    $scope.hotelSearch.childrenAgeList = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11];

    $scope.hotelSearch.roomCount = 1;

    //$scope.hotel.adultCountMin = 1;
    //$scope.hotel.adultCountMax = 5;

    //$scope.hotel.childCountMin = 0;
    //$scope.hotel.childCountMax = 4;

    //$scope.hotel.nightCountMin = 1;
    //$scope.hotel.nightCountMax = 7;

    //$scope.hotel.roomCountMin = 1;
    //$scope.hotel.roomCountMax = 8;

    $scope.view = {}
    $scope.view.showHotelSearch = false;
    $scope.searchlocation = false;
    $scope.wrongParam = false;

    //$scope.addValue = function (variableName, amount) {
    //    var minCount = 1;
    //    if (variableName == 'adultCount') {
    //        $scope.hotel.adultCount = $scope.hotel.adultCount + amount;
    //        if ($scope.hotel.adultCount < $scope.hotel.adultCountMin) $scope.hotel.adultCount++;
    //        else if ($scope.hotel.adultCount > $scope.hotel.adultCountMax) $scope.hotel.adultCount--;
    //    }
    //    else if (variableName == 'childCount') {
    //        $scope.hotel.childCount = $scope.hotel.childCount + amount;
    //        if ($scope.hotel.childCount < $scope.hotel.childCountMin) $scope.hotel.childCount++;
    //        else if ($scope.hotel.childCount > $scope.hotel.childCountMax) $scope.hotel.childCount--;
    //    }
    //    else if (variableName == 'nightCount') {
    //        $scope.hotel.nightCount = $scope.hotel.nightCount + amount;
    //        if ($scope.hotel.nightCount < $scope.hotel.nightCountMin) $scope.hotel.nightCount++;
    //        else if ($scope.hotel.nightCount > $scope.hotel.nightCountMax) $scope.hotel.nightCount--;
    //    }
    //    else if (variableName == 'roomCount') {
    //        $scope.hotel.roomCount = $scope.hotel.roomCount + amount;
    //        if ($scope.hotel.roomCount < $scope.hotel.roomCountMin) $scope.hotel.roomCount++;
    //        else if ($scope.hotel.roomCount > $scope.hotel.roomCountMax) $scope.hotel.roomCount--;
    //    }
    //}

    //var resource = $resource(HotelAutocompleteConfig.Url + '/:prefix',
    //       { prefix: '@prefix'},
    //       {
    //           query: {
    //               method: 'GET',
    //               params: { },
    //               isArray: false
    //           }
    //       }
    //   );

    //$scope.$watch('hotel.locationDisplay', function (newValue, oldValue, ccc) {
    //    if (newValue.length >= 3) {
    //        resource.query({ prefix: newValue }).$promise.then(function (data) {
    //            $timeout(function () {
    //                $scope.hotel.hotelAutocomplete = data.hotelAutocomplete;
    //                $log.debug($scope.hotel.hotelAutocomplete);
    //            }, 0);
    //        });
    //    }
    //});

    $scope.setCheckinDate = function (scope, date) {
        scope.$apply(function () {
            scope.hotelSearch.checkinDate = moment(date, "MM-DD-YYYY");
            scope.hotelSearch.checkinDateDisplay = $scope.hotelSearch.checkinDate.locale("id").format('LL');
        });
    }

    $scope.$watch('hotel.nightCount', function (newValue, oldValue) {
        //var scope = angular.element($('.hotel-date-picker')).scope();
        //$scope.setCheckinDate(scope, $scope.hotel.checkinDate);
        //$scope.hotel.checkoutDate = $scope.hotel.checkinDate;
        if (oldValue != newValue) {
            var cod = moment($scope.hotel.checkinDate);
            $scope.hotel.checkoutDate = moment(cod).add($scope.hotel.nightCount, 'days');
        }
        
    });
        
    $scope.hotel.searchHotel = function () {
        hotelSearchSvc.gotoHotelSearch($scope.hotelSearch);
    };

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
    //=============== hotel end ======================
}]);

// Calendar 2016 Controller
app.controller('campaignController', [
    '$scope', function ($scope) {
        
    }
]);

//********************
// hotel form search function
jQuery(document).ready(function($) {
    //Show hotel
    $('.form-hotel-location').click(function (evt) {
        evt.stopPropagation();
        $('.search-hotel').show();
        $('.search-calendar-hotel, .select-age .option').hide();
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
        $('.search-hotel, .select-age .option').hide();
    });

    // Select Age Childeren
    $('body .select-age').on('click', function (evt) {
        evt.stopPropagation();
        $(this).parent().siblings().children('div').children('.option').hide();
        $(this).children('.option').toggle();
        $('.search-calendar-hotel, .search-hotel').hide();
    });
});
