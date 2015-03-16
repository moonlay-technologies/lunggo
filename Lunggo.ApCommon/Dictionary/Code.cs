using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages;

namespace Lunggo.ApCommon.Dictionary
{
    public static class Code
    {
        public static readonly Dictionary<int, Airline> Airline = PopulateAirline();
        public static readonly Dictionary<int, Airport> Airport = PopulateAirport();
        public static readonly Dictionary<int, Hotel> Hotel = PopulateHotel();

        private static Dictionary<int, Hotel> PopulateHotel()
        {
            var i = 0;
            var result = new Dictionary<int, Hotel>();
            using (var file = new StreamReader(@"C:\Users\Admin\Documents\Visual Studio 2013\Projects\lunggo\Lunggo.ApCommon\Dictionary\HotelList.csv"))
            {
                var line = file.ReadLine();
                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    var splittedLine = line.Split('|');
                    result.Add(i, new Hotel
                    {
                        LocationId = long.Parse(splittedLine[0]),
                        CountryCode = splittedLine[1],
                        CountryName = splittedLine[2],
                        RegionName = splittedLine[3],
                        LocationName = splittedLine[4],
                        State = splittedLine[5],
                        Priority = splittedLine[6].IsEmpty() ? (int?) null : int.Parse(splittedLine[6]),
                        Latitude = splittedLine[7].IsEmpty() ? (decimal?) null : decimal.Parse(splittedLine[7]),
                        Longitude = splittedLine[8].IsEmpty() ? (decimal?) null : decimal.Parse(splittedLine[8]),
                        IsRegion = splittedLine[9].IsEmpty() ? (bool?) null : bool.Parse(splittedLine[9]),
                        IsAirport = splittedLine[10].IsEmpty() ? (bool?) null : bool.Parse(splittedLine[10]),
                        IsActive = splittedLine[11].IsEmpty() ? (bool?) null : bool.Parse(splittedLine[11])
                    });
                    i++;
                }
            }
            return result;
        }

