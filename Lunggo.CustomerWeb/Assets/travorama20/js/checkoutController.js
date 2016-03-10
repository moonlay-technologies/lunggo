// travorama angular app - checkout controller
app.controller('checkoutController', [
    '$http', '$scope', '$interval', '$location', function ($http, $scope, $interval, $location) {

        $scope.returnUrl = document.referrer == (window.location.origin + window.location.pathname + window.location.search) ? '/' : document.referrer;

        // set hash to page 1
        angular.element(document).ready(function () {
            if (getParam('page') == 2) {
                $location.hash('page-2');
            } else {
                $location.hash('page-1');
            }
        });

        $(window).on('hashchange', function () {
            if ($location.hash() != 'page-1' || $location.hash() != 'page-2') {
            }
        });

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
            { name: 'Mr', value: 'Mister' },
            { name: 'Mrs', value: 'Mistress' },
            { name: 'Ms', value: 'Miss' }
        ];

        $scope.CreditCard = {
            TwoClickToken: 'false',
            Name: '',
            Month: '01',
            Year: 2016,
            Cvv: '',
            Number: ''
        };

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
        $scope.TransferConfig.GetUniqueCode(price);

        $scope.currency = 'IDR';
        $scope.language = langCode;
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

        $scope.book = {
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
                $scope.book.postData = ' "TransferToken" : "' + $scope.TransferConfig.Token + '" , "Payment.Data.Data0" : "' + $scope.CreditCard.Token + '", "Payment.Data.Data1" : "' + $scope.CreditCard.Name + '", "Payment.Data.Data20" : "' + $scope.loggedIn + '", "Payment.Data.Data9" : "' + $scope.buyerInfo.email + '", "Token":"' + $scope.token + '", "Payment.Currency":"' + $scope.currency + '", "DiscountCode":"' + $scope.voucher.confirmedCode + '", "Payment.Method":"' + $scope.paymentMethod + '", "Contact.Title" :"' + $scope.buyerInfo.title + '","Contact.Name":"' + $scope.buyerInfo.fullname + '", "Contact.CountryCode":"' + $scope.buyerInfo.countryCode + '", "Contact.Phone":"' + $scope.buyerInfo.phone + '","Contact.Email":"' + $scope.buyerInfo.email + '","Language":"' + $scope.language + '"';
                for (var i = 0; i < $scope.passengers.length; i++) {

                    // check nationality
                    if (!$scope.passportRequired) {
                        $scope.passengers[i].passport.number = '';
                        $scope.passengers[i].passport.expire.date = '';
                        $scope.passengers[i].passport.expire.month = '';
                        $scope.passengers[i].passport.expire.year = '';
                        $scope.passengers[i].passport.expire.full = '';
                        if (!$scope.nationalityRequired) {
                            $scope.passengers[i].passport.country = '';
                        }
                    }
                    if (!$scope.idRequired) {
                        $scope.passengers[i].idNumber = '';
                    }
                    // birthdate
                    $scope.passengers[i].birth.full = $scope.passengers[i].birth.year + '/' + ('0' + (parseInt($scope.passengers[i].birth.month) + 1)).slice(-2) + '/' + ('0' + $scope.passengers[i].birth.date).slice(-2);
                    // passport expiry date
                    $scope.passengers[i].passport.expire.full = $scope.passengers[i].passport.expire.year + '/' + ('0' + (parseInt($scope.passengers[i].passport.expire.month) + 1)).slice(-2) + '/' + ('0' + $scope.passengers[i].passport.expire.date).slice(-2);

                    $scope.book.postData = $scope.book.postData + (',"Passengers[' + i + '].Type": "' + $scope.passengers[i].type + '", "Passengers[' + i + '].Title": "' + $scope.passengers[i].title + '", "Passengers[' + i + '].FirstName":"' + $scope.passengers[i].firstname + '", "Passengers[' + i + '].LastName": "' + $scope.passengers[i].lastname + '", "Passengers[' + i + '].BirthDate":"' + $scope.passengers[i].birth.full + '", "Passengers[' + i + '].PassportNumber":"' + $scope.passengers[i].passport.number + '", "Passengers[' + i + '].PassportExpiryDate":"' + $scope.passengers[i].passport.expire.full + '", "Passengers[' + i + '].idNumber":"' + $scope.passengers[i].idNumber + '", "Passengers[' + i + '].Country":"' + $scope.passengers[i].passport.country + '"');
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

                    if (returnData.data.IsSuccess) {
                        $scope.book.isSuccess = true;
                        $scope.book.rsvNo = returnData.data.RsvNo;

                        $('form#rsvno input#rsvno-input').val(returnData.data.RsvNo);
                        $('form#rsvno').submit();

                    } else {
                        $scope.book.isSuccess = false;
                    }

                }, function (returnData) {
                    console.log(returnData);
                    $scope.book.checked = true;
                    $scope.book.isSuccess = false;
                });

            }
        };


        $scope.passengers = [];
        $scope.passengersForm = {
            valid: false,
            checked: false
        };

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

        $scope.flightDetail.departureFullDate = departureDate; // development only. Please change the value to actual date on production
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

        $scope.passportRequired = passportRequired;
        $scope.idRequired = idRequired;
        $scope.nationalityRequired = nationalityRequired;

        $scope.bookingDate = new Date();

        $scope.countries = [{ code: 'AF', name: 'Afghanistan' }, { code: 'AX', name: 'Aland Islands' }, { code: 'AL', name: 'Albania' }, { code: 'DZ', name: 'Algeria' }, { code: 'AS', name: 'American Samoa' }, { code: 'AD', name: 'Andorra' }, { code: 'AO', name: 'Angola' }, { code: 'AI', name: 'Anguilla' }, { code: 'AQ', name: 'Antarctica' }, { code: 'AG', name: 'Antigua And Barbuda' }, { code: 'AR', name: 'Argentina' }, { code: 'AM', name: 'Armenia' }, { code: 'AW', name: 'Aruba' }, { code: 'AU', name: 'Australia' }, { code: 'AT', name: 'Austria' }, { code: 'AZ', name: 'Azerbaijan' }, { code: 'BS', name: 'Bahamas' }, { code: 'BH', name: 'Bahrain' }, { code: 'BD', name: 'Bangladesh' }, { code: 'BB', name: 'Barbados' }, { code: 'BY', name: 'Belarus' }, { code: 'BE', name: 'Belgium' }, { code: 'BZ', name: 'Belize' }, { code: 'BJ', name: 'Benin' }, { code: 'BM', name: 'Bermuda' }, { code: 'BT', name: 'Bhutan' }, { code: 'BO', name: 'Bolivia' }, { code: 'BA', name: 'Bosnia And Herzegovina' }, { code: 'BW', name: 'Botswana' }, { code: 'BV', name: 'Bouvet Island' }, { code: 'BR', name: 'Brazil' }, { code: 'IO', name: 'British Indian Ocean Territory' }, { code: 'BN', name: 'Brunei Darussalam' }, { code: 'BG', name: 'Bulgaria' }, { code: 'BF', name: 'Burkina Faso' }, { code: 'BI', name: 'Burundi' }, { code: 'KH', name: 'Cambodia' }, { code: 'CM', name: 'Cameroon' }, { code: 'CA', name: 'Canada' }, { code: 'CV', name: 'Cape Verde' }, { code: 'KY', name: 'Cayman Islands' }, { code: 'CF', name: 'Central African Republic' }, { code: 'TD', name: 'Chad' }, { code: 'CL', name: 'Chile' }, { code: 'CN', name: 'China' }, { code: 'CX', name: 'Christmas Island' }, { code: 'CC', name: 'Cocos (Keeling) Islands' }, { code: 'CO', name: 'Colombia' }, { code: 'KM', name: 'Comoros' }, { code: 'CG', name: 'Congo' }, { code: 'CD', name: 'Congo, Democratic Republic' }, { code: 'CK', name: 'Cook Islands' }, { code: 'CR', name: 'Costa Rica' }, { code: 'CI', name: 'Cote D\'Ivoire' }, { code: 'HR', name: 'Croatia' }, { code: 'CU', name: 'Cuba' }, { code: 'CY', name: 'Cyprus' }, { code: 'CZ', name: 'Czech Republic' }, { code: 'DK', name: 'Denmark' }, { code: 'DJ', name: 'Djibouti' }, { code: 'DM', name: 'Dominica' }, { code: 'DO', name: 'Dominican Republic' }, { code: 'EC', name: 'Ecuador' }, { code: 'EG', name: 'Egypt' }, { code: 'SV', name: 'El Salvador' }, { code: 'GQ', name: 'Equatorial Guinea' }, { code: 'ER', name: 'Eritrea' }, { code: 'EE', name: 'Estonia' }, { code: 'ET', name: 'Ethiopia' }, { code: 'FK', name: 'Falkland Islands (Malvinas)' }, { code: 'FO', name: 'Faroe Islands' }, { code: 'FJ', name: 'Fiji' }, { code: 'FI', name: 'Finland' }, { code: 'FR', name: 'France' }, { code: 'GF', name: 'French Guiana' }, { code: 'PF', name: 'French Polynesia' }, { code: 'TF', name: 'French Southern Territories' }, { code: 'GA', name: 'Gabon' }, { code: 'GM', name: 'Gambia' }, { code: 'GE', name: 'Georgia' }, { code: 'DE', name: 'Germany' }, { code: 'GH', name: 'Ghana' }, { code: 'GI', name: 'Gibraltar' }, { code: 'GR', name: 'Greece' }, { code: 'GL', name: 'Greenland' }, { code: 'GD', name: 'Grenada' }, { code: 'GP', name: 'Guadeloupe' }, { code: 'GU', name: 'Guam' }, { code: 'GT', name: 'Guatemala' }, { code: 'GG', name: 'Guernsey' }, { code: 'GN', name: 'Guinea' }, { code: 'GW', name: 'Guinea-Bissau' }, { code: 'GY', name: 'Guyana' }, { code: 'HT', name: 'Haiti' }, { code: 'HM', name: 'Heard Island & Mcdonald Islands' }, { code: 'VA', name: 'Holy See (Vatican City State)' }, { code: 'HN', name: 'Honduras' }, { code: 'HK', name: 'Hong Kong' }, { code: 'HU', name: 'Hungary' }, { code: 'IS', name: 'Iceland' }, { code: 'IN', name: 'India' }, { code: 'ID', name: 'Indonesia' }, { code: 'IR', name: 'Iran, Islamic Republic Of' }, { code: 'IQ', name: 'Iraq' }, { code: 'IE', name: 'Ireland' }, { code: 'IM', name: 'Isle Of Man' }, { code: 'IL', name: 'Israel' }, { code: 'IT', name: 'Italy' }, { code: 'JM', name: 'Jamaica' }, { code: 'JP', name: 'Japan' }, { code: 'JE', name: 'Jersey' }, { code: 'JO', name: 'Jordan' }, { code: 'KZ', name: 'Kazakhstan' }, { code: 'KE', name: 'Kenya' }, { code: 'KI', name: 'Kiribati' }, { code: 'KR', name: 'Korea' }, { code: 'KW', name: 'Kuwait' }, { code: 'KG', name: 'Kyrgyzstan' }, { code: 'LA', name: 'Lao People\'s Democratic Republic' }, { code: 'LV', name: 'Latvia' }, { code: 'LB', name: 'Lebanon' }, { code: 'LS', name: 'Lesotho' }, { code: 'LR', name: 'Liberia' }, { code: 'LY', name: 'Libyan Arab Jamahiriya' }, { code: 'LI', name: 'Liechtenstein' }, { code: 'LT', name: 'Lithuania' }, { code: 'LU', name: 'Luxembourg' }, { code: 'MO', name: 'Macao' }, { code: 'MK', name: 'Macedonia' }, { code: 'MG', name: 'Madagascar' }, { code: 'MW', name: 'Malawi' }, { code: 'MY', name: 'Malaysia' }, { code: 'MV', name: 'Maldives' }, { code: 'ML', name: 'Mali' }, { code: 'MT', name: 'Malta' }, { code: 'MH', name: 'Marshall Islands' }, { code: 'MQ', name: 'Martinique' }, { code: 'MR', name: 'Mauritania' }, { code: 'MU', name: 'Mauritius' }, { code: 'YT', name: 'Mayotte' }, { code: 'MX', name: 'Mexico' }, { code: 'FM', name: 'Micronesia, Federated States Of' }, { code: 'MD', name: 'Moldova' }, { code: 'MC', name: 'Monaco' }, { code: 'MN', name: 'Mongolia' }, { code: 'ME', name: 'Montenegro' }, { code: 'MS', name: 'Montserrat' }, { code: 'MA', name: 'Morocco' }, { code: 'MZ', name: 'Mozambique' }, { code: 'MM', name: 'Myanmar' }, { code: 'NA', name: 'Namibia' }, { code: 'NR', name: 'Nauru' }, { code: 'NP', name: 'Nepal' }, { code: 'NL', name: 'Netherlands' }, { code: 'AN', name: 'Netherlands Antilles' }, { code: 'NC', name: 'New Caledonia' }, { code: 'NZ', name: 'New Zealand' }, { code: 'NI', name: 'Nicaragua' }, { code: 'NE', name: 'Niger' }, { code: 'NG', name: 'Nigeria' }, { code: 'NU', name: 'Niue' }, { code: 'NF', name: 'Norfolk Island' }, { code: 'MP', name: 'Northern Mariana Islands' }, { code: 'NO', name: 'Norway' }, { code: 'OM', name: 'Oman' }, { code: 'PK', name: 'Pakistan' }, { code: 'PW', name: 'Palau' }, { code: 'PS', name: 'Palestinian Territory, Occupied' }, { code: 'PA', name: 'Panama' }, { code: 'PG', name: 'Papua New Guinea' }, { code: 'PY', name: 'Paraguay' }, { code: 'PE', name: 'Peru' }, { code: 'PH', name: 'Philippines' }, { code: 'PN', name: 'Pitcairn' }, { code: 'PL', name: 'Poland' }, { code: 'PT', name: 'Portugal' }, { code: 'PR', name: 'Puerto Rico' }, { code: 'QA', name: 'Qatar' }, { code: 'RE', name: 'Reunion' }, { code: 'RO', name: 'Romania' }, { code: 'RU', name: 'Russian Federation' }, { code: 'RW', name: 'Rwanda' }, { code: 'BL', name: 'Saint Barthelemy' }, { code: 'SH', name: 'Saint Helena' }, { code: 'KN', name: 'Saint Kitts And Nevis' }, { code: 'LC', name: 'Saint Lucia' }, { code: 'MF', name: 'Saint Martin' }, { code: 'PM', name: 'Saint Pierre And Miquelon' }, { code: 'VC', name: 'Saint Vincent And Grenadines' }, { code: 'WS', name: 'Samoa' }, { code: 'SM', name: 'San Marino' }, { code: 'ST', name: 'Sao Tome And Principe' }, { code: 'SA', name: 'Saudi Arabia' }, { code: 'SN', name: 'Senegal' }, { code: 'RS', name: 'Serbia' }, { code: 'SC', name: 'Seychelles' }, { code: 'SL', name: 'Sierra Leone' }, { code: 'SG', name: 'Singapore' }, { code: 'SK', name: 'Slovakia' }, { code: 'SI', name: 'Slovenia' }, { code: 'SB', name: 'Solomon Islands' }, { code: 'SO', name: 'Somalia' }, { code: 'ZA', name: 'South Africa' }, { code: 'GS', name: 'South Georgia And Sandwich Isl.' }, { code: 'ES', name: 'Spain' }, { code: 'LK', name: 'Sri Lanka' }, { code: 'SD', name: 'Sudan' }, { code: 'SR', name: 'Suriname' }, { code: 'SJ', name: 'Svalbard And Jan Mayen' }, { code: 'SZ', name: 'Swaziland' }, { code: 'SE', name: 'Sweden' }, { code: 'CH', name: 'Switzerland' }, { code: 'SY', name: 'Syrian Arab Republic' }, { code: 'TW', name: 'Taiwan' }, { code: 'TJ', name: 'Tajikistan' }, { code: 'TZ', name: 'Tanzania' }, { code: 'TH', name: 'Thailand' }, { code: 'TL', name: 'Timor-Leste' }, { code: 'TG', name: 'Togo' }, { code: 'TK', name: 'Tokelau' }, { code: 'TO', name: 'Tonga' }, { code: 'TT', name: 'Trinidad And Tobago' }, { code: 'TN', name: 'Tunisia' }, { code: 'TR', name: 'Turkey' }, { code: 'TM', name: 'Turkmenistan' }, { code: 'TC', name: 'Turks And Caicos Islands' }, { code: 'TV', name: 'Tuvalu' }, { code: 'UG', name: 'Uganda' }, { code: 'UA', name: 'Ukraine' }, { code: 'AE', name: 'United Arab Emirates' }, { code: 'GB', name: 'United Kingdom' }, { code: 'US', name: 'United States' }, { code: 'UM', name: 'United States Outlying Islands' }, { code: 'UY', name: 'Uruguay' }, { code: 'UZ', name: 'Uzbekistan' }, { code: 'VU', name: 'Vanuatu' }, { code: 'VE', name: 'Venezuela' }, { code: 'VN', name: 'Viet Nam' }, { code: 'VG', name: 'Virgin Islands, British' }, { code: 'VI', name: 'Virgin Islands, U.S.' }, { code: 'WF', name: 'Wallis And Futuna' }, { code: 'EH', name: 'Western Sahara' }, { code: 'YE', name: 'Yemen' }, { code: 'ZM', name: 'Zambia' }, { code: 'ZW', name: 'Zimbabwe' }];

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
        // generate date
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
        // **********
        // if payment use credit card
        // validate credit card
        $scope.validateCreditCard = function () {

        }
        // **********
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

        // log $scope
        $scope.printScope = function () {
            console.log($scope);
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
]);// checkout controller

// travorama angular app - confirmation controller
app.controller('confirmationController', [
    '$http', '$scope', function ($http, $scope) {

        $scope.pageLoaded = true;

    }
]);// confirmation controller