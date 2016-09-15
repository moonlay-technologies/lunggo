// User Account Controller

app.controller('siteHeaderController', [
    '$http', '$scope', function ($http, $scope) {
        $scope.profileloaded = false;
        $scope.email = '';
        $scope.trial = 0;
        $scope.authProfile = {}
        $scope.ProfileConfig = {
            getProfile: function () {
                if ($scope.trial > 3) {
                    $scope.trial = 0;
                }
                $scope.name = '';
                $http({
                    method: 'GET',
                    url: GetProfileConfig.Url,
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).then(function (returnData) {
                    if (returnData.data.status == "200") {
                        $scope.pageLoaded = true;
                        $scope.isLogin = true;
                        console.log('Success getting Profile');
                        $scope.email = returnData.data.email;
                        $scope.profileloaded = true;
                        console.log(returnData);
                        $scope.name = returnData.data.name;
                        //$scope.lastName = returnData.last;
                    }
                    else {
                        console.log('There is an error');
                        console.log('Error : ' + returnData.data.error);
                        $scope.pageLoaded = true;
                        $scope.isLogin = false;
                        $scope.profileloaded = true;
                        console.log(returnData);
                    }
                }).catch(function (returnData) {
                    $scope.trial++;
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.ProfileConfig.getProfile();
                    }
                    else {
                        console.log('Failed to Login');
                        $scope.pageLoaded = true;
                        $scope.isLogin = false;
                        $scope.profileloaded = true;
                    }
                });
            }
        }
        //Set Authorization to get Profile
        $scope.authAccess = getAuthAccess();
        if ($scope.authAccess == 2) {
            $scope.ProfileConfig.getProfile();
        }
        else {
            $scope.isLogin = false;
            $scope.pageLoaded = true;
        }
        $scope.returnUrl = document.referrer;
        $scope.logout = {
            isLogout: false,
            getLogout: function () {
                eraseCookie('accesstoken');
                eraseCookie('refreshtoken');
                eraseCookie('authkey');
                $scope.isLogin = false;
                $scope.logout.isLogout =  true;
                window.location.href = "/";
            }
        }
    }]
);