        private static Dictionary<int, Airport> PopulateAirport()
        {
            return new Dictionary<int, Airport>
            {
                {1, new Airport { Code = "BTH", Name = "Hang Nadim Int'l", City = "Batam", Region = "Sumatera"}},
                {2, new Airport { Code = "BTJ", Name = "Sultan Iskandar Muda Int'l", City = "Banda Aceh", Region = "Sumatera"}},
                {3, new Airport { Code = "KNO", Name = "Kuala Namu Int'l", City = "Deli Serdang", Region = "Sumatera"}},
                {4, new Airport { Code = "SIX", Name = "Dr. Ferdinand Lumban Tobing", City = "Sibolga", Region = "Sumatera"}},
                {5, new Airport { Code = "SGT", Name = "Silangit Int'l", City = "Siborong-borong", Region = "Sumatera"}},
                {6, new Airport { Code = "LSW", Name = "Malikus Saleh", City = "Lhokseumawe", Region = "Sumatera"}},
                {7, new Airport { Code = "RGT", Name = "Japura", City = "Rengat", Region = "Sumatera"}},
                {8, new Airport { Code = "MEQ", Name = "Cut Nyak Dhien", City = "Nagan Raya", Region = "Sumatera"}},
                {9, new Airport { Code = "PDG", Name = "Minangkabau Int'l", City = "Kota Padang", Region = "Sumatera"}},
                {10, new Airport { Code = "PKU", Name = "Sultan Syarif Kasim II Int'l", City = "Pekanbaru", Region = "Sumatera"}},
                {11, new Airport { Code = "PLM", Name = "Sultan Mahmud Badaruddin II Int'l", City = "Palembang", Region = "Sumatera"}},
                {12, new Airport { Code = "TNJ", Name = "Raja Haji Fisabilillah Int'l", City = "Tanjungpinang", Region = "Sumatera"}},
                {13, new Airport { Code = "BDO", Name = "Husein Sastranegara Int'l", City = "Bandung", Region = "Jawa"}},
                {14, new Airport { Code = "CGK", Name = "Soekarno-Hatta Int'l", City = "Jakarta", Region = "Jawa"}},
                {15, new Airport { Code = "JOG", Name = "Adi Sucipto Int'l", City = "Yogyakarta", Region = "Jawa"}},
                {16, new Airport { Code = "SOC", Name = "Adisumarmo Int'l", City = "Solo", Region = "Jawa"}},
                {17, new Airport { Code = "SRG", Name = "Achmad Yani Int'l", City = "Semarang", Region = "Jawa"}},
                {18, new Airport { Code = "SUB", Name = "Juanda Int'l", City = "Surabaya", Region = "Jawa"}},
                {19, new Airport { Code = "MSI", Name = "Valia Rahma Int'l", City = "Masalembo", Region = "Jawa"}},
                {20, new Airport { Code = "JBB", Name = "Notohadinegoro", City = "Jember", Region = "Jawa"}},
                {21, new Airport { Code = "BWX", Name = "Blimbingsari", City = "Banyuwangi", Region = "Jawa"}},
                {22, new Airport { Code = "DPS", Name = "Ngurah Rai Int'l", City = "Denpasar", Region = "Bali"}},
                {23, new Airport { Code = "LOP", Name = "Lombok Int'l", City = "Lombok Tengah", Region = "Nusa Tenggara"}},
                {24, new Airport { Code = "MOF", Name = "Wai Oti", City = "Maumere", Region = "Nusa Tenggara"}},
                {25, new Airport { Code = "TMC", Name = "Tambolaka", City = "Waikabubak", Region = "Nusa Tenggara"}},
                {26, new Airport { Code = "LKA", Name = "Gewayantana", City = "Larantuka", Region = "Nusa Tenggara"}},
                {27, new Airport { Code = "SWQ", Name = "Sultan Muhammad Kaharuddin III", City = "Sumbawa Besar", Region = "Nusa Tenggara"}},
                {28, new Airport { Code = "MLK", Name = "Melalan", City = "Sendawar", Region = "Kalimantan"}},
                {29, new Airport { Code = "PKY", Name = "Tjilik Riwut", City = "Palangka Raya", Region = "Kalimantan"}},
                {30, new Airport { Code = "TRK", Name = "Juwata Int'l", City = "Tarakan", Region = "Kalimantan"}},
                {31, new Airport { Code = "SRI", Name = "Temindung", City = "Samarinda", Region = "Kalimantan"}},
                {32, new Airport { Code = "BEJ", Name = "Kalimarau Int'l", City = "Berau", Region = "Kalimantan"}},
                {33, new Airport { Code = "BPN", Name = "Sultan Aji Muhammad Sulaiman", City = "Balikpapan", Region = "Kalimantan"}},
                {34, new Airport { Code = "NNX", Name = "Warukin", City = "Tabalong", Region = "Kalimantan"}},
                {35, new Airport { Code = "BDJ", Name = "Syamsuddin Noor Int'l", City = "Banjarmasin", Region = "Kalimantan"}},
                {36, new Airport { Code = "MTW", Name = "Beringin", City = "Muara Teweh", Region = "Kalimantan"}},
                {37, new Airport { Code = "MDC", Name = "Sam Ratulangi Int'l", City = "Manado", Region = "Sulawesi"}},
                {38, new Airport { Code = "UPG", Name = "Sultan Hasanuddin Int'l", City = "Makassar", Region = "Sulawesi"}},
                {39, new Airport { Code = "KDI", Name = "Haluoleo Int'l", City = "Kendari", Region = "Sulawesi"}},
                {40, new Airport { Code = "LUW", Name = "Syukuran Aminuddin Amir", City = "Luwuk", Region = "Sulawesi"}},
                {41, new Airport { Code = "GTO", Name = "Jalaluddin", City = "Gorontalo", Region = "Sulawesi"}},
                {42, new Airport { Code = "WKB", Name = "Matahora", City = "Wangi-wangi", Region = "Sulawesi"}},
                {43, new Airport { Code = "TMI", Name = "Maranggo", City = "Pulau Tomia", Region = "Sulawesi"}},
                {44, new Airport { Code = "NBX", Name = "Yos Sudarso Int'l", City = "Nabire", Region = "Papua"}},
                {45, new Airport { Code = "BIK", Name = "Frans Kaisiepo", City = "Biak", Region = "Papua"}},
                {46, new Airport { Code = "ORG", Name = "Iskak Int'l", City = "Oksibil", Region = "Papua"}},
                {47, new Airport { Code = "TMH", Name = "Tanah Merah", City = "Tanah Merah", Region = "Papua"}},

                {48, new Airport { Code = "ICN", Name = "Incheon Int'l", City = "Seoul", Region = "Korea Selatan"}},
                {49, new Airport { Code = "SIN", Name = "Changi Int'l", City = "Singapura", Region = "Singapura"}},
                {50, new Airport { Code = "KTM", Name = "Tribhuvan Int'l", City = "Kathmandu", Region = "Nepal"}},
                {51, new Airport { Code = "KUL", Name = "KLIA", City = "Kuala Lumpur", Region = "Malaysia"}}
            };
        }

