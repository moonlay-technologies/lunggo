// travorama angular app - checkout controller
app.controller('checkoutController', [
    '$http', '$scope', '$interval', '$location', '$log', function ($http, $scope, $interval, $location, $log) {

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
        $scope.trial = 0;
        $scope.currentPage = 1;
        $scope.pageLoaded = true;
        $scope.loginShown = false;
        $scope.checkoutForm = {
            loading: false
        };
        $scope.stepClass = '';
        $scope.titles = [
            {name: 'Pilih Titel', value:''},
            { name: 'Tn.', value: 'Mister' },
            { name: 'Ny.', value: 'Mistress' },
            { name: 'Nn.', value: 'Miss' }
        ];

        $scope.correctName = true;
        $scope.adultCount = adultPassenger;
        $scope.adultFare = adultFare;
        $scope.adultNetFare = adultNetFare;
        $scope.childCount = childPassenger;
        $scope.childFare = childFare;
        $scope.childNetFare = childNetFare;
        $scope.infantCount = infantPassenger;
        $scope.infantFare = infantFare;
        $scope.infantNetFare = infantNetFare;
        $scope.totalAdultFare = totalAdultFare;
        $scope.totalChildFare = totalChildFare;
        $scope.totalInfantFare = totalInfantFare;
        $scope.language = langCode;
        $scope.token = token;
        $scope.trips = trips;
        $scope.initialPrice = price;
        $scope.originalPrice = originalPrice;
        $scope.totalPrice = price;
        $scope.passportRequired = passportRequired;
        $scope.issueDatePassportRequired = issueDatePassportRequired;
        $scope.idRequired = idRequired;
        $scope.nationalityRequired = nationalityRequired;
        $scope.birthDateRequired = birthDateRequired;

        $scope.expired = false;
        $scope.expiryDate = new Date(expiryDate);
        $interval(function () {
            var nowTime = new Date();
            if (nowTime > $scope.expiryDate) {
                $scope.expired = true;
            }
        }, 1000);
        $scope.monthNames = ["Januari", "Februari", "Maret", "April", "Mei", "Juni", "Juli", "Agustus", "September", "Oktober", "November", "Desember"];
        $scope.months = [
            { value: 0, name: 'Bulan' },
            { value: 1, name: 'Januari' },
            { value: 2, name: 'Februari' },
            { value: 3, name: 'Maret' },
            { value: 4, name: 'April' },
            { value: 5, name: 'Mei' },
            { value: 6, name: 'Juni' },
            { value: 7, name: 'Juli' },
            { value: 8, name: 'Agustus' },
            { value: 9, name: 'September' },
            { value: 10, name: 'Oktober' },
            { value: 11, name: 'November' },
            { value: 12, name: 'Desember' }
        ];

        $scope.form = {
            email: '',
            password: '',
            submitting: false,
            isLogin: false,
            submitted: false
        };

        $scope.passengers = [];
        $scope.passengersForm = {
            valid: false,
            checked: false
        };

        $scope.flightDetail = {};
        $scope.flightDetail.departureFullDate = new Date(departureDate);
        $scope.flightDetail.passportFullDate = new Date(passportDate);
        $scope.flightDetail.passportCreatedDate = new Date(departureDate);

        $scope.bookingDate = new Date();
        $scope.countries = [{ name: 'Pilih Negara', dial_code: 'xx', code: '' }, { name: 'Afghanistan', dial_code: '93', code: 'AF' }, { name: 'Aland Islands', dial_code: '358', code: 'AX' }, { name: 'Albania', dial_code: '355', code: 'AL' }, { name: 'Algeria', dial_code: '213', code: 'DZ' }, { name: 'AmericanSamoa', dial_code: '1 684', code: 'AS' }, { name: 'Andorra', dial_code: '376', code: 'AD' }, { name: 'Angola', dial_code: '244', code: 'AO' }, { name: 'Anguilla', dial_code: '1 264', code: 'AI' }, { name: 'Antarctica', dial_code: '672', code: 'AQ' }, { name: 'Antigua and Barbuda', dial_code: '1268', code: 'AG' }, { name: 'Argentina', dial_code: '54', code: 'AR' }, { name: 'Armenia', dial_code: '374', code: 'AM' }, { name: 'Aruba', dial_code: '297', code: 'AW' }, { name: 'Australia', dial_code: '61', code: 'AU' }, { name: 'Austria', dial_code: '43', code: 'AT' }, { name: 'Azerbaijan', dial_code: '994', code: 'AZ' }, { name: 'Bahamas', dial_code: '1 242', code: 'BS' }, { name: 'Bahrain', dial_code: '973', code: 'BH' }, { name: 'Bangladesh', dial_code: '880', code: 'BD' }, { name: 'Barbados', dial_code: '1 246', code: 'BB' }, { name: 'Belarus', dial_code: '375', code: 'BY' }, { name: 'Belgium', dial_code: '32', code: 'BE' }, { name: 'Belize', dial_code: '501', code: 'BZ' }, { name: 'Benin', dial_code: '229', code: 'BJ' }, { name: 'Bermuda', dial_code: '1 441', code: 'BM' }, { name: 'Bhutan', dial_code: '975', code: 'BT' }, { name: 'Bolivia, Plurinational State of', dial_code: '591', code: 'BO' }, { name: 'Bosnia and Herzegovina', dial_code: '387', code: 'BA' }, { name: 'Botswana', dial_code: '267', code: 'BW' }, { name: 'Brazil', dial_code: '55', code: 'BR' }, { name: 'British Indian Ocean Territory', dial_code: '246', code: 'IO' }, { name: 'Brunei Darussalam', dial_code: '673', code: 'BN' }, { name: 'Bulgaria', dial_code: '359', code: 'BG' }, { name: 'Burkina Faso', dial_code: '226', code: 'BF' }, { name: 'Burundi', dial_code: '257', code: 'BI' }, { name: 'Cambodia', dial_code: '855', code: 'KH' }, { name: 'Cameroon', dial_code: '237', code: 'CM' }, { name: 'Canada', dial_code: '1', code: 'CA' }, { name: 'Cape Verde', dial_code: '238', code: 'CV' }, { name: 'Cayman Islands', dial_code: ' 345', code: 'KY' }, { name: 'Central African Republic', dial_code: '236', code: 'CF' }, { name: 'Chad', dial_code: '235', code: 'TD' }, { name: 'Chile', dial_code: '56', code: 'CL' }, { name: 'China', dial_code: '86', code: 'CN' }, { name: 'Christmas Island', dial_code: '61', code: 'CX' }, { name: 'Cocos (Keeling) Islands', dial_code: '61', code: 'CC' }, { name: 'Colombia', dial_code: '57', code: 'CO' }, { name: 'Comoros', dial_code: '269', code: 'KM' }, { name: 'Congo', dial_code: '242', code: 'CG' }, { name: 'Congo, The Democratic Republic of the Congo', dial_code: '243', code: 'CD' }, { name: 'Cook Islands', dial_code: '682', code: 'CK' }, { name: 'Costa Rica', dial_code: '506', code: 'CR' }, { name: "Cote d'Ivoire", dial_code: '225', code: 'CI' }, { name: 'Croatia', dial_code: '385', code: 'HR' }, { name: 'Cuba', dial_code: '53', code: 'CU' }, { name: 'Cyprus', dial_code: '357', code: 'CY' }, { name: 'Czech Republic', dial_code: '420', code: 'CZ' }, { name: 'Denmark', dial_code: '45', code: 'DK' }, { name: 'Djibouti', dial_code: '253', code: 'DJ' }, { name: 'Dominica', dial_code: '1 767', code: 'DM' }, { name: 'Dominican Republic', dial_code: '1 849', code: 'DO' }, { name: 'Ecuador', dial_code: '593', code: 'EC' }, { name: 'Egypt', dial_code: '20', code: 'EG' }, { name: 'El Salvador', dial_code: '503', code: 'SV' }, { name: 'Equatorial Guinea', dial_code: '240', code: 'GQ' }, { name: 'Eritrea', dial_code: '291', code: 'ER' }, { name: 'Estonia', dial_code: '372', code: 'EE' }, { name: 'Ethiopia', dial_code: '251', code: 'ET' }, { name: 'Falkland Islands (Malvinas)', dial_code: '500', code: 'FK' }, { name: 'Faroe Islands', dial_code: '298', code: 'FO' }, { name: 'Fiji', dial_code: '679', code: 'FJ' }, { name: 'Finland', dial_code: '358', code: 'FI' }, { name: 'France', dial_code: '33', code: 'FR' }, { name: 'French Guiana', dial_code: '594', code: 'GF' }, { name: 'French Polynesia', dial_code: '689', code: 'PF' }, { name: 'Gabon', dial_code: '241', code: 'GA' }, { name: 'Gambia', dial_code: '220', code: 'GM' }, { name: 'Georgia', dial_code: '995', code: 'GE' }, { name: 'Germany', dial_code: '49', code: 'DE' }, { name: 'Ghana', dial_code: '233', code: 'GH' }, { name: 'Gibraltar', dial_code: '350', code: 'GI' }, { name: 'Greece', dial_code: '30', code: 'GR' }, { name: 'Greenland', dial_code: '299', code: 'GL' }, { name: 'Grenada', dial_code: '1 473', code: 'GD' }, { name: 'Guadeloupe', dial_code: '590', code: 'GP' }, { name: 'Guam', dial_code: '1 671', code: 'GU' }, { name: 'Guatemala', dial_code: '502', code: 'GT' }, { name: 'Guernsey', dial_code: '44', code: 'GG' }, { name: 'Guinea', dial_code: '224', code: 'GN' }, { name: 'Guinea-Bissau', dial_code: '245', code: 'GW' }, { name: 'Guyana', dial_code: '595', code: 'GY' }, { name: 'Haiti', dial_code: '509', code: 'HT' }, { name: 'Holy See (Vatican City State)', dial_code: '379', code: 'VA' }, { name: 'Honduras', dial_code: '504', code: 'HN' }, { name: 'Hong Kong', dial_code: '852', code: 'HK' }, { name: 'Hungary', dial_code: '36', code: 'HU' }, { name: 'Iceland', dial_code: '354', code: 'IS' }, { name: 'India', dial_code: '91', code: 'IN' }, { name: 'Indonesia', dial_code: '62', code: 'ID' }, { name: 'Iran, Islamic Republic of Persian Gulf', dial_code: '98', code: 'IR' }, { name: 'Iraq', dial_code: '964', code: 'IQ' }, { name: 'Ireland', dial_code: '353', code: 'IE' }, { name: 'Isle of Man', dial_code: '44', code: 'IM' }, { name: 'Israel', dial_code: '972', code: 'IL' }, { name: 'Italy', dial_code: '39', code: 'IT' }, { name: 'Jamaica', dial_code: '1 876', code: 'JM' }, { name: 'Japan', dial_code: '81', code: 'JP' }, { name: 'Jersey', dial_code: '44', code: 'JE' }, { name: 'Jordan', dial_code: '962', code: 'JO' }, { name: 'Kazakhstan', dial_code: '7 7', code: 'KZ' }, { name: 'Kenya', dial_code: '254', code: 'KE' }, { name: 'Kiribati', dial_code: '686', code: 'KI' }, { name: "Korea, Democratic People's Republic of Korea", dial_code: '850', code: 'KP' }, { name: 'Korea, Republic of South Korea', dial_code: '82', code: 'KR' }, { name: 'Kuwait', dial_code: '965', code: 'KW' }, { name: 'Kyrgyzstan', dial_code: '996', code: 'KG' }, { name: 'Laos', dial_code: '856', code: 'LA' }, { name: 'Latvia', dial_code: '371', code: 'LV' }, { name: 'Lebanon', dial_code: '961', code: 'LB' }, { name: 'Lesotho', dial_code: '266', code: 'LS' }, { name: 'Liberia', dial_code: '231', code: 'LR' }, { name: 'Libyan Arab Jamahiriya', dial_code: '218', code: 'LY' }, { name: 'Liechtenstein', dial_code: '423', code: 'LI' }, { name: 'Lithuania', dial_code: '370', code: 'LT' }, { name: 'Luxembourg', dial_code: '352', code: 'LU' }, { name: 'Macao', dial_code: '853', code: 'MO' }, { name: 'Macedonia', dial_code: '389', code: 'MK' }, { name: 'Madagascar', dial_code: '261', code: 'MG' }, { name: 'Malawi', dial_code: '265', code: 'MW' }, { name: 'Malaysia', dial_code: '60', code: 'MY' }, { name: 'Maldives', dial_code: '960', code: 'MV' }, { name: 'Mali', dial_code: '223', code: 'ML' }, { name: 'Malta', dial_code: '356', code: 'MT' }, { name: 'Marshall Islands', dial_code: '692', code: 'MH' }, { name: 'Martinique', dial_code: '596', code: 'MQ' }, { name: 'Mauritania', dial_code: '222', code: 'MR' }, { name: 'Mauritius', dial_code: '230', code: 'MU' }, { name: 'Mayotte', dial_code: '262', code: 'YT' }, { name: 'Mexico', dial_code: '52', code: 'MX' }, { name: 'Micronesia, Federated States of Micronesia', dial_code: '691', code: 'FM' }, { name: 'Moldova', dial_code: '373', code: 'MD' }, { name: 'Monaco', dial_code: '377', code: 'MC' }, { name: 'Mongolia', dial_code: '976', code: 'MN' }, { name: 'Montenegro', dial_code: '382', code: 'ME' }, { name: 'Montserrat', dial_code: '1664', code: 'MS' }, { name: 'Morocco', dial_code: '212', code: 'MA' }, { name: 'Mozambique', dial_code: '258', code: 'MZ' }, { name: 'Myanmar', dial_code: '95', code: 'MM' }, { name: 'Namibia', dial_code: '264', code: 'NA' }, { name: 'Nauru', dial_code: '674', code: 'NR' }, { name: 'Nepal', dial_code: '977', code: 'NP' }, { name: 'Netherlands', dial_code: '31', code: 'NL' }, { name: 'Netherlands Antilles', dial_code: '599', code: 'AN' }, { name: 'New Caledonia', dial_code: '687', code: 'NC' }, { name: 'New Zealand', dial_code: '64', code: 'NZ' }, { name: 'Nicaragua', dial_code: '505', code: 'NI' }, { name: 'Niger', dial_code: '227', code: 'NE' }, { name: 'Nigeria', dial_code: '234', code: 'NG' }, { name: 'Niue', dial_code: '683', code: 'NU' }, { name: 'Norfolk Island', dial_code: '672', code: 'NF' }, { name: 'Northern Mariana Islands', dial_code: '1 670', code: 'MP' }, { name: 'Norway', dial_code: '47', code: 'NO' }, { name: 'Oman', dial_code: '968', code: 'OM' }, { name: 'Pakistan', dial_code: '92', code: 'PK' }, { name: 'Palau', dial_code: '680', code: 'PW' }, { name: 'Palestinian Territory, Occupied', dial_code: '970', code: 'PS' }, { name: 'Panama', dial_code: '507', code: 'PA' }, { name: 'Papua New Guinea', dial_code: '675', code: 'PG' }, { name: 'Paraguay', dial_code: '595', code: 'PY' }, { name: 'Peru', dial_code: '51', code: 'PE' }, { name: 'Philippines', dial_code: '63', code: 'PH' }, { name: 'Pitcairn', dial_code: '872', code: 'PN' }, { name: 'Poland', dial_code: '48', code: 'PL' }, { name: 'Portugal', dial_code: '351', code: 'PT' }, { name: 'Puerto Rico', dial_code: '1 939', code: 'PR' }, { name: 'Qatar', dial_code: '974', code: 'QA' }, { name: 'Romania', dial_code: '40', code: 'RO' }, { name: 'Russia', dial_code: '7', code: 'RU' }, { name: 'Rwanda', dial_code: '250', code: 'RW' }, { name: 'Reunion', dial_code: '262', code: 'RE' }, { name: 'Saint Barthelemy', dial_code: '590', code: 'BL' }, { name: 'Saint Helena, Ascension and Tristan Da Cunha', dial_code: '290', code: 'SH' }, { name: 'Saint Kitts and Nevis', dial_code: '1 869', code: 'KN' }, { name: 'Saint Lucia', dial_code: '1 758', code: 'LC' }, { name: 'Saint Martin', dial_code: '590', code: 'MF' }, { name: 'Saint Pierre and Miquelon', dial_code: '508', code: 'PM' }, { name: 'Saint Vincent and the Grenadines', dial_code: '1 784', code: 'VC' }, { name: 'Samoa', dial_code: '685', code: 'WS' }, { name: 'San Marino', dial_code: '378', code: 'SM' }, { name: 'Sao Tome and Principe', dial_code: '239', code: 'ST' }, { name: 'Saudi Arabia', dial_code: '966', code: 'SA' }, { name: 'Senegal', dial_code: '221', code: 'SN' }, { name: 'Serbia', dial_code: '381', code: 'RS' }, { name: 'Seychelles', dial_code: '248', code: 'SC' }, { name: 'Sierra Leone', dial_code: '232', code: 'SL' }, { name: 'Singapore', dial_code: '65', code: 'SG' }, { name: 'Slovakia', dial_code: '421', code: 'SK' }, { name: 'Slovenia', dial_code: '386', code: 'SI' }, { name: 'Solomon Islands', dial_code: '677', code: 'SB' }, { name: 'Somalia', dial_code: '252', code: 'SO' }, { name: 'South Africa', dial_code: '27', code: 'ZA' }, { name: 'South Georgia and the South Sandwich Islands', dial_code: '500', code: 'GS' }, { name: 'Spain', dial_code: '34', code: 'ES' }, { name: 'Sri Lanka', dial_code: '94', code: 'LK' }, { name: 'Sudan', dial_code: '249', code: 'SD' }, { name: 'Suriname', dial_code: '597', code: 'SR' }, { name: 'Svalbard and Jan Mayen', dial_code: '47', code: 'SJ' }, { name: 'Swaziland', dial_code: '268', code: 'SZ' }, { name: 'Sweden', dial_code: '46', code: 'SE' }, { name: 'Switzerland', dial_code: '41', code: 'CH' }, { name: 'Syrian Arab Republic', dial_code: '963', code: 'SY' }, { name: 'Taiwan', dial_code: '886', code: 'TW' }, { name: 'Tajikistan', dial_code: '992', code: 'TJ' }, { name: 'Tanzania, United Republic of Tanzania', dial_code: '255', code: 'TZ' }, { name: 'Thailand', dial_code: '66', code: 'TH' }, { name: 'Timor-Leste', dial_code: '670', code: 'TL' }, { name: 'Togo', dial_code: '228', code: 'TG' }, { name: 'Tokelau', dial_code: '690', code: 'TK' }, { name: 'Tonga', dial_code: '676', code: 'TO' }, { name: 'Trinidad and Tobago', dial_code: '1 868', code: 'TT' }, { name: 'Tunisia', dial_code: '216', code: 'TN' }, { name: 'Turkey', dial_code: '90', code: 'TR' }, { name: 'Turkmenistan', dial_code: '993', code: 'TM' }, { name: 'Turks and Caicos Islands', dial_code: '1 649', code: 'TC' }, { name: 'Tuvalu', dial_code: '688', code: 'TV' }, { name: 'Uganda', dial_code: '256', code: 'UG' }, { name: 'Ukraine', dial_code: '380', code: 'UA' }, { name: 'United Arab Emirates', dial_code: '971', code: 'AE' }, { name: 'United Kingdom', dial_code: '44', code: 'GB' }, { name: 'United States', dial_code: '1', code: 'US' }, { name: 'Uruguay', dial_code: '598', code: 'UY' }, { name: 'Uzbekistan', dial_code: '998', code: 'UZ' }, { name: 'Vanuatu', dial_code: '678', code: 'VU' }, { name: 'Venezuela, Bolivarian Republic of Venezuela', dial_code: '58', code: 'VE' }, { name: 'Vietnam', dial_code: '84', code: 'VN' }, { name: 'Virgin Islands, British', dial_code: '1 284', code: 'VG' }, { name: 'Virgin Islands, U.S.', dial_code: '1 340', code: 'VI' }, { name: 'Wallis and Futuna', dial_code: '681', code: 'WF' }, { name: 'Yemen', dial_code: '967', code: 'YE' }, { name: 'Zambia', dial_code: '260', code: 'ZM' }, { name: 'Zimbabwe', dial_code: '263', code: 'ZW' }];
        $scope.displayCountry = function (code) {
            for (var i = 0; i < $scope.countries.length; i++) {
                if ($scope.countries[i].code === code) {
                    return $scope.countries[i].name;
                }
            }
        };

        $scope.form = {
            incompleteContactTitle: false,
            incompleteContactName: false,
            incompleteContactPhone: false,
            incompleteContactEmail: false,
            hasDuplicatePaxName: false,
            incompleteTitles: false,
            incompleteBirthDates: false,
            incompletePassportData: false,
            checkNameIncomplete: false
        }

        $(".phone").autocomplete({
            source: $scope.countries,
        });

        //************ Functions for Validations and Warning Message **********
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

        $scope.checkName = function (name) {
            var re = /^[a-zA-Z ]+$/;
            if (name == null || name.match(re)) {
                return true;
            } else {
                return false;
            }
        }

        $scope.CheckTitle = function(passengers) {
            var valid = true;
            for(var x = 0; x < passengers.length; x++)
            {
                if (passengers[x].title == $scope.titles[0].value || passengers[x].title == "" ) {
                    valid = false;
                }
            }
            return valid;
        }

        $scope.CheckDate = function (passengers) {
            var valid = true;
             for (var x = 0; x < passengers.length; x++) {
                if (passengers[x].birth.date === $scope.dates($scope.flightDetail.departureFullDate.getMonth(), $scope.flightDetail.departureFullDate.getYear)[0]
                   || passengers[x].birth.month === $scope.months[0].value.toString() || passengers[x].birth.year == $scope.generateYear(passengers[x].type)[0]) 
                {
                    valid = false;
                }
                else if (passengers[x].type != 'adult') {
                    if ((passengers[x].birth.date == null || passengers[x].birth.month == null || passengers[x].birth.year == null)
                        || $scope.invalidDate(passengers[x].birth.date, passengers[x].birth.month, passengers[x].birth.year)) {
                        valid = false;
                    }
                }
                else if (passengers[x].type == 'adult') {
                    if (($scope.birthDateRequired && (passengers[x].birth.date == null || passengers[x].birth.month == null || passengers[x].birth.year == null))
                        || $scope.invalidDate(passengers[x].birth.date, passengers[x].birth.month, passengers[x].birth.year)) {
                        valid = false;
                    }
                }
            }
            return valid;
        }

        $scope.CheckOnlyAdult = function() {
            var valid = true;
            for (var x = 0; x < $scope.passengers.length; x++) {
                if ($scope.passengers[x].type != 'adult') {
                    valid = false;
                }               
            }
            return valid;
        }

        $scope.CheckPassportDate = function (passengers) {
            var valid = true;
            for (var x = 0; x < passengers.length; x++) {
                if (passengers[x].passport.expire.date === $scope.dates($scope.flightDetail.departureFullDate.getMonth(), $scope.flightDetail.departureFullDate.getYear())[0]
                   || passengers[x].passport.expire.month === $scope.months[0].value.toString() ||
                    passengers[x].passport.expire.year == $scope.generateYear(passengers[x].type)[0] ||
                    $scope.invalidDate(passengers[x].passport.expire.date, passengers[x].passport.expire.month, passengers[x].passport.expire.year) ||
                    $scope.invalidPassportExpiry(passengers[x].passport.expire.date, passengers[x].passport.expire.month, passengers[x].passport.expire.year)) {
                    valid = false;
                }
                else if (passengers[x].passport.expire.date == null || passengers[x].passport.expire.month == null
                    || passengers[x].passport.expire.year == null || $scope.invalidDate(passengers[x].passport.expire.date, passengers[x].passport.expire.month, passengers[x].passport.expire.year)) {
                    valid = false;
                } 
            }
            return valid;
        }

        $scope.checkNameComplete = function(passengers) {
            var valid = true;
            for (var x = 0; x < passengers.length; x++) {
                if (passengers[x].name == null || passengers[x].name == "") {
                    valid = false;
                }
            }
            return valid;
        }

        $scope.hasDuplicatePaxName = function () {
            var namesSoFar = [];
            for (var i = 0; i < $scope.passengers.length; ++i) {
                var value = $scope.passengers[i].name;
                if (value == null) {
                    return false;
                }
                if (value.length != 0) {
                    if (namesSoFar.indexOf(value.toLowerCase()) > -1) {
                        return true;
                    } else {
                        //Not in the array
                        namesSoFar.push(value.toLowerCase());
                    }
                }
            }
            return false;
        }

        $scope.passportNumberFilled = function(paxes) {
            var valid = true;
            for (var i = 0; i < paxes.length; ++i) {
                var value = paxes[i].passport.number;
                if (value == null) {
                    valid = false;
                } else {
                    if (value.length == 0) {
                        valid = false;
                    }
                }
            }
            return valid;
        }

        $scope.checkEmail = function(email) {
            if (email == null || email == "") {
                return false;
            } else {
                return true;
            }
        }

        $scope.checkPhone = function(phone) {
            if ($scope.buyerInfo.countryCode == 'xx') {
                return false;
            } else if ($scope.buyerInfo.countryCode != 'xx' && (phone == null || phone == "")) {
                return false;
            } else {
                return true;
            }
        }

        $scope.invalidDate = function (day, month, year) {
            if (year % 4 == 0 && month == 2) {
                if (day > 29) {
                    return true;
                } else {
                    return false;
                }
            } else {
                if (month == 2 && (day == null || day > 28)) {
                    return true;
                } else if (month == 4 || month == 6 || month == 9 || month == 11) {
                    if (day > 30) {
                        return true;
                    } else {
                        return false;
                    }
                } else {
                    return false;
                }
            }
        }

        $scope.invalidPassportExpiry = function(date, month, year) {
            var expiry = new Date(year, month, date);
            if ($scope.flightDetail.departureFullDate >= expiry.setMonth(expiry.getMonth() - 6)) {
                return true;
            } else {
                return false;
            }
        }

        $scope.validateForm = function (page) {
            if (page == 1) {
                if (!$scope.CheckTitle([$scope.buyerInfo])) {
                    $scope.form.incompleteContactTitle = true;
                } else {
                    $scope.form.incompleteContactTitle = false;
                }

                if (!$scope.checkNameComplete([$scope.buyerInfo])) {
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

                if (!$scope.form.incompleteContactTitle && !$scope.form.incompleteContactName
                    && !$scope.form.incompleteContactPhone && !$scope.form.incompleteContactEmail) {
                    $scope.changePage(2);
                }

            }
            if (page == 2) {
                if ($scope.hasDuplicatePaxName()) {
                    $scope.form.hasDuplicatePaxName = true;
                } else {
                    $scope.form.hasDuplicatePaxName = false;
                }

                if (!$scope.checkNameComplete($scope.passengers)) {
                    $scope.form.checkNameIncomplete = true;
                } else {
                    $scope.form.checkNameIncomplete = false;
                }

                if (!$scope.CheckTitle($scope.passengers)) {
                    $scope.form.incompleteTitles = true;
                } else {
                    $scope.form.incompleteTitles = false;
                }

                if (($scope.birthDateRequired || (!$scope.birthDateRequired && !$scope.CheckOnlyAdult())) && !$scope.CheckDate($scope.passengers)) {
                    $scope.form.incompleteBirthDates = true;
                } else {
                    $scope.form.incompleteBirthDates = false;
                }

                if ($scope.passportRequired && (!$scope.CheckPassportDate($scope.passengers) || !$scope.passportNumberFilled($scope.passengers))) {
                    $scope.form.incompletePassportData = true;
                } else {
                    $scope.form.incompletePassportData = false;
                }

                //if ($scope.issueDatePassportRequired) {
                //    $scope.form.incompletePassportData = false;
                //} else {
                //    $scope.form.incompletePassportData = true;
                //}

                if (!$scope.form.hasDuplicatePaxName && !$scope.form.incompleteTitles && !$scope.form.incompleteBirthDates
                    && !$scope.form.incompletePassportData && !$scope.form.checkNameIncomplete) {
                    $scope.changePage(3);
                }
            }
        }

        //*****************************END*****************************
        
        //********************FUNCTIONS TO FILL FORM*******************

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
                    years = years.reverse();
                    return ['Tahun'].concat(years);
                    break;
                case 'child':
                    listYear((departureDate.getFullYear() - 12), (departureDate.getFullYear() - 2));
                    years = years.reverse();
                    return ['Tahun'].concat(years);
                    break;
                case 'infant':
                    listYear((departureDate.getFullYear() - 2), $scope.bookingDate.getFullYear());
                    years = years.reverse();
                    return ['Tahun'].concat(years);
                    break;
                case 'passport':
                    listYear($scope.flightDetail.passportFullDate.getFullYear(), ($scope.flightDetail.passportFullDate.getFullYear() + 15));
                    //years = years.reverse();
                    return ['Tahun'].concat(years);
                    break;
                case 'createdPassport':
                    listYear(($scope.flightDetail.passportCreatedDate.getFullYear() - 15), ($scope.flightDetail.passportCreatedDate.getFullYear()));
                    //years = years.reverse();  
                    return ['Tahun'].concat(years);
                    break;
            }

        }
        // generate date
        $scope.dates = function (month, year) {
            var dates = ['Tanggal'];
            var maxDate = -1;
            // check leap year
            if (year % 4 == 0 && year % 100 != 0 && month == 1) {
                maxDate = 29;
            } else {
                if (month == 1) {
                    maxDate = 28;
                } else if (month == 3 || month == 5 || month == 8 || month == 10) {
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
                passenger.birth = {};
                passenger.type = 'adult';
            } else if (passenger.type == 'infant') {
                passenger.birth = {};
                passenger.type = 'infant';
            } else if (passenger.type == 'child') {
                passenger.birth = {};
                passenger.type = 'child';
            }
            if (nationalityRequired == true) {
                passenger.nationality = 'Indonesia';
            }
            passenger.passport = {
                expire: {},
                created: {}
            }
        }
        // validate passenger birthday
        $scope.validateBirthday = function (passenger) {
            if (passenger.type == 'adult') {
                if (passenger.birth.year >= $scope.flightDetail.departureFullDate.getFullYear() - 12) {
                    passenger.birth.year = $scope.flightDetail.departureFullDate.getFullYear() - 12;
                    if (passenger.birth.month - 1 >= $scope.flightDetail.departureFullDate.getMonth()) {
                        passenger.birth.month = $scope.flightDetail.departureFullDate.getMonth() + 1;
                        if (passenger.birth.date > $scope.flightDetail.departureFullDate.getDate()) {
                            passenger.birth.date = $scope.flightDetail.departureFullDate.getDate();
                        }
                    }
                }
            }

            else if (passenger.type == 'child') {
                var minYear = $scope.flightDetail.departureFullDate.getFullYear() - 12;
                var maxYear = $scope.flightDetail.departureFullDate.getFullYear() - 2;
                if (passenger.birth.year == minYear) {
                    if (passenger.birth.month - 1 <= $scope.flightDetail.departureFullDate.getMonth()) {
                        passenger.birth.month = $scope.flightDetail.departureFullDate.getMonth() + 1;
                        if (passenger.birth.date < $scope.flightDetail.departureFullDate.getDate() + 1) {
                            passenger.birth.date = $scope.flightDetail.departureFullDate.getDate() + 1;
                        }
                    }
                } else if (passenger.birth.year == maxYear) {
                    if (passenger.birth.month - 1 >= $scope.flightDetail.departureFullDate.getMonth()) {
                        passenger.birth.month = $scope.flightDetail.departureFullDate.getMonth() + 1;
                        if (passenger.birth.date > $scope.flightDetail.departureFullDate.getDate()) {
                            passenger.birth.date = $scope.flightDetail.departureFullDate.getDate();
                        }
                    }
                }
            }
            else if (passenger.type == 'infant') {
                minYear = $scope.flightDetail.departureFullDate.getFullYear() - 2;
                maxYear = $scope.bookingDate.getFullYear();
                if (passenger.birth.year == minYear) {
                    if (passenger.birth.month - 1 <= $scope.flightDetail.departureFullDate.getMonth()) {
                        passenger.birth.month = $scope.flightDetail.departureFullDate.getMonth() + 1;
                        if (passenger.birth.date < $scope.flightDetail.departureFullDate.getDate() + 1) {
                            passenger.birth.date = $scope.flightDetail.departureFullDate.getDate() + 1;
                        }
                    }
                }
                else if (passenger.birth.year == maxYear) {
                    if (passenger.birth.month > $scope.flightDetail.bookingDate.getMonth() - 2 &&
                        passenger.birth.year == $scope.flightDetail.bookingDate.getFullYear()) {
                        passenger.birth.month = $scope.bookingDate.getMonth() + 1;
                        if (passenger.birth.date > $scope.bookingDate.getDate()) {
                            passenger.birth.date = $scope.bookingDate.getDate();
                        }
                    }
                }
            }
        }
        // validate passport expiry date
        $scope.validatePassport = function (passenger) {
            if (passenger.passport.expire.year == $scope.flightDetail.passportFullDate.getFullYear()) {
                if (passenger.passport.expire.month - 1 <= $scope.flightDetail.passportFullDate.getMonth()) {
                    passenger.passport.expire.month = $scope.flightDetail.passportFullDate.getMonth() + 1;
                    if (passenger.passport.expire.date <= $scope.flightDetail.passportFullDate.getDate()) {
                        passenger.passport.expire.date = $scope.flightDetail.passportFullDate.getDate();
                    }
                }
            }
        }

        //CHANGE TITLE
        $scope.changeTitle = function (title) {
            if (title == 'Mister')
                return 'Tn.';
            else if (title == 'Mistress')
                return 'Ny.';
            else if (title == 'Miss')
                return 'Nn.';
        }
        //************************* END ******************************

        //*********************** LOG IN *****************************
        //Get Profile
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
                            $log.debug('There is an error');
                            $log.debug('Error : ' + returnData.data.error);
                            $log.debug(returnData);
                            window.location.href = $location.absUrl();
                        }
                    }).catch(function () {
                        $scope.trial++;
                        if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                        {
                            $scope.TakeProfileConfig.TakeProfile();
                        }
                        else
                        {
                            $scope.buyerInfo = {};
                            $log.debug('Failed to Get Profile');
                            window.location.href = $location.absUrl();
                        }
                        
                    });
                }
                else {
                    $log.debug('Not Authorized');
                }
                
            }
        }

        //Function Login in Checkout Page
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
                    if (returnData.data.status == '200') {
                        setCookie("accesstoken", returnData.data.accessToken, returnData.data.expTime);
                        setCookie("refreshtoken", returnData.data.refreshToken, returnData.data.expTime);
                        setCookie("authkey", returnData.data.accessToken, returnData.data.expTime);
                        $scope.TakeProfileConfig.TakeProfile();
                    }
                    else {
                        window.location.href = $location.absUrl();
                        $scope.form.submitting = false;
                        $scope.form.submitted = false;
                        $scope.form.isLogin = false;
                        //Return langsung
                    }
                }).catch(function () {
                    $scope.trial++;
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.form.submit();
                    }
                    else {
                        $log.debug('Failed to Login');
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

        $scope.loggedIn = getAuthAccess();

        $scope.buyerInfo = {};
        if ($scope.loggedIn == 2) {
            // call API get Profile
            var authAccess = getAuthAccess();
            if (authAccess == 2) {
                $http({
                    method: 'GET',
                    url: GetProfileConfig.Url,
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).then(function (returnData) {
                    if (returnData.data.status == "200") {
                        $log.debug('Success getting Profile');
                        $scope.buyerInfo.name = returnData.data.name;
                        $scope.buyerInfo.countryCode = parseInt(returnData.data.countryCallCd);
                        $scope.buyerInfo.phone = parseInt(returnData.data.phone);
                        $scope.buyerInfo.email = returnData.data.email;
                    }
                    else {
                        $scope.buyerInfo = {};
                    }
                }).catch(function () {
                    $scope.buyerInfo = {};
                });
            }
            else {
                $scope.buyerInfo = {};
                $log.debug('Not Authorized');
            }

        } else {
            $scope.buyerInfo = {};
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

        //************************* END ******************************

        //*********************** BOOKING ****************************
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
                if ($scope.trial > 3) {
                    $scope.trial = 0;
                }
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
                        $scope.passengers[i].passport.created.date = '';
                        $scope.passengers[i].passport.created.month = '';
                        $scope.passengers[i].passport.created.year = '';
                        $scope.passengers[i].passport.created.full = '';
                        if (!$scope.nationalityRequired) {
                            $scope.passengers[i].nationality = '';
                        }
                    }
                    if (!$scope.idRequired) {
                        $scope.passengers[i].idNumber = '';
                    }
                    
                    // birthdate
                    $scope.passengers[i].birth.full = $scope.passengers[i].birth.year
                        + '/' + ('0' + (parseInt($scope.passengers[i].birth.month))).slice(-2)
                        + '/' + ('0' + $scope.passengers[i].birth.date).slice(-2);
                    // passport expiry date
                    $scope.passengers[i].passport.expire.full = $scope.passengers[i].passport.expire.year + '/' + ('0' + (parseInt($scope.passengers[i].passport.expire.month) )).slice(-2) + '/' + ('0' + $scope.passengers[i].passport.expire.date).slice(-2);
                    // passport created date
                    $scope.passengers[i].passport.created.full = $scope.passengers[i].passport.created.year + '/' + ('0' + (parseInt($scope.passengers[i].passport.created.month))).slice(-2) + '/' + ('0' + $scope.passengers[i].passport.created.date).slice(-2);


                    $scope.paxData = $scope.paxData + '{ "type":"' + $scope.passengers[i].type + '", "title":"' + $scope.passengers[i].title + '" , "name":"' + $scope.passengers[i].name + '" ';

                    if ($scope.birthDateRequired || $scope.passengers[i].type != 'adult') {
                        $scope.paxData = $scope.paxData + ', "dob":"' + $scope.passengers[i].birth.full + '"';
                    }

                    if ($scope.nationalityRequired) {
                        $scope.paxData = $scope.paxData + ', "nationality":"' + $scope.passengers[i].nationality+ '"';
                    }

                    if ($scope.passportRequired) {
                        $scope.paxData = $scope.paxData + ', "passportNo":"' + $scope.passengers[i].passport.number + '" , "passportExp":"' + $scope.passengers[i].passport.expire.full + '" , "passportCountry":"' + $scope.passengers[i].passport.country + '" ';
                        if ($scope.issueDatePassportRequired) {
                            $scope.paxData = $scope.paxData + ', "passportIssueDate":"' + $scope.passengers[i].passport.created.full + '"';
                        }
                    }

                    if (i != $scope.passengers.length - 1) {
                        $scope.paxData = $scope.paxData + '}' + ',';
                    } else {
                        $scope.paxData = $scope.paxData + '}';
                    }        
                }
                
                $scope.paxData = $scope.paxData + ']';
                $scope.book.postData = '{' + $scope.book.postData + ',' + $scope.paxData + '}';
                $log.debug($scope.book.postData);
                $scope.book.postData = JSON.parse($scope.book.postData);

                $log.debug($scope.book.postData);

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
                        if (returnData.data.status == '200' && (returnData.data.rsvNo != null || returnData.data.rsvNo != '' )) {
                            if (returnData.data.itinChanged) {
                                $scope.book.isSuccess = false;
                                $scope.book.checked = true;
                                $scope.book.booking = false;
                            }

                            else if (returnData.data.priceChanged) {
                                $scope.book.isPriceChanged = true;
                                $scope.book.isSuccess = true;
                                $scope.book.newPrice = returnData.data.price;
                                $scope.book.checked = false;
                                $scope.book.booking = false;
                            }

                            else if (returnData.data.rsvNo != null && returnData.data.rsvNo.length != 0) {
                                $scope.book.isSuccess = true;
                                $scope.book.rsvNo = returnData.data.rsvNo;
                                $('form#rsvno input#rsvno-input').val(returnData.data.rsvNo);
                                $('form#rsvno').submit();
                                $scope.book.checked = true;
                                $scope.book.booking = false;
                            }
                           
                            else  {
                                $scope.book.isSuccess = false;
                                $scope.book.checked = true;
                                $scope.book.booking = false;
                            }

                        } else {
                            if (returnData.data.price != null) {
                                $scope.book.isPriceChanged = true;
                                $scope.book.isSuccess = false;
                                $scope.book.newPrice = returnData.data.price;
                                $scope.book.booking = false;
                                $scope.book.checked = true;
                            }
                            else {
                                $scope.book.isSuccess = false;
                                $scope.book.checked = true;
                                $scope.book.booking = false;
                                $log.debug(returnData);
                                $scope.errorMessage = returnData.data.error;
                            }
                        }

                    }).catch(function (returnData) {
                        $scope.trial++;
                        if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                        {
                            $scope.book.send();
                        }
                        else
                        {
                            $log.debug(returnData);
                            $scope.book.checked = true;
                            $scope.book.isSuccess = false;
                        }
                        
                    });
                }
                else {
                    $log.debug('Not Authorized');
                    $scope.book.checked = true;
                    $scope.book.isSuccess = false;
                }

            }
        };
        //************************* END ******************************

        //*********************** OTHERS *****************************
        
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

        // submit checkout form TBD
        //$scope.submitCheckoutForm = function () {
        //    $scope.checkoutForm.loading = true;
        //}

        // log $scope TBD
        $scope.printScope = function () {
            $log.debug($scope);
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

        //************************* END ******************************
    }
]);// checkout controller

