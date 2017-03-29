// travorama angular app - payment controller
app.controller('paymentController', [
    '$http', '$scope', '$location', '$log',function ($http, $scope, $location, $log) {

        //********************
        // variables

        angular.element(document).ready(function () {
            $scope.UniqueCodePaymentConfig.GetUniqueCode($scope.rsvNo);
            
        });
        
        $scope.currentPage = 4;
        $scope.trial = 0;
        $scope.pageLoaded = true;
        $scope.loginShown = false;
        $scope.mustSelectBank = false;
        $scope.checkoutForm = {
            loading: false
        };
        $scope.paymentTimeout = paymentTimeout;
        $scope.paymentMethod = ''; 
        $scope.trips = trips;
        $scope.submethod = '';
        $scope.stepClass = '';
        $scope.redirectionUrl = redirectionUrl;
        $scope.expired = false;
        $scope.errorLog = '';
        $scope.errorMessage = '';
        $scope.currency = 'IDR';
        $scope.rsvNo = rsvNo;
        $scope.currentchoice = '';
        $scope.loggedIn = loggedIn;
        $scope.initialPrice = price;
        $scope.totalPrice = price;
        $scope.notifCardLength = false;
        $scope.originalPrice = originalPrice;
        $scope.buyerInfo = {};
        var date = new Date();
        $scope.randomYear = (date.getFullYear() + 2).toString();
        $scope.yearnow = date.getFullYear() + 2;
        $scope.dateOver = false;
        $scope.months = [
            { value: 0, name: 'Bulan' },
            { value: 01, name: 'Januari' },
            { value: 02, name: 'Februari' },
            { value: 03, name: 'Maret' },
            { value: 04, name: 'April' },
            { value: 05, name: 'Mei' },
            { value: 06, name: 'Juni' },
            { value: 07, name: 'Juli' },
            { value: 08, name: 'Agustus' },
            { value: 09, name: 'September' },
            { value: 10, name: 'Oktober' },
            { value: 11, name: 'November' },
            { value: 12, name: 'Desember' }
        ];

        $scope.MandiriClickPay = {
            Token: '',
            CardNo: ''
        };
        
        window.setInterval(function () {
            var nowTime = new Date();
            if (nowTime > $scope.paymentTimeout) {
                $scope.expired = true;
            }
        }, 1000);
        // ************************ CreditCard and BIN Discount *****************************
        $scope.CreditCard = {
            TwoClickToken: 'false',
            Name: '',
            Month: '01',
            Year: 2016,
            Cvv: '',
            Number: ''
        };

        $scope.binDiscount = {
            amount: 0,
            displayName: '',
            status: '',
            reset: function (method) {
                $scope.currentchoice = method;
                if (method == 'cc') {
                    if ($scope.binDiscount.amount != 0) {
                        $scope.binDiscount.available = true;
                    } else {
                        $scope.binDiscount.available = false;
                    }
                } else {
                    $scope.binDiscount.available = false;
                }
            },
            available: false,
            replaceDiscount: false,
            receive: false,
            checked: false,
            checking: false,
            text: '',
            tohex: function (str) {

                var hex, i;

                var result = "";
                for (i = 0; i < str.length; i++) {
                    hex = str.charCodeAt(i).toString(16);
                    result += ("000" + hex).slice(-4);
                }

                return result;

            },
            check: function () {
                if ($scope.trial > 3) {
                    $scope.trial = 0;
                }

                //Check Authorization
                var authAccess = getAuthAccess();
                $scope.binDiscount.checking = true;
                if (authAccess == 1 || authAccess == 2) {
                    var hash = CryptoJS.SHA512($scope.CreditCard.Number);
                    var hex = hash.toString(CryptoJS.enc.Hex);
                    $http({
                        method: 'POST',
                        url: CheckBinDiscountConfig.Url,
                        data: {
                            bin: $scope.CreditCard.Number.substring(0, 6),
                            hashedPan: hex,
                            rsvno: $scope.rsvNo,
                            voucherCode: $scope.voucher.code
                        },
                        headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                    }).then(function (returnData) {
                        //$log.debug(returnData);
                        if (returnData.data.status == 200) {
                            if (returnData.data.isAvailable) {
                                $scope.binDiscount.amount = returnData.data.amount;
                                $scope.binDiscount.displayName = returnData.data.name;
                                // get unique code for transfer payment
                                $scope.binDiscount.status = 'Success';
                                if ($scope.binDiscount.amount != 0) {
                                    $scope.binDiscount.text = 'Anda menggunakan promo ' + $scope.binDiscount.displayName + '.';
                                }
                                $scope.methodDiscount.available = false;
                                $scope.binDiscount.available = true;
                                $scope.binDiscount.replaceDiscount = returnData.data.replaceOriginalDiscount;
                                $scope.UniqueCodePaymentConfig.GetUniqueCode($scope.rsvNo, '', $scope.CreditCard.Number.substring(0, 6));
                            } else {
                                $scope.binDiscount.amount = 0;
                                $scope.binDiscount.checked = true;
                                $scope.binDiscount.status = returnData.data.error;
                                if (returnData.data.name != null) {
                                    $scope.binDiscount.text = 'Maaf, kuota promo ' + returnData.data.name + ' hari ini telah habis.';
                                } else {
                                    $scope.binDiscount.text = null;
                                }
                                $scope.binDiscount.replaceDiscount = false;
                                $scope.binDiscount.available = false;
                            }
                        }
                        else {
                            $scope.binDiscount.amount = 0;
                            $scope.binDiscount.checked = true;
                            $scope.binDiscount.status = returnData.data.error;
                            $scope.binDiscount.text = '';
                        }
                    }).catch(function () {
                        $scope.trial++;
                        if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                        {
                            $scope.binDiscount.check();
                        }
                        else {
                            $scope.binDiscount.amount = 0;
                            $scope.binDiscount.checked = true;
                            $scope.binDiscount.checking = false;
                            $scope.binDiscount.text = '';
                        }
                    });
                }
                else {
                    $log.debug('Not Authorized');
                    $scope.binDiscount.amount = 0;
                    $scope.binDiscount.checking = false;
                }
            }
        }

        $scope.methodDiscount = {
            amount: 0,
            displayName: '',
            status: '',
            firstRequest : true,
            reset: function (method) {
                $scope.currentchoice = method;
                if (method == 'VA') {
                    if ($scope.methodDiscount.amount != 0) {
                        $scope.methodDiscount.available = true;
                    } else {
                        $scope.methodDiscount.available = false;
                    }
                } else {
                    $scope.methodDiscount.available = false;
                }
            },
            available: false,
            replaceDiscount: false,
            receive: false,
            checked: false,
            checking: false,
            text: '',
            wait: false,
            check: function() {
                $scope.currentchoice = 'VA';
                if ($scope.methodDiscount.firstRequest) {
                    $scope.methodDiscount.wait = true;
                    if ($scope.trial > 3) {
                        $scope.trial = 0;
                    }
                    //Check Authorization
                    var authAccess = getAuthAccess();
                    $scope.methodDiscount.checking = true;
                    var vouchercode;
                    if ($scope.voucher.receive) {
                        vouchercode = $scope.voucher.code;
                    } else {
                        vouchercode = '';
                    }
                    if (authAccess == 1 || authAccess == 2) {
                        $http({
                            method: 'POST',
                            url: CheckMethodDiscountConfig.Url,
                            data: {
                                rsvno: $scope.rsvNo,
                                voucherCode: vouchercode
                            },
                            headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                        }).then(function(returnData) {
                            $scope.methodDiscount.wait = false;
                            $scope.methodDiscount.firstRequest = false;
                            if (returnData.data.status == 200) {
                                if (returnData.data.isAvailable) {
                                    $scope.methodDiscount.amount = returnData.data.amount;
                                    $scope.methodDiscount.displayName = returnData.data.name;
                                    // get unique code for transfer payment
                                    $scope.methodDiscount.status = 'Success';
                                    if ($scope.methodDiscount.amount != 0) {
                                        $scope.methodDiscount.text = 'Anda menggunakan promo ' + $scope.methodDiscount.displayName + '.';
                                    }
                                    if ($scope.paymentMethod == 'VirtualAccount') {
                                        $scope.methodDiscount.available = true;
                                        $scope.methodDiscount.replaceDiscount = returnData.data.replaceOriginalDiscount;
                                    } else {
                                        $scope.methodDiscount.available = false;
                                        $scope.methodDiscount.replaceDiscount = false;
                                    }

                                    $scope.binDiscount.replaceDiscount = false;
                                    $scope.UniqueCodePaymentConfig.GetUniqueCode($scope.rsvNo, '', $scope.CreditCard.Number.substring(0, 6));
                                } else {
                                    $scope.methodDiscount.amount = 0;
                                    $scope.methodDiscount.checked = true;
                                    $scope.methodDiscount.status = returnData.data.error;
                                    if (returnData.data.name != null) {
                                        $scope.methodDiscount.text = 'Maaf, kuota promo ' + returnData.data.name + ' hari ini telah habis.';
                                    } else {
                                        $scope.methodDiscount.text = null;
                                    }
                                    $scope.methodDiscount.available = false;
                                    $scope.methodDiscount.replaceDiscount = false;
                                }
                            } else {
                                $scope.methodDiscount.amount = 0;
                                $scope.methodDiscount.checked = true;
                                $scope.methodDiscount.status = returnData.data.error;
                                $scope.methodDiscount.text = '';
                            }
                        }).catch(function() {
                            $scope.trial++;
                            if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                            {
                                $scope.methodDiscount.check();
                            } else {
                                $scope.methodDiscount.wait = false;
                                $scope.methodDiscount.amount = 0;
                                $scope.methodDiscount.checked = true;
                                $scope.methodDiscount.checking = false;
                                $scope.methodDiscount.text = '';
                                $scope.methodDiscount.firstRequest = false;
                            }
                        });
                    } else {
                        $log.debug('Not Authorized');
                        $scope.methodDiscount.amount = 0;
                        $scope.methodDiscount.checking = false;
                        $scope.methodDiscount.firstRequest = false;
                    }
                } else {
                    if ($scope.methodDiscount.amount != 0) {
                        $scope.methodDiscount.available = true;
                    }
                }
            }
        }

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

        // ********************************** END *********************************************

        // ******************************** UNIQUE CODE ***************************************
        $scope.UniqueCodePaymentConfig = {
            UniqueCode: 0,
            Token: '',
            GetUniqueCode: function (rsvNo, voucherCode, bin) {

                if ($scope.trial > 3) {
                    $scope.trial = 0;
                }

                if (!rsvNo) {
                    rsvNo = $scope.rsvNo;
                }

                //Check Authorization
                var authAccess = getAuthAccess();
                if (authAccess == 1 || authAccess == 2) {
                    $http({
                        method: 'POST',
                        url: uniqueCodePaymentConfig.Url,
                        data: {
                            rsvNo: rsvNo,
                            discCode: voucherCode,
                            bin: bin,
                        },
                        headers: {
                            'Authorization': 'Bearer ' + getCookie('accesstoken')
                        }
                    }).then(function (returnData) {
                        $log.debug('Getting Unique Payment Code');
                        //$log.debug(returnData);
                        $scope.UniqueCodePaymentConfig.UniqueCode = returnData.data.code;
                    }).catch(function (returnData) {
                        $scope.trial++;
                        if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                        {
                            $scope.UniqueCodePaymentConfig.GetUniqueCode($scope.rsvNo, voucherCode);
                        }
                        else {
                            $log.debug('Failed to get Unique Payment Code');
                        }

                    });
                }
                else {
                    $log.debug('Not Authorized');
                }

            }
        };
       
        // ********************************** END *********************************************

        // ******************************** VOUCHER *******************************************
        $scope.voucher = {
            confirmedCode: '',
            receive: false,
            code: '',
            amount: 0,
            status: '',
            checking: false,
            checked: false,
            check: function () {
                if ($scope.trial > 3) {
                    $scope.trial = 0;
                }
                //Check Authorization
                var authAccess = getAuthAccess();
                $scope.voucher.checking = true;
                if (authAccess == 1 || authAccess == 2) {
                    $http({
                        method: 'POST',
                        url: CheckVoucherConfig.Url,
                        data: {
                            code: $scope.voucher.code,
                            rsvno: $scope.rsvNo
                        },
                        headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                    }).then(function (returnData) {
                        //$log.debug(returnData);
                        $scope.voucher.checking = false;
                        $scope.voucher.checked = true;
                        if (returnData.data.discount > 0) {
                            $scope.voucher.amount = returnData.data.discount;
                            $scope.voucher.confirmedCode = $scope.voucher.code;
                            $scope.voucher.displayName = returnData.data.name;
                            $scope.binDiscount.available = false;
                            $scope.methodDiscount.available = false;
                            $scope.binDiscount.replaceDiscount = false;
                            $scope.methodDiscount.replaceDiscount = false;
                            // get unique code for transfer payment
                            $scope.voucher.status = 'Success';
                            $scope.binDiscount.text = '';
                            $scope.methodDiscount.text = '';
                            $scope.UniqueCodePaymentConfig.GetUniqueCode($scope.rsvNo, $scope.voucher.code);
                            $scope.voucher.receive = true;
                        }
                        else {
                            $scope.voucher.checked = true;
                            $scope.voucher.status = returnData.data.error;
                            $scope.voucher.receive = false;
                        }
                    }).catch(function (returnData) {
                        $scope.trial++;
                        if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                        {
                            $scope.voucher.check();
                        }
                        else {
                            $scope.voucher.checked = true;
                            $scope.voucher.checking = false;
                        }
                    });
                }
                else {
                    $log.debug('Not Authorized');
                    $scope.voucher.checking = false;
                }
            },
            reset: function () {
                $scope.voucher.code = '';
                $scope.voucher.amount = 0;
                $scope.voucher.confirmedCode = '';
                $scope.voucher.checked = false;
                $scope.voucher.status = '';
                if ($scope.currentchoice == 'cc') {
                    $scope.binDiscount.check();
                }
                else if ($scope.currentchoice == 'VA') {
                    $scope.methodDiscount.check();
                }
                // get unique code for transfer payment
                $scope.UniqueCodePaymentConfig.GetUniqueCode($scope.rsvNo, $scope.voucher.code);
            }
        };
        
        // ********************************** END *********************************************

        // ****************************** SELECT BANK *****************************************
        $scope.selectBank = function(bank) {
            $scope.submethod = bank;
        }

        // ********************************** END *********************************************
        
        // ****************************** SUBMIT PAYMENT **************************************

        $scope.pay = {
            url: FlightPayConfig.Url,
            postData: '',
            continueVoucher: false,
            continueBIN: false,
            rsvNo: '',
            inputValid: true,
            paying: false,
            transfer: false,
            virtualAccount: false,
            go: false,
            test: 0,
            chooseOther : false,
            clickpay: false,
            ccdata: false,
            checked: false,
            isSuccess: false,
            ccChecked: false,
            setPaymentMethod: function () { //payment
                $scope.pay.continueBIN = false;
                $scope.pay.continueVoucher = false;
                if (!$scope.validation()) {
                    $scope.pay.inputValid = false;
                } else {
                    if ($scope.paymentMethod == 'CreditCard') {

                        if ($scope.CreditCard.Number == null || $scope.CreditCard.Number.length < 12 || $scope.CreditCard.Number.length > 19) {
                            $scope.notifCardLength = true;
                        } else {
                            $scope.notifCardLength = false;
                        Veritrans.url = VeritransTokenConfig.Url;
                        Veritrans.client_key = VeritransTokenConfig.ClientKey;
                            var card = function() {
                                var gross_amount = 0;
                                if ($scope.voucher.confirmedCode) {
                                    gross_amount = $scope.totalPrice - $scope.CreditCardPromo.Amount - $scope.voucher.amount + $scope.UniqueCodePaymentConfig.UniqueCode;
                                } else {
                                    if ($scope.binDiscount.available) {
                                        if ($scope.binDiscount.replaceDiscount) {
                                            gross_amount = $scope.originalPrice - $scope.CreditCardPromo.Amount + $scope.UniqueCodePaymentConfig.UniqueCode - $scope.binDiscount.amount;
                                        } else {
                                            gross_amount = $scope.totalPrice - $scope.CreditCardPromo.Amount + $scope.UniqueCodePaymentConfig.UniqueCode - $scope.binDiscount.amount;
                                        }
                                    } else {
                                        gross_amount = $scope.totalPrice - $scope.CreditCardPromo.Amount + $scope.UniqueCodePaymentConfig.UniqueCode;
                                    }
                                }
                                $log.debug("gross_amount = " + gross_amount);
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
                                    'gross_amount': gross_amount
                                }
                            } else {
                                return {
                                    'card_cvv': $scope.CreditCard.Cvv,
                                    'token_id': $scope.CreditCard.TwoClickToken,

                                    'two_click': true,
                                    'secure': true,
                                    'bank': 'mandiri',
                                    'gross_amount': gross_amount
                                }
                            }
                        };

                        // run the veritrans function to check credit card
                        Veritrans.token(card, callback);

                        function callback(response) {
                            if (response.redirect_url) {
                                // 3Dsecure transaction. Open 3Dsecure dialog
                                $log.debug('Open Dialog 3Dsecure');
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
                    } else {
                        if ($scope.paymentMethod == 'MandiriClickPay') {
                            if ($scope.MandiriClickPay.CardNo == null || $scope.MandiriClickPay.CardNo.length < 12 || $scope.MandiriClickPay.CardNo.length > 19) {
                                $scope.notifCardLength = true;
                            } else {
                                $scope.notifCardLength = false;
                        $scope.pay.send();
                    }
                        } else {
                            $scope.pay.send();
                }
                
                    }
                }
                
            },
            send: function () {
                if ($scope.trial > 3) {
                    $scope.trial = 0;
                }
                $scope.pay.isSuccess = false,
                $scope.pay.checked = false;
                $scope.pay.isPaying = false;
                //generate payment data
                if ($scope.paymentMethod == 'BankTransfer') {
                    if ($scope.redirectionUrl == null || $scope.redirectionUrl.length == 0) {
                        $scope.pay.postData = '"rsvNo" : "' + $scope.rsvNo + '", "discCd":"' + $scope.voucher.confirmedCode + '" , "method":"2", "submethod" : "1"' ;
                        $scope.pay.transfer = true;
                    }
                }
                else {
                    switch ($scope.paymentMethod) {
                        case "CreditCard":
                            var hash = CryptoJS.SHA512($scope.CreditCard.Number);
                            var hex = hash.toString(CryptoJS.enc.Hex);
                            $scope.PaymentData = '"method":"1","creditCard":' + '{' + ' "tokenId":"' + $scope.CreditCard.Token + '","holderName":"' + $scope.CreditCard.Name + '","hashedPan":"' + hex + '","reqBinDiscount":"' + $scope.binDiscount.available + '"}';
                            break;
                        case "MandiriClickPay":
                            var cardNoLast10 = $scope.MandiriClickPay.CardNo + "";
                            cardNoLast10 = cardNoLast10.substr(cardNoLast10.length - 10);
                            var rsvNoLast5 = $scope.rsvNo + "";
                            rsvNoLast5 = rsvNoLast5.substr(rsvNoLast5.length - 5);
                            var netprice = $scope.initialPrice - $scope.voucher.amount + $scope.UniqueCodePaymentConfig.UniqueCode;
                            $scope.PaymentData = '"method":"3","mandiriClickPay":' + '{' + ' "cardNo":"' + $scope.MandiriClickPay.CardNo + '","token":"' + $scope.MandiriClickPay.Token + '","cardNoLast10":"' + cardNoLast10 + '","amount":"' + netprice + '","rsvNoLast5":"' + rsvNoLast5 + '"' + '}';
                            $scope.pay.clickpay = true;
                            break;
                        case "CimbClicks":
                            $scope.PaymentData = '"method":"4","cimbClicks":' + '{' + ' "description":"Pembayaran melalui CimbClicks"' + '}';
                            break;
                        case "VirtualAccount":
                            $scope.PaymentData = '"method":"5","virtualAccount":' + '{' + ' "bank":"permata"' + '},' + '"submethod" : "3"';
                            $scope.pay.virtualAccount = true;
                            break;
                        case "MandiriBillPayment":
                            var label1 = "Payment for booking a flight";
                            var value1 = "debt";
                            $scope.PaymentData = '"method":"13","mandiriBillPayment":' + '{' + ' "label1":"' + label1 + '","value1":"' + value1 + '"' + '}';
                            break;
                    }
                    $scope.pay.postData = ' "rsvNo" : "' + $scope.rsvNo + '", "discCd":"' + $scope.voucher.confirmedCode + '",' + $scope.PaymentData;
                }

                $scope.pay.postData = '{' + $scope.pay.postData + '}';
                $scope.pay.postData = JSON.parse($scope.pay.postData);
                $log.debug($scope.pay.postData);
                if ($scope.paymentMethod != "BankTransfer" && $scope.paymentMethod != 'VirtualAccount' || ($scope.redirectionUrl != null && $scope.redirectionUrl.length != 0)) {
                    $scope.pay.go = true;
                    $scope.pay.bayar();
                }

            },
            close: function () {
                $scope.pay.checked = false;
                $scope.pay.isSuccess = false;
                $scope.pay.isPaying = false;
            },
            middle: function (method) {
                if (method == 'transfer') {
                    $scope.pay.transfer = false;
                    $scope.pay.go = true;
                    $scope.pay.bayar();
                }
                else if (method == 'virtualAccount') {
                    $scope.pay.virtualAccount = false;
                    $scope.pay.go = true;
                    $scope.pay.bayar();
                }
            },
            bayar: function () {
                $scope.pay.isPaying = true;
                var authAccess = getAuthAccess();
                if (authAccess == 1 || authAccess == 2) {
                    //send form
                    $http({
                        method: 'POST',
                        url: $scope.pay.url,
                        data: $scope.pay.postData,
                        headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                    }).then(function (returnData) {
                        $log.debug(returnData);

                        if (returnData.data.status == '200') {
                            $scope.pay.isSuccess = true;
                            $scope.pay.rsvNo = $scope.rsvNo;
                            $('form#rsvno input#rsvno-input').val($scope.pay.rsvNo);
                            $('form#rsvno input#url-input').val(returnData.data.redirectionUrl);
                            $('form#rsvno').submit();
                            $scope.redirectUrl = returnData.data.redirectionUrl;
                            $scope.pay.checked = true;
                        } else {
                            //Error Handling right Here
                            //$log.debug('Status : ' + returnData.status);
                            //$log.debug('Error : ' + returnData.data.error);
                            $scope.pay.go = false;
                            switch (returnData.data.error) {
                                case undefined:
                                    if (returnData.data.paymentStatus == 4) {
                                        $scope.errorLog = 'Payment Denied';
                                        $scope.errorMessage = 'Pembayaran Anda gagal. Silakan perbaiki data pembayaran atau gunakan alat pembayaran lainnya';
                                        $scope.pay.checked = true;
                                        $scope.pay.isSuccess = false;
                                        $scope.pay.isPaying = false;
                                        $scope.pay.chooseOther = true;
                                        $log.debug($scope.errorLog);
                                    } else {
                                        $scope.errorLog = 'Payment Denied';
                                        $scope.errorMessage = 'Terjadi kesalahan pada sistem. Silakan membayar menggunakan metode pembayaran lainnya';
                                        $scope.pay.checked = true;
                                        $scope.pay.isSuccess = false;
                                        $scope.pay.isPaying = false;
                                        $scope.pay.chooseOther = true;
                                        $log.debug($scope.errorLog);
                                    }
                                    break;
                                case 'ERPPAY01':
                                    $scope.errorLog = 'Missing reservation number or method';
                                    $scope.errorMessage = 'Nomor reservasi Anda tidak ditemukan';
                                    $scope.ReturnUrl = "/";
                                    $scope.pay.checked = true;
                                    $scope.pay.isSuccess = false;
                                    $scope.pay.isPaying = false;
                                    $log.debug($scope.errorLog);
                                    break;
                                case 'ERPPAY02':
                                    $scope.errorLog = 'Not authorised to use selected payment method';
                                    $scope.errorMessage = 'Silakan pilih metode pembayaran lain';
                                    $scope.PageConfig.ReturnUrl = windows.location;
                                    $scope.pay.checked = true;
                                    $scope.pay.isSuccess = false;
                                    $scope.pay.isPaying = false;
                                    $log.debug($scope.errorLog);
                                    break;
                                case 'ERPPAY03':
                                    $scope.errorLog = 'You have already choose one method before';
                                    $scope.errorMessage = 'Anda telah memilih metode pembayaran sebelumnya. Anda akan ' +
                                        'dialihkan ke halaman pembayaran';
                                    $('form#rsvno input#rsvno-input').val($scope.rsvNo);
                                    $('form#rsvno input#url-input').val($scope.redirectionUrl);
                                    $('form#rsvno').submit();
                                    $scope.pay.go = true;
                                    $scope.pay.checked = false;
                                    $scope.pay.isSuccess = true;

                                    break;
                                case 'ERPPAY04':
                                    $scope.errorLog = 'Reservation not found';
                                    $scope.errorMessage = 'Reservasi Anda tidak ditemukan';
                                    $scope.ReturnUrl = "/";
                                    $scope.pay.checked = true;
                                    $scope.pay.isSuccess = false;
                                    $scope.pay.isPaying = false;
                                    $log.debug($scope.errorLog);
                                    break;
                                case 'ERPPAY05':
                                    $scope.errorLog = 'Promo is over (voucher)';
                                    if (!$scope.pay.clickpay) {
                                        $scope.errorMessage = 'Mohon maaf, kuota promo ' + $scope.voucher.displayName + ' hari ini telah habis. Apakah Anda ingin melanjutkan pembayaran?';

                                    } else {
                                        $scope.errorMessage = 'Mohon maaf, kuota promo ' + $scope.voucher.displayName + ' hari ini telah habis. Silakan masukkan kembali token Anda untuk melanjutkan pembayaran';

                                    }
                                    $scope.voucher.amount = 0;
                                    $scope.voucher.confirmedCode = '';
                                    $scope.pay.continueVoucher = true;
                                    $scope.pay.checked = false;
                                    $scope.pay.isPaying = false;
                                    $scope.pay.go = false;

                                    break;
                                case 'ERPPAY06':
                                    $scope.errorLog = 'Promo is over (BIN)';
                                    $scope.errorMessage = 'Mohon maaf, kuota promo ' + $scope.binDiscount.displayName + ' hari ini telah habis. Apakah Anda ingin melanjutkan pembayaran?';
                                    $scope.binDiscount.replaceDiscount = false;
                                    $scope.binDiscount.available = false;
                                    $scope.binDiscount.amount = 0;
                                    $scope.pay.checked = false;
                                    $scope.pay.isPaying = false;
                                    $scope.pay.go = false;
                                    $scope.pay.continueBIN = true;

                                    $log.debug($scope.errorLog);
                                    break;
                                case 'ERRGEN98':
                                    $scope.errorLog = 'Invalid JSON Format';
                                    $scope.errorMessage = 'Terjadi kesalahan pada sistem';
                                    $scope.ReturnUrl = "/";
                                    $scope.pay.checked = true;
                                    $scope.pay.isSuccess = false;
                                    $scope.pay.isPaying = false;
                                    $log.debug($scope.errorLog);
                                    break;
                                case 'ERRGEN99':
                                    $scope.errorLog = 'There is a problem on the server';
                                    $scope.errorMessage = 'Terjadi kesalahan pada sistem';
                                    $scope.ReturnUrl = "/";
                                    $scope.pay.checked = true;
                                    $scope.pay.isSuccess = false;
                                    $scope.pay.isPaying = false;
                                    $log.debug($scope.errorLog);
                                    break;
                            }
                        }
                    }).catch(function (returnData) {
                        $scope.trial++;
                        if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                        {
                            $scope.pay.send();
                        }
                        else {
                            $scope.pay.checked = true;
                            $scope.pay.isSuccess = false;
                            $scope.pay.isPaying = false;
                        }
                    });
                }
                else {
                    $log.debug('Not Authorized');
                    $scope.pay.checked = false;
                    $scope.pay.isSuccess = false;
                    $scope.pay.isPaying = false;
                }
            },
            continuePay: function (term) {
                if (term == 'voucher') {

                    $scope.pay.continueVoucher = false;
                    $scope.pay.test = 0;
                    $scope.pay.setPaymentMethod();

                } else {
                    $scope.pay.continueBIN = false;
                    $scope.binDiscount.displayName = '';
                    $scope.pay.test = 0;
                    $scope.pay.setPaymentMethod();
                }
            }
        }

        // ********************************** END *********************************************

        // ********************************* LOG IN *******************************************
        if ($scope.loggedIn) {
            $scope.buyerInfo.fullname = buyerInfo.fullname;
            $scope.buyerInfo.countryCode = buyerInfo.countryCode;
            $scope.buyerInfo.phone = buyerInfo.phone;
            $scope.buyerInfo.email = buyerInfo.email;
        } else {
            $scope.buyerInfo = {};
        }
        
        // ********************************** END *********************************************

        // ****************************** TRANSFER WINDOW *************************************

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
        $log.debug($scope.rightNow);
        $log.debug(parseInt($scope.transferWindow[0]));
        $log.debug(parseInt($scope.transferWindow[1]));
        $log.debug($scope.transferWindowOpen);

        // ********************************** END *********************************************

        // ********************************** OTHERS ******************************************

        //TBD
        $scope.hotelstar = function () {
            if ($scope.hotelDetails.starRating == 1) {
                return 'star';
            }
            if ($scope.hotelDetails.starRating == 2) {
                return 'star star-2';
            }
            if ($scope.hotelDetails.starRating == 3) {
                return 'star star-3';
            }
            if ($scope.hotelDetails.starRating == 4) {
                return 'star star-4';
            }
            if ($scope.hotelDetails.starRating == 5) {
                return 'star star-5';
            }
        }

        $scope.msToTime = function (duration) {

            var seconds = parseInt((duration / 1000) % 60),
                 minutes = parseInt((duration / (1000 * 60)) % 60),
                 hours = parseInt((duration / (1000 * 60 * 60)));
       
            hours = hours;
            minutes = minutes;
            seconds = seconds;
            return hours + "j " + minutes + "m";
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

        $scope.validation = function() {
            if ($scope.paymentMethod == 'CreditCard') {
                if (!$scope.checkNumber($scope.CreditCard.Number) || !$scope.checkName($scope.CreditCard.Name)
                    || !$scope.checkDate($scope.CreditCard.Month, $scope.CreditCard.Year)) {
                    if (!$scope.checkDate($scope.CreditCard.Month, $scope.CreditCard.Year)) {
                        $('.ccDate').addclass('has-error');
                    }
                    return false;
                } else {
                    return true;
                }
            } else if ($scope.paymentMethod == 'MandiriClickPay') {
                if (!$scope.checkNumber($scope.MandiriClickPay.CardNo) || !$scope.checkNumber($scope.MandiriClickPay.Token)) {
                    return false;
                } else {
                    return true;
                }
            } else {
                return true;
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

        $scope.generateYear = function () {
            var now = new Date();
            var yearNow = now.getFullYear();
            var years = [];

            function listYear(min, max) {
                for (var i = min; i <= max; i++) {
                    years.push(i);
                }
            }
            listYear(yearNow, (yearNow + 20));
            //years = years.reverse();
            return ['Tahun'].concat(years);
        }

        $scope.$watch('paymentMethod', function(newValue, oldValue) {
            if (newValue != oldValue) {
                $scope.notifCardLength = false;
                $scope.dateOver = false;
            }
        },true);
        // ********************************** END *********************************************
    }
]);

// Travorama Account controller
app.controller('b2BPaymentController', [
    '$http', '$scope', '$log', function ($http, $scope, $log) {

        var hash = (location.hash);
        // variables
        $scope.CreditCardData = {
            TwoClickToken: 'false',
            Name: '',
            Month: '01',
            Year: 2016,
            Cvv: '',
            Number: ''
        };

        $scope.months = [
            { value: 0, name: 'Bulan' },
            { value: 01, name: 'Januari' },
            { value: 02, name: 'Februari' },
            { value: 03, name: 'Maret' },
            { value: 04, name: 'April' },
            { value: 05, name: 'Mei' },
            { value: 06, name: 'Juni' },
            { value: 07, name: 'Juli' },
            { value: 08, name: 'Agustus' },
            { value: 09, name: 'September' },
            { value: 10, name: 'Oktober' },
            { value: 11, name: 'November' },
            { value: 12, name: 'Desember' }
        ];

        //General Variables
        $scope.pageLoaded = true;
        $scope.email = 'Test';
        $scope.trial = 0;
        $scope.loading = false;
        $scope.currentSection = '';
        $scope.profileForm = {
            active: false
        };
        $scope.passwordForm = {
            active: false
        };
        $scope.isConfirm = false;
        $scope.close = function () {
            $scope.isConfirm = false;
        }

        $scope.userProfile = {
            email: '',
            name: '',
            countryCallCd: '',
            phone: '',
            edit: false,
            updating: false
        }

        $scope.bookingstatus = function (types) {
            if (types == 12)
                return 'Approved';
            else if (types == 13)
                return 'Pending';
            else if (types == 6)
                return 'Expired';
            else if (types == 11)
                return 'Rejected';
            else if (types == 8)
                return 'Cancelled';
            else if (types == 10)
                return 'Failed';
            else if (types == 0)
                return 'Pending';
        }

        $scope.password = {
            edit: false,
            updating: false,
            failed: false
        }

        $scope.closePopUp = function() {
            $scope.primaryUpdated = false;
            $scope.cardDeleted = false;
            window.location.reload();
        }

        $scope.countries = [{ "name": "Afghanistan", "dial_code": "93", "code": "AF" }, { "name": "Aland Islands", "dial_code": "358", "code": "AX" }, { "name": "Albania", "dial_code": "355", "code": "AL" }, { "name": "Algeria", "dial_code": "213", "code": "DZ" }, { "name": "AmericanSamoa", "dial_code": "1 684", "code": "AS" }, { "name": "Andorra", "dial_code": "376", "code": "AD" }, { "name": "Angola", "dial_code": "244", "code": "AO" }, { "name": "Anguilla", "dial_code": "1 264", "code": "AI" }, { "name": "Antarctica", "dial_code": "672", "code": "AQ" }, { "name": "Antigua and Barbuda", "dial_code": "1268", "code": "AG" }, { "name": "Argentina", "dial_code": "54", "code": "AR" }, { "name": "Armenia", "dial_code": "374", "code": "AM" }, { "name": "Aruba", "dial_code": "297", "code": "AW" }, { "name": "Australia", "dial_code": "61", "code": "AU" }, { "name": "Austria", "dial_code": "43", "code": "AT" }, { "name": "Azerbaijan", "dial_code": "994", "code": "AZ" }, { "name": "Bahamas", "dial_code": "1 242", "code": "BS" }, { "name": "Bahrain", "dial_code": "973", "code": "BH" }, { "name": "Bangladesh", "dial_code": "880", "code": "BD" }, { "name": "Barbados", "dial_code": "1 246", "code": "BB" }, { "name": "Belarus", "dial_code": "375", "code": "BY" }, { "name": "Belgium", "dial_code": "32", "code": "BE" }, { "name": "Belize", "dial_code": "501", "code": "BZ" }, { "name": "Benin", "dial_code": "229", "code": "BJ" }, { "name": "Bermuda", "dial_code": "1 441", "code": "BM" }, { "name": "Bhutan", "dial_code": "975", "code": "BT" }, { "name": "Bolivia, Plurinational State of", "dial_code": "591", "code": "BO" }, { "name": "Bosnia and Herzegovina", "dial_code": "387", "code": "BA" }, { "name": "Botswana", "dial_code": "267", "code": "BW" }, { "name": "Brazil", "dial_code": "55", "code": "BR" }, { "name": "British Indian Ocean Territory", "dial_code": "246", "code": "IO" }, { "name": "Brunei Darussalam", "dial_code": "673", "code": "BN" }, { "name": "Bulgaria", "dial_code": "359", "code": "BG" }, { "name": "Burkina Faso", "dial_code": "226", "code": "BF" }, { "name": "Burundi", "dial_code": "257", "code": "BI" }, { "name": "Cambodia", "dial_code": "855", "code": "KH" }, { "name": "Cameroon", "dial_code": "237", "code": "CM" }, { "name": "Canada", "dial_code": "1", "code": "CA" }, { "name": "Cape Verde", "dial_code": "238", "code": "CV" }, { "name": "Cayman Islands", "dial_code": " 345", "code": "KY" }, { "name": "Central African Republic", "dial_code": "236", "code": "CF" }, { "name": "Chad", "dial_code": "235", "code": "TD" }, { "name": "Chile", "dial_code": "56", "code": "CL" }, { "name": "China", "dial_code": "86", "code": "CN" }, { "name": "Christmas Island", "dial_code": "61", "code": "CX" }, { "name": "Cocos (Keeling) Islands", "dial_code": "61", "code": "CC" }, { "name": "Colombia", "dial_code": "57", "code": "CO" }, { "name": "Comoros", "dial_code": "269", "code": "KM" }, { "name": "Congo", "dial_code": "242", "code": "CG" }, { "name": "Congo, The Democratic Republic of the Congo", "dial_code": "243", "code": "CD" }, { "name": "Cook Islands", "dial_code": "682", "code": "CK" }, { "name": "Costa Rica", "dial_code": "506", "code": "CR" }, { "name": "Cote d'Ivoire", "dial_code": "225", "code": "CI" }, { "name": "Croatia", "dial_code": "385", "code": "HR" }, { "name": "Cuba", "dial_code": "53", "code": "CU" }, { "name": "Cyprus", "dial_code": "357", "code": "CY" }, { "name": "Czech Republic", "dial_code": "420", "code": "CZ" }, { "name": "Denmark", "dial_code": "45", "code": "DK" }, { "name": "Djibouti", "dial_code": "253", "code": "DJ" }, { "name": "Dominica", "dial_code": "1 767", "code": "DM" }, { "name": "Dominican Republic", "dial_code": "1 849", "code": "DO" }, { "name": "Ecuador", "dial_code": "593", "code": "EC" }, { "name": "Egypt", "dial_code": "20", "code": "EG" }, { "name": "El Salvador", "dial_code": "503", "code": "SV" }, { "name": "Equatorial Guinea", "dial_code": "240", "code": "GQ" }, { "name": "Eritrea", "dial_code": "291", "code": "ER" }, { "name": "Estonia", "dial_code": "372", "code": "EE" }, { "name": "Ethiopia", "dial_code": "251", "code": "ET" }, { "name": "Falkland Islands (Malvinas)", "dial_code": "500", "code": "FK" }, { "name": "Faroe Islands", "dial_code": "298", "code": "FO" }, { "name": "Fiji", "dial_code": "679", "code": "FJ" }, { "name": "Finland", "dial_code": "358", "code": "FI" }, { "name": "France", "dial_code": "33", "code": "FR" }, { "name": "French Guiana", "dial_code": "594", "code": "GF" }, { "name": "French Polynesia", "dial_code": "689", "code": "PF" }, { "name": "Gabon", "dial_code": "241", "code": "GA" }, { "name": "Gambia", "dial_code": "220", "code": "GM" }, { "name": "Georgia", "dial_code": "995", "code": "GE" }, { "name": "Germany", "dial_code": "49", "code": "DE" }, { "name": "Ghana", "dial_code": "233", "code": "GH" }, { "name": "Gibraltar", "dial_code": "350", "code": "GI" }, { "name": "Greece", "dial_code": "30", "code": "GR" }, { "name": "Greenland", "dial_code": "299", "code": "GL" }, { "name": "Grenada", "dial_code": "1 473", "code": "GD" }, { "name": "Guadeloupe", "dial_code": "590", "code": "GP" }, { "name": "Guam", "dial_code": "1 671", "code": "GU" }, { "name": "Guatemala", "dial_code": "502", "code": "GT" }, { "name": "Guernsey", "dial_code": "44", "code": "GG" }, { "name": "Guinea", "dial_code": "224", "code": "GN" }, { "name": "Guinea-Bissau", "dial_code": "245", "code": "GW" }, { "name": "Guyana", "dial_code": "595", "code": "GY" }, { "name": "Haiti", "dial_code": "509", "code": "HT" }, { "name": "Holy See (Vatican City State)", "dial_code": "379", "code": "VA" }, { "name": "Honduras", "dial_code": "504", "code": "HN" }, { "name": "Hong Kong", "dial_code": "852", "code": "HK" }, { "name": "Hungary", "dial_code": "36", "code": "HU" }, { "name": "Iceland", "dial_code": "354", "code": "IS" }, { "name": "India", "dial_code": "91", "code": "IN" }, { "name": "Indonesia", "dial_code": "62", "code": "ID" }, { "name": "Iran, Islamic Republic of Persian Gulf", "dial_code": "98", "code": "IR" }, { "name": "Iraq", "dial_code": "964", "code": "IQ" }, { "name": "Ireland", "dial_code": "353", "code": "IE" }, { "name": "Isle of Man", "dial_code": "44", "code": "IM" }, { "name": "Israel", "dial_code": "972", "code": "IL" }, { "name": "Italy", "dial_code": "39", "code": "IT" }, { "name": "Jamaica", "dial_code": "1 876", "code": "JM" }, { "name": "Japan", "dial_code": "81", "code": "JP" }, { "name": "Jersey", "dial_code": "44", "code": "JE" }, { "name": "Jordan", "dial_code": "962", "code": "JO" }, { "name": "Kazakhstan", "dial_code": "7 7", "code": "KZ" }, { "name": "Kenya", "dial_code": "254", "code": "KE" }, { "name": "Kiribati", "dial_code": "686", "code": "KI" }, { "name": "Korea, Democratic People's Republic of Korea", "dial_code": "850", "code": "KP" }, { "name": "Korea, Republic of South Korea", "dial_code": "82", "code": "KR" }, { "name": "Kuwait", "dial_code": "965", "code": "KW" }, { "name": "Kyrgyzstan", "dial_code": "996", "code": "KG" }, { "name": "Laos", "dial_code": "856", "code": "LA" }, { "name": "Latvia", "dial_code": "371", "code": "LV" }, { "name": "Lebanon", "dial_code": "961", "code": "LB" }, { "name": "Lesotho", "dial_code": "266", "code": "LS" }, { "name": "Liberia", "dial_code": "231", "code": "LR" }, { "name": "Libyan Arab Jamahiriya", "dial_code": "218", "code": "LY" }, { "name": "Liechtenstein", "dial_code": "423", "code": "LI" }, { "name": "Lithuania", "dial_code": "370", "code": "LT" }, { "name": "Luxembourg", "dial_code": "352", "code": "LU" }, { "name": "Macao", "dial_code": "853", "code": "MO" }, { "name": "Macedonia", "dial_code": "389", "code": "MK" }, { "name": "Madagascar", "dial_code": "261", "code": "MG" }, { "name": "Malawi", "dial_code": "265", "code": "MW" }, { "name": "Malaysia", "dial_code": "60", "code": "MY" }, { "name": "Maldives", "dial_code": "960", "code": "MV" }, { "name": "Mali", "dial_code": "223", "code": "ML" }, { "name": "Malta", "dial_code": "356", "code": "MT" }, { "name": "Marshall Islands", "dial_code": "692", "code": "MH" }, { "name": "Martinique", "dial_code": "596", "code": "MQ" }, { "name": "Mauritania", "dial_code": "222", "code": "MR" }, { "name": "Mauritius", "dial_code": "230", "code": "MU" }, { "name": "Mayotte", "dial_code": "262", "code": "YT" }, { "name": "Mexico", "dial_code": "52", "code": "MX" }, { "name": "Micronesia, Federated States of Micronesia", "dial_code": "691", "code": "FM" }, { "name": "Moldova", "dial_code": "373", "code": "MD" }, { "name": "Monaco", "dial_code": "377", "code": "MC" }, { "name": "Mongolia", "dial_code": "976", "code": "MN" }, { "name": "Montenegro", "dial_code": "382", "code": "ME" }, { "name": "Montserrat", "dial_code": "1664", "code": "MS" }, { "name": "Morocco", "dial_code": "212", "code": "MA" }, { "name": "Mozambique", "dial_code": "258", "code": "MZ" }, { "name": "Myanmar", "dial_code": "95", "code": "MM" }, { "name": "Namibia", "dial_code": "264", "code": "NA" }, { "name": "Nauru", "dial_code": "674", "code": "NR" }, { "name": "Nepal", "dial_code": "977", "code": "NP" }, { "name": "Netherlands", "dial_code": "31", "code": "NL" }, { "name": "Netherlands Antilles", "dial_code": "599", "code": "AN" }, { "name": "New Caledonia", "dial_code": "687", "code": "NC" }, { "name": "New Zealand", "dial_code": "64", "code": "NZ" }, { "name": "Nicaragua", "dial_code": "505", "code": "NI" }, { "name": "Niger", "dial_code": "227", "code": "NE" }, { "name": "Nigeria", "dial_code": "234", "code": "NG" }, { "name": "Niue", "dial_code": "683", "code": "NU" }, { "name": "Norfolk Island", "dial_code": "672", "code": "NF" }, { "name": "Northern Mariana Islands", "dial_code": "1 670", "code": "MP" }, { "name": "Norway", "dial_code": "47", "code": "NO" }, { "name": "Oman", "dial_code": "968", "code": "OM" }, { "name": "Pakistan", "dial_code": "92", "code": "PK" }, { "name": "Palau", "dial_code": "680", "code": "PW" }, { "name": "Palestinian Territory, Occupied", "dial_code": "970", "code": "PS" }, { "name": "Panama", "dial_code": "507", "code": "PA" }, { "name": "Papua New Guinea", "dial_code": "675", "code": "PG" }, { "name": "Paraguay", "dial_code": "595", "code": "PY" }, { "name": "Peru", "dial_code": "51", "code": "PE" }, { "name": "Philippines", "dial_code": "63", "code": "PH" }, { "name": "Pitcairn", "dial_code": "872", "code": "PN" }, { "name": "Poland", "dial_code": "48", "code": "PL" }, { "name": "Portugal", "dial_code": "351", "code": "PT" }, { "name": "Puerto Rico", "dial_code": "1 939", "code": "PR" }, { "name": "Qatar", "dial_code": "974", "code": "QA" }, { "name": "Romania", "dial_code": "40", "code": "RO" }, { "name": "Russia", "dial_code": "7", "code": "RU" }, { "name": "Rwanda", "dial_code": "250", "code": "RW" }, { "name": "Reunion", "dial_code": "262", "code": "RE" }, { "name": "Saint Barthelemy", "dial_code": "590", "code": "BL" }, { "name": "Saint Helena, Ascension and Tristan Da Cunha", "dial_code": "290", "code": "SH" }, { "name": "Saint Kitts and Nevis", "dial_code": "1 869", "code": "KN" }, { "name": "Saint Lucia", "dial_code": "1 758", "code": "LC" }, { "name": "Saint Martin", "dial_code": "590", "code": "MF" }, { "name": "Saint Pierre and Miquelon", "dial_code": "508", "code": "PM" }, { "name": "Saint Vincent and the Grenadines", "dial_code": "1 784", "code": "VC" }, { "name": "Samoa", "dial_code": "685", "code": "WS" }, { "name": "San Marino", "dial_code": "378", "code": "SM" }, { "name": "Sao Tome and Principe", "dial_code": "239", "code": "ST" }, { "name": "Saudi Arabia", "dial_code": "966", "code": "SA" }, { "name": "Senegal", "dial_code": "221", "code": "SN" }, { "name": "Serbia", "dial_code": "381", "code": "RS" }, { "name": "Seychelles", "dial_code": "248", "code": "SC" }, { "name": "Sierra Leone", "dial_code": "232", "code": "SL" }, { "name": "Singapore", "dial_code": "65", "code": "SG" }, { "name": "Slovakia", "dial_code": "421", "code": "SK" }, { "name": "Slovenia", "dial_code": "386", "code": "SI" }, { "name": "Solomon Islands", "dial_code": "677", "code": "SB" }, { "name": "Somalia", "dial_code": "252", "code": "SO" }, { "name": "South Africa", "dial_code": "27", "code": "ZA" }, { "name": "South Georgia and the South Sandwich Islands", "dial_code": "500", "code": "GS" }, { "name": "Spain", "dial_code": "34", "code": "ES" }, { "name": "Sri Lanka", "dial_code": "94", "code": "LK" }, { "name": "Sudan", "dial_code": "249", "code": "SD" }, { "name": "Suriname", "dial_code": "597", "code": "SR" }, { "name": "Svalbard and Jan Mayen", "dial_code": "47", "code": "SJ" }, { "name": "Swaziland", "dial_code": "268", "code": "SZ" }, { "name": "Sweden", "dial_code": "46", "code": "SE" }, { "name": "Switzerland", "dial_code": "41", "code": "CH" }, { "name": "Syrian Arab Republic", "dial_code": "963", "code": "SY" }, { "name": "Taiwan", "dial_code": "886", "code": "TW" }, { "name": "Tajikistan", "dial_code": "992", "code": "TJ" }, { "name": "Tanzania, United Republic of Tanzania", "dial_code": "255", "code": "TZ" }, { "name": "Thailand", "dial_code": "66", "code": "TH" }, { "name": "Timor-Leste", "dial_code": "670", "code": "TL" }, { "name": "Togo", "dial_code": "228", "code": "TG" }, { "name": "Tokelau", "dial_code": "690", "code": "TK" }, { "name": "Tonga", "dial_code": "676", "code": "TO" }, { "name": "Trinidad and Tobago", "dial_code": "1 868", "code": "TT" }, { "name": "Tunisia", "dial_code": "216", "code": "TN" }, { "name": "Turkey", "dial_code": "90", "code": "TR" }, { "name": "Turkmenistan", "dial_code": "993", "code": "TM" }, { "name": "Turks and Caicos Islands", "dial_code": "1 649", "code": "TC" }, { "name": "Tuvalu", "dial_code": "688", "code": "TV" }, { "name": "Uganda", "dial_code": "256", "code": "UG" }, { "name": "Ukraine", "dial_code": "380", "code": "UA" }, { "name": "United Arab Emirates", "dial_code": "971", "code": "AE" }, { "name": "United Kingdom", "dial_code": "44", "code": "GB" }, { "name": "United States", "dial_code": "1", "code": "US" }, { "name": "Uruguay", "dial_code": "598", "code": "UY" }, { "name": "Uzbekistan", "dial_code": "998", "code": "UZ" }, { "name": "Vanuatu", "dial_code": "678", "code": "VU" }, { "name": "Venezuela, Bolivarian Republic of Venezuela", "dial_code": "58", "code": "VE" }, { "name": "Vietnam", "dial_code": "84", "code": "VN" }, { "name": "Virgin Islands, British", "dial_code": "1 284", "code": "VG" }, { "name": "Virgin Islands, U.S.", "dial_code": "1 340", "code": "VI" }, { "name": "Wallis and Futuna", "dial_code": "681", "code": "WF" }, { "name": "Yemen", "dial_code": "967", "code": "YE" }, { "name": "Zambia", "dial_code": "260", "code": "ZM" }, { "name": "Zimbabwe", "dial_code": "263", "code": "ZW" }];


        // Pass Data into MVC Controller
        $scope.submitRsvNo = function (rsvNo) {
            $('form#rsvno input#rsvno-input').val(rsvNo);
            $('form#rsvno').submit();
        }

        $scope.getCountries = function (dialCode) {
            for (var i = 0; i < $scope.countries.length; i++) {
                if ($scope.countries[i].dial_code == dialCode) {
                    return $scope.countries[i].name;
                }
            }
        }

        //Get SavedCreditCard

        $scope.creditCard = {
            getCreditCard: function () {
                if ($scope.trial > 3) {
                    $scope.trial = 0;
                }
                var authAccess = getAuthAccess();
                $scope.loading = true;
                if (authAccess == 2) {
                    $http({
                        method: 'GET',
                        url: GetCreditCardConfig.Url,
                        headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                    }).then(function (returnData) {
                        $scope.loading = false;
                        if (returnData.data.status == "200") {
                            $log.debug('Success getting Transaction');
                            $scope.dataCreditCard = returnData.data.creditCards;
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
                            $scope.creditCard.getCreditCard();
                        }
                        else {
                            $log.debug('Failed to Get CreditCard');
                        }
                    });
                }
                else {
                    $log.debug('Not Authorized');
                }
            }
        }

        $scope.updatePrimaryCard = function (maskedCardNumber) {
            $scope.primaryUpdated = false;
            var authAccess = getAuthAccess();
            if (authAccess == 2) {
                //authorized
                $http({
                    url: SetPrimaryCardConfig.Url,
                    method: 'POST',
                    data: {
                        maskedCardNumber: maskedCardNumber
                    },
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).then(function (returnData) {
                    //$log.debug(returnData);
                    if (returnData.data.status == '200') {
                        $log.debug('Success updating primary card');
                        $scope.primaryUpdated = true;

                    }
                    else {
                        $log.debug(returnData.data.error);
                        $log.debug(returnData);
                        $scope.primaryUpdated = false;
                    }
                }).catch(function (returnData) {
                    $scope.trial++;
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.updatePrimaryCard(rsvNo, status);
                    }
                    else {
                        $log.debug('Failed Update Primary Card');
                        $log.debug(returnData);
                        $scope.primaryUpdated = false;
                    }
                });
            }
            else { //if not authorized
                $scope.primaryUpdated = false;
            }

        }

        $scope.deleteCard = function (maskedCardNo) {
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
                }).then(function (returnData) {
                    if (returnData.data.status == '200') {
                        $log.debug('Success Delete User');
                        $scope.cardDeleted = true;
                    }
                    else {
                        $log.debug(returnData.data.error);
                        $log.debug(returnData);
                        $scope.cardDeleted = false;
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
                        $scope.cardDeleted = false;
                    }
                });
            }
            else { //if not authorized
                $scope.cardDeleted = false;
            }
        }

        //Change Profile and Password

        $scope.addCreditCard = {
            inputValid: true,
            ccdata: false,
            getPreToken: function () {
                if (!$scope.validation()) {
                    $scope.addCreditCard.inputValid = false;
                } else {
                    if ($scope.CreditCardData.Number == null || $scope.CreditCardData.Number.length < 12 || $scope.CreditCardData.Number.length > 19) {
                        $scope.notifCardLength = true;
                    } else {
                        $scope.notifCardLength = false;
                        Veritrans.url = VeritransTokenConfig.Url;
                        Veritrans.client_key = VeritransTokenConfig.ClientKey;
                        var card = function () {
                            var gross_amount = 15000;
                            $log.debug("gross_amount = " + gross_amount);
                            if ($scope.CreditCardData.TwoClickToken == 'false') {
                                $scope.addCreditCard.ccdata = true;

                                return {
                                    'card_number': $scope.CreditCardData.Number,
                                    'card_exp_month': $scope.CreditCardData.Month,
                                    'card_exp_year': $scope.CreditCardData.Year,
                                    'card_cvv': $scope.CreditCardData.Cvv,

                                    // Set 'secure', 'bank', and 'gross_amount', if the merchant wants transaction to be processed with 3D Secure
                                    'secure': true,
                                    'bank': 'mandiri',
                                    'gross_amount': gross_amount
                                }
                            } else {
                                return {
                                    'card_cvv': $scope.CreditCardData.Cvv,
                                    'token_id': $scope.CreditCardData.TwoClickToken,

                                    'two_click': true,
                                    'secure': true,
                                    'bank': 'mandiri',
                                    'gross_amount': gross_amount
                                }
                            }
                        };

                        // run the veritrans function to check credit card
                        Veritrans.token(card, callback);

                        function callback(response) {
                            if (response.redirect_url) {
                                // 3Dsecure transaction. Open 3Dsecure dialog
                                $log.debug('Open Dialog 3Dsecure');
                                openDialog(response.redirect_url);

                            } else if (response.status_code == '200') {
                                // success 3d secure or success normal
                                //close 3d secure dialog if any
                                closeDialog();

                                // store token data in input #token_id and then submit form to merchant server
                                $("#vt-token").val(response.token_id);
                                $scope.CreditCardData.Token = response.token_id;

                                $scope.addCreditCard.addCC();

                            } else {
                                // failed request token
                                //close 3d secure dialog if any
                                closeDialog();
                                $('#submit-button').removeAttr('disabled');
                                // Show status message.
                                $('#message').text(response.status_message);
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
            addCC: function() {
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
                            token: $scope.CreditCardData.Token,
                            cardHolderName: $scope.CreditCardData.Name,
                            cardExpirymonth: $scope.CreditCardData.Month,
                            cardExpiryYear : $scope.CreditCardData.Year
                        },
                        headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                    }).then(function (returnData) {
                        //$log.debug(returnData);
                        if (returnData.data.status == '200') {
                            $log.debug('Success Adding new Credit Card');
                            $scope.addCreditCard.updating = false;
                            $scope.addCreditCard.updated = true;
                        }
                        else {
                            $log.debug(returnData.data.error);
                            $log.debug(returnData);
                            $scope.addCreditCard.edit = true;
                            $scope.addCreditCard.updating = false;
                        }
                    }).catch(function (returnData) {
                        $scope.trial++;
                        if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                        {
                            $scope.addCreditCard(name);
                        }
                        else {
                            $log.debug('Failed requesting change profile');
                            $scope.addCreditCard.edit = true;
                            $scope.addCreditCard.updating = false;
                        }
                    });
                }
                else { //if not authorized
                    $scope.addCreditCard.edit = true;
                    $scope.addCreditCard.updating = false;
                }
            }
            
        }

        $scope.validation = function () {
            if (!$scope.checkNumber($scope.CreditCardData.Number) || !$scope.checkName($scope.CreditCardData.Name)
                    || !$scope.checkDate($scope.CreditCardData.Month, $scope.CreditCardData.Year)) {
                if (!$scope.checkDate($scope.CreditCardData.Month, $scope.CreditCardData.Year)) {
                    $('.ccDate').addclass('has-error');
                }
                return false;
            } else {
                return true;
            }
        }

        $scope.generateYear = function () {
            var now = new Date();
            var yearNow = now.getFullYear();
            var years = [];

            function listYear(min, max) {
                for (var i = min; i <= max; i++) {
                    years.push(i);
                }
            }
            listYear(yearNow, (yearNow + 20));
            //years = years.reverse();
            return ['Tahun'].concat(years);
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

        //Executing Get Credit Card
        $scope.creditCard.getCreditCard();

        $scope.changeSection = function (name) {
            $scope.currentSection = name;
        }

        if (hash == '#order') {
            $scope.changeSection('order');
        } else {
            $scope.changeSection('profile');
        }

    }
]);// account controller end

jQuery(document).ready(function ($) {
    // Payment Desktop
    $('input[name="PMInput"]').click(function () {
        var val = $(this).val();
        $('.selected-bank').hide();

        var id = $('input[value="' + val + '"]').closest('.selected-bank');
        $(id).show();

        $('input[value="' + val + '"]').attr('checked', true);
    });

    // Payment Mobile
    $('input[name="paymentMethod"]').click(function () {
        var val = $(this).val();
        $('.selected-bank').hide();

        var id = $('input[value="' + val + '"]').closest('.selected-bank');
        $(id).show();

        $('html, body').animate({
            scrollTop: $("#" + val).offset().top
        }, 700);

        $('input[value="' + val + '"]').attr('checked', true);
    });
})
