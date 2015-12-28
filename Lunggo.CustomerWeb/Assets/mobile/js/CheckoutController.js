app.controller('CheckoutController', ['$http', '$scope', '$rootScope', '$interval', '$location', function($http, $scope, $rootScope, $interval, $location) {
    
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
    // init function
    $scope.CheckoutConfig.GeneratePassenger();
    // countries
    $scope.Countries = Countries;

    // return URL
    $scope.PageConfig.ReturnUrl = document.referrer == (window.location.origin + window.location.pathname + window.location.search) ? '/' : document.referrer;

    // print scope
    $scope.PrintScope = function() {
        console.log($scope);
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