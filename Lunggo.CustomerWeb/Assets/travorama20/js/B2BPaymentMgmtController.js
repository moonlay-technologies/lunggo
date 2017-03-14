app.controller('B2BPaymentMgmtFormController', ['$scope', '$log', '$http', function ($scope, $log, $http) {
    $scope.pageLoaded = false;
    $scope.creditCards = [];
    $scope.ccType = function(digit) { //untuk nunjukin tipeCc
        if (digit.slice(0,1) == '4') {
            return 'Visa';
        }else if (digit.slice(0,1) == '5') {
            return 'MasterCard';
        } else if (digit.slice(0, 2) == '34' || digit.slice(0, 2) == '37') {
            return 'American Express';
        }else if (digit.slice(0, 2) == '35') {
            return 'JCB';
        }
        else {
            return '';
        }
    }

    $scope.emptyCc = false;
    $scope.getSavedCc = function () {
        var authAccess = getAuthAccess();
        if (authAccess == 2) {
            $http({
                method: 'GET',
                url: GetCreditCardConfig.Url,
                headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') },
                async: false
            }).then(function (returnData) {
                $scope.pageLoaded = true;
                $('.wait').modal('hide');
                if (returnData != null) {
                    if (returnData.data != null) {
                        $scope.creditCards = [];
                        if (returnData.data.creditCards != null && returnData.data.creditCards.length > 0) {
                            for (var i = 0; i < returnData.data.creditCards.length; i++) {
                                $scope.creditCards.push({
                                    cardExpiry: new Date(returnData.data.creditCards[i].cardExpiry),
                                    cardHolderName: returnData.data.creditCards[i].cardHolderName,
                                    isPrimaryCard: returnData.data.creditCards[i].isPrimaryCard,
                                    maskedCardNumber: returnData.data.creditCards[i].maskedCardNumber,
                                    type: $scope.ccType(returnData.data.creditCards[i].maskedCardNumber)
                                });
                                if (returnData.data.creditCards[i].isPrimaryCard) {
                                    $scope.currentPrimary = i;
                                }
                            }
                            $scope.cloneIndex = $scope.creditCards.length;
                            $log.debug($scope.creditCards);
                        } else {
                            $scope.emptyCc = true;
                            $scope.currentPrimary = 0;
                        }
                    } else {
                        //show blank
                    }
                } else {
                    //show blank
                }

            }).catch(function () {
                $scope.pageLoaded = true;
                 $('.wait').modal('hide');
            });
        }
    }

    $scope.getSavedCc();

    $scope.PaymentStatus = {
        firstload: true,
        check : function() {
            var authAccess = getAuthAccess();
            if (authAccess == 2) {
                $http({
                    method: 'GET',
                    url: CheckPaymentDisabilityStatusConfig.Url,
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') },
                    async: false
                }).then(function (returnData) {
                    if (returnData.status == 200) {
                        if (returnData.data.isPaymentDisabled == null) {
                            $scope.PaymentStatus.disabled = true;
                            $scope.PaymentStatus.firstload = false;
                        } else {
                            $scope.PaymentStatus.disabled = returnData.data.isPaymentDisabled;
                            if (!returnData.data.isPaymentDisabled) {
                                $('.switchery').click();
                            } else {
                                $scope.PaymentStatus.firstload = false;
                            }
                        }
                        
                    } else {
                        
                    }
                }).catch(function () {
                });
            }
        },
        disabled: false,
        state: false,
        set: function (status) {
            $('.wait').modal({
                backdrop: 'static',
                keyboard: false
            });
            var authAccess = getAuthAccess();
            if (authAccess == 2) {
                $http({
                    method: 'POST',
                    url: SetPaymentDisabilityStatusConfig.Url,
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') },
                    async: false,
                    data: {
                        status: status
                    }
                }).then(function (returnData) {
                    $('.wait').modal('hide');
                    if (returnData.data.status == 200) {
                        if (status == true) {
                            $scope.PaymentStatus.disabled = true;
                        } else {
                            $scope.PaymentStatus.disabled = false;
                        }
                        $('.setPaymentStatusSucceed').modal({
                            backdrop: 'static',
                            keyboard: false
                        });
                        $scope.PaymentStatus.reset();
                    } else {
                        $('.setPaymentStatusFailed').modal({
                            backdrop: 'static',
                            keyboard: false
                        });
                    }
                }).catch(function () {
                    $('.wait').modal('hide');
                    $('.setPaymentStatusFailed').modal({
                        backdrop: 'static',
                        keyboard: false
                    });
                });
            }
        },
        reset: function () {
            $scope.PaymentStatus.firstload = true;
            $('.switchery').click();
        },
        yes : false
    }

    $scope.PaymentStatus.check();

    $('#paymentStatus').on('change', function () {
        if ($scope.PaymentStatus.firstload) {
            $scope.PaymentStatus.firstload = false;
        } else {
            $scope.PaymentStatus.reset();
            $scope.PaymentStatus.state = this.checked;
            $('.setPaymentStatus').modal({
                backdrop: 'static',
                keyboard: false
            });
        }
        
    });

    $('body .remove-payment').hide();
    $scope.cloneIndex = 0;
    function clone() {
        $(".newCc").removeClass("newCc");
        if ($scope.cloneIndex == 0) {
            var isEmpty = $('#payment-item-0').hasClass("emptyCc") == true;
            if (isEmpty) {
                $("#payment-item-0").removeClass("hide");
            }
        } else {
            $('body #add-payment').closest('.payment-body').find('#payment-item-0').clone(false, false)
                .insertAfter( $( ".payment-item" ).last())
                .attr('id', "payment-item-" + $scope.cloneIndex)
                .attr('count', $scope.cloneIndex ).addClass("newCc");
            $scope.cloneIndex++;
            $('.newCc .cc-no').text('New Card Number');
            $('.newCc .btn-edit-payment').removeClass("hide");
            $('.newCc .expiry').text('Expires mm/yy');
            $('.newCc .set-status').hide();
            $('.newCc .current-status').hide();
            $('.newCc .remove-payment').prop("disabled", false);
            $('.newCc').addClass('notSaved');
            $('.newCc .logoCard').append('<div class="iconCc" style="font-size: 45px;line-height: 100%;height: 46px"><span><i class="fa fa-credit-card m-r-20"></i></span></div>');
            $('.newCc .logoCard .icon-visa').hide();
            $('.newCc .logoCard .icon-jcb').hide();
            $('.newCc .logoCard .icon-amex').hide();
            $('.newCc .logoCard .icon-mastercard').hide();
            $('.newCc .remove-payment').removeClass('disableDel');
            $('.newCc .remove-payment').removeClass('ng-hide');
            $('.newCc .remove-payment').show();
            if ($scope.cloneIndex > 1) {
                $(this).closest('.payment-body').find('#payment-item-' + $scope.cloneIndex + ' .remove-payment').show();
                $(this).closest('.payment-body').find('#payment-item-' + $scope.cloneIndex + ' .pi-status').find('.current-status').hide();
            }

            $('.newCc .btn-edit-payment').click(function () {
                $scope.currentEdit.cc = '';
                $scope.currentEdit.cvc = '';
                $scope.currentEdit.cardHolderName = '';
                $scope.currentEdit.month = '';
                $scope.currentEdit.year = '';
                
                var index = $(this).closest('.payment-item').attr("count");
                $scope.addCreditCard.index = index;
                    
                var size = $('input[name=slideup_toggler]:checked').val();
                var modalElem = $('.modal-edit-payment.form-add-cc');
                if (size == "mini") {
                    $('.modal-edit-payment.form-add-cc').modal('show');
                } else {
                    $('.modal-edit-payment.form-add-cc').modal('show');
                    if (size == "default") {
                        modalElem.children('.modal-dialog').removeClass('modal-lg');
                    } else if (size == "full") {
                        modalElem.children('.modal-dialog').addClass('modal-lg');
                    }
                }
                $('.modal-edit-payment .form-control').val('');
            });

            $('.newCc .remove-payment').click(function () {
                var parent = $(this).closest('.payment-item');
                if (parent.hasClass('notSaved')) {
                    parent.find(".remove-payment").attr("data-target", ".none");
                    parent.remove();
                } else {
                    var maskedCc = $(".newCc .cc-no").attr("cc");
                    $scope.deleteCardData.cc = maskedCc;
                    var index = $('.newCc').attr("count");
                    $scope.deleteCardData.index = index;
                }
            });

            $('.newCc .set-status').click(function () {
                var ccNo = $('.newCc .cc-no').attr("cc");
                var index = $('.newCc').attr("count");
                $scope.ccToSetPrimary = ccNo;
                $scope.primaryIndex = index;
            });
        }      
    }

    $scope.validation = function () {
        if (!$scope.checkNumber($scope.currentEdit.cc) || !$scope.checkName($scope.currentEdit.cardHolderName)
                || !$scope.checkDate(parseInt($scope.currentEdit.month), parseInt($scope.currentEdit.year))) {
            if (!$scope.checkDate($scope.currentEdit.month, $scope.currentEdit.year)) {
                alert('error');
            }
            return false;
        } else {
            return true;
        }
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
    $scope.trial = 0;

    $('.saveNewCc').click(function () {
        $scope.addCreditCard.getPreToken();
    });
    $scope.addCreditCard = {
        index: '',
        ccNo : '',
        getPreToken: function () {
            if (!$scope.validation()) {
                $scope.addCreditCard.inputValid = false;
            } else {
                if ($scope.currentEdit.cc == null || $scope.currentEdit.cc.length < 12 || $scope.currentEdit.cc.length > 19) {
                    $scope.notifCardLength = true;
                } else {
                    $scope.notifCardLength = false;
                    Veritrans.url = VeritransTokenConfig.Url;
                    Veritrans.client_key = VeritransTokenConfig.ClientKey;
                    var card = function () {
                        var gross_amount = 10000;
                        if ($scope.currentEdit.TwoClickToken == 'false') {
                            $scope.addCreditCard.ccdata = true;
                            return {
                                'card_number': $scope.currentEdit.cc,
                                'card_exp_month': $scope.currentEdit.month,
                                'card_exp_year': $scope.currentEdit.year,
                                'card_cvv': $scope.currentEdit.cvc,
                                'secure': true,
                                'bank': 'mandiri',
                                'gross_amount': gross_amount
                            }
                        } else {
                            return {
                                'card_cvv': $scope.currentEdit.cvc,
                                'token_id': $scope.currentEdit.TwoClickToken,
                                'two_click': true,
                                'secure': true,
                                'bank': 'mandiri',
                                'gross_amount': gross_amount
                            }
                        }
                    };

                    Veritrans.token(card, callback);

                    function callback(response) {
                        if (response.redirect_url) {
                            $log.debug('Open Dialog 3Dsecure');
                            openDialog(response.redirect_url);

                        } else if (response.status_code == '200') {
                            closeDialog();
                            $("#vt-token").val(response.token_id);
                            $scope.currentEdit.Token = response.token_id;

                            $scope.addCreditCard.addCC();

                        } else {
                            closeDialog();
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
        addCC: function () {
            $('.modal-edit-payment.form-add-cc').modal('hide');
            $('.wait').modal({
                backdrop: 'static',
                keyboard: false
            });
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
                        token: $scope.currentEdit.Token,
                        cardHolderName: $scope.currentEdit.cardHolderName,
                        cardExpirymonth: parseInt($scope.currentEdit.month),
                        cardExpiryYear: parseInt($scope.currentEdit.year)
                    },
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).then(function (returnData) {
                    $('.wait').modal("hide");
                    if (returnData.data.status == '200') {
                        $('.addCcSucceed').modal({
                            backdrop: 'static',
                        });
                        $log.debug('Success Adding new Credit Card');
                        clone();
                        $('.modal-edit-payment.form-add-cc').modal('hide');
                        //SHOW data cc in new Cc after saving
                        var currIndex = $scope.cloneIndex - 1;
                        if ($scope.emptyCc) {
                            currIndex = 0;
                            $('.emptyCc').show();
                            $('.emptyCc').removeClass('emptyCc');
                            $scope.cloneIndex = 1;
                            $('.payment-item[count=' + currIndex + '] .current-status').removeClass('ng-hide');
                            $('.payment-item[count=' + currIndex + '] .current-status').show();
                            $('.payment-item[count=' + currIndex + '] .set-status').hide();
                            $('.payment-item[count=' + currIndex + '] .remove-payment').prop("disabled", true);
                            $('.payment-item[count=' + currIndex + '] .remove-payment').addClass("disableDel");
                            $scope.emptyCc = false;
                            $scope.currentPrimary = 0;
                        } else {
                            $('.payment-item[count=' + currIndex + '] .set-status').removeClass('ng-hide');
                            $('.payment-item[count=' + currIndex + '] .current-status').hide();
                            $('.payment-item[count=' + currIndex + '] .set-status').show();
                            $('.payment-item[count=' + currIndex + '] .remove-payment').prop("disabled", false);
                        }
                        $('.payment-item[count=' + currIndex+ '] .cc-no').text($scope.ccType($scope.currentEdit.cc) + ' ****' + $scope.currentEdit.cc.slice(-4));
                        $('.payment-item[count=' + currIndex+ '] .expiry').text("Expires " + $scope.currentEdit.month + '/' + $scope.currentEdit.year.slice(-2));
                        var maskedCc = $scope.currentEdit.cc.slice(0, 1) + '************' + $scope.currentEdit.cc.slice(-4);
                        //give attribute and hide/show icon for later edit
                        $('.payment-item[count=' + currIndex+ '] .cc-no').attr('cc', maskedCc);
                        $('.payment-item[count=' + currIndex+ ']').removeClass('notSaved');
                        $('.payment-item[count=' + currIndex+ '] .btn-edit-payment').hide();
                        //$('.payment-item[count=' + currIndex+ '] .logoCard .iconCc').hide();
                        //show cc logo
                        if ($scope.ccType($scope.currentEdit.cc) == 'Visa') {
                            $('.payment-item[count=' + currIndex + '] .logoCard .icon-visa').removeClass('ng-hide');
                            $('.payment-item[count=' + currIndex + '] .logoCard .icon-visa').removeClass('hide');
                            $('.payment-item[count=' + currIndex + '] .logoCard .icon-visa').show();
                            $('.payment-item[count=' + currIndex + '] .logoCard .iconCc').hide();
                        } else if ($scope.ccType($scope.currentEdit.cc) == 'MasterCard') {
                            $('.payment-item[count=' + currIndex + '] .logoCard .icon-mastercard').removeClass('hide');
                            $('.payment-item[count=' + currIndex + '] .logoCard .icon-mastercard').removeClass('ng-hide');
                            $('.payment-item[count=' + currIndex + '] .logoCard .icon-mastercard').show();
                            $('.payment-item[count=' + currIndex + '] .logoCard .iconCc').hide();
                        } else if ($scope.ccType($scope.currentEdit.cc) == 'JCB') {
                            $('.payment-item[count=' + currIndex + '] .logoCard .icon-jcb').removeClass('hide');
                            $('.payment-item[count=' + currIndex + '] .logoCard .icon-jcb').removeClass('ng-hide');
                            $('.payment-item[count=' + currIndex + '] .logoCard .icon-jcb').show();
                            $('.payment-item[count=' + currIndex + '] .logoCard .iconCc').hide();
                        }
                        else if ($scope.ccType($scope.currentEdit.cc) == 'Amex') {
                            $('.payment-item[count=' + currIndex + '] .logoCard .icon-amex').removeClass('hide');
                            $('.payment-item[count=' + currIndex + '] .logoCard .icon-amex').removeClass('ng-hide');
                            $('.payment-item[count=' + currIndex + '] .logoCard .icon-amex').show();
                            $('.payment-item[count=' + currIndex + '] .logoCard .iconCc').hide();
                        } else {
                            $('.payment-item[count=' + currIndex + '] .logoCard .iconCc').show();
                        }
                    }
                    else {
                        $('.wait').modal("hide");
                        $('.addCcFailed').modal({
                            backdrop: 'static',
                        });
                        $('.addCcFailed .close').click(function () {
                            $('.addCcFailed').modal("hide");
                        });
                        $log.debug(returnData.data.error);
                        $log.debug(returnData);
                    }
                    $('.newCc .btn-payment-edit').hide();
                }).catch(function () {
                    $scope.trial++;
                    $('.newCc .btn-payment-edit').hide();
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.addCreditCard(name);
                    }
                    else {
                        $('.wait').modal("hide");
                        $('.addCcFailed').modal({
                            backdrop: 'static',
                        });
                        $('.addCcFailed .close').click(function () {
                            $('.addCcFailed').modal("hide");
                        });
                        $log.debug('Failed requesting change profile');
                    }
                });
            }
            else { //if not authorized
                $('.wait').modal("hide");
                $('.newCc .btn-payment-edit').hide();
                $('.addCcFailed').modal({
                    backdrop: 'static',
                });
            }
        }
    }
    

    function showEditForm() {
        var size = $('input[name=slideup_toggler]:checked').val();
        var modalElem = $('.modal-edit-payment.form-add-cc');
        if (size == "mini") {
            $('.modal-edit-payment.form-add-cc').modal('show');
        } else {
            $('.modal-edit-payment.form-add-cc').modal('show');
            if (size == "default") {
                modalElem.children('.modal-dialog').removeClass('modal-lg');
            } else if (size == "full") {
                modalElem.children('.modal-dialog').addClass('modal-lg');
            }
        }
        $('.modal-edit-payment .form-control').val('');
    }

    $('body #add-payment').on('click', showEditForm);
    $scope.currentEdit = {
        cc: '',
        type: '',
        month: '',
        year: '',
        cardHolderName: '',
        address: '',
        cvc: '',
        TwoClickToken: 'false',
        Token: ''
    };

    $scope.message = '';
    $scope.trial = 0;
    $scope.currentPrimary = '';
    $scope.ccToSetPrimary = '';
    $scope.primaryIndex = '';
    $scope.setPrimaryData = function(cc, no) {
        $scope.ccToSetPrimary = cc;
        $scope.primaryIndex = no;
    }

    $scope.setPrimary = function (cc, index) {
        $('.wait').modal({
            backdrop: 'static',
            keyboard: false
        });
        $scope.trial = 0;
        var authAccess = getAuthAccess();
        if (authAccess == 2) {
            $http({
                url: SetPrimaryCardConfig.Url,
                method: 'POST',
                data: {
                    maskedCardNumber: cc
                },
                headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
            }).then(function (returnData) {
                $('.wait').modal("hide");
                if (returnData.data.status == 200) {
                    $('.setPrimarySucceed').modal({
                        backdrop: 'static',
                        keyboard: false
                    });
                    $('.payment-item[count=' + $scope.currentPrimary + '] .current-status').hide();
                    $('.payment-item[count=' + $scope.currentPrimary + '] .set-status').show();
                    $('.payment-item[count=' + $scope.currentPrimary + '] .set-status').removeClass('ng-hide');
                    $('.disableDel').prop("disabled", false);
                    $('.disableDel').removeClass("ng-hide");
                    $('.disableDel').show();
                    $('.disableDel').removeClass('disableDel');
                    $('.payment-item[count=' + index + '] .current-status').removeClass('ng-hide');
                    $('.payment-item[count=' + index + '] .current-status').show();
                    $('.payment-item[count=' + index + '] .set-status').hide();
                    $('.payment-item[count=' + index + '] .remove-payment').prop("disabled", true);
                    $('.payment-item[count=' + index + '] .remove-payment').addClass("disableDel");
                    $('.payment-item[count=' + index + '] .remove-payment').hide();
                    $scope.message = 'Setting Primary Card is Successful';
                    $scope.currentPrimary = index;
                } else {
                    $('.setPrimaryFailed').modal({
                        backdrop: 'static',
                        keyboard: false
                    });
                    $('.setPrimaryFailed .close').click(function() {
                        $('.setPrimaryFailed').modal("hide");
                    });
                    $log.debug(returnData.data.error);
                }
            }).catch(function(returnData) {
                $scope.trial++;
                if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                {
                    $scope.setPrimary(cc);
                } else {
                    $('.setPrimaryFailed').modal({
                        backdrop: 'static',
                        keyboard: false
                    });
                    $('.setPrimaryFailed .close').click(function () {
                        $('.setPrimaryFailed').modal("hide");
                    });
                    $log.debug('Failed Update Reservation');
                    $log.debug(returnData);
                    $scope.rsvUpdated = false;
                }
            });       
        }          
    }

    $scope.deleteCardData= {
        cc: '',
        set: function(no, index) {
            $scope.deleteCardData.cc = no;
            $scope.deleteCardData.index = index;
        },
        index : ''
    }

    $scope.deleteCard = function (maskedCardNo, index) {
        $('.wait').modal({
            backdrop: 'static',
            keyboard: false
        });
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
            }).then(function(returnData) {
                $('.wait').modal("hide");
                if (returnData.data.status == '200') {
                    $log.debug('Success Delete CC');
                    $('.deleteCcSucceed').modal({
                        backdrop: 'static',
                    });
                    $('.payment-item[count=' + index + ']').remove();
                } else {
                    $log.debug(returnData.data.error);
                    $log.debug(returnData);
                    $('.deleteCcFailed').modal({
                        backdrop: 'static',
                    });
                    $('.deleteCcFailed .close').click(function() {
                        $('.deleteCcFailed').modal("hide");
                    });
                }
            }).catch(function(returnData) {
                $scope.trial++;
                if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                {
                    $scope.deleteUser();
                } else {
                    $('.deleteCcFailed').modal({
                        backdrop: 'static'
                    });
                    $log.debug('Failed Add User');
                    $log.debug(returnData);
                }
            });
        } else { //if not authorized
            $('.deleteCcFailed').modal({
                backdrop: 'static',
            });
        }
    }   
}]);