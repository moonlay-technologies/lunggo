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
        var date = $('#checkInDate').val().split(' ');
        var day = parseInt(date[0]);
        var month = parseInt($scope.getMonthNumber(date[1]));
        var year = parseInt(date[2]);
        var selectedDate = new Date(year, month, day);
        $scope.hotelSearch.checkinDate = selectedDate;
        $scope.hotelSearch.checkoutDate = moment($scope.hotelSearch.checkinDate).add($scope.hotelSearch.nightCount, 'days');
        var valCheckInDate = $scope.setDateWriting($scope.hotelSearch.checkinDate.getDate(),
            $scope.hotelSearch.checkinDate.getMonth(), $scope.hotelSearch.checkinDate.getFullYear());
        $('#valueCheckInDate').val(valCheckInDate);
        $(this).datepicker('hide');
    });
}]);

app.controller('B2BPaymentMgmtFormController', ['$scope', '$log', '$http', function ($scope, $log, $http) {
    $scope.pageLoaded = true;
    $scope.creditCards = [];
    $scope.ccType = function(digit) {
        if (digit.slice(0,1) == '4') {
            return 'Visa';
        }else if (digit.slice(0,1) == '5') {
            return 'MasterCard';
        } else if (digit.slice(0, 2) == '34' || digit.slice(0, 2) == '37') {
            return 'American Express';
        }else {
            return '';
        }
    }

    $scope.emptyCc = false;
    $scope.getSavedCc = function () {
        var authAccess = getAuthAccess();
        if (authAccess == 2) {
            $http({
                method: 'GET',
                url: GetCreditCardConfig.Url,
                headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') },
                async: false
            }).then(function(returnData) {
                if (returnData != null) {
                    if (returnData.data != null) {
                        $scope.creditCards = returnData.data.creditCards;
                        if ($scope.creditCards != null && $scope.creditCards.length > 0) {
                            for (var i = 0; i < $scope.creditCards.length; i++) {
                                $scope.creditCards[i].cardExpiry = new Date($scope.creditCards[i].cardExpiry);
                                $scope.creditCards[i].type = $scope.ccType($scope.creditCards[i].maskedCardNumber);
                                if ($scope.creditCards[i].isPrimaryCard) {
                                    $scope.currentPrimary = i;
                                }
                            }
                        } else {
                            $scope.emptyCc = true;
                        }
                    } else {
                        //show blank
                    }
                } else {
                    //show blank
                }

            }).catch(function() {
            });
        }
    }

    $scope.getSavedCc();

    $('body .remove-payment').hide();
    var cloneIndex = $(".payment-item").length;

    function clone() {
        $(".newCc").removeClass("newCc");
        cloneIndex = $(".payment-item").length;
        if (cloneIndex == 1) {
            var isEmpty = $('#payment-item-1').hasClass("emptyCc") == true;
            if (isEmpty) {
                $("#payment-item-1").removeClass("hide");
            }
        } else {
            cloneIndex++;
            $(this).closest('.payment-body').find('#payment-item-1').clone(false, false)
                .insertAfter($('[id^="payment-item-1"]').last())
                .attr('id', "payment-item-" + cloneIndex)
                .attr('count', cloneIndex - 1).addClass("newCc");
            $('.newCc .cc-no').text('New Card Number');
            $('.newCc .btn-edit-payment').removeClass("hide");
            $('.newCc .expiry').text('Expires mm/yy');
            $('.newCc .set-status').hide();
            $('.newCc .current-status').hide();
            $('.newCc .remove-payment').prop("disabled", false);

            if (cloneIndex > 1) {
                $(this).closest('.payment-body').find('#payment-item-' + cloneIndex + ' .remove-payment').show();
                $(this).closest('.payment-body').find('#payment-item-' + cloneIndex + ' .pi-status').find('.current-status').hide();
            }

            $('.newCc .btn-edit-payment').click(function () {
                $scope.currentEdit.cc = '';
                $scope.currentEdit.cvc = '';
                $scope.currentEdit.cardHolderName = '';
                $scope.currentEdit.month = '';
                $scope.currentEdit.year = '';

                var size = $('input[name=slideup_toggler]:checked').val();
                var modalElem = $('.modal-edit-payment.form-add-cc');
                if (size == "mini") {
                    $('.modal-edit-payment.form-add-cc').modal('show');
                } else {
                    $('.modal-edit-payment.form-add-cc').modal('show');
                    if (size == "default") {
                        modalElem.children('.modal-dialog').removeClass('modal-lg');
                    } else if (size == "full") {
                        modalElem.children('.modal-dialog').addClass('modal-lg');
                    }
                }
            });

            $('.newCc .remove-payment').click(function () {
                if ($scope.hasBeenSaved) {
                    var maskedCc = $(".newCc .cc-no").attr("cc");
                    $scope.deleteCardData.cc = maskedCc;
                    var index = $('.newCc').attr("count");
                    $scope.deleteCardData.index = index;
                    //remove();
                } else {
                    remove();
                }
            });

            $('.saveNewCc').click(function () {
                $scope.addCreditCard.getPreToken();
                //$('.modal-edit-payment.form-add-cc').modal('hide');
                //$('.newCc .cc-no').text($scope.ccType($scope.currentEdit.cc) + ' ****' + $scope.currentEdit.cc.slice(-4));
                //$('.newCc .expiry').text($scope.currentEdit.month + '/' + $scope.currentEdit.year.slice(-2));
                //var maskedCc = $scope.currentEdit.cc.slice(0, 1) + '************' + $scope.currentEdit.cc.slice(-4);
                //$('.newCc .cc-no').attr('cc', maskedCc);
            });

            $('.newCc .set-status').click(function () {
                var ccNo = $('.newCc .cc-no').attr("cc");
                var index = $('.newCc').attr("count");
                $scope.ccToSetPrimary = ccNo;
                $scope.primaryIndex = index;
            });
        }      
    }


    $scope.validation = function () {
        if (!$scope.checkNumber($scope.currentEdit.cc) || !$scope.checkName($scope.currentEdit.cardHolderName)
                || !$scope.checkDate(parseInt($scope.currentEdit.month), parseInt($scope.currentEdit.year))) {
            if (!$scope.checkDate($scope.currentEdit.month, $scope.currentEdit.year)) {
                alert('error');
            }
            return false;
        } else {
            return true;
        }
    }

    $scope.checkName = function (name) {
        var re = /^[a-zA-Z ]+$/;
        if (name == null || name.match(re)) {
            return true;
        } else {
            return false;
        }
    }

    $scope.checkNumber = function (number) {
        var re = /^[0-9]+$/;
        if (number == "") {
            return true;
        }

        if (number == null || number.match(re)) {
            return true;
        } else {
            return false;
        }
    }

    $scope.checkDate = function (month, year) {
        if (month == '0' || year == 'Tahun') {
            $scope.dateOver = true;
            return false;
        }

        var now = new Date();
        var monthNow = now.getMonth();
        var yearNow = now.getFullYear();

        if (year > yearNow) {
            return true;
        }
        else if (year == yearNow) {
            if (month < monthNow + 1) {
                $scope.dateOver = true;
                return false;
            } else {
                return true;
            }
        } else {
            $scope.dateOver = true;
            return false;
        }
    }
    $scope.trial = 0;
    $scope.hasBeenSaved = false;
    $scope.addCreditCard = {
        inputValid: true,
        ccdata: false,
        getPreToken: function () {
            if (!$scope.validation()) {
                $scope.addCreditCard.inputValid = false;
            } else {
                if ($scope.currentEdit.cc == null || $scope.currentEdit.cc.length < 12 || $scope.currentEdit.cc.length > 19) {
                    $scope.notifCardLength = true;
                } else {
                    $scope.notifCardLength = false;
                    Veritrans.url = VeritransTokenConfig.Url;
                    Veritrans.client_key = VeritransTokenConfig.ClientKey;
                    var card = function () {
                        var gross_amount = 15000;
                        if ($scope.currentEdit.TwoClickToken == 'false') {
                            $scope.addCreditCard.ccdata = true;
                            return {
                                'card_number': $scope.currentEdit.cc,
                                'card_exp_month': $scope.currentEdit.month,
                                'card_exp_year': $scope.currentEdit.year,
                                'card_cvv': $scope.currentEdit.cvc,
                                'secure': true,
                                'bank': 'mandiri',
                                'gross_amount': gross_amount
                            }
                        } else {
                            return {
                                'card_cvv': $scope.currentEdit.cvc,
                                'token_id': $scope.currentEdit.TwoClickToken,
                                'two_click': true,
                                'secure': true,
                                'bank': 'mandiri',
                                'gross_amount': gross_amount
                            }
                        }
                    };

                    Veritrans.token(card, callback);

                    function callback(response) {
                        if (response.redirect_url) {
                            $log.debug('Open Dialog 3Dsecure');
                            openDialog(response.redirect_url);

                        } else if (response.status_code == '200') {
                            closeDialog();
                            $("#vt-token").val(response.token_id);
                            $scope.currentEdit.Token = response.token_id;

                            $scope.addCreditCard.addCC();

                        } else {
                            closeDialog();
                            $log.debug(JSON.stringify(response));
                        }
                    }

                    // Open 3DSecure dialog box
                    function openDialog(url) {
                        $.fancybox.open({
                            href: url,
                            type: 'iframe',
                            autoSize: false,
                            width: 400,
                            height: 420,
                            closeBtn: false,
                            modal: true
                        });
                    }

                    // Close 3DSecure dialog box
                    function closeDialog() {
                        $.fancybox.close();
                    }
                }
            }

        },
        addCC: function () {
            $('.addCcwait').modal({
                backdrop: 'static',
                keyboard: false
            });
            if ($scope.trial > 3) {
                $scope.trial = 0;
            }
            $log.debug('submitting form');
            $scope.addCreditCard.updating = true;
            var authAccess = getAuthAccess();
            if (authAccess == 2) {
                //authorized
                $http({
                    url: AddCreditCardConfig.Url,
                    method: 'POST',
                    data: {
                        token: $scope.currentEdit.Token,
                        cardHolderName: $scope.currentEdit.cardHolderName,
                        cardExpirymonth: parseInt($scope.currentEdit.month),
                        cardExpiryYear: parseInt($scope.currentEdit.year)
                    },
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).then(function (returnData) {
                    $('.addCcwait').modal("hide");
                    if (returnData.data.status == '200') {
                        $('.addCcSucceed').modal({
                            backdrop: 'static',
                        });
                        $scope.hasBeenSaved = true;
                        $log.debug('Success Adding new Credit Card');
                        $scope.addCreditCard.updating = false;
                        $scope.addCreditCard.updated = true;
                        $('.modal-edit-payment.form-add-cc').modal('hide');
                        $('.newCc .cc-no').text($scope.ccType($scope.currentEdit.cc) + ' ****' + $scope.currentEdit.cc.slice(-4));
                        $('.newCc .expiry').text($scope.currentEdit.month + '/' + $scope.currentEdit.year.slice(-2));
                        var maskedCc = $scope.currentEdit.cc.slice(0, 1) + '************' + $scope.currentEdit.cc.slice(-4);
                        $('.newCc .cc-no').attr('cc', maskedCc);

                        if ($scope.creditCards == null || $scope.creditCards.length == 0) {
                            $('.newCc .current-status').removeClass('ng-hide');
                            $('.newCc .current-status').show();
                            $('.newCc .set-status').hide();
                            $('.newCc .remove-payment').prop("disabled", true);
                            $('.newCc .remove-payment').addClass("disableDel");
                        } else {
                            $('.newCc .set-status').removeClass('ng-hide');
                            $('.newCc .current-status').hide();
                            $('.newCc .set-status').show();
                            $('.newCc .remove-payment').prop("disabled", false);
                        }
                        $scope.hasBeenSaved = true;
                    }
                    else {
                        $('.addCcFailed').modal({
                            backdrop: 'static',
                        });
                        $('.addCcFailed .close').click(function () {
                            $('.addCcFailed').modal("hide");
                        });
                        $log.debug(returnData.data.error);
                        $log.debug(returnData);
                        $scope.addCreditCard.edit = true;
                        $scope.addCreditCard.updating = false;
                        $scope.hasBeenSaved = false;
                    }
                    $('.newCc .btn-payment-edit').hide();
                }).catch(function () {
                    $scope.trial++;
                    $('.newCc .btn-payment-edit').hide();
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.addCreditCard(name);
                    }
                    else {
                        $('.addCcFailed').modal({
                            backdrop: 'static',
                        });
                        $('.addCcFailed .close').click(function () {
                            $('.addCcFailed').modal("hide");
                        });
                        $log.debug('Failed requesting change profile');
                        $scope.addCreditCard.edit = true;
                        $scope.addCreditCard.updating = false;
                        $scope.hasBeenSaved = false;
                    }
                });
            }
            else { //if not authorized
                $('.newCc .btn-payment-edit').hide();
                $scope.addCreditCard.edit = true;
                $scope.addCreditCard.updating = false;
            }
        }

    }

    function remove() {
        $(this).parents('.payment-item').remove();
    }

    $('body #add-payment').on('click', clone);

    $scope.currentEdit = {
        cc: '',
        type: '',
        month: '',
        year: '',
        cardHolderName: '',
        address: '',
        cvc: '',
        TwoClickToken: 'false',
        Token: ''
    };

    $scope.showEditForm = function (cc) {
        var size = $('input[name=slideup_toggler]:checked').val();
        var modalElem = $('.firstCc.modal-edit-payment');
        if (size == "mini") {
            $('.firstCc.modal-edit-payment').modal('show');
        } else {
            $('.firstCc.modal-edit-payment').modal('show');
            if (size == "default") {
                modalElem.children('.modal-dialog').removeClass('modal-lg');
            } else if (size == "full") {
                modalElem.children('.modal-dialog').addClass('modal-lg');
            }
        }

        if (cc != null) {
            $scope.currentEdit.cc = cc.maskedCardNumber;
            $scope.currentEdit.type = $scope.ccType(cc.maskedCardNumber);
            $scope.currentEdit.month = cc.cardExpiry.getMonth() + 1;
            $scope.currentEdit.year = cc.cardExpiry.getFullYear();
            $scope.currentEdit.cardHolderName = cc.cardHolderName;
        }     
    }
    $scope.message = '';
    $scope.trial = 0;
    $scope.currentPrimary = '';
    $scope.ccToSetPrimary = '';
    $scope.primaryIndex = '';
    $scope.setPrimaryData = function(cc, no) {
        $scope.ccToSetPrimary = cc;
        $scope.primaryIndex = no;
    }

    $scope.setPrimary = function (cc, index) {
        $('.wait').modal({
            backdrop: 'static',
            keyboard: false
        });
        $scope.trial = 0;
        var authAccess = getAuthAccess();
        if (authAccess == 2) {
            $http({
                url: SetPrimaryCardConfig.Url,
                method: 'POST',
                data: {
                    maskedCardNumber: cc
                },
                headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
            }).then(function (returnData) {us
                $('.wait').modal("hide");
                if (returnData.data.status == 200) {
                    $('.setPrimarySucceed').modal({
                        backdrop: 'static',
                        keyboard: false
                    });
                    //$('.payment-item[count=' + index + ']');
                    $('.payment-item[count=' + $scope.currentPrimary + '] .current-status').hide();
                    $('.payment-item[count=' + $scope.currentPrimary + '] .set-status').show();
                    $('.payment-item[count=' + $scope.currentPrimary + '] .set-status').removeClass('ng-hide');
                    $('.disableDel').prop("disabled", false);
                    $('.disableDel').removeClass('disableDel');
                    $('.payment-item[count=' + index + '] .current-status').removeClass('ng-hide');
                    $('.payment-item[count=' + index + '] .current-status').show();
                    $('.payment-item[count=' + index + '] .set-status').hide();
                    $('.payment-item[count=' + index + '] .remove-payment').prop("disabled", true);
                    $('.payment-item[count=' + index + '] .remove-payment').addClass("disableDel");
                    $scope.message = 'Setting Primary Card is Successful';
                    $scope.currentPrimary = index;
                } else {
                    $('.setPrimaryFailed').modal({
                        backdrop: 'static',
                        keyboard: false
                    });
                    $('.setPrimaryFailed .close').click(function() {
                        $('.setPrimaryFailed').modal("hide");
                    });
                    $log.debug(returnData.data.error);
                }
            }).catch(function(returnData) {
                $scope.trial++;
                if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                {
                    $scope.setPrimary(cc);
                } else {
                    $('.setPrimaryFailed').modal({
                        backdrop: 'static',
                        keyboard: false
                    });
                    $('.setPrimaryFailed .close').click(function () {
                        $('.setPrimaryFailed').modal("hide");
                    });
                    $log.debug('Failed Update Reservation');
                    $log.debug(returnData);
                    $scope.rsvUpdated = false;
                }
            });       
        }          
    }

    $scope.cardDeleted = false;
    $scope.deleteCardData= {
        cc: '',
        set: function(no, index) {
            $scope.deleteCardData.cc = no;
            $scope.deleteCardData.index = index;
        },
        index : ''
    }

    $scope.deleteCard = function (maskedCardNo, index) {
        if ($scope.hasBeenSaved) {
            $('.deleteCcwait').modal({
                backdrop: 'static',
                keyboard: false
            });
            $scope.cardDeleted = false;
            var authAccess = getAuthAccess();
            if (authAccess == 2) {
                //authorized
                $http({
                    url: DeleteCreditCardConfig.Url,
                    method: 'POST',
                    data: {
                        maskedCardNo: maskedCardNo
                    },
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).then(function(returnData) {
                    $('.deleteCcwait').modal("hide");
                    if (returnData.data.status == '200') {
                        $log.debug('Success Delete CC');
                        $scope.cardDeleted = true;
                        $('.deleteCcSucceed').modal({
                            backdrop: 'static',
                            //keyboard: false
                        });
                        $('.payment-item[count=' + index + ']')
                        dve();
                    } else {
                        $log.debug(returnData.data.error);
                        $log.debug(returnData);
                        $scope.cardDeleted = false;
                        $('.deleteCcFailed').modal({
                            backdrop: 'static',
                            //keyboard: false
                        });
                        $('.deleteCcFailed .close').click(function() {
                            $('.deleteCcFailed').modal("hide");
                        });
                    }
                }).catch(function(returnData) {
                    $scope.trial++;
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.deleteUser();
                    } else {
                        $('.deleteCcFailed').modal({
                            backdrop: 'static',
                            //keyboard: false
                        });
                        $('.deleteCcFailed .close').click(function() {
                            //$('.deleteCcFailed').modal("hide");
                        });
                        $log.debug('Failed Add User');
                        $log.debug(returnData);
                        $scope.cardDeleted = false;
                    }
                });
            } else { //if not authorized
                $scope.cardDeleted = false;
            }
        } else {
            $('.newCc').remove();
        }       
    }
        
}]);