app.controller('UserAccountController', ['$http', '$scope', '$rootScope', '$location',function ($http, $scope, $rootScope, $location) {

    $scope.PageConfig = $rootScope.PageConfig;
    $scope.Countries = $rootScope.Countries;
    //$scope.Order;
    $scope.trial = 0;
    $scope.pageLoaded = true;
    $scope.formMessage = '';
    $scope.NotifBox = false;
    //if (order.length) {
    //    $scope.Order = JSON.parse(order);
    //}
    $scope.getdata = function () {
        if ($scope.trial > 3) {
            $scope.trial = 0;
        }
        var authAccess = getAuthAccess();
        if (authAccess == 2) {
            $http({
                method: 'GET',
                url: GetProfileConfig.Url,
                headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
            }).then(function (returnData) {
                if (returnData.status == "200") {
                    $scope.pageLoaded = true;
                    $scope.isLogin = true;
                    console.log('Success getting Profile');
                    $scope.email = returnData.data.email;
                    $scope.profileloaded = true;
                    console.log(returnData);
                    $scope.UserProfile.name = returnData.data.name;
                    $scope.UserProfile.email = returnData.data.email;
                    $scope.UserProfile.phone = returnData.data.phone;
                    $scope.UserProfile.country = returnData.data.countryCallCd;
                }
                else {
                    console.log('There is an error');
                    console.log('Error : ' + returnData.data.error);
                    $scope.pageLoaded = true;
                    $scope.isLogin = false;
                    $scope.profileloaded = true;
                    console.log(returnData);
                }
            }).catch(function (returnData) {
                $scope.trial++;
                if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                {
                    $scope.getdata();
                }
                else
                {
                    console.log('Failed to Login');
                    $scope.pageLoaded = true;
                    $scope.isLogin = false;
                    $scope.profileloaded = true;
                }
            });
        }
        else
        {
            console.log('Not Authorized');
            $scope.pageLoaded = true;
            $scope.isLogin = false;
            $scope.profileloaded = true;
        }
        
    }
    $scope.getdata();
    // order status
    $scope.OrderStatus = function(num) {
        var text = '';
        switch (num) {
            case 1:
                text = 'Cancel';
                break;
            case 2:
                text = 'Pending';
                break;
            case 3:
                text = 'Settled';
                break;
            case 4:
                text = 'Denied';
                break;
            case 5:
                text = 'Expired';
                break;
            case 6:
                text = 'Veryfing';
                break;
            case 7:
                text = 'Challanged';
                break;
        }
        return text;
    }
    // flight type
    $scope.FlightType = function(num) {
        var text = '';
        switch (num) {
            case 0:
                text = 'OneWay';
                break;
            case 1:
                text = 'Return';
                break;
        }
        return text;
    }

    // change page
    $scope.PageConfig.ActivePage = 'menu';
    $scope.PageConfig.ActivePageChanged = false;
    $scope.PageConfig.ChangePage = function(page) {
        $scope.PageConfig.ActivePage = page;
        $scope.PageConfig.ActivePageChanged = true;
        $location.hash(page);
    }
    angular.element(document).ready(function () {
        $location.hash('menu');
    });

    $scope.$watch('PageConfig.ActivePage', function() {
        $scope.NotifBox = false;
    });
    
    $scope.$watch(function () {
        return location.hash;
    }, function (value) {
        if (!$scope.PageConfig.ActivePageChanged) {
            if (value != '') {
                $scope.PageConfig.ChangePage('menu');
            }
        } else {
            if (value == '' || value == '##menu') {
                $scope.PageConfig.ActivePage = 'menu';
            } else if (value == '##history' || value == 'history') {
                $scope.PageConfig.ActivePage = 'history';
            } else if (value == '##profile' || value == 'profile') {
                $scope.PageConfig.ActivePage = 'profile';
            }
        }
    });
    // user profile
    $scope.countries = [{ name: 'Pilih Negara', dial_code: 'xx', code: '' }, { name: 'Afghanistan', dial_code: '93', code: 'AF' }, { name: 'Aland Islands', dial_code: '358', code: 'AX' }, { name: 'Albania', dial_code: '355', code: 'AL' }, { name: 'Algeria', dial_code: '213', code: 'DZ' }, { name: 'AmericanSamoa', dial_code: '1 684', code: 'AS' }, { name: 'Andorra', dial_code: '376', code: 'AD' }, { name: 'Angola', dial_code: '244', code: 'AO' }, { name: 'Anguilla', dial_code: '1 264', code: 'AI' }, { name: 'Antarctica', dial_code: '672', code: 'AQ' }, { name: 'Antigua and Barbuda', dial_code: '1268', code: 'AG' }, { name: 'Argentina', dial_code: '54', code: 'AR' }, { name: 'Armenia', dial_code: '374', code: 'AM' }, { name: 'Aruba', dial_code: '297', code: 'AW' }, { name: 'Australia', dial_code: '61', code: 'AU' }, { name: 'Austria', dial_code: '43', code: 'AT' }, { name: 'Azerbaijan', dial_code: '994', code: 'AZ' }, { name: 'Bahamas', dial_code: '1 242', code: 'BS' }, { name: 'Bahrain', dial_code: '973', code: 'BH' }, { name: 'Bangladesh', dial_code: '880', code: 'BD' }, { name: 'Barbados', dial_code: '1 246', code: 'BB' }, { name: 'Belarus', dial_code: '375', code: 'BY' }, { name: 'Belgium', dial_code: '32', code: 'BE' }, { name: 'Belize', dial_code: '501', code: 'BZ' }, { name: 'Benin', dial_code: '229', code: 'BJ' }, { name: 'Bermuda', dial_code: '1 441', code: 'BM' }, { name: 'Bhutan', dial_code: '975', code: 'BT' }, { name: 'Bolivia, Plurinational State of', dial_code: '591', code: 'BO' }, { name: 'Bosnia and Herzegovina', dial_code: '387', code: 'BA' }, { name: 'Botswana', dial_code: '267', code: 'BW' }, { name: 'Brazil', dial_code: '55', code: 'BR' }, { name: 'British Indian Ocean Territory', dial_code: '246', code: 'IO' }, { name: 'Brunei Darussalam', dial_code: '673', code: 'BN' }, { name: 'Bulgaria', dial_code: '359', code: 'BG' }, { name: 'Burkina Faso', dial_code: '226', code: 'BF' }, { name: 'Burundi', dial_code: '257', code: 'BI' }, { name: 'Cambodia', dial_code: '855', code: 'KH' }, { name: 'Cameroon', dial_code: '237', code: 'CM' }, { name: 'Canada', dial_code: '1', code: 'CA' }, { name: 'Cape Verde', dial_code: '238', code: 'CV' }, { name: 'Cayman Islands', dial_code: ' 345', code: 'KY' }, { name: 'Central African Republic', dial_code: '236', code: 'CF' }, { name: 'Chad', dial_code: '235', code: 'TD' }, { name: 'Chile', dial_code: '56', code: 'CL' }, { name: 'China', dial_code: '86', code: 'CN' }, { name: 'Christmas Island', dial_code: '61', code: 'CX' }, { name: 'Cocos (Keeling) Islands', dial_code: '61', code: 'CC' }, { name: 'Colombia', dial_code: '57', code: 'CO' }, { name: 'Comoros', dial_code: '269', code: 'KM' }, { name: 'Congo', dial_code: '242', code: 'CG' }, { name: 'Congo, The Democratic Republic of the Congo', dial_code: '243', code: 'CD' }, { name: 'Cook Islands', dial_code: '682', code: 'CK' }, { name: 'Costa Rica', dial_code: '506', code: 'CR' }, { name: "Cote d'Ivoire", dial_code: '225', code: 'CI' }, { name: 'Croatia', dial_code: '385', code: 'HR' }, { name: 'Cuba', dial_code: '53', code: 'CU' }, { name: 'Cyprus', dial_code: '357', code: 'CY' }, { name: 'Czech Republic', dial_code: '420', code: 'CZ' }, { name: 'Denmark', dial_code: '45', code: 'DK' }, { name: 'Djibouti', dial_code: '253', code: 'DJ' }, { name: 'Dominica', dial_code: '1 767', code: 'DM' }, { name: 'Dominican Republic', dial_code: '1 849', code: 'DO' }, { name: 'Ecuador', dial_code: '593', code: 'EC' }, { name: 'Egypt', dial_code: '20', code: 'EG' }, { name: 'El Salvador', dial_code: '503', code: 'SV' }, { name: 'Equatorial Guinea', dial_code: '240', code: 'GQ' }, { name: 'Eritrea', dial_code: '291', code: 'ER' }, { name: 'Estonia', dial_code: '372', code: 'EE' }, { name: 'Ethiopia', dial_code: '251', code: 'ET' }, { name: 'Falkland Islands (Malvinas)', dial_code: '500', code: 'FK' }, { name: 'Faroe Islands', dial_code: '298', code: 'FO' }, { name: 'Fiji', dial_code: '679', code: 'FJ' }, { name: 'Finland', dial_code: '358', code: 'FI' }, { name: 'France', dial_code: '33', code: 'FR' }, { name: 'French Guiana', dial_code: '594', code: 'GF' }, { name: 'French Polynesia', dial_code: '689', code: 'PF' }, { name: 'Gabon', dial_code: '241', code: 'GA' }, { name: 'Gambia', dial_code: '220', code: 'GM' }, { name: 'Georgia', dial_code: '995', code: 'GE' }, { name: 'Germany', dial_code: '49', code: 'DE' }, { name: 'Ghana', dial_code: '233', code: 'GH' }, { name: 'Gibraltar', dial_code: '350', code: 'GI' }, { name: 'Greece', dial_code: '30', code: 'GR' }, { name: 'Greenland', dial_code: '299', code: 'GL' }, { name: 'Grenada', dial_code: '1 473', code: 'GD' }, { name: 'Guadeloupe', dial_code: '590', code: 'GP' }, { name: 'Guam', dial_code: '1 671', code: 'GU' }, { name: 'Guatemala', dial_code: '502', code: 'GT' }, { name: 'Guernsey', dial_code: '44', code: 'GG' }, { name: 'Guinea', dial_code: '224', code: 'GN' }, { name: 'Guinea-Bissau', dial_code: '245', code: 'GW' }, { name: 'Guyana', dial_code: '595', code: 'GY' }, { name: 'Haiti', dial_code: '509', code: 'HT' }, { name: 'Holy See (Vatican City State)', dial_code: '379', code: 'VA' }, { name: 'Honduras', dial_code: '504', code: 'HN' }, { name: 'Hong Kong', dial_code: '852', code: 'HK' }, { name: 'Hungary', dial_code: '36', code: 'HU' }, { name: 'Iceland', dial_code: '354', code: 'IS' }, { name: 'India', dial_code: '91', code: 'IN' }, { name: 'Indonesia', dial_code: '62', code: 'ID' }, { name: 'Iran, Islamic Republic of Persian Gulf', dial_code: '98', code: 'IR' }, { name: 'Iraq', dial_code: '964', code: 'IQ' }, { name: 'Ireland', dial_code: '353', code: 'IE' }, { name: 'Isle of Man', dial_code: '44', code: 'IM' }, { name: 'Israel', dial_code: '972', code: 'IL' }, { name: 'Italy', dial_code: '39', code: 'IT' }, { name: 'Jamaica', dial_code: '1 876', code: 'JM' }, { name: 'Japan', dial_code: '81', code: 'JP' }, { name: 'Jersey', dial_code: '44', code: 'JE' }, { name: 'Jordan', dial_code: '962', code: 'JO' }, { name: 'Kazakhstan', dial_code: '7 7', code: 'KZ' }, { name: 'Kenya', dial_code: '254', code: 'KE' }, { name: 'Kiribati', dial_code: '686', code: 'KI' }, { name: "Korea, Democratic People's Republic of Korea", dial_code: '850', code: 'KP' }, { name: 'Korea, Republic of South Korea', dial_code: '82', code: 'KR' }, { name: 'Kuwait', dial_code: '965', code: 'KW' }, { name: 'Kyrgyzstan', dial_code: '996', code: 'KG' }, { name: 'Laos', dial_code: '856', code: 'LA' }, { name: 'Latvia', dial_code: '371', code: 'LV' }, { name: 'Lebanon', dial_code: '961', code: 'LB' }, { name: 'Lesotho', dial_code: '266', code: 'LS' }, { name: 'Liberia', dial_code: '231', code: 'LR' }, { name: 'Libyan Arab Jamahiriya', dial_code: '218', code: 'LY' }, { name: 'Liechtenstein', dial_code: '423', code: 'LI' }, { name: 'Lithuania', dial_code: '370', code: 'LT' }, { name: 'Luxembourg', dial_code: '352', code: 'LU' }, { name: 'Macao', dial_code: '853', code: 'MO' }, { name: 'Macedonia', dial_code: '389', code: 'MK' }, { name: 'Madagascar', dial_code: '261', code: 'MG' }, { name: 'Malawi', dial_code: '265', code: 'MW' }, { name: 'Malaysia', dial_code: '60', code: 'MY' }, { name: 'Maldives', dial_code: '960', code: 'MV' }, { name: 'Mali', dial_code: '223', code: 'ML' }, { name: 'Malta', dial_code: '356', code: 'MT' }, { name: 'Marshall Islands', dial_code: '692', code: 'MH' }, { name: 'Martinique', dial_code: '596', code: 'MQ' }, { name: 'Mauritania', dial_code: '222', code: 'MR' }, { name: 'Mauritius', dial_code: '230', code: 'MU' }, { name: 'Mayotte', dial_code: '262', code: 'YT' }, { name: 'Mexico', dial_code: '52', code: 'MX' }, { name: 'Micronesia, Federated States of Micronesia', dial_code: '691', code: 'FM' }, { name: 'Moldova', dial_code: '373', code: 'MD' }, { name: 'Monaco', dial_code: '377', code: 'MC' }, { name: 'Mongolia', dial_code: '976', code: 'MN' }, { name: 'Montenegro', dial_code: '382', code: 'ME' }, { name: 'Montserrat', dial_code: '1664', code: 'MS' }, { name: 'Morocco', dial_code: '212', code: 'MA' }, { name: 'Mozambique', dial_code: '258', code: 'MZ' }, { name: 'Myanmar', dial_code: '95', code: 'MM' }, { name: 'Namibia', dial_code: '264', code: 'NA' }, { name: 'Nauru', dial_code: '674', code: 'NR' }, { name: 'Nepal', dial_code: '977', code: 'NP' }, { name: 'Netherlands', dial_code: '31', code: 'NL' }, { name: 'Netherlands Antilles', dial_code: '599', code: 'AN' }, { name: 'New Caledonia', dial_code: '687', code: 'NC' }, { name: 'New Zealand', dial_code: '64', code: 'NZ' }, { name: 'Nicaragua', dial_code: '505', code: 'NI' }, { name: 'Niger', dial_code: '227', code: 'NE' }, { name: 'Nigeria', dial_code: '234', code: 'NG' }, { name: 'Niue', dial_code: '683', code: 'NU' }, { name: 'Norfolk Island', dial_code: '672', code: 'NF' }, { name: 'Northern Mariana Islands', dial_code: '1 670', code: 'MP' }, { name: 'Norway', dial_code: '47', code: 'NO' }, { name: 'Oman', dial_code: '968', code: 'OM' }, { name: 'Pakistan', dial_code: '92', code: 'PK' }, { name: 'Palau', dial_code: '680', code: 'PW' }, { name: 'Palestinian Territory, Occupied', dial_code: '970', code: 'PS' }, { name: 'Panama', dial_code: '507', code: 'PA' }, { name: 'Papua New Guinea', dial_code: '675', code: 'PG' }, { name: 'Paraguay', dial_code: '595', code: 'PY' }, { name: 'Peru', dial_code: '51', code: 'PE' }, { name: 'Philippines', dial_code: '63', code: 'PH' }, { name: 'Pitcairn', dial_code: '872', code: 'PN' }, { name: 'Poland', dial_code: '48', code: 'PL' }, { name: 'Portugal', dial_code: '351', code: 'PT' }, { name: 'Puerto Rico', dial_code: '1 939', code: 'PR' }, { name: 'Qatar', dial_code: '974', code: 'QA' }, { name: 'Romania', dial_code: '40', code: 'RO' }, { name: 'Russia', dial_code: '7', code: 'RU' }, { name: 'Rwanda', dial_code: '250', code: 'RW' }, { name: 'Reunion', dial_code: '262', code: 'RE' }, { name: 'Saint Barthelemy', dial_code: '590', code: 'BL' }, { name: 'Saint Helena, Ascension and Tristan Da Cunha', dial_code: '290', code: 'SH' }, { name: 'Saint Kitts and Nevis', dial_code: '1 869', code: 'KN' }, { name: 'Saint Lucia', dial_code: '1 758', code: 'LC' }, { name: 'Saint Martin', dial_code: '590', code: 'MF' }, { name: 'Saint Pierre and Miquelon', dial_code: '508', code: 'PM' }, { name: 'Saint Vincent and the Grenadines', dial_code: '1 784', code: 'VC' }, { name: 'Samoa', dial_code: '685', code: 'WS' }, { name: 'San Marino', dial_code: '378', code: 'SM' }, { name: 'Sao Tome and Principe', dial_code: '239', code: 'ST' }, { name: 'Saudi Arabia', dial_code: '966', code: 'SA' }, { name: 'Senegal', dial_code: '221', code: 'SN' }, { name: 'Serbia', dial_code: '381', code: 'RS' }, { name: 'Seychelles', dial_code: '248', code: 'SC' }, { name: 'Sierra Leone', dial_code: '232', code: 'SL' }, { name: 'Singapore', dial_code: '65', code: 'SG' }, { name: 'Slovakia', dial_code: '421', code: 'SK' }, { name: 'Slovenia', dial_code: '386', code: 'SI' }, { name: 'Solomon Islands', dial_code: '677', code: 'SB' }, { name: 'Somalia', dial_code: '252', code: 'SO' }, { name: 'South Africa', dial_code: '27', code: 'ZA' }, { name: 'South Georgia and the South Sandwich Islands', dial_code: '500', code: 'GS' }, { name: 'Spain', dial_code: '34', code: 'ES' }, { name: 'Sri Lanka', dial_code: '94', code: 'LK' }, { name: 'Sudan', dial_code: '249', code: 'SD' }, { name: 'Suriname', dial_code: '597', code: 'SR' }, { name: 'Svalbard and Jan Mayen', dial_code: '47', code: 'SJ' }, { name: 'Swaziland', dial_code: '268', code: 'SZ' }, { name: 'Sweden', dial_code: '46', code: 'SE' }, { name: 'Switzerland', dial_code: '41', code: 'CH' }, { name: 'Syrian Arab Republic', dial_code: '963', code: 'SY' }, { name: 'Taiwan', dial_code: '886', code: 'TW' }, { name: 'Tajikistan', dial_code: '992', code: 'TJ' }, { name: 'Tanzania, United Republic of Tanzania', dial_code: '255', code: 'TZ' }, { name: 'Thailand', dial_code: '66', code: 'TH' }, { name: 'Timor-Leste', dial_code: '670', code: 'TL' }, { name: 'Togo', dial_code: '228', code: 'TG' }, { name: 'Tokelau', dial_code: '690', code: 'TK' }, { name: 'Tonga', dial_code: '676', code: 'TO' }, { name: 'Trinidad and Tobago', dial_code: '1 868', code: 'TT' }, { name: 'Tunisia', dial_code: '216', code: 'TN' }, { name: 'Turkey', dial_code: '90', code: 'TR' }, { name: 'Turkmenistan', dial_code: '993', code: 'TM' }, { name: 'Turks and Caicos Islands', dial_code: '1 649', code: 'TC' }, { name: 'Tuvalu', dial_code: '688', code: 'TV' }, { name: 'Uganda', dial_code: '256', code: 'UG' }, { name: 'Ukraine', dial_code: '380', code: 'UA' }, { name: 'United Arab Emirates', dial_code: '971', code: 'AE' }, { name: 'United Kingdom', dial_code: '44', code: 'GB' }, { name: 'United States', dial_code: '1', code: 'US' }, { name: 'Uruguay', dial_code: '598', code: 'UY' }, { name: 'Uzbekistan', dial_code: '998', code: 'UZ' }, { name: 'Vanuatu', dial_code: '678', code: 'VU' }, { name: 'Venezuela, Bolivarian Republic of Venezuela', dial_code: '58', code: 'VE' }, { name: 'Vietnam', dial_code: '84', code: 'VN' }, { name: 'Virgin Islands, British', dial_code: '1 284', code: 'VG' }, { name: 'Virgin Islands, U.S.', dial_code: '1 340', code: 'VI' }, { name: 'Wallis and Futuna', dial_code: '681', code: 'WF' }, { name: 'Yemen', dial_code: '967', code: 'YE' }, { name: 'Zambia', dial_code: '260', code: 'ZM' }, { name: 'Zimbabwe', dial_code: '263', code: 'ZW' }];

    $scope.UserProfile = userProfile;
    $scope.UserProfile.Editing = false;
    $scope.UserProfile.Edit = function () {
        if ($scope.UserProfile.Editing == false) {
            $scope.UserProfile.Editing = true;
        } else {
            $scope.UserProfile.Editing = false;
        }
    }
    $scope.EditProfile = editProfile;
    $scope.EditProfile.Updating = false;
    $scope.EditProfile.Failed = false;
    $scope.EditProfile.Submit = function (name) {
        if ($scope.trial > 3) {
            $scope.trial = 0;
        }
        // edit profile form
        if (name == 'profile') {
            $scope.UserProfile.edit = !($scope.UserProfile.edit);
            $scope.UserProfile.updated = false;
        }
        else if (name == 'profileSave') {
            console.log('submitting form');
            $scope.UserProfile.updating = true;
            var authAccess = getAuthAccess();
            if (authAccess == 2) {
                // submit form to URL
                $http({
                    url: ChangeProfileConfig.Url,
                    method: 'PATCH',
                    data: {
                        name: editProfile.name,
                        phone: editProfile.phone,
                        countryCallCd: editProfile.country
                    },
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).then(function (returnData) {
                    console.log(returnData);
                    if (returnData.data.status == '200') {
                        console.log('Success requesting change profile');
                        //$scope.UserProfile.address = editProfile.address;
                        $scope.UserProfile.name = editProfile.name;

                        $scope.UserProfile.phone = editProfile.phone;
                        $scope.UserProfile.country = editProfile.country;
                        $scope.UserProfile.edit = false;
                        $scope.UserProfile.updating = false;
                        $scope.UserProfile.updated = true;
                        $scope.formMessage = 'Profil Anda Telah Berhasil Diperbaharui';
                        $scope.NotifBox = true;
                    }
                    else {
                        console.log(returnData.data.error);
                        $scope.UserProfile.edit = true;
                        $scope.UserProfile.updating = false;
                        $scope.formMessage = 'Profil Anda Belum Diperbaharui';
                        $scope.NotifBox = true;
                    }
                }).catch(function (returnData) {
                    $scope.trial++;
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.EditProfile.Submit(name);
                    }
                    else
                    {
                        console.log('Failed requesting change profile');
                        $scope.profileForm.edit = true;
                        $scope.UserProfile.updating = false;
                        $scope.formMessage = 'Profile Anda Belum Diperbaharui';
                        $scope.NotifBox = true;
                    }
                });
            }
            else
            {
                console.log('Not Authorized');
                $scope.profileForm.edit = true;
                $scope.UserProfile.updating = false;
                $scope.formMessage = 'Profile Anda Belum Diperbaharui';
                $scope.NotifBox = true;
            }
            
        }
        if (name == 'password') {
            $scope.password.edit = !($scope.password.edit);
            $scope.password.failed = false;
        }
        else if (name == 'passwordSave') {

            $scope.password.updating = true;
            $scope.password.failed = false;

            console.log('submitting form');
            var authAccess = getAuthAccess();
            if (authAccess == 2) {
                // submit form to URL
                $http({
                    url: ChangePasswordConfig.Url,
                    method: 'POST',
                    data: {
                        newPassword: $scope.passwordForm.newPassword,
                        oldPassword: $scope.passwordForm.currentPassword
                    },
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).then(function (returnData) {
                    $scope.passwordForm.newPassword = '';
                    $scope.passwordForm.currentPassword = '';
                    $scope.passwordForm.confirmationPassword = '';
                    if (returnData.data.status == '200') {
                        console.log('Success requesting reset password');
                        console.log(returnData);
                        $scope.password.edit = false;
                        $scope.password.updating = false;
                    }
                    else {
                        console.log(returnData.data.error);
                        console.log(returnData);
                        $scope.password.updating = false;
                        $scope.password.failed = true;
                    }
                }).catch(function (returnData) {
                    $scope.trial++;
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.EditProfile.Submit(name);
                    }
                    else {
                        console.log('Failed requesting reset password');
                        $scope.password.edit = true;
                        $scope.password.updating = false;
                    }
                });
            }
            else {
                console.log('Not Authorized');
                $scope.password.edit = true;
                $scope.password.updating = false;
            }
            
        }
        
    }
    // user password
    $scope.UserPassword = {
        CurrentPassword: '',
        NewPassword: '',
        ConfirmPassword: '',
        Editing: false,
        Message: '',
        Edit: function() {
            if ($scope.UserPassword.Editing == false) {
                $scope.UserPassword.Editing = true;
            } else {
                $scope.UserPassword.Editing = false;
            }
        },
        Updating: false,
        Failed: false,
        Submit: function () {
            if ($scope.trial > 3) {
                $scope.trial = 0;
            }
            $scope.UserPassword.Updating = true;
            var authAccess = getAuthAccess();
            if (authAccess == 2) {
                $http({
                    url: ChangePasswordConfig.Url,
                    method: 'POST',
                    data: {
                        newPassword: $scope.UserPassword.NewPassword,
                        oldPassword: $scope.UserPassword.CurrentPassword
                    },
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).then(function (returnData) {
                    $scope.UserPassword.NewPassword = '';
                    $scope.UserPassword.CurrentPassword = '';
                    $scope.UserPassword.ConfirmPassword = '';
                    if (returnData.data.status == '200') {
                        console.log('Success requesting reset password');
                        console.log(returnData);
                        $scope.UserPassword.Updating = false;
                        $scope.UserPassword.Editing = false;
                        $scope.formMessage = 'Password Anda Telah Berhasil Diperbaharui';
                        $scope.NotifBox = true;
                    }
                    else {
                        console.log(returnData);
                        $scope.UserPassword.Updating = false;
                        $scope.UserPassword.Editing = false;
                        //if (returnData.data.Description == 'ModelInvalid') {
                        //    $scope.formMessage = 'Password Baru yang Anda Masukkan Tidak Cocok dengan yang Dikonfimasi';
                        //} else {
                        $scope.formMessage = 'Password Lama yang Anda Masukkan Tidak Tepat';
                        //}
                        $scope.NotifBox = true;

                    }
                }).catch(function (returnData) {
                    $scope.trial++;
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.UserPassword.Submit();
                    }
                    else {
                        console.log('Failed requesting reset password');
                        $scope.UserPassword.Editing = false;
                        $scope.UserPassword.Updating = false;
                        $scope.formMessage = 'Permohonan ganti password Anda gagal';
                        $scope.NotifBox = true;
                    }
                });
            }
            else {
                console.log('Not Authorized');
                $scope.UserPassword.Editing = false;
                $scope.UserPassword.Updating = false;
                $scope.formMessage = 'Permohonan ganti password Anda gagal';
                $scope.NotifBox = true;
            }

            
        }
    };

    //Get Transaction History

    $scope.trxHistory = {
        getTrxHistory: function () {
            if ($scope.trial > 3) {
                $scope.trial = 0;
            }
            //Check Authorization
            var authAccess = getAuthAccess();
            if (authAccess == 2) {
                $http({
                    method: 'GET',
                    url: TrxHistoryConfig.Url,
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).then(function (returnData) {
                    if (returnData.data.status == "200") {
                        console.log('Success getting Transaction');
                        console.log(returnData);
                        $scope.flightlist = returnData.data.flights;
                        var flights = [];

                        for (var i = 0; i < $scope.flightlist.length; i++) {
                            if ($scope.flightlist[i] !== null) {
                                flights.push($scope.flightlist[i]);
                            }
                        }
                        $scope.flightlist = flights;
                        console.log($scope.flightlist);
                    }
                    else {
                        console.log('There is an error');
                        console.log('Error : ' + returnData.data.error);
                        console.log(returnData);
                    }
                }).catch(function (returnData) {
                    $scope.trial++;
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.trxHistory.getTrxHistory();
                    }
                    else {
                        console.log('Failed to Get Transaction History');
                    }
                });
            }
            else {
                console.log('Not Authorized');
            }
            
        }
    }

    $scope.trxHistory.getTrxHistory();
    
    $scope.returnUrl = "/";
    $scope.logout = {
        isLogout: false,
        getLogout: function () {
            eraseCookie('accesstoken');
            eraseCookie('refreshtoken');
            eraseCookie('authkey');
            //$scope.isLogin = false;
            $scope.logout.isLogout = true;
            window.location.href = "/";
        }
    }
}]);// User Account Controller

