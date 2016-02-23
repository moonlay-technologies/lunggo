// checkout controller
app.controller('CheckoutController', ['$http', '$scope', '$rootScope', '$interval', '$location', function ($http, $scope, $rootScope, $interval, $location) {
    
    // *****
    // general variables
    $scope.PageConfig = $rootScope.PageConfig;

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
            if ($scope.CheckoutConfig.Passenger[0] > 0) {
                for (var i = 0; i < $scope.CheckoutConfig.Passenger[0]; i++) {
                    var x = { typeName: $scope.CheckoutConfig.PassengerTypeName[0], type: 'adult' };
                    $scope.CheckoutConfig.Passengers.push(x);
                }
            }
            if ($scope.CheckoutConfig.Passenger[1]> 0) {
                for (var i = 0; i < $scope.CheckoutConfig.Passenger[1]; i++) {
                    var x = { typeName: $scope.CheckoutConfig.PassengerTypeName[1], type: 'child' };
                    $scope.CheckoutConfig.Passengers.push(x);
                }
            }
            if ($scope.CheckoutConfig.Passenger[2] > 0) {
                for (var i = 0; i < $scope.CheckoutConfig.Passenger[2]; i++) {
                    var x = { typeName: $scope.CheckoutConfig.PassengerTypeName[2], type: 'infant' };
                    $scope.CheckoutConfig.Passengers.push(x);
                }
            }
        },
        // identity requirement
        PassportRequired: CheckoutDetail.PassportRequired,
        IdRequired: CheckoutDetail.IdRequired,
        NationalityRequired: CheckoutDetail.NationalityRequired,
        // buyer info
        BuyerInfo: CheckoutDetail.BuyerInfo
    };

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
        Method : ''
    };

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
                listYear((departureDate.getFullYear() - 2), $scope.bookingDate.getFullYear());
                return years.reverse();
                break;
            case 'passport':
                listYear($scope.flightDetail.passportFullDate.getFullYear(), ($scope.flightDetail.passportFullDate.getFullYear() + 10));
                return years;
                break;
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