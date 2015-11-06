// Travorama Account controller
app.controller('accountController', [
    '$http', '$scope' , function($http, $scope) {   

        var hash = (location.hash);

        // variables
        $scope.pageLoaded = true;
        $scope.currentSection = '';
        $scope.profileForm = {
            active : false
        };
        $scope.passwordForm = {
            active: false
        };
        
        $scope.userProfile = userProfile;
        $scope.userProfile.edit = false;
        $scope.userProfile.updating = false;

        $scope.password = {}
        $scope.password.edit = false;
        $scope.password.updating = false;

        // $scope.countries = ["Afghanistan", "Albania", "Algeria", "Andorra", "Angola", "Anguilla", "Antigua &amp; Barbuda", "Argentina", "Armenia", "Aruba", "Australia", "Austria", "Azerbaijan", "Bahamas", "Bahrain", "Bangladesh", "Barbados", "Belarus", "Belgium", "Belize", "Benin", "Bermuda", "Bhutan", "Bolivia", "Bosnia &amp; Herzegovina", "Botswana", "Brazil", "British Virgin Islands", "Brunei", "Bulgaria", "Burkina Faso", "Burundi", "Cambodia", "Cameroon", "Cape Verde", "Cayman Islands", "Chad", "Chile", "China", "Colombia", "Congo", "Cook Islands", "Costa Rica", "Cote D Ivoire", "Croatia", "Cruise Ship", "Cuba", "Cyprus", "Czech Republic", "Denmark", "Djibouti", "Dominica", "Dominican Republic", "Ecuador", "Egypt", "El Salvador", "Equatorial Guinea", "Estonia", "Ethiopia", "Falkland Islands", "Faroe Islands", "Fiji", "Finland", "France", "French Polynesia", "French West Indies", "Gabon", "Gambia", "Georgia", "Germany", "Ghana", "Gibraltar", "Greece", "Greenland", "Grenada", "Guam", "Guatemala", "Guernsey", "Guinea", "Guinea Bissau", "Guyana", "Haiti", "Honduras", "Hong Kong", "Hungary", "Iceland", "India", "Indonesia", "Iran", "Iraq", "Ireland", "Isle of Man", "Israel", "Italy", "Jamaica", "Japan", "Jersey", "Jordan", "Kazakhstan", "Kenya", "Kuwait", "Kyrgyz Republic", "Laos", "Latvia", "Lebanon", "Lesotho", "Liberia", "Libya", "Liechtenstein", "Lithuania", "Luxembourg", "Macau", "Macedonia", "Madagascar", "Malawi", "Malaysia", "Maldives", "Mali", "Malta", "Mauritania", "Mauritius", "Mexico", "Moldova", "Monaco", "Mongolia", "Montenegro", "Montserrat", "Morocco", "Mozambique", "Namibia", "Nepal", "Netherlands", "Netherlands Antilles", "New Caledonia", "New Zealand", "Nicaragua", "Niger", "Nigeria", "Norway", "Oman", "Pakistan", "Palestine", "Panama", "Papua New Guinea", "Paraguay", "Peru", "Philippines", "Poland", "Portugal", "Puerto Rico", "Qatar", "Reunion", "Romania", "Russia", "Rwanda", "Saint Pierre &amp; Miquelon", "Samoa", "San Marino", "Satellite", "Saudi Arabia", "Senegal", "Serbia", "Seychelles", "Sierra Leone", "Singapore", "Slovakia", "Slovenia", "South Africa", "South Korea", "Spain", "Sri Lanka", "St Kitts &amp; Nevis", "St Lucia", "St Vincent", "St. Lucia", "Sudan", "Suriname", "Swaziland", "Sweden", "Switzerland", "Syria", "Taiwan", "Tajikistan", "Tanzania", "Thailand", "Timor L'Este", "Togo", "Tonga", "Trinidad &amp; Tobago", "Tunisia", "Turkey", "Turkmenistan", "Turks &amp; Caicos", "Uganda", "Ukraine", "United Arab Emirates", "United Kingdom", "Uruguay", "Uzbekistan", "Venezuela", "Vietnam", "Virgin Islands (US)", "Yemen", "Zambia", "Zimbabwe"];
        $scope.countries = [{ name: "AFGHANISTAN", twoLetter: "AF", threeLetter: "AFG", phone: "004" }, { name: "ALBANIA", twoLetter: "AL", threeLetter: "ALB", phone: "008" }, { name: "ALGERIA", twoLetter: "DZ", threeLetter: "DZA", phone: "012" }, { name: "AMERICAN SAMOA", twoLetter: "AS", threeLetter: "ASM", phone: "016" }, { name: "ANDORRA", twoLetter: "AD", threeLetter: "AND", phone: "020" }, { name: "ANGOLA", twoLetter: "AO", threeLetter: "AGO", phone: "024" }, { name: "ANGUILLA", twoLetter: "AI", threeLetter: "AIA", phone: "660" }, { name: "ANTARCTICA", twoLetter: "AQ", threeLetter: "ATA", phone: "010" }, { name: "ANTIGUA AND BARBUDA", twoLetter: "AG", threeLetter: "ATG", phone: "028" }, { name: "ARGENTINA", twoLetter: "AR", threeLetter: "ARG", phone: "032" }, { name: "ARMENIA", twoLetter: "AM", threeLetter: "ARM", phone: "051" }, { name: "ARUBA", twoLetter: "AW", threeLetter: "ABW", phone: "533" }, { name: "AUSTRALIA", twoLetter: "AU", threeLetter: "AUS", phone: "036" }, { name: "AUSTRIA", twoLetter: "AT", threeLetter: "AUT", phone: "040" }, { name: "AZERBAIJAN", twoLetter: "AZ", threeLetter: "AZE", phone: "031" }, { name: "BAHAMAS", twoLetter: "BS", threeLetter: "BHS", phone: "044" }, { name: "BAHRAIN", twoLetter: "BH", threeLetter: "BHR", phone: "048" }, { name: "BANGLADESH", twoLetter: "BD", threeLetter: "BGD", phone: "050" }, { name: "BARBADOS", twoLetter: "BB", threeLetter: "BRB", phone: "052" }, { name: "BELARUS", twoLetter: "BY", threeLetter: "BLR", phone: "112" }, { name: "BELGIUM", twoLetter: "BE", threeLetter: "BEL", phone: "056" }, { name: "BELIZE", twoLetter: "BZ", threeLetter: "BLZ", phone: "084" }, { name: "BENIN", twoLetter: "BJ", threeLetter: "BEN", phone: "204" }, { name: "BERMUDA", twoLetter: "BM", threeLetter: "BMU", phone: "060" }, { name: "BHUTAN", twoLetter: "BT", threeLetter: "BTN", phone: "064" }, { name: "BOLIVIA", twoLetter: "BO", threeLetter: "BOL", phone: "068" }, { name: "BOSNIA AND HERZEGOVINA", twoLetter: "BA", threeLetter: "BIH", phone: "070" }, { name: "BOTSWANA", twoLetter: "BW", threeLetter: "BWA", phone: "072" }, { name: "BOUVET ISLAND", twoLetter: "BV", threeLetter: "BVT", phone: "074" }, { name: "BRAZIL", twoLetter: "BR", threeLetter: "BRA", phone: "076" }, { name: "BRITISH INDIAN OCEAN TERRITORY", twoLetter: "IO", threeLetter: "IOT", phone: "086" }, { name: "BRUNEI DARUSSALAM", twoLetter: "BN", threeLetter: "BRN", phone: "096" }, { name: "BULGARIA", twoLetter: "BG", threeLetter: "BGR", phone: "100" }, { name: "BURKINA FASO", twoLetter: "BF", threeLetter: "BFA", phone: "854" }, { name: "BURUNDI", twoLetter: "BI", threeLetter: "BDI", phone: "108" }, { name: "CAMBODIA", twoLetter: "KH", threeLetter: "KHM", phone: "116" }, { name: "CAMEROON", twoLetter: "CM", threeLetter: "CMR", phone: "120" }, { name: "CANADA", twoLetter: "CA", threeLetter: "CAN", phone: "124" }, { name: "CAPE VERDE", twoLetter: "CV", threeLetter: "CPV", phone: "132" }, { name: "CAYMAN ISLANDS", twoLetter: "KY", threeLetter: "CYM", phone: "136" }, { name: "CENTRAL AFRICAN REPUBLIC", twoLetter: "CF", threeLetter: "CAF", phone: "140" }, { name: "CHAD", twoLetter: "TD", threeLetter: "TCD", phone: "148" }, { name: "CHILE", twoLetter: "CL", threeLetter: "CHL", phone: "152" }, { name: "CHINA", twoLetter: "CN", threeLetter: "CHN", phone: "156" }, { name: "CHRISTMAS ISLAND", twoLetter: "CX", threeLetter: "CXR", phone: "162" }, { name: "COCOS (KEELING) ISLANDS", twoLetter: "CC", threeLetter: "CCK", phone: "166" }, { name: "COLOMBIA", twoLetter: "CO", threeLetter: "COL", phone: "170" }, { name: "COMOROS", twoLetter: "KM", threeLetter: "COM", phone: "174" }, { name: "CONGO", twoLetter: "CG", threeLetter: "COG", phone: "178" }, { name: "COOK ISLANDS", twoLetter: "CK", threeLetter: "COK", phone: "184" }, { name: "COSTA RICA", twoLetter: "CR", threeLetter: "CRI", phone: "188" }, { name: "COTE D'IVOIRE", twoLetter: "CI", threeLetter: "CIV", phone: "384" }, { name: "CROATIA (local name: Hrvatska)", twoLetter: "HR", threeLetter: "HRV", phone: "191" }, { name: "CUBA", twoLetter: "CU", threeLetter: "CUB", phone: "192" }, { name: "CYPRUS", twoLetter: "CY", threeLetter: "CYP", phone: "196" }, { name: "CZECH REPUBLIC", twoLetter: "CZ", threeLetter: "CZE", phone: "203" }, { name: "DENMARK", twoLetter: "DK", threeLetter: "DNK", phone: "208" }, { name: "DJIBOUTI", twoLetter: "DJ", threeLetter: "DJI", phone: "262" }, { name: "DOMINICA", twoLetter: "DM", threeLetter: "DMA", phone: "212" }, { name: "DOMINICAN REPUBLIC", twoLetter: "DO", threeLetter: "DOM", phone: "214" }, { name: "EAST TIMOR", twoLetter: "TL", threeLetter: "TLS", phone: "626" }, { name: "ECUADOR", twoLetter: "EC", threeLetter: "ECU", phone: "218" }, { name: "EGYPT", twoLetter: "EG", threeLetter: "EGY", phone: "818" }, { name: "EL SALVADOR", twoLetter: "SV", threeLetter: "SLV", phone: "222" }, { name: "EQUATORIAL GUINEA", twoLetter: "GQ", threeLetter: "GNQ", phone: "226" }, { name: "ERITREA", twoLetter: "ER", threeLetter: "ERI", phone: "232" }, { name: "ESTONIA", twoLetter: "EE", threeLetter: "EST", phone: "233" }, { name: "ETHIOPIA", twoLetter: "ET", threeLetter: "ETH", phone: "210" }, { name: "FALKLAND ISLANDS (MALVINAS)", twoLetter: "FK", threeLetter: "FLK", phone: "238" }, { name: "FAROE ISLANDS", twoLetter: "FO", threeLetter: "FRO", phone: "234" }, { name: "FIJI", twoLetter: "FJ", threeLetter: "FJI", phone: "242" }, { name: "FINLAND", twoLetter: "FI", threeLetter: "FIN", phone: "246" }, { name: "FRANCE", twoLetter: "FR", threeLetter: "FRA", phone: "250" }, { name: "FRANCE, METROPOLITAN", twoLetter: "FX", threeLetter: "FXX", phone: "249" }, { name: "FRENCH GUIANA", twoLetter: "GF", threeLetter: "GUF", phone: "254" }, { name: "FRENCH POLYNESIA", twoLetter: "PF", threeLetter: "PYF", phone: "258" }, { name: "FRENCH SOUTHERN TERRITORIES", twoLetter: "TF", threeLetter: "ATF", phone: "260" }, { name: "GABON", twoLetter: "GA", threeLetter: "GAB", phone: "266" }, { name: "GAMBIA", twoLetter: "GM", threeLetter: "GMB", phone: "270" }, { name: "GEORGIA", twoLetter: "GE", threeLetter: "GEO", phone: "268" }, { name: "GERMANY", twoLetter: "DE", threeLetter: "DEU", phone: "276" }, { name: "GHANA", twoLetter: "GH", threeLetter: "GHA", phone: "288" }, { name: "GIBRALTAR", twoLetter: "GI", threeLetter: "GIB", phone: "292" }, { name: "GREECE", twoLetter: "GR", threeLetter: "GRC", phone: "300" }, { name: "GREENLAND", twoLetter: "GL", threeLetter: "GRL", phone: "304" }, { name: "GRENADA", twoLetter: "GD", threeLetter: "GRD", phone: "308" }, { name: "GUADELOUPE", twoLetter: "GP", threeLetter: "GLP", phone: "312" }, { name: "GUAM", twoLetter: "GU", threeLetter: "GUM", phone: "316" }, { name: "GUATEMALA", twoLetter: "GT", threeLetter: "GTM", phone: "320" }, { name: "GUINEA", twoLetter: "GN", threeLetter: "GIN", phone: "324" }, { name: "GUINEA-BISSAU", twoLetter: "GW", threeLetter: "GNB", phone: "624" }, { name: "GUYANA", twoLetter: "GY", threeLetter: "GUY", phone: "328" }, { name: "HAITI", twoLetter: "HT", threeLetter: "HTI", phone: "332" }, { name: "HEARD ISLAND & MCDONALD ISLANDS", twoLetter: "HM", threeLetter: "HMD", phone: "334" }, { name: "HONDURAS", twoLetter: "HN", threeLetter: "HND", phone: "340" }, { name: "HONG KONG", twoLetter: "HK", threeLetter: "HKG", phone: "344" }, { name: "HUNGARY", twoLetter: "HU", threeLetter: "HUN", phone: "348" }, { name: "ICELAND", twoLetter: "IS", threeLetter: "ISL", phone: "352" }, { name: "INDIA", twoLetter: "IN", threeLetter: "IND", phone: "356" }, { name: "INDONESIA", twoLetter: "ID", threeLetter: "IDN", phone: "360" }, { name: "IRAN, ISLAMIC REPUBLIC OF", twoLetter: "IR", threeLetter: "IRN", phone: "364" }, { name: "IRAQ", twoLetter: "IQ", threeLetter: "IRQ", phone: "368" }, { name: "IRELAND", twoLetter: "IE", threeLetter: "IRL", phone: "372" }, { name: "ISRAEL", twoLetter: "IL", threeLetter: "ISR", phone: "376" }, { name: "ITALY", twoLetter: "IT", threeLetter: "ITA", phone: "380" }, { name: "JAMAICA", twoLetter: "JM", threeLetter: "JAM", phone: "388" }, { name: "JAPAN", twoLetter: "JP", threeLetter: "JPN", phone: "392" }, { name: "JORDAN", twoLetter: "JO", threeLetter: "JOR", phone: "400" }, { name: "KAZAKHSTAN", twoLetter: "KZ", threeLetter: "KAZ", phone: "398" }, { name: "KENYA", twoLetter: "KE", threeLetter: "KEN", phone: "404" }, { name: "KIRIBATI", twoLetter: "KI", threeLetter: "KIR", phone: "296" }, { name: "KOREA, DEMOCRATIC PEOPLE'S REPUBLIC OF", twoLetter: "KP", threeLetter: "PRK", phone: "408" }, { name: "KOREA, REPUBLIC OF", twoLetter: "KR", threeLetter: "KOR", phone: "410" }, { name: "KUWAIT", twoLetter: "KW", threeLetter: "KWT", phone: "414" }, { name: "KYRGYZSTAN", twoLetter: "KG", threeLetter: "KGZ", phone: "417" }, { name: "LAO PEOPLE'S DEMOCRATIC REPUBLIC", twoLetter: "LA", threeLetter: "LAO", phone: "418" }, { name: "LATVIA", twoLetter: "LV", threeLetter: "LVA", phone: "428" }, { name: "LEBANON", twoLetter: "LB", threeLetter: "LBN", phone: "422" }, { name: "LESOTHO", twoLetter: "LS", threeLetter: "LSO", phone: "426" }, { name: "LIBERIA", twoLetter: "LR", threeLetter: "LBR", phone: "430" }, { name: "LIBYAN ARAB JAMAHIRIYA", twoLetter: "LY", threeLetter: "LBY", phone: "434" }, { name: "LIECHTENSTEIN", twoLetter: "LI", threeLetter: "LIE", phone: "438" }, { name: "LITHUANIA", twoLetter: "LT", threeLetter: "LTU", phone: "440" }, { name: "LUXEMBOURG", twoLetter: "LU", threeLetter: "LUX", phone: "442" }, { name: "MACAU", twoLetter: "MO", threeLetter: "MAC", phone: "446" }, { name: "MACEDONIA, THE FORMER YUGOSLAV REPUBLIC OF", twoLetter: "MK", threeLetter: "MKD", phone: "807" }, { name: "MADAGASCAR", twoLetter: "MG", threeLetter: "MDG", phone: "450" }, { name: "MALAWI", twoLetter: "MW", threeLetter: "MWI", phone: "454" }, { name: "MALAYSIA", twoLetter: "MY", threeLetter: "MYS", phone: "458" }, { name: "MALDIVES", twoLetter: "MV", threeLetter: "MDV", phone: "462" }, { name: "MALI", twoLetter: "ML", threeLetter: "MLI", phone: "466" }, { name: "MALTA", twoLetter: "MT", threeLetter: "MLT", phone: "470" }, { name: "MARSHALL ISLANDS", twoLetter: "MH", threeLetter: "MHL", phone: "584" }, { name: "MARTINIQUE", twoLetter: "MQ", threeLetter: "MTQ", phone: "474" }, { name: "MAURITANIA", twoLetter: "MR", threeLetter: "MRT", phone: "478" }, { name: "MAURITIUS", twoLetter: "MU", threeLetter: "MUS", phone: "480" }, { name: "MAYOTTE", twoLetter: "YT", threeLetter: "MYT", phone: "175" }, { name: "MEXICO", twoLetter: "MX", threeLetter: "MEX", phone: "484" }, { name: "MICRONESIA, FEDERATED STATES OF", twoLetter: "FM", threeLetter: "FSM", phone: "583" }, { name: "MOLDOVA, REPUBLIC OF", twoLetter: "MD", threeLetter: "MDA", phone: "498" }, { name: "MONACO", twoLetter: "MC", threeLetter: "MCO", phone: "492" }, { name: "MONGOLIA", twoLetter: "MN", threeLetter: "MNG", phone: "496" }, { name: "MONTSERRAT", twoLetter: "MS", threeLetter: "MSR", phone: "500" }, { name: "MOROCCO", twoLetter: "MA", threeLetter: "MAR", phone: "504" }, { name: "MOZAMBIQUE", twoLetter: "MZ", threeLetter: "MOZ", phone: "508" }, { name: "MYANMAR", twoLetter: "MM", threeLetter: "MMR", phone: "104" }, { name: "NAMIBIA", twoLetter: "NA", threeLetter: "NAM", phone: "516" }, { name: "NAURU", twoLetter: "NR", threeLetter: "NRU", phone: "520" }, { name: "NEPAL", twoLetter: "NP", threeLetter: "NPL", phone: "524" }, { name: "NETHERLANDS", twoLetter: "NL", threeLetter: "NLD", phone: "528" }, { name: "NETHERLANDS ANTILLES", twoLetter: "AN", threeLetter: "ANT", phone: "530" }, { name: "NEW CALEDONIA", twoLetter: "NC", threeLetter: "NCL", phone: "540" }, { name: "NEW ZEALAND", twoLetter: "NZ", threeLetter: "NZL", phone: "554" }, { name: "NICARAGUA", twoLetter: "NI", threeLetter: "NIC", phone: "558" }, { name: "NIGER", twoLetter: "NE", threeLetter: "NER", phone: "562" }, { name: "NIGERIA", twoLetter: "NG", threeLetter: "NGA", phone: "566" }, { name: "NIUE", twoLetter: "NU", threeLetter: "NIU", phone: "570" }, { name: "NORFOLK ISLAND", twoLetter: "NF", threeLetter: "NFK", phone: "574" }, { name: "NORTHERN MARIANA ISLANDS", twoLetter: "MP", threeLetter: "MNP", phone: "580" }, { name: "NORWAY", twoLetter: "NO", threeLetter: "NOR", phone: "578" }, { name: "OMAN", twoLetter: "OM", threeLetter: "OMN", phone: "512" }, { name: "PAKISTAN", twoLetter: "PK", threeLetter: "PAK", phone: "586" }, { name: "PALAU", twoLetter: "PW", threeLetter: "PLW", phone: "585" }, { name: "PANAMA", twoLetter: "PA", threeLetter: "PAN", phone: "591" }, { name: "PAPUA NEW GUINEA", twoLetter: "PG", threeLetter: "PNG", phone: "598" }, { name: "PARAGUAY", twoLetter: "PY", threeLetter: "PRY", phone: "600" }, { name: "PERU", twoLetter: "PE", threeLetter: "PER", phone: "604" }, { name: "PHILIPPINES", twoLetter: "PH", threeLetter: "PHL", phone: "608" }, { name: "PITCAIRN", twoLetter: "PN", threeLetter: "PCN", phone: "612" }, { name: "POLAND", twoLetter: "PL", threeLetter: "POL", phone: "616" }, { name: "PORTUGAL", twoLetter: "PT", threeLetter: "PRT", phone: "620" }, { name: "PUERTO RICO", twoLetter: "PR", threeLetter: "PRI", phone: "630" }, { name: "QATAR", twoLetter: "QA", threeLetter: "QAT", phone: "634" }, { name: "REUNION", twoLetter: "RE", threeLetter: "REU", phone: "638" }, { name: "ROMANIA", twoLetter: "RO", threeLetter: "ROU", phone: "642" }, { name: "RUSSIAN FEDERATION", twoLetter: "RU", threeLetter: "RUS", phone: "643" }, { name: "RWANDA", twoLetter: "RW", threeLetter: "RWA", phone: "646" }, { name: "SAINT KITTS AND NEVIS", twoLetter: "KN", threeLetter: "KNA", phone: "659" }, { name: "SAINT LUCIA", twoLetter: "LC", threeLetter: "LCA", phone: "662" }, { name: "SAINT VINCENT AND THE GRENADINES", twoLetter: "VC", threeLetter: "VCT", phone: "670" }, { name: "SAMOA", twoLetter: "WS", threeLetter: "WSM", phone: "882" }, { name: "SAN MARINO", twoLetter: "SM", threeLetter: "SMR", phone: "674" }, { name: "SAO TOME AND PRINCIPE", twoLetter: "ST", threeLetter: "STP", phone: "678" }, { name: "SAUDI ARABIA", twoLetter: "SA", threeLetter: "SAU", phone: "682" }, { name: "SENEGAL", twoLetter: "SN", threeLetter: "SEN", phone: "686" }, { name: "SERBIA", twoLetter: "RS", threeLetter: "SRB", phone: "688" }, { name: "SEYCHELLES", twoLetter: "SC", threeLetter: "SYC", phone: "690" }, { name: "SIERRA LEONE", twoLetter: "SL", threeLetter: "SLE", phone: "694" }, { name: "SINGAPORE", twoLetter: "SG", threeLetter: "SGP", phone: "702" }, { name: "SLOVAKIA (Slovak Republic)", twoLetter: "SK", threeLetter: "SVK", phone: "703" }, { name: "SLOVENIA", twoLetter: "SI", threeLetter: "SVN", phone: "705" }, { name: "SOLOMON ISLANDS", twoLetter: "SB", threeLetter: "SLB", phone: "90" }, { name: "SOMALIA", twoLetter: "SO", threeLetter: "SOM", phone: "706" }, { name: "SOUTH AFRICA", twoLetter: "ZA", threeLetter: "ZAF", phone: "710" }, { name: "SPAIN", twoLetter: "ES", threeLetter: "ESP", phone: "724" }, { name: "SRI LANKA", twoLetter: "LK", threeLetter: "LKA", phone: "144" }, { name: "SAINT HELENA", twoLetter: "SH", threeLetter: "SHN", phone: "654" }, { name: "SAINT PIERRE AND MIQUELON", twoLetter: "PM", threeLetter: "SPM", phone: "666" }, { name: "SUDAN", twoLetter: "SD", threeLetter: "SDN", phone: "736" }, { name: "SURINAME", twoLetter: "SR", threeLetter: "SUR", phone: "740" }, { name: "SVALBARD AND JAN MAYEN ISLANDS", twoLetter: "SJ", threeLetter: "SJM", phone: "744" }, { name: "SWAZILAND", twoLetter: "SZ", threeLetter: "SWZ", phone: "748" }, { name: "SWEDEN", twoLetter: "SE", threeLetter: "SWE", phone: "752" }, { name: "SWITZERLAND", twoLetter: "CH", threeLetter: "CHE", phone: "756" }, { name: "SYRIAN ARAB REPUBLIC", twoLetter: "SY", threeLetter: "SYR", phone: "760" }, { name: "TAIWAN, PROVINCE OF CHINA", twoLetter: "TW", threeLetter: "TWN", phone: "158" }, { name: "TAJIKISTAN", twoLetter: "TJ", threeLetter: "TJK", phone: "762" }, { name: "TANZANIA, UNITED REPUBLIC OF", twoLetter: "TZ", threeLetter: "TZA", phone: "834" }, { name: "THAILAND", twoLetter: "TH", threeLetter: "THA", phone: "764" }, { name: "TOGO", twoLetter: "TG", threeLetter: "TGO", phone: "768" }, { name: "TOKELAU", twoLetter: "TK", threeLetter: "TKL", phone: "772" }, { name: "TONGA", twoLetter: "TO", threeLetter: "TON", phone: "776" }, { name: "TRINIDAD AND TOBAGO", twoLetter: "TT", threeLetter: "TTO", phone: "780" }, { name: "TUNISIA", twoLetter: "TN", threeLetter: "TUN", phone: "788" }, { name: "TURKEY", twoLetter: "TR", threeLetter: "TUR", phone: "792" }, { name: "TURKMENISTAN", twoLetter: "TM", threeLetter: "TKM", phone: "795" }, { name: "TURKS AND CAICOS ISLANDS", twoLetter: "TC", threeLetter: "TCA", phone: "796" }, { name: "TUVALU", twoLetter: "TV", threeLetter: "TUV", phone: "798" }, { name: "UGANDA", twoLetter: "UG", threeLetter: "UGA", phone: "800" }, { name: "UKRAINE", twoLetter: "UA", threeLetter: "UKR", phone: "804" }, { name: "UNITED ARAB EMIRATES", twoLetter: "AE", threeLetter: "ARE", phone: "784" }, { name: "UNITED KINGDOM", twoLetter: "GB", threeLetter: "GBR", phone: "826" }, { name: "UNITED STATES", twoLetter: "US", threeLetter: "USA", phone: "840" }, { name: "UNITED STATES MINOR OUTLYING ISLANDS", twoLetter: "UM", threeLetter: "UMI", phone: "581" }, { name: "URUGUAY", twoLetter: "UY", threeLetter: "URY", phone: "858" }, { name: "UZBEKISTAN", twoLetter: "UZ", threeLetter: "UZB", phone: "860" }, { name: "VANUATU", twoLetter: "VU", threeLetter: "VUT", phone: "548" }, { name: "VATICAN CITY STATE (HOLY SEE)", twoLetter: "VA", threeLetter: "VAT", phone: "336" }, { name: "VENEZUELA", twoLetter: "VE", threeLetter: "VEN", phone: "862" }, { name: "VIET NAM", twoLetter: "VN", threeLetter: "VNM", phone: "704" }, { name: "VIRGIN ISLANDS (BRITISH)", twoLetter: "VG", threeLetter: "VGB", phone: "92" }, { name: "VIRGIN ISLANDS (U.S.)", twoLetter: "VI", threeLetter: "VIR", phone: "850" }, { name: "WALLIS AND FUTUNA ISLANDS", twoLetter: "WF", threeLetter: "WLF", phone: "876" }, { name: "WESTERN SAHARA", twoLetter: "EH", threeLetter: "ESH", phone: "732" }, { name: "YEMEN", twoLetter: "YE", threeLetter: "YEM", phone: "887" }, { name: "YUGOSLAVIA", twoLetter: "YU", threeLetter: "YUG", phone: "891" }, { name: "ZAIRE", twoLetter: "ZR", threeLetter: "ZAR", phone: "180" }, { name: "ZAMBIA", twoLetter: "ZM", threeLetter: "ZMB", phone: "894" }, { name: "ZIMBABWE", twoLetter: "ZW", threeLetter: "ZWE", phone: "716" }];

        // functions
        $scope.changeSection = function (name) {
            $scope.currentSection = name;
        }

        $scope.editForm = function (name) {
            // edit profile form
            if (name == 'profile') {
                $scope.userProfile.edit = !($scope.userProfile.edit);
            }
            else if (name == 'profileSave') {
                console.log('submitting form');
                // submit form to URL
                $http({
                    url: ChangeProfileConfig.Url,
                    method: 'POST',
                    data: {
                        Address: $scope.userProfile.address,
                        FirstName: $scope.userProfile.firstname,
                        LastName: $scope.userProfile.lastname,
                        PhoneNumber: $scope.userProfile.phone,
                        CountryCd: $scope.userProfile.country
                    }
                }).then(function (returnData) {
                    if (returnData.data.Status == 'Success') {
                        console.log('Success requesting change profile');
                        console.log(returnData);
                        $scope.profileForm.edit = false;
                    }
                    else {
                        console.log(returnData.data.Description);
                        console.log(returnData);
                        $scope.profileForm.edit = true;
                    }
                }, function (returnData) {
                    console.log('Failed requesting change profile');
                    console.log(returnData);
                    $scope.profileForm.edit = true;
                });
            }
            if (name == 'password') {
                $scope.password.edit = !($scope.password.edit);
            }
            else if (name == 'passwordSave') {
                console.log('submitting form');
                // submit form to URL
                $http({
                    url: ChangePasswordConfig.Url,
                    method: 'POST',
                    data: {
                        NewPassword: $scope.passwordForm.newPassword,
                        OldPassword: $scope.passwordForm.currentPassword,
                        ConfirmPassword: $scope.passwordForm.confirmationPassword
                    }
                }).then(function (returnData) {
                    $scope.passwordForm.newPassword = '';
                    $scope.passwordForm.currentPassword = '';
                    $scope.passwordForm.confirmationPassword = '';
                    if (returnData.data.Status == 'Success') {
                        console.log('Success requesting reset password');
                        console.log(returnData);
                        $scope.password.edit = false;
                    }
                    else {
                        console.log(returnData.data.Description);
                        console.log(returnData);
                        $scope.password.edit = true;
                    }
                }, function (returnData) {
                    console.log('Failed requesting reset password');
                    console.log(returnData);
                    $scope.password.edit = true;
                });
            }
        }

        $scope.passwordForm.submit = function () {
            $scope.passwordForm.submitting = true;
            console.log('submitting form');
            // submit form to URL
            $http({
                url: ChangePasswordConfig.Url,
                method: 'POST',
                data: {
                    password: $scope.passwordForm.newPassword
                }
            }).then(function (returnData) {
                if (returnData.data.Status == 'Success') {
                    console.log('Success requesting reset password');
                    console.log(returnData);
                    $scope.passwordForm.submitting = false;
                    $scope.passwordForm.submitted = true;
                }
                else {
                    console.log(returnData.data.Description);
                    console.log(returnData);
                    $scope.passwordForm.submitting = false;
                }
            }, function (returnData) {
                console.log('Failed requesting reset password');
                console.log(returnData);
                $scope.passwordForm.submitting = false;
            });
        }

        if (hash == '#order') {
            $scope.changeSection('order');
        } else {
            $scope.changeSection('profile');
        }

    }
]);// account controller


