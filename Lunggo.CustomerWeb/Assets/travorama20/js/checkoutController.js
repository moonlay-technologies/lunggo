// travorama angular app - checkout controller
app.controller('checkoutController', [
    '$http', '$scope', '$interval',function($http, $scope, $interval) {

        //********************
        // variables
        $scope.currentPage = 1;
        $scope.pageLoaded = true;
        $scope.loginShown = false;
        $scope.checkoutForm = {
            loading: false
        };
        $scope.paymentMethod = '';
        $scope.stepClass = '';
        $scope.titles = [
            { name: 'Mr', value: 'mr' },
            { name: 'Mrs', value: 'mrs' },
            { name: 'Ms', value: 'ms' }
        ];

        $scope.token = token;
        $scope.trips = trips;
        $scope.initialPrice = price;
        $scope.totalPrice = price;
        $scope.expired = false;
        $scope.expiryDate = new Date(expiryDate);
        $interval(function () {
            var nowTime = new Date();
            if (nowTime > $scope.expiryDate) {
                $scope.expired = true;
            }
        }, 1000);
        $scope.voucher = {
            confirmedCode: '',
            code: '',
            amount: 0,
            checking: false,
            checked: false,
            check: function() {
                $scope.voucher.checking = true;
                $http({
                    method: 'GET',
                    url: CheckVoucherConfig.Url,
                    data: {
                        token: $scope.token,
                        code: $scope.voucher.code,
                        email: $scope.buyerInfo.email
                    }
                }).then(function(returnData) {
                    console.log('Success Getting Voucher Code');
                    console.log(returnData);
                    if (returnData.Amount > 0) {
                        $scope.voucher.amount = returnData.Amount;
                        $scope.voucher.confirmedCode = $scope.voucher.code;
                    }
                    $scope.voucher.checked = true;
                    $scope.voucher.checking = false;
                }, function(returnData) {
                    console.log('Failed to Checking Voucher Code');
                    console.log(returnData);
                    $scope.voucher.checked = true;
                    $scope.voucher.checking = false;
                });
            }
        };

        $scope.booking = false;

        $scope.passengers = [];
        $scope.passengersForm = {
            valid: false,
            checked: false
        };

        $scope.buyerInfo = {};
        $scope.adultPassenger = [];
        $scope.childPassenger = [];
        $scope.infantPassenger = [];

        $scope.months = [
            { value: 0, name: 'January' },
            { value: 1, name: 'February' },
            { value: 2, name: 'March' },
            { value: 3, name: 'April' },
            { value: 4, name: 'May' },
            { value: 5, name: 'June' },
            { value: 6, name: 'July' },
            { value: 7, name: 'August' },
            { value: 8, name: 'September' },
            { value: 9, name: 'October' },
            { value: 10, name: 'November' },
            { value: 11, name: 'December' },
        ];

        $scope.flightDetail = {};

        $scope.flightDetail.departureFullDate = '2016-10-12'; // development only. Please change the value to actual date on production
        $scope.flightDetail.departureDate = -1;
        $scope.flightDetail.departureMonth = -1;
        $scope.flightDetail.departureYear = -1;
        $scope.flightDetail.minYearChild = -1;
        $scope.flightDetail.minYearInfant = -1;
        $scope.flightDetail.passportFullDate = -1;
        $scope.flightDetail.passportDate = -1;
        $scope.flightDetail.passportMonth = -1;
        $scope.flightDetail.passportYear = -1;
        $scope.flightDetail.generateDepartureDate = function(fullDate) {
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

        $scope.passportRequired = passportRequired;
        $scope.idRequired = idRequired;
        $scope.nationalityRequired = nationalityRequired;

        $scope.countries = ["Afghanistan", "Albania", "Algeria", "Andorra", "Angola", "Anguilla", "Antigua &amp; Barbuda", "Argentina", "Armenia", "Aruba", "Australia", "Austria", "Azerbaijan", "Bahamas", "Bahrain", "Bangladesh", "Barbados", "Belarus", "Belgium", "Belize", "Benin", "Bermuda", "Bhutan", "Bolivia", "Bosnia &amp; Herzegovina", "Botswana", "Brazil", "British Virgin Islands", "Brunei", "Bulgaria", "Burkina Faso", "Burundi", "Cambodia", "Cameroon", "Cape Verde", "Cayman Islands", "Chad", "Chile", "China", "Colombia", "Congo", "Cook Islands", "Costa Rica", "Cote D Ivoire", "Croatia", "Cruise Ship", "Cuba", "Cyprus", "Czech Republic", "Denmark", "Djibouti", "Dominica", "Dominican Republic", "Ecuador", "Egypt", "El Salvador", "Equatorial Guinea", "Estonia", "Ethiopia", "Falkland Islands", "Faroe Islands", "Fiji", "Finland", "France", "French Polynesia", "French West Indies", "Gabon", "Gambia", "Georgia", "Germany", "Ghana", "Gibraltar", "Greece", "Greenland", "Grenada", "Guam", "Guatemala", "Guernsey", "Guinea", "Guinea Bissau", "Guyana", "Haiti", "Honduras", "Hong Kong", "Hungary", "Iceland", "India", "Indonesia", "Iran", "Iraq", "Ireland", "Isle of Man", "Israel", "Italy", "Jamaica", "Japan", "Jersey", "Jordan", "Kazakhstan", "Kenya", "Kuwait", "Kyrgyz Republic", "Laos", "Latvia", "Lebanon", "Lesotho", "Liberia", "Libya", "Liechtenstein", "Lithuania", "Luxembourg", "Macau", "Macedonia", "Madagascar", "Malawi", "Malaysia", "Maldives", "Mali", "Malta", "Mauritania", "Mauritius", "Mexico", "Moldova", "Monaco", "Mongolia", "Montenegro", "Montserrat", "Morocco", "Mozambique", "Namibia", "Nepal", "Netherlands", "Netherlands Antilles", "New Caledonia", "New Zealand", "Nicaragua", "Niger", "Nigeria", "Norway", "Oman", "Pakistan", "Palestine", "Panama", "Papua New Guinea", "Paraguay", "Peru", "Philippines", "Poland", "Portugal", "Puerto Rico", "Qatar", "Reunion", "Romania", "Russia", "Rwanda", "Saint Pierre &amp; Miquelon", "Samoa", "San Marino", "Satellite", "Saudi Arabia", "Senegal", "Serbia", "Seychelles", "Sierra Leone", "Singapore", "Slovakia", "Slovenia", "South Africa", "South Korea", "Spain", "Sri Lanka", "St Kitts &amp; Nevis", "St Lucia", "St Vincent", "St. Lucia", "Sudan", "Suriname", "Swaziland", "Sweden", "Switzerland", "Syria", "Taiwan", "Tajikistan", "Tanzania", "Thailand", "Timor L'Este", "Togo", "Tonga", "Trinidad &amp; Tobago", "Tunisia", "Turkey", "Turkmenistan", "Turks &amp; Caicos", "Uganda", "Ukraine", "United Arab Emirates", "United Kingdom", "Uruguay", "Uzbekistan", "Venezuela", "Vietnam", "Virgin Islands (US)", "Yemen", "Zambia", "Zimbabwe"];

        //********************
        // general functions
        // generate years
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
                    listYear((departureDate.getFullYear() - 12), departureDate.getFullYear());
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
        // generate date
        $scope.dates = function (month, year) {
            var dates = [];
            var maxDate = -1;
            // check leap year
            if (year % 4 == 0 &&  month == 1) {
                maxDate = 29;
            } else {
                if (month == 2) {
                    maxDate = 28;
                } else if (month == 3 || month == 5 || month == 8 || month == 10 || month == 12 ) {
                    maxDate = 30;
                } else{
                    maxDate = 31;
                }
            }
            for (var i = 1; i <= maxDate; i++) {
                dates.push(i);
            }
            return dates;
        }
        // init passenger
        $scope.initPassenger = function (passenger) {
            if (passenger.type == 'adult') {
                passenger.birth = {
                    date: 1,
                    month: 0,
                    year: 2000
                };
            } else {
                passenger.birth = {
                    date: $scope.flightDetail.departureDate,
                    month: $scope.flightDetail.departureMonth,
                    year: $scope.flightDetail.departureYear
                };
            }
            if (nationalityRequired == true) {
                passenger.nationality = 'Indonesia';
            }
            passenger.passport = {
                expire: {
                    date: $scope.flightDetail.passportDate,
                    month: $scope.flightDetail.passportMonth,
                    year : $scope.flightDetail.passportYear
                }
            }
        }
        // validate passenger birthday
        $scope.validateBirthday = function (passenger) {
            if (passenger.type != 'adult') {
                // set minimum date for passenger
                var minYear = -1;
                if (passenger.type == 'child') {
                    minYear = $scope.flightDetail.minYearChild;
                } else if (passenger.type == 'infant') {
                    minYear = $scope.flightDetail.minYearInfant;
                }

                if (passenger.birth.year == minYear) {
                    if (passenger.birth.month < $scope.flightDetail.departureMonth) {
                        passenger.birth.month = $scope.flightDetail.departureMonth;
                        if (passenger.birth.date < $scope.flightDetail.departureDate) {
                            passenger.birth.date = $scope.flightDetail.departureDate;
                        }
                    }
                }
            }
        }
        // validate passport expiry date
        $scope.validatePassport = function(passenger) {
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
        // generate passenger
        $scope.generatePassenger = function () {
            if (adultPassenger > 0) {
                for (var i = 0; i < adultPassenger; i++) {
                    var x = { typeName: adultTypeName, type: 'adult' };
                    $scope.passengers.push(x);
                }
            }
            if (childPassenger > 0) {
                for (var i = 0; i < childPassenger; i++) {
                    var x = { typeName: childTypeName, type: 'child' };
                    $scope.passengers.push(x);
                }
            }
            if (infantPassenger > 0) {
                for (var i = 0; i < infantPassenger; i++) {
                    var x = { typeName: infantTypeName, type: 'infant' };
                    $scope.passengers.push(x);
                }
            }
        }
        $scope.generatePassenger();
        // change page
        $scope.changePage = function (page) {
            // check if page target is 4
            // do validation if page target is 4
            /*
            if ($scope.currentPage == 3 && page == 4) {

                // check each form
                for (var i = 0; i < $scope.passengers.length; i++) {

                    if ($scope.passengers[i].firstname && $scope.passengers[i].lastname && $scope.passengers[i].birthday) {
                        $scope.passengers[i].valid = true;
                    } else {
                        $scope.passengers[i].valid = false;
                    }

                    // if passport required
                    if ($scope.passportRequired) {
                        if ($scope.passengers[i].passport.number && $scope.passengers[i].passport.expire) {
                            $scope.passengers[i].valid = true;
                        } else {
                            $scope.passengers[i].valid = false;
                        }
                    }

                    // if id required
                    if ($scope.idRequired) {
                        if ($scope.passengers[i].idNumber) {
                            $scope.passengers[i].valid = true;
                        } else {
                            $scope.passengers[i].valid = false;
                        }
                    }

                    // check if 
                    if ($scope.passengers[i].valid == true) {
                        $scope.passengersForm.valid = true;
                    } else {
                        $scope.passengersForm.valid = false;
                    }
                    $scope.passengersForm.checked = true;

                }

                // check all form valid
                if ($scope.passengersForm.valid == true) {
                    $scope.currentPage = page;
                    $scope.stepClass = 'active-' + page;
                }


            } else {
            */
                // change current page variable
                $scope.currentPage = page;
                // change step class
                $scope.stepClass = 'active-' + page;
            // }

        }
        // change page after login
        $scope.changePage(currentPage);
        // $scope.changePage(4); // development only

        // toggle Travorama Login
        $scope.toggleLogin = function() {
            if ($scope.loginShown == false) {
                $scope.loginShown = true;
            } else {
                $scope.loginShown = false;
            }
        }

        // submit checkout form
        $scope.submitCheckoutForm = function() {
            $scope.checkoutForm.loading = true;
        }

        // log $scope
        $scope.testForm = function () {
            console.log($scope);
        }

    }
]);// checkout controller

// travorama angular app - confirmation controller
app.controller('confirmationController', [
    '$http', '$scope', function($http, $scope) {

        $scope.pageLoaded = true;

    }
]);// checkout controller