        private static Dictionary<int, Airline> PopulateAirline()
        {
            return new Dictionary<int, Airline>
            {
                {1, new Airline { Code = "QZ", Name = "AirAsia"}},
                {2, new Airline { Code = "JT", Name = "Lion Air"}},
                {3, new Airline { Code = "SJ", Name = "Sriwijaya Air"}},
                {4, new Airline { Code = "QG", Name = "Citylink"}},
                {5, new Airline { Code = "MZ", Name = "Merpati Nusantara"}},
                {6, new Airline { Code = "MV", Name = "Aviastar"}},
                {7, new Airline { Code = "ID", Name = "Batik Air"}},
                {8, new Airline { Code = "TN", Name = "Trigana Air"}},
                {9, new Airline { Code = "KD", Name = "KalStar Aviation"}},
                {10, new Airline { Code = "FS", Name = "Airfast Indonesia"}},
                {11, new Airline { Code = "IW", Name = "Wings Air"}},
                {12, new Airline { Code = "XN", Name = "Express Air"}},
                {13, new Airline { Code = "SY", Name = "Sky Aviation"}},
                {14, new Airline { Code = "SI", Name = "Susi Air"}},
                {15, new Airline { Code = "GA", Name = "Garuda Indonesia"}},
                {16, new Airline { Code = "CA", Name = "Air China"}},
                {17, new Airline { Code = "AK", Name = "AirAsia"}},
                {18, new Airline { Code = "NH", Name = "All Nippon Airways"}},
                {19, new Airline { Code = "CX", Name = "Cathay Pacific"}},
                {20, new Airline { Code = "5J", Name = "Cebu Pacific"}},
                {21, new Airline { Code = "CI", Name = "China Airlines"}},
                {22, new Airline { Code = "CZ", Name = "China Southern"}},
                {23, new Airline { Code = "EK", Name = "Emirates"}},
                {24, new Airline { Code = "EY", Name = "Etihad Airways"}},
                {25, new Airline { Code = "BR", Name = "EVA Air"}},
                {26, new Airline { Code = "JL", Name = "Japan Airlines"}},
                {27, new Airline { Code = "JQ", Name = "Jetstar Airways"}},
                {28, new Airline { Code = "KL", Name = "KLM"}},
                {29, new Airline { Code = "KE", Name = "Korean Air"}},
                {30, new Airline { Code = "KU", Name = "Kuwait Airways"}},
                {31, new Airline { Code = "MH", Name = "Malaysia Airlines"}},
                {32, new Airline { Code = "MJ", Name = "Mihin Lanka"}},
                {33, new Airline { Code = "PR", Name = "Philipphine Airlines"}},
                {34, new Airline { Code = "QF", Name = "Qantas"}},
                {35, new Airline { Code = "QR", Name = "Qatar Airways"}},
                {36, new Airline { Code = "BI", Name = "Royal Brunei Airlines"}},
                {37, new Airline { Code = "SV", Name = "Saudi Arabian Airlines"}},
                {38, new Airline { Code = "3U", Name = "Sichuan Airlines"}},
                {39, new Airline { Code = "SQ", Name = "Singapore Airlines"}},
                {40, new Airline { Code = "TQ", Name = "Thai Airways"}},
                {41, new Airline { Code = "TR", Name = "Tiger Airways"}},
                {42, new Airline { Code = "TK", Name = "Turkish Airlines"}},
                {43, new Airline { Code = "VF", Name = "Valuair"}},
                {44, new Airline { Code = "VN", Name = "Vietnam Airlines"}},
                {45, new Airline { Code = "IY", Name = "Yemenia"}},
            };
        }
    }

    public class Airline
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class Airport
    {
        public string Code { get; set; }

        public string Name { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
    }
    public class Hotel
    {
        public long LocationId { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string RegionName { get; set; }
        public string LocationName { get; set; }
        public string State { get; set; }
        public int? Priority { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public bool? IsRegion { get; set; }
        public bool? IsAirport { get; set; }
        public bool? IsActive { get; set; }
    }
}
