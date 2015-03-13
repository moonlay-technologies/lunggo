using System.Collections.Generic;

namespace Lunggo.Flight.Dictionary
{
    public static class FlightCode
    {
        public static readonly Dictionary<int, AirlineCode> Airline = PopulateAirline();
        public static readonly Dictionary<int, AirportCode> Airport = PopulateAirport();

        private static Dictionary<int, AirportCode> PopulateAirport()
        {
            return new Dictionary<int, AirportCode>
            {
                {1, new AirportCode ("BTH", "Hang Nadim Int'l", "Batam", "Sumatera")},
                {2, new AirportCode ("BTJ", "Sultan Iskandar Muda Int'l", "Banda Aceh", "Sumatera")},
                {3, new AirportCode ("KNO", "Kuala Namu Int'l", "Deli Serdang", "Sumatera")},
                {4, new AirportCode ("SIX", "Dr. Ferdinand Lumban Tobing", "Sibolga", "Sumatera")},
                {5, new AirportCode ("SGT", "Silangit Int'l", "Siborong-borong", "Sumatera")},
                {6, new AirportCode ("LSW", "Malikus Saleh", "Lhokseumawe", "Sumatera")},
                {7, new AirportCode ("RGT", "Japura", "Rengat", "Sumatera")},
                {8, new AirportCode ("MEQ", "Cut Nyak Dhien", "Nagan Raya", "Sumatera")},
                {9, new AirportCode ("PDG", "Minangkabau Int'l", "Kota Padang", "Sumatera")},
                {10, new AirportCode ("PKU", "Sultan Syarif Kasim II Int'l", "Pekanbaru", "Sumatera")},
                {11, new AirportCode ("PLM", "Sultan Mahmud Badaruddin II Int'l", "Palembang", "Sumatera")},
                {12, new AirportCode ("TNJ", "Raja Haji Fisabilillah Int'l", "Tanjungpinang", "Sumatera")},
                {13, new AirportCode ("BDO", "Husein Sastranegara Int'l", "Bandung", "Jawa")},
                {14, new AirportCode ("CGK", "Soekarno-Hatta Int'l", "Jakarta", "Jawa")},
                {15, new AirportCode ("JOG", "Adi Sucipto Int'l", "Yogyakarta", "Jawa")},
                {16, new AirportCode ("SOC", "Adisumarmo Int'l", "Solo", "Jawa")},
                {17, new AirportCode ("SRG", "Achmad Yani Int'l", "Semarang", "Jawa")},
                {18, new AirportCode ("SUB", "Juanda Int'l", "Surabaya", "Jawa")},
                {19, new AirportCode ("MSI", "Valia Rahma Int'l", "Masalembo", "Jawa")},
                {20, new AirportCode ("JBB", "Notohadinegoro", "Jember", "Jawa")},
                {21, new AirportCode ("BWX", "Blimbingsari", "Banyuwangi", "Jawa")},
                {22, new AirportCode ("DPS", "Ngurah Rai Int'l", "Denpasar", "Bali")},
                {23, new AirportCode ("LOP", "Lombok Int'l", "Lombok Tengah", "Nusa Tenggara")},
                {24, new AirportCode ("MOF", "Wai Oti", "Maumere", "Nusa Tenggara")},
                {25, new AirportCode ("TMC", "Tambolaka", "Waikabubak", "Nusa Tenggara")},
                {26, new AirportCode ("LKA", "Gewayantana", "Larantuka", "Nusa Tenggara")},
                {27, new AirportCode ("SWQ", "Sultan Muhammad Kaharuddin III", "Sumbawa Besar", "Nusa Tenggara")},
                {28, new AirportCode ("MLK", "Melalan", "Sendawar", "Kalimantan")},
                {29, new AirportCode ("PKY", "Tjilik Riwut", "Palangka Raya", "Kalimantan")},
                {30, new AirportCode ("TRK", "Juwata Int'l", "Tarakan", "Kalimantan")},
                {31, new AirportCode ("SRI", "Temindung", "Samarinda", "Kalimantan")},
                {32, new AirportCode ("BEJ", "Kalimarau Int'l", "Berau", "Kalimantan")},
                {33, new AirportCode ("BPN", "Sultan Aji Muhammad Sulaiman", "Balikpapan", "Kalimantan")},
                {34, new AirportCode ("NNX", "Warukin", "Tabalong", "Kalimantan")},
                {35, new AirportCode ("BDJ", "Syamsuddin Noor Int'l", "Banjarmasin", "Kalimantan")},
                {36, new AirportCode ("MTW", "Beringin", "Muara Teweh", "Kalimantan")},
                {37, new AirportCode ("MDC", "Sam Ratulangi Int'l", "Manado", "Sulawesi")},
                {38, new AirportCode ("UPG", "Sultan Hasanuddin Int'l", "Makassar", "Sulawesi")},
                {39, new AirportCode ("KDI", "Haluoleo Int'l", "Kendari", "Sulawesi")},
                {40, new AirportCode ("LUW", "Syukuran Aminuddin Amir", "Luwuk", "Sulawesi")},
                {41, new AirportCode ("GTO", "Jalaluddin", "Gorontalo", "Sulawesi")},
                {42, new AirportCode ("WKB", "Matahora", "Wangi-wangi", "Sulawesi")},
                {43, new AirportCode ("TMI", "Maranggo", "Pulau Tomia", "Sulawesi")},
                {44, new AirportCode ("NBX", "Yos Sudarso Int'l", "Nabire", "Papua")},
                {45, new AirportCode ("BIK", "Frans Kaisiepo", "Biak", "Papua")},
                {46, new AirportCode ("ORG", "Iskak Int'l", "Oksibil", "Papua")},
                {47, new AirportCode ("TMH", "Tanah Merah", "Tanah Merah", "Papua")},

                {48, new AirportCode ("ICN", "Incheon Int'l", "Seoul", "Korea Selatan")},
                {49, new AirportCode ("SIN", "Changi Int'l", "Singapura", "Singapura")},
                {50, new AirportCode ("KTM", "Tribhuvan Int'l", "Kathmandu", "Nepal")},
                {51, new AirportCode ("KUL", "KLIA", "Kuala Lumpur", "Malaysia")}
            };
        }

