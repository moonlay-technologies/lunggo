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
        $scope.stepClass = '';
        $scope.titles = [
            { name: 'Tn.', value: 'Mister' },
            { name: 'Ny.', value: 'Mistress' },
            { name: 'Nn.', value: 'Miss' }
        ];

        $scope.msToTime = function (duration) {
        
            var milliseconds = parseInt((duration % 1000) / 100),
                 seconds = parseInt((duration / 1000) % 60),
                 minutes = parseInt((duration / (1000 * 60)) % 60),
                 hours = parseInt((duration / (1000 * 60 * 60)));
            hours = hours;
            minutes = minutes;
            seconds = seconds;
            return hours + "j " + minutes + "m";
        }

        //silubab
        $scope.adultCount = adultPassenger;
        $scope.adultFare = adultFare;
        $scope.childCount = childPassenger;
        $scope.childFare = childFare;
        $scope.infantCount = infantPassenger;
        $scope.infantFare = infantFare;

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
        $scope.monthNames = ["Januari", "Februari", "Maret", "April", "Mei", "Juni", "Juli", "Agustus", "September", "Oktober", "November", "Desember"];
        
        $scope.form = {
            email: '',
            password: '',
            submitting: false,
            isLogin: false,
            submitted: false
        };
        //Function Login in Checkout Page
        $scope.form.submit = function () {
            $scope.form.submitting = true;
            $http.post(LoginConfig.Url, {
                email: $scope.form.email,
                password: $scope.form.password,
                clientId: 'Jajal',
                clientSecret: 'Standar'
            }).success(function (returnData) {
                $scope.form.submitting = false;
                $scope.form.submitted = true;
                if (returnData.status == '200') {
                    setCookie('accesstoken', returnData.accessToken, returnData.expTime);
                    setRefreshCookie('refreshtoken', returnData.refreshToken);
                    $scope.TakeProfileConfig.TakeProfile();
                    $scope.loggedIn = getCookie('accesstoken');
                }
                else {
                    window.location.href = $location.absUrl();
                    $scope.form.submitting = false;
                    $scope.form.submitted = false;
                    $scope.form.isLogin = false;
                    //Return langsung
                }
            }, function (returnData) {
                console.log('Failed to Login');
                $scope.form.submitting = false;
                $scope.form.submitted = false;
                $scope.form.isLogin = false;
                window.location.href = $location.absUrl();
                //return langsung
            });
        }
        
        //Get Profile
        $scope.TakeProfileConfig = {
            TakeProfile: function () {
                //Check Authorization
                var authAccess = getAuthAccess();
                if (authAccess == 1) {
                    $scope.getFlightHeader = 'Bearer ' + getCookie('accesstoken');
                }
                else {
                    $scope.getFlightHeader = null;
                }
                $http.get(GetProfileConfig.Url, {
                    headers: { 'Authorization': $scope.getFlightHeader }
                }).success(function (returnData) {
                    if (returnData.status == "200") {
                        $scope.buyerInfo.name = returnData.name;
                        $scope.buyerInfo.countryCode = parseInt(returnData.countryCallCd);
                        $scope.buyerInfo.phone = parseInt(returnData.phone);
                        $scope.buyerInfo.email = returnData.email;
                        window.location.href = $location.absUrl();
                    }
                    else {
                        $scope.buyerInfo = {};
                        console.log('There is an error');
                        console.log('Error : ' + returnData.error);
                        console.log(returnData);
                        window.location.href = $location.absUrl();
                    }
                }, function (returnData) {
                    $scope.buyerInfo = {};
                    console.log('Failed to Get Profile');
                    console.log(returnData);
                    window.location.href = $location.absUrl();
                });
            }
        }


        $scope.book = {
            booking: false,
            url: FlightBookConfig.Url,
            postData: '',
            checked: false,
            newPrice : '',
            rsvNo: '',
            isSuccess: false,
            isPriceChanged: false,
            
            send: function () {
                $scope.book.booking = true;
                $scope.book.isPriceChanged = false;
                $scope.book.checked = false;
                $scope.contactData = '{' + ' "title":"' + $scope.buyerInfo.title + '",  "name" :"' + $scope.buyerInfo.name + '","countryCallCd":"' + $scope.buyerInfo.countryCode + '","phone":"' + $scope.buyerInfo.phone + '","email":"' + $scope.buyerInfo.email + '"' + '}';
                $scope.paxData = ' "pax" : [ ';

                // generate data
                $scope.book.postData =' "token":"' + $scope.token + '",  "contact" :' + $scope.contactData + ',"lang":"' + $scope.language + '"';
                for (var i = 0; i < $scope.passengers.length; i++) {

                    // check nationality
                    if (!$scope.passportRequired) {
                        $scope.passengers[i].passport.number = '';
                        $scope.passengers[i].passport.expire.date = '';
                        $scope.passengers[i].passport.expire.month = '';
                        $scope.passengers[i].passport.expire.year = '';
                        $scope.passengers[i].passport.expire.full = '';
                        if (!$scope.nationalityRequired) {
                            $scope.passengers[i].nationality = '';
                        }
                    }
                    if (!$scope.idRequired) {
                        $scope.passengers[i].idNumber = '';
                    }
                    
                    // birthdate
                    $scope.passengers[i].birth.full = $scope.passengers[i].birth.year
                        + '/' + ('0' + (parseInt($scope.passengers[i].birth.month) + 1)).slice(-2)
                        + '/' + ('0' + $scope.passengers[i].birth.date).slice(-2);
                    // passport expiry date
                    $scope.passengers[i].passport.expire.full = $scope.passengers[i].passport.expire.year + '/' + ('0' + (parseInt($scope.passengers[i].passport.expire.month) + 1)).slice(-2) + '/' + ('0' + $scope.passengers[i].passport.expire.date).slice(-2);

                    $scope.paxData = $scope.paxData + '{ "type":"' + $scope.passengers[i].type + '", "title":"' + $scope.passengers[i].title + '" , "name":"' + $scope.passengers[i].name + '" ';

                    if ($scope.birthDateRequired) {
                        $scope.paxData = $scope.paxData + ', "dob":"' + $scope.passengers[i].birth.full + '"';
                    }

                    if ($scope.nationalityRequired) {
                        $scope.paxData = $scope.paxData + ', "nationality":"' + $scope.passengers[i].nationality+ '"';
                    }

                    if ($scope.passportRequired) {
                        $scope.paxData = $scope.paxData + ', "passportNo":"' + $scope.passengers[i].passport.number + '" , "passportExp":"' + $scope.passengers[i].passport.expire.full + '" , "passportCountry":"' + $scope.passengers[i].passport.country + '" ';
                    }

                    if (i != $scope.passengers.length - 1) {
                        $scope.paxData = $scope.paxData + '}' + ',';
                    } else {
                        $scope.paxData = $scope.paxData + '}';
                    }

                    //if (!$scope.passportRequired) {
                    //    if (i != $scope.passengers.length - 1) {
                    //        $scope.paxData = $scope.paxData + '{ "type":"' + $scope.passengers[i].type + '", "title":"' + $scope.passengers[i].title + '" , "name":"' + $scope.passengers[i].name + '" , "dob":"' + $scope.passengers[i].birth.full + '" , "nationality":"' + $scope.passengers[i].nationality+ '" },';
                    //    }
                    //    else {
                    //        $scope.paxData = $scope.paxData + '{ "type":"' + $scope.passengers[i].type + '", "title":"' + $scope.passengers[i].title + '" , "name":"' + $scope.passengers[i].name + '" , "dob":"' + $scope.passengers[i].birth.full + '"  , "nationality":"' + $scope.passengers[i].nationality + '" }';
                    //    }
                    //}
                    //else
                    //{
                    //    if (i != $scope.passengers.length - 1) {
                    //        $scope.paxData = $scope.paxData + '{ "type":"' + $scope.passengers[i].type + '", "title":"' + $scope.passengers[i].title + '" , "name":"' + $scope.passengers[i].name + '" , "dob":"' + $scope.passengers[i].birth.full + '", "passportNo":"' + $scope.passengers[i].passport.number + '" , "passportExp":"' + $scope.passengers[i].passport.expire.full + '" , "passportCountry":"' + $scope.passengers[i].passport.country + '" , "nationality":"' + $scope.passengers[i].passport.country + '" },';
                    //    }
                    //    else {
                    //        $scope.paxData = $scope.paxData + '{ "type":"' + $scope.passengers[i].type + '", "title":"' + $scope.passengers[i].title + '" , "name":"' + $scope.passengers[i].name + '" , "dob":"' + $scope.passengers[i].birth.full + '" , "passportNo":"' + $scope.passengers[i].passport.number + '" , "passportExp":"' + $scope.passengers[i].passport.expire.full + '" , "passportCountry":"' + $scope.passengers[i].passport.country + '" , "nationality":"' + $scope.passengers[i].passport.country + '" }';
                    //    }
                    //}                 
                }
                
                $scope.paxData = $scope.paxData + ']';
                $scope.book.postData = '{' + $scope.book.postData + ',' + $scope.paxData + '}';
                console.log($scope.book.postData);
                $scope.book.postData = JSON.parse($scope.book.postData);

                console.log($scope.book.postData);

                //Check Authorization
                var authAccess = getAuthAccess();
                if (authAccess == 1) {
                    $scope.getFlightHeader = 'Bearer ' + getCookie('accesstoken');
                }
                else {
                    $scope.getFlightHeader = null;
                }

                // send form
                $http({
                    method: 'POST',
                    url: $scope.book.url,
                    data: $scope.book.postData,
                    headers: { 'Authorization': $scope.getFlightHeader }
                }).then(function (returnData) {
                    //console.log(returnData);
                    if (returnData.data.status == '200' && (returnData.data.rsvNo != '' || returnData.data.rsvNo != null)) {
                        if (returnData.data.price != null) { 
                            $scope.book.priceChanged = true;
                            $scope.book.isSuccess = true;
                            $scope.book.newPrice = returnData.data.price;
                            $scope.book.checked = false;
                        }
                        else {
                            $scope.book.isSuccess = true;
                            $scope.book.rsvNo = returnData.data.rsvNo;

                            $('form#rsvno input#rsvno-input').val(returnData.data.rsvNo);
                            $('form#rsvno').submit();
                            $scope.book.checked = true;
                        }
                        
                    } else {
                        if (returnData.data.price != null) {
                            $scope.book.isPriceChanged = true;
                            $scope.book.isSuccess = false;
                            $scope.book.newPrice = returnData.data.price;
                            $scope.book.checked = true;
                        }
                        else {
                            $scope.book.isSuccess = false;
                            $scope.book.checked = true;
                            console.log(returnData);
                            $scope.errorMessage = returnData.data.error;
                        }
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

        $scope.loggedIn = getCookie('accesstoken');

        $scope.buyerInfo = {};
        if ($scope.loggedIn) {
            // call API get Profile
            var authAccess = getAuthAccess();
            if (authAccess == 1) {
                $scope.getFlightHeader = 'Bearer ' + getCookie('accesstoken');
            }
            else {
                $scope.getFlightHeader = null;
            }
            $http.get(GetProfileConfig.Url, {
                headers: { 'Authorization': $scope.getFlightHeader }
            }).success(function (returnData) {
                if (returnData.status == "200") {
                    console.log('Success getting Profile');
                    $scope.buyerInfo.name = returnData.name;
                    $scope.buyerInfo.countryCode = parseInt(returnData.countryCallCd);
                    $scope.buyerInfo.phone = parseInt(returnData.phone);
                    $scope.buyerInfo.email = returnData.email;
                }
                else {
                    $scope.buyerInfo = {};
                }
            }, function (returnData) {
                $scope.buyerInfo = {};
            });
        } else {
            $scope.buyerInfo = {};
        }
        $scope.adultPassenger = [];
        $scope.childPassenger = [];
        $scope.infantPassenger = [];

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

        $scope.flightDetail = {};

        $scope.flightDetail.departureFullDate = new Date(departureDate);
        $scope.flightDetail.beforeDepartureFullDate = new Date(beforeDepartureDate);
        $scope.flightDetail.passportFullDate = new Date(passportDate);

        $scope.passportRequired = passportRequired;
        $scope.idRequired = idRequired;
        $scope.nationalityRequired = nationalityRequired;
        $scope.birthDateRequired = birthDateRequired;

        $scope.bookingDate = new Date();

        $scope.countries = [{ code: 'AF', name: 'Afghanistan' }, { code: 'AX', name: 'Aland Islands' }, { code: 'AL', name: 'Albania' }, { code: 'DZ', name: 'Algeria' }, { code: 'AS', name: 'American Samoa' }, { code: 'AD', name: 'Andorra' }, { code: 'AO', name: 'Angola' }, { code: 'AI', name: 'Anguilla' }, { code: 'AQ', name: 'Antarctica' }, { code: 'AG', name: 'Antigua And Barbuda' }, { code: 'AR', name: 'Argentina' }, { code: 'AM', name: 'Armenia' }, { code: 'AW', name: 'Aruba' }, { code: 'AU', name: 'Australia' }, { code: 'AT', name: 'Austria' }, { code: 'AZ', name: 'Azerbaijan' }, { code: 'BS', name: 'Bahamas' }, { code: 'BH', name: 'Bahrain' }, { code: 'BD', name: 'Bangladesh' }, { code: 'BB', name: 'Barbados' }, { code: 'BY', name: 'Belarus' }, { code: 'BE', name: 'Belgium' }, { code: 'BZ', name: 'Belize' }, { code: 'BJ', name: 'Benin' }, { code: 'BM', name: 'Bermuda' }, { code: 'BT', name: 'Bhutan' }, { code: 'BO', name: 'Bolivia' }, { code: 'BA', name: 'Bosnia And Herzegovina' }, { code: 'BW', name: 'Botswana' }, { code: 'BV', name: 'Bouvet Island' }, { code: 'BR', name: 'Brazil' }, { code: 'IO', name: 'British Indian Ocean Territory' }, { code: 'BN', name: 'Brunei Darussalam' }, { code: 'BG', name: 'Bulgaria' }, { code: 'BF', name: 'Burkina Faso' }, { code: 'BI', name: 'Burundi' }, { code: 'KH', name: 'Cambodia' }, { code: 'CM', name: 'Cameroon' }, { code: 'CA', name: 'Canada' }, { code: 'CV', name: 'Cape Verde' }, { code: 'KY', name: 'Cayman Islands' }, { code: 'CF', name: 'Central African Republic' }, { code: 'TD', name: 'Chad' }, { code: 'CL', name: 'Chile' }, { code: 'CN', name: 'China' }, { code: 'CX', name: 'Christmas Island' }, { code: 'CC', name: 'Cocos (Keeling) Islands' }, { code: 'CO', name: 'Colombia' }, { code: 'KM', name: 'Comoros' }, { code: 'CG', name: 'Congo' }, { code: 'CD', name: 'Congo, Democratic Republic' }, { code: 'CK', name: 'Cook Islands' }, { code: 'CR', name: 'Costa Rica' }, { code: 'CI', name: 'Cote D\'Ivoire' }, { code: 'HR', name: 'Croatia' }, { code: 'CU', name: 'Cuba' }, { code: 'CY', name: 'Cyprus' }, { code: 'CZ', name: 'Czech Republic' }, { code: 'DK', name: 'Denmark' }, { code: 'DJ', name: 'Djibouti' }, { code: 'DM', name: 'Dominica' }, { code: 'DO', name: 'Dominican Republic' }, { code: 'EC', name: 'Ecuador' }, { code: 'EG', name: 'Egypt' }, { code: 'SV', name: 'El Salvador' }, { code: 'GQ', name: 'Equatorial Guinea' }, { code: 'ER', name: 'Eritrea' }, { code: 'EE', name: 'Estonia' }, { code: 'ET', name: 'Ethiopia' }, { code: 'FK', name: 'Falkland Islands (Malvinas)' }, { code: 'FO', name: 'Faroe Islands' }, { code: 'FJ', name: 'Fiji' }, { code: 'FI', name: 'Finland' }, { code: 'FR', name: 'France' }, { code: 'GF', name: 'French Guiana' }, { code: 'PF', name: 'French Polynesia' }, { code: 'TF', name: 'French Southern Territories' }, { code: 'GA', name: 'Gabon' }, { code: 'GM', name: 'Gambia' }, { code: 'GE', name: 'Georgia' }, { code: 'DE', name: 'Germany' }, { code: 'GH', name: 'Ghana' }, { code: 'GI', name: 'Gibraltar' }, { code: 'GR', name: 'Greece' }, { code: 'GL', name: 'Greenland' }, { code: 'GD', name: 'Grenada' }, { code: 'GP', name: 'Guadeloupe' }, { code: 'GU', name: 'Guam' }, { code: 'GT', name: 'Guatemala' }, { code: 'GG', name: 'Guernsey' }, { code: 'GN', name: 'Guinea' }, { code: 'GW', name: 'Guinea-Bissau' }, { code: 'GY', name: 'Guyana' }, { code: 'HT', name: 'Haiti' }, { code: 'HM', name: 'Heard Island & Mcdonald Islands' }, { code: 'VA', name: 'Holy See (Vatican City State)' }, { code: 'HN', name: 'Honduras' }, { code: 'HK', name: 'Hong Kong' }, { code: 'HU', name: 'Hungary' }, { code: 'IS', name: 'Iceland' }, { code: 'IN', name: 'India' }, { code: 'ID', name: 'Indonesia' }, { code: 'IR', name: 'Iran, Islamic Republic Of' }, { code: 'IQ', name: 'Iraq' }, { code: 'IE', name: 'Ireland' }, { code: 'IM', name: 'Isle Of Man' }, { code: 'IL', name: 'Israel' }, { code: 'IT', name: 'Italy' }, { code: 'JM', name: 'Jamaica' }, { code: 'JP', name: 'Japan' }, { code: 'JE', name: 'Jersey' }, { code: 'JO', name: 'Jordan' }, { code: 'KZ', name: 'Kazakhstan' }, { code: 'KE', name: 'Kenya' }, { code: 'KI', name: 'Kiribati' }, { code: 'KR', name: 'Korea' }, { code: 'KW', name: 'Kuwait' }, { code: 'KG', name: 'Kyrgyzstan' }, { code: 'LA', name: 'Lao People\'s Democratic Republic' }, { code: 'LV', name: 'Latvia' }, { code: 'LB', name: 'Lebanon' }, { code: 'LS', name: 'Lesotho' }, { code: 'LR', name: 'Liberia' }, { code: 'LY', name: 'Libyan Arab Jamahiriya' }, { code: 'LI', name: 'Liechtenstein' }, { code: 'LT', name: 'Lithuania' }, { code: 'LU', name: 'Luxembourg' }, { code: 'MO', name: 'Macao' }, { code: 'MK', name: 'Macedonia' }, { code: 'MG', name: 'Madagascar' }, { code: 'MW', name: 'Malawi' }, { code: 'MY', name: 'Malaysia' }, { code: 'MV', name: 'Maldives' }, { code: 'ML', name: 'Mali' }, { code: 'MT', name: 'Malta' }, { code: 'MH', name: 'Marshall Islands' }, { code: 'MQ', name: 'Martinique' }, { code: 'MR', name: 'Mauritania' }, { code: 'MU', name: 'Mauritius' }, { code: 'YT', name: 'Mayotte' }, { code: 'MX', name: 'Mexico' }, { code: 'FM', name: 'Micronesia, Federated States Of' }, { code: 'MD', name: 'Moldova' }, { code: 'MC', name: 'Monaco' }, { code: 'MN', name: 'Mongolia' }, { code: 'ME', name: 'Montenegro' }, { code: 'MS', name: 'Montserrat' }, { code: 'MA', name: 'Morocco' }, { code: 'MZ', name: 'Mozambique' }, { code: 'MM', name: 'Myanmar' }, { code: 'NA', name: 'Namibia' }, { code: 'NR', name: 'Nauru' }, { code: 'NP', name: 'Nepal' }, { code: 'NL', name: 'Netherlands' }, { code: 'AN', name: 'Netherlands Antilles' }, { code: 'NC', name: 'New Caledonia' }, { code: 'NZ', name: 'New Zealand' }, { code: 'NI', name: 'Nicaragua' }, { code: 'NE', name: 'Niger' }, { code: 'NG', name: 'Nigeria' }, { code: 'NU', name: 'Niue' }, { code: 'NF', name: 'Norfolk Island' }, { code: 'MP', name: 'Northern Mariana Islands' }, { code: 'NO', name: 'Norway' }, { code: 'OM', name: 'Oman' }, { code: 'PK', name: 'Pakistan' }, { code: 'PW', name: 'Palau' }, { code: 'PS', name: 'Palestinian Territory, Occupied' }, { code: 'PA', name: 'Panama' }, { code: 'PG', name: 'Papua New Guinea' }, { code: 'PY', name: 'Paraguay' }, { code: 'PE', name: 'Peru' }, { code: 'PH', name: 'Philippines' }, { code: 'PN', name: 'Pitcairn' }, { code: 'PL', name: 'Poland' }, { code: 'PT', name: 'Portugal' }, { code: 'PR', name: 'Puerto Rico' }, { code: 'QA', name: 'Qatar' }, { code: 'RE', name: 'Reunion' }, { code: 'RO', name: 'Romania' }, { code: 'RU', name: 'Russian Federation' }, { code: 'RW', name: 'Rwanda' }, { code: 'BL', name: 'Saint Barthelemy' }, { code: 'SH', name: 'Saint Helena' }, { code: 'KN', name: 'Saint Kitts And Nevis' }, { code: 'LC', name: 'Saint Lucia' }, { code: 'MF', name: 'Saint Martin' }, { code: 'PM', name: 'Saint Pierre And Miquelon' }, { code: 'VC', name: 'Saint Vincent And Grenadines' }, { code: 'WS', name: 'Samoa' }, { code: 'SM', name: 'San Marino' }, { code: 'ST', name: 'Sao Tome And Principe' }, { code: 'SA', name: 'Saudi Arabia' }, { code: 'SN', name: 'Senegal' }, { code: 'RS', name: 'Serbia' }, { code: 'SC', name: 'Seychelles' }, { code: 'SL', name: 'Sierra Leone' }, { code: 'SG', name: 'Singapore' }, { code: 'SK', name: 'Slovakia' }, { code: 'SI', name: 'Slovenia' }, { code: 'SB', name: 'Solomon Islands' }, { code: 'SO', name: 'Somalia' }, { code: 'ZA', name: 'South Africa' }, { code: 'GS', name: 'South Georgia And Sandwich Isl.' }, { code: 'ES', name: 'Spain' }, { code: 'LK', name: 'Sri Lanka' }, { code: 'SD', name: 'Sudan' }, { code: 'SR', name: 'Suriname' }, { code: 'SJ', name: 'Svalbard And Jan Mayen' }, { code: 'SZ', name: 'Swaziland' }, { code: 'SE', name: 'Sweden' }, { code: 'CH', name: 'Switzerland' }, { code: 'SY', name: 'Syrian Arab Republic' }, { code: 'TW', name: 'Taiwan' }, { code: 'TJ', name: 'Tajikistan' }, { code: 'TZ', name: 'Tanzania' }, { code: 'TH', name: 'Thailand' }, { code: 'TL', name: 'Timor-Leste' }, { code: 'TG', name: 'Togo' }, { code: 'TK', name: 'Tokelau' }, { code: 'TO', name: 'Tonga' }, { code: 'TT', name: 'Trinidad And Tobago' }, { code: 'TN', name: 'Tunisia' }, { code: 'TR', name: 'Turkey' }, { code: 'TM', name: 'Turkmenistan' }, { code: 'TC', name: 'Turks And Caicos Islands' }, { code: 'TV', name: 'Tuvalu' }, { code: 'UG', name: 'Uganda' }, { code: 'UA', name: 'Ukraine' }, { code: 'AE', name: 'United Arab Emirates' }, { code: 'GB', name: 'United Kingdom' }, { code: 'US', name: 'United States' }, { code: 'UM', name: 'United States Outlying Islands' }, { code: 'UY', name: 'Uruguay' }, { code: 'UZ', name: 'Uzbekistan' }, { code: 'VU', name: 'Vanuatu' }, { code: 'VE', name: 'Venezuela' }, { code: 'VN', name: 'Viet Nam' }, { code: 'VG', name: 'Virgin Islands, British' }, { code: 'VI', name: 'Virgin Islands, U.S.' }, { code: 'WF', name: 'Wallis And Futuna' }, { code: 'EH', name: 'Western Sahara' }, { code: 'YE', name: 'Yemen' }, { code: 'ZM', name: 'Zambia' }, { code: 'ZW', name: 'Zimbabwe' }];
        $scope.displayCountry = function (code) {
            for (var i = 0; i < $scope.countries.length; i++) {
                if ($scope.countries[i].code === code) {
                    return $scope.countries[i].name;
                }
            }
        }
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
                    date: $scope.flightDetail.departureFullDate.getDate(),
                    month: $scope.flightDetail.departureFullDate.getMonth(),
                    year: ($scope.flightDetail.departureFullDate.getFullYear() - 12)
                };
                passenger.type = 'adult';
            } else if (passenger.type == 'infant') {
                passenger.birth = {
                    date: $scope.flightDetail.beforeDepartureFullDate.getDate(),
                    month: $scope.flightDetail.beforeDepartureFullDate.getMonth(),
                    year: $scope.flightDetail.beforeDepartureFullDate.getFullYear()
                };
                passenger.type = 'infant';
            } else if (passenger.type == 'child') {
                passenger.birth = {
                    date: $scope.flightDetail.beforeDepartureFullDate.getDate(),
                    month: $scope.flightDetail.beforeDepartureFullDate.getMonth(),
                    year: ($scope.flightDetail.beforeDepartureFullDate.getFullYear() - 2)
                };
                passenger.type = 'child';
            }
            if (nationalityRequired == true) {
                passenger.nationality = 'Indonesia';
            }
            passenger.passport = {
                expire: {
                    date: $scope.flightDetail.passportFullDate.getDate(),
                    month: $scope.flightDetail.passportFullDate.getMonth(),
                    year: $scope.flightDetail.passportFullDate.getFullYear(),
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
                    minYear = $scope.flightDetail.departureFullDate.getFullYear() - 12;
                } else if (passenger.type == 'infant') {
                    minYear = $scope.flightDetail.departureFullDate.getFullYear() - 2;
                }

                if (passenger.birth.year == minYear) {
                    if (passenger.birth.month <= $scope.flightDetail.departureFullDate.getMonth()) {
                        passenger.birth.month = $scope.flightDetail.departureFullDate.getMonth();
                        if (passenger.birth.date < $scope.flightDetail.departureFullDate.getDate()) {
                            passenger.birth.date = $scope.flightDetail.departureFullDate.getDate();
                        }
                    }
                } else if (passenger.birth.year == $scope.bookingDate.getFullYear()) {
                    if (passenger.birth.month >= $scope.flightDetail.departureFullDate.getMonth()) {
                        passenger.birth.month = $scope.flightDetail.departureFullDate.getMonth();
                        if (passenger.birth.date > $scope.flightDetail.departureFullDate.getDate()) {
                            passenger.birth.date = $scope.flightDetail.departureFullDate.getDate();
                        }
                    }
                }
            }
        }
        // validate passport expiry date
        $scope.validatePassport = function (passenger) {
            if (passenger.passport.expire.year == $scope.flightDetail.passportDepartureFullDate.getFullYear()) {
                if (passenger.passport.expire.month < $scope.flightDetail.passportDepartureFullDate.getMonth()) {
                    passenger.passport.expire.month = $scope.flightDetail.passportDepartureFullDate.getMonth();
                    if (passenger.passport.expire.date < $scope.flightDetail.passportDepartureFullDate.getDate()) {
                        passenger.passport.expire.date = $scope.flightDetail.passportDepartureFullDate.getDate();
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

        $scope.changeTitle = function(title) {
            if (title == 'Mister')
                return 'Tn.';
            else if (title == 'Mistress')
                return 'Ny.';
            else if (title == 'Miss')
                return 'Nn.';
        }

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
    }
]);// checkout controller

// travorama angular app - confirmation controller
app.controller('confirmationController', [
    '$http', '$scope', function ($http, $scope) {

        $scope.pageLoaded = true;
        $scope.msToTime = function (duration) {

            var milliseconds = parseInt((duration % 1000) / 100),
                 seconds = parseInt((duration / 1000) % 60),
                 minutes = parseInt((duration / (1000 * 60)) % 60),
                 hours = parseInt((duration / (1000 * 60 * 60)));
            hours = hours;
            minutes = minutes;
            seconds = seconds;
            return hours + "j " + minutes + "m";
        }

    }
]);// confirmation controller

// travorama angular app - confirmation controller
app.controller('thankyouController', [
    '$http', '$scope', function ($http, $scope) {

        $scope.pageLoaded = true;
        $scope.msToTime = function (duration) {

            var milliseconds = parseInt((duration % 1000) / 100),
                 seconds = parseInt((duration / 1000) % 60),
                 minutes = parseInt((duration / (1000 * 60)) % 60),
                 hours = parseInt((duration / (1000 * 60 * 60)));
            hours = hours;
            minutes = minutes;
            seconds = seconds;
            return hours + "j " + minutes + "m";
        }

    }
]);// confirmation controller