// travorama angular app - payment controller
app.controller('paymentController', [
    '$http', '$scope', '$location', '$log', function ($http, $scope, $location, $log) {

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
        $scope.bankName = '';
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
        $scope.totalPrice = Math.round(price);
        $scope.notifCardLength = false;
        $scope.originalPrice = originalPrice;
        $scope.buyerInfo = {};
        var date = new Date();
        $scope.randomYear = (date.getFullYear() + 2).toString();
        $scope.yearnow = date.getFullYear() + 2;
        $scope.dateOver = false;

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
            // TwoClickToken: 'false',
            Number: '',
            Name: '',
            Month: '',
            Year: '',
            Cvv: ''
        };

        //// bin == Bank Identification Number, untuk identifikasi dari bank mana
        $scope.binDiscount = {
            amount: 0,
            displayName: '',
            status: '',
            reset: function (method) {
                $scope.currentchoice = method;
                ////  available = true if amount <> 0 AND method cc
                $scope.binDiscount.available = $scope.binDiscount.amount != 0 && method == 'cc';
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
                        } else {
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
            firstRequest: true,
            reset: function (method) {
                $scope.currentchoice = method;
                ////  available = true if amount <> 0 AND method va
                $scope.methodDiscount.available = $scope.methodDiscount.amount != 0 && method == 'VA';
            },
            available: false,
            replaceDiscount: false,
            receive: false,
            checked: false,
            checking: false,
            text: '',
            wait: false,
            check: function () {
                $scope.currentchoice = 'VA';
                if ($scope.methodDiscount.firstRequest) {
                    $scope.methodDiscount.wait = true;
                    if ($scope.trial > 3) $scope.trial = 0;
                    //Check Authorization
                    $scope.methodDiscount.checking = true;
                    var vouchercode = ($scope.voucher.receive) ? $scope.voucher.code : '';
                    var authAccess = getAuthAccess();
                    if (authAccess == 1 || authAccess == 2) {
                        $http({
                            method: 'POST',
                            url: CheckMethodDiscountConfig.Url,
                            data: {
                                rsvno: $scope.rsvNo,
                                voucherCode: vouchercode
                            },
                            headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                        }).then(function (result) {
                            $scope.methodDiscount.wait = false;
                            $scope.methodDiscount.firstRequest = false;
                            var data = result.data;
                            if (data.status == 200) {
                                if (data.isAvailable) {
                                    $scope.methodDiscount.amount = data.amount;
                                    $scope.methodDiscount.displayName = data.name;
                                    // get unique code for transfer payment
                                    $scope.methodDiscount.status = 'Success';
                                    if ($scope.methodDiscount.amount != 0) {
                                        $scope.methodDiscount.text = 'Anda menggunakan promo ' + $scope.methodDiscount.displayName + '.';
                                    }
                                    if ($scope.paymentMethod == 'VirtualAccount') {
                                        $scope.methodDiscount.available = true;
                                        $scope.methodDiscount.replaceDiscount = data.replaceOriginalDiscount;
                                    } else {
                                        $scope.methodDiscount.available = false;
                                        $scope.methodDiscount.replaceDiscount = false;
                                    }
                                    $scope.binDiscount.replaceDiscount = false;
                                    $scope.UniqueCodePaymentConfig.GetUniqueCode($scope.rsvNo, '', $scope.CreditCard.Number.substring(0, 6));
                                } else {
                                    $scope.methodDiscount.amount = 0;
                                    $scope.methodDiscount.checked = true;
                                    $scope.methodDiscount.status = data.error;
                                    $scope.methodDiscount.text = (data.name) ? 'Maaf, kuota promo ' + data.name + ' hari ini telah habis.' : null;
                                    $scope.methodDiscount.available = false;
                                    $scope.methodDiscount.replaceDiscount = false;
                                }
                            } else {
                                $scope.methodDiscount.amount = 0;
                                $scope.methodDiscount.checked = true;
                                $scope.methodDiscount.status = data.error;
                                $scope.methodDiscount.text = '';
                            }
                        }).catch(function () {
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
                    $scope.methodDiscount.available = $scope.methodDiscount.amount != 0;
                }
            }
        }

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

        // ********************************** END ******************************************************************************************

        // ****************************** SUBMIT PAYMENT **************************************

        $scope.pay = {
            url: FlightPayConfig.Url,
            continueVoucher: false,
            continueBIN: false,
            rsvNo: '',
            inputValid: true,
            paying: false,
            transfer: false,
            virtualAccount: false,
            go: false,
            test: 0,
            chooseOther: false,
            clickpay: false,
            ccdata: false,
            checked: false,
            isSuccess: false,
            postData: {
                method: '',
                submethod: ''
            },
            ccChecked: false,
            setPaymentMethod: function () {
                $scope.pay.continueBIN = false;
                $scope.pay.continueVoucher = false;
                if ($scope.paymentMethod == 'CreditCard') {
                    if ($scope.ccValidation() == false) {
                        $scope.pay.inputValid = false;
                        return;
                    }
                    var ccNumber = $scope.CreditCard.Number;
                    if (ccNumber == null || ccNumber.length < 12 || ccNumber.length > 19) {
                        $scope.notifCardLength = true;
                    } else {
                        $scope.notifCardLength = false;
                        Veritrans.url = VeritransTokenConfig.Url;
                        Veritrans.client_key = VeritransTokenConfig.ClientKey;
                        var card = function () {
                            $scope.pay.ccdata = true;
                            var gross_amount = $scope.totalPrice - $scope.voucher.amount +
                                    $scope.UniqueCodePaymentConfig.UniqueCode + $scope.getMdr();
                            // if ($scope.voucher.confirmedCode) {
                            // } else {
                            // if ($scope.binDiscount.available) {
                            //     if ($scope.binDiscount.replaceDiscount) {
                            //         gross_amount = $scope.originalPrice + $scope.UniqueCodePaymentConfig.UniqueCode - $scope.binDiscount.amount;
                            //     } else {
                            //         gross_amount = $scope.totalPrice + $scope.UniqueCodePaymentConfig.UniqueCode - $scope.binDiscount.amount;
                            //     }
                            // } else {
                            // gross_amount = $scope.totalPrice + $scope.UniqueCodePaymentConfig.UniqueCode;
                            // }
                            // }
                            // if ($scope.CreditCard.TwoClickToken == 'false') {
                            return {
                                'card_number': ccNumber,
                                'card_exp_month': $scope.CreditCard.Month,
                                'card_exp_year': $scope.CreditCard.Year,
                                'card_cvv': $scope.CreditCard.Cvv,
                                // Set 'secure', 'bank', and 'gross_amount', if the merchant wants transaction to be processed with 3D Secure
                                'secure': true,
                                'bank': 'mandiri',
                                'gross_amount': gross_amount
                            }
                            // else {
                            //     return {
                            //         'token_id': $scope.CreditCard.TwoClickToken,
                            //         'two_click': true,
                            //     }
                            // }
                        };

                        // run the veritrans function to check credit card
                        Veritrans.token(card, function callback(response) {
                            if (response.redirect_url) {
                                // 3Dsecure transaction. Open 3Dsecure dialog
                                // $log.debug('Open Dialog 3Dsecure');
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
                        });

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
                } else { // NOT Credit Card
                    if ($scope.paymentMethod == 'MandiriClickPay') {
                        var cardNo = $scope.MandiriClickPay.CardNo;
                        if (cardNo == null || cardNo.length < 12 || cardNo.length > 19) {
                            $scope.notifCardLength = true;
                        } else {
                            $scope.notifCardLength = false;
                            $scope.pay.send();
                        }
                    } else {
                        $scope.pay.send();
                    }
                }
            },
            send: function () {
                if ($scope.trial > 3) $scope.trial = 0;
                $scope.pay.isSuccess = false,
                $scope.pay.checked = false;
                $scope.pay.isPaying = false;
                $scope.pay.postData.rsvNo = $scope.rsvNo;
                $scope.pay.postData.discCd = $scope.voucher.confirmedCode;
                //generate payment data
                switch ($scope.paymentMethod) {
                    case "BankTransfer":
                        if ($scope.redirectionUrl == null || $scope.redirectionUrl.length == 0) {
                            if ($scope.pay.postData.submethod == "Other")
                                $scope.pay.showOtherBankPopup = true;
                            else
                                $scope.pay.showPopup = true;
                            break;
                        }
                        break;
                    case "CreditCard":
                        console.log($scope.CreditCard.Number);
                        var hash = CryptoJS.SHA512($scope.CreditCard.Number.toString());
                        var hex = hash.toString(CryptoJS.enc.Hex);
                        $scope.pay.postData.creditCard = {
                            tokenId: $scope.CreditCard.Token,
                            holderName: $scope.CreditCard.Name,
                            hashedPan: hex,
                            reqBinDiscount: $scope.binDiscount.available
                        };
                        break;
                    case "MandiriClickPay":
                        var cardNoLast10 = $scope.MandiriClickPay.CardNo + ""; // gak ngerti ini buat apa ditambah "" , convert to string?
                        cardNoLast10 = cardNoLast10.substr(cardNoLast10.length - 10);
                        var rsvNoLast5 = $scope.rsvNo + "";
                        rsvNoLast5 = rsvNoLast5.substr(rsvNoLast5.length - 5);
                        var netprice = $scope.initialPrice - $scope.voucher.amount + $scope.UniqueCodePaymentConfig.UniqueCode;
                        $scope.pay.postData.mandiriClickPay = {
                            cardNo: $scope.MandiriClickPay.CardNo,
                            token: $scope.MandiriClickPay.Token,
                            cardNoLast10: cardNoLast10,
                            amount: netprice,
                            rsvNoLast5: rsvNoLast5
                        };
                        $scope.pay.clickpay = true;
                        break;
                    case "CimbClicks":
                        $scope.pay.postData.cimbClicks = {
                            description: "Pembayaran melalui CimbClicks"
                        };
                        break;
                    case "VirtualAccount":
                        if ($scope.pay.postData.submethod == "Other")
                            $scope.pay.showOtherBankPopup = true;
                        else
                            $scope.pay.showPopup = true;
                        break;
                    case "MandiriBillPayment":
                        $scope.pay.postData.mandiriBillPayment = {
                            label1: "Payment for booking a flight",
                            value1: "debt"
                        };
                        break;
                }
                if ($scope.paymentMethod != "BankTransfer" && $scope.paymentMethod != 'VirtualAccount'
                    || ($scope.redirectionUrl != null && $scope.redirectionUrl.length != 0)) {
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
                if (method == 'transfer' || method == 'virtualAccount') {
                    $scope.pay.showPopup = false;
                    $scope.pay.showOtherBankPopup = false;
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
                        if (returnData.data.status == '200') {
                            $scope.pay.isSuccess = true;
                            $scope.pay.rsvNo = $scope.rsvNo;
                            $('form#rsvno input#rsvno-input').val($scope.pay.rsvNo);
                            $('form#rsvno input#url-input').val(returnData.data.redirectionUrl);
                            $('form#rsvno').submit();
                            $scope.redirectUrl = returnData.data.redirectionUrl;
                            $scope.pay.checked = true;
                        } else { //Error Handling right Here
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
                                    break;
                                case 'ERPPAY05':
                                    $scope.errorLog = 'Promo is over (voucher)';
                                    $scope.errorMessage = 'Mohon maaf, kuota promo ' + $scope.voucher.displayName + ' hari ini telah habis. ';
                                    if ($scope.pay.clickpay) {
                                        $scope.errorMessage += 'Silakan masukkan kembali token Anda untuk melanjutkan pembayaran';
                                    } else {
                                        $scope.errorMessage += 'Apakah Anda ingin melanjutkan pembayaran?';
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
                                    break;
                                case 'ERRGEN98':
                                    $scope.errorLog = 'Invalid JSON Format';
                                    $scope.errorMessage = 'Terjadi kesalahan pada sistem';
                                    $scope.ReturnUrl = "/";
                                    $scope.pay.checked = true;
                                    $scope.pay.isSuccess = false;
                                    $scope.pay.isPaying = false;
                                    break;
                                case 'ERRGEN99':
                                    $scope.errorLog = 'There is a problem on the server';
                                    $scope.errorMessage = 'Terjadi kesalahan pada sistem';
                                    $scope.ReturnUrl = "/";
                                    $scope.pay.checked = true;
                                    $scope.pay.isSuccess = false;
                                    $scope.pay.isPaying = false;
                                    break;
                            }
                        }
                    }).catch(function (returnData) {
                        $scope.trial++;
                        if (refreshAuthAccess() && $scope.trial < 4) { //refresh cookie
                            $scope.pay.send();
                        } else {
                            $scope.pay.checked = true;
                            $scope.pay.isSuccess = false;
                            $scope.pay.isPaying = false;
                        }
                    });
                } else {
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

        //// check if name only contains alphabetical, [space], or null
        $scope.checkName = function (name) {
            var re = /^[a-zA-Z ]+$/;
            return (name == null || name == "" || re.test(name));
        }

        //// check if input is a number or null
        $scope.checkNumber = function (number) {
            var re = /^[0-9]+$/;
            return (number == "" || number == null || re.test(number))
        }

        $scope.ccValidation = function () {
            var validName = $scope.checkName($scope.CreditCard.Name);
            var validDate = $scope.checkDate($scope.CreditCard.Month, $scope.CreditCard.Year);
            if (validName && validDate) {
                return true;
            } else {
                // if (!validDate) $('.ccDate').addclass('has-error');
                return false;
            }
        }

        $scope.checkDate = function (month, year) {
            if (month == 0 || month == 'Bulan' || year == 'Tahun') {
                $scope.dateOver = false;
                return true;
            }

            var now = new Date();
            var monthNow = now.getMonth();
            var yearNow = now.getFullYear();

            if (year > yearNow) {
                $scope.dateOver = false;
                return true;
            } else if (year == yearNow) {
                if (month < monthNow + 1) {
                    $scope.dateOver = true;
                    return false;
                } else {
                    $scope.dateOver = false;
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

        $scope.$watch('paymentMethod', function (newValue, oldValue) {
            if (newValue != oldValue) {
                $scope.notifCardLength = false;
                $scope.dateOver = false;
            }
        }, true);

        // ********************************** END *********************************************

        $scope.getMdr = function () {
            var method = 999;
            switch ($scope.paymentMethod) {
                case 'BankTransfer': method = 0; break;
                case 'CreditCard': method = 1; break;
                case 'VirtualAccount': method = 2; break;
            }
            function select(obj) {
                return obj.PaymentMethod == method;
            }
            var a = mdr.filter(select)[0];
            ///// TODO : include discount
            let price = $scope.totalPrice - $scope.voucher.amount + $scope.UniqueCodePaymentConfig.UniqueCode;
            return a ? Math.ceil(price * a.Percentage / 100) : 0;
        }

        //// opening / closing the accordion
        //// using manual jQuery DOM Manipulation, because jQuery-UI accordion
        //// present bug that can disrupt payment method's price calculation
        // $(".box-payment .accordion-header").click( function(){
        $scope.selectSubMethod = function (subMethod) {
            $scope.pay.postData.submethod = subMethod;
            if (subMethod == 'Mandiri') {
                $scope.paymentMethod = $scope.pay.postData.method = 'BankTransfer';
                // $scope.methodDiscount.reset('nVA');
            } else {
                $scope.paymentMethod = $scope.pay.postData.method = 'VirtualAccount';
                // $scope.methodDiscount.check();
            }
        }
        $scope.selectMethod = function ($event, method) {
            // $scope.binDiscount.reset('ncc');
            // $scope.methodDiscount.reset('nVA');
            var paymentHeader = $($event.currentTarget);
            $scope.pay.postData.submethod = '';

            //// CLOSING the accordion
            if ($scope.paymentMethod == method) {
                $scope.paymentMethod = '';
                $(".box-payment .accordion-header").
                    removeClass("ui-accordion-header-active").
                    next(".accordion-content").slideUp(200);
            }
                //// OPENING the accordion
            else {
                $scope.paymentMethod = method;
                paymentHeader.addClass("ui-accordion-header-active").
                    next(".accordion-content").slideDown(200);

                //// close all other accordion
                $(".box-payment .accordion-header").not(paymentHeader).
                    removeClass("ui-accordion-header-active").
                    next(".accordion-content").slideUp(200);
            }
            $scope.pay.postData.method = $scope.paymentMethod;
        }

        $(".kode-voucher a").click(function () {
            $(this).next().toggle(200);
        });

    }]);