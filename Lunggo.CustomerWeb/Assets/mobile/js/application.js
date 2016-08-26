// check if angular exist
if (typeof (angular) == 'object') {
    
    var app = angular.module('Travorama', ['ngRoute']);
    // root scope
    app.run(function($rootScope) {
        $.datepicker.setDefaults(
            $.extend(
            { 'dateFormat': 'dd/mm/yy' },
            $.datepicker.regional['id']
            )
        );
        //$.datepicker($.datepicker.regional["id-ID"]);
        //('.ui-datepicker').addClass('notranslate');

        var trial = 0;

        // general page config and function
        $rootScope.PageConfig = {
            
            // **********
            // Popular Destination
            PopularDestination : {
                Popular: [
                    { Name: 'Soekarno Hatta Intl.', City: 'Jakarta', Country: 'Indonesia', Code: 'CGK' },
                    { Name: 'Ngurah Rai Intl.', City: 'Denpasar', Country: 'Indonesia', Code: 'DPS' },
                    { Name: 'Juanda', City: 'Surabaya', Country: 'Indonesia', Code: 'SUB' },
                    { Name: 'Kuala Namu Intl.', City: 'Medan', Country: 'Indonesia', Code: 'KNO' },
                    { Name: 'Sultan Hasanudin', City: 'Makassar', Country: 'Indonesia', Code: 'UPG' },
                    { Name: 'Adisucipto', City: 'Yogyakarta', Country: 'Indonesia', Code: 'JOG' },
                    { Name: 'Changi', City: 'Singapore', Country: 'Singapore', Code: 'SIN' },
                    { Name: 'Suvarnabhumi Intl.', City: 'Bangkok', Country: 'Thailand', Code: 'BKK' },
                    { Name: 'Hat Yai', City: 'Hat Yai', Country: 'Thailand', Code: 'HDY' },
                    { Name: 'Phuket Intl.', City: 'Phuket', Country: 'Thailand', Code: 'HKT' },
                    { Name: 'Sultan Ismail Intl', City: 'Johor Bahru', Country: 'Malaysia', Code: 'JHB' },
                    { Name: 'Kota-Kinabalu Intl. Arpt.', City: 'Kota Kinabalu', Country: 'Malaysia', Code: 'BKI' },
                    { Name: 'Kuala Lumpur Intl. Arpt.', City: 'Kuala Lumpur', Country: 'Malaysia', Code: 'KUL' },
                    { Name: 'Kuching', City: 'Kuching', Country: 'Malaysia', Code: 'KCH' },
                    { Name: 'Penang Intl.', City: 'Penang', Country: 'Malaysia', Code: 'PEN' },
                    { Name: 'Phnom Penh Intl.', City: 'Phnom Penh', Country: 'Cambodia', Code: 'PNH' },
                    { Name: 'Ninoy Aquino Intl.', City: 'Manila', Country: 'Philippines', Code: 'MNL' },
                    { Name: 'Noibai Intl.', City: 'Hanoi', Country: 'Vietnam', Code: 'HAN' },
                    { Name: 'Tan Son Nhat Intl.', City: 'Ho Chi Minh City', Country: 'Vietnam', Code: 'SGN' },
                    { Name: 'Macau Intl.', City: 'Macau', Country: 'Macau', Code: 'MFM' },
                    { Name: 'New Baiyun', City: 'Guangzhou', Country: 'China', Code: 'CAN' },
                    { Name: 'Hong Kong Intl.', City: 'Hong Kong', Country: 'Hong Kong', Code: 'HKG' },
                    { Name: 'Tullamarine', City: 'Melbourne', Country: 'Australia', Code: 'MEL' },
                    { Name: 'Perth Intl.', City: 'Perth', Country: 'Australia', Code: 'PER' },
                    { Name: 'Aeroporto Internacional Guarulhos', City: 'Sao Paulo', Country: 'Brazil', Code: 'GRU' },
                    { Name: 'Hartsfield-jackson Atlanta Intl.', City: 'Atlanta', Country: 'United States', Code: 'ATL' },
                    { Name: 'Thurgood Marshall', City: 'Baltimore/Washington', Country: 'United States', Code: 'BWI' },
                    { Name: 'Logan Intl.', City: 'Boston', Country: 'United States', Code: 'BOS' },
                    { Name: 'Chicago O hare Intl.', City: 'Chicago', Country: 'United States', Code: 'ORD' },
                    { Name: 'Dallas/Fort Worth Intl.', City: 'Dallas', Country: 'United States', Code: 'DFW' },
                    { Name: 'McCarran Intl', City: 'Las Vegas', Country: 'United States', Code: 'LAS' },
                    { Name: 'Los Angeles Intl.', City: 'Los Angeles', Country: 'United States', Code: 'LAX' },
                    { Name: 'Miami International Airport', City: 'Miami', Country: 'United States', Code: 'MIA' },
                    { Name: 'Newark Liberty Intl.', City: 'Newark', Country: 'United States', Code: 'EWR' },
                    { Name: 'Orlando Intl. Arpt.', City: 'Orlando', Country: 'United States', Code: 'MCO' },
                    { Name: 'San Diego Intl. Arpt.', City: 'San Diego', Country: 'United States', Code: 'SAN' },
                    { Name: 'San Francisco Intl.', City: 'San Francisco', Country: 'United States', Code: 'SFO' },
                    { Name: 'Mineta San Jose Intl. Arpt.', City: 'San Jose', Country: 'United States', Code: 'SJC' },
                    { Name: 'Seattle-Tacoma Intl.', City: 'Seattle', Country: 'United States', Code: 'SEA' },
                    { Name: 'Lester B. Pearson Intl.', City: 'Toronto', Country: 'Canada', Code: 'YYZ' },
                    { Name: 'Vancouver International Airport', City: 'Vancouver', Country: 'Canada', Code: 'MFM' }
                ],
                Indonesia: [
                    { Name: 'Soekarno Hatta Intl.', City: 'Jakarta', Country: 'Indonesia', Code: 'CGK' },
                    { Name: 'Husein Sastranegara', City: 'Bandung', Country: 'Indonesia', Code: 'BDO' },
                    { Name: 'Adi Sumarmo', City: 'Solo', Country: 'Indonesia', Code: 'SOC' },
                    { Name: 'Achmad Yani', City: 'Semarang', Country: 'Indonesia', Code: 'SRG' },
                    { Name: 'Juanda', City: 'Surabaya', Country: 'Indonesia', Code: 'SUB' },
                    { Name: 'Adisucipto', City: 'Yogyakarta', Country: 'Indonesia', Code: 'JOG' },
                    { Name: 'Kuala Namu Intl.', City: 'Medan', Country: 'Indonesia', Code: 'KNO' },
                    { Name: 'Minangkabau Intl. Arpt.', City: 'Padang', Country: 'Indonesia', Code: 'PDG' },
                    { Name: 'Radin Inten II', City: 'Bandar Lampung', Country: 'Indonesia', Code: 'TKG' },
                    { Name: 'Sultan Iskandar Muda Arpt.', City: 'Banda Aceh', Country: 'Indonesia', Code: 'BTJ' },
                    { Name: 'Fatmawati Soekarno', City: 'Bengkulu', Country: 'Indonesia', Code: 'BKS' },
                    { Name: 'Hang Nadim', City: 'Batam', Country: 'Indonesia', Code: 'BTH' },
                    { Name: 'Sultan Thaha Syaifuddin', City: 'Jambi', Country: 'Indonesia', Code: 'DJB' },
                    { Name: 'Depati Amir', City: 'Pangkalpinang', Country: 'Indonesia', Code: 'PGK' },
                    { Name: 'Sultan Syarif Kasim II', City: 'Pekanbaru', Country: 'Indonesia', Code: 'PKU' },
                    { Name: 'Pinang Kampai', City: 'Dumai', Country: 'Indonesia', Code: 'DUM' },
                    { Name: 'Binaka', City: 'Gunung Sitoli', Country: 'Indonesia', Code: 'GNS' },
                    { Name: 'Maimun Saleh', City: 'Sabang', Country: 'Indonesia', Code: 'SBG' },
                    { Name: 'Ngurah Rai Intl.', City: 'Denpasar, Bali', Country: 'Indonesia', Code: 'DPS' },
                    { Name: 'Komodo', City: 'Labuan Bajo', Country: 'Indonesia', Code: 'LBJ' },
                    { Name: 'Lombok International Airport', City: 'Lombok', Country: 'Indonesia', Code: 'LOP' },
                    { Name: 'El Tari', City: 'Kupang', Country: 'Indonesia', Code: 'KOE' },
                    { Name: 'Haliwen', City: 'Atambua', Country: 'Indonesia', Code: 'ABU' },
                    { Name: 'Muhammad Salahuddin', City: 'Bima', Country: 'Indonesia', Code: 'BMU' },
                    { Name: 'Sultan Aji Muhammad Sulaiman', City: 'Balikpapan', Country: 'Indonesia', Code: 'BPN' },
                    { Name: 'Sjamsudin Noor', City: 'Banjarmasin', Country: 'Indonesia', Code: 'BDJ' },
                    { Name: 'Supadio', City: 'Pontianak', Country: 'Indonesia', Code: 'PNK' },
                    { Name: 'Tjilik Riwut', City: 'Palangkaraya', Country: 'Indonesia', Code: 'PKY' },
                    { Name: 'Temindung', City: 'Samarinda', Country: 'Indonesia', Code: 'SRI' },
                    { Name: 'H. Asan Kampai', City: 'Sampit', Country: 'Indonesia', Code: 'SMQ' },
                    { Name: 'Rahadi Oesman', City: 'Ketapang', Country: 'Indonesia', Code: 'KTG' },
                    { Name: 'Sultan Hasanuddin', City: 'Makassar', Country: 'Indonesia', Code: 'UPG' },
                    { Name: 'Sam Ratulangi', City: 'Manado', Country: 'Indonesia', Code: 'MDC' },
                    { Name: 'Jalaluddin', City: 'Gorontalo', Country: 'Indonesia', Code: 'GTO' },
                    { Name: 'Haluoleo', City: 'Kendari', Country: 'Indonesia', Code: 'KDI' },
                    { Name: 'Mutiara Sis Aljufri', City: 'Palu', Country: 'Indonesia', Code: 'PLW' },
                    { Name: 'Betoambari', City: 'Baubau', Country: 'Indonesia', Code: 'BUW' },
                    { Name: 'Kasiguncu', City: 'Poso', Country: 'Indonesia', Code: 'PSJ' },
                    { Name: 'Pongtiku', City: 'Tana Toraja', Country: 'Indonesia', Code: 'TTR' },
                    { Name: 'Lalos', City: 'Tolitoli', Country: 'Indonesia', Code: 'TLI' },
                    { Name: 'Pattimura', City: 'Ambon', Country: 'Indonesia', Code: 'AMQ' },
                    { Name: 'Sentani', City: 'Jayapura', Country: 'Indonesia', Code: 'DJJ' },
                    { Name: 'Sultan Babullah', City: 'Ternate', Country: 'Indonesia', Code: 'TTE' },
                    { Name: 'Jeffman', City: 'Sorong', Country: 'Indonesia', Code: 'SOQ' },
                    { Name: 'Torea', City: 'Fak Fak', Country: 'Indonesia', Code: 'FKQ' },
                    { Name: 'Kebar', City: 'Manokwari', Country: 'Indonesia', Code: 'KEQ' },
                    { Name: 'Kimaan', City: 'Merauke', Country: 'Indonesia', Code: 'KMM' },
                    { Name: 'Morotai Island', City: 'Pitu', Country: 'Indonesia', Code: 'OTI' },
                    { Name: 'Frans Kaisepo', City: 'Biak', Country: 'Indonesia', Code: 'BIK' },
                    { Name: 'Timika', City: 'Tembagapura', Country: 'Indonesia', Code: 'TIM' },
                    { Name: 'Wamena', City: 'Wamena', Country: 'Indonesia', Code: 'WMX' }
                ],
                SouthEastAsia: [
                    { Name: 'Changi', City: 'Singapore', Country: 'Singapore', Code: 'SIN' },
                    { Name: 'Suvarnabhumi Intl.', City: 'Bangkok', Country: 'Thailand', Code: 'BKK' },
                    { Name: 'Hat Yai', City: 'Hat Yai', Country: 'Thailand', Code: 'HDY' },
                    { Name: 'Phuket Intl.', City: 'Phuket', Country: 'Thailand', Code: 'HKT' },
                    { Name: 'Sultan Ismail Intl', City: 'Johor Bahru', Country: 'Malaysia', Code: 'JHB' },
                    { Name: 'Kota-Kinabalu Intl. Arpt.', City: 'Kota Kinabalu', Country: 'Malaysia', Code: 'BKI' },
                    { Name: 'Kuala Lumpur Intl. Arpt.', City: 'Kuala Lumpur', Country: 'Malaysia', Code: 'KUL' },
                    { Name: 'Kuching', City: 'Kuching', Country: 'Malaysia', Code: 'KCH' },
                    { Name: 'Penang Intl.', City: 'Penang', Country: 'Malaysia', Code: 'PEN' },
                    { Name: 'Ninoy Aquino Intl.', City: 'Manila', Country: 'Philippines', Code: 'MNL' },
                    { Name: 'Phnom Penh Intl.', City: 'Phnom Penh', Country: 'Cambodia', Code: 'PNH' },
                    { Name: 'Noibai Intl.', City: 'Hanoi', Country: 'Vietnam', Code: 'HAN' },
                    { Name: 'Tan Son Nhat Intl.', City: 'Ho Chi Minh City', Country: 'Vietnam', Code: 'SGN' },
                    { Name: 'Bandar Seri Begwan Intl. Arpt.', City: 'Bandar Seri Begawan', Country: 'Brunei Darussalam', Code: 'BWN' },
                    { Name: 'Wattay', City: 'Vientiane', Country: 'Laos', Code: 'VTE' },
                    { Name: 'Mingaladon', City: 'Yangon', Country: 'Myanmar', Code: 'TRGN' },
                    { Name: 'Presidente Nicolau Lobato Intl. Arpt.', City: 'Dili', Country: 'East Timor', Code: 'DIL'}
                ],
                EastAsia: [
                    { Name: 'Beihai Fucheng', City: 'Beihai', Country: 'China', Code: 'BHY' },
                    { Name: 'Beijing Capital Int.', City: 'Beijing', Country: 'China', Code: 'PEK' },
                    { Name: 'Pu Dong', City: 'Shanghai', Country: 'China', Code: 'PVG' },
                    { Name: 'Shuangliu', City: 'Chengdu', Country: 'China', Code: 'CTU' },
                    { Name: 'Chongqing Jiangbei Intl', City: 'Chongqing', Country: 'China', Code: 'CKG' },
                    { Name: 'Ganzhou Huangjin', City: 'Ganzhou', Country: 'China', Code: 'KOW' },
                    { Name: 'New Baiyun', City: 'Guangzhou', Country: 'China', Code: 'CAN' },
                    { Name: 'Hangzhou Xiaoshan.', City: 'Hangzhou', Country: 'China', Code: 'HGH' },
                    { Name: 'Harbin Taiping', City: 'Harbin', Country: 'China', Code: 'HRB' },
                    { Name: 'Jinan Yaoqiang', City: 'Jinan', Country: 'China', Code: 'TNA' },
                    { Name: 'Lianyungang', City: 'Lianyungang', Country: 'China', Code: 'LYG' },
                    { Name: 'Nanchang Changbei', City: 'Nanchang', Country: 'China', Code: 'KHN' },
                    { Name: 'Nanyang Jiangying', City: 'Nanyang', Country: 'China', Code: 'NNY' },
                    { Name: 'Shenzhen Baoan', City: 'Shenzen', Country: 'China', Code: 'SZX' },
                    { Name: 'Tianjin Binhai Intl.', City: 'Tianjin', Country: 'China', Code: 'TSN' },
                    { Name: 'Xian Xianyang', City: 'Xi An', Country: 'China', Code: 'XIY' },
                    { Name: 'Zhengzhou Xinzheng', City: 'Zhengzhou', Country: 'China', Code: 'CGO' },
                    { Name: 'Tokyo Haneda Intl.', City: 'Tokyo', Country: 'Japan', Code: 'HND' },
                    { Name: 'Kansai', City: 'Kyoto', Country: 'Japan', Code: 'UKY' },
                    { Name: 'Itami', City: 'Osaka', Country: 'Japan', Code: 'ITM' },
                    { Name: 'Aomori', City: 'Aomori', Country: 'Japan', Code: 'AOJ' },
                    { Name: 'Asahikawa', City: 'Asahikawa', Country: 'Japan', Code: 'AKJ' },
                    { Name: 'Fukuoka', City: 'Fukuoka', Country: 'Japan', Code: 'FUK' },
                    { Name: 'Hakodate', City: 'Hakodate', Country: 'Japan', Code: 'HKD' },
                    { Name: 'Hiroshima Intl.', City: 'Hiroshima', Country: 'Japan', Code: 'HIJ' },
                    { Name: 'Kagoshima', City: 'Kagoshima', Country: 'Japan', Code: 'KOJ' },
                    { Name: 'Komatsu', City: 'Komatsu', Country: 'Japan', Code: 'KMQ' },
                    { Name: 'Kumamoto', City: 'Kumamoto', Country: 'Japan', Code: 'KMJ' },
                    { Name: 'Kushiro', City: 'Kushiro', Country: 'Japan', Code: 'KUH' },
                    { Name: 'Matsuyama', City: 'Matsuyama', Country: 'Japan', Code: 'MYJ' },
                    { Name: 'Memanbetsu', City: 'Memanbetsu', Country: 'Japan', Code: 'MMB' },
                    { Name: 'Nagoya-Komaki AFB', City: 'Nagoya', Country: 'Japan', Code: 'NGO' },
                    { Name: 'Oita', City: 'Oita', Country: 'Japan', Code: 'OIT' },
                    { Name: 'Okayama', City: 'Okayama', Country: 'Japan', Code: 'OKJ' },
                    { Name: 'Naha', City: 'Okinawa', Country: 'Japan', Code: 'OKA' },
                    { Name: 'New Chitose Arpt.', City: 'Sapporo', Country: 'Japan', Code: 'CTS' },
                    { Name: 'Takamatsu', City: 'Takamatsu', Country: 'Japan', Code: 'TAK' },
                    { Name: 'Hong Kong Intl.', City: 'Hong Kong', Country: 'Hong Kong', Code: 'HKG' },
                    { Name: 'Kaoshiung Intl. Arpt.', City: 'Kaohsiung', Country: 'Taiwan', Code: 'KHH' },
                    { Name: 'Chingchuankang', City: 'Taichung', Country: 'Taiwan', Code: 'RMQ' },
                    { Name: 'Taiwan Taoyuan Intl.', City: 'Taipei', Country: 'Taiwan', Code: 'TPE' },
                    { Name: 'Incheon Intl.', City: 'Seoul', Country: 'South Korea', Code: 'ICN' },
                    { Name: 'Gimhae', City: 'Busan', Country: 'South Korea', Code: 'PUS' },
                    { Name: 'Jeju Arpt.', City: 'Jeju', Country: 'South Korea', Code: 'CJU' },
                    { Name: 'Gwangju', City: 'Gwangju', Country: 'South Korea', Code: 'KWJ' }
                ],
                Europe: [
                    { Name: 'Vienna Intl. Airport', City: 'Vienna', Country: 'Austria', Code: 'VIE' },
                    { Name: 'Amsterdam-Schiphol', City: 'Amsterdam', Country: 'Netherland', Code: 'AMS' },
                    { Name: 'Brussels Arpt.', City: 'Brussels', Country: 'Belgium', Code: 'BRU' },
                    { Name: 'Copenhagen', City: 'Copenhagen', Country: 'Denmark', Code: 'CPH' },
                    { Name: 'Billund', City: 'Billund', Country: 'Denmark', Code: 'BLL' },
                    { Name: 'Helsinki-Vantaa', City: 'Helsinki', Country: 'Finland', Code: 'HEL' },
                    { Name: 'Ferenc Liszt', City: 'Budapest', Country: 'Hungary', Code: 'BUD' },
                    { Name: 'Esenboga', City: 'Ankara', Country: 'Turkey', Code: 'ESB' },
                    { Name: 'Ataturk', City: 'Istanbul', Country: 'Turkey', Code: 'IST' },
                    { Name: 'Belfast Intl. Arpt.', City: 'Belfast', Country: 'United Kingdom', Code: 'BFS' },
                    { Name: 'Edinburgh', City: 'Edinburgh', Country: 'United Kingdom', Code: 'EDI' },
                    { Name: 'London Heathrow', City: 'London', Country: 'United Kingdom', Code: 'LHR' },
                    { Name: 'Manchester', City: 'Manchester', Country: 'United Kingdom', Code: 'MAN' },
                    { Name: 'Glasgow', City: 'Glasgow', Country: 'United Kingdom', Code: 'GLA' },
                    { Name: 'Dublin', City: 'Dublin', Country: 'Ireland', Code: 'DUB' },
                    { Name: 'Berlin Schonefeld Airport', City: 'Berlin', Country: 'Germany', Code: 'SXF' },
                    { Name: 'Frankfurt Intl. Arpt.', City: 'Frankfurt', Country: 'Germany', Code: 'FRA' },
                    { Name: 'Hamburg Arpt.', City: 'Hamburg', Country: 'Germany', Code: 'HAM' },
                    { Name: 'Flughafen Munchen', City: 'Munchen', Country: 'Germany', Code: 'MUC' },
                    { Name: 'Nuremberg', City: 'Nuremberg', Country: 'Germany', Code: 'NUE' },
                    { Name: 'EuroArpt. Basel Mulhouse Freiburg', City: 'Basel', Country: 'Switzerland', Code: 'BSL' },
                    { Name: 'Geneva', City: 'Geneva', Country: 'Switzerland', Code: 'GVA' },
                    { Name: 'Zurich-Kloten', City: 'Zurich', Country: 'Switzerland', Code: 'ZRH' },
                    { Name: 'Gardermoen', City: 'Oslo', Country: 'Norway', Code: 'OSL' },
                    { Name: 'Torp Sandefjord', City: 'Sandefjord', Country: 'Norway', Code: 'TRF' },
                    { Name: 'Sola', City: 'Stavenger', Country: 'Norway', Code: 'SVG' },
                    { Name: 'Frederic Chopin', City: 'Warsaw', Country: 'Poland', Code: 'WAW' },
                    { Name: 'Charles De Gaulle', City: 'Paris', Country: 'France', Code: 'CDG' },
                    { Name: 'Lyon - Saint-Exupery', City: 'Lyon', Country: 'France', Code: 'LYS' },
                    { Name: 'Marseille Provence Arpt.', City: 'Marseille', Country: 'France', Code: 'MRS' },
                    { Name: 'Toulouse-Blagnac', City: 'Toulouse', Country: 'France', Code: 'TLS' },
                    { Name: 'Václav Havel', City: 'Prague', Country: 'Japan', Code: 'PRG' },
                    { Name: 'Domodedovo', City: 'Moscow', Country: 'Russia', Code: 'DME' },
                    { Name: 'Nikola Tesla', City: 'Belgrade', Country: 'Serbia', Code: 'BEG' },
                    { Name: 'Vienna Intl. Airport.', City: 'Vienna', Country: 'Austria', Code: 'VIE' },
                    { Name: 'Malpensa', City: 'Milan', Country: 'Italy', Code: 'MXP' },
                    { Name: 'Leonardo da Vinci Intl.', City: 'Rome', Country: 'Italy', Code: 'FCO' },
                    { Name: 'Marco Polo', City: 'Venice', Country: 'Italy', Code: 'VCE' },
                    { Name: 'Eleftherios Venizelos', City: 'Athens', Country: 'Greece', Code: 'ATH' },
                    { Name: 'Macedonia Intl.', City: 'Thessaloniki', Country: 'Greece', Code: 'SKG' },
                    { Name: 'El Prat', City: 'Barcelona', Country: 'Spain', Code: 'BCN' },
                    { Name: 'Adolfo Suárez Madrid–Barajas', City: 'Madrid', Country: 'Spain', Code: 'MAD' },
                    { Name: 'Arlanda', City: 'Stockholm', Country: 'Sweden', Code: 'ARN' }
                ],
                Oceania: [
                    { Name: 'Adelaide Intl. Arpt.', City: 'Adelaide', Country: 'Australia', Code: 'ADL' },
                    { Name: 'Brisbane Intl.', City: 'Brisbane', Country: 'Australia', Code: 'BNE' },
                    { Name: 'Cairns', City: 'Cairns', Country: 'Australia', Code: 'CNS' },
                    { Name: 'Darwin', City: 'Darwin', Country: 'Australia', Code: 'DRW' },
                    { Name: 'Gold Coast', City: 'Gold Coast', Country: 'Australia', Code: 'OOL' },
                    { Name: 'Hobart', City: 'Hobart', Country: 'Australia', Code: 'HBA' },
                    { Name: 'Mackay', City: 'Mackay', Country: 'Australia', Code: 'MKY' },
                    { Name: 'Maroochydore', City: 'Sunshine Coast', Country: 'Australia', Code: 'MCY' },
                    { Name: 'Tullamarine', City: 'Melbourne', Country: 'Australia', Code: 'MEL' },
                    { Name: 'Perth Intl.', City: 'Perth', Country: 'Australia', Code: 'PER' },
                    { Name: 'Whitsunday Coast', City: 'Proserpine', Country: 'Australia', Code: 'PPP' },
                    { Name: 'Sydney', City: 'Sydney', Country: 'Australia', Code: 'SYD' },
                    { Name: 'Auckland Intl.', City: 'Auckland', Country: 'New Zealand', Code: 'AKL' },
                    { Name: 'Christchurch Intl.', City: 'Christchurch', Country: 'New Zealand', Code: 'CHC' },
                    { Name: 'Wellington Intl.', City: 'Wellington', Country: 'New Zealand', Code: 'WLG' },
                    { Name: 'Faa a', City: 'Papeete', Country: 'French Polynesia', Code: 'PPT' }
                ],
                Others: [
                    { Name: 'Shah Amanat Intl. Arpt.', City: 'Chittagong', Country: 'Bangladesh', Code: 'CGP' },
                    { Name: 'Shahjalal Intl. Arpt.', City: 'Dhaka', Country: 'Bangladesh', Code: 'DAC' },
                    { Name: 'Kempegowda Intl. Arpt.', City: 'Bangalore', Country: 'India', Code: 'BLR' },
                    { Name: 'Chennai Intl. Arpt.', City: 'Chennai', Country: 'India', Code: 'MAA' },
                    { Name: 'Gaya', City: 'Geneva', Country: 'India', Code: 'GAY' },
                    { Name: 'Hyderabad Intl. Arpt.', City: 'Hyderabad', Country: 'India', Code: 'HYD' },
                    { Name: 'Cochin Intl.', City: 'Oslo', Country: 'India', Code: 'COK' },
                    { Name: 'Netaji Subhas Chandra Bose', City: 'Kolkata', Country: 'India', Code: 'CCU' },
                    { Name: 'Chhatrapati Shivaji Intl.', City: 'Mumbai', Country: 'India', Code: 'BOM' },
                    { Name: 'Indira Gandhi Intl.', City: 'New Delhi', Country: 'India', Code: 'DEL' },
                    { Name: 'Tiruchirappalli Intl.', City: 'Tiruchirappalli', Country: 'India', Code: 'TRZ' },
                    { Name: 'Lal Bahadur Shastri', City: 'Varanasi', Country: 'India', Code: 'VNS' },
                    { Name: 'Vishakhapatnam', City: 'Vishakhapatnam', Country: 'India', Code: 'VTZ' },
                    { Name: 'Tribhuvan', City: 'Kathmandu', Country: 'Nepal', Code: 'KTM' },
                    { Name: 'Bandaranaike', City: 'Colombo', Country: 'Sri Lanka', Code: 'CMB' },
                    { Name: 'Male Intl.', City: 'Male', Country: 'Maldives', Code: 'MLE' },
                    { Name: 'King Fahd Intl. Arpt.', City: 'Dammam', Country: 'Saudi Arabia', Code: 'DMM' },
                    { Name: 'King Abdulaziz Intl.', City: 'Jeddah', Country: 'Saudi Arabia', Code: 'JED' },
                    { Name: 'Mohammad Bin Abdulaziz', City: 'Madinah', Country: 'Saudi Arabia', Code: 'MED' },
                    { Name: 'King Khalid Intl.', City: 'Riyadh', Country: 'Saudi Arabia', Code: 'RUH' },
                    { Name: 'Bahrain Intl.', City: 'Manama', Country: 'Bahrain', Code: 'BAH' },
                    { Name: 'Muscat Intl.', City: 'Muscat', Country: 'Oman', Code: 'MCT' },
                    { Name: 'Hamad Intl.', City: 'Doha', Country: 'Qatar', Code: 'DOH' },
                    { Name: 'Abu Dhabi Intl.', City: 'Abu Dhabi', Country: 'United Arab Emirates', Code: 'AUH' },
                    { Name: 'Kuwait Intl.', City: 'Kuwait City', Country: 'Kuwait', Code: 'KWI' },
                    { Name: 'Cairo Intl.', City: 'Cairo', Country: 'Egypt', Code: 'CAI' },
                    { Name: 'Queen Alia Intl', City: 'Amman', Country: 'Jordan', Code: 'AMM' }

                ]

                
            },

            // **********
            // General variables
            Loaded: true,
            Busy: false,

            // **********
            // functions

            // refresh page
            Refreshing: false,
            RefreshPage: function () {
                $rootScope.PageConfig.Refreshing = true;
                location.reload();
            },// refresh page

            // body no scroll
            BodyNoScroll: false,
            SetBodyNoScroll: function (state) {
                $rootScope.PageConfig.BodyNoScroll = state;
            }, // body no scroll end

            // toggle burger menu
            BurgerShown: false,
            ToggleBurger : function() {

                if ($rootScope.PageConfig.BurgerShown == false) {
                    $rootScope.PageConfig.BurgerShown = true;
                    $rootScope.PageConfig.SetBodyNoScroll(true);
                } else {
                    $rootScope.PageConfig.BurgerShown = false;
                    $rootScope.PageConfig.SetBodyNoScroll(false);
                }

            }, // toggle burger menu end

            // tab menu
            TabShown: false,
            TabMenu: function () {

                if ($rootScope.PageConfig.TabShown == false) {
                    $rootScope.PageConfig.TabShown = true;
                    $rootScope.PageConfig.SetBodyNoScroll(true);
                } else {
                    $rootScope.PageConfig.TabShown = false;
                    $rootScope.PageConfig.SetBodyNoScroll(false);
                }

            }, // tab menu end

            // page overlay
            ActiveOverlay: '',
            SetOverlay: function (overlay) {
                console.log('changing overlay to : ' + overlay);
                if (typeof(overlay) == 'undefined') {
                    $rootScope.PageConfig.ActiveOverlay = '';
                    $rootScope.PageConfig.SetBodyNoScroll(true);
                } else {
                    if ( overlay == '' || overlay == ' ' ) {
                        $rootScope.PageConfig.ActiveOverlay = '';
                        $rootScope.PageConfig.SetBodyNoScroll(false);
                    } else {
                        $rootScope.PageConfig.ActiveOverlay = overlay;
                        $rootScope.PageConfig.SetBodyNoScroll(true);
                    }
                }
                //if (!overlay) {
                //    $rootScope.PageConfig.ActiveOverlay = '';
                //    $rootScope.PageConfig.BodyNoScroll = false;
                //} else {
                //    $rootScope.PageConfig.ActiveOverlay = overlay;
                //    $rootScope.PageConfig.BodyNoScroll = true;
                //}
            }, // page overlay end

            // page popup
            Popup: '',
            SetPopup: function (popup) {
                if (popup) {
                    $rootScope.PageConfig.Popup = popup;
                    $rootScope.PageConfig.SetBodyNoScroll(true);
                } else {
                    $rootScope.PageConfig.Popup = '';
                    $rootScope.PageConfig.SetBodyNoScroll(false);
                }
            }, // page popup end

        }; // $rootScope.PageConfig

        
        // countries
        $rootScope.Countries = {
            List: Countries,
            GetCountry: function(dialcode) {
                for (var i = 0; i < Countries.length; i++) {
                    if ($rootScope.Countries.List[i].dial_code == dialcode) {
                        return $rootScope.Countries.List[i].name;
                    }
                }
            }
        };//$rootScope.Countries
        $rootScope.flightclass = [
            { name: 'Kelas Ekonomi', value: 'y' },
            { name: 'Kelas Bisnis', value: 'c' },
            { name: 'Kelas Utama', value: 'f' }
        ];

        // datepicker
        
        $rootScope.DatePicker = {
            Settings: {
                MinDate: '',
                DateFormat: '',
                Target: '',
                ChangeMonth: false,
                ChangeYear: false,
                SelectedDate : ''
            },
            ChangeLanguage: function(date) {
                var newdate = "";
                var day = date.substring(0, 3);
                var mth = date.substring(8, 11);
                switch(day) {
                    case 'Sen':
                        newdate += 'Mon, ' + date.substring(5, 7);
                        break;
                    case 'Sel':
                        newdate += 'Tue, ' + date.substring(5, 7);
                        break;
                    case 'Rab':
                        newdate += 'Wed, ' + date.substring(5, 7);
                        break;
                    case 'Kam':
                        newdate += 'Thu, ' + date.substring(5, 7);
                        break;
                    case 'Jum':
                        newdate += 'Fri, ' + date.substring(5, 7);
                        break;
                    case 'Sab':
                        newdate += 'Sat, ' + date.substring(5, 7);
                        break;
                    case 'Min':
                        newdate += 'Sun, ' + date.substring(5, 7);
                        break;
                }

                switch (mth) {
                    case 'Jan':
                        newdate += ' Jan ' + date.substring(12, 16);
                        break;
                    case 'Feb':
                        newdate += ' Feb ' + date.substring(12, 16);
                        break;
                    case 'Mar':
                        newdate += ' Mar ' + date.substring(12, 16);
                        break;
                    case 'Apr':
                        newdate += ' Apr ' + date.substring(12, 16);
                        break;
                    case 'Mei':
                        newdate += ' May ' + date.substring(12, 16);
                        break;
                    case 'Jun':
                        newdate += ' Jun ' + date.substring(12, 16);
                        break;
                    case 'Jul':
                        newdate += ' Jul ' + date.substring(12, 16);
                        break;
                    case 'Agu':
                        newdate += ' Aug ' + date.substring(12, 16);
                        break;
                    case 'Sep':
                        newdate += ' Sep ' + date.substring(12, 16);
                        break;
                    case 'Okt':
                        newdate += ' Oct ' + date.substring(12, 16);
                        break;
                    case 'Nov':
                        newdate += ' Nov ' + date.substring(12, 16);
                        break;
                    case 'Des':
                        newdate += ' Dec ' + date.substring(12, 16);
                        break;
                }

                return newdate;
            },
            SetOption: function (options, overlay, position) {
                overlay = overlay || 'flight-form';
                if (position == 'search-single' || position == 'search-return') {
                    $rootScope.FlightSearchForm.DepartureDate = new Date($rootScope.FlightSearchForm.DepartureDate);
                    $rootScope.FlightSearchForm.ReturnDate = new Date($rootScope.FlightSearchForm.ReturnDate);
                }
                console.log($rootScope.FlightSearchForm.DepartureDate);
                console.log($rootScope.FlightSearchForm.ReturnDate);
                $('.ui-datepicker').datepicker({
                    
                    onSelect: function (date) {
                        
                        var trsdate = $rootScope.DatePicker.ChangeLanguage(date);
                        if (position == 'search-return' || position == 'search-single') {
                            $rootScope.PageConfig.SetOverlay('flight-form');
                        } else {
                            $rootScope.PageConfig.SetOverlay(overlay);
                        }
                       
                        console.log("tes123: "+ trsdate);
                        $rootScope.DatePicker.Settings.SelectedDate = new Date(trsdate);
                        $($rootScope.DatePicker.Settings.Target).val(trsdate);
                        $($rootScope.DatePicker.Settings.Target).trigger('input');
                        
                        var depdate = new Date($rootScope.FlightSearchForm.DepartureDate);
                        var retdate = new Date($rootScope.FlightSearchForm.ReturnDate);
                        if ($rootScope.DatePicker.Settings.Target == '.flight-search-form-departure') {
                            depdate = new Date(trsdate);
                            $rootScope.FlightSearchForm.DepartureDate = depdate;
                        } else {
                            retdate = new Date(trsdate);
                            $rootScope.FlightSearchForm.ReturnDate = retdate;
                        }
                      
                        if (depdate > retdate) {
                            $('.form__departure .field-container span').text(date);
                            $('.form__return .field-container span').text(date);
                            if (position == null) {
                                $('.ui-datepicker.departure-date').datepicker("setDate", new Date(trsdate));
                                $rootScope.FlightSearchForm.ReturnDate = new Date(trsdate);
                            } else if (position == 'search-single' || position == 'search-return') {
                                //$('.form-departure-date span').text(date);
                                //$('.form-return-date span').text(date);
                                $('.ui-datepicker.departure-date').datepicker("setDate", new Date(trsdate));
                                $('.ui-datepicker.return-date').datepicker("setDate", new Date(trsdate));
                                $rootScope.PageConfig.SetOverlay('flight-form');
                            }
                            
                        } else {
                            if ($rootScope.DatePicker.Settings.Target == '.flight-search-form-departure') {
                                $('.form__departure .field-container span').text(date);
                                if (position == null) {
                                    //$('.form__departure .field-container span').text(date);
                                    $('.ui-datepicker.departure-date').datepicker("setDate", new Date(trsdate));
                                } else if (position == 'search-single' || position == 'search-return') {
                                    //$('.form-departure-date span').text(date);
                                    $('.ui-datepicker.departure-date').datepicker("setDate", new Date(trsdate));
                                }
                            } else {
                                //if (position == null) {
                                $('.form__return .field-container span').text(date);
                                //}
                                if (position == 'search-single' || position == 'search-return')
                                {
                                    //$('.form-return-date span').text(date);
                                    $('.ui-datepicker.return-date').datepicker("setDate", new Date(trsdate));

                                }    
                            }
                        }
                    },
                    
                });
                // set default value for datepicker
                //if (options.MinDate) {
                //    $rootScope.DatePicker.Settings.MinDate = options.MinDate;
                //} else {
                //    $rootScope.DatePicker.Settings.MinDate = new Date();
                //}
                
                if (options.Target == 'departure') {
                    //$(".ui-datepicker").datepicker("option", "showOn", "hide");
                    
                    $rootScope.DatePicker.Settings.Target = '.flight-search-form-departure';
                    $('.ui-datepicker').datepicker('option', 'minDate', new Date());
                    if ($rootScope.FlightSearchForm.Trip == "true" && $rootScope.FlightSearchForm.ReturnDate) {
                        var mydate = $rootScope.FlightSearchForm.ReturnDate;
                        var day = mydate.getDay();
                        var mth = mydate.getMonth();
                        $('.ui-datepicker').datepicker('option', 'maxDate', $rootScope.FlightSearchForm.ReturnDate);
                    }
                    
                    
                    else {
                        console.log($rootScope.DatePicker.Settings.SelectedDate);
                        $('.ui-datepicker').datepicker('option', 'maxDate', null);
                    }
                    if ($rootScope.FlightSearchForm.Trip == "true" && $rootScope.FlightSearchForm.DepartureDate) {
                        $('.ui-datepicker').datepicker('option', 'maxDate', null);
                    }
                } else {
                    $rootScope.DatePicker.Settings.Target = '.flight-search-form-return';
                    if ($rootScope.FlightSearchForm.DepartureDate) {
                        $('.ui-datepicker').datepicker('option', 'minDate', $rootScope.FlightSearchForm.DepartureDate);
                    }
                    if ($rootScope.FlightSearchForm.Trip == "true") {
                        if ($rootScope.FlightSearchForm.ReturnDate) {
                            $('.ui-datepicker').datepicker('option', 'minDate', $rootScope.FlightSearchForm.DepartureDate);
                            $('.ui-datepicker').datepicker('option', 'maxDate', null);
                        }
                    }
                   
                }
                $rootScope.DatePicker.Settings.DateFormat = 'D, dd M yy';
                $rootScope.DatePicker.Settings.ChangeMonth = false ;
                $rootScope.DatePicker.Settings.ChangeYear = false;

                // set option to datepicker
                $('.ui-datepicker').datepicker('option', 'prevText', '');
                $('.ui-datepicker').datepicker('option', 'nextText', '');
                $('.ui-datepicker.departure-date').datepicker('option', 'dateFormat', $rootScope.DatePicker.Settings.DateFormat);
                $('.ui-datepicker.return-date').datepicker('option', 'dateFormat', $rootScope.DatePicker.Settings.DateFormat);
                
                // set on choose date function

            },
            SetDefaultReturnDate: function (val) {
                if (val == true) {
                    $rootScope.FlightSearchForm.Trip = "true";
                    //if ($rootScope.FlightSearchForm.DepartureDate.getDate() > $rootScope.FlightSearchForm.ReturnDate.getDate()) {
                    //    $rootScope.FlightSearchForm.ReturnDate.setDate($rootScope.FlightSearchForm.DepartureDate.getDate() + 1);
                    //}
                } else {
                    $rootScope.FlightSearchForm.Trip = "false";
                }

            }
        };// datepicker
        

        // flight search form
        $rootScope.FlightSearchForm = {
            Trip: 'false',
            AirportOrigin: {
                City: 'Jakarta',
                Code: 'CGK',
                Country: 'Indonesia'
            },// origin
            AirportDestination: {
                City: 'Denpasar',
                Code: 'DPS',
                Country: 'Indonesia'
            },// destination
            DepartureDate: '',
            ReturnDate: '',
            Passenger: [1, 0, 0],
            Cabin: 'y',
            AutoComplete: {
                Target: 'departure',
                Keyword: '',
                MinLength: 3,
                GetAirport: function (keyword) {
                    if (trial > 3)
                    {
                        trial = 0;
                    }
                    keyword = keyword || $rootScope.FlightSearchForm.AutoComplete.Keyword;
                    var url = FlightAutocompleteConfig.Url + keyword;

                    if (keyword.length >= $rootScope.FlightSearchForm.AutoComplete.MinLength) {
                        $('autocomplete-loading .text-loading').show();
                        if (typeof ($rootScope.FlightSearchForm.AutoComplete.Cache[keyword]) != "undefined") {
                            $rootScope.FlightSearchForm.AutoComplete.Result = $rootScope.FlightSearchForm.AutoComplete.Cache[keyword];
                            generateSearchResult($rootScope.FlightSearchForm.AutoComplete.Result);
                            if ($rootScope.FlightSearchForm.AutoComplete.Result.length > 0) {
                                $('.autocomplete-no-result').hide();
                                $('.autocomplete-loading .text-loading').hide();
                                $('.autocomplete-result').show();
                            } else {
                                $('.autocomplete-loading .text-loading').hide();
                                $('.autocomplete-result').hide();
                                $('.autocomplete-no-result').show();
                            }
                        } else {
                            $.ajax({
                                url: FlightAutocompleteConfig.Url + keyword,
                                headers: { 'Authorization': 'Bearer ' + getCookie('accesstoken') }
                            }).done(function(returnData) {
                                $('.autocomplete-loading .text-loading').hide();
                                $rootScope.FlightSearchForm.AutoComplete.Loading = false;
                                $rootScope.FlightSearchForm.AutoComplete.Result = returnData.airports;
                                $rootScope.FlightSearchForm.AutoComplete.Cache[keyword] = returnData.airports;
                                generateSearchResult($rootScope.FlightSearchForm.AutoComplete.Result);
                                if (returnData.airports.length > 0) {
                                    $('.autocomplete-no-result').hide();
                                    $('.autocomplete-loading .text-loading').hide();
                                    $('.autocomplete-result').show();
                                } else {
                                    $('.autocomplete-loading .text-loading').hide();
                                    $('.autocomplete-result').hide();
                                    $('.autocomplete-no-result').show();
                                }
                            }).error(function (returnData) {
                                trial++;
                                console.log(trial);
                                if (refreshAuthAccess() && trial < 4) //refresh cookie
                                {
                                    $rootScope.FlightSearchForm.AutoComplete.GetAirport(keyword);
                                }
                            });
                        }
                    }
                },
                Loading: false,
                Result: [],
                Cache: {},
                Reset: function (target) {
                    $rootScope.FlightSearchForm.AutoComplete.Target = target || 'departure';
                    
                    $rootScope.FlightSearchForm.AutoComplete.Keyword = '';
                    $rootScope.FlightSearchForm.AutoComplete.Result = [];
                    $rootScope.FlightSearchForm.AutoComplete.Loading = false;
                },
                SetAirport: function (target, airport) {
                    if (target == 'departure') {
                        if ($rootScope.FlightSearchForm.AirportDestination.City == airport.City) {
                            $('.btnSubmit').addClass('disabled');
                        } else {
                            $('.btnSubmit').removeClass('disabled');
                        }
                        $rootScope.FlightSearchForm.AirportOrigin = airport;
                    } else {
                        if ($rootScope.FlightSearchForm.AirportOrigin.City == airport.City) {
                            $('.btnSubmit').addClass('disabled');
                        } else {
                            $('.btnSubmit').removeClass('disabled');
                        }
                        $rootScope.FlightSearchForm.AirportDestination = airport;
                    }
                    console.log(airport);
                }
            },// auto complete
            PassengerPicker: {
                ActiveType: 'adult',
                TotalMaxPassenger: 9,
                TotalCurrentPassenger: 1,
                PreviousPassenger: [1,0,0],
                List: [1,2,3,4,5,6,7,8,9],
                Reset: function(type) {
                    $rootScope.FlightSearchForm.PassengerPicker.ActiveType = type;
                    var minPassenger = type == 'adult' ? 1 : 0;
                    var maxPassenger = type == 'adult' ? 9 : 8;
                    $rootScope.FlightSearchForm.PassengerPicker.List = [];
                    for (var i = minPassenger ; i <= maxPassenger ; i++) {
                        $rootScope.FlightSearchForm.PassengerPicker.List.push(i);
                    }
                    // reset total current passenger
                    $rootScope.FlightSearchForm.PassengerPicker.PreviousPassenger = $rootScope.FlightSearchForm.Passenger;
                    $rootScope.FlightSearchForm.PassengerPicker.TotalCurrentPassenger = $rootScope.FlightSearchForm.PassengerPicker.PreviousPassenger[0] + $rootScope.FlightSearchForm.PassengerPicker.PreviousPassenger[1] + $rootScope.FlightSearchForm.PassengerPicker.PreviousPassenger[2];
                    switch (type) {
                        case 'adult':
                            $rootScope.FlightSearchForm.PassengerPicker.TotalCurrentPassenger = $rootScope.FlightSearchForm.PassengerPicker.TotalCurrentPassenger - $rootScope.FlightSearchForm.PassengerPicker.PreviousPassenger[0];
                            break;
                        case 'children':
                            $rootScope.FlightSearchForm.PassengerPicker.TotalCurrentPassenger = $rootScope.FlightSearchForm.PassengerPicker.TotalCurrentPassenger - $rootScope.FlightSearchForm.PassengerPicker.PreviousPassenger[1];
                            break;
                        case 'infant':
                            $rootScope.FlightSearchForm.PassengerPicker.TotalCurrentPassenger = $rootScope.FlightSearchForm.PassengerPicker.TotalCurrentPassenger - $rootScope.FlightSearchForm.PassengerPicker.PreviousPassenger[2];
                            break;
                    }
                },
                Set: function (number, overlay) {
                    overlay = overlay || 'flight-form' ;
                    switch ($rootScope.FlightSearchForm.PassengerPicker.ActiveType) {
                        case 'adult':
                            if ((number + $rootScope.FlightSearchForm.PassengerPicker.TotalCurrentPassenger) > $rootScope.FlightSearchForm.PassengerPicker.TotalMaxPassenger ) {
                                console.log('Passenger cannot be more than 9');
                            } else {
                                $rootScope.FlightSearchForm.Passenger[0] = number;
                            }
                            break;
                        case 'children':
                            if ((number + $rootScope.FlightSearchForm.PassengerPicker.TotalCurrentPassenger) > $rootScope.FlightSearchForm.PassengerPicker.TotalMaxPassenger) {
                                console.log('Passenger cannot be more than 9');
                            } else {
                                $rootScope.FlightSearchForm.Passenger[1] = number;
                            }
                            break;
                        case 'infant':
                            if (((number + $rootScope.FlightSearchForm.PassengerPicker.TotalCurrentPassenger) > $rootScope.FlightSearchForm.PassengerPicker.TotalMaxPassenger)) {
                                console.log('Passenger cannot be more than 9');
                            } else {
                                if (number > $rootScope.FlightSearchForm.Passenger[0]) {
                                    console.log('Infant cannot be more than adult');
                                } else {
                                    $rootScope.FlightSearchForm.Passenger[2] = number;
                                }
                            }
                            break;
                    }
                    $rootScope.FlightSearchForm.PassengerPicker.TotalCurrentPassenger = $rootScope.FlightSearchForm.Passenger[0] + $rootScope.FlightSearchForm.Passenger[1] + $rootScope.FlightSearchForm.Passenger[2];
                    $rootScope.PageConfig.SetOverlay(overlay);
                }
            },// passenger picker
            Url: '',
            Submit: function() {
                console.log($rootScope.FlightSearchForm);
                console.log('Generating search form');

                if ($rootScope.FlightSearchForm.DepartureDate == '') {
                    var departure = new Date();
                    departure.setDate( departure.getDate() + 1);
                    $rootScope.FlightSearchForm.DepartureDate = departure;
                }

                if ($rootScope.FlightSearchForm.ReturnDate == '') {
                    var todayDate = new Date();
                    var returnDate = new Date();
                    returnDate.setDate(todayDate.getDate() + 2);
                    $rootScope.FlightSearchForm.ReturnDate = returnDate;
                } 

                $rootScope.FlightSearchForm.Url = FlightSearchConfig.GenerateSearchParam({
                    trip: ($rootScope.FlightSearchForm.Trip == 'true'),
                    departureDate: $rootScope.FlightSearchForm.DepartureDate,
                    returnDate: $rootScope.FlightSearchForm.ReturnDate,
                    origin: $rootScope.FlightSearchForm.AirportOrigin.Code,
                    destination: $rootScope.FlightSearchForm.AirportDestination.Code,
                    adult: $rootScope.FlightSearchForm.Passenger[0],
                    children: $rootScope.FlightSearchForm.Passenger[1],
                    infant: $rootScope.FlightSearchForm.Passenger[2],
                    cabin: $rootScope.FlightSearchForm.Cabin
                });

                // redirect page to search page
                window.location = window.location.origin + '/id/Flight/Search?info=' + $rootScope.FlightSearchForm.Url  ;

            }// submit
        };//$rootScope.FlightSearchForm

        $('.autocomplete-result ul.result').on('click', 'li', function (overlay) {

            //var overlay = '' || flight
            var locationCode = $(this).find('.airport__code').html();
            var locationCity = $(this).find('.airport__location').html().split(",")[0];
            var locationCountry = $(this).find('.airport__location').html().split(",")[1].trim();
            var locationName = $(this).find('.airport__name').html();
            var airport = { City: locationCity, Code: locationCode, Country: locationCountry, Name: locationName };
            if ($rootScope.FlightSearchForm.AutoComplete.Target == 'departure') {
                if ($rootScope.FlightSearchForm.AirportDestination.City == airport.City) {
                    $('.btnSubmit').addClass('disabled');
                } else {
                    $('.btnSubmit').removeClass('disabled');
                }
                $rootScope.FlightSearchForm.AirportOrigin = airport;
            } else {
                if ($rootScope.FlightSearchForm.AirportOrigin.City == airport.City) {
                    $('.btnSubmit').addClass('disabled');
                } else {
                    $('.btnSubmit').removeClass('disabled');
                }
                $rootScope.FlightSearchForm.AirportDestination = airport;
            }
            //$rootScope.PageConfig.SetOverlay();
        });

        function generateSearchResult(list) {
            $('.autocomplete-result ul').empty();
            for (var i = 0 ; i < list.length; i++) {
                $('.autocomplete-result ul').append
                (
                    '<li class="airport">' + 
                        '<a class="airport__link">' + 
                        '</a>' +
                        '<span class="airport__code">'+ list[i].code +'</span>' +
                        '<div class="airport__detail">' +
                            '<p class="airport__location">' + list[i].city + ', ' + list[i].country + '</p>' +
                            '<p class="airport__name">' + list[i].name + '</p>' +
                        '</div>' +
                      '</li>'
                );
            }
        }

        $rootScope.initAirport = function(target, airport, city) {
            if (target == 'departure') {
                $rootScope.FlightSearchForm.AirportOrigin.Code = airport;
                $rootScope.FlightSearchForm.AirportOrigin.City = city;
            } else {
                $rootScope.FlightSearchForm.AirportDestination.Code = airport;
                $rootScope.FlightSearchForm.AirportDestination.City = city;
            }
        }

        // set default date for departure date and return date
        if ($rootScope.FlightSearchForm.DepartureDate == '') {
            var departure = new Date();
            departure.setDate(departure.getDate());
            $rootScope.FlightSearchForm.DepartureDate = departure;
        }
        if ($rootScope.FlightSearchForm.ReturnDate == '') {
            //var todayDate = new Date();
            var returnDate = new Date();
            returnDate.setDate(returnDate.getDate());
            $rootScope.FlightSearchForm.ReturnDate = returnDate;
        } 
    });//app.run

}

