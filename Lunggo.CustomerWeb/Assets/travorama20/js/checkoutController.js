// travorama angular app - checkout controller
app.controller('checkoutController', [
    '$http', '$scope', function($http, $scope) {

        //********************
        // variables
        $scope.currentPage = 1;
        $scope.loginShown = false;
        $scope.stepClass = '';
        $scope.titles = [
            { name: 'Mr', value: 'mr' },
            { name: 'Mrs', value: 'mrs' },
            { name: 'Ms', value: 'ms' }
        ];
        $scope.childTitle = [
            { name: 'Mr', value: 'mr' },
            { name: 'Ms', value: 'ms' }
        ];

        $scope.passengers = [];

        $scope.buyerInfo = {};
        $scope.adultPassenger = [];
        $scope.childPassenger = [];
        $scope.infantPassenger = [];

        $scope.passportRequired = passportRequired;
        $scope.idRequired = idRequired;
        $scope.nationalityRequired = nationalityRequired;

        $scope.months = [
            { name: 'January', value: 1 },
            { name: 'February', value: 2 },
            { name: 'March', value: 3 },
            { name: 'April', value: 4 },
            { name: 'May', value: 5 },
            { name: 'June', value: 6 },
            { name: 'July', value: 7 },
            { name: 'August', value: 8 },
            { name: 'September', value: 9 },
            { name: 'October', value: 10 },
            { name: 'November', value: 11 },
            { name: 'December', value: 12 }
        ];

        $scope.countries = ["Afghanistan", "Albania", "Algeria", "Andorra", "Angola", "Anguilla", "Antigua &amp; Barbuda", "Argentina", "Armenia", "Aruba", "Australia", "Austria", "Azerbaijan", "Bahamas", "Bahrain", "Bangladesh", "Barbados", "Belarus", "Belgium", "Belize", "Benin", "Bermuda", "Bhutan", "Bolivia", "Bosnia &amp; Herzegovina", "Botswana", "Brazil", "British Virgin Islands", "Brunei", "Bulgaria", "Burkina Faso", "Burundi", "Cambodia", "Cameroon", "Cape Verde", "Cayman Islands", "Chad", "Chile", "China", "Colombia", "Congo", "Cook Islands", "Costa Rica", "Cote D Ivoire", "Croatia", "Cruise Ship", "Cuba", "Cyprus", "Czech Republic", "Denmark", "Djibouti", "Dominica", "Dominican Republic", "Ecuador", "Egypt", "El Salvador", "Equatorial Guinea", "Estonia", "Ethiopia", "Falkland Islands", "Faroe Islands", "Fiji", "Finland", "France", "French Polynesia", "French West Indies", "Gabon", "Gambia", "Georgia", "Germany", "Ghana", "Gibraltar", "Greece", "Greenland", "Grenada", "Guam", "Guatemala", "Guernsey", "Guinea", "Guinea Bissau", "Guyana", "Haiti", "Honduras", "Hong Kong", "Hungary", "Iceland", "India", "Indonesia", "Iran", "Iraq", "Ireland", "Isle of Man", "Israel", "Italy", "Jamaica", "Japan", "Jersey", "Jordan", "Kazakhstan", "Kenya", "Kuwait", "Kyrgyz Republic", "Laos", "Latvia", "Lebanon", "Lesotho", "Liberia", "Libya", "Liechtenstein", "Lithuania", "Luxembourg", "Macau", "Macedonia", "Madagascar", "Malawi", "Malaysia", "Maldives", "Mali", "Malta", "Mauritania", "Mauritius", "Mexico", "Moldova", "Monaco", "Mongolia", "Montenegro", "Montserrat", "Morocco", "Mozambique", "Namibia", "Nepal", "Netherlands", "Netherlands Antilles", "New Caledonia", "New Zealand", "Nicaragua", "Niger", "Nigeria", "Norway", "Oman", "Pakistan", "Palestine", "Panama", "Papua New Guinea", "Paraguay", "Peru", "Philippines", "Poland", "Portugal", "Puerto Rico", "Qatar", "Reunion", "Romania", "Russia", "Rwanda", "Saint Pierre &amp; Miquelon", "Samoa", "San Marino", "Satellite", "Saudi Arabia", "Senegal", "Serbia", "Seychelles", "Sierra Leone", "Singapore", "Slovakia", "Slovenia", "South Africa", "South Korea", "Spain", "Sri Lanka", "St Kitts &amp; Nevis", "St Lucia", "St Vincent", "St. Lucia", "Sudan", "Suriname", "Swaziland", "Sweden", "Switzerland", "Syria", "Taiwan", "Tajikistan", "Tanzania", "Thailand", "Timor L'Este", "Togo", "Tonga", "Trinidad &amp; Tobago", "Tunisia", "Turkey", "Turkmenistan", "Turks &amp; Caicos", "Uganda", "Ukraine", "United Arab Emirates", "United Kingdom", "Uruguay", "Uzbekistan", "Venezuela", "Vietnam", "Virgin Islands (US)", "Yemen", "Zambia", "Zimbabwe"];

        //********************
        // general functions
        // get number
        $scope.getNumber = function (number) {
            var numbers = [];
            number = parseInt(number);
            for (var i = 1; i <= number; i++) {
                numbers.push(i);
            }
            return numbers;
        }
        // generate passenger'
        $scope.generatePassenger = function () {
            if (adultPassenger > 0) {
                for (var i = 0; i < adultPassenger; i++) {
                    var x = { type: adultTypeName };
                    $scope.passengers.push( x );
                }
            }
            if (childPassenger > 0) {
                for (var i = 0; i < childPassenger; i++) {
                    var x = { type: childTypeName };
                    $scope.passengers.push(x);
                }
            }
            if (infantPassenger > 0) {
                for (var i = 0; i < infantPassenger; i++) {
                    var x = { type: infantTypeName };
                    $scope.passengers.push(x);
                }
            }
        }
        $scope.generatePassenger();

        // generate years
        $scope.getYears = function(date,max) {
            var years = [];
            var currentYear = new Date(date);
            currentYear = currentYear.getYear();
            max = currentYear - max;
            for (var i = currentYear; i >= max ; i--) {
                years.push(i);
            }
            return years;

        }
        // change page
        $scope.changePage = function (page) {
            // change current page variable
            $scope.currentPage = page;
            // change step class
            $scope.stepClass = 'active-' + page;
        }
        // change page after login
        // $scope.changePage(currentPage);
        $scope.changePage(3); // development only

        // toggle Travorama Login
        $scope.toggleLogin = function() {
            if ($scope.loginShown == false) {
                $scope.loginShown = true;
            } else {
                $scope.loginShown = false;
            }
        }

        // test validate form
        $scope.testForm = function (formSelect) {
            if (formSelect == "buyerForm") {
                console.log($scope.buyerForm);
            } else if (formSelect == "passenger") {
                console.log('Adult Passengers :');
                console.log($scope.adultPassenger);
                // console.log($scope.adult1Form);
            } else {
                console.log($scope);
            }
        }

    }
]);// checkout controller