// Contact Controller
app.controller('ContactController', ['$http', '$scope', '$rootScope', function ($http, $scope, $rootScope) {

    $scope.PageConfig = $rootScope.PageConfig;

}]);// Contact Controller

// Order Detail Controller
app.controller('OrderDetailController', ['$http', '$scope', '$rootScope', function ($http, $scope, $rootScope) {

    $scope.pageLoaded = true;
    $scope.trial = 0;
    $scope.isExist = false;
    $scope.PageConfig = $rootScope.PageConfig;
    $scope.orderDate = new Date(orderDate);
    $scope.rsvNo = rsvNo;
    $scope.var = 3;
    $scope.flight = [];
    $scope.title = function(title) {
        if (title == '1')
            return 'Tn.';
        else if (title == '2')
            return 'Ny.';
        else if (title == '3')
            return 'Nn.';
    }
    $scope.paxtype = function(types) {
        if (types == 1)
            return 'Dewasa';
        else if (types == 2)
            return 'Anak';
        else if (types == 3)
            return 'Bayi';
    }

    // Pass Data into MVC Controller
    $scope.submitRsvNo = function (rsvNo, rsvStatus) {
        $('form#rsvno input#rsvno-input').val(rsvNo);
        $('form#rsvno input#message-input').val(rsvStatus);
        $('form#rsvno').submit();
    }

    $scope.methodpayment = function(types) {
        if (types == 1)
            return 'Kartu Kredit';
        else if (types == 2)
            return 'Transfer Antar Bank';
        else if (types == 3)
            return 'Mandiri Clickpay';
        else if (types == 4)
            return 'Cimb Clicks';
        else if (types == 5)
            return 'Virtual Account';
        else if (types == 6)
            return 'BCA Klikpay';
        else if (types == 7)
            return 'Epay BRI ';
        else if (types == 8)
            return 'Telkomsel T-Cash';
        else if (types == 9)
            return 'XL Tunai';
        else if (types == 10)
            return 'BBM Money';
        else if (types == 11)
            return 'Indosat Dompetku';
        else if (types == 12)
            return 'Mandiri E-Cash';
        else if (types == 13)
            return 'Mandiri Bill Payment';
        else if (types == 14)
            return 'Indomaret';
        else if (types == 15)
            return 'Credit';
        else if (types == 16)
            return 'Deposit';
    }

    $scope.paymentstatus = function(types) {
        if (types == 3)
            return 'Lunas';
        else if (types == 1)
            return 'Dibatalkan';
        else if (types == 2)
            return 'Menunggu Pembayaran';
        else if (types == 4)
            return 'Ditolak';
        else if (types == 5)
            return 'Kadaluarsa';
        else if (types == 6)
            return 'Memverifikasi Pembayaran';
        else if (types == 7)
            return 'Butuh Verifikasi Ulang';
        else if (types == 8)
            return 'Gagal';
    }

    $scope.ButtonText = function (status, method) {
        if (status == 1) { return 'Halaman Pembayaran'; }

        else if (status == 2 || status == 3) {
            if (method == 2 || method == 5) {
                return 'Instruksi Pembayaran';
            }
            else if (method == 4) {
                return 'Halaman Pembayaran';
            }
            else {
                return 'Lihat Detail';
            }
        }

        else if (status == 7 || status == 9 || status == 10) {
            return 'Lihat Detail';
        }

        else if (status == 4 || status == 5) { return 'Cetak E-tiket'; }

        else if (status == 6 || status == 8) { return 'Cari Penerbangan'; }

    }

    $scope.closeOverlay = function() {
        window.location.href = "/";
    }
    $scope.datafailed = false;
    $scope.getRsv = function () {
        if ($scope.trial > 3) {
            $scope.trial = 0;
        }
        $scope.errormsg = '';
        var authAccess = getAuthAccess();
        if (authAccess == 2 || authAccess == 1)
        {
            $http.get(GetReservationConfig.Url + $scope.rsvNo, {
                headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') },
                rsvNo: $scope.rsvNo
            }).then(function (returnData) {
                if (returnData.data.status == "200") {
                    $scope.flight = returnData.data.flight;
                    $scope.datafailed = false;
                    $scope.isExist = true;
                }
                else {
                    console.log('There is an error');
                    console.log('Error : ' + returnData.data.error);
                    if (returnData.data.error == "ERARSV01") {
                        $scope.errormsg = 'No Reservation Matched';
                    }
                    else if (returnData.data.error == "ERARSV02") {
                        $scope.errormsg = 'Not Authorised';
                    }
                    else if (returnData.data.error == "ERRGEN99") {
                        $scope.errormsg = 'Problem on Server';
                    }

                    $scope.datafailed = true;
                    console.log(returnData);
                    window.location.href = "/";
                }
            }).catch(function (returnData) {
                $scope.trial++;
                if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                {
                    $scope.getRsv();
                }
                else {
                    console.log('Failed to Get Detail');
                    $scope.datafailed = true;
                }
                
            });
        }
        else {
            console.log('Not authorized to get reservation');
            $scope.datafailed = true;
        }
        
    }

    $scope.getRsv();

}]);// Order Detail Controller

