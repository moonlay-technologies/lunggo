// checkout controller
app.controller('CheckoutController', ['$http', '$scope', '$rootScope', '$interval', '$location', '$log',
    function ($http, $scope, $rootScope, $interval, $location, $log) {
    
    // *****
    // general variables
        
    $scope.correctName = true;
    $scope.pageLoaded = true;
    $scope.trial = 0;
    $scope.language = langCode;
    $scope.buyerInfo = {};
    $scope.token = CheckoutDetail.Token;
    $scope.currency = 'IDR';
        $scope.initialPrice = CheckoutDetail.Price;
    $scope.passengers = [];
    $scope.bookingDate = new Date();
    $scope.Countries = Countries;
    $scope.flightDetail = {
        departureFullDate : new Date(CheckoutDetail.DepartureDate),
        beforeDepartureFullDate: new Date(CheckoutDetail.BeforeDepartureDate),
        passportDepartureFullDate : new Date(CheckoutDetail.PassportDepartureDate)
    };
    
    $scope.CheckoutConfig = {
        Token: CheckoutDetail.Token,
        ExpiryDate: CheckoutDetail.ExpiryDate,
        // trips
        Trips: CheckoutDetail.Trips,
        // price
        Price: CheckoutDetail.Price,
        FinalPrice: CheckoutDetail.Price,
        OriginalPrice: CheckoutDetail.OriginalPrice,
        // passenger
        Passenger: CheckoutDetail.Passenger,
        PassengerTypeName: CheckoutDetail.PassengerName,
        TotalAdultFare: CheckoutDetail.TotalAdultFare,
        TotalChildFare: CheckoutDetail.TotalChildFare,
        TotalInfantFare: CheckoutDetail.TotalInfantFare,
        // generate passenger
        GeneratePassenger: function () {
            var today = new Date();

            if ($scope.CheckoutConfig.Passenger[0] > 0) {
                for (var i = 0; i < $scope.CheckoutConfig.Passenger[0]; i++) {
                    var x = { typeName: $scope.CheckoutConfig.PassengerTypeName[0], type: 'adult', birth: today };
                    $scope.passengers.push(x);
                }
            }
            if ($scope.CheckoutConfig.Passenger[1] > 0) {
                for (var i = 0; i < $scope.CheckoutConfig.Passenger[1]; i++) {
                    var x = { typeName: $scope.CheckoutConfig.PassengerTypeName[1], type: 'child' };
                    $scope.passengers.push(x);
                }
            }
            if ($scope.CheckoutConfig.Passenger[2] > 0) {
                for (var i = 0; i < $scope.CheckoutConfig.Passenger[2]; i++) {
                    var x = { typeName: $scope.CheckoutConfig.PassengerTypeName[2], type: 'infant' };
                    $scope.passengers.push(x);
                }
            }
        },
        // identity requirement
        PassportRequired: CheckoutDetail.PassportRequired,
        IdRequired: CheckoutDetail.IdRequired,
        NationalityRequired: CheckoutDetail.NationalityRequired,
        BirthDateRequired: CheckoutDetail.BirthDateRequired,
        // buyer info
        BuyerInfo: CheckoutDetail.BuyerInfo
    };

    $scope.CheckoutConfig.GeneratePassenger();
    $scope.PageConfig = $rootScope.PageConfig;
    $scope.PageConfig.ExpiryDate = {
        Expired: false,
        Time: $scope.CheckoutConfig.ExpiryDate,
        Start: function () {
            var expiryTime = new Date($scope.PageConfig.ExpiryDate.Time);
            if ($scope.PageConfig.ExpiryDate.Expired || $scope.PageConfig.ExpiryDate.Starting) return;
            $interval(function () {
                $scope.PageConfig.ExpiryDate.Starting = true;
                var nowTime = new Date();
                if (nowTime > expiryTime) {
                    $scope.PageConfig.ExpiryDate.Expired = true;
                }
            }, 1000);
        },
        Starting: false
    };

    $scope.PageConfig.ExpiryDate.Start();
    $scope.PageConfig.ReturnUrl = document.referrer == (window.location.origin + window.location.pathname + window.location.search) ? '/' : document.referrer;

    // paging function
    // set hash to page 1
    angular.element(document).ready(function () {
        $location.hash('page-1');
    });
    angular.element(window).on('hashchange', function () {
        if ($location.hash() == '') {
            $scope.PageConfig.ActivePage = 1;
            $location.hash('page-1');
        }
    });
    
    $scope.form = {
        incompleteContactTitle: false,
        incompleteContactName: false,
        incompleteContactPhone: false,
        incompleteContactEmail: false,
        hasDuplicatePaxName: false,
        incompleteTitles: false,
        incompleteBirthDates: false,
        incompletePassportData: false,
        checkNameIncomplete: false
    }
     
    //titles
    $scope.titles = [
            { name: 'Pilih Titel', value: '' },
            { name: 'Tn.', value: 'Mister' },
            { name: 'Ny.', value: 'Mistress' },
            { name: 'Nn.', value: 'Miss' }
    ];
    $scope.titleKids = [
            { name: 'Pilih Titel', value: '' },
            { name: 'Tn.', value: 'Mister' },
            { name: 'Nn.', value: 'Miss' }
    ];
    // buyer info

    $scope.months = [
            { value: 0, name: 'Bulan' },
            { value: 1, name: 'Januari' },
            { value: 2, name: 'Februari' },
            { value: 3, name: 'Maret' },
            { value: 4, name: 'April' },
            { value: 5, name: 'Mei' },
            { value: 6, name: 'Juni' },
            { value: 7, name: 'Juli' },
            { value: 8, name: 'Agustus' },
            { value: 9, name: 'September' },
            { value: 10, name: 'Oktober' },
            { value: 11, name: 'November' },
            { value: 12, name: 'Desember' }

    ];
    
    if ($scope.loggedIn) {
        $scope.buyerInfo.name = buyerInfo.name;
        $scope.buyerInfo.countryCode = buyerInfo.countryCode;
        $scope.buyerInfo.phone = buyerInfo.phone;
        $scope.buyerInfo.email = buyerInfo.email;
    } else {
        $scope.buyerInfo = {};
    }
    
    $scope.$watch(function () {
        return location.hash;
    }, function (value) {
        if (!$scope.PageConfig.ActivePageChanged) {
            $scope.PageConfig.ChangePage(1);
            $scope.PageConfig.ActivePageChanged = true;
        } else {
            value = value.split('-');
            value = value[1];
            if (value > 0) {
                $scope.PageConfig.ChangePage(value);
            }
        }
    });

    // change page
    $scope.PageConfig.ActivePageChanged = false;
    $scope.PageConfig.ActivePage = 1;
    $scope.PageConfig.ChangePage = function (page) {
        $scope.PageConfig.ActivePage = page;
        $location.hash("page-" + page);
    }
  
    //**********Functions for Validations and Warning Message********
    
    $scope.hasDuplicatePaxName = function () {
        var namesSoFar = [];
        for (var i = 0; i < $scope.passengers.length; ++i) {
            var value = $scope.passengers[i].name;
            if (value == null) {
                return false;
            }
            if (value.length != 0) {
                if (namesSoFar.indexOf(value.toLowerCase()) > -1) {
                    return true;
                } else {
                    //Not in the array
                    namesSoFar.push(value.toLowerCase());
                }
            }
        }
        return false;
    }

    $scope.CheckOnlyAdult = function () {
        var valid = true;
        for (var x = 0; x < $scope.passengers.length; x++) {
            if ($scope.passengers[x].type != 'adult') {
                valid = false;
            }
        }
        return valid;
    }

    $scope.checkName = function (name) {

        var re = /^[a-zA-Z ]+$/;
        var x = $scope.buyerInfo.name;
        if (name == null || name.match(re)) {
            return true;
        } else {
            return false;
        }
    }

    $scope.CheckDate = function (passengers) {
        var valid = true;
        for (var x = 0; x < passengers.length; x++) {
            if (passengers[x].birth.date === $scope.dates($scope.flightDetail.departureFullDate.getMonth(), $scope.flightDetail.departureFullDate.getYear)[0]
               || passengers[x].birth.month === $scope.months[0].value.toString() ||
                passengers[x].birth.year == $scope.generateYear(passengers[x].type)[0] ||
                $scope.invalidDate(passengers[x].birth.date, passengers[x].birth.month, passengers[x].birth.year)) {
                valid = false;
            }
            else if (passengers[x].type != 'adult') {
                if (passengers[x].birth.date == null || passengers[x].birth.month == null || passengers[x].birth.year == null
                    || $scope.invalidDate(passengers[x].birth.date, passengers[x].birth.month, passengers[x].birth.year)) {
                    valid = false;
                }
            }
            else if (passengers[x].type == 'adult') {
                if ($scope.CheckoutConfig.BirthDateRequired && (passengers[x].birth.date == null || passengers[x].birth.month == null ||
                    passengers[x].birth.year == null) || $scope.invalidDate(passengers[x].birth.date, passengers[x].birth.month, passengers[x].birth.year)) {
                    valid = false;
                }
            }
        }
        return valid;
    }

    $scope.CheckPassportDate = function (passengers) {
        var valid = true;
        for (var x = 0; x < passengers.length; x++) {
            if (passengers[x].passport.expire.date === $scope.dates($scope.flightDetail.departureFullDate.getMonth(), $scope.flightDetail.departureFullDate.getYear)[0]
               || passengers[x].passport.expire.month === $scope.months[0].value.toString() ||
                passengers[x].passport.expire.year == $scope.generateYear(passengers[x].type)[0]
                || $scope.invalidDate(passengers[x].passport.expire.date, passengers[x].passport.expire.month, passengers[x].passport.expire.year)) {
                valid = false;
            }
            else if (passengers[x].passport.expire.date == null || passengers[x].passport.expire.month == null ||
                passengers[x].passport.expire.year == null ||
                $scope.invalidDate(passengers[x].passport.expire.date, passengers[x].passport.expire.month, passengers[x].passport.expire.year)) {
                valid = false;
            }
        }
        return valid;
    }

    $scope.passportNumberFilled = function (paxes) {
        var valid = true;
        for (var i = 0; i < paxes.length; ++i) {
            var value = paxes[i].passport.number;
            if (value == null) {
                valid = false;
            } else {
                if (value.length == 0) {
                    valid = false;
                }
            }
        }
        return valid;
    }

    $scope.checkNameComplete = function (passengers) {
        var valid = true;
        for (var x = 0; x < passengers.length; x++) {
            if (passengers[x].name == null || passengers[x].name == "") {
                valid = false;
            }
        }
        return valid;
    }

    $scope.checkEmail = function (email) {
        if (email == null || email == "") {
            return false;
        } else {
            return true;
        }
    }

    $scope.checkPhone = function (phone) {
        if ($scope.buyerInfo.countryCode == 'xx') {
            return false;
        } else if ($scope.buyerInfo.countryCode != 'xx' && (phone == null || phone == "")) {
            return false;
        } else {
            return true;
        }
    }

    $scope.CheckTitle = function (passengers) {
        var valid = true;
        for (var x = 0; x < passengers.length; x++) {
            if (passengers[x].title == $scope.titles[0].value) {
                valid = false;
            }
        }
        return valid;
    }

    $scope.invalidDate = function (day, month, year) {
        if (year % 4 == 0 && month == 2) {
            if (day > 29) {
                return true;
            } else {
                return false;
            }
        } else {
            if (month == 2 && (day == null || day > 28)) {
                return true;
            } else if (month == 4 || month == 6 || month == 9 || month == 11) {
                if (day > 30) {
                    return true;
                } else {
                    return false;
                }
            } else {
                return false;
            }
        }
    }

    //*****************************END*****************************

    //********************FUNCTIONS TO FILL FORM*******************

    $scope.validateForm = function (page) {
        if (page == 1) {
            if (!$scope.CheckTitle([$scope.buyerInfo])) {
                $scope.form.incompleteContactTitle = true;
            } else {
                $scope.form.incompleteContactTitle = false;
            }

            if (!$scope.checkNameComplete([$scope.buyerInfo])) {
                $scope.form.incompleteContactName = true;
            } else {
                $scope.form.incompleteContactName = false;
            }


            if (!$scope.checkPhone($scope.buyerInfo.phone)) {
                $scope.form.incompleteContactPhone = true;
            } else {
                $scope.form.incompleteContactPhone = false;
            }

            if (!$scope.checkEmail($scope.buyerInfo.email)) {
                $scope.form.incompleteContactEmail = true;
            } else {
                $scope.form.incompleteContactEmail = false;
            }

            if (!$scope.form.incompleteContactTitle && !$scope.form.incompleteContactName
                && !$scope.form.incompleteContactPhone && !$scope.form.incompleteContactEmail) {
                $scope.PageConfig.ChangePage(2);
            }

        }
        if (page == 2) {
            if ($scope.hasDuplicatePaxName()) {
                $scope.form.hasDuplicatePaxName = true;
            } else {
                $scope.form.hasDuplicatePaxName = false;
            }

            if (!$scope.checkNameComplete($scope.passengers)) {
                $scope.form.checkNameIncomplete = true;
            } else {
                $scope.form.checkNameIncomplete = false;
            }

            if (!$scope.CheckTitle($scope.passengers)) {
                $scope.form.incompleteTitles = true;
            } else {
                $scope.form.incompleteTitles = false;
            }

            if (($scope.CheckoutConfig.BirthDateRequired || (!$scope.CheckoutConfig.BirthDateRequired && !$scope.CheckOnlyAdult())) && !$scope.CheckDate($scope.passengers)) {
                $scope.form.incompleteBirthDates = true;
            } else {
                $scope.form.incompleteBirthDates = false;
            }

            if ($scope.CheckoutConfig.PassportRequired && (!$scope.CheckPassportDate($scope.passengers) || !$scope.passportNumberFilled($scope.passengers))) {
                $scope.form.incompletePassportData = true;
            } else {
                $scope.form.incompletePassportData = false;
            }

            if (!$scope.form.hasDuplicatePaxName && !$scope.form.incompleteTitles && !$scope.form.incompleteBirthDates
                && !$scope.form.incompletePassportData && !$scope.form.checkNameIncomplete) {
                $scope.PageConfig.ChangePage(3);
            }
        }
    }
    
    $scope.getPassportDateInit = function () {
        var datego = new Date($scope.flightDetail.departureFullDate);
        return new Date(new Date(datego).setMonth(datego.getMonth() + 6));
    }

    $scope.getPassportMonth = function () {
        if ($scope.flightDetail.departureFullDate.getMonth() + 6 > $scope.months.length) {
            return $scope.flightDetail.departureFullDate.getMonth() - 6;
        } else return $scope.flightDetail.departureFullDate.getMonth() + 6;
    }

    $scope.getPassportYear = function () {

        if ($scope.getPassportMonth() > $scope.flightDetail.departureFullDate.getMonth()) {
            return $scope.flightDetail.departureYear;
        } else {
            return ($scope.flightDetail.departureYear + 1);
        }
    }
    
    $scope.dates = function (month, year) {
        var dates = ['Tanggal'];
        var maxDate = -1;
        // check leap year
        if (year % 4 == 0 && month == 1) {
            maxDate = 29;
        } else {
            if (month == 1) {
                maxDate = 28;
            } else if (month == 3 || month == 5 || month == 8 || month == 10 || month == 12) {
                maxDate = 30;
            } else {
                maxDate = 31;
            }
        }
        for (var i = 1; i <= maxDate; i++) {
            dates.push(i);
        }
        return dates;
    }

    $scope.changeTitle = function (title) {
        if (title == 'Mister')
            return 'Tn.';
        else if (title == 'Mistress')
            return 'Ny.';
        else if (title == 'Miss')
            return 'Nn.';
    }
   
    $scope.getMonth = function(m) {
        if (m == 1) {
            return 'Januari';
        }
        else if (m == 2) {
            return 'Februari';
        }
        else if (m == 3) {
            return 'Maret';
        }
        else if (m == 4) {
            return 'April';
        }
        else if (m == 5) {
            return 'Mei';
        }
        else if (m == 6) {
            return 'Juni';
        }
        else if (m == 7) {
            return 'Juli';
        }
        else if (m == 8) {
            return 'Agustus';
        }
        else if (m == 9) {
            return 'September';
        }
        else if (m == 10) {
            return 'Oktober';
        }
        else if (m == 11) {
            return 'November';
        }
        else if (m == 12) {
            return 'Desember';
        }

    }

    $scope.generateYear = function (type) {
        var years = [];

        function listYear(min, max) {
            for (var i = min; i <= max; i++) {
                years.push(i);
            }
        }

        switch (type) {
            case 'adult':
                listYear(($scope.flightDetail.departureFullDate.getFullYear() - 120), ($scope.flightDetail.departureFullDate.getFullYear() - 12));
                years = years.reverse();
                return ['Tahun'].concat(years);
                break;
            case 'child':
                listYear(($scope.flightDetail.beforeDepartureFullDate.getFullYear() - 12), ($scope.flightDetail.beforeDepartureFullDate.getFullYear() - 2));
                years = years.reverse();
                return ['Tahun'].concat(years);
                break;
            case 'infant':
                listYear(($scope.flightDetail.beforeDepartureFullDate.getFullYear() - 2), $scope.bookingDate.getFullYear());
                years = years.reverse();
                return ['Tahun'].concat(years);
                break;
            case 'passport':
                listYear($scope.flightDetail.passportDepartureFullDate.getFullYear(), ($scope.flightDetail.passportDepartureFullDate.getFullYear() + 10));
                return ['Tahun'].concat(years);
                break;
        }

    }

    // init passenger
    $scope.initPassenger = function (passenger) {
        if (passenger.type == 'adult') {
            passenger.birth = {};
        } else if (passenger.type == 'infant') {
            passenger.birth = {};
        } else if (passenger.type == 'child') {
            passenger.birth = {};
        }
        if ($scope.CheckoutConfig.NationalityRequired == true) {
            passenger.nationality = 'Indonesia';
        }
        passenger.passport = {
            expire: {}
        }
    }

    $scope.validateBirthday = function (passenger) {
        if (passenger.type == 'adult') {
            if (passenger.birth.year >= $scope.flightDetail.departureFullDate.getFullYear() - 12) {
                passenger.birth.year = $scope.flightDetail.departureFullDate.getFullYear() - 12;
                if (passenger.birth.month - 1 >= $scope.flightDetail.departureFullDate.getMonth()) {
                    passenger.birth.month = $scope.flightDetail.departureFullDate.getMonth() + 1;
                    if (passenger.birth.date > $scope.flightDetail.departureFullDate.getDate()) {
                        passenger.birth.date = $scope.flightDetail.departureFullDate.getDate();
                    }
                }
            }
        }

        else if (passenger.type == 'child') {
            var minYear = $scope.flightDetail.departureFullDate.getFullYear() - 12;
            var maxYear = $scope.flightDetail.departureFullDate.getFullYear() - 2;
            if (passenger.birth.year == minYear) {
                if (passenger.birth.month - 1 <= $scope.flightDetail.departureFullDate.getMonth()) {
                    passenger.birth.month = $scope.flightDetail.departureFullDate.getMonth() + 1;
                    if (passenger.birth.date < $scope.flightDetail.departureFullDate.getDate() + 1) {
                        passenger.birth.date = $scope.flightDetail.departureFullDate.getDate() + 1;
                    }
                }
            } else if (passenger.birth.year == maxYear) {
                if (passenger.birth.month - 1 >= $scope.flightDetail.departureFullDate.getMonth()) {
                    passenger.birth.month = $scope.flightDetail.departureFullDate.getMonth() + 1;
                    if (passenger.birth.date > $scope.flightDetail.departureFullDate.getDate()) {
                        passenger.birth.date = $scope.flightDetail.departureFullDate.getDate();
                    }
                }
            }
        }
        else if (passenger.type == 'infant') {
            minYear = $scope.flightDetail.departureFullDate.getFullYear() - 2;
            maxYear = $scope.bookingDate.getFullYear();
            if (passenger.birth.year == minYear) {
                if (passenger.birth.month - 1 <= $scope.flightDetail.departureFullDate.getMonth()) {
                    passenger.birth.month = $scope.flightDetail.departureFullDate.getMonth() + 1;
                    if (passenger.birth.date < $scope.flightDetail.departureFullDate.getDate() + 1) {
                        passenger.birth.date = $scope.flightDetail.departureFullDate.getDate() + 1;
                    }
                }
            }
            else if (passenger.birth.year == maxYear) {
                if ((passenger.birth.month - 1 >= $scope.flightDetail.departureFullDate.getMonth() - 2 &&
                        passenger.birth.year == $scope.flightDetail.departureFullDate.getFullYear()) ||
                        passenger.birth.month - 1 >= $scope.bookingDate.getMonth() - 1) {
                    passenger.birth.month = $scope.bookingDate.getMonth() + 1;
                    if (passenger.birth.date > $scope.bookingDate.getDate()) {
                        passenger.birth.date = $scope.bookingDate.getDate();
                    }
                }
            }
        }
    }

    // validate passport expiry date
    $scope.validatePassport = function (passenger) {
        if (passenger.passport.expire.year == $scope.flightDetail.passportDepartureFullDate.getFullYear()) {
            if (passenger.passport.expire.month - 1 <= $scope.flightDetail.passportDepartureFullDate.getMonth()) {
                passenger.passport.expire.month = $scope.flightDetail.passportDepartureFullDate.getMonth() + 1;
                if (passenger.passport.expire.date <= $scope.flightDetail.passportDepartureFullDate.getDate()) {
                    passenger.passport.expire.date = $scope.flightDetail.passportDepartureFullDate.getDate();
                }
            }
        }
    }
    // get number
    $scope.getNumber = function (number) {
        var numbers = [];
        number = parseInt(number);
        for (var i = 1; i <= number; i++) {
            numbers.push(i);
        }
        return numbers;
    }

    //*****************************END*****************************

    // book
    $scope.book = {
        isPriceChanged: false,
        newPrice: '',
        booking: false,
        url: FlightBookConfig.Url,
        postData: '',
        checked: false,
        rsvNo: '',
        isSuccess: false,
        send: function () {
            if ($scope.trial > 3) {
                    $scope.trial = 0;
                }
            $scope.book.booking = true;
            $scope.contactData = '{' + ' "title":"' + $scope.buyerInfo.title + '",  "name" :"' + $scope.buyerInfo.name + '","countryCallCd":"' + $scope.buyerInfo.countryCode + '","phone":"' + $scope.buyerInfo.phone + '","email":"' + $scope.buyerInfo.email + '"' + '}';
            $scope.paxData = ' "pax" : [';
            // generate data
            $scope.book.postData = ' "token":"' + $scope.token + '",  "contact" :' + $scope.contactData + ',"lang":"' + $scope.language + '"';
            for (var i = 0; i < $scope.passengers.length; i++) {

                // check nationality
                if (!$scope.CheckoutConfig.PassportRequired) {
                    $scope.passengers[i].passport.number = '';
                    $scope.passengers[i].passport.expire = {};
                    $scope.passengers[i].passport.expire.date = '';
                    $scope.passengers[i].passport.expire.month = '';
                    $scope.passengers[i].passport.expire.year = '';
                    $scope.passengers[i].passport.expire.full = '';
                    if (!$scope.CheckoutConfig.NationalityRequired) {
                        $scope.passengers[i].nationality = '';
                    }
                }
                if (!$scope.CheckoutConfig.IdRequired) {
                    $scope.passengers[i].idNumber = '';
                }

                $scope.passengers[i].birth.full = $scope.passengers[i].birth.year
                        + '/' + ('0' + (parseInt($scope.passengers[i].birth.month))).slice(-2)
                        + '/' + ('0' + $scope.passengers[i].birth.date).slice(-2);

                // passport expiry date
                $scope.passengers[i].passport.expire.full = $scope.passengers[i].passport.expire.year + '/' + ('0'
                    + (parseInt($scope.passengers[i].passport.expire.month))).slice(-2) + '/'
                    + ('0' + $scope.passengers[i].passport.expire.date).slice(-2);

                // birthdate
            
                $scope.paxData = 
                    $scope.paxData + '{ "type":"' + $scope.passengers[i].type + '", ' +
                                        '"title":"' + $scope.passengers[i].title + '" , ' +
                                        '"name":"' + $scope.passengers[i].name + '" ';


                if ($scope.CheckoutConfig.BirthDateRequired || $scope.passengers[i].type != 'adult') {
                    $scope.paxData = $scope.paxData + ', "dob":"' + $scope.passengers[i].birth.full + '"';
                }

                if ($scope.CheckoutConfig.NationalityRequired) {
                    $scope.paxData = $scope.paxData + ', "nationality":"' + $scope.passengers[i].nationality + '"';
                }

                if ($scope.CheckoutConfig.PassportRequired) {
                    $scope.paxData = $scope.paxData + ', "passportNo":"' + $scope.passengers[i].passport.number + '" , "passportExp":"' + $scope.passengers[i].passport.expire.full + '" , "passportCountry":"' + $scope.passengers[i].passport.country + '" ';
                }

                if (i != $scope.passengers.length - 1) {
                    $scope.paxData = $scope.paxData + '}' + ',';
                } else {
                    $scope.paxData = $scope.paxData + '}';
                }

            }
            $scope.paxData = $scope.paxData + ']';
            $scope.book.postData = '{' + $scope.book.postData + ',' + $scope.paxData + '}';
           
            $scope.book.postData = JSON.parse($scope.book.postData);
       
            //Check Authorization
            var authAccess = getAuthAccess();
            if (authAccess == 1 || authAccess == 2) {
                // send form
                $http({
                    method: 'POST',
                    url: $scope.book.url,
                    data: ($scope.book.postData),
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                    
                }).then(function (returnData) {
                   
                    $scope.book.checked = true;
                    $scope.book.booking = false;

                    if (returnData.data.status == '200') {
                        if ( returnData.data.rsvNo != null || returnData.data.rsvNo != '') {
                            if (returnData.data.itinChanged) {
                                $scope.book.isSuccess = false;
                                $scope.book.checked = true;
                                $scope.book.booking = false;
                            }

                            else if (returnData.data.priceChanged) {
                                $scope.book.isPriceChanged = true;
                                $scope.book.isSuccess = true;
                                $scope.book.newPrice = returnData.data.price;
                                $scope.book.checked = false;
                                $scope.book.booking = false;
                            }

                            else if (returnData.data.rsvNo != null && returnData.data.rsvNo.length != 0) {
                                $scope.book.isSuccess = true;
                                $scope.book.rsvNo = returnData.data.rsvNo;
                                $('form#rsvno input#rsvno-input').val(returnData.data.rsvNo);
                                $('form#rsvno').submit();
                                $scope.book.checked = true;
                                $scope.book.booking = false;
                            }

                            else {
                                $scope.book.isSuccess = false;
                                $scope.book.checked = true;
                                $scope.book.booking = false;
                            }
                        } else {
                            if (returnData.data.price != null) {
                                $scope.book.isPriceChanged = true;
                                $scope.book.isSuccess = false;
                                $scope.book.newPrice = returnData.data.price;
                                $scope.book.checked = true;
                            } else if (returnData.data.itinChanged) {
                                $scope.book.isSuccess = false;
                                $scope.book.checked = true;
                                $scope.book.booking = false;
                            }
                            else {
                                $scope.book.isSuccess = false;
                                $scope.book.checked = true;
                                $scope.errorMessage = returnData.data.error;
                            }
                        }
                    } else {
                        if (returnData.data.error == "ERFBOO03") {
                            $scope.PageConfig.ExpiryDate.Expired = true;
                        }
                        else if (returnData.data.error == "ERFBOO01") {
                            $scope.book.invalidData = true;
                        } else {
                            $scope.book.isSuccess = false;
                            $scope.book.checked = true;
                        }
                    }
                }).catch(function (returnData) {
                    $scope.trial++;
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.book.send();
                    }
                    else {
                        $scope.book.checked = true;
                        $scope.book.isSuccess = false;
                        $scope.book.booking = false;
                    }
                    
                });
            }
            else {
                $log.debug('Not Authorized');
                $scope.book.checked = true;
                $scope.book.isSuccess = false;
                $scope.book.booking = false;
            }       
        }
    }

    // expiry date
   
    //$scope.displayCountry = function (code) {
    //    for (var i = 0; i < $scope.Countries.length; i++) {
    //        if ($scope.Countries[i].code === code) {
    //            return $scope.Countries[i].name;
    //        }
    //    }
    //}
    
    
    // date, months, and year
    
    

    //$scope.incMth = function() {
    //    for (var z = 0; z < $scope.passengers.length; z++) {
    //        $scope.passengers[z].birth.month = parseInt($scope.passengers[z].birth.month, 10) + 1;
    //    }
    //}
    // *****
    
    // *****


}]);