// home controller
app.controller('B2BFlightSearchFormController', ['$scope', '$log', '$http', '$location', '$resource', '$timeout', function ($scope, $log, $http, $location, $resource, $timeout) {
    $(document).ready(function () {

        $.getScript("js.cookie.js", function () { });

        if (Cookies.get('origin')) {
            $scope.searchParam.OriginAirport = Cookies.get('origin');
        } else {
            $scope.searchParam.OriginAirport = 'CGK';
        }

        if (Cookies.get('originCity')) {
            $scope.searchParam.OriginCity = Cookies.get('originCity');
        } else {
            $scope.searchParam.OriginCity = 'Jakarta';
        }

        if (Cookies.get('destination')) {
            $scope.searchParam.DestinationAirport = Cookies.get('destination');
        } else {
            $scope.searchParam.DestinationAirport = 'DPS';
        }

        if (Cookies.get('destinationCity')) {
            $scope.searchParam.DestinationCity = Cookies.get('destinationCity');
        } else {
            $$scope.searchParam.DestinationCity = 'Denpasar';
        }

        if (Cookies.get('departure')) {
            $scope.searchParam.DepartureDate = new Date(Cookies.get('departure'));
        } else {
            $scope.searchParam.DepartureDate = new Date();
        }

        if (Cookies.get('return')) {
            $scope.searchParam.ReturnDate = new Date(Cookies.get('return'));
        } else {
            $scope.searchParam.ReturnDate = moment().locale("id").add(1, 'days');
        }

        $('#originPlace').val(($scope.searchParam.OriginCity + " (" + $scope.searchParam.OriginAirport + ")"));
        $('#destinationPlace').val(($scope.searchParam.DestinationCity + " (" + $scope.searchParam.DestinationAirport + ")"));

        var valDepDate = $scope.setDateWriting($scope.searchParam.DepartureDate.getDate(),
                $scope.searchParam.DepartureDate.getMonth(), $scope.searchParam.DepartureDate.getFullYear());
        var valRetDate = $scope.setDateWriting($scope.searchParam.ReturnDate.getDate(),
            $scope.searchParam.ReturnDate.getMonth(), $scope.searchParam.ReturnDate.getFullYear());
        $('#departureDate').val(valDepDate);
        $('#returnDate').val(valRetDate);

        $('#departureDate').datepicker('setStartDate', new Date(todayDate.getFullYear(), todayDate.getMonth(), todayDate.getDate()));
        $('#returnDate').datepicker('setStartDate', new Date($scope.searchParam.DepartureDate.getFullYear(),
            $scope.searchParam.DepartureDate.getMonth(), $scope.searchParam.DepartureDate.getDate()));

        if (Cookies.get('type')) {
            if (Cookies.get('type') == 'OneWay') {
                $scope.OneWay = true;
                $('#yes').prop('checked', true);
                $('#no').prop('checked', false);
                $('.retDate').addClass('disabled');
            } else {
                $scope.OneWay = false;
                $('#no').prop('checked', true);
                $('#yes').prop('checked', false);
                $('.retDate').removeClass('disabled');
            }
        } else {
            $scope.OneWay = true;
            $('#yes').prop('checked', true);
            $('#no').prop('checked', false);
            $('.retDate').addClass('disabled');
        }
        
        if (Cookies.get('adult')) {
            $scope.Passenger.Adult = parseInt(Cookies.get('adult'));
        } else {
            $scope.Passenger.Adult = 1;
        }

        if (Cookies.get('child')) {
            $scope.Passenger.Child = parseInt(Cookies.get('child'));
        } else {
            $scope.Passenger.Child = 0;
        }

        if (Cookies.get('infant')) {
            $scope.Passenger.Infant = parseInt(Cookies.get('infant'));
        } else {
            $scope.Passenger.Infant = 0;
        }

        $('.adultPax').val($scope.Passenger.Adult);
        $(".adultPax .cs-placeholder").text($scope.Passenger.Adult);
        $(".childPax .cs-placeholder").text($scope.Passenger.Child);
        $(".infantPax .cs-placeholder").text($scope.Passenger.Infant);
        $('.childPax').val($scope.Passenger.Child);
        $('.infantPax').val($scope.Passenger.Infant);
        if (Cookies.get('cabin')) {
            $scope.searchParam.CabinClass = Cookies.get('cabin');
        } else {
            $scope.searchParam.CabinClass = 'y';
        }
        $('.pax').val($scope.searchParam.CabinClass);
        var result = $scope.cabinClass.filter(function (obj) {
            return obj.value == $scope.searchParam.CabinClass;
        })[0].name;
        $(".pax .cs-placeholder").text(result);
    });

    $scope.Passenger = {
        Adult: '',
        Child: '',
        Infant: '',
        PaxAmtAdult: [1, 2, 3, 4, 5, 6, 7, 8, 9],
        PaxAmtElse: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]
    }

    $scope.OneWay = false;

    $("input[name=triptype]").change(function () {
        if (this.value == 'OneWay') {
            $scope.OneWay = true;
            $('.retDate').addClass('disabled');
        } else {
            $scope.OneWay = false;
            $('.retDate').removeClass('disabled');
        }
    });
    
    $scope.setDateWriting = function (day, month, year) {
        return day.toString() + ' ' + $scope.getMonthName(month) + ' ' + year.toString();
    }

    $scope.getMonthName = function (month) {
        switch (month) {
            case 0:
                return 'January';
            case 1:
                return 'February';
            case 2:
                return 'March';
            case 3:
                return 'April';
            case 4:
                return 'May';
            case 5:
                return 'June';
            case 6:
                return 'July';
            case 7:
                return 'August';
            case 8:
                return 'October';
            case 9:
                return 'November';
            case 10:
                return 'October';
            case 11:
                return 'December';
        }
    }

    $scope.getMonthNumber = function (month) {
        switch (month) {
            case 'January':
                return 0;
            case 'February':
                return 1;
            case 'March':
                return 2;
            case 'April':
                return 3;
            case 'May':
                return 4;
            case 'June':
                return 5;
            case 'July':
                return 6;
            case 'August':
                return 7;
            case 'September':
                return 8;
            case 'October':
                return 9;
            case 'November':
                return 10;
            case 'December':
                return 11;
        }
    }
    
    var todayDate = new Date();
    var tmrwDate = new Date(new Date().getTime() + 24 * 60 * 60 * 1000);
    
    $scope.searchParam = {
        DepartureDate: todayDate,
        ReturnDate: tmrwDate,
        OriginCity: 'Jakarta',
        OriginAirport: 'JKT',
        DestinationCity: 'Denpasar (Bali)',
        DestinationAirport: 'DPS',
        Origin:'',
        Destination: '',
        CabinClass:'y'
    }
    
    $('.search-location').hide();

    $scope.$watch('Passenger.Adult',
        function (newValue) {
            newValue = parseInt(newValue);
            var TotalPax = $scope.Passenger.Child + $scope.Passenger.Infant;
            if (9 - TotalPax < newValue) {
                alert('Jumlah penumpang tidak boleh lebih dari sembilan orang');
                $scope.Passenger.Adult = 9 - TotalPax;
                $('.adultPax').val(9 - TotalPax);
            } else {
                $scope.Passenger.Adult = newValue;
            }         
        });

    $scope.$watch('Passenger.Child',
        function (newValue) {
            newValue = parseInt(newValue);
            var TotalPax = $scope.Passenger.Adult + $scope.Passenger.Infant;
            if (9 - TotalPax < newValue) {
                alert('Jumlah penumpang tidak boleh lebih dari sembilan orang');
                $scope.Passenger.Child = 9 - TotalPax;
                $('.childPax').val(9 - TotalPax);
            } else {
                $scope.Passenger.Child = newValue;
            }
        });

    $scope.$watch('Passenger.Infant',
        function (newValue) {
            newValue = parseInt(newValue);
            if (newValue > $scope.Passenger.Adult) {
                alert('Jumlah bayi tidak boleh lebih dari penumpang dewasa');
                $('.infantPax').val($scope.Passenger.Infant);
                $scope.Passenger.Infant = $scope.Passenger.Adult;
            } else {
                var TotalPax = $scope.Passenger.Adult + $scope.Passenger.Child;
                if (9 - TotalPax < newValue) {
                    alert('Jumlah penumpang tidak boleh lebih dari sembilan orang');
                    $scope.Passenger.Infant = 9 - TotalPax;
                    $('.infantPax').val(9 - TotalPax);
                } else {
                    $scope.Passenger.Infant = newValue;
                }
            }
        });

    $('#originPlace, #destinationPlace').click(function() {
        $(this).select();
    });

    $('#originPlace').keyup(function (evt) {
        if (evt.keyCode == 27) {
            hideLocation();
        } else {
            if ($(this).val().length >= 3) {
                $('.search-location .location-recommend').hide();
                $('.search-location .location-search').show();
                getLocation($(this).val());
            } else {
                $('.search-location .location-recommend').hide();
                $('.search-location .location-search').show();
                $('.search-location .location-search .autocomplete-pre .text-pre').show();
                $('.search-location .location-search .autocomplete-result').hide();
                $('.search-location .location-search .autocomplete-no-result').hide();
            }
        }
    });

    $('#originPlace').keydown(function (evt) {
        if (evt.keyCode == 9 || evt.which == 9) {
            evt.preventDefault();
        }
    });

    $('#originPlace').focusout(function () {
        $(this).val(($scope.searchParam.OriginCity + " (" + $scope.searchParam.OriginAirport + ")"));
    });

    $('#destinationPlace').keyup(function (evt) {
        if (evt.keyCode == 27) {
            hideLocation();
        } else {
            if ($(this).val().length >= 3) {
                $('.search-location .location-recommend').hide();
                $('.search-location .location-search').show();
                getLocation($(this).val());
            } else {
                $('.search-location .location-recommend').hide();
                $('.search-location .location-search').show();
                $('.search-location .location-search .autocomplete-pre .text-pre').show();
                $('.search-location .location-search .autocomplete-result').hide();
                $('.search-location .location-search .autocomplete-no-result').hide();
            }
        }
    });
    $('#destinationPlace').keydown(function (evt) {
        if (evt.keyCode == 9 || evt.which == 9) {
            evt.preventDefault();
        }
    });
    $('#destinationPlace').focusout(function (evt) {
        $(this).val(($scope.searchParam.DestinationCity + " (" + $scope.searchParam.DestinationAirport + ")"));
    });

    var trial = 0;
    function generateSearchResult(list) {
        $('.autocomplete-result ul').empty();
        for (var i = 0 ; i < list.length; i++) {
            $('.autocomplete-result ul').append('<li href="#" data-code="' + list[i].code + '" data-city="' + list[i].city + '">' + list[i].city + ' (' + list[i].code + '), ' + list[i].name + ', ' + list[i].country + '</li>');
        }
    }

    $('.close-location').click(function() {
        $('.search-location').hide();
    });
    function getLocation(keyword) {
        if (trial > 3) {
            trial = 0;
        }
        $.ajax({
            url: FlightAutocompleteConfig.Url + keyword,
            headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
        }).done(function (returnData) {
            $('.autocomplete-pre .text-pre').hide();
            $('.autocomplete-pre .text-loading').hide();
            var result = returnData.airports;
            console.log(returnData);
            generateSearchResult(result);
            if (returnData.airports.length > 0) {
                $('.autocomplete-no-result').hide();
                $('.autocomplete-pre .text-loading').hide();
                $('.autocomplete-result').show();
            } else {
                $('.autocomplete-pre .text-loading').hide();
                $('.autocomplete-result').hide();
                $('.autocomplete-no-result').show();
            }
        }).error(function () {
            trial++;
            if (refreshAuthAccess() && trial < 4) //refresh cookie
            {
                getLocation(keyword);
            }
        });       
    }

    $scope.recommendation = '';
    $scope.showLocation = function(text) {
        $('.search-location').show();
        $scope.recommendation = text;
        showLocation(text);
    }

    function showLocation(place) {
        place = place || $('.search-location').attr('data-place');
        $('.search-location .location-recommend').show();
        $('.search-location .location-search').hide();
        if (place == 'origin') {
            $('.search-location .location-header .origin').removeClass('hidden');
            $('.search-location .location-header .destination').addClass('hidden');
        } else {
            $('.search-location .location-header .origin').addClass('hidden');
            $('.search-location .location-header .destination').removeClass('hidden');
        }
        $('.search-location').attr('data-place', place);
        $('.search-location').attr('id', place);
        $('.search-location').show();
    }
    $('.search-location .location-recommend .nav-click.prev').click(function (evt) {
        evt.preventDefault();
        if (parseInt($('.search-location .location-recommend .tab-header nav ul').css('margin-left')) < 0) {
            $('.search-location .location-recommend .tab-header nav ul').css('margin-left', '+=135px');
        }
    });
    $('.search-location .location-recommend .nav-click.next').click(function (evt) {
        evt.preventDefault();
        if (parseInt($('.search-location .location-recommend .tab-header nav ul').css('margin-left')) > -(135 * ($('.search-location .location-recommend .tab-header nav ul li').length - 4))) {
            $('.search-location .location-recommend .tab-header nav ul').css('margin-left', '-=135px');
        }
    });
    $('.search-location .location-recommend nav ul li ').click(function () {
        var showClass = $(this).attr('data-show');
        $(this).addClass('active');
        $(this).siblings().removeClass('active');
        $('.search-location .location-recommend .tab-content>div').removeClass('active');
        $('.search-location .location-recommend .tab-content>div.' + showClass).addClass('active');
    });
    $('.search-location .location-recommend .tab-content a').click(function (evt, sharedProperties) {
        evt.preventDefault();
        var locationCode = $(this).attr('data-code');
        var locationCity = $(this).text();
        if ($scope.recommendation == 'origin') {
            if (locationCity != $scope.searchParam.DestinationCity) {
                $scope.searchParam.OriginAirport = locationCode;
                $scope.searchParam.OriginCity = locationCity;
                $scope.searchParam.Origin = $(this).text() + ' (' + locationCode + ')';
                $('#originPlace').val($(this).text() + ' (' + locationCode + ')');
            } else {
                alert('Kota Asal dan Tujuan Tidak Boleh Sama');
            }

        } else {
            if (locationCity != $scope.searchParam.OriginCity) {
                $scope.searchParam.DestinationAirport = locationCode;
                $scope.searchParam.DestinationCity = locationCity;
                $('#destinationPlace').val($(this).text() + ' (' + locationCode + ')');
                $('.flight-submit-button').removeClass('disabled');
            } else {
                alert('Kota Asal dan Tujuan Tidak Boleh Sama');
            }
        }
        hideLocation();
    });

    $('.autocomplete-result ul').on('click', 'li', function () {
        var locationCode = $(this).attr('data-code');
        var locationCity = $(this).attr('data-city');
        if ($scope.recommendation == 'origin') {
            if (locationCity != $scope.searchParam.DestinationCity) {
                $scope.searchParam.OriginAirport = locationCode;
                $scope.searchParam.OriginCity = locationCity;
                $scope.searchParam.Origin = $(this).text() + ' (' + locationCode + ')';
                $('#originPlace').val($(this).text());
            } else {
                alert('Kota Asal dan Tujuan Tidak Boleh Sama');
                $('.flight-submit-button').addClass('disabled');
            }

        } else {
            if (locationCity != $scope.searchParam.OriginCity) {
                $scope.searchParam.DestinationAirport = locationCode;
                $scope.searchParam.DestinationCity = locationCity;
                $('#destinationPlace').val($(this).text());
                $('.flight-submit-button').removeClass('disabled');
            } else {
                alert('Kota Asal dan Tujuan Tidak Boleh Sama');
                $('.flight-submit-button').addClass('disabled');
            }
        }
        hideLocation();
    });
    function hideLocation() {
        $('.search-location').hide();
    }

    $scope.selectCalendar = '';
    $scope.setOptionCalendar = function(target) {
        $scope.selectCalendar = target;
    }
    
    $('#departureDate').change(function () {
        var date = $('#departureDate').val().split(' ');
        var day = parseInt(date[0]);
        var month = (date[1]);
        var year = parseInt(date[2]);
        var selectedDate = new Date(year, $scope.getMonthNumber(month), day);
        if ($scope.OneWay) {
            $scope.searchParam.DepartureDate = new Date(year, $scope.getMonthNumber(month), day);
            if ($scope.searchParam.ReturnDate < $scope.searchParam.DepartureDate) {
                $scope.searchParam.ReturnDate = $scope.searchParam.DepartureDate;
                $('#returnDate').datepicker('update', $scope.searchParam.ReturnDate);               
            }
        } else {
            if (selectedDate > $scope.searchParam.ReturnDate) {
                $scope.searchParam.DepartureDate = new Date(year, $scope.getMonthNumber(month), day);
                $scope.searchParam.ReturnDate = selectedDate;
                $('#returnDate').datepicker('update', selectedDate);
            } else {
                $scope.searchParam.DepartureDate = new Date(year, $scope.getMonthNumber(month), day);
            }
        }
        var valDepDate = $scope.setDateWriting($scope.searchParam.DepartureDate.getDate(),
            $scope.searchParam.DepartureDate.getMonth(), $scope.searchParam.DepartureDate.getFullYear());
        var valRetDate = $scope.setDateWriting($scope.searchParam.ReturnDate.getDate(),
            $scope.searchParam.ReturnDate.getMonth(), $scope.searchParam.ReturnDate.getFullYear());
        $('#valueDepDate').val(valDepDate);
        $('#valueRetDate').val(valRetDate);
        $(this).datepicker('hide');

        $('#returnDate').datepicker('setStartDate', new Date($scope.searchParam.DepartureDate.getFullYear(),
            $scope.searchParam.DepartureDate.getMonth(), $scope.searchParam.DepartureDate.getDate()));
    });

    $('#returnDate').change(function () {
        var date = $('#returnDate').val().split(' ');
        var day = parseInt(date[0]);
        var month = (date[1]);
        var year = parseInt(date[2]);
        var selectedDate = new Date(year, $scope.getMonthNumber(month), day);
        if (selectedDate < $scope.searchParam.DepartureDate) {
            $scope.searchParam.DepartureDate = selectedDate;
            $('#departureDate').datepicker('update', selectedDate);
            $scope.searchParam.ReturnDate = selectedDate;
        } else {
            $scope.searchParam.ReturnDate = selectedDate;
        }
        var valDepDate = $scope.setDateWriting($scope.searchParam.DepartureDate.getDate(),
            $scope.searchParam.DepartureDate.getMonth(), $scope.searchParam.DepartureDate.getFullYear());
        var valRetDate = $scope.setDateWriting($scope.searchParam.ReturnDate.getDate(),
            $scope.searchParam.ReturnDate.getMonth(), $scope.searchParam.ReturnDate.getFullYear());
        $('#valueDepDate').val(valDepDate);
        $('#valueRetDate').val(valRetDate);
        $(this).datepicker('hide');

        $('#returnDate').datepicker('setStartDate', new Date($scope.searchParam.DepartureDate.getFullYear(),
            $scope.searchParam.DepartureDate.getMonth(), $scope.searchParam.DepartureDate.getDate()));
    });

    $('#valueDepDate').click(function (){
        $('#departureDate').datepicker('show');
    });
    $('#valueRetDate').click(function () {
        $('#returnDate').datepicker('show');
    });

    $scope.cabinClass = [
    { name: 'Ekonomi', value: 'y' },
    { name: 'Bisnis', value: 'c' },
    { name: 'Utama', value: 'f' }];

    $scope.gotoSearchResult = function() {
        var url = '/id/tiket-pesawat/cari/';
        $scope.searchParam.OriginCity = $scope.searchParam.OriginCity.replace(/\s+/g, '-');
        $scope.searchParam.OriginCity = $scope.searchParam.OriginCity.replace(/[^0-9a-zA-Z-]/gi, '');
        $scope.searchParam.DestinationCity = $scope.searchParam.DestinationCity.replace(/\s+/g, '-');
        $scope.searchParam.DestinationCity = $scope.searchParam.DestinationCity.replace(/[^0-9a-zA-Z-]/gi, '');

        url += $scope.searchParam.OriginCity + '-' + $scope.searchParam.DestinationCity + '-' +
            $scope.searchParam.OriginAirport + '-' + $scope.searchParam.DestinationAirport + '/';

        var searchParam;
        if ($scope.OneWay) {
            searchParam = $scope.searchParam.OriginAirport + $scope.searchParam.DestinationAirport +
                ("0" + $scope.searchParam.DepartureDate.getDate()).slice(-2) + ("0" + ($scope.searchParam.DepartureDate.getMonth() + 1)).slice(-2) +
                $scope.searchParam.DepartureDate.getFullYear().toString().slice(-2) + '-' + $scope.Passenger.Adult.toString() + $scope.Passenger.Child.toString()
                + $scope.Passenger.Infant.toString() + $scope.searchParam.CabinClass;
        } else {
            searchParam = $scope.searchParam.OriginAirport + $scope.searchParam.DestinationAirport +
                ("0" + $scope.searchParam.DepartureDate.getDate()).slice(-2) + ("0" + ($scope.searchParam.DepartureDate.getMonth() + 1)).slice(-2) +
                $scope.searchParam.DepartureDate.getFullYear().toString().slice(-2) + '~' + $scope.searchParam.DestinationAirport +
                $scope.searchParam.OriginAirport + ("0" + $scope.searchParam.ReturnDate.getDate()).slice(-2) + ("0" + ($scope.searchParam.ReturnDate.getMonth() + 1)).slice(-2) +
                $scope.searchParam.ReturnDate.getFullYear().toString().slice(-2) +
                '-' + $scope.Passenger.Adult.toString() + $scope.Passenger.Child.toString()
                + $scope.Passenger.Infant.toString() + $scope.searchParam.CabinClass;
        }

        url += searchParam;
        setCookie();
        window.location.href = url;
    };

    // set cookie
    function setCookie() {
        var triptype;
        if ($scope.OneWay) {
            triptype = 'OneWay';
        } else {
            triptype = 'Return';
        }
        Cookies.set('origin', $scope.searchParam.OriginAirport, { expires: 9999 });
        Cookies.set('originCity', $scope.searchParam.OriginCity, { expires: 9999 });
        Cookies.set('destination', $scope.searchParam.DestinationAirport, { expires: 9999 });
        Cookies.set('destinationCity', $scope.searchParam.DestinationCity, { expires: 9999 });
        Cookies.set('departure', $scope.searchParam.DepartureDate, { expires: 9999 });
        Cookies.set('return', $scope.searchParam.ReturnDate, { expires: 9999 });
        Cookies.set('type', triptype, { expires: 9999 });
        Cookies.set('adult', $scope.Passenger.Adult, { expires: 9999 });
        Cookies.set('child', $scope.Passenger.Child, { expires: 9999 });
        Cookies.set('infant', $scope.Passenger.Infant, { expires: 9999 });
        Cookies.set('cabin', $scope.searchParam.CabinClass, { expires: 9999 });
    }
}]);