// Check Order Controller
app.controller('CheckOrderController', ['$http', '$scope', '$rootScope', function($http, $scope, $rootScope) {
    
    $scope.PageConfig = $rootScope.PageConfig;
    $scope.pageLoaded = true;
    $scope.Order = {
        Number: '',
        Name : ''
    };
}]);// Check Order Controller

// Login Controller
app.controller('LoginController', ['$http', '$scope', '$rootScope', function($http, $scope, $rootScope) {

    $scope.PageConfig = $rootScope.PageConfig;
    $scope.trial = 0;
    $scope.pageLoaded = true;
    $scope.overlay = false;
    $scope.returnUrl = document.referrer;
    //if ($scope.User.Message) {
    //    $scope.overlay = true;
    //} else {
    //    $scope.overlay = false;
    //}
    $scope.closeOverlay = function() {
        $scope.overlay = false;
        $scope.User.Message = '';
    }

    $scope.User = {
        Email: '',
        newEmail: '',
        Resubmitting: false,
        Resubmitted: false,
        Password: '',
        Message: '',
        Sending: false,
        Sent: false,
        Send: function () {
            if ($scope.trial > 3) {
                $scope.trial = 0;
            }
            $scope.User.Sending = true;
            var authAccess = getAuthAccess();
            if (authAccess == 2 || authAccess == 1)
            {
                $http({
                    method: 'POST',
                    url: LoginConfig.Url,
                    data:{
                        userName: $scope.User.Email,
                        password: $scope.User.Password,
                        clientId: 'WWxoa2VrOXFSWFZOUXpSM1QycEpORTB5U1RWT1IxcHNXVlJOTTFsWFZYaE5hbVJwVFVSSk5FOUVTbWxOUkVVMFRrUlNhVmxxVlhwT01sbDNUbXBvYkUxNlJUMD0=',
                        clientSecret: 'VFVSTk1sbHFiR3hhVjAweFdXMUdhbHBYVVhwYVIxRXpUWHBCTlUxRVRtdGFhbHBvV1ZSU2FFMUhSbXhOUkdob1dtcEpkMDVSUFQwPQ=='
                    },
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).then(function (returnData) {
                    $scope.User.Resubmitting = false;
                    if (returnData.data.status == '200') {
                        setCookie("accesstoken", returnData.data.accessToken, returnData.data.expTime);
                        setCookie("refreshtoken", returnData.data.refreshToken, returnData.data.expTime);
                        setCookie("authkey", returnData.data.accessToken, returnData.data.expTime);
                        $scope.User.Email = '';
                        $scope.User.Resubmitted = true;
                        window.location.href = "/";
                    }
                    else {
                        $scope.overlay = true;
                        if (returnData.data.error == 'ERALOG01') {
                            $scope.User.Message = 'RefreshNeeded';
                        }
                        else if (returnData.data.error == 'ERALOG02') {
                                $scope.User.Message = 'InvalidInputData';
                        } //
                        else if (returnData.data.error == 'ERALOG03') {
                            $scope.User.Message = 'AlreadyRegisteredButUnconfirmed';
                            $scope.User.Resubmitted = false;
                        } //
                        else if (returnData.data.error == 'ERALOG04') {
                                $scope.User.Message = 'Failed';
                        } //
                        else if (returnData.data.error == 'ERALOG05') {
                            $scope.User.Message = 'NotRegistered';
                        } //
                        else {
                                $scope.errorMessage = 'Failed';
                        }
                        console.log('Error : ' + returnData.data.error);
                        $scope.User.Sending = false;
                    }
                }).catch(function (data) {
                    $scope.trial++;
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.User.Send();
                    }
                    else {
                        console.log('Failed requesting confirmation e-mail');
                        $scope.User.Message = 'Failed';
                    }
                });
            }
            else
            {
                $scope.User.Message = 'Failed';
            }
                
        },

        Reconfirm: function () {
            if ($scope.trial > 3) {
                $scope.trial = 0;
            }
            $scope.User.Resubmitting = true;
            var authAccess = getAuthAccess();
            if (authAccess == 1 || authAccess == 2)
            {
                $http({
                    url: ResendConfirmationEmailConfig.Url,
                    method: 'POST',
                    data: {
                        email: $scope.User.Email,
                    },
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).then(function (returnData) {
                    $scope.User.Resubmitting = false;

                    if (returnData.data.status == "200") {
                        $scope.User.Email = '';
                        $scope.User.Password = '';
                        $scope.User.Resubmitted = true;
                    }

                }).catch(function (returnData) {
                    $scope.trial++;
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.User.Reconfirm();
                    }
                    else {
                        console.log('Failed requesting confirmation e-mail');
                        $scope.submitting = false;
                        $scope.submitted = false;
                    }
                });
            }
            else
            {
                console.log('Not Authorized');
                $scope.submitting = false;
                $scope.submitted = false;
            }
                
        }
    }
}]);// Login Controller