        private static Dictionary<int, AirlineCode> PopulateAirline()
        {
            return new Dictionary<int, AirlineCode>
            {
                {1, new AirlineCode ("QZ", "AirAsia")},
                {2, new AirlineCode ("JT", "Lion Air")},
                {3, new AirlineCode ("SJ", "Sriwijaya Air")},
                {4, new AirlineCode ("QG", "Citylink")},
                {5, new AirlineCode ("MZ", "Merpati Nusantara")},
                {6, new AirlineCode ("MV", "Aviastar")},
                {7, new AirlineCode ("ID", "Batik Air")},
                {8, new AirlineCode ("TN", "Trigana Air")},
                {9, new AirlineCode ("KD", "KalStar Aviation")},
                {10, new AirlineCode ("FS", "Airfast Indonesia")},
                {11, new AirlineCode ("IW", "Wings Air")},
                {12, new AirlineCode ("XN", "Express Air")},
                {13, new AirlineCode ("SY", "Sky Aviation")},
                {14, new AirlineCode ("SI", "Susi Air")},
                {15, new AirlineCode ("GA", "Garuda Indonesia")},
                {16, new AirlineCode ("CA", "Air China")},
                {17, new AirlineCode ("AK", "AirAsia")},
                {18, new AirlineCode ("NH", "All Nippon Airways")},
                {19, new AirlineCode ("CX", "Cathay Pacific")},
                {20, new AirlineCode ("5J", "Cebu Pacific")},
                {21, new AirlineCode ("CI", "China Airlines")},
                {22, new AirlineCode ("CZ", "China Southern")},
                {23, new AirlineCode ("EK", "Emirates")},
                {24, new AirlineCode ("EY", "Etihad Airways")},
                {25, new AirlineCode ("BR", "EVA Air")},
                {26, new AirlineCode ("JL", "Japan Airlines")},
                {27, new AirlineCode ("JQ", "Jetstar Airways")},
                {28, new AirlineCode ("KL", "KLM")},
                {29, new AirlineCode ("KE", "Korean Air")},
                {30, new AirlineCode ("KU", "Kuwait Airways")},
                {31, new AirlineCode ("MH", "Malaysia Airlines")},
                {32, new AirlineCode ("MJ", "Mihin Lanka")},
                {33, new AirlineCode ("PR", "Philipphine Airlines")},
                {34, new AirlineCode ("QF", "Qantas")},
                {35, new AirlineCode ("QR", "Qatar Airways")},
                {36, new AirlineCode ("BI", "Royal Brunei Airlines")},
                {37, new AirlineCode ("SV", "Saudi Arabian Airlines")},
                {38, new AirlineCode ("3U", "Sichuan Airlines")},
                {39, new AirlineCode ("SQ", "Singapore Airlines")},
                {40, new AirlineCode ("TQ", "Thai Airways")},
                {41, new AirlineCode ("TR", "Tiger Airways")},
                {42, new AirlineCode ("TK", "Turkish Airlines")},
                {43, new AirlineCode ("VF", "Valuair")},
                {44, new AirlineCode ("VN", "Vietnam Airlines")},
                {45, new AirlineCode ("IY", "Yemenia")},
            };
        }
    }

    public class AirlineCode
    {
        public string Code;
        public string Name;

        public AirlineCode(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }

    public class AirportCode
    {
        public string Code;
        public string Name;
        public string City;
        public string Region;

        public AirportCode(string code, string name, string city, string region)
        {
            Code = code;
            Name = name;
            City = city;
            Region = region;
        }
    }
}
