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
        $('#valueDepDate').val(valDepDate);
        $('#valueRetDate').val(valRetDate);

        if (Cookies.get('type')) {
            if (Cookies.get('type') == 'OneWay') {
                $scope.OneWay = true;
                $('#yes').prop('checked', true);
                $('#no').prop('checked', false);
                $('.retDate').addClass('disabled');
                //$("input[name=triptype]").attr('value', 'OneWay');
            } else {
                $scope.OneWay = false;
                $('#no').prop('checked', true);
                $('#yes').prop('checked', false);
                $('.retDate').removeClass('disabled');
                //$("input[name=triptype]").attr('value', 'Return');
            }
        } else {
            $scope.OneWay = true;
            $('#yes').prop('checked', true);
            $('#no').prop('checked', false);
            $('.retDate').addClass('disabled');
            //$("input[name=triptype]").attr('value', 'OneWay');
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
        $('.childPax').val($scope.Passenger.Child);
        $('.infantPax').val($scope.Passenger.Infant);
        if (Cookies.get('cabin')) {
            $scope.searchParam.CabinClass = Cookies.get('cabin');
        } else {
            $scope.searchParam.CabinClass = 'y';
        }
        $('.pax').val($scope.searchParam.CabinClass);
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

    $scope.$watch('Passenger.Infant',
        function (newValue, oldValue) {
            newValue = parseInt(newValue);
            oldValue = parseInt(oldValue);
            if (newValue > $scope.Passenger.Adult) {
                alert('Jumlah bayi tidak boleh lebih dari penumpang dewasa');
                $scope.Passenger.Infant = oldValue;
            } else {
                $scope.TotalPax = $scope.Passenger.Adult + $scope.Passenger.Child;
                if (9 - $scope.TotalPax < newValue) {
                    alert('Jumlah penumpang tidak boleh lebih dari sembilan orang');
                    $scope.Passenger.Infant = oldValue;
                } else {
                    $scope.Passenger.Infant = newValue;
                }        
            }
        });

    $scope.$watch('Passenger.Adult',
        function (newValue, oldValue) {
            newValue = parseInt(newValue);
            oldValue = parseInt(oldValue);
            $scope.TotalPax = $scope.Passenger.Child + $scope.Passenger.Infant;
            if (9 - $scope.TotalPax < newValue) {
                alert('Jumlah penumpang tidak boleh lebih dari sembilan orang');
                $scope.Passenger.Adult = oldValue;
            } else {
                $scope.Passenger.Adult = newValue;
            }         
        });

    $scope.$watch('Passenger.Child',
        function (newValue, oldValue) {
            newValue = parseInt(newValue);
            oldValue = parseInt(oldValue);
            $scope.TotalPax = $scope.Passenger.Adult + $scope.Passenger.Infant;
            if (9 - $scope.TotalPax < newValue) {
                alert('Jumlah penumpang tidak boleh lebih dari sembilan orang');
                $scope.Passenger.Child = oldValue;
            } else {
                $scope.Passenger.Child = newValue;
            }
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
        //hideCalendar();
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
        //$log.debug($('#departureDate').val());
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
    });

    $('#returnDate').change(function () {
        //$log.debug($('#returnDate').val());
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
app.controller('B2BHotelSearchFormController', ['$scope', '$log', 'hotelSearchSvc', function ($scope, hotelSearchSvc) {
    $(document).ready(function () {

        $.getScript("js.cookie.js", function () { });
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
        Location: '',
        LocationCode: '',
        CheckInDate: todayDate,
        CheckOutDate: tmrwDate,
        Nights: '1',
        Rooms: '2',
        Occupancies: [
        {
            Adult: 1,
            Children: 0,
            ChildrenAges: [0,0,0,0]
        }],
        NightList: [1, 2, 3, 4, 5, 6, 7],
        RoomList: [1, 2, 3, 4, 5, 6, 7, 8],
        AdultList: [1, 2, 3, 4, 5],
        ChildList: [0, 1, 2, 3, 4],
        ChildAgeList: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11]
    }
    
    for (var x = 0; x < 7; x++) {
        $scope.searchParam.Occupancies.push({
            Adult: 1,
            Children: 0,
            ChildrenAges: [0, 0, 0, 0]
        });
    }
    
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

    var trial = 0;
    function generateSearchResult(list) {
        $('.autocomplete-result ul').empty();
        for (var i = 0 ; i < list.length; i++) {
            $('.autocomplete-result ul').append('<li href="#" data-code="' + list[i].code + '" data-city="' + list[i].city + '">' + list[i].city + ' (' + list[i].code + '), ' + list[i].name + ', ' + list[i].country + '</li>');
        }
    }
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
    $scope.showLocation = function (text) {
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
        //hideCalendar();
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
    $scope.setOptionCalendar = function (target) {
        $scope.selectCalendar = target;
    }
    $('#checkInDate').change(function () {
        //$log.debug($('#returnDate').val());
        var date = $('#checkInDate').val().split(' ');
        var day = parseInt(date[0]);
        var month = (date[1]);
        var year = parseInt(date[2]);
        var selectedDate = new Date(year, $scope.getMonthNumber(month), day);
        $scope.searchParam.CheckInDate = selectedDate;
        var checkOutDate= new Date(selectedDate);
        checkOutDate.setDate(checkOutDate.getDate() + $scope.searchParam.Nights);

        var valCheckInDate = $scope.setDateWriting($scope.searchParam.CheckInDate.getDate(),
            $scope.searchParam.CheckInDate.getMonth(), $scope.searchParam.CheckInDate.getFullYear());
        $('#valueCheckInDate').val(valCheckInDate);
        $(this).datepicker('hide');
    });

    $('#valueCheckInDate').click(function () {
        $('#checkInDate').datepicker('show');
    });

    $scope.gotoSearchResult = function () {
        url += searchParam;
        setCookie();
        window.location.href = url;

    };

    // set cookie
    function setCookie() {
        
    }
}]);