// Register Controller
app.controller('RegisterController', ['$http', '$scope', '$rootScope', function($http, $scope, $rootScope) {

    $scope.PageConfig = $rootScope.PageConfig;
    $scope.pageLoaded = true;
    $scope.trial = 0;
    $scope.overlay = false;
    $scope.closeOverlay = function () {
        $scope.overlay = false;
        $scope.User.Sending = false;
        $scope.User.Sent = false;
    }
    $scope.validateEmail = function (email) {
        var re = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;
        return re.test(email);
    }
    $scope.User = {
        Resubmitted: false,
        Resubmitting: false,
        resubmitok: false,
        Email: '',
        Message: '',
        Invalid : false,
        Error: false,
        Registered: false,
        Newsletter: false,
        EmailSent: false,
        EmailConfirmed: false,
        Sending: false,
        Sent: false,
        Reconfirm: function () {
            if ($scope.trial > 3) {
                $scope.trial = 0;
            }
            $scope.User.Resubmitting = true;
            $http({
                url: ResendConfirmationEmailConfig.Url,
                method: 'POST',
                data: {
                    email: $scope.User.Email,
                },
                headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
            }).then(function (returnData) {
                $scope.User.Resubmitting = false;
                

                if (returnData.data.status == "200") {
                    $scope.User.Email = '';
                    $scope.User.Resubmitted = true;
                }

            }).catch(function (returnData) {
                $scope.trial++;
                if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                {
                    $scope.User.Reconfirm();
                }
                else {
                    console.log('Failed requesting confirmation e-mail');
                    $scope.submitting = false;
                    $scope.submitted = false;
                }
                
            });
        },
        Send : function() {
            if (!($scope.validateEmail($scope.User.Email))) {
                $scope.User.Email = '';
                alert('Alamat E-mail Tidak Valid');
            } else {
                if ($scope.User.Email) {
                    $scope.User.Submit();
                } else {
                    alert('Masukkan Alamat E-mail dengan Format name@domain.com');
                }
            }
        },
        Submit: function () {
            if ($scope.trial > 3) {
                $scope.trial = 0;
            }
            $scope.User.Sending = true;
            var authAccess = getAuthAccess();
            if (authAccess == 1 || authAccess == 2)
            {
                $http({
                    url: RegisterConfig.Url,
                    method: 'POST',
                    data: {
                        email: $scope.User.Email,
                    },
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).then(function (returnData) {
                    $scope.User.Sending = false;
                    $scope.User.Sent = true;
                    if (returnData.data.status == '200') {
                        $scope.User.Registered = false;
                        $scope.User.EmailSent = false;
                        $scope.User.EmailConfirmed = false;
                        $scope.User.Email = '';
                    }
                    else {
                        switch (returnData.data.error) {
                            case "ERAREG02":
                                $scope.User.Registered = true;
                                $scope.User.EmailSent = false;
                                $scope.User.EmailConfirmed = true;
                                $scope.User.Email = '';
                                break;
                            case "ERAREG03":
                                $scope.User.Registered = true;
                                $scope.User.EmailSent = true;
                                $scope.User.EmailConfirmed = false;
                                break;
                            case "ERRGEN99":
                                $scope.User.Error = true;
                                break;
                            case "ERAREG01":
                                $scope.User.Invalid = true;
                                break;
                        }
                    }

                    //
                    if ($scope.Subscribe) {
                        $scope.User.Newsletter = true;
                    } else {
                        $scope.User.Newsletter = false;
                    }

                }).catch(function (returnData) {
                    $scope.trial++;
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.User.Submit();
                    }
                    else {
                        console.log('Failed');
                        $scope.User.Sending = false;
                        $scope.User.Sent = false;
                    }
                    
                });
            }
            else
            {
                $scope.form.submitting = false;
                $scope.form.submitted = false;
            }
            
        },
        ReconfirmSent: function() {
            $scope.User.Resubmitted = false;
        }
    }

    $scope.Subscribe = function () {
        var temp = false;
        if ($scope.trial > 3) {
            $scope.trial = 0;
        }
        $http({
            url: SubscribeConfig.Url,
            method: 'POST',
            data: {
                'address': $scope.User.Email,
            },
            headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
        }).then(function (returnData) {
            $scope.User.showBox = true;
            console.log('Succeed');
            $scope.User.Sending = false;
            if (returnData == true) {
                temp = true;
            } else {
                temp = false;
            }
            //$scope.User.Sent = true;
        }).catch(function (returnData) {
            $scope.trial++;
            if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
            {
                $scope.Subscribe();
            }
            else {
                $scope.User.showBox = true;
                $scope.User.Sending = false;
                $scope.User.Sent = false;
                temp = false;
            }
            
        });

        return temp;
    }
}]);// Register Controller

