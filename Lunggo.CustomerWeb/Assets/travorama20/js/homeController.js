// home controller
app.controller('homeController', ['$scope', '$log', '$http', '$location', '$resource', function ($scope, $log, $http, $location, $resource) {

    $scope.departureDate = departureDate;
    $scope.topDestinations = topDestinations;
    $scope.flightDestination = {
        name: indexPageDestination,
        code: indexPageDestinationsCode
    };

    $scope.hotelCalendar = {};
    $scope.hotelCalendar.show = true;
    $scope.changeTab = function (){
    }

    //@Url.Action("Search", "Hotel")?zzz={{departureDate}}" method="POST"
    //=============== hotel start ======================

    $scope.hotel = {};
    $scope.hotel.location = "";
    $scope.hotel.checkinDate = moment().locale("id").add(5, 'days');
    $scope.hotel.checkinDateDisplay = $scope.hotel.checkinDate.locale("id").format('LL');
    $scope.hotel.checkoutDate = moment().locale("id").add(7, 'days');
    $scope.hotel.adultCount = 2;
    $scope.hotel.childCount = 1;
    $scope.hotel.nightCount = 2;
    $scope.hotel.roomCount = 1;

    $scope.hotel.adultCountMin = 1;
    $scope.hotel.adultCountMax = 5;

    $scope.hotel.childCountMin = 0;
    $scope.hotel.childCountMax = 4;

    $scope.hotel.nightCountMin = 1;
    $scope.hotel.nightCountMax = 7;

    $scope.hotel.roomCountMin = 1;
    $scope.hotel.roomCountMax = 8;

    $scope.addValue = function (variableName, amount) {
        var minCount = 1;
        if (variableName == 'adultCount') {
            $scope.hotel.adultCount = $scope.hotel.adultCount + amount;
            if ($scope.hotel.adultCount < $scope.hotel.adultCountMin) $scope.hotel.adultCount++;
            else if ($scope.hotel.adultCount > $scope.hotel.adultCountMax) $scope.hotel.adultCount--;
        }
        else if (variableName == 'childCount') {
            $scope.hotel.childCount = $scope.hotel.childCount + amount;
            if ($scope.hotel.childCount < $scope.hotel.childCountMin) $scope.hotel.childCount++;
            else if ($scope.hotel.childCount > $scope.hotel.childCountMax) $scope.hotel.childCount--;
        }
        else if (variableName == 'nightCount') {
            $scope.hotel.nightCount = $scope.hotel.nightCount + amount;
            if ($scope.hotel.nightCount < $scope.hotel.nightCountMin) $scope.hotel.nightCount++;
            else if ($scope.hotel.nightCount > $scope.hotel.nightCountMax) $scope.hotel.nightCount--;
        }
        else if (variableName == 'roomCount') {
            $scope.hotel.roomCount = $scope.hotel.roomCount + amount;
            if ($scope.hotel.roomCount < $scope.hotel.roomCountMin) $scope.hotel.roomCount++;
            else if ($scope.hotel.roomCount > $scope.hotel.roomCountMax) $scope.hotel.roomCount--;
        }
    }

    var resource = $resource('//api.local.travorama.com/v1/autocomplete/hotel/:prefix',
           { prefix: '@prefix'},
           {
               query: {
                   method: 'GET',
                   params: { },
                   isArray: false
               }
           }
       );

    $scope.$watch('hotel.location', function (newValue, oldValue, ccc) {
        if (newValue.length >= 3) {
            resource.query({ prefix: newValue }).$promise.then(function (data) {
                $log.debug(data);
                $scope.hotel.hotelAutocomplete = data.hotelAutocomplete;
            });
        }
    });

    


    $scope.hotel.searchHotel = function (){
        $log.debug('searching hotel');
        location.href = '/id/Hotel/Search/' + $scope.hotel.searchParam();
        //$http({
        //   url: "/id/Hotel/Search", 
        //   method: "GET",
        //   params: {aaa : "kode123", bbb : "silubab" }
        //   })
    };

    $scope.hotel.searchParam = function (){
        return ("?info=" + 
            //[$scope.hotel.location,
            ["16121",
             $scope.hotel.checkinDate.toISOString(),
             $scope.hotel.checkoutDate.toISOString(),
             $scope.hotel.adultCount,
             $scope.hotel.childCount,
             $scope.hotel.nightCount,
             $scope.hotel.roomCount].join('.')
        )
    }


    $scope.setCheckinDate = function (scope, date) {
        scope.$apply(function () {
            scope.hotel.checkinDate = moment(date, "MM-DD-YYYY");
            scope.hotel.checkinDateDisplay = $scope.hotel.checkinDate.locale("id").format('LL');
        });
    }

    $('.hotel-date-picker').datepicker({
        numberOfMonths: 1,
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
        }
    });
    //=============== hotel end ======================
    
}]);

// Calendar 2016 Controller
app.controller('campaignController', [
    '$scope', function ($scope) {
        
    }
]);