// Travorama forgot password controller
app.controller('passwordController', [
    '$http', '$scope', function ($http, $scope) {

        $scope.pageLoaded = true;
        $scope.form = {
            submitted: false,
            submitting: false,
            email: ''
        };
        $scope.logConsole = function(data) {
            console.log(data);
        }

        $scope.form.submit = function () {
            $scope.form.submitting = true;
            console.log('submitting form');
            // submit form to URL
            $http({
                url: ForgotPasswordConfig.Url,
                method: 'POST',
                data: {
                    email : $scope.form.email
                }
            }).then(function (returnData) {
                if (returnData.data.Status == 'Success') {
                    console.log('Success requesting reset password');
                    console.log(returnData);
                    $scope.form.submitting = false;
                    $scope.form.submitted = true;
                }
                else {
                    console.log(returnData.data.Description);
                    console.log(returnData);
                    $scope.form.submitting = false;
                }
            }, function (returnData) {
                console.log('Failed requesting reset password');
                console.log(returnData);
                $scope.form.submitting = false;
            });
        }

    }
]);// account controller


// order detail controller
app.controller('orderDetailController', [
    '$http', '$scope', function ($http, $scope) {

        $scope.getTime = function(dateTime) {
            return new Date(dateTime);
        }

        $scope.currentSection = 'order';
        $scope.contactDetail = contactDetail;
        $scope.passengerDetail = passengerDetail;
        $scope.flightDetail = flightDetail;
        $scope.refundDetail = refundDetail;

    }
]);