// Forgot Password Controller
app.controller('ForgotpasswordController', ['$http', '$scope', '$rootScope', function ($http, $scope, $rootScope) {

    $scope.PageConfig = $rootScope.PageConfig;
    $scope.pageLoaded = true;
    $scope.trial = 0;
    $scope.overlay = false;
    $scope.closeOverlay = function() {
        $scope.overlay = false;
        $scope.EmailForm.Sending = false;
        $scope.EmailForm.Sent = false;
    }
    $scope.emailReconfirm = "";
    $scope.EmailForm = {
        Email: "",
        Email1: "",
        Resubmit: false,
        SentReconfirm: false,
        Sending: false,
        Sent: false,
        ReturnData : {
            Found: false,
            EmailConfirmed: false,
            Error: false
        },
        SendForm: function () {
            if ($scope.trial > 3) {
                $scope.trial = 0;
            }
            $scope.EmailForm.Sending = true;
            // send form
            // submit form to URL
            var authAccess = getAuthAccess();
            if (authAccess == 1 || authAccess == 2) {
                $scope.getFlightHeader = 'Bearer ' + getCookie('accesstoken');
                $scope.emailReconfirm = $scope.EmailForm.Email;
                //console.log($scope.emailReconfirm);
                $http({
                    url: ForgotPasswordConfig.Url,
                    method: 'POST',
                    data: {
                        userName: $scope.EmailForm.Email
                    },
                    headers: { 'Authorization': $scope.getFlightHeader }
                }).then(function (returnData) {
                    $scope.EmailForm.Sending = false;
                    $scope.EmailForm.Sent = true;
                    console.log(returnData);
                    if (returnData.data.status == '200') {
                        $scope.EmailForm.ReturnData.Found = true;
                        $scope.EmailForm.ReturnData.EmailConfirmed = true;
                    }
                    else {
                        switch (returnData.data.error) {
                            case "ERAFPW01":
                                $scope.EmailForm.ReturnData.Found = false;
                                $scope.EmailForm.ReturnData.EmailConfirmed = false;
                                $scope.EmailForm.Sending = false;
                                break;
                            case "ERAFPW02":
                                $scope.EmailForm.ReturnData.Found = false;
                                $scope.EmailForm.ReturnData.EmailConfirmed = true;
                                $scope.EmailForm.Sending = false;
                                break;
                            case "ERAFPW03":
                                $scope.EmailForm.ReturnData.Found = true;
                                $scope.EmailForm.ReturnData.EmailConfirmed = false;
                               
                                $scope.EmailForm.Sending = false;
                                break;
                            case "ERRGEN99":
                                $scope.EmailForm.ReturnData.Error = true;
                                break;
                        }
                    }

                }).catch(function (returnData) {
                    $scope.trial++;
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.EmailForm.SendForm();
                    }
                    else {
                        console.log('Failed requesting reset password');
                        $scope.EmailForm.Sending = false;
                        $scope.EmailForm.ReturnData.Error = true;
                    }
                    
                });
            }
            else {
                $scope.EmailForm.Sending = false;
                $scope.EmailForm.ReturnData.Error = true;
            }
            
        },
        Reconfirm: function () {
            if ($scope.trial > 3) {
                $scope.trial = 0;
            }
                $scope.EmailForm.Resubmit = true;
                var authAccess = getAuthAccess();
                //console.log("at reconfirm: " + $scope.emailReconfirm);
                if (authAccess == 1 || authAccess == 2)
                {
                    $http({
                        url: ResendConfirmationEmailConfig.Url,
                        method: 'POST',
                        data: {
                            email: $scope.emailReconfirm,
                        },
                        headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                    }).then(function (returnData) {
                        $scope.EmailForm.Resubmit = false;
                        $scope.EmailForm.SentReconfirm = true;

                        if (returnData.data.status == "200") {
                            $scope.EmailForm.Email = '';
                        }

                    }).catch(function (returnData) {
                        $scope.trial++;
                        if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                        {
                            $scope.EmailForm.Reconfirm();
                        }
                        else {
                            console.log('Failed requesting reset password');
                            $scope.EmailForm.Resubmit = false;
                            $scope.EmailForm.SentReconfirm = false;
                        }

                    });
                }
                else
                {
                    console.log('Not Authorized');
                    $scope.EmailForm.Resubmit = false;
                    $scope.EmailForm.SentReconfirm = false;
                }
                
            
        }
    };
}]);// ForgotPasswordController