// home controller
app.controller('B2BHotelSearchFormController', ['$scope', '$log', '$http', '$location', '$resource', '$timeout', 'hotelSearchSvc',
    function ($scope, $log, $http, $location, $resource, $timeout, hotelSearchSvc) {
    $(document).ready(function () {

        $.getScript("js.cookie.js", function () { });
        
        if (Cookies.get('hotelSearchLocationDisplay')) {
            $scope.hotelSearch.locationDisplay = Cookies.get('hotelSearchLocationDisplay');
        } else {
            $scope.hotelSearch.locationDisplay = 'Bali, Indonesia';
        }

        if (Cookies.get('hotelSearchLocation')) {
            $scope.hotelSearch.location = Cookies.get('hotelSearchLocation');
        } else {
            $scope.hotelSearch.location = 16173;
        }

        if (Cookies.get('urlCountry')) {
            $scope.hotelSearch.urlData.country = Cookies.get('urlCountry');
        } else {
            $scope.hotelSearch.urlData.country = 'Indonesia';
        }

        if (Cookies.get('urlDestination')) {
            $scope.hotelSearch.urlData.destination = Cookies.get('urlDestination');
        } else {
            $scope.hotelSearch.urlData.destination = 'Bali';
        }

        if (Cookies.get('urlZone')) {
            $scope.hotelSearch.urlData.zone = Cookies.get('urlZone');
        } else {
            $scope.hotelSearch.urlData.zone = null;
        }

        if (Cookies.get('urlArea')) {
            $scope.hotelSearch.urlData.area = Cookies.get('urlArea');
        } else {
            $scope.hotelSearch.urlData.area = null;
        }

        if (Cookies.get('urlType')) {
            $scope.hotelSearch.urlData.type = Cookies.get('urlType');
        } else {
            $scope.hotelSearch.urlData.type = 'Destination';
        }

        if (Cookies.get('hotelSearchCheckInDate')) {
            $scope.hotelSearch.checkinDate = new Date(Cookies.get('hotelSearchCheckInDate'));
            $scope.hotelSearch.checkinDateDisplay = moment($scope.hotelSearch.checkinDate).locale("id").format('LL');
            $('.ui-datepicker.checkindate').datepicker("setDate", new Date($scope.hotelSearch.checkinDateDisplay));
        } else {
            $scope.hotelSearch.checkinDate = moment().locale("id").add(5, 'days');
            $scope.hotelSearch.checkinDateDisplay = moment($scope.hotelSearch.checkinDate).locale("id").format('LL');
        }

        var valCheckInDate = $scope.setDateWriting($scope.hotelSearch.checkinDate.getDate(),
            $scope.hotelSearch.checkinDate.getMonth(), $scope.hotelSearch.checkinDate.getFullYear());
        $('#checkInDate').val(valCheckInDate);
        $('#checkInDate').datepicker('setStartDate', new Date(todayDate.getFullYear(), todayDate.getMonth(), todayDate.getDate()));

        if (Cookies.get('hotelSearchCheckOutDate')) {
            $scope.hotelSearch.checkoutDate = new Date(Cookies.get('hotelSearchCheckOutDate'));
            $scope.hotelSearch.checkoutDateDisplay = moment($scope.hotelSearch.checkoutDate).locale("id").format('LL');
        } else {
            $scope.hotelSearch.checkoutDate = moment().locale("id").add(7, 'days');
            $scope.hotelSearch.checkoutDateDisplay = moment($scope.hotelSearch.checkoutDate).locale("id").format('LL');
        }

        if (Cookies.get('hotelSearchNights')) {
            $scope.hotelSearch.nightCount = Cookies.get('hotelSearchNights');
        } else {
            $scope.hotelSearch.nightCount = 2;
        }

        $(".nightCount .cs-placeholder").text($scope.hotelSearch.nightCount);
        $(".nightCount").val($scope.hotelSearch.nightCount);
        if (Cookies.get('hotelSearchRooms')) {
            $scope.hotelSearch.roomCount = Cookies.get('hotelSearchRooms');
        } else {
            $scope.hotelSearch.roomCount = 1;
        }
        $(".roomCount .cs-placeholder").text($scope.hotelSearch.roomCount);
        $(".roomCount").val($scope.hotelSearch.roomCount);
        if (Cookies.getJSON('hotelSearchOccupancies')) {
            var occupancies = Cookies.getJSON('hotelSearchOccupancies');
            $scope.hotelSearch.occupancies = [];   
            for (var f = 0; f < occupancies.length; f++) {
                $scope.hotelSearch.occupancies.push({
                    adultCount: parseInt(occupancies[f].adultCount),
                    childCount: parseInt(occupancies[f].childCount),
                    childrenAges: []
                });
                for (var g = 0; g < 4; g++) {
                    $scope.hotelSearch.occupancies[f].childrenAges.push(parseInt(occupancies[f].childrenAges[g]));
                }
            }
        } else {
            $scope.hotelSearch.occupancies = [];
            for (var i = 0; i <= 7; i++) {
                $scope.hotelSearch.occupancies.push({
                    adultCount: 1,
                    childCount: 0,
                    childrenAges: [0, 0, 0, 0]
                });
            }
        }

        for (var x = 0; x < $scope.hotelSearch.occupancies.length; x++) {
            $(".occ-adult-" + x + " .cs-placeholder").text($scope.hotelSearch.occupancies[x].adultCount);
            $(".occ-adult-" + x).val($scope.hotelSearch.occupancies[x].adultCount);
            $(".occ-child-" + x + " .cs-placeholder").text($scope.hotelSearch.occupancies[x].childCount);
            $(".occ-child-" + x).val($scope.hotelSearch.occupancies[x].childCount);
            for (var y = 0; y < 4; y++) {
                $(".occ-age-" + x + "-" + y + " .cs-placeholder").text($scope.hotelSearch.occupancies[x].childrenAges[y]);
                $(".occ-age-" + x + "-" + y).val($scope.hotelSearch.occupancies[x].childrenAges[y]);
            }
        }
    });

    //$.fn.datepicker.defaults.language = 'id';
    var todayDate = new Date();
    $scope.pageLoaded = true;
    $scope.setDateWriting = function (day, month, year) {
        return day.toString() + ' ' + $scope.getMonthName(month) + ' ' + year.toString();
    }

    $scope.searchParam = {
        NightList: [1, 2, 3, 4, 5, 6, 7],
        RoomList: [1, 2, 3, 4, 5, 6, 7, 8],
        AdultList: [1, 2, 3, 4, 5],
        ChildList: [0, 1, 2, 3, 4],
        ChildAgeList: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11]
    }

    $scope.view = {
        showHotelSearch: false
    }

    $scope.showPopularDestinations = false;
    $scope.autocompletePre = false;
    $('.search-hotel').hide();
    hotelSearchSvc.initializeSearchForm($scope);
    $scope.getMonthName = function (month) {
        switch (month) {
            case 0:
                return 'January';
            case 1:
                return 'February';
            case 2:
                return 'March';
            case 3:
                return 'April';
            case 4:
                return 'May';
            case 5:
                return 'June';
            case 6:
                return 'July';
            case 7:
                return 'August';
            case 8:
                return 'October';
            case 9:
                return 'November';
            case 10:
                return 'October';
            case 11:
                return 'December';
        }
    }

    $scope.getMonthNumber = function (month) {
        switch (month) {
            case 'January':
                return 0;
            case 'February':
                return 1;
            case 'March':
                return 2;
            case 'April':
                return 3;
            case 'May':
                return 4;
            case 'June':
                return 5;
            case 'July':
                return 6;
            case 'August':
                return 7;
            case 'September':
                return 8;
            case 'October':
                return 9;
            case 'November':
                return 10;
            case 'December':
                return 11;
        }
    }

    $('.hotel-location').click(function (evt) {
        $(this).select();
        evt.stopPropagation();
        $scope.view.showHotelSearch = true;
        $scope.showPopularDestinations = true;
        $('.search-hotel').show();
        $('.hotel-autocomplete-pre').hide();
        $('.hotel-autocomplete-no-result').hide();
    });

    $('.hotel-location').keyup(function () {
        $scope.showPopularDestinations = false;
        var value = $('.hotel-location').val();
        if (value.length > 0 && value.length < 3) {
            $('.hotel-autocomplete-result.popular').hide();
            $('.hotel-autocomplete-pre').show();
            $('.hotel-autocomplete-pre .typing').show();
            $('.hotel-autocomplete-pre .searching').hide();
            $('.hotel-autocomplete-result.api-result').show();
        } else if (value.length >= 3) {
            $('.hotel-autocomplete-result.popular').hide();
            $('.hotel-autocomplete-pre').hide();
            $('.hotel-autocomplete-result.api-result').show();
        } else {
            $('.hotel-autocomplete-pre').hide();
            $('.hotel-autocomplete-result.popular').show();
        }
        
    });
    //hideHotel hotel
    function hideHotel() {
        $scope.view.showHotelSearch = false;
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
    
    
    $('#checkInDate').change(function () {
        var date = $('#checkInDate').val().split('/');
        var day = parseInt(date[0]);
        var month = parseInt(date[1]);
        var year = parseInt(date[2]);
        var selectedDate = new Date(year, month, day);
        $scope.hotelSearch.checkinDate = selectedDate;
        var checkOutDate= new Date(selectedDate);
        $scope.hotelSearch.checkoutDate = checkOutDate.setDate(checkOutDate.getDate() + $scope.hotelSearch.nightCount);
        var valCheckInDate = $scope.setDateWriting($scope.hotelSearch.checkinDate.getDate(),
            $scope.hotelSearch.checkinDate.getMonth(), $scope.hotelSearch.checkinDate.getFullYear());
        $('#valueCheckInDate').val(valCheckInDate);
        $(this).datepicker('hide');
    });
    }]);

app.controller('B2BLoginController', ['$scope', '$log', '$http', '$location', '$resource', '$timeout', 
    function ($scope, $log, $http, $location, $resource, $timeout) {
       
}]);