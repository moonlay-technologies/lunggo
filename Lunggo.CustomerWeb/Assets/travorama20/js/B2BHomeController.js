// home controller
app.controller('B2BFlightSearchFormController', ['$scope', '$log', '$http', '$location', '$resource', '$timeout', function ($scope, $log, $http, $location, $resource, $timeout) {
    $scope.Passenger = {
        Adult: 1,
        Child: 0,
        Infant: 0,
        PaxAmtAdult: [1, 2, 3, 4, 5, 6, 7, 8, 9],
        PaxAmtElse: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]
    }

    $scope.OneWay = false;
    
    $("input[name=triptype]").change(function () {
        if (this.value == 'oneway') {
            $scope.OneWay = true;
        } else {
            $scope.OneWay = false;
        }
    });

    $scope.searchParam = {
        DepartureDate: '',
        ReturnDate: '',
        OriginCity: '',
        OriginAirport: '',
        DestinationCity: '',
        DestinationAirport: '',
        Origin:'',
        Destination: ''
    }
    $('.search-location').hide();


    $scope.$watch('searchParam.Origin',
        function(newValue) {
            if (newValue != null && newValue.length >= 3) {
                $('.search-location .location-recommend').hide();
                $('.search-location .location-search').show();
                getLocation(newValue);
            }
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
            //FlightSearchConfig.autocomplete.loading = false;
            var result = returnData.airports;
            //FlightSearchConfig.autocomplete.cache[keyword] = returnData.airports;
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
        }).error(function (returnData) {
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
                //$('.flight-submit-button').removeClass('disabled');
            } else {
                alert('Kota Asal dan Tujuan Tidak Boleh Sama');
                $('.flight-submit-button').addClass('disabled');
            }

        } else {
            if (locationCity != $scope.searchParam.OriginCity) {
                $scope.searchParam.DestinationAirport = locationCode;
                $scope.searchParam.DestinationCity = locationCity;
                $('#destinationPlace').val($(this).text() + ' (' + locationCode + ')');
                $('.flight-submit-button').removeClass('disabled');
            } else {
                alert('Kota Asal dan Tujuan Tidak Boleh Sama');
                $('.flight-submit-button').addClass('disabled');
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
                //$('.flight-submit-button').removeClass('disabled');
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
        console.log("BERHASIL");
    });
    function hideLocation() {
        $('.search-location').hide();
    }
}]);