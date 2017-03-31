app.controller('B2BUserManagementController', [
    '$http', '$log', '$scope', function ($http, $log, $scope) {

        var hash = (location.hash);
        // variables

        //General Variables
        $scope.pageLoaded = true;
        $scope.email = 'Test';
        $scope.trial = 0;
        $scope.loading = false;
        $scope.enableEdit = false;
        $scope.currentSection = '';
        $scope.profileForm = {
            active: false
        };
        $scope.userData = {
            email: '',
            name: '',
            countryCallCd: '',
            phone: '',
            position: '',
            department: '',
            branch: '',
            approverId: '',
            role: []
        }
        $scope.roleData = {
            email: '',
            role: ''
        }
        $scope.countries = [{ "name": "Afghanistan", "dial_code": "93", "code": "AF" }, { "name": "Aland Islands", "dial_code": "358", "code": "AX" }, { "name": "Albania", "dial_code": "355", "code": "AL" }, { "name": "Algeria", "dial_code": "213", "code": "DZ" }, { "name": "AmericanSamoa", "dial_code": "1 684", "code": "AS" }, { "name": "Andorra", "dial_code": "376", "code": "AD" }, { "name": "Angola", "dial_code": "244", "code": "AO" }, { "name": "Anguilla", "dial_code": "1 264", "code": "AI" }, { "name": "Antarctica", "dial_code": "672", "code": "AQ" }, { "name": "Antigua and Barbuda", "dial_code": "1268", "code": "AG" }, { "name": "Argentina", "dial_code": "54", "code": "AR" }, { "name": "Armenia", "dial_code": "374", "code": "AM" }, { "name": "Aruba", "dial_code": "297", "code": "AW" }, { "name": "Australia", "dial_code": "61", "code": "AU" }, { "name": "Austria", "dial_code": "43", "code": "AT" }, { "name": "Azerbaijan", "dial_code": "994", "code": "AZ" }, { "name": "Bahamas", "dial_code": "1 242", "code": "BS" }, { "name": "Bahrain", "dial_code": "973", "code": "BH" }, { "name": "Bangladesh", "dial_code": "880", "code": "BD" }, { "name": "Barbados", "dial_code": "1 246", "code": "BB" }, { "name": "Belarus", "dial_code": "375", "code": "BY" }, { "name": "Belgium", "dial_code": "32", "code": "BE" }, { "name": "Belize", "dial_code": "501", "code": "BZ" }, { "name": "Benin", "dial_code": "229", "code": "BJ" }, { "name": "Bermuda", "dial_code": "1 441", "code": "BM" }, { "name": "Bhutan", "dial_code": "975", "code": "BT" }, { "name": "Bolivia, Plurinational State of", "dial_code": "591", "code": "BO" }, { "name": "Bosnia and Herzegovina", "dial_code": "387", "code": "BA" }, { "name": "Botswana", "dial_code": "267", "code": "BW" }, { "name": "Brazil", "dial_code": "55", "code": "BR" }, { "name": "British Indian Ocean Territory", "dial_code": "246", "code": "IO" }, { "name": "Brunei Darussalam", "dial_code": "673", "code": "BN" }, { "name": "Bulgaria", "dial_code": "359", "code": "BG" }, { "name": "Burkina Faso", "dial_code": "226", "code": "BF" }, { "name": "Burundi", "dial_code": "257", "code": "BI" }, { "name": "Cambodia", "dial_code": "855", "code": "KH" }, { "name": "Cameroon", "dial_code": "237", "code": "CM" }, { "name": "Canada", "dial_code": "1", "code": "CA" }, { "name": "Cape Verde", "dial_code": "238", "code": "CV" }, { "name": "Cayman Islands", "dial_code": " 345", "code": "KY" }, { "name": "Central African Republic", "dial_code": "236", "code": "CF" }, { "name": "Chad", "dial_code": "235", "code": "TD" }, { "name": "Chile", "dial_code": "56", "code": "CL" }, { "name": "China", "dial_code": "86", "code": "CN" }, { "name": "Christmas Island", "dial_code": "61", "code": "CX" }, { "name": "Cocos (Keeling) Islands", "dial_code": "61", "code": "CC" }, { "name": "Colombia", "dial_code": "57", "code": "CO" }, { "name": "Comoros", "dial_code": "269", "code": "KM" }, { "name": "Congo", "dial_code": "242", "code": "CG" }, { "name": "Congo, The Democratic Republic of the Congo", "dial_code": "243", "code": "CD" }, { "name": "Cook Islands", "dial_code": "682", "code": "CK" }, { "name": "Costa Rica", "dial_code": "506", "code": "CR" }, { "name": "Cote d'Ivoire", "dial_code": "225", "code": "CI" }, { "name": "Croatia", "dial_code": "385", "code": "HR" }, { "name": "Cuba", "dial_code": "53", "code": "CU" }, { "name": "Cyprus", "dial_code": "357", "code": "CY" }, { "name": "Czech Republic", "dial_code": "420", "code": "CZ" }, { "name": "Denmark", "dial_code": "45", "code": "DK" }, { "name": "Djibouti", "dial_code": "253", "code": "DJ" }, { "name": "Dominica", "dial_code": "1 767", "code": "DM" }, { "name": "Dominican Republic", "dial_code": "1 849", "code": "DO" }, { "name": "Ecuador", "dial_code": "593", "code": "EC" }, { "name": "Egypt", "dial_code": "20", "code": "EG" }, { "name": "El Salvador", "dial_code": "503", "code": "SV" }, { "name": "Equatorial Guinea", "dial_code": "240", "code": "GQ" }, { "name": "Eritrea", "dial_code": "291", "code": "ER" }, { "name": "Estonia", "dial_code": "372", "code": "EE" }, { "name": "Ethiopia", "dial_code": "251", "code": "ET" }, { "name": "Falkland Islands (Malvinas)", "dial_code": "500", "code": "FK" }, { "name": "Faroe Islands", "dial_code": "298", "code": "FO" }, { "name": "Fiji", "dial_code": "679", "code": "FJ" }, { "name": "Finland", "dial_code": "358", "code": "FI" }, { "name": "France", "dial_code": "33", "code": "FR" }, { "name": "French Guiana", "dial_code": "594", "code": "GF" }, { "name": "French Polynesia", "dial_code": "689", "code": "PF" }, { "name": "Gabon", "dial_code": "241", "code": "GA" }, { "name": "Gambia", "dial_code": "220", "code": "GM" }, { "name": "Georgia", "dial_code": "995", "code": "GE" }, { "name": "Germany", "dial_code": "49", "code": "DE" }, { "name": "Ghana", "dial_code": "233", "code": "GH" }, { "name": "Gibraltar", "dial_code": "350", "code": "GI" }, { "name": "Greece", "dial_code": "30", "code": "GR" }, { "name": "Greenland", "dial_code": "299", "code": "GL" }, { "name": "Grenada", "dial_code": "1 473", "code": "GD" }, { "name": "Guadeloupe", "dial_code": "590", "code": "GP" }, { "name": "Guam", "dial_code": "1 671", "code": "GU" }, { "name": "Guatemala", "dial_code": "502", "code": "GT" }, { "name": "Guernsey", "dial_code": "44", "code": "GG" }, { "name": "Guinea", "dial_code": "224", "code": "GN" }, { "name": "Guinea-Bissau", "dial_code": "245", "code": "GW" }, { "name": "Guyana", "dial_code": "595", "code": "GY" }, { "name": "Haiti", "dial_code": "509", "code": "HT" }, { "name": "Holy See (Vatican City State)", "dial_code": "379", "code": "VA" }, { "name": "Honduras", "dial_code": "504", "code": "HN" }, { "name": "Hong Kong", "dial_code": "852", "code": "HK" }, { "name": "Hungary", "dial_code": "36", "code": "HU" }, { "name": "Iceland", "dial_code": "354", "code": "IS" }, { "name": "India", "dial_code": "91", "code": "IN" }, { "name": "Indonesia", "dial_code": "62", "code": "ID" }, { "name": "Iran, Islamic Republic of Persian Gulf", "dial_code": "98", "code": "IR" }, { "name": "Iraq", "dial_code": "964", "code": "IQ" }, { "name": "Ireland", "dial_code": "353", "code": "IE" }, { "name": "Isle of Man", "dial_code": "44", "code": "IM" }, { "name": "Israel", "dial_code": "972", "code": "IL" }, { "name": "Italy", "dial_code": "39", "code": "IT" }, { "name": "Jamaica", "dial_code": "1 876", "code": "JM" }, { "name": "Japan", "dial_code": "81", "code": "JP" }, { "name": "Jersey", "dial_code": "44", "code": "JE" }, { "name": "Jordan", "dial_code": "962", "code": "JO" }, { "name": "Kazakhstan", "dial_code": "7 7", "code": "KZ" }, { "name": "Kenya", "dial_code": "254", "code": "KE" }, { "name": "Kiribati", "dial_code": "686", "code": "KI" }, { "name": "Korea, Democratic People's Republic of Korea", "dial_code": "850", "code": "KP" }, { "name": "Korea, Republic of South Korea", "dial_code": "82", "code": "KR" }, { "name": "Kuwait", "dial_code": "965", "code": "KW" }, { "name": "Kyrgyzstan", "dial_code": "996", "code": "KG" }, { "name": "Laos", "dial_code": "856", "code": "LA" }, { "name": "Latvia", "dial_code": "371", "code": "LV" }, { "name": "Lebanon", "dial_code": "961", "code": "LB" }, { "name": "Lesotho", "dial_code": "266", "code": "LS" }, { "name": "Liberia", "dial_code": "231", "code": "LR" }, { "name": "Libyan Arab Jamahiriya", "dial_code": "218", "code": "LY" }, { "name": "Liechtenstein", "dial_code": "423", "code": "LI" }, { "name": "Lithuania", "dial_code": "370", "code": "LT" }, { "name": "Luxembourg", "dial_code": "352", "code": "LU" }, { "name": "Macao", "dial_code": "853", "code": "MO" }, { "name": "Macedonia", "dial_code": "389", "code": "MK" }, { "name": "Madagascar", "dial_code": "261", "code": "MG" }, { "name": "Malawi", "dial_code": "265", "code": "MW" }, { "name": "Malaysia", "dial_code": "60", "code": "MY" }, { "name": "Maldives", "dial_code": "960", "code": "MV" }, { "name": "Mali", "dial_code": "223", "code": "ML" }, { "name": "Malta", "dial_code": "356", "code": "MT" }, { "name": "Marshall Islands", "dial_code": "692", "code": "MH" }, { "name": "Martinique", "dial_code": "596", "code": "MQ" }, { "name": "Mauritania", "dial_code": "222", "code": "MR" }, { "name": "Mauritius", "dial_code": "230", "code": "MU" }, { "name": "Mayotte", "dial_code": "262", "code": "YT" }, { "name": "Mexico", "dial_code": "52", "code": "MX" }, { "name": "Micronesia, Federated States of Micronesia", "dial_code": "691", "code": "FM" }, { "name": "Moldova", "dial_code": "373", "code": "MD" }, { "name": "Monaco", "dial_code": "377", "code": "MC" }, { "name": "Mongolia", "dial_code": "976", "code": "MN" }, { "name": "Montenegro", "dial_code": "382", "code": "ME" }, { "name": "Montserrat", "dial_code": "1664", "code": "MS" }, { "name": "Morocco", "dial_code": "212", "code": "MA" }, { "name": "Mozambique", "dial_code": "258", "code": "MZ" }, { "name": "Myanmar", "dial_code": "95", "code": "MM" }, { "name": "Namibia", "dial_code": "264", "code": "NA" }, { "name": "Nauru", "dial_code": "674", "code": "NR" }, { "name": "Nepal", "dial_code": "977", "code": "NP" }, { "name": "Netherlands", "dial_code": "31", "code": "NL" }, { "name": "Netherlands Antilles", "dial_code": "599", "code": "AN" }, { "name": "New Caledonia", "dial_code": "687", "code": "NC" }, { "name": "New Zealand", "dial_code": "64", "code": "NZ" }, { "name": "Nicaragua", "dial_code": "505", "code": "NI" }, { "name": "Niger", "dial_code": "227", "code": "NE" }, { "name": "Nigeria", "dial_code": "234", "code": "NG" }, { "name": "Niue", "dial_code": "683", "code": "NU" }, { "name": "Norfolk Island", "dial_code": "672", "code": "NF" }, { "name": "Northern Mariana Islands", "dial_code": "1 670", "code": "MP" }, { "name": "Norway", "dial_code": "47", "code": "NO" }, { "name": "Oman", "dial_code": "968", "code": "OM" }, { "name": "Pakistan", "dial_code": "92", "code": "PK" }, { "name": "Palau", "dial_code": "680", "code": "PW" }, { "name": "Palestinian Territory, Occupied", "dial_code": "970", "code": "PS" }, { "name": "Panama", "dial_code": "507", "code": "PA" }, { "name": "Papua New Guinea", "dial_code": "675", "code": "PG" }, { "name": "Paraguay", "dial_code": "595", "code": "PY" }, { "name": "Peru", "dial_code": "51", "code": "PE" }, { "name": "Philippines", "dial_code": "63", "code": "PH" }, { "name": "Pitcairn", "dial_code": "872", "code": "PN" }, { "name": "Poland", "dial_code": "48", "code": "PL" }, { "name": "Portugal", "dial_code": "351", "code": "PT" }, { "name": "Puerto Rico", "dial_code": "1 939", "code": "PR" }, { "name": "Qatar", "dial_code": "974", "code": "QA" }, { "name": "Romania", "dial_code": "40", "code": "RO" }, { "name": "Russia", "dial_code": "7", "code": "RU" }, { "name": "Rwanda", "dial_code": "250", "code": "RW" }, { "name": "Reunion", "dial_code": "262", "code": "RE" }, { "name": "Saint Barthelemy", "dial_code": "590", "code": "BL" }, { "name": "Saint Helena, Ascension and Tristan Da Cunha", "dial_code": "290", "code": "SH" }, { "name": "Saint Kitts and Nevis", "dial_code": "1 869", "code": "KN" }, { "name": "Saint Lucia", "dial_code": "1 758", "code": "LC" }, { "name": "Saint Martin", "dial_code": "590", "code": "MF" }, { "name": "Saint Pierre and Miquelon", "dial_code": "508", "code": "PM" }, { "name": "Saint Vincent and the Grenadines", "dial_code": "1 784", "code": "VC" }, { "name": "Samoa", "dial_code": "685", "code": "WS" }, { "name": "San Marino", "dial_code": "378", "code": "SM" }, { "name": "Sao Tome and Principe", "dial_code": "239", "code": "ST" }, { "name": "Saudi Arabia", "dial_code": "966", "code": "SA" }, { "name": "Senegal", "dial_code": "221", "code": "SN" }, { "name": "Serbia", "dial_code": "381", "code": "RS" }, { "name": "Seychelles", "dial_code": "248", "code": "SC" }, { "name": "Sierra Leone", "dial_code": "232", "code": "SL" }, { "name": "Singapore", "dial_code": "65", "code": "SG" }, { "name": "Slovakia", "dial_code": "421", "code": "SK" }, { "name": "Slovenia", "dial_code": "386", "code": "SI" }, { "name": "Solomon Islands", "dial_code": "677", "code": "SB" }, { "name": "Somalia", "dial_code": "252", "code": "SO" }, { "name": "South Africa", "dial_code": "27", "code": "ZA" }, { "name": "South Georgia and the South Sandwich Islands", "dial_code": "500", "code": "GS" }, { "name": "Spain", "dial_code": "34", "code": "ES" }, { "name": "Sri Lanka", "dial_code": "94", "code": "LK" }, { "name": "Sudan", "dial_code": "249", "code": "SD" }, { "name": "Suriname", "dial_code": "597", "code": "SR" }, { "name": "Svalbard and Jan Mayen", "dial_code": "47", "code": "SJ" }, { "name": "Swaziland", "dial_code": "268", "code": "SZ" }, { "name": "Sweden", "dial_code": "46", "code": "SE" }, { "name": "Switzerland", "dial_code": "41", "code": "CH" }, { "name": "Syrian Arab Republic", "dial_code": "963", "code": "SY" }, { "name": "Taiwan", "dial_code": "886", "code": "TW" }, { "name": "Tajikistan", "dial_code": "992", "code": "TJ" }, { "name": "Tanzania, United Republic of Tanzania", "dial_code": "255", "code": "TZ" }, { "name": "Thailand", "dial_code": "66", "code": "TH" }, { "name": "Timor-Leste", "dial_code": "670", "code": "TL" }, { "name": "Togo", "dial_code": "228", "code": "TG" }, { "name": "Tokelau", "dial_code": "690", "code": "TK" }, { "name": "Tonga", "dial_code": "676", "code": "TO" }, { "name": "Trinidad and Tobago", "dial_code": "1 868", "code": "TT" }, { "name": "Tunisia", "dial_code": "216", "code": "TN" }, { "name": "Turkey", "dial_code": "90", "code": "TR" }, { "name": "Turkmenistan", "dial_code": "993", "code": "TM" }, { "name": "Turks and Caicos Islands", "dial_code": "1 649", "code": "TC" }, { "name": "Tuvalu", "dial_code": "688", "code": "TV" }, { "name": "Uganda", "dial_code": "256", "code": "UG" }, { "name": "Ukraine", "dial_code": "380", "code": "UA" }, { "name": "United Arab Emirates", "dial_code": "971", "code": "AE" }, { "name": "United Kingdom", "dial_code": "44", "code": "GB" }, { "name": "United States", "dial_code": "1", "code": "US" }, { "name": "Uruguay", "dial_code": "598", "code": "UY" }, { "name": "Uzbekistan", "dial_code": "998", "code": "UZ" }, { "name": "Vanuatu", "dial_code": "678", "code": "VU" }, { "name": "Venezuela, Bolivarian Republic of Venezuela", "dial_code": "58", "code": "VE" }, { "name": "Vietnam", "dial_code": "84", "code": "VN" }, { "name": "Virgin Islands, British", "dial_code": "1 284", "code": "VG" }, { "name": "Virgin Islands, U.S.", "dial_code": "1 340", "code": "VI" }, { "name": "Wallis and Futuna", "dial_code": "681", "code": "WF" }, { "name": "Yemen", "dial_code": "967", "code": "YE" }, { "name": "Zambia", "dial_code": "260", "code": "ZM" }, { "name": "Zimbabwe", "dial_code": "263", "code": "ZW" }];

        $scope.getCountries = function (dialCode) {
            for (var i = 0; i < $scope.countries.length; i++) {
                if ($scope.countries[i].dial_code == dialCode) {
                    return $scope.countries[i].name;
                }
            }
        }

        $scope.editUser = {
            email: '',
            name: '',
            countryCallCd: '',
            phone: '',
            department: '',
            branch: '',
            position: '',
            approval: '',
            role:[],
            edit: false,
            updating: false
        }

        $scope.roles = roles;
        if ($scope.roles != null & $scope.roles.length > 0 && $scope.roles.indexOf("Customer") > -1) {
            $scope.roles.splice($scope.roles.indexOf("Customer"), 1);
        }
        $scope.users = users;
        $scope.approvers = approvers;
        $scope.branches = branches;
        $scope.departments = departments;
        $scope.positions = positions;
        $scope.userNames = names;

        var substringMatcher = function (strs) {
            return function findMatches(q, cb) {
                var matches, substrRegex;

                // an array that will be populated with substring matches
                matches = [];

                // regex used to determine if a string contains the substring `q`
                substrRegex = new RegExp(q, 'i');
                $.each(strs, function (i, str) {
                    if (substrRegex.test(str)) {
                        matches.push(str);
                    }
                });

                cb(matches);
            };
        };
     
        //$('#searchName').typeahead({
        //    hint: true,
        //    highlight: true,
        //    minLength: 1
        //}, {
        //    name: 'userNames',
        //    source: substringMatcher($scope.userNames),
        //});

        //$("#searchName").select2({
        //    //placeholder: "Select a type",
        //    data: [{
        //        id: 0,
        //        text: 'enhancement'
        //    }, {
        //        id: 1,
        //        text: ''
        //    }, {
        //        id: 2,
        //        text: 'duplicate'
        //    }]
        //});

        //$('#searchName').on('typeahead:selected', function(evt, item) {
        //    $scope.userFilter.name = item;
        //});

        //$('#searchPosition').typeahead({
        //    hint: true,
        //    highlight: true,
        //    minLength: 1
        //}, {
        //    name: 'positions',
        //    source: substringMatcher($scope.positions)
        //});

        //$('#searchPosition').on('typeahead:selected', function (evt, item) {
        //    $scope.userFilter.position = item;
        //});

        //$('#searchBranch').typeahead({
        //    hint: true,
        //    highlight: true,
        //    minLength: 1
        //}, {
        //    name: 'branches',
        //    source: substringMatcher($scope.branches)
        //});

        //$('#searchBranch').on('typeahead:selected', function (evt, item) {
        //    $scope.userFilter.branch = item;
        //});

        //$('#searchDepartment').typeahead({
        //    hint: true,
        //    highlight: true,
        //    minLength: 1
        //}, {
        //    name: 'departments',
        //    source: substringMatcher($scope.departments)
        //});

        //$('#searchDepartment').on('typeahead:selected', function (evt, item) {
        //    $scope.userFilter.department = item;
        //});

        $scope.userNames = [];
        $scope.userPositions = [];
        $scope.showListApproverAdd = false;
        $scope.showListApproverEdit = false;
        $scope.selectedRole = [];
        $scope.currentEditIndex = 0;
        $scope.EditUserProfile = function (index) {
            $scope.currentEditIndex = index;
            $scope.editUser.email = $scope.users[index].email;
            
            //$scope.editUser.userId = $scope.users[index].userId;
            if ($scope.users[index].firstName == null) {
                $scope.users[index].firstName = "";
            }

            if ($scope.users[index].lastName == null) {
                $scope.users[index].lastName = "";
            }

            if ($scope.users[index].firstName == "" && $scope.users[index].lastName == "") {
                $scope.editUser.name = "";
            } else {
                $scope.editUser.name = $scope.users[index].firstName + " " + $scope.users[index].lastName;
            }
            
            $scope.editUser.countryCallCd = $scope.users[index].countryCallCd;
            $scope.editUser.phone = parseInt($scope.users[index].phoneNumber);
            $scope.editUser.department = $scope.users[index].department;
            $scope.editUser.branch = $scope.users[index].branch;
            $scope.editUser.position = $scope.users[index].position;
            $scope.editUser.approverId = $scope.users[index].approverId;
            $scope.editUser.approver = $scope.users[index].approverName;
            $scope.editUser.role = $scope.users[index].roleName;
            $scope.selectedRole = [];
            for (var k = 0; k < $scope.users[index].roleName.length; k++) {
                $scope.selectedRole.push($scope.users[index].roleName[k]);
                //$scope.selectRoleUpdate($scope.users[index].roleName[i]);
            }
            $('#edit-user input').prop("checked", false);
            for (var i = 0; i < $scope.users[index].roleName.length; i++) {
                $('#edit-user input#edit-' + $scope.users[index].roleName[i]).prop("checked", true);
                
            }

            if ($scope.users[index].roleName.indexOf('Booker') < 0) {
                $scope.showListApproverEdit = false;
            } else {
                $scope.showListApproverEdit = true;
                $scope.approverExc = [];
                for (var i = 0; i < $scope.approvers.length; i++) {
                    if ($scope.approvers[i].name != $scope.editUser.name) {
                        $scope.approverExc.push({
                            name: $scope.approvers[i].name,
                            userId: $scope.approvers[i].userId
                        });
                    }
                }
            }
            $('#edit-user').modal('show');
            
        }

        $scope.UpdateUserLock = function (userId, status, index) {
            $('.wait').modal({
                backdrop: 'static',
                keyboard: false
            });
            if (status == "LOCK") {
                $scope.lockUser = true;
            } else {
                $scope.lockUser = false;
            }
            $scope.lockUpdated = false;
            var authAccess = getAuthAccess();
            if (authAccess == 2) {
                //authorized
                $http({
                    url: UpdateUserLockConfig.Url,
                    method: 'POST',
                    data: {
                        userId: userId,
                        IsLocked: $scope.lockUser
                    },
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).then(function (returnData) {
                    //$log.debug(returnData);
                    $(".wait").modal("hide");
                    if (returnData.data.status == '200') {
                        $log.debug('Success updating user Lock');
                        $scope.lockUpdated = true;
                        if (status == "LOCK") {
                            $scope.users[index].isLocked = true;
                        } else {
                            $scope.users[index].isLocked = false;
                        }
                        $(".lockSucceed").modal({
                            backdrop: 'static',
                        });
                    }
                    else {
                        $log.debug(returnData.data.error);
                        $log.debug(returnData);
                        $scope.lockUpdated = false;
                        $(".lockFailed").modal({
                            backdrop: 'static',
                        });
                    }
                }).catch(function (returnData) {
                    $scope.trial++;
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.UpdateUserLock(userId, status);
                    }
                    else {
                        $log.debug('Failed Update User Lock');
                        $log.debug(returnData);
                        $scope.lockUpdated = false;
                        $(".lockFailed").modal({
                            backdrop: 'static',
                        });
                    }
                });
            }
            else { //if not authorized
                $scope.lockUpdated = false;
                $(".wait").modal("hide");
            }
        }

        $scope.userFilter = {
            roles: [],
            name: '',
            email: '',
            position: '',
            department: '',
            branch: '',
        }

        
        $scope.sortByType = {
            ascendingName: "ascendingname", descendingName: "descendingname",
            ascendingPosition: "ascendingposition", descendingPosition: "descendingposition",
            ascendingDepartment: "ascendingdepartment", descendingDepartment: "descendingdepartment",
            ascendingBranch: "ascendingbranch", descendingBranch: "descendingbranch",
            ascendingEmail: "ascendingemail", descendingEmail: "descendingemail",
        }

        $scope.userSorting = $scope.sortByType.ascendingName;
        //Get User
        $scope.User = {
            Reset: function() {
                $scope.userFilter.roles = [];
                $scope.userFilter.name = '';
                $scope.userFilter.email = '';
                $scope.userFilter.department = '';
                $scope.userFilter.branch = '';
                $scope.userFilter.position = '';
                $("#searchName").val('');
                $("#searchPosition").val('');
                $("#searchBranch").val('');
                $("#searchDepartment").val('');
                $scope.User.GetUser();
            },
            GetUser: function () {
                $('#searchName').val($scope.userFilter.name);
                $('#searchPosition').val($scope.userFilter.position);
                $('#searchBranch').val($scope.userFilter.branch);
                $('#searchDepartment').val($scope.userFilter.department);

                $('.wait').modal({
                    backdrop: 'static',
                    keyboard: false
                });

                
                if ($scope.trial > 3) {
                    $scope.trial = 0;
                }
                var authAccess = getAuthAccess();
                if (authAccess == 2) {
                    $http({
                        method: 'POST',
                        url: GetUserConfig.Url,
                        async: false,
                        data: {
                          "userFilter" : {
                              "roles": $scope.userFilter.roles,
                              "name": $scope.userFilter.name,
                              "email": $scope.userFilter.email,
                              "department": $scope.userFilter.department,
                              "branch": $scope.userFilter.branch,
                              "position": $scope.userFilter.position
                          },
                            "userSorting": $scope.userSorting
                        },
                        headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                    }).then(function (returnData) {
                        $(".wait").modal("hide");
                        if (returnData.status == "200") {
                            $log.debug('Success getting All Users');
                            //$scope.roles = returnData.data.roles;
                            $scope.users = returnData.data.users;
                            //$scope.approvers = returnData.data.approvers;
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
                            $scope.User.GetUser();
                        }
                        else {
                            $log.debug('Failed to Get Profile');
                        }
                    });
                }
                else {
                    $(".wait").modal("hide");
                    $log.debug('Not Authorized');
                }
            }
        }

        //Update Role

        $scope.setUpdateRole = function () {
            $scope.enableEdit = true;
        }

        $scope.cancelUpdate = function () {
            $scope.enableEdit = false;
        }


        $scope.updateRole = function (email) {
            $('.wait').modal({
                backdrop: 'static',
                keyboard: false
            });
            $scope.roleUpdated = false;
            var authAccess = getAuthAccess();
            if (authAccess == 2) {
                //authorized
                $http({
                    url: UpdateRoleConfig.Url,
                    method: 'POST',
                    data: {
                        userName: email,
                        role: $scope.roleData.role
                    },
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).then(function (returnData) {
                    $(".wait").modal("hide");
                    if (returnData.data.status == '200') {
                        $log.debug('Success updating Profile');
                        $scope.roleUpdated = true;
                        $(".updateSucceed").modal({
                            backdrop: 'static',
                        });
                    }
                    else {
                        $log.debug(returnData.data.error);
                        $log.debug(returnData);
                        $scope.roleUpdated = false;
                        $(".updateFailed").modal({
                            backdrop: 'static',
                        });
                    }
                }).catch(function (returnData) {
                    $scope.trial++;
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.updateRole(email, role);
                    }
                    else {
                        $log.debug('Failed Update Role');
                        $log.debug(returnData);
                        $scope.roleUpdated = false;
                        $(".updateFailed").modal({
                            backdrop: 'static',
                        });
                    }
                });
            }
            else { //if not authorized
                $(".wait").modal("hide");
                $scope.roleUpdated = false;
            }
        }

        $scope.closePopUp = function () {
            $scope.userAdded = false;
            $scope.roleUpdated = false;
            $scope.userDeleted = false;
            window.location.reload();
        }

        $(".modal#add-user").on('show.bs.modal', function () {
            $scope.showListApproverAdd = false;
            $scope.userData.email = '';
            $scope.userData.name = '';
            $scope.userData.countryCallCd = '';
            $scope.userData.phone = '';
            $scope.userData.position = '';
            $scope.userData.department = '';
            $scope.userData.branch = '';
            $scope.userData.approverId = '';
            $scope.userData.role = [];
            $(".modal#add-user input").prop("checked", false);
            $(".modal#add-user input").val('');
            $('.modal#add-user select').val('');
            $('#approverAdd').hide();

        });

        //Add User
        $scope.addUser = function () {
            $('.wait').modal({
                backdrop: 'static',
                keyboard: false
            });
            $scope.userAdded = false;
            //$scope.userData.role = $scope.selectedRole;
            var authAccess = getAuthAccess();
            var data;
            if ($scope.userData.role.indexOf("Booker") > -1) {
                if ($scope.userData.approverId == null || $scope.userData.approverId.length == 0) {
                    $('.mustHaveApprover').modal({
                        backdrop: 'static',
                        keyboard: false
                    });
                    $('.wait').modal("hide");
                    return;
                } else {
                    data = {
                        email: $scope.userData.email,
                        name: $scope.userData.name,
                        countryCallCd: $scope.userData.countryCallCd,
                        phone: $scope.userData.phone,
                        position: $scope.userData.position,
                        department: $scope.userData.department,
                        branch: $scope.userData.branch,
                        approverId: $scope.userData.approverId,
                        roles: $scope.userData.role
                    }
                }                
            } else {
                data = {
                    email: $scope.userData.email,
                    name: $scope.userData.name,
                    countryCallCd: $scope.userData.countryCallCd,
                    phone: $scope.userData.phone,
                    position: $scope.userData.position,
                    department: $scope.userData.department,
                    branch: $scope.userData.branch,
                    roles: $scope.userData.role
                }
            }
            if (authAccess == 2) {
                //authorized
                $http({
                    url: AddUserConfig.Url,
                    method: 'POST',
                    data: data,
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).then(function (returnData) {
                    $(".wait").modal("hide");
                    if (returnData.data.status == '200') {
                        $log.debug('Success Add User');
                        var roles = [];
                        if ($scope.userData.role.length > 0) {
                            for (var i = 0; i < $scope.userData.role.length; i++) {
                                roles.push($scope.userData.role[i]);
                                if ($scope.userData.role[i] == 'Approver') {
                                    $scope.approvers.push({
                                        userId: returnData.data.userId,
                                        name: returnData.data.name
                                    });
                                }
                            }
                        }
                        var splittedName = $scope.userData.name.split(' ');
                        var approverName;
                        if (roles.indexOf("Booker") == -1) {
                            approverName = "";
                        } else {
                            var approvers = $.grep($scope.approvers, function(e) { return e.userId == $scope.userData.approverId; });
                            if (approvers == null || approvers.length == 0) {
                                approverName = "";
                            } else {
                                approverName = approvers[0].name;
                            }
                        }
                        $scope.userAdded = true;
                        $scope.users.push({
                            email: $scope.userData.email,
                            firstName: splittedName[0],
                            lastName: splittedName.slice(1, splittedName.length).join(' '),
                            branch: $scope.userData.branch,
                            position: $scope.userData.position,
                            approverId: $scope.userData.approverId,
                            roleName:roles,
                            department: $scope.userData.department,
                            approverName: approverName,
                            phoneNumber: $scope.userData.phone,
                            countryCallCd: $scope.userData.countryCallCd
                        });
                        $scope.userNames.push($scope.userData.name);

                        var foundPosition = false;
                        var foundBranch = false;
                        var foundDepartment = false;
                        
                        for (var i = 0; i < $scope.positions.length; i++) {
                            if ($scope.positions[i] != null) {
                                if ($scope.positions[i].toLowerCase() == $scope.userData.position.toLowerCase()) {
                                    foundPosition = true;
                                    break;
                                }
                            }
                            
                        }

                        if (foundPosition) {
                            $scope.positions.push($scope.userData.position);
                        }

                        for (var i = 0; i < $scope.branches.length; i++) {
                            if ($scope.branches[i] != null) {
                                if ($scope.branches[i].toLowerCase() == $scope.userData.branch.toLowerCase()) {
                                    foundBranch = true;
                                    break;
                                }
                            }
                            
                        }

                        if (foundBranch) {
                            $scope.branches.push($scope.userData.branch);
                        }

                        for (var i = 0; i < $scope.departments.length; i++) {
                            if ($scope.departments[i] != null) {
                                if ($scope.departments[i].toLowerCase() == $scope.userData.department.toLowerCase()) {
                                    foundDepartment = true;
                                    break;
                                }
                            }
                            
                        }

                        if (foundDepartment) {
                            $scope.departments.push($scope.userData.department);
                        }
                        
                        $(".addSucceed").modal({
                            backdrop: 'static',
                        });
                    }
                    else {
                        $log.debug(returnData.data.error);
                        $log.debug(returnData);
                        $scope.userAdded = false;
                        $(".addFailed").modal({
                            backdrop: 'static',
                        });
                    }
                }).catch(function (returnData) {
                    $scope.trial++;
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.addUser();
                    }
                    else {
                        $log.debug('Failed Add User');
                        $log.debug(returnData);
                        $scope.userAdded = false;
                        $(".addFailed").modal({
                            backdrop: 'static',
                        });
                    }
                });
            }
            else { //if not authorized
                $scope.userAdded = false;
            }
        }

        $scope.updateUser = function () {
            $(".modal#edit-user").modal("hide");
            $('.wait').modal({
                backdrop: 'static',
                keyboard: false
            });
            $scope.updatingUser = false;
            $scope.editUser.role = $scope.selectedRole;
            var data;
            if ($scope.editUser.role.indexOf("Booker") > -1) {
                if ($scope.editUser.approverId == null || $scope.editUser.approverId.length == 0) {
                    $('.mustHaveApprover').modal({
                        backdrop: 'static',
                        keyboard: false
                    });
                    $('.wait').modal("hide");
                    return;
                } else {
                    data = {
                        email: $scope.editUser.email,
                        name: $scope.editUser.name,
                        countryCallCd: $scope.editUser.countryCallCd,
                        phone: $scope.editUser.phone,
                        position: $scope.editUser.position,
                        department: $scope.editUser.department,
                        branch: $scope.editUser.branch,
                        approverId: $scope.editUser.approverId,
                        roles: $scope.editUser.role
                    }
                }
                
            } else {
                data = {
                    email: $scope.editUser.email,
                    name: $scope.editUser.name,
                    countryCallCd: $scope.editUser.countryCallCd,
                    phone: $scope.editUser.phone,
                    position: $scope.editUser.position,
                    department: $scope.editUser.department,
                    branch: $scope.editUser.branch,
                    roles: $scope.editUser.role
                }
            }
            var authAccess = getAuthAccess();
            if (authAccess == 2) {
                //authorized
                $http({
                    url: UpdateUserConfig.Url,
                    method: 'POST',
                    data: data,
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).then(function (returnData) {
                    $(".wait").modal("hide");
                    if (returnData.data.status == '200') {
                        $log.debug('Success Update User');
                        $scope.updatingUser = true;
                        //window.location.reload();
                        $(".updateSucceed").modal({
                            backdrop: 'static',
                        });

                        var splittedName = $scope.editUser.name.split(' ');
                        $scope.users[$scope.currentEditIndex].email = $scope.editUser.email;
                        $scope.users[$scope.currentEditIndex].firstName = splittedName[0];
                        $scope.users[$scope.currentEditIndex].lastName = splittedName.slice(1, splittedName.length).join(' ');
                        $scope.users[$scope.currentEditIndex].branch = $scope.editUser.branch;
                        $scope.users[$scope.currentEditIndex].position = $scope.editUser.position;
                        $scope.users[$scope.currentEditIndex].department = $scope.editUser.department;
                        $scope.users[$scope.currentEditIndex].countryCallCd = $scope.editUser.countryCallCd;
                        $scope.users[$scope.currentEditIndex].phoneNumber = $scope.editUser.phone;


                        if ($scope.editUser.role != null && $scope.editUser.role.length > 0) {
                            $scope.users[$scope.currentEditIndex].roleName = $scope.editUser.role;
                        }
                        
                        $scope.users[$scope.currentEditIndex].approverId = $scope.editUser.approverId;
                        //$.grep(myArray, function(e){ return e.id == id; });
                        $scope.users[$scope.currentEditIndex].approverName =
                            $.grep($scope.approvers, function (e) { return e.userId == $scope.editUser.approverId; })[0].name;
                    }
                    else {
                        $log.debug(returnData.data.error);
                        $log.debug(returnData);
                        $scope.updatingUser = false;
                        $(".updateFailed").modal({
                            backdrop: 'static',
                        });
                    }
                }).catch(function (returnData) {
                    $scope.trial++;
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.updateUser();
                    }
                    else {
                        $log.debug('Failed Update User');
                        $log.debug(returnData);
                        $scope.updatingUser = false;
                        $(".updateFailed").modal({
                            backdrop: 'static',
                        });
                    }
                });
            }
            else { //if not authorized
                $(".wait").modal("hide");
                $scope.updatingUser = false;
            }
        }

        //Delete User
        $scope.deleteUser = function (email) {
            $scope.userDeleted = false;
            var authAccess = getAuthAccess();
            if (authAccess == 2) {
                //authorized
                $http({
                    url: DeleteUserConfig.Url,
                    method: 'POST',
                    data: {
                        email: email
                    },
                    headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                }).then(function(returnData) {
                    if (returnData.data.status == '200') {
                        $log.debug('Success Add User');
                        $scope.userDeleted = true;
                    } else {
                        $log.debug(returnData.data.error);
                        $log.debug(returnData);
                        $scope.userDeleted = false;
                        window.location.reload();
                    }
                }).catch(function(returnData) {
                    $scope.trial++;
                    if (refreshAuthAccess() && $scope.trial < 4) //refresh cookie
                    {
                        $scope.deleteUser();
                    } else {
                        $log.debug('Failed Add User');
                        $log.debug(returnData);
                        $scope.userDeleted = false;
                    }
                })
            } else { //if not authorized
                $scope.userDeleted = false;
            }
        }

        //Executing Get User
        //$scope.User.GetUser();

        $scope.changeSection = function (name) {
            $scope.currentSection = name;
        }

        if (hash == '#order') {
            $scope.changeSection('order');
        } else {
            $scope.changeSection('profile');
        }

        //Filter and Sort User
        $scope.inputFilterByRole = function(value) {
            if ($scope.userFilter.roles == null || $scope.userFilter.roles.length == 0) {
                $scope.userFilter.roles = [value];
            }
            else {
                var roleFilterIndex = $scope.userFilter.roles.indexOf(value);
                if (roleFilterIndex < 0) {
                    $scope.userFilter.roles.push(value);
                }
                else {
                    $scope.userFilter.roles.splice(roleFilterIndex, 1);
                }
            }
        }

        $scope.approverExc = [];
        $scope.selectRoleUpdate = function (value) {
            if ($scope.selectedRole == null || $scope.selectedRole.length == 0) {
                $scope.selectedRole = [value];
                if (value == 'Booker') {
                    $scope.showListApproverEdit = true;
                    $scope.approverExc = [];
                    for (var i = 0; i < $scope.approvers.length; i++) {
                        if ($scope.approvers[i].name != $scope.editUser.name) {
                            $scope.approverExc.push({
                                name: $scope.approvers[i].name,
                                userId: $scope.approvers[i].userId
                            });
                        }
                    }
                } else {
                    $scope.showListApproverEdit = false;
                }
            }
            else {
                var roleFilterIndex = $scope.selectedRole.indexOf(value);
                if (roleFilterIndex < 0) {
                    $scope.selectedRole.push(value);
                    if (value == 'Booker') {
                        $scope.showListApproverEdit = true;
                        $scope.approverExc = [];
                        for (var i = 0; i < $scope.approvers.length; i++) {
                            if ($scope.approvers[i].name != $scope.editUser.name) {
                                $scope.approverExc.push({
                                    name: $scope.approvers[i].name,
                                    userId: $scope.approvers[i].userId
                                });
                            }
                        }
                    }
                }
                else {
                    if (value == 'Booker') {
                        $scope.showListApproverEdit = false;
                    }
                    $scope.selectedRole.splice(roleFilterIndex, 1);
                }
            }
        }

        $scope.selectRoleAdd = function (value) {
            if ($scope.userData.role == null || $scope.userData.role.length == 0) {
                $scope.userData.role = [value];
                if (value == 'Booker') {
                    $scope.showListApproverAdd = true;
                    $('#approverAdd').show();
                } else {
                    $scope.showListApproverAdd = false;
                }
            }
            else {
                var roleFilterIndex = $scope.userData.role.indexOf(value);
                if (roleFilterIndex < 0) {
                    $scope.userData.role.push(value);
                    if (value == 'Booker') {
                        $scope.showListApproverAdd = true;
                        $('#approverAdd').show();
                    }
                }
                else {
                    if (value == 'Booker') {
                        $scope.showListApproverAdd = false;
                    }
                    $scope.userData.role.splice(roleFilterIndex, 1);
                }
            }
        }

        $("input#checkbox-booker").change(function () {
            $scope.inputFilterByRole("Booker");
        });

        $("input#checkbox-approver").change(function () {
            $scope.inputFilterByRole("Approver");
        });

        $("input#checkbox-finance").change(function () {
            $scope.inputFilterByRole("Finance");
        });

        $("input#checkbox-sa").change(function () {
            $scope.inputFilterByRole("Admin");
        });

        $("li#sort-name").click(function() {
            $("#selectedSort").text("Nama");
            $scope.userSorting = $scope.sortByType.ascendingName;
            $scope.User.GetUser();
        });

        $("li#sort-email").click(function () {
            $("#selectedSort").text("Alamat Email");
            $scope.userSorting = $scope.sortByType.ascendingEmail;
            $scope.User.GetUser();
        });

        $("li#sort-position").click(function () {
            $("#selectedSort").text("Jabatan");
            $scope.userSorting = $scope.sortByType.ascendingPosition;
            $scope.User.GetUser();
        });

        $("li#sort-branch").click(function () {
            $("#selectedSort").text("Cabang");
            $scope.userSorting = $scope.sortByType.ascendingBranch;
            $scope.User.GetUser();
        });

        $("li#sort-department").click(function () {
            $("#selectedSort").text("Departemen");
            $scope.userSorting = $scope.sortByType.ascendingDepartment;
            $scope.User.GetUser();
        });
    }
]);