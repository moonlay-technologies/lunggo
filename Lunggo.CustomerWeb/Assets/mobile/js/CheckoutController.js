// checkout controller
app.controller('CheckoutController', ['$http', '$scope', '$rootScope', '$interval', '$location', function ($http, $scope, $rootScope, $interval, $location) {
    
    // *****
    // general variables
    $scope.PageConfig = $rootScope.PageConfig;
    $scope.pageLoaded = true;
    $scope.CheckoutConfig = {
        Token: CheckoutDetail.Token,
        ExpiryDate: CheckoutDetail.ExpiryDate,
        // trips
        Trips: CheckoutDetail.Trips,
        // price
        Price: CheckoutDetail.Price,
        FinalPrice: CheckoutDetail.Price,
        // passenger
        Passenger: CheckoutDetail.Passenger,
        Passengers: [],
        PassengerTypeName: CheckoutDetail.PassengerName,
        // generate passenger
        GeneratePassenger: function () {
            var today = new Date();
            
            if ($scope.CheckoutConfig.Passenger[0] > 0) {
                for (var i = 0; i < $scope.CheckoutConfig.Passenger[0]; i++) {
                    var x = { typeName: $scope.CheckoutConfig.PassengerTypeName[0], type: 'adult', birth: today };
                    $scope.passengers.push(x);
                }
            }
            if ($scope.CheckoutConfig.Passenger[1]> 0) {
                for (var i = 0; i < $scope.CheckoutConfig.Passenger[1]; i++) {
                    var x = { typeName: $scope.CheckoutConfig.PassengerTypeName[1], type: 'child'  };
                    $scope.passengers.push(x);
                }
            }
            if ($scope.CheckoutConfig.Passenger[2] > 0) {
                for (var i = 0; i < $scope.CheckoutConfig.Passenger[2]; i++) {
                    var x = { typeName: $scope.CheckoutConfig.PassengerTypeName[2], type: 'infant' };
                    $scope.passengers.push(x);
                }
            }
            //$scope.incMth();
        },
        // identity requirement
        PassportRequired: CheckoutDetail.PassportRequired,
        IdRequired: CheckoutDetail.IdRequired,
        NationalityRequired: CheckoutDetail.NationalityRequired,
        // buyer info
        BuyerInfo: CheckoutDetail.BuyerInfo
    };

    $scope.parseInt = parseInt;

    $scope.token = CheckoutDetail.Token;
    $scope.currency = 'IDR';
    $scope.initialPrice = CheckoutDetail.Price;

    // buyer info
    $scope.buyerInfo = {};
    if ($scope.loggedIn) {
        $scope.buyerInfo.fullname = buyerInfo.fullname;
        $scope.buyerInfo.countryCode = buyerInfo.countryCode;
        $scope.buyerInfo.phone = buyerInfo.phone;
        $scope.buyerInfo.email = buyerInfo.email;
    } else {
        $scope.buyerInfo = {};
    }

    // passengers
    $scope.passengers = [];

    // flight detail
    $scope.flightDetail = {};

    $scope.flightDetail.departureFullDate = CheckoutDetail.DepartureDate; // development only. Please change the value to actual date on production
    $scope.flightDetail.departureDate = -1;
    $scope.flightDetail.departureMonth = -1;
    $scope.flightDetail.departureYear = -1;
    $scope.flightDetail.minYearChild = -1;
    $scope.flightDetail.minYearInfant = -1;
    $scope.flightDetail.passportFullDate = -1;
    $scope.flightDetail.passportDate = -1;
    $scope.flightDetail.passportMonth = -1;
    $scope.flightDetail.passportYear = -1;
    $scope.flightDetail.generateDepartureDate = function (fullDate) {
        fullDate = new Date(fullDate);
        $scope.flightDetail.departureDate = fullDate.getDate();
        $scope.flightDetail.departureMonth = fullDate.getMonth();
        $scope.flightDetail.departureYear = fullDate.getFullYear();
        $scope.flightDetail.minYearChild = fullDate.getFullYear() - 12;
        $scope.flightDetail.minYearInfant = fullDate.getFullYear() - 2;
        // generate passport min expiry date
        $scope.flightDetail.passportFullDate = new Date(fullDate);
        $scope.flightDetail.passportFullDate.setMonth($scope.flightDetail.passportFullDate.getMonth() + 6);
        $scope.flightDetail.passportDate = $scope.flightDetail.passportFullDate.getDate();
        $scope.flightDetail.passportMonth = $scope.flightDetail.passportFullDate.getMonth();
        $scope.flightDetail.passportYear = $scope.flightDetail.passportFullDate.getFullYear();
    }
    $scope.flightDetail.generateDepartureDate($scope.flightDetail.departureFullDate);

    // payment detail
    $scope.paymentDetail = {
        method : ''
    }//$scope.paymentDetail

    $scope.paymentMethod = '';

    // credit card
    $scope.CreditCard = {
        TwoClickToken: 'false',
        Name: '',
        Month: '01',
        Year: 2016,
        Cvv: '',
        Number: ''
    }//$scope.CreditCard
    
    // transfer config
    $scope.TransferConfig = {
        UniqueCode: 0,
        Token: '',
        GetUniqueCode: function (sentPrice) {
            if (!sentPrice) {
                sentPrice = price;
            }
            // get unique payment code
            $http({
                method: 'GET',
                url: TransferConfig.Url,
                params: {
                    price: sentPrice
                }
            }).then(function (returnData) {
                console.log('Getting Unique Payment Code');
                console.log(returnData);
                $scope.TransferConfig.UniqueCode = returnData.data.transfer_code;
                $scope.TransferConfig.Token = returnData.data.token

            }, function (returnData) {
                console.log('Failed to get Unique Payment Code');
                console.log(returnData);
            });
        }
    };
    $scope.TransferConfig.GetUniqueCode($scope.CheckoutConfig.Price);

    // voucher code
    $scope.voucher = {
        confirmedCode: '',
        code: '',
        amount: 0,
        status: '',
        checking: false,
        checked: false,
        check: function () {
            $scope.voucher.checking = true;
            $http({
                method: 'GET',
                url: CheckVoucherConfig.Url,
                params: {
                    token: $scope.token,
                    code: $scope.voucher.code,
                    email: $scope.buyerInfo.email,
                    price: $scope.initialPrice
                }
            }).then(function (returnData) {
                console.log(returnData);
                $scope.voucher.checking = false;
                $scope.voucher.checked = true;
                $scope.voucher.status = returnData.data.ValidationStatus;
                if (returnData.data.Discount > 0) {
                    $scope.voucher.amount = returnData.data.Discount;
                    $scope.voucher.confirmedCode = $scope.voucher.code;
                    $scope.voucher.displayName = returnData.data.DisplayName;
                    // get unique code for transfer payment
                    $scope.TransferConfig.GetUniqueCode($scope.initialPrice - $scope.voucher.amount);
                }
            }, function (returnData) {
                $scope.voucher.checked = true;
                $scope.voucher.checking = false;
            });
        },
        reset: function () {
            $scope.voucher.code = '';
            $scope.voucher.amount = 0;
            $scope.voucher.confirmedCode = '';
            $scope.voucher.checked = false;
            // get unique code for transfer payment
            $scope.TransferConfig.GetUniqueCode($scope.initialPrice);
        }
    };

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
        ccChecked: false,
        checkCreditCard: function () {
            if ($scope.paymentMethod == 'CreditCard') {

                Veritrans.url = VeritransTokenConfig.Url;
                Veritrans.client_key = VeritransTokenConfig.ClientKey;
                var card = function () {
                    if ($scope.CreditCard.TwoClickToken == 'false') {
                        return {
                            'card_number': $scope.CreditCard.Number,
                            'card_exp_month': $scope.CreditCard.Month,
                            'card_exp_year': $scope.CreditCard.Year,
                            'card_cvv': $scope.CreditCard.Cvv,

                            // Set 'secure', 'bank', and 'gross_amount', if the merchant wants transaction to be processed with 3D Secure
                            'secure': true,
                            'bank': 'mandiri',
                            'gross_amount': $scope.initialPrice - $scope.CreditCardPromo.Amount - $scope.voucher.amount
                        }
                    } else {
                        return {
                            'card_cvv': $scope.CreditCard.Cvv,
                            'token_id': $scope.CreditCard.TwoClickToken,

                            'two_click': true,
                            'secure': true,
                            'bank': 'mandiri',
                            'gross_amount': $scope.initialPrice - $scope.CreditCardPromo.Amount - $scope.voucher.amount
                        }
                    }
                };

                // run the veritrans function to check credit card
                Veritrans.token(card, callback);

                function callback(response) {
                    if (response.redirect_url) {
                        // 3Dsecure transaction. Open 3Dsecure dialog
                        console.log('Open Dialog 3Dsecure');
                        openDialog(response.redirect_url);

                    } else if (response.status_code == '200') {
                        // success 3d secure or success normal
                        //close 3d secure dialog if any
                        closeDialog();

                        // store token data in input #token_id and then submit form to merchant server
                        $("#vt-token").val(response.token_id);
                        $scope.CreditCard.Token = response.token_id;

                        $scope.book.send();

                    } else {
                        // failed request token
                        //close 3d secure dialog if any
                        closeDialog();
                        $('#submit-button').removeAttr('disabled');
                        // Show status message.
                        $('#message').text(response.status_message);
                        console.log(JSON.stringify(response));
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

            } else {
                $scope.book.send();
            }
        },
        send: function () {
            $scope.book.booking = true;

            // generate data
            $scope.book.postData = ' "Token":"' + $scope.token + '",  "Contact.Title" :"' + $scope.buyerInfo.title + '","Contact.Name":"' + $scope.buyerInfo.fullName + '", "Contact.CountryCode":"' + $scope.buyerInfo.countryCode + '", "Contact.Phone":"' + $scope.buyerInfo.phone + '","Contact.Email":"' + $scope.buyerInfo.email + '","Language":"' + "ID"+ '"';
            for (var i = 0; i < $scope.passengers.length; i++) {

                // check nationality
                if (!$scope.passportRequired) {
                    $scope.passengers[i].passport.number = '';
                    $scope.passengers[i].passport.expire = {};
                    $scope.passengers[i].passport.expire.date = '';
                    $scope.passengers[i].passport.expire.month = '';
                    $scope.passengers[i].passport.expire.year = '';
                    $scope.passengers[i].passport.expire.full = '';
                    if (!$scope.CheckoutConfig.NationalityRequired) {
                        $scope.passengers[i].passport.country = '';
                    }
                }
                if (!$scope.idRequired) {
                    $scope.passengers[i].idNumber = '';
                }

                // passport expiry date
                $scope.passengers[i].passport.expire.full = $scope.passengers[i].passport.expire.year + '/' + ('0'
                    + (parseInt($scope.passengers[i].passport.expire.month) + 1)).slice(-2) + '/'
                    + ('0' + $scope.passengers[i].passport.expire.date).slice(-2);

                // birthdate
                $scope.passengers[i].birth.full = $scope.passengers[i].birth.year
                    + '/' + ('0' + (parseInt($scope.passengers[i].birth.month) + 1)).slice(-2) + '/'
                    + ('0' + $scope.passengers[i].birth.date).slice(-2);

                $scope.book.postData = $scope.book.postData
                    + (',"Passengers[' + i + '].Type": "' + $scope.passengers[i].type
                        + '", "Passengers[' + i + '].Title": "' + $scope.passengers[i].title + '", "Passengers[' + i + '].FirstName":"'
                        + $scope.passengers[i].firstName + '", "Passengers[' + i + '].LastName": "'
                        + $scope.passengers[i].lastName + '", "Passengers[' + i + '].BirthDate":"'
                        + $scope.passengers[i].birth.full + '", "Passengers[' + i + '].PassportNumber":"'
                        + $scope.passengers[i].passport.number
                        + '", "Passengers[' + i + '].PassportExpiryDate":"' + $scope.passengers[i].passport.expire.full
                        + '", "Passengers[' + i + '].idNumber":"' + $scope.passengers[i].idNumber
                        + '", "Passengers[' + i + '].Country":"' + $scope.passengers[i].passport.country +'"');
                //);
            }
            $scope.book.postData = '{' + $scope.book.postData + '}';
            $scope.book.postData = JSON.parse($scope.book.postData);

            console.log($scope.book.postData);

            // send form
            $http({
                method: 'POST',
                url: $scope.book.url,
                data: $.param($scope.book.postData),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            }).then(function (returnData) {
                console.log(returnData);

                $scope.book.checked = true;
                $scope.book.booking = false;
                if (returnData.data.IsSuccess) {
                    if (returnData.data.NewPrice != null) {
                        $scope.book.isPriceChanged = true;
                        $scope.book.isSuccess = true;
                        $scope.book.newPrice = returnData.data.NewPrice;
                        $scope.book.checked = false;
                    }
                    else {
                        $scope.book.isSuccess = true;
                        $scope.book.rsvNo = returnData.data.RsvNo;

                        $('form#rsvno input#rsvno-input').val(returnData.data.RsvNo);
                        $('form#rsvno').submit();
                        $scope.book.checked = true;
                    }
                    //$scope.book.isSuccess = true;
                    //$scope.book.rsvNo = returnData.data.RsvNo;

                    //$('form#rsvno input#rsvno-input').val(returnData.data.RsvNo);
                    //$('form#rsvno').submit();

                } else {
                    $scope.book.booking = false;
                    $scope.book.checked = true;
                    $scope.book.isSuccess = false;
                }

            }, function (returnData) {
                console.log(returnData);
                $scope.book.checked = true;
                $scope.book.isSuccess = false;
                $scope.book.booking = false;
            });

        }
    }//$scope.book

    $scope.getPassportMonth = function () {
        if ($scope.flightDetail.departureMonth + 6 > $scope.months.length) {
            return $scope.flightDetail.departureMonth - 6;
        } else return $scope.flightDetail.departureMonth + 6;
    }

    $scope.getPassportYear = function () {
        
        if ($scope.getPassportMonth() > $scope.flightDetail.departureMonth) {
            return $scope.flightDetail.departureYear;
        } else {
            return ($scope.flightDetail.departureYear + 1);
        }
    }

    // credit card promo checker
    $scope.CreditCardPromo = {
        Type: '',
        Valid: false,
        PromoName: '',
        Amount: 0,
        Check: function (creditCardNumber) {
            if ($scope.paymentMethod == 'CreditCard') {
                if (creditCardNumber) {
                    var firstNum = creditCardNumber.toString().charAt(0);
                    var minOrder = 200000;
                    var nowDate = new Date();
                    var utcDate = nowDate.getTime() + (nowDate.getTimezoneOffset() * 60000);
                    var jakartaDate = new Date(utcDate + (3600000 * 7));;
                    var jakartaDay = jakartaDate.getDay();
                    var endOfCampaign = new Date('31 March 2016');

                    var creditCardString = creditCardNumber.toString();

                    // check credit card type
                    if (firstNum == 5) {
                        $scope.CreditCardPromo.Type = 'mastercard';
                    } else if (firstNum == 4) {
                        $scope.CreditCardPromo.Type = 'visa';
                    } else {
                        $scope.CreditCardPromo.Type = '';
                    }

                    //**********
                    // Danamon Sweet Valentine
                    var valentineDate = new Date('14 February 2016');
                    if (creditCardString.length > 5 && (jakartaDate.getDate() == valentineDate.getDate() && jakartaDate.getMonth() == valentineDate.getMonth())) {
                        var danamonList = ['456798', '456799', '425857', '432449', '540731', '559228', '516634', '542260', '552239', '523983', '552338'];
                        creditCardString = creditCardString.substr(0, 6);

                        // if Danamon Card
                        if (danamonList.indexOf(creditCardString) > -1) {
                            $scope.CreditCardPromo.PromoName = 'Danamon Sweet Valentine';
                            $scope.CreditCardPromo.Valid = true;
                            $scope.CreditCardPromo.Amount = Math.floor($scope.initialPrice * 14 / 100);
                            // reset voucher
                            $scope.voucher.amount = 0;
                            $scope.voucher.checking = false;
                            $scope.voucher.checked = false;
                            $scope.voucher.confirmedCode = '';
                        }
                        return;
                    }

                    //**********
                    // Wonderful Wednesday with Visa
                    if (firstNum == 4 && $scope.initialPrice >= minOrder && jakartaDay == 3 && jakartaDate < endOfCampaign) {
                        $scope.CreditCardPromo.PromoName = 'Wonderful Wednesday with Visa';
                        $scope.CreditCardPromo.Type = 'visa';
                        $scope.CreditCardPromo.Valid = true;
                        $scope.CreditCardPromo.Amount = 50000;
                        // reset voucher
                        $scope.voucher.amount = 0;
                        $scope.voucher.checking = false;
                        $scope.voucher.checked = false;
                        $scope.voucher.confirmedCode = '';
                        return;
                    }

                    //**********
                    // Reset
                    $scope.CreditCardPromo.Valid = false;
                    $scope.CreditCardPromo.Amount = 0;

                } else {
                    $scope.CreditCardPromo.Valid = false;
                    $scope.CreditCardPromo.Amount = 0;
                }
            } else {
                $scope.CreditCardPromo.Valid = false;
                $scope.CreditCardPromo.Amount = 0;
            }
        }
    }//$scope.CreditCardPromo

    // expiry date
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

    // init function
    $scope.CheckoutConfig.GeneratePassenger();
    // countries
    $scope.Countries = Countries;
    //titles
    $scope.titles = [
            { name: 'Mr', value: 'Mister' },
            { name: 'Mrs', value: 'Mistress' },
            { name: 'Ms', value: 'Miss' }
    ];
    $scope.titleKids = [
            { name: 'Mr', value: 'Mister' },
            { name: 'Ms', value: 'Miss' }
    ];
    // return URL
    $scope.PageConfig.ReturnUrl = document.referrer == (window.location.origin + window.location.pathname + window.location.search) ? '/' : document.referrer;

    // print scope
    $scope.PrintScope = function() {
        console.log($scope);
    }
    $scope.PrintForm = function() {
        console.log($scope.PassengerForm.$error);
    }

    // date, months, and year
    $scope.dates = function (month, year) {
        var dates = [];
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
    $scope.months = [
            { value: 0, name: 'Januari' },
            { value: 1, name: 'Februari' },
            { value: 2, name: 'Maret' },
            { value: 3, name: 'April' },
            { value: 4, name: 'Mei' },
            { value: 5, name: 'Juni' },
            { value: 6, name: 'Juli' },
            { value: 7, name: 'Agustus' },
            { value: 8, name: 'September' },
            { value: 9, name: 'Oktober' },
            { value: 10, name: 'November' },
            { value: 11, name: 'Desember' }
    ];
    $scope.generateYear = function (type) {
        var departureDate = new Date($scope.flightDetail.departureFullDate);
        var years = [];

        function listYear(min, max) {
            for (var i = min; i <= max; i++) {
                years.push(i);
            }
        }

        switch (type) {
            case 'adult':
                listYear((departureDate.getFullYear() - 120), (departureDate.getFullYear() - 12));
                return years.reverse();
                break;
            case 'child':
                listYear((departureDate.getFullYear() - 12), (departureDate.getFullYear() - 2));
                return years.reverse();
                break;
            case 'infant':
                listYear((departureDate.getFullYear() - 2), departureDate.getFullYear());
                return years.reverse();
                break;
            case 'passport':
                listYear($scope.flightDetail.passportFullDate.getFullYear(), ($scope.flightDetail.passportFullDate.getFullYear() + 10));
                return years;
                break;
        }

    }

    $scope.incMth = function() {
        for (var z = 0; z < $scope.passengers.length; z++) {
            $scope.passengers[z].birth.month = parseInt($scope.passengers[z].birth.month, 10) + 1;
        }
    }

    // init passenger
    $scope.initPassenger = function (passenger) {
        if (passenger.type == 'adult') {
            passenger.birth = {
                date: $scope.flightDetail.departureDate,
                month: $scope.flightDetail.departureMonth,
                year: ($scope.flightDetail.departureYear - 12)
            };
        } else if (passenger.type == 'infant') {
            passenger.birth = {
                date: $scope.flightDetail.departureDate,
                month: $scope.flightDetail.departureMonth,
                year: $scope.bookingDate.getFullYear()
            };
        } else if (passenger.type == 'child') {
            passenger.birth = {
                date: $scope.flightDetail.departureDate,
                month: $scope.flightDetail.departureMonth,
                year: ($scope.flightDetail.departureYear - 2)
            };
        }
        if (nationalityRequired == true) {
            passenger.nationality = 'Indonesia';
        }
        passenger.passport = {
            expire: {
                date: $scope.flightDetail.passportDate,
                month: $scope.flightDetail.passportMonth,
                year: $scope.flightDetail.passportYear
            }
        }
    }

    
    // validate passenger birthday
    $scope.validateBirthday = function (passenger) {
        if (passenger.type != 'adult') {
            // set minimum date for passenger
            var minYear = -1;
            var currentDate = new Date();
            if (passenger.type == 'child') {
                minYear = $scope.flightDetail.minYearChild;
            } else if (passenger.type == 'infant') {
                minYear = $scope.flightDetail.minYearInfant;
            }

            if (passenger.birth.year == minYear) {
                if (passenger.birth.month <= $scope.flightDetail.departureMonth) {
                    passenger.birth.month = $scope.flightDetail.departureMonth;
                    if (passenger.birth.date < $scope.flightDetail.departureDate) {
                        passenger.birth.date = $scope.flightDetail.departureDate;
                    }
                }
            } else if (passenger.birth.year == $scope.bookingDate.getFullYear()) {
                if (passenger.birth.month >= $scope.flightDetail.departureMonth) {
                    passenger.birth.month = $scope.flightDetail.departureMonth;
                    if (passenger.birth.date > $scope.flightDetail.departureDate) {
                        passenger.birth.date = $scope.flightDetail.departureDate;
                    }
                }
            }
        }
    }
    // validate passport expiry date
    $scope.validatePassport = function (passenger) {
        if (passenger.passport.expire.year == $scope.flightDetail.passportYear) {
            if (passenger.passport.expire.month < $scope.flightDetail.passportMonth) {
                passenger.passport.expire.month = $scope.flightDetail.passportMonth;
                if (passenger.passport.expire.date < $scope.flightDetail.passportDate) {
                    passenger.passport.expire.date = $scope.flightDetail.passportDate;
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

    // general function end
    // *****

    // *****
    // paging function
    // set hash to page 1
    angular.element(document).ready(function () {
        //$scope.PageConfig.ActivePage = 1;
        $location.hash('page-1');
    });
    angular.element(window).on('hashchange', function () {
        if ($location.hash() == '') {
            $scope.PageConfig.ActivePage = 1;
            $location.hash('page-1');
        }
    });
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
    $scope.PageConfig.ChangePage = function(page) {
        $scope.PageConfig.ActivePage = page;
        $location.hash("page-"+page);
    }
    // paging function end
    // *****

    // *****
    // voucher function
    $scope.CheckoutConfig.Voucher = {
        Code: '',
        Validated: false,
        Validating: false,
        Valid: false,
        Amount: 0,
        Status: '',
        // validate voucher
        Check: function () {
            console.log('Validating Voucher');
            $scope.CheckoutConfig.Voucher.Validating = true;
            $http({
                method: 'GET',
                url: CheckVoucherConfig.Url,
                params: {
                    token: $scope.CheckoutConfig.Token,
                    code: $scope.CheckoutConfig.Voucher.Code,
                    email: $scope.CheckoutConfig.BuyerInfo.Email,
                    price: $scope.CheckoutConfig.Price
                }
            }).then(function (returnData) {
                console.log('Voucher code validated :');
                console.log(returnData);
                $scope.CheckoutConfig.Voucher.Validating = false;
                $scope.CheckoutConfig.Voucher.Validated = true;
                $scope.CheckoutConfig.Voucher.Status = returnData.data.ValidationStatus;
                if (returnData.data.Discount > 0) {
                    $scope.CheckoutConfig.Voucher.Valid = true;
                    $scope.CheckoutConfig.Voucher.Amount = returnData.data.Discount;
                } else {
                    $scope.CheckoutConfig.Voucher.Valid = false;
                    $scope.CheckoutConfig.Voucher.Status = returnData.data.ValidationStatus;
                }
            }, function (returnData) {
                $scope.CheckoutConfig.Voucher.Validated = true;
                $scope.CheckoutConfig.Voucher.Validating = false;
                $scope.CheckoutConfig.Voucher.Valid = false;
                console.log('Error validating voucher. Reason : ');
                console.log(returnData);
            });
        },
        // reset voucher
        Reset: function() {
            $scope.CheckoutConfig.Voucher.Validated = false;
            $scope.CheckoutConfig.Voucher.Code = '';
            $scope.CheckoutConfig.Voucher.Valid = false;
            $scope.CheckoutConfig.Voucher.Status = '';
            $scope.CheckoutConfig.Voucher.Amount = -1;
        }
    };

    // voucher function end
    // *****


}]);

// payment confirmation payment
app.controller('FlightConfirmationController', ['$http', '$scope', '$rootScope', '$interval', '$location', function ($http, $scope, $rootScope, $interval, $location) {

    $scope.PageConfig = $rootScope.PageConfig;
    $scope.DatePicker = $rootScope.DatePicker;

    $scope.UserForm = {
        Confirmation: {
            Name: '',
            Bank: {
                Name: '',
                Number: ''
            },
            Amount : 0
        }
    };

}]);