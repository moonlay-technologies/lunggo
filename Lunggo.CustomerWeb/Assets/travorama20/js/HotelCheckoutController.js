// travorama angular app - checkout controller
app.controller('hotelcheckoutController', [
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
        
        function daysBetween(date1, date2) {

            // The number of milliseconds in one day
            var ONE_DAY = 1000 * 60 * 60 * 24

            // Convert both dates to milliseconds
            var date1_ms = date1.getTime()
            var date2_ms = date2.getTime()

            // Calculate the difference in milliseconds
            var difference_ms = Math.abs(date1_ms - date2_ms)

            // Convert back to days and return
            return Math.round(difference_ms / ONE_DAY)
        }

        
        //Declaration From CSHTML
        $scope.token = token;
        $scope.guestInfo = {};
        $scope.guestInfo.name = '';
        $scope.netFare = netFare;
        $scope.originalFare = originalFare;
        $scope.expired = false;
        $scope.expiryDate = new Date(expirytime);
        $scope.hotelDetail = hotelDetail;
        $scope.totalRoom = totalRoom;
        $scope.checkin = new Date(checkin.substring(0, 4), checkin.substring(4, 6) - 1, checkin.substring(6, 8));
        $scope.checkout = new Date(checkout.substring(0, 4), checkout.substring(4, 6) - 1, checkout.substring(6, 8));
        $scope.nights = daysBetween($scope.checkin, $scope.checkout);
        $scope.countries = [{ name: 'Pilih Negara', dial_code: 'xx', code: '' }, { name: 'Afghanistan', dial_code: '93', code: 'AF' }, { name: 'Aland Islands', dial_code: '358', code: 'AX' }, { name: 'Albania', dial_code: '355', code: 'AL' }, { name: 'Algeria', dial_code: '213', code: 'DZ' }, { name: 'AmericanSamoa', dial_code: '1 684', code: 'AS' }, { name: 'Andorra', dial_code: '376', code: 'AD' }, { name: 'Angola', dial_code: '244', code: 'AO' }, { name: 'Anguilla', dial_code: '1 264', code: 'AI' }, { name: 'Antarctica', dial_code: '672', code: 'AQ' }, { name: 'Antigua and Barbuda', dial_code: '1268', code: 'AG' }, { name: 'Argentina', dial_code: '54', code: 'AR' }, { name: 'Armenia', dial_code: '374', code: 'AM' }, { name: 'Aruba', dial_code: '297', code: 'AW' }, { name: 'Australia', dial_code: '61', code: 'AU' }, { name: 'Austria', dial_code: '43', code: 'AT' }, { name: 'Azerbaijan', dial_code: '994', code: 'AZ' }, { name: 'Bahamas', dial_code: '1 242', code: 'BS' }, { name: 'Bahrain', dial_code: '973', code: 'BH' }, { name: 'Bangladesh', dial_code: '880', code: 'BD' }, { name: 'Barbados', dial_code: '1 246', code: 'BB' }, { name: 'Belarus', dial_code: '375', code: 'BY' }, { name: 'Belgium', dial_code: '32', code: 'BE' }, { name: 'Belize', dial_code: '501', code: 'BZ' }, { name: 'Benin', dial_code: '229', code: 'BJ' }, { name: 'Bermuda', dial_code: '1 441', code: 'BM' }, { name: 'Bhutan', dial_code: '975', code: 'BT' }, { name: 'Bolivia, Plurinational State of', dial_code: '591', code: 'BO' }, { name: 'Bosnia and Herzegovina', dial_code: '387', code: 'BA' }, { name: 'Botswana', dial_code: '267', code: 'BW' }, { name: 'Brazil', dial_code: '55', code: 'BR' }, { name: 'British Indian Ocean Territory', dial_code: '246', code: 'IO' }, { name: 'Brunei Darussalam', dial_code: '673', code: 'BN' }, { name: 'Bulgaria', dial_code: '359', code: 'BG' }, { name: 'Burkina Faso', dial_code: '226', code: 'BF' }, { name: 'Burundi', dial_code: '257', code: 'BI' }, { name: 'Cambodia', dial_code: '855', code: 'KH' }, { name: 'Cameroon', dial_code: '237', code: 'CM' }, { name: 'Canada', dial_code: '1', code: 'CA' }, { name: 'Cape Verde', dial_code: '238', code: 'CV' }, { name: 'Cayman Islands', dial_code: ' 345', code: 'KY' }, { name: 'Central African Republic', dial_code: '236', code: 'CF' }, { name: 'Chad', dial_code: '235', code: 'TD' }, { name: 'Chile', dial_code: '56', code: 'CL' }, { name: 'China', dial_code: '86', code: 'CN' }, { name: 'Christmas Island', dial_code: '61', code: 'CX' }, { name: 'Cocos (Keeling) Islands', dial_code: '61', code: 'CC' }, { name: 'Colombia', dial_code: '57', code: 'CO' }, { name: 'Comoros', dial_code: '269', code: 'KM' }, { name: 'Congo', dial_code: '242', code: 'CG' }, { name: 'Congo, The Democratic Republic of the Congo', dial_code: '243', code: 'CD' }, { name: 'Cook Islands', dial_code: '682', code: 'CK' }, { name: 'Costa Rica', dial_code: '506', code: 'CR' }, { name: "Cote d'Ivoire", dial_code: '225', code: 'CI' }, { name: 'Croatia', dial_code: '385', code: 'HR' }, { name: 'Cuba', dial_code: '53', code: 'CU' }, { name: 'Cyprus', dial_code: '357', code: 'CY' }, { name: 'Czech Republic', dial_code: '420', code: 'CZ' }, { name: 'Denmark', dial_code: '45', code: 'DK' }, { name: 'Djibouti', dial_code: '253', code: 'DJ' }, { name: 'Dominica', dial_code: '1 767', code: 'DM' }, { name: 'Dominican Republic', dial_code: '1 849', code: 'DO' }, { name: 'Ecuador', dial_code: '593', code: 'EC' }, { name: 'Egypt', dial_code: '20', code: 'EG' }, { name: 'El Salvador', dial_code: '503', code: 'SV' }, { name: 'Equatorial Guinea', dial_code: '240', code: 'GQ' }, { name: 'Eritrea', dial_code: '291', code: 'ER' }, { name: 'Estonia', dial_code: '372', code: 'EE' }, { name: 'Ethiopia', dial_code: '251', code: 'ET' }, { name: 'Falkland Islands (Malvinas)', dial_code: '500', code: 'FK' }, { name: 'Faroe Islands', dial_code: '298', code: 'FO' }, { name: 'Fiji', dial_code: '679', code: 'FJ' }, { name: 'Finland', dial_code: '358', code: 'FI' }, { name: 'France', dial_code: '33', code: 'FR' }, { name: 'French Guiana', dial_code: '594', code: 'GF' }, { name: 'French Polynesia', dial_code: '689', code: 'PF' }, { name: 'Gabon', dial_code: '241', code: 'GA' }, { name: 'Gambia', dial_code: '220', code: 'GM' }, { name: 'Georgia', dial_code: '995', code: 'GE' }, { name: 'Germany', dial_code: '49', code: 'DE' }, { name: 'Ghana', dial_code: '233', code: 'GH' }, { name: 'Gibraltar', dial_code: '350', code: 'GI' }, { name: 'Greece', dial_code: '30', code: 'GR' }, { name: 'Greenland', dial_code: '299', code: 'GL' }, { name: 'Grenada', dial_code: '1 473', code: 'GD' }, { name: 'Guadeloupe', dial_code: '590', code: 'GP' }, { name: 'Guam', dial_code: '1 671', code: 'GU' }, { name: 'Guatemala', dial_code: '502', code: 'GT' }, { name: 'Guernsey', dial_code: '44', code: 'GG' }, { name: 'Guinea', dial_code: '224', code: 'GN' }, { name: 'Guinea-Bissau', dial_code: '245', code: 'GW' }, { name: 'Guyana', dial_code: '595', code: 'GY' }, { name: 'Haiti', dial_code: '509', code: 'HT' }, { name: 'Holy See (Vatican City State)', dial_code: '379', code: 'VA' }, { name: 'Honduras', dial_code: '504', code: 'HN' }, { name: 'Hong Kong', dial_code: '852', code: 'HK' }, { name: 'Hungary', dial_code: '36', code: 'HU' }, { name: 'Iceland', dial_code: '354', code: 'IS' }, { name: 'India', dial_code: '91', code: 'IN' }, { name: 'Indonesia', dial_code: '62', code: 'ID' }, { name: 'Iran, Islamic Republic of Persian Gulf', dial_code: '98', code: 'IR' }, { name: 'Iraq', dial_code: '964', code: 'IQ' }, { name: 'Ireland', dial_code: '353', code: 'IE' }, { name: 'Isle of Man', dial_code: '44', code: 'IM' }, { name: 'Israel', dial_code: '972', code: 'IL' }, { name: 'Italy', dial_code: '39', code: 'IT' }, { name: 'Jamaica', dial_code: '1 876', code: 'JM' }, { name: 'Japan', dial_code: '81', code: 'JP' }, { name: 'Jersey', dial_code: '44', code: 'JE' }, { name: 'Jordan', dial_code: '962', code: 'JO' }, { name: 'Kazakhstan', dial_code: '7 7', code: 'KZ' }, { name: 'Kenya', dial_code: '254', code: 'KE' }, { name: 'Kiribati', dial_code: '686', code: 'KI' }, { name: "Korea, Democratic People's Republic of Korea", dial_code: '850', code: 'KP' }, { name: 'Korea, Republic of South Korea', dial_code: '82', code: 'KR' }, { name: 'Kuwait', dial_code: '965', code: 'KW' }, { name: 'Kyrgyzstan', dial_code: '996', code: 'KG' }, { name: 'Laos', dial_code: '856', code: 'LA' }, { name: 'Latvia', dial_code: '371', code: 'LV' }, { name: 'Lebanon', dial_code: '961', code: 'LB' }, { name: 'Lesotho', dial_code: '266', code: 'LS' }, { name: 'Liberia', dial_code: '231', code: 'LR' }, { name: 'Libyan Arab Jamahiriya', dial_code: '218', code: 'LY' }, { name: 'Liechtenstein', dial_code: '423', code: 'LI' }, { name: 'Lithuania', dial_code: '370', code: 'LT' }, { name: 'Luxembourg', dial_code: '352', code: 'LU' }, { name: 'Macao', dial_code: '853', code: 'MO' }, { name: 'Macedonia', dial_code: '389', code: 'MK' }, { name: 'Madagascar', dial_code: '261', code: 'MG' }, { name: 'Malawi', dial_code: '265', code: 'MW' }, { name: 'Malaysia', dial_code: '60', code: 'MY' }, { name: 'Maldives', dial_code: '960', code: 'MV' }, { name: 'Mali', dial_code: '223', code: 'ML' }, { name: 'Malta', dial_code: '356', code: 'MT' }, { name: 'Marshall Islands', dial_code: '692', code: 'MH' }, { name: 'Martinique', dial_code: '596', code: 'MQ' }, { name: 'Mauritania', dial_code: '222', code: 'MR' }, { name: 'Mauritius', dial_code: '230', code: 'MU' }, { name: 'Mayotte', dial_code: '262', code: 'YT' }, { name: 'Mexico', dial_code: '52', code: 'MX' }, { name: 'Micronesia, Federated States of Micronesia', dial_code: '691', code: 'FM' }, { name: 'Moldova', dial_code: '373', code: 'MD' }, { name: 'Monaco', dial_code: '377', code: 'MC' }, { name: 'Mongolia', dial_code: '976', code: 'MN' }, { name: 'Montenegro', dial_code: '382', code: 'ME' }, { name: 'Montserrat', dial_code: '1664', code: 'MS' }, { name: 'Morocco', dial_code: '212', code: 'MA' }, { name: 'Mozambique', dial_code: '258', code: 'MZ' }, { name: 'Myanmar', dial_code: '95', code: 'MM' }, { name: 'Namibia', dial_code: '264', code: 'NA' }, { name: 'Nauru', dial_code: '674', code: 'NR' }, { name: 'Nepal', dial_code: '977', code: 'NP' }, { name: 'Netherlands', dial_code: '31', code: 'NL' }, { name: 'Netherlands Antilles', dial_code: '599', code: 'AN' }, { name: 'New Caledonia', dial_code: '687', code: 'NC' }, { name: 'New Zealand', dial_code: '64', code: 'NZ' }, { name: 'Nicaragua', dial_code: '505', code: 'NI' }, { name: 'Niger', dial_code: '227', code: 'NE' }, { name: 'Nigeria', dial_code: '234', code: 'NG' }, { name: 'Niue', dial_code: '683', code: 'NU' }, { name: 'Norfolk Island', dial_code: '672', code: 'NF' }, { name: 'Northern Mariana Islands', dial_code: '1 670', code: 'MP' }, { name: 'Norway', dial_code: '47', code: 'NO' }, { name: 'Oman', dial_code: '968', code: 'OM' }, { name: 'Pakistan', dial_code: '92', code: 'PK' }, { name: 'Palau', dial_code: '680', code: 'PW' }, { name: 'Palestinian Territory, Occupied', dial_code: '970', code: 'PS' }, { name: 'Panama', dial_code: '507', code: 'PA' }, { name: 'Papua New Guinea', dial_code: '675', code: 'PG' }, { name: 'Paraguay', dial_code: '595', code: 'PY' }, { name: 'Peru', dial_code: '51', code: 'PE' }, { name: 'Philippines', dial_code: '63', code: 'PH' }, { name: 'Pitcairn', dial_code: '872', code: 'PN' }, { name: 'Poland', dial_code: '48', code: 'PL' }, { name: 'Portugal', dial_code: '351', code: 'PT' }, { name: 'Puerto Rico', dial_code: '1 939', code: 'PR' }, { name: 'Qatar', dial_code: '974', code: 'QA' }, { name: 'Romania', dial_code: '40', code: 'RO' }, { name: 'Russia', dial_code: '7', code: 'RU' }, { name: 'Rwanda', dial_code: '250', code: 'RW' }, { name: 'Reunion', dial_code: '262', code: 'RE' }, { name: 'Saint Barthelemy', dial_code: '590', code: 'BL' }, { name: 'Saint Helena, Ascension and Tristan Da Cunha', dial_code: '290', code: 'SH' }, { name: 'Saint Kitts and Nevis', dial_code: '1 869', code: 'KN' }, { name: 'Saint Lucia', dial_code: '1 758', code: 'LC' }, { name: 'Saint Martin', dial_code: '590', code: 'MF' }, { name: 'Saint Pierre and Miquelon', dial_code: '508', code: 'PM' }, { name: 'Saint Vincent and the Grenadines', dial_code: '1 784', code: 'VC' }, { name: 'Samoa', dial_code: '685', code: 'WS' }, { name: 'San Marino', dial_code: '378', code: 'SM' }, { name: 'Sao Tome and Principe', dial_code: '239', code: 'ST' }, { name: 'Saudi Arabia', dial_code: '966', code: 'SA' }, { name: 'Senegal', dial_code: '221', code: 'SN' }, { name: 'Serbia', dial_code: '381', code: 'RS' }, { name: 'Seychelles', dial_code: '248', code: 'SC' }, { name: 'Sierra Leone', dial_code: '232', code: 'SL' }, { name: 'Singapore', dial_code: '65', code: 'SG' }, { name: 'Slovakia', dial_code: '421', code: 'SK' }, { name: 'Slovenia', dial_code: '386', code: 'SI' }, { name: 'Solomon Islands', dial_code: '677', code: 'SB' }, { name: 'Somalia', dial_code: '252', code: 'SO' }, { name: 'South Africa', dial_code: '27', code: 'ZA' }, { name: 'South Georgia and the South Sandwich Islands', dial_code: '500', code: 'GS' }, { name: 'Spain', dial_code: '34', code: 'ES' }, { name: 'Sri Lanka', dial_code: '94', code: 'LK' }, { name: 'Sudan', dial_code: '249', code: 'SD' }, { name: 'Suriname', dial_code: '597', code: 'SR' }, { name: 'Svalbard and Jan Mayen', dial_code: '47', code: 'SJ' }, { name: 'Swaziland', dial_code: '268', code: 'SZ' }, { name: 'Sweden', dial_code: '46', code: 'SE' }, { name: 'Switzerland', dial_code: '41', code: 'CH' }, { name: 'Syrian Arab Republic', dial_code: '963', code: 'SY' }, { name: 'Taiwan', dial_code: '886', code: 'TW' }, { name: 'Tajikistan', dial_code: '992', code: 'TJ' }, { name: 'Tanzania, United Republic of Tanzania', dial_code: '255', code: 'TZ' }, { name: 'Thailand', dial_code: '66', code: 'TH' }, { name: 'Timor-Leste', dial_code: '670', code: 'TL' }, { name: 'Togo', dial_code: '228', code: 'TG' }, { name: 'Tokelau', dial_code: '690', code: 'TK' }, { name: 'Tonga', dial_code: '676', code: 'TO' }, { name: 'Trinidad and Tobago', dial_code: '1 868', code: 'TT' }, { name: 'Tunisia', dial_code: '216', code: 'TN' }, { name: 'Turkey', dial_code: '90', code: 'TR' }, { name: 'Turkmenistan', dial_code: '993', code: 'TM' }, { name: 'Turks and Caicos Islands', dial_code: '1 649', code: 'TC' }, { name: 'Tuvalu', dial_code: '688', code: 'TV' }, { name: 'Uganda', dial_code: '256', code: 'UG' }, { name: 'Ukraine', dial_code: '380', code: 'UA' }, { name: 'United Arab Emirates', dial_code: '971', code: 'AE' }, { name: 'United Kingdom', dial_code: '44', code: 'GB' }, { name: 'United States', dial_code: '1', code: 'US' }, { name: 'Uruguay', dial_code: '598', code: 'UY' }, { name: 'Uzbekistan', dial_code: '998', code: 'UZ' }, { name: 'Vanuatu', dial_code: '678', code: 'VU' }, { name: 'Venezuela, Bolivarian Republic of Venezuela', dial_code: '58', code: 'VE' }, { name: 'Vietnam', dial_code: '84', code: 'VN' }, { name: 'Virgin Islands, British', dial_code: '1 284', code: 'VG' }, { name: 'Virgin Islands, U.S.', dial_code: '1 340', code: 'VI' }, { name: 'Wallis and Futuna', dial_code: '681', code: 'WF' }, { name: 'Yemen', dial_code: '967', code: 'YE' }, { name: 'Zambia', dial_code: '260', code: 'ZM' }, { name: 'Zimbabwe', dial_code: '263', code: 'ZW' }];
        $scope.diffPerson = false;
        $scope.roomType = $scope.hotelDetail.room[0].roomName;
        $scope.roomService = 0;
        $scope.currentPage = 1;
        $scope.stepClass = '';
        $scope.loginShown = false;
        $scope.loggedIn = false;
        $scope.titles = [
            { name: 'Pilih Titel', value: '' },
            { name: 'Tn.', value: 'Mister' },
            { name: 'Ny.', value: 'Mistress' },
            { name: 'Nn.', value: 'Miss' }
        ];
       
        $scope.language = langCode;
        $interval(function () {
            var nowTime = new Date();
            if (nowTime > $scope.expiryDate) {
                $scope.expired = true;
            }
        }, 1000);
        //Serving easier Data
        $scope.capitalizeFirstLetter = function (sentence) {
            var words = sentence.split(" ");
            var text = "";
            for (var i = 0; i < words.length; i++) {
                text += words[i].substring(0, 1) + words[i].substring(1, words[i].length).toLowerCase() + " ";
            }
            return text;
        }

        $scope.totalpax = function () {
            var sum = 0;
            for (var i = 0; i < $scope.hotelDetail.room.length; i++) {
                for (var j = 0; j < $scope.hotelDetail.room[i].rates.length; j++) {
                    sum += ($scope.hotelDetail.room[i].rates[j].adultCount + $scope.hotelDetail.room[i].rates[j].childCount) *
                        $scope.hotelDetail.room[i].rates[j].roomCount;
                }
            }
            return sum;
        }

        $scope.hotelstar = function () {
            if ($scope.hotelDetail.starRating == 1) {
                return 'star';
            }
            if ($scope.hotelDetail.starRating == 2) {
                return 'star star-2';
            }
            if ($scope.hotelDetail.starRating == 3) {
                return 'star star-3';
            }
            if ($scope.hotelDetail.starRating == 4) {
                return 'star star-4';
            }
            if ($scope.hotelDetail.starRating == 5) {
                return 'star star-5';
            }
        }

        //Check data 
        $scope.CheckTitle = function (passenger) {
            var valid = true;

            if (passenger.title == $scope.titles[0].value || passenger.title == "") {
                valid = false;
            }

            return valid;
        }

        $scope.checkName = function (name) {
            var re = /^[a-zA-Z ]+$/;
            var x = $scope.buyerInfo.name;
            if (name == null || name.match(re)) {
                return true;
            } else {
                return false;
            }
        }

        $scope.changeTitle = function (title) {
            if (title == 'Mister')
                return 'Tn.';
            else if (title == 'Mistress')
                return 'Ny.';
            else if (title == 'Miss')
                return 'Nn.';
        }

        $scope.checkNameComplete = function (name) {
            if (name == null) {
                return false;
            } else {
                return name.length != 0;
            }
        }

        $scope.checkEmail = function (email) {
            if (email == null || email == "") {
                return false;
            } else {
                return true;
            }
        }

        $scope.checkPhone = function (phone) {
            if ($scope.buyerInfo.countryCode == 'xx') {
                return false;
            } else if ($scope.buyerInfo.countryCode != 'xx' && (phone == null || phone == "")) {
                return false;
            } else {
                return true;
            }
        }

        $scope.$watch('buyerInfo.name', function () {
            if ($scope.diffPerson == false) {
                if ($scope.buyerInfo.name != null && $scope.buyerInfo.title != "") {
                    $scope.guestInfo.name = $scope.buyerInfo.name;
                    $scope.guestInfo.title = $scope.buyerInfo.title;
                }              
            }
        });

        $scope.$watch('buyerInfo.title', function () {
            if ($scope.diffPerson == false) {
                if ($scope.buyerInfo.name != null && $scope.buyerInfo.title != "") {
                    $scope.guestInfo.name = $scope.buyerInfo.name;
                    $scope.guestInfo.title = $scope.buyerInfo.title;
                }
            }
        });

        $scope.cleanGuestInfo = function () {
            if ($scope.diffPerson) {
                $scope.guestInfo.name = '';
            } else {
                $scope.guestInfo.name = $scope.buyerInfo.name;
                $scope.guestInfo.title = $scope.buyerInfo.title;
            }
        }

        $scope.form = {
            incompleteContactTitle: false,
            incompleteContactName: false,
            incompleteContactPhone: false,
            incompleteContactEmail: false,
            incompleteGuestTitle: false,
            incompleteGuestName: false,
        }

        $scope.validateForm = function (page) {
            if (page == 1) {
                if (!$scope.CheckTitle($scope.buyerInfo.title)) {
                    $scope.form.incompleteContactTitle = true;
                } else {
                    $scope.form.incompleteContactTitle = false;
                }

                if (!$scope.checkNameComplete($scope.buyerInfo.name)) {
                    $scope.form.incompleteContactName = true;
                } else {
                    $scope.form.incompleteContactName = false;
                }


                if (!$scope.checkPhone($scope.buyerInfo.phone)) {
                    $scope.form.incompleteContactPhone = true;
                } else {
                    $scope.form.incompleteContactPhone = false;
                }

                if (!$scope.checkEmail($scope.buyerInfo.email)) {
                    $scope.form.incompleteContactEmail = true;
                } else {
                    $scope.form.incompleteContactEmail = false;
                }

                if (!$scope.CheckTitle($scope.guestInfo.title)) {
                    $scope.form.incompleGuestTitle = true;
                } else {
                    $scope.form.incompleteGuestTitle = false;
                }

                if (!$scope.checkNameComplete($scope.guestInfo.name)) {
                    $scope.form.incompleteGuestName = true;
                } else {
                    $scope.form.incompleteGuestName = false;
                }

                if (!$scope.form.incompleteContactTitle && !$scope.form.incompleteContactName
                    && !$scope.form.incompleteContactPhone && !$scope.form.incompleteContactEmail
                    && !$scope.form.incompleteGuestTitle && !$scope.form.incompleteGuestName) {
                    $scope.changePage(2);
                }

            }
        }
       
        //Change Page
       
        $scope.changePage = function (page) {
            $location.hash("page-" + page);
            // change current page variable
            $scope.currentPage = page;
            // change step class
            $scope.stepClass = 'active-' + page;

        }
        // change page after login
        $scope.form.submit = function () {
            if ($scope.trial > 3) {
                $scope.trial = 0;
            }
            $scope.form.submitting = true;
            var authAccess = getAuthAccess();
            if (authAccess == 1 || authAccess == 2) {
                $http({
                    method: 'POST',
                    url: LoginConfig.Url,
                    data: {
                        userName: $scope.form.email,
                        password: $scope.form.password,
                        clientId: 'V2toa2VrOXFSWFZOUXpSM1QycEZlRTlIVlhwYWFrVjVUVVJrYlZsVVp6Vk5WRlp0VGtSR2FrOUhSWGhhYWsweFRucGpNRTE2U1RCT2VtTjNXbTFKZDFwcVFUMD0=',
                        clientSecret: 'V2tkS2FFOUVhek5QUjFsNFRucFpNVmt5UlRST2JVWnNXVmRKTTA1dFVtaFBSMDVyV1dwQk5WcEhTWGxPZWtwcVRVUkpNVTFCUFQwPQ=='
                    },
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).then(function (returnData) {
                    $scope.form.submitting = false;
                    $scope.form.submitted = true;
                    if (returnData.data.status == 200) {
                        setCookie("accesstoken", returnData.data.accessToken, returnData.data.expTime);
                        setCookie("refreshtoken", returnData.data.refreshToken, returnData.data.expTime);
                        setCookie("authkey", returnData.data.accessToken, returnData.data.expTime);
                        $scope.TakeProfileConfig.TakeProfile();
                        $scope.loggedIn = true;
                    }
                    else {
                        window.location.href = $location.absUrl();
                        $scope.form.submitting = false;
                        $scope.form.submitted = false;
                        $scope.form.isLogin = false;
                        //Return langsung
                    }
                }).catch(function (returnData) {
                    $scope.trial++;
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.form.submit();
                    }
                    else {
                        console.log('Failed to Login');
                        $scope.form.submitting = false;
                        $scope.form.submitted = false;
                        $scope.form.isLogin = false;
                        window.location.href = $location.absUrl();
                        //return langsung
                    }

                });
            }
            else {
                $scope.form.submitting = false;
                $scope.form.submitted = false;
                $scope.form.isLogin = false;
            }

        }

        $scope.TakeProfileConfig = {
            TakeProfile: function () {
                if ($scope.trial > 3) {
                    $scope.trial = 0;
                }
                //Check Authorization
                var authAccess = getAuthAccess();
                if (authAccess == 2) {
                    $http({
                        method: 'GET',
                        url: GetProfileConfig.Url,
                        headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                    }).then(function (returnData) {
                        if (returnData.status == "200") {
                            $scope.buyerInfo.name = returnData.data.name;
                            $scope.buyerInfo.countryCode = parseInt(returnData.data.countryCallCd);
                            $scope.buyerInfo.phone = parseInt(returnData.data.phone);
                            $scope.buyerInfo.email = returnData.data.email;
                            window.location.href = $location.absUrl();
                        }
                        else {
                            $scope.buyerInfo = {};
                            console.log('There is an error');
                            console.log('Error : ' + returnData.data.error);
                            console.log(returnData);
                            window.location.href = $location.absUrl();
                        }
                    }).catch(function (returnData) {
                        $scope.trial++;
                        if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                        {
                            $scope.TakeProfileConfig.TakeProfile();
                        }
                        else {
                            $scope.buyerInfo = {};
                            console.log('Failed to Get Profile');
                            window.location.href = $location.absUrl();
                        }

                    });
                }
                else {
                    console.log('Not Authorized');
                }

            }
        }
        $scope.changePage(currentPage);

        $scope.toggleLogin = function () {
            if ($scope.loginShown == false) {
                $scope.loginShown = true;
            } else {
                $scope.loginShown = false;
            }
        }

        $scope.book = {
            booking: false,
            url: HotelBookConfig.Url,
            postData: '',
            checked: false,
            newPrice: '',
            rsvNo: '',
            isSuccess: false,
            isPriceChanged: false,

            send: function () {
                if ($scope.trial > 3) {
                    $scope.trial = 0;
                }
                $scope.book.booking = true;
                $scope.book.isPriceChanged = false;
                $scope.book.checked = false;
                $scope.contactData = '{' + ' "title":"' + $scope.buyerInfo.title + '",  "name" :"' + $scope.buyerInfo.name + '","countryCallCd":"' + $scope.buyerInfo.countryCode + '","phone":"' + $scope.buyerInfo.phone + '","email":"' + $scope.buyerInfo.email + '"' + '}';
                $scope.paxData = ' "pax" : [ ';

                // generate data
                $scope.book.postData = ' "token":"' + $scope.token + '",  "contact" :' + $scope.contactData + ',"lang":"' + $scope.language + '"';
                $scope.paxData = $scope.paxData + '{ "type":"1", "title":"' + $scope.guestInfo.title + '" , "name":"' + $scope.guestInfo.name + '" }]';
                $scope.specialReq = '"specialRequest":"' + $scope.buyerInfo.message + '"';
                $scope.book.postData = '{' + $scope.book.postData + ',' + $scope.paxData + ',' + $scope.specialReq + '}';
                console.log($scope.book.postData);
                $scope.book.postData = JSON.parse($scope.book.postData);

                console.log($scope.book.postData);

                //Check Authorization
                var authAccess = getAuthAccess();
                if (authAccess == 1 || authAccess == 2) {
                    // send form
                    $http({
                        method: 'POST',
                        url: $scope.book.url,
                        data: $scope.book.postData,
                        headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                    }).then(function (returnData) {
                        //console.log(returnData);
                        if (returnData.data.status == '200') {
                            if (returnData.data.newPrice != null) {
                                $scope.book.isPriceChanged = true;
                                $scope.book.isSuccess = true;
                                $scope.book.newPrice = returnData.data.newPrice;
                                $scope.book.checked = false;
                                $scope.book.booking = false;
                            }
                            else {
                                $scope.book.isSuccess = true;
                                $scope.book.rsvNo = returnData.data.rsvNo;
                                $('form#rsvno input#rsvno-input').val(returnData.data.rsvNo);
                                $('form#rsvno').submit();
                                $scope.book.checked = true;
                                $scope.book.booking = false;
                            }

                        } else {
                            if (returnData.data.error == "ERHBOO01") {
                                //This is Invalid Data
                                $scope.book.isSuccess = false;
                                $scope.book.checked = true;
                                $scope.book.booking = false;
                                console.log(returnData);
                                $scope.errorMessage = 'Data yang Anda masukkan tidak lengkap. Silakan ulangi pemesanan.';
                            }
                            else if (returnData.data.error == "ERHBOO02") {
                                //Expired
                                $scope.book.isSuccess = false;
                                $scope.book.checked = true;
                                $scope.book.booking = false;
                                console.log(returnData);
                                $scope.errorMessage = 'Waktu checkout sudah habis. Silakan ulangi pencarian.';
                            } else {
                                // if (returnData.data.error == "ERHBOO03")
                                //Any IsValid = false in response
                                $scope.book.isSuccess = false;
                                $scope.book.checked = true;
                                $scope.book.booking = false;
                                console.log(returnData);
                                $scope.errorMessage = 'Mohon maaf, pemesanan hotel tidak dapat dilanjutkan karena hotel sudah penuh.';
                                
                            }
                            console.log($scope.errorMessage);
                        }

                    }).catch(function (returnData) {
                        $scope.trial++;
                        if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                        {
                            $scope.book.send();
                        }
                        else {
                            console.log(returnData);
                            $scope.book.checked = true;
                            $scope.book.isSuccess = false;
                        }

                    });
                }
                else {
                    console.log('Not Authorized');
                    $scope.book.checked = true;
                    $scope.book.isSuccess = false;
                }

            }
        };
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

        angular.element(document).ready(function () {
            $scope.rsvNo = window.location.search.toString().split('=')[1];
        });
        $scope.returnUrl = window.location.origin;
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
       
        $scope.refresh = function () {
            setTimeout(function () {
                //
                //Check Authorization
                var authAccess = getAuthAccess();
                if (authAccess == 1 || authAccess == 2) {
                    // send form
                    $http({
                        method: 'GET',
                        url: GetReservationConfig.Url + $scope.rsvNo,
                        headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                    }).
                        then(function (returnData) {
                            //console.log(returnData);
                            if (returnData.data.status != "500") {
                                if (returnData.data.flight.payment.status != '2') {
                                    window.location.reload(1);
                                    //window.location.replace("https://local.travorama.com/id/Flight/Thankyou?rsvNo=160226537584#/%23page-3");
                                    console.log(returnData.data.flight.payment.status);
                                }
                                else {
                                    $scope.refresh();
                                }
                            }
                            else {
                                $scope.refresh();
                            }

                        }).catch(function () {
                            $scope.refresh();
                        });
                }
                else {
                    console.log('Not Authorized');
                }
            }, 15000);
        }
    }
]);// confirmation controller