app.controller('ResetpasswordController', ['$http', '$scope', '$rootScope', function ($http, $scope, $rootScope) {

        $scope.PageConfig = $rootScope.PageConfig;
        $scope.pageLoaded = true;
        $scope.trial = 0;
        $scope.form = {
            submitted: false,
            submitting: false,
            succeed: false,
            userEmail: userEmail,
            code: code,
            password: '',
            start: true,
        };
        $('.text').keyup(updateCount);
        $('.text').keydown(updateCount);

        function updateCount() {
            var cs = $(this).val().length;
            if (cs < 6) {
                $('#characters').text('Panjang password kurang dari 6 karakter');
            } else {
                $('#characters').text('');
            }
        }
        $scope.errorMessage = '',
        $scope.errorLog = '',
        $scope.submit = function () {
            if ($scope.trial > 3) {
                $scope.trial = 0;
            }
            $scope.form.submitting = true;
            var authAccess = getAuthAccess();
            if (authAccess == 2 || authAccess == 1)
            {
                $http({
                    url: ResetPasswordConfig.Url,
                    method: 'POST',
                    data: {
                        Username: $scope.form.userEmail,
                        Password: $scope.form.password,
                        Code: $scope.form.code
                    },
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).then(function (returnData) {
                    if (returnData.data.status == '200') {
                        console.log('Success requesting reset password');
                        console.log(returnData);
                        $scope.form.submitting = false;
                        $scope.form.submitted = true;
                        $scope.form.succeed = true;
                        $scope.form.start = false;
                    }
                    else {
                        switch (returnData.data.error) {
                            case 'ERARST01':
                                $scope.errorMessage = 'Pastikan Anda telah mendaftar di travorama untuk melakukan reset password ini';
                                break;
                            case 'ERARST02':
                                $scope.errorLog = 'User Not Found';
                                $scope.errorMessage = 'User ini tidak ditemukan';
                                break;
                            case 'ERARST03':
                                $scope.errorLog = 'Failed to reset password';
                                $scope.errorMessage = 'Password Anda tidak berhasil direset';
                                break;
                            case 'ERRGEN98':
                                $scope.errorLog = 'Invalid JSON Format';
                                $scope.errorMessage = 'Password Anda tidak berhasil direset';
                                break;
                            case 'ERRGEN99':
                                $scope.errorLog = 'There is a problem on the server';
                                $scope.errorMessage = 'Pastikan Anda telah mendaftar di travorama untuk melakukan reset password ini';
                                break;
                            default:
                                $scope.errorMessage = 'Password Anda tidak berhasil direset';
                        }
                        console.log(returnData.data.Description);
                        console.log(returnData);
                        $scope.form.succeed = false;
                        $scope.form.start = false;
                        $scope.form.submitted = true;
                        $scope.form.submitting = false;
                    }
                }).catch(function (returnData) {
                    $scope.trial++;
                    $scope.errorMessage = 'Mohon maaf. Password Anda tidak berhasil direset';
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.submit();
                    }
                    else {
                        console.log('Failed requesting reset password');
                        $scope.form.submitting = false;
                        $scope.form.lala = true;
                        $scope.abc = true;
                    }
                });
            }

            else
            {
                $scope.form.submitting = false;
                $scope.form.lala = true;
            }
            
        }
    }
]);