// --------------------
// general variables

// countries list
var Countries = [{ "name": "Afghanistan", "dial_code": "93", "code": "AF" }, { "name": "Aland Islands", "dial_code": "358", "code": "AX" }, { "name": "Albania", "dial_code": "355", "code": "AL" }, { "name": "Algeria", "dial_code": "213", "code": "DZ" }, { "name": "American Samoa", "dial_code": "1 684", "code": "AS" }, { "name": "Andorra", "dial_code": "376", "code": "AD" }, { "name": "Angola", "dial_code": "244", "code": "AO" }, { "name": "Anguilla", "dial_code": "1 264", "code": "AI" }, { "name": "Antarctica", "dial_code": "672", "code": "AQ" }, { "name": "Antigua and Barbuda", "dial_code": "1268", "code": "AG" }, { "name": "Argentina", "dial_code": "54", "code": "AR" }, { "name": "Armenia", "dial_code": "374", "code": "AM" }, { "name": "Aruba", "dial_code": "297", "code": "AW" }, { "name": "Australia", "dial_code": "61", "code": "AU" }, { "name": "Austria", "dial_code": "43", "code": "AT" }, { "name": "Azerbaijan", "dial_code": "994", "code": "AZ" }, { "name": "Bahamas", "dial_code": "1 242", "code": "BS" }, { "name": "Bahrain", "dial_code": "973", "code": "BH" }, { "name": "Bangladesh", "dial_code": "880", "code": "BD" }, { "name": "Barbados", "dial_code": "1 246", "code": "BB" }, { "name": "Belarus", "dial_code": "375", "code": "BY" }, { "name": "Belgium", "dial_code": "32", "code": "BE" }, { "name": "Belize", "dial_code": "501", "code": "BZ" }, { "name": "Benin", "dial_code": "229", "code": "BJ" }, { "name": "Bermuda", "dial_code": "1 441", "code": "BM" }, { "name": "Bhutan", "dial_code": "975", "code": "BT" }, { "name": "Bolivia, Plurinational State of", "dial_code": "591", "code": "BO" }, { "name": "Bosnia and Herzegovina", "dial_code": "387", "code": "BA" }, { "name": "Botswana", "dial_code": "267", "code": "BW" }, { "name": "Brazil", "dial_code": "55", "code": "BR" }, { "name": "British Indian Ocean Territory", "dial_code": "246", "code": "IO" }, { "name": "Brunei Darussalam", "dial_code": "673", "code": "BN" }, { "name": "Bulgaria", "dial_code": "359", "code": "BG" }, { "name": "Burkina Faso", "dial_code": "226", "code": "BF" }, { "name": "Burundi", "dial_code": "257", "code": "BI" }, { "name": "Cambodia", "dial_code": "855", "code": "KH" }, { "name": "Cameroon", "dial_code": "237", "code": "CM" }, { "name": "Canada", "dial_code": "1", "code": "CA" }, { "name": "Cape Verde", "dial_code": "238", "code": "CV" }, { "name": "Cayman Islands", "dial_code": " 345", "code": "KY" }, { "name": "Central African Republic", "dial_code": "236", "code": "CF" }, { "name": "Chad", "dial_code": "235", "code": "TD" }, { "name": "Chile", "dial_code": "56", "code": "CL" }, { "name": "China", "dial_code": "86", "code": "CN" }, { "name": "Christmas Island", "dial_code": "61", "code": "CX" }, { "name": "Cocos (Keeling) Islands", "dial_code": "61", "code": "CC" }, { "name": "Colombia", "dial_code": "57", "code": "CO" }, { "name": "Comoros", "dial_code": "269", "code": "KM" }, { "name": "Congo", "dial_code": "242", "code": "CG" }, { "name": "Congo, The Democratic Republic of the Congo", "dial_code": "243", "code": "CD" }, { "name": "Cook Islands", "dial_code": "682", "code": "CK" }, { "name": "Costa Rica", "dial_code": "506", "code": "CR" }, { "name": "Cote d'Ivoire", "dial_code": "225", "code": "CI" }, { "name": "Croatia", "dial_code": "385", "code": "HR" }, { "name": "Cuba", "dial_code": "53", "code": "CU" }, { "name": "Cyprus", "dial_code": "357", "code": "CY" }, { "name": "Czech Republic", "dial_code": "420", "code": "CZ" }, { "name": "Denmark", "dial_code": "45", "code": "DK" }, { "name": "Djibouti", "dial_code": "253", "code": "DJ" }, { "name": "Dominica", "dial_code": "1 767", "code": "DM" }, { "name": "Dominican Republic", "dial_code": "1 849", "code": "DO" }, { "name": "Ecuador", "dial_code": "593", "code": "EC" }, { "name": "Egypt", "dial_code": "20", "code": "EG" }, { "name": "El Salvador", "dial_code": "503", "code": "SV" }, { "name": "Equatorial Guinea", "dial_code": "240", "code": "GQ" }, { "name": "Eritrea", "dial_code": "291", "code": "ER" }, { "name": "Estonia", "dial_code": "372", "code": "EE" }, { "name": "Ethiopia", "dial_code": "251", "code": "ET" }, { "name": "Falkland Islands (Malvinas)", "dial_code": "500", "code": "FK" }, { "name": "Faroe Islands", "dial_code": "298", "code": "FO" }, { "name": "Fiji", "dial_code": "679", "code": "FJ" }, { "name": "Finland", "dial_code": "358", "code": "FI" }, { "name": "France", "dial_code": "33", "code": "FR" }, { "name": "French Guiana", "dial_code": "594", "code": "GF" }, { "name": "French Polynesia", "dial_code": "689", "code": "PF" }, { "name": "Gabon", "dial_code": "241", "code": "GA" }, { "name": "Gambia", "dial_code": "220", "code": "GM" }, { "name": "Georgia", "dial_code": "995", "code": "GE" }, { "name": "Germany", "dial_code": "49", "code": "DE" }, { "name": "Ghana", "dial_code": "233", "code": "GH" }, { "name": "Gibraltar", "dial_code": "350", "code": "GI" }, { "name": "Greece", "dial_code": "30", "code": "GR" }, { "name": "Greenland", "dial_code": "299", "code": "GL" }, { "name": "Grenada", "dial_code": "1 473", "code": "GD" }, { "name": "Guadeloupe", "dial_code": "590", "code": "GP" }, { "name": "Guam", "dial_code": "1 671", "code": "GU" }, { "name": "Guatemala", "dial_code": "502", "code": "GT" }, { "name": "Guernsey", "dial_code": "44", "code": "GG" }, { "name": "Guinea", "dial_code": "224", "code": "GN" }, { "name": "Guinea-Bissau", "dial_code": "245", "code": "GW" }, { "name": "Guyana", "dial_code": "595", "code": "GY" }, { "name": "Haiti", "dial_code": "509", "code": "HT" }, { "name": "Holy See (Vatican City State)", "dial_code": "379", "code": "VA" }, { "name": "Honduras", "dial_code": "504", "code": "HN" }, { "name": "Hong Kong", "dial_code": "852", "code": "HK" }, { "name": "Hungary", "dial_code": "36", "code": "HU" }, { "name": "Iceland", "dial_code": "354", "code": "IS" }, { "name": "India", "dial_code": "91", "code": "IN" }, { "name": "Indonesia", "dial_code": "62", "code": "ID" }, { "name": "Iran, Islamic Republic of Persian Gulf", "dial_code": "98", "code": "IR" }, { "name": "Iraq", "dial_code": "964", "code": "IQ" }, { "name": "Ireland", "dial_code": "353", "code": "IE" }, { "name": "Isle of Man", "dial_code": "44", "code": "IM" }, { "name": "Israel", "dial_code": "972", "code": "IL" }, { "name": "Italy", "dial_code": "39", "code": "IT" }, { "name": "Jamaica", "dial_code": "1 876", "code": "JM" }, { "name": "Japan", "dial_code": "81", "code": "JP" }, { "name": "Jersey", "dial_code": "44", "code": "JE" }, { "name": "Jordan", "dial_code": "962", "code": "JO" }, { "name": "Kazakhstan", "dial_code": "7 7", "code": "KZ" }, { "name": "Kenya", "dial_code": "254", "code": "KE" }, { "name": "Kiribati", "dial_code": "686", "code": "KI" }, { "name": "Korea, Democratic People's Republic of Korea", "dial_code": "850", "code": "KP" }, { "name": "Korea, Republic of South Korea", "dial_code": "82", "code": "KR" }, { "name": "Kuwait", "dial_code": "965", "code": "KW" }, { "name": "Kyrgyzstan", "dial_code": "996", "code": "KG" }, { "name": "Laos", "dial_code": "856", "code": "LA" }, { "name": "Latvia", "dial_code": "371", "code": "LV" }, { "name": "Lebanon", "dial_code": "961", "code": "LB" }, { "name": "Lesotho", "dial_code": "266", "code": "LS" }, { "name": "Liberia", "dial_code": "231", "code": "LR" }, { "name": "Libyan Arab Jamahiriya", "dial_code": "218", "code": "LY" }, { "name": "Liechtenstein", "dial_code": "423", "code": "LI" }, { "name": "Lithuania", "dial_code": "370", "code": "LT" }, { "name": "Luxembourg", "dial_code": "352", "code": "LU" }, { "name": "Macao", "dial_code": "853", "code": "MO" }, { "name": "Macedonia", "dial_code": "389", "code": "MK" }, { "name": "Madagascar", "dial_code": "261", "code": "MG" }, { "name": "Malawi", "dial_code": "265", "code": "MW" }, { "name": "Malaysia", "dial_code": "60", "code": "MY" }, { "name": "Maldives", "dial_code": "960", "code": "MV" }, { "name": "Mali", "dial_code": "223", "code": "ML" }, { "name": "Malta", "dial_code": "356", "code": "MT" }, { "name": "Marshall Islands", "dial_code": "692", "code": "MH" }, { "name": "Martinique", "dial_code": "596", "code": "MQ" }, { "name": "Mauritania", "dial_code": "222", "code": "MR" }, { "name": "Mauritius", "dial_code": "230", "code": "MU" }, { "name": "Mayotte", "dial_code": "262", "code": "YT" }, { "name": "Mexico", "dial_code": "52", "code": "MX" }, { "name": "Micronesia, Federated States of Micronesia", "dial_code": "691", "code": "FM" }, { "name": "Moldova", "dial_code": "373", "code": "MD" }, { "name": "Monaco", "dial_code": "377", "code": "MC" }, { "name": "Mongolia", "dial_code": "976", "code": "MN" }, { "name": "Montenegro", "dial_code": "382", "code": "ME" }, { "name": "Montserrat", "dial_code": "1664", "code": "MS" }, { "name": "Morocco", "dial_code": "212", "code": "MA" }, { "name": "Mozambique", "dial_code": "258", "code": "MZ" }, { "name": "Myanmar", "dial_code": "95", "code": "MM" }, { "name": "Namibia", "dial_code": "264", "code": "NA" }, { "name": "Nauru", "dial_code": "674", "code": "NR" }, { "name": "Nepal", "dial_code": "977", "code": "NP" }, { "name": "Netherlands", "dial_code": "31", "code": "NL" }, { "name": "Netherlands Antilles", "dial_code": "599", "code": "AN" }, { "name": "New Caledonia", "dial_code": "687", "code": "NC" }, { "name": "New Zealand", "dial_code": "64", "code": "NZ" }, { "name": "Nicaragua", "dial_code": "505", "code": "NI" }, { "name": "Niger", "dial_code": "227", "code": "NE" }, { "name": "Nigeria", "dial_code": "234", "code": "NG" }, { "name": "Niue", "dial_code": "683", "code": "NU" }, { "name": "Norfolk Island", "dial_code": "672", "code": "NF" }, { "name": "Northern Mariana Islands", "dial_code": "1 670", "code": "MP" }, { "name": "Norway", "dial_code": "47", "code": "NO" }, { "name": "Oman", "dial_code": "968", "code": "OM" }, { "name": "Pakistan", "dial_code": "92", "code": "PK" }, { "name": "Palau", "dial_code": "680", "code": "PW" }, { "name": "Palestinian Territory, Occupied", "dial_code": "970", "code": "PS" }, { "name": "Panama", "dial_code": "507", "code": "PA" }, { "name": "Papua New Guinea", "dial_code": "675", "code": "PG" }, { "name": "Paraguay", "dial_code": "595", "code": "PY" }, { "name": "Peru", "dial_code": "51", "code": "PE" }, { "name": "Philippines", "dial_code": "63", "code": "PH" }, { "name": "Pitcairn", "dial_code": "872", "code": "PN" }, { "name": "Poland", "dial_code": "48", "code": "PL" }, { "name": "Portugal", "dial_code": "351", "code": "PT" }, { "name": "Puerto Rico", "dial_code": "1 939", "code": "PR" }, { "name": "Qatar", "dial_code": "974", "code": "QA" }, { "name": "Romania", "dial_code": "40", "code": "RO" }, { "name": "Russia", "dial_code": "7", "code": "RU" }, { "name": "Rwanda", "dial_code": "250", "code": "RW" }, { "name": "Reunion", "dial_code": "262", "code": "RE" }, { "name": "Saint Barthelemy", "dial_code": "590", "code": "BL" }, { "name": "Saint Helena, Ascension and Tristan Da Cunha", "dial_code": "290", "code": "SH" }, { "name": "Saint Kitts and Nevis", "dial_code": "1 869", "code": "KN" }, { "name": "Saint Lucia", "dial_code": "1 758", "code": "LC" }, { "name": "Saint Martin", "dial_code": "590", "code": "MF" }, { "name": "Saint Pierre and Miquelon", "dial_code": "508", "code": "PM" }, { "name": "Saint Vincent and the Grenadines", "dial_code": "1 784", "code": "VC" }, { "name": "Samoa", "dial_code": "685", "code": "WS" }, { "name": "San Marino", "dial_code": "378", "code": "SM" }, { "name": "Sao Tome and Principe", "dial_code": "239", "code": "ST" }, { "name": "Saudi Arabia", "dial_code": "966", "code": "SA" }, { "name": "Senegal", "dial_code": "221", "code": "SN" }, { "name": "Serbia", "dial_code": "381", "code": "RS" }, { "name": "Seychelles", "dial_code": "248", "code": "SC" }, { "name": "Sierra Leone", "dial_code": "232", "code": "SL" }, { "name": "Singapore", "dial_code": "65", "code": "SG" }, { "name": "Slovakia", "dial_code": "421", "code": "SK" }, { "name": "Slovenia", "dial_code": "386", "code": "SI" }, { "name": "Solomon Islands", "dial_code": "677", "code": "SB" }, { "name": "Somalia", "dial_code": "252", "code": "SO" }, { "name": "South Africa", "dial_code": "27", "code": "ZA" }, { "name": "South Georgia and the South Sandwich Islands", "dial_code": "500", "code": "GS" }, { "name": "Spain", "dial_code": "34", "code": "ES" }, { "name": "Sri Lanka", "dial_code": "94", "code": "LK" }, { "name": "Sudan", "dial_code": "249", "code": "SD" }, { "name": "Suriname", "dial_code": "597", "code": "SR" }, { "name": "Svalbard and Jan Mayen", "dial_code": "47", "code": "SJ" }, { "name": "Swaziland", "dial_code": "268", "code": "SZ" }, { "name": "Sweden", "dial_code": "46", "code": "SE" }, { "name": "Switzerland", "dial_code": "41", "code": "CH" }, { "name": "Syrian Arab Republic", "dial_code": "963", "code": "SY" }, { "name": "Taiwan", "dial_code": "886", "code": "TW" }, { "name": "Tajikistan", "dial_code": "992", "code": "TJ" }, { "name": "Tanzania, United Republic of Tanzania", "dial_code": "255", "code": "TZ" }, { "name": "Thailand", "dial_code": "66", "code": "TH" }, { "name": "Timor-Leste", "dial_code": "670", "code": "TL" }, { "name": "Togo", "dial_code": "228", "code": "TG" }, { "name": "Tokelau", "dial_code": "690", "code": "TK" }, { "name": "Tonga", "dial_code": "676", "code": "TO" }, { "name": "Trinidad and Tobago", "dial_code": "1 868", "code": "TT" }, { "name": "Tunisia", "dial_code": "216", "code": "TN" }, { "name": "Turkey", "dial_code": "90", "code": "TR" }, { "name": "Turkmenistan", "dial_code": "993", "code": "TM" }, { "name": "Turks and Caicos Islands", "dial_code": "1 649", "code": "TC" }, { "name": "Tuvalu", "dial_code": "688", "code": "TV" }, { "name": "Uganda", "dial_code": "256", "code": "UG" }, { "name": "Ukraine", "dial_code": "380", "code": "UA" }, { "name": "United Arab Emirates", "dial_code": "971", "code": "AE" }, { "name": "United Kingdom", "dial_code": "44", "code": "GB" }, { "name": "United States", "dial_code": "1", "code": "US" }, { "name": "Uruguay", "dial_code": "598", "code": "UY" }, { "name": "Uzbekistan", "dial_code": "998", "code": "UZ" }, { "name": "Vanuatu", "dial_code": "678", "code": "VU" }, { "name": "Venezuela, Bolivarian Republic of Venezuela", "dial_code": "58", "code": "VE" }, { "name": "Vietnam", "dial_code": "84", "code": "VN" }, { "name": "Virgin Islands, British", "dial_code": "1 284", "code": "VG" }, { "name": "Virgin Islands, U.S.", "dial_code": "1 340", "code": "VI" }, { "name": "Wallis and Futuna", "dial_code": "681", "code": "WF" }, { "name": "Yemen", "dial_code": "967", "code": "YE" }, { "name": "Zambia", "dial_code": "260", "code": "ZM" }, { "name": "Zimbabwe", "dial_code": "263", "code": "ZW" }];

