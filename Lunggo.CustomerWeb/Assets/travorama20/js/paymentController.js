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
        $scope.hotelDetails = hotelDetails;
        $scope.totalRoom = totalRoom;
        $scope.checkin = checkin;
        $scope.checkout = checkout;
        $scope.nights = nights;
        $scope.totalpax = totalpax;
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
                if (method == 'cc') {
                    if ($scope.binDiscount.amount != 0) {
                        $scope.binDiscount.replaceDiscount = true;
                    }
                } else {
                    $scope.binDiscount.replaceDiscount = false;
                }
                $scope.currentchoice = method;
            },
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
                                $scope.binDiscount.replaceDiscount = true;
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
                            $scope.binDiscount.replaceDiscount = false;
                            // get unique code for transfer payment
                            $scope.voucher.status = 'Success';
                            $scope.binDiscount.text = '';
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
                                if ($scope.binDiscount.replaceDiscount && !$scope.voucher.confirmedCode) {
                                    gross_amount = $scope.originalPrice - $scope.CreditCardPromo.Amount + $scope.UniqueCodePaymentConfig.UniqueCode - $scope.binDiscount.amount;
                                } else {
                                    gross_amount = $scope.totalPrice - $scope.CreditCardPromo.Amount - $scope.voucher.amount + $scope.UniqueCodePaymentConfig.UniqueCode;
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
                            $scope.PaymentData = '"method":"1","creditCard":' + '{' + ' "tokenId":"' + $scope.CreditCard.Token + '","holderName":"' + $scope.CreditCard.Name + '","hashedPan":"' + hex + '","reqBinDiscount":"' + $scope.binDiscount.replaceDiscount + '"}';
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
                                        $scope.errorMessage = 'Alat pembayaran Anda tidak dapat digunakan. Silakan membayar menggunakan alat pembayaran lainnya';
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
            }
        },true);
        // ********************************** END *********************************************
    }
]);

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
        }, 1000);

        $('input[value="' + val + '"]').attr('chekcked', true);
    });
})