// Travorama reset controller
app.controller('resetController', [
    '$http', '$scope', function ($http, $scope) {

        $scope.pageLoaded = true;
        $scope.form = {
            submitted: false,
            submitting: false,
            userEmail: userEmail,
            code : code
        };
        $scope.form.submit = function() {
            $scope.form.submitting = true;

            $http({
                url: ResetPasswordConfig.Url,
                method: 'POST',
                data: {
                    Password: $scope.form.password,
                    ConfirmPassword: $scope.form.password,
                    Email: $scope.form.userEmail,
                    Code: $scope.form.code
                }
            }).then(function (returnData) {
                if (returnData.data.Status == 'Success') {
                    console.log('Success requesting reset password');
                    console.log(returnData);
                    $scope.form.submitting = false;
                    $scope.form.submitted = true;
                }
                else {
                    console.log(returnData.data.Description);
                    console.log(returnData);
                    $scope.form.submitting = false;
                }
            }, function (returnData) {
                console.log('Failed requesting reset password');
                console.log(returnData);
                $scope.form.submitting = false;
            });

        }

    }
]);// reset controller

// Travorama Check Order Controller
app.controller('checkController', [
    '$http', '$scope', function ($http, $scope) {

        $scope.pageLoaded = true;
        $scope.form = {
            orderNo: '',
            lastname: '',
            submitting : false
        };

    }
]);// reset controller

// Travorama Check Order Controller
app.controller('authController', [
    '$scope', function ($scope) {

        $scope.pageLoaded = true;
        $scope.form = {
            email: '',
            password: '',
            submitting: false
        };

    }
]);// reset controller