app.controller('B2BUserManagementController', [
    '$http', '$log', '$scope', function ($http, $log, $scope) {

        var hash = (location.hash);
        // variables

        //General Variables
        $scope.pageLoaded = true;
        $scope.email = 'Test';
        $scope.trial = 0;
        $scope.loading = false;
        $scope.enableEdit = false;
        $scope.currentSection = '';
        $scope.profileForm = {
            active: false
        };
        $scope.userData = {
            email: '',
            name: '',
            countryCallCd: '',
            phone: '',
            position: '',
            department: '',
            branch: '',
            approverId: '',
            role: []
        }
        $scope.roleData = {
            email: '',
            role: ''
        }
        $scope.countries = [{ "name": "Afghanistan", "dial_code": "93", "code": "AF" }, { "name": "Aland Islands", "dial_code": "358", "code": "AX" }, { "name": "Albania", "dial_code": "355", "code": "AL" }, { "name": "Algeria", "dial_code": "213", "code": "DZ" }, { "name": "AmericanSamoa", "dial_code": "1 684", "code": "AS" }, { "name": "Andorra", "dial_code": "376", "code": "AD" }, { "name": "Angola", "dial_code": "244", "code": "AO" }, { "name": "Anguilla", "dial_code": "1 264", "code": "AI" }, { "name": "Antarctica", "dial_code": "672", "code": "AQ" }, { "name": "Antigua and Barbuda", "dial_code": "1268", "code": "AG" }, { "name": "Argentina", "dial_code": "54", "code": "AR" }, { "name": "Armenia", "dial_code": "374", "code": "AM" }, { "name": "Aruba", "dial_code": "297", "code": "AW" }, { "name": "Australia", "dial_code": "61", "code": "AU" }, { "name": "Austria", "dial_code": "43", "code": "AT" }, { "name": "Azerbaijan", "dial_code": "994", "code": "AZ" }, { "name": "Bahamas", "dial_code": "1 242", "code": "BS" }, { "name": "Bahrain", "dial_code": "973", "code": "BH" }, { "name": "Bangladesh", "dial_code": "880", "code": "BD" }, { "name": "Barbados", "dial_code": "1 246", "code": "BB" }, { "name": "Belarus", "dial_code": "375", "code": "BY" }, { "name": "Belgium", "dial_code": "32", "code": "BE" }, { "name": "Belize", "dial_code": "501", "code": "BZ" }, { "name": "Benin", "dial_code": "229", "code": "BJ" }, { "name": "Bermuda", "dial_code": "1 441", "code": "BM" }, { "name": "Bhutan", "dial_code": "975", "code": "BT" }, { "name": "Bolivia, Plurinational State of", "dial_code": "591", "code": "BO" }, { "name": "Bosnia and Herzegovina", "dial_code": "387", "code": "BA" }, { "name": "Botswana", "dial_code": "267", "code": "BW" }, { "name": "Brazil", "dial_code": "55", "code": "BR" }, { "name": "British Indian Ocean Territory", "dial_code": "246", "code": "IO" }, { "name": "Brunei Darussalam", "dial_code": "673", "code": "BN" }, { "name": "Bulgaria", "dial_code": "359", "code": "BG" }, { "name": "Burkina Faso", "dial_code": "226", "code": "BF" }, { "name": "Burundi", "dial_code": "257", "code": "BI" }, { "name": "Cambodia", "dial_code": "855", "code": "KH" }, { "name": "Cameroon", "dial_code": "237", "code": "CM" }, { "name": "Canada", "dial_code": "1", "code": "CA" }, { "name": "Cape Verde", "dial_code": "238", "code": "CV" }, { "name": "Cayman Islands", "dial_code": " 345", "code": "KY" }, { "name": "Central African Republic", "dial_code": "236", "code": "CF" }, { "name": "Chad", "dial_code": "235", "code": "TD" }, { "name": "Chile", "dial_code": "56", "code": "CL" }, { "name": "China", "dial_code": "86", "code": "CN" }, { "name": "Christmas Island", "dial_code": "61", "code": "CX" }, { "name": "Cocos (Keeling) Islands", "dial_code": "61", "code": "CC" }, { "name": "Colombia", "dial_code": "57", "code": "CO" }, { "name": "Comoros", "dial_code": "269", "code": "KM" }, { "name": "Congo", "dial_code": "242", "code": "CG" }, { "name": "Congo, The Democratic Republic of the Congo", "dial_code": "243", "code": "CD" }, { "name": "Cook Islands", "dial_code": "682", "code": "CK" }, { "name": "Costa Rica", "dial_code": "506", "code": "CR" }, { "name": "Cote d'Ivoire", "dial_code": "225", "code": "CI" }, { "name": "Croatia", "dial_code": "385", "code": "HR" }, { "name": "Cuba", "dial_code": "53", "code": "CU" }, { "name": "Cyprus", "dial_code": "357", "code": "CY" }, { "name": "Czech Republic", "dial_code": "420", "code": "CZ" }, { "name": "Denmark", "dial_code": "45", "code": "DK" }, { "name": "Djibouti", "dial_code": "253", "code": "DJ" }, { "name": "Dominica", "dial_code": "1 767", "code": "DM" }, { "name": "Dominican Republic", "dial_code": "1 849", "code": "DO" }, { "name": "Ecuador", "dial_code": "593", "code": "EC" }, { "name": "Egypt", "dial_code": "20", "code": "EG" }, { "name": "El Salvador", "dial_code": "503", "code": "SV" }, { "name": "Equatorial Guinea", "dial_code": "240", "code": "GQ" }, { "name": "Eritrea", "dial_code": "291", "code": "ER" }, { "name": "Estonia", "dial_code": "372", "code": "EE" }, { "name": "Ethiopia", "dial_code": "251", "code": "ET" }, { "name": "Falkland Islands (Malvinas)", "dial_code": "500", "code": "FK" }, { "name": "Faroe Islands", "dial_code": "298", "code": "FO" }, { "name": "Fiji", "dial_code": "679", "code": "FJ" }, { "name": "Finland", "dial_code": "358", "code": "FI" }, { "name": "France", "dial_code": "33", "code": "FR" }, { "name": "French Guiana", "dial_code": "594", "code": "GF" }, { "name": "French Polynesia", "dial_code": "689", "code": "PF" }, { "name": "Gabon", "dial_code": "241", "code": "GA" }, { "name": "Gambia", "dial_code": "220", "code": "GM" }, { "name": "Georgia", "dial_code": "995", "code": "GE" }, { "name": "Germany", "dial_code": "49", "code": "DE" }, { "name": "Ghana", "dial_code": "233", "code": "GH" }, { "name": "Gibraltar", "dial_code": "350", "code": "GI" }, { "name": "Greece", "dial_code": "30", "code": "GR" }, { "name": "Greenland", "dial_code": "299", "code": "GL" }, { "name": "Grenada", "dial_code": "1 473", "code": "GD" }, { "name": "Guadeloupe", "dial_code": "590", "code": "GP" }, { "name": "Guam", "dial_code": "1 671", "code": "GU" }, { "name": "Guatemala", "dial_code": "502", "code": "GT" }, { "name": "Guernsey", "dial_code": "44", "code": "GG" }, { "name": "Guinea", "dial_code": "224", "code": "GN" }, { "name": "Guinea-Bissau", "dial_code": "245", "code": "GW" }, { "name": "Guyana", "dial_code": "595", "code": "GY" }, { "name": "Haiti", "dial_code": "509", "code": "HT" }, { "name": "Holy See (Vatican City State)", "dial_code": "379", "code": "VA" }, { "name": "Honduras", "dial_code": "504", "code": "HN" }, { "name": "Hong Kong", "dial_code": "852", "code": "HK" }, { "name": "Hungary", "dial_code": "36", "code": "HU" }, { "name": "Iceland", "dial_code": "354", "code": "IS" }, { "name": "India", "dial_code": "91", "code": "IN" }, { "name": "Indonesia", "dial_code": "62", "code": "ID" }, { "name": "Iran, Islamic Republic of Persian Gulf", "dial_code": "98", "code": "IR" }, { "name": "Iraq", "dial_code": "964", "code": "IQ" }, { "name": "Ireland", "dial_code": "353", "code": "IE" }, { "name": "Isle of Man", "dial_code": "44", "code": "IM" }, { "name": "Israel", "dial_code": "972", "code": "IL" }, { "name": "Italy", "dial_code": "39", "code": "IT" }, { "name": "Jamaica", "dial_code": "1 876", "code": "JM" }, { "name": "Japan", "dial_code": "81", "code": "JP" }, { "name": "Jersey", "dial_code": "44", "code": "JE" }, { "name": "Jordan", "dial_code": "962", "code": "JO" }, { "name": "Kazakhstan", "dial_code": "7 7", "code": "KZ" }, { "name": "Kenya", "dial_code": "254", "code": "KE" }, { "name": "Kiribati", "dial_code": "686", "code": "KI" }, { "name": "Korea, Democratic People's Republic of Korea", "dial_code": "850", "code": "KP" }, { "name": "Korea, Republic of South Korea", "dial_code": "82", "code": "KR" }, { "name": "Kuwait", "dial_code": "965", "code": "KW" }, { "name": "Kyrgyzstan", "dial_code": "996", "code": "KG" }, { "name": "Laos", "dial_code": "856", "code": "LA" }, { "name": "Latvia", "dial_code": "371", "code": "LV" }, { "name": "Lebanon", "dial_code": "961", "code": "LB" }, { "name": "Lesotho", "dial_code": "266", "code": "LS" }, { "name": "Liberia", "dial_code": "231", "code": "LR" }, { "name": "Libyan Arab Jamahiriya", "dial_code": "218", "code": "LY" }, { "name": "Liechtenstein", "dial_code": "423", "code": "LI" }, { "name": "Lithuania", "dial_code": "370", "code": "LT" }, { "name": "Luxembourg", "dial_code": "352", "code": "LU" }, { "name": "Macao", "dial_code": "853", "code": "MO" }, { "name": "Macedonia", "dial_code": "389", "code": "MK" }, { "name": "Madagascar", "dial_code": "261", "code": "MG" }, { "name": "Malawi", "dial_code": "265", "code": "MW" }, { "name": "Malaysia", "dial_code": "60", "code": "MY" }, { "name": "Maldives", "dial_code": "960", "code": "MV" }, { "name": "Mali", "dial_code": "223", "code": "ML" }, { "name": "Malta", "dial_code": "356", "code": "MT" }, { "name": "Marshall Islands", "dial_code": "692", "code": "MH" }, { "name": "Martinique", "dial_code": "596", "code": "MQ" }, { "name": "Mauritania", "dial_code": "222", "code": "MR" }, { "name": "Mauritius", "dial_code": "230", "code": "MU" }, { "name": "Mayotte", "dial_code": "262", "code": "YT" }, { "name": "Mexico", "dial_code": "52", "code": "MX" }, { "name": "Micronesia, Federated States of Micronesia", "dial_code": "691", "code": "FM" }, { "name": "Moldova", "dial_code": "373", "code": "MD" }, { "name": "Monaco", "dial_code": "377", "code": "MC" }, { "name": "Mongolia", "dial_code": "976", "code": "MN" }, { "name": "Montenegro", "dial_code": "382", "code": "ME" }, { "name": "Montserrat", "dial_code": "1664", "code": "MS" }, { "name": "Morocco", "dial_code": "212", "code": "MA" }, { "name": "Mozambique", "dial_code": "258", "code": "MZ" }, { "name": "Myanmar", "dial_code": "95", "code": "MM" }, { "name": "Namibia", "dial_code": "264", "code": "NA" }, { "name": "Nauru", "dial_code": "674", "code": "NR" }, { "name": "Nepal", "dial_code": "977", "code": "NP" }, { "name": "Netherlands", "dial_code": "31", "code": "NL" }, { "name": "Netherlands Antilles", "dial_code": "599", "code": "AN" }, { "name": "New Caledonia", "dial_code": "687", "code": "NC" }, { "name": "New Zealand", "dial_code": "64", "code": "NZ" }, { "name": "Nicaragua", "dial_code": "505", "code": "NI" }, { "name": "Niger", "dial_code": "227", "code": "NE" }, { "name": "Nigeria", "dial_code": "234", "code": "NG" }, { "name": "Niue", "dial_code": "683", "code": "NU" }, { "name": "Norfolk Island", "dial_code": "672", "code": "NF" }, { "name": "Northern Mariana Islands", "dial_code": "1 670", "code": "MP" }, { "name": "Norway", "dial_code": "47", "code": "NO" }, { "name": "Oman", "dial_code": "968", "code": "OM" }, { "name": "Pakistan", "dial_code": "92", "code": "PK" }, { "name": "Palau", "dial_code": "680", "code": "PW" }, { "name": "Palestinian Territory, Occupied", "dial_code": "970", "code": "PS" }, { "name": "Panama", "dial_code": "507", "code": "PA" }, { "name": "Papua New Guinea", "dial_code": "675", "code": "PG" }, { "name": "Paraguay", "dial_code": "595", "code": "PY" }, { "name": "Peru", "dial_code": "51", "code": "PE" }, { "name": "Philippines", "dial_code": "63", "code": "PH" }, { "name": "Pitcairn", "dial_code": "872", "code": "PN" }, { "name": "Poland", "dial_code": "48", "code": "PL" }, { "name": "Portugal", "dial_code": "351", "code": "PT" }, { "name": "Puerto Rico", "dial_code": "1 939", "code": "PR" }, { "name": "Qatar", "dial_code": "974", "code": "QA" }, { "name": "Romania", "dial_code": "40", "code": "RO" }, { "name": "Russia", "dial_code": "7", "code": "RU" }, { "name": "Rwanda", "dial_code": "250", "code": "RW" }, { "name": "Reunion", "dial_code": "262", "code": "RE" }, { "name": "Saint Barthelemy", "dial_code": "590", "code": "BL" }, { "name": "Saint Helena, Ascension and Tristan Da Cunha", "dial_code": "290", "code": "SH" }, { "name": "Saint Kitts and Nevis", "dial_code": "1 869", "code": "KN" }, { "name": "Saint Lucia", "dial_code": "1 758", "code": "LC" }, { "name": "Saint Martin", "dial_code": "590", "code": "MF" }, { "name": "Saint Pierre and Miquelon", "dial_code": "508", "code": "PM" }, { "name": "Saint Vincent and the Grenadines", "dial_code": "1 784", "code": "VC" }, { "name": "Samoa", "dial_code": "685", "code": "WS" }, { "name": "San Marino", "dial_code": "378", "code": "SM" }, { "name": "Sao Tome and Principe", "dial_code": "239", "code": "ST" }, { "name": "Saudi Arabia", "dial_code": "966", "code": "SA" }, { "name": "Senegal", "dial_code": "221", "code": "SN" }, { "name": "Serbia", "dial_code": "381", "code": "RS" }, { "name": "Seychelles", "dial_code": "248", "code": "SC" }, { "name": "Sierra Leone", "dial_code": "232", "code": "SL" }, { "name": "Singapore", "dial_code": "65", "code": "SG" }, { "name": "Slovakia", "dial_code": "421", "code": "SK" }, { "name": "Slovenia", "dial_code": "386", "code": "SI" }, { "name": "Solomon Islands", "dial_code": "677", "code": "SB" }, { "name": "Somalia", "dial_code": "252", "code": "SO" }, { "name": "South Africa", "dial_code": "27", "code": "ZA" }, { "name": "South Georgia and the South Sandwich Islands", "dial_code": "500", "code": "GS" }, { "name": "Spain", "dial_code": "34", "code": "ES" }, { "name": "Sri Lanka", "dial_code": "94", "code": "LK" }, { "name": "Sudan", "dial_code": "249", "code": "SD" }, { "name": "Suriname", "dial_code": "597", "code": "SR" }, { "name": "Svalbard and Jan Mayen", "dial_code": "47", "code": "SJ" }, { "name": "Swaziland", "dial_code": "268", "code": "SZ" }, { "name": "Sweden", "dial_code": "46", "code": "SE" }, { "name": "Switzerland", "dial_code": "41", "code": "CH" }, { "name": "Syrian Arab Republic", "dial_code": "963", "code": "SY" }, { "name": "Taiwan", "dial_code": "886", "code": "TW" }, { "name": "Tajikistan", "dial_code": "992", "code": "TJ" }, { "name": "Tanzania, United Republic of Tanzania", "dial_code": "255", "code": "TZ" }, { "name": "Thailand", "dial_code": "66", "code": "TH" }, { "name": "Timor-Leste", "dial_code": "670", "code": "TL" }, { "name": "Togo", "dial_code": "228", "code": "TG" }, { "name": "Tokelau", "dial_code": "690", "code": "TK" }, { "name": "Tonga", "dial_code": "676", "code": "TO" }, { "name": "Trinidad and Tobago", "dial_code": "1 868", "code": "TT" }, { "name": "Tunisia", "dial_code": "216", "code": "TN" }, { "name": "Turkey", "dial_code": "90", "code": "TR" }, { "name": "Turkmenistan", "dial_code": "993", "code": "TM" }, { "name": "Turks and Caicos Islands", "dial_code": "1 649", "code": "TC" }, { "name": "Tuvalu", "dial_code": "688", "code": "TV" }, { "name": "Uganda", "dial_code": "256", "code": "UG" }, { "name": "Ukraine", "dial_code": "380", "code": "UA" }, { "name": "United Arab Emirates", "dial_code": "971", "code": "AE" }, { "name": "United Kingdom", "dial_code": "44", "code": "GB" }, { "name": "United States", "dial_code": "1", "code": "US" }, { "name": "Uruguay", "dial_code": "598", "code": "UY" }, { "name": "Uzbekistan", "dial_code": "998", "code": "UZ" }, { "name": "Vanuatu", "dial_code": "678", "code": "VU" }, { "name": "Venezuela, Bolivarian Republic of Venezuela", "dial_code": "58", "code": "VE" }, { "name": "Vietnam", "dial_code": "84", "code": "VN" }, { "name": "Virgin Islands, British", "dial_code": "1 284", "code": "VG" }, { "name": "Virgin Islands, U.S.", "dial_code": "1 340", "code": "VI" }, { "name": "Wallis and Futuna", "dial_code": "681", "code": "WF" }, { "name": "Yemen", "dial_code": "967", "code": "YE" }, { "name": "Zambia", "dial_code": "260", "code": "ZM" }, { "name": "Zimbabwe", "dial_code": "263", "code": "ZW" }];

        $scope.getCountries = function (dialCode) {
            for (var i = 0; i < $scope.countries.length; i++) {
                if ($scope.countries[i].dial_code == dialCode) {
                    return $scope.countries[i].name;
                }
            }
        }

        $scope.editUser = {
            email: '',
            name: '',
            countryCallCd: '',
            phone: '',
            department: '',
            branch: '',
            position: '',
            approval: '',
            role:[],
            edit: false,
            updating: false
        }

        $scope.selectedRole = [];

        $scope.EditUserProfile = function(index) {
            $scope.editUser.email = $scope.users[index].email;
            //$scope.editUser.userId = $scope.users[index].userId;
            $scope.editUser.name = $scope.users[index].firstName + " " + $scope.users[index].lastName;
            $scope.editUser.countryCallCd = $scope.users[index].countryCallCd;
            $scope.editUser.phone = $scope.users[index].phoneNumber;
            $scope.editUser.department = $scope.users[index].department;
            $scope.editUser.branch = $scope.users[index].branch;
            $scope.editUser.position = $scope.users[index].position;
            $scope.editUser.approverId = $scope.users[index].approverId;
            $scope.editUser.approver = $scope.users[index].approverName;
            $scope.editUser.role = $scope.users[index].roleName;
            $scope.selectedRole = [];
            $('#edit-user').modal('show');
            
        }

        $scope.UpdateUserLock = function(userId, status) {
            if (status == "LOCK") {
                $scope.lockUser = true;
            } else {
                $scope.lockUser = false;
            }
            $scope.lockUpdated = false;
            var authAccess = getAuthAccess();
            if (authAccess == 2) {
                //authorized
                $http({
                    url: UpdateUserLockConfig.Url,
                    method: 'POST',
                    data: {
                        userId: userId,
                        IsLocked: $scope.lockUser
                    },
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).then(function (returnData) {
                    //$log.debug(returnData);
                    if (returnData.data.status == '200') {
                        $log.debug('Success updating user Lock');
                        $scope.lockUpdated = true;

                    }
                    else {
                        $log.debug(returnData.data.error);
                        $log.debug(returnData);
                        $scope.lockUpdated = false;
                    }
                }).catch(function (returnData) {
                    $scope.trial++;
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.UpdateUserLock(userId, status);
                    }
                    else {
                        $log.debug('Failed Update User Lock');
                        $log.debug(returnData);
                        $scope.lockUpdated = false;
                    }
                });
            }
            else { //if not authorized
                $scope.lockUpdated = false;
            }
        }

        //Get User
        $scope.User = {
            GetUser: function () {
                if ($scope.trial > 3) {
                    $scope.trial = 0;
                }
                var authAccess = getAuthAccess();
                if (authAccess == 2) {
                    $http({
                        method: 'POST',
                        url: GetUserConfig.Url,
                        async: false,
                        headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                    }).then(function (returnData) {
                        if (returnData.status == "200") {
                            $log.debug('Success getting All Users');
                            $scope.roles = returnData.data.roles;
                            $scope.users = returnData.data.users;
                            $scope.approvers = returnData.data.approvers;
                        }
                        else {
                            $log.debug('There is an error');
                            $log.debug('Error : ' + returnData.data.error);
                            $log.debug(returnData);
                        }
                    }).catch(function (returnData) {
                        $scope.trial++;
                        if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                        {
                            $scope.User.GetUser();
                        }
                        else {
                            $log.debug('Failed to Get Profile');
                        }
                    });
                }
                else {
                    $log.debug('Not Authorized');
                }
            }
        }

        //Update Role

        $scope.setUpdateRole = function () {
            $scope.enableEdit = true;
        }

        $scope.cancelUpdate = function () {
            $scope.enableEdit = false;
        }


        $scope.updateRole = function (email) {
            $scope.roleUpdated = false;
            var authAccess = getAuthAccess();
            if (authAccess == 2) {
                //authorized
                $http({
                    url: UpdateRoleConfig.Url,
                    method: 'POST',
                    data: {
                        userName: email,
                        role: $scope.roleData.role
                    },
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).then(function (returnData) {
                    //$log.debug(returnData);
                    if (returnData.data.status == '200') {
                        $log.debug('Success updating Profile');
                        $scope.roleUpdated = true;

                    }
                    else {
                        $log.debug(returnData.data.error);
                        $log.debug(returnData);
                        $scope.roleUpdated = false;
                    }
                }).catch(function (returnData) {
                    $scope.trial++;
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.updateRole(email, role);
                    }
                    else {
                        $log.debug('Failed Update Role');
                        $log.debug(returnData);
                        $scope.roleUpdated = false;
                    }
                });
            }
            else { //if not authorized
                $scope.roleUpdated = false;
            }
        }

        $scope.closePopUp = function () {
            $scope.userAdded = false;
            $scope.roleUpdated = false;
            $scope.userDeleted = false;
            window.location.reload();
        }

        //Add User
        $scope.addUser = function () {
            $scope.userAdded = false;
            $scope.userData.role = $scope.selectedRole;
            var authAccess = getAuthAccess();
            if (authAccess == 2) {
                //authorized
                $http({
                    url: AddUserConfig.Url,
                    method: 'POST',
                    data: {
                        email: $scope.userData.email,
                        name: $scope.userData.name,
                        countryCallCd: $scope.userData.countryCallCd,
                        phone: $scope.userData.phone,
                        position: $scope.userData.position,
                        department: $scope.userData.department,
                        branch: $scope.userData.branch,
                        approverId: $scope.userData.approverId,
                        role: $scope.userData.role
                    },
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).then(function (returnData) {
                    if (returnData.data.status == '200') {
                        $log.debug('Success Add User');
                        $scope.userAdded = true;
                        window.location.reload();
                    }
                    else {
                        $log.debug(returnData.data.error);
                        $log.debug(returnData);
                        $scope.userAdded = false;
                    }
                }).catch(function (returnData) {
                    $scope.trial++;
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.addUser();
                    }
                    else {
                        $log.debug('Failed Add User');
                        $log.debug(returnData);
                        $scope.userAdded = false;
                    }
                });
            }
            else { //if not authorized
                $scope.userAdded = false;
            }
        }

        $scope.updateUser = function() {
            $scope.updatingUser = false;
            $scope.editUser.role = $scope.selectedRole;

            var authAccess = getAuthAccess();
            if (authAccess == 2) {
                //authorized
                $http({
                    url: UpdateUserConfig.Url,
                    method: 'POST',
                    data: {
                        email: $scope.editUser.email,
                        name: $scope.editUser.name,
                        countryCallCd: $scope.editUser.countryCallCd,
                        phone: $scope.editUser.phone,
                        branch: $scope.editUser.branch,
                        position: $scope.editUser.position,
                        department: $scope.editUser.department,
                        approverId: $scope.editUser.approverId,
                        roles: $scope.editUser.role
                    },
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).then(function (returnData) {
                    if (returnData.data.status == '200') {
                        $log.debug('Success Update User');
                        $scope.updatingUser = true;
                        window.location.reload();
                    }
                    else {
                        $log.debug(returnData.data.error);
                        $log.debug(returnData);
                        $scope.updatingUser = false;
                    }
                }).catch(function (returnData) {
                    $scope.trial++;
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.updateUser();
                    }
                    else {
                        $log.debug('Failed Update User');
                        $log.debug(returnData);
                        $scope.updatingUser = false;
                    }
                });
            }
            else { //if not authorized
                $scope.updatingUser = false;
            }
        }

        //Delete User
        $scope.deleteUser = function (email) {
            $scope.userDeleted = false;
            var authAccess = getAuthAccess();
            if (authAccess == 2) {
                //authorized
                $http({
                    url: DeleteUserConfig.Url,
                    method: 'POST',
                    data: {
                        email: email
                    },
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).then(function (returnData) {
                    if (returnData.data.status == '200') {
                        $log.debug('Success Add User');
                        $scope.userDeleted = true;
                    }
                    else {
                        $log.debug(returnData.data.error);
                        $log.debug(returnData);
                        $scope.userDeleted = false;
                        window.location.reload();
                    }
                }).catch(function (returnData) {
                    $scope.trial++;
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.deleteUser();
                    }
                    else {
                        $log.debug('Failed Add User');
                        $log.debug(returnData);
                        $scope.userDeleted = false;
                    }
                });
            }
            else { //if not authorized
                $scope.userDeleted = false;
            }
        }

        //Executing Get User
        $scope.User.GetUser();

        $scope.changeSection = function (name) {
            $scope.currentSection = name;
        }

        if (hash == '#order') {
            $scope.changeSection('order');
        } else {
            $scope.changeSection('profile');
        }

    }
]);