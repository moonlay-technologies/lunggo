// travorama angular app - payment controller
app.controller('paymentController', [
    '$http', '$scope', '$interval', '$location', function ($http, $scope, $interval, $location) {
        //********************
        // variables
        //********************
        // variables
        $scope.currentPage = 4;
        $scope.pageLoaded = true;
        $scope.loginShown = false;
        $scope.checkoutForm = {
            loading: false
        };
        $scope.paymentMethod = ''; //Payment
        $scope.trips = trips;
        $scope.stepClass = '';

        //CreditCard
        $scope.CreditCard = {
            TwoClickToken: 'false',
            Name: '',
            Month: '01',
            Year: 2016,
            Cvv: '',
            Number: ''
        };

        //Mandiri CLick Pay
        $scope.MandiriClickPay = {
            Token: '',
            CardNo: ''
        };

        $scope.msToTime = function (duration) {

            var milliseconds = parseInt((duration % 1000) / 100),
                 seconds = parseInt((duration / 1000) % 60),
                 minutes = parseInt((duration / (1000 * 60)) % 60),
                 hours = parseInt((duration / (1000 * 60 * 60)));
            //hours = parseInt((duration / (1000 * 60 * 60)) % 24);
            //days = parseInt((duration / (1000 * 60 * 60 * 24)));
            hours = hours;
            minutes = minutes;
            seconds = seconds;
            //var hours = convertedDuration[0];
            //var minutes = convertedDuration[1];
            return hours + "j " + minutes + "m";
        }

        //Unique Code For Bank Transfer
        $scope.TransferConfig = {
            UniqueCode: 0,
            Token: '',
            GetUniqueCode: function (rsvNo, voucherCode) {
                if (!rsvNo) {
                    rsvNo = $scope.rsvNo;
                }

                //Check Authorization
                var authAccess = getAuthAccess();
                if (authAccess == 1) {
                    $scope.getFlightHeader = 'Bearer ' + getCookie('accesstoken');
                }
                else {
                    $scope.getFlightHeader = null;
                }

                $http({
                    method: 'POST',
                    url: TransferConfig.Url,
                    data: {
                        rsvNo: rsvNo,
                        discCode: voucherCode
                    },
                    headers: { 'Authorization': $scope.getFlightHeader }
                }).then(function (returnData) {
                    console.log('Getting Unique Payment Code');
                    //console.log(returnData);
                    $scope.TransferConfig.UniqueCode = returnData.data.fee;
                }, function (returnData) {
                    console.log('Failed to get Unique Payment Code');
                    console.log(returnData);
                });
            }
        };
        $scope.currency = 'IDR';
        $scope.rsvNo = rsvNo;
        $scope.TransferConfig.GetUniqueCode($scope.rsvNo); //payment
        $scope.initialPrice = price;
        $scope.totalPrice = price;

        //Voucher
        $scope.voucher = {
            confirmedCode: '',
            code: '',
            amount: 0,
            status: '',
            checking: false,
            checked: false,
            check: function () {

                //Check Authorization
                var authAccess = getAuthAccess();
                if (authAccess == 1) {
                    $scope.getFlightHeader = 'Bearer ' + getCookie('accesstoken');
                }
                else {
                    $scope.getFlightHeader = null;
                }

                $scope.voucher.checking = true;
                $http({
                    method: 'POST',
                    url: CheckVoucherConfig.Url,
                    data: {
                        code: $scope.voucher.code,
                        rsvno: $scope.rsvNo
                    },
                    headers: { 'Authorization': $scope.getFlightHeader }
                }).then(function (returnData) {
                    //console.log(returnData);
                    $scope.voucher.checking = false;
                    $scope.voucher.checked = true;
                    if (returnData.data.discount > 0) {
                        $scope.voucher.amount = returnData.data.discount;
                        $scope.voucher.confirmedCode = $scope.voucher.code;
                        $scope.voucher.displayName = returnData.data.name;
                        // get unique code for transfer payment
                        $scope.voucher.status = 'Success';
                        $scope.TransferConfig.GetUniqueCode($scope.rsvNo, $scope.voucher.code);
                    }
                    else
                    {
                        $scope.voucher.checked = true;
                        $scope.voucher.status = returnData.data.error;
                    }
                }, function (returnData) {
                    $scope.voucher.checked = true;
                    $scope.voucher.checking = false;
                    $scope.voucher.status = returnData.data.error;
                });
            },
            reset: function () {
                $scope.voucher.code = '';
                $scope.voucher.amount = 0;
                $scope.voucher.confirmedCode = '';
                $scope.voucher.checked = false;
                // get unique code for transfer payment
                $scope.TransferConfig.GetUniqueCode($scope.rsvNo, $scope.voucher.code);
            }
        };

        $scope.pay = {
            url: FlightPayConfig.Url,
            postData: '',
            rsvNo: '',
            paying: false,
            ccdata: false,
            checked:false,
            isSuccess: '',
            ccChecked: false,
            setPaymentMethod: function () { //payment
                if ($scope.paymentMethod == 'CreditCard') {

                    Veritrans.url = VeritransTokenConfig.Url;
                    Veritrans.client_key = VeritransTokenConfig.ClientKey;
                    var card = function () {
                        if ($scope.CreditCard.TwoClickToken == 'false') {
                            $scope.pay.ccdata = true;
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

                            $scope.pay.send();

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
                    $scope.pay.send();
                }
            },
            send: function () {
                $scope.pay.isSuccess = "",
                $scope.pay.paying = true;
                $scope.pay.checked = false;
                //generate payment data
                if ($scope.paymentMethod == 'BankTransfer' || $scope.paymentMethod == 'MandiriBillPayment' || $scope.paymentMethod == 'CimbClicks') {
                    $scope.pay.postData = ' "rsvNo" : "' + $scope.rsvNo + '", "discCd":"' + $scope.voucher.confirmedCode + '" , "method":"' + $scope.paymentMethod + '"';
                }
                else
                {
                    switch ($scope.paymentMethod) {
                        case "CreditCard": //NotTested
                            //$scope.PaymentData = '{' + ' "tokenId":"' + $scope.CreditCard.Token + '",  "bank" :"' + $scope.buyerInfo.fullname + '","holderName":"' + $scope.CreditCard.Name + '","holderEmail":"' + $scope.buyerInfo.email + '","installmentTerm":"' + $scope.buyerInfo.email + '","bins":"' + $scope.buyerInfo.phone + '","saveTokenId":"' + $scope.buyerInfo.phone + '"' + '}';
                            $scope.PaymentData = '{' + ' "creditCard":' + '{' + ' "tokenId":"' + $scope.CreditCard.Token + '","holderName":"' + $scope.CreditCard.Name + '"' + '}'+ '}';//'{' + ' "tokenId":"' + $scope.CreditCard.Token + '","holderName":"' + $scope.CreditCard.Name + '"' + '}';
                            break;
                       case "MandiriClickPay": //Data belum benar, card number dan token
                           $scope.PaymentData = '{' + ' "mandiriClickPay":' + '{' + ' "cardNo":"' + $scope.MandiriClickPay.CardNo + '","token":"' + $scope.MandiriClickPay.Token + '"' + '}' + '}';
                            break;
                        //case "CimbClicks": //gk ada data
                        //    $scope.PaymentData = '{' + ' "description":"' + $scope.CreditCard.Token + '"' + '}';
                        //    break;
                        case "VirtualAccount": // Done
                            $scope.PaymentData = '{' + ' "bank":"Permata"' + '}'; 
                            break;
                        //case "MandiriBillPayment": //Data belum benar
                        //    $scope.PaymentData = '{' + ' "label1":"' + $scope.CreditCard.Token + '",  "value1" :"' + $scope.buyerInfo.fullname + '","label2":"' + $scope.CreditCard.Name + '","value2":"' + $scope.buyerInfo.email + '","label3":"' + $scope.buyerInfo.email + '","value3":"' + $scope.buyerInfo.phone + '","label4":"' + $scope.buyerInfo.phone + '","value4":"' + $scope.buyerInfo.phone + '"' + '}';
                        //    break;
                    }
                    $scope.pay.postData = ' "rsvNo" : "' + $scope.rsvNo + '", "discCd":"' + $scope.voucher.confirmedCode + '" , "data":' + $scope.PaymentData + ', "method":"' + $scope.paymentMethod + '"';
                }
                
                $scope.pay.postData = '{' + $scope.pay.postData + '}';
                $scope.pay.postData = JSON.parse($scope.pay.postData);

                //Check Authorization
                var authAccess = getAuthAccess();
                if (authAccess == 1) {
                    $scope.getFlightHeader = 'Bearer ' + getCookie('accesstoken');
                }
                else {
                    $scope.getFlightHeader = null;
                }

                //send form
                $http({
                    method: 'POST',
                    url: $scope.pay.url,
                    data: $scope.pay.postData,
                    headers: { 'Authorization': $scope.getFlightHeader }
                }).then(function (returnData) {
                    //console.log(returnData);

                    if (returnData.data.status == '200') {
                        $scope.pay.isSuccess = true;
                        $scope.pay.rsvNo = $scope.rsvNo;
                        $('form#rsvno input#rsvno-input').val($scope.pay.rsvNo);
                        $('form#rsvno input#url-input').val(returnData.data.redirectionUrl);//url-input
                        $('form#rsvno').submit();
                        $scope.pay.checked = true;
                    }
                    else {
                        $scope.pay.checked = true;
                        $scope.pay.isSuccess = false;
                        //Error Handling right Here
                        //console.log('Status : ' + returnData.status);
                        //console.log('Error : ' + returnData.data.error);
                        switch (returnData.data.error) {
                            case 'ERPPAY01':
                                $scope.errorMessage = 'Missing reservation number or method';
                                break;
                            case 'ERPPAY02':
                                $scope.errorMessage = 'Not authorized to use selected payment method';
                                break;
                            case 'ERPPAY03':
                                $scope.errorMessage = 'You have already choose one method before';
                                break;
                            case 'ERRGEN99':
                                $scope.errorMessage = 'There is a problem on the server';
                                break;
                        }
                        console.log($scope.errorMessage);
                    }
                }, function (returnData) {
                        console.log(returnData);
                        $scope.pay.checked = true;
                        $scope.pay.isSuccess = false;
                    });
                $scope.pay.paying = false;
            }
        }
        
        $scope.loggedIn = loggedIn;

        $scope.buyerInfo = {};
        if ($scope.loggedIn) {
            $scope.buyerInfo.fullname = buyerInfo.fullname;
            $scope.buyerInfo.countryCode = buyerInfo.countryCode;
            $scope.buyerInfo.phone = buyerInfo.phone;
            $scope.buyerInfo.email = buyerInfo.email;
        } else {
            $scope.buyerInfo = {};
        }

        // change page
        $scope.changePage = function (page) {
            $location.hash("page-" + page);
            // change current page variable
            $scope.currentPage = page;
            // change step class
            $scope.stepClass = 'active-' + page;

        }
        // change page after login
        $scope.changePage(currentPage);

        $scope.$watch(function () {
            return location.hash;
        }, function (value) {
            value = value.split('-');
            value = value[1];
            if (value > 0) {
                $scope.changePage(value);
            }
        });

        // toggle Travorama Login
        $scope.toggleLogin = function () {
            if ($scope.loginShown == false) {
                $scope.loginShown = true;
            } else {
                $scope.loginShown = false;
            }
        }

        // submit checkout form
        $scope.submitCheckoutForm = function () {
            $scope.checkoutForm.loading = true;
        }


        //********************
        // transfer window
        $scope.transferWindow = transferWindow;
        $scope.rightNow = new Date();
        $scope.rightNow = ($scope.rightNow.getHours() + '' + $scope.rightNow.getMinutes());
        $scope.rightNow = parseInt($scope.rightNow);
        $scope.transferWindowOpen = true;
        if ($scope.rightNow >= parseInt($scope.transferWindow[0]) && $scope.rightNow <= parseInt($scope.transferWindow[1])) {
            $scope.transferWindowOpen = true;
        } else {
            $scope.transferWindowOpen = false;
        }
        console.log($scope.rightNow);
        console.log(parseInt($scope.transferWindow[0]));
        console.log(parseInt($scope.transferWindow[1]));
        console.log($scope.transferWindowOpen);

        //********************
        // VISA Wonderful Wednesday Promo
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
                            var creditCardString = creditCardString.substr(0, 6);

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
        };


    }
]);