var getCountry = function (dialCode) {
    for (var i = 0; i < Countries.length; i++) {
        if (Countries[i].dial_code == dialCode) {
            return Countries[i].name;
        }
    }
}
// --------------------
// general functions

// get parameter
function getParam(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

function getAnonymousFirstAccess() {
    var status = 0;
    $.ajax({
        url: LoginConfig.Url,
        method: 'POST',
        async: false,
        data: JSON.stringify({ "clientId": "Jajal", "clientSecret": "Standar" }),
        contentType: 'application/json',
    }).done(function (returnData) {
        if (returnData.status == '200') {
            setCookie("accesstoken", returnData.accessToken, returnData.expTime);
            setCookie("refreshtoken", returnData.refreshToken, returnData.expTime);
            if (getCookie('accesstoken')) {
                status = 1;
            }
            else {
                status = 0;
            }
        }
        else {
            status = 0;
        }
    });
    return status;
}


function getAnonymousAccessByRefreshToken(refreshToken) {
    var status = 0;
    $.ajax({
        url: LoginConfig.Url,
        method: 'POST',
        async: false,
        data: JSON.stringify({ "refreshtoken": refreshToken, "clientId": "Jajal", "clientSecret": "Standar" }),
        contentType: 'application/json',
    }).done(function (returnData) {
        if (returnData.status == '200') {
            setCookie("accesstoken", returnData.accessToken, returnData.expTime);
            setCookie("refreshtoken", returnData.refreshToken, returnData.expTime);
            if (getCookie('accesstoken')) {
                status = 1;
            }
            else {
                status = 0;
            }
        }
        else {
            status = 0;
        }
    });
    return status;
}


function getLoginAccessByRefreshToken(refreshToken) {
    var status = 0;
    $.ajax({
        url: LoginConfig.Url,
        method: 'POST',
        async: false,
        data: JSON.stringify({ "refreshtoken": refreshToken, "clientId": "Jajal", "clientSecret": "Standar" }),
        contentType: 'application/json',
    }).done(function (returnData) {
        if (returnData.status == '200') {
            setCookie("accesstoken", returnData.accessToken, returnData.expTime);
            setCookie("refreshtoken", returnData.refreshToken, returnData.expTime);
            setCookie("authkey", returnData.accessToken, returnData.expTime);
            if (getCookie('accesstoken')) {
                status = 2;
            }
            else {
                status = 0;
            }
        }
        else {
            status = 0;
        }
    });
    return status;
}

function getAuthAccess() {
    var token = getCookie('accesstoken');
    var refreshToken = getCookie('refreshtoken');
    var authKey = getCookie('authkey');
    var status = 0;

    if (authKey) {
        if (token) {
            return 2;
        }
        else {
            if (refreshToken) {
                status = getLoginAccessByRefreshToken(refreshToken);
                if (status == 0) {
                    status = getAnonymousFirstAccess();
                }
            }
            else {
                return 0; //harusnya gak pernah masuk sini
            }
        }
    }
    else {
        if (token) {
            return 1;
        }
        else {
            //Get Anonymous Token By Refresh Token
            if (refreshToken) {
                status = getAnonymousAccessByRefreshToken(refreshToken);
                if (status == 0) {
                    status = getAnonymousFirstAccess();
                }
            }
            else {
                //For Anynomius at first
                status = getAnonymousFirstAccess();
            }
        }
    }
    return status;
}


function refreshAuthAccess() {
    /*
    * If failed to get Authorization, but accesstoken is still exist
    */
    var token = getCookie('accesstoken');
    var refreshToken = getCookie('refreshtoken');
    var authKey = getCookie('authkey');
    var status = 0;
    if (refreshToken) {
        if (authKey) {
            status = getLoginAccessByRefreshToken(refreshToken);
            if (status == 0) {
                status = getAnonymousFirstAccess();
                eraseCookie('authkey');
            }

            if (status == 2) {
                return true;
            }
            else {
                return false;
            }
        }
        else {
            status = getAnonymousAccessByRefreshToken(refreshToken);
            if (status == 0) {
                status = getAnonymousFirstAccess();
            }

            if (status == 1 || status == 2) {
                return true;
            }
            else {
                return false;
            }
        }
    }
    else {
        getAnonymousFirstAccess();
        return true;
    }
}

//********************
// accordion functions
function accordionFunctions() {
    //Accordion Help Section by W3School
    var acc = document.getElementsByClassName("accordion");
    var i;

    for (i = 0; i < acc.length; i++) {
        acc[i].onclick = function () {
            this.classList.toggle("active");
            this.nextElementSibling.classList.toggle("show");
        }
    }
}

//    $rootScope.FlightSearchForm.AutoComplete.Loading = true;
//    // if result exist in cache
//    if (keyword in $rootScope.FlightSearchForm.AutoComplete.Cache) {
//        $rootScope.FlightSearchForm.AutoComplete.Result = $rootScope.FlightSearchForm.AutoComplete.Cache[keyword];
//        $rootScope.FlightSearchForm.AutoComplete.Loading = false;
//    } else {
//        $.get(url).done(
//            function (returnData) {
//                $rootScope.FlightSearchForm.AutoComplete.Result = returnData;
//                $rootScope.FlightSearchForm.AutoComplete.Loading = false;
//                // add result to cache
//                $rootScope.FlightSearchForm.AutoComplete.Cache[keyword] = returnData;
//            }
//        ).fail(
//            function (returnData) {
//                console.log('Failed to get airport list');
//                console.log(returnData);
//                $rootScope.FlightSearchForm.AutoComplete.Loading = false;
//            }
//        );
//    }
//}