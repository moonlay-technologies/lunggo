using System.Collections.Generic;

namespace Lunggo.Flight.Dictionary
{
    public static class FlightCode
    {
        public static readonly Dictionary<int, Code> Airline = PopulateAirline();
        public static readonly Dictionary<int, Code> Airport = PopulateAirport();

        private static Dictionary<int, Code> PopulateAirport()
        {
            return new Dictionary<int, Code>
            {
                {1, new Code ("BTH", "Bandar Udara Internasional Hang Nadim, Batam")},
                {2, new Code ("BTJ", "Bandar Udara Internasional Sultan Iskandar Muda , Banda Aceh")},
                {3, new Code ("KNO", "Bandar Udara Internasional Kuala Namu, Deli Serdang")},
                {4, new Code ("SIX", "Bandar Udara Dr. Ferdinand Lumban Tobing, Sibolga")},
                {5, new Code ("SGT", "Bandar Udara Internasional Silangit, Siborong-borong")},
                {6, new Code ("LSW", "Bandar Udara Malikus Saleh, Lhokseumawe")},
                {7, new Code ("RGT", "Bandar Udara Japura, Rengat")},
                {8, new Code ("MEQ", "Bandar Udara Cut Nyak Dhien, Nagan Raya")},
                {9, new Code ("PDG", "Bandar Udara Internasional Minangkabau, Kota Padang")},
                {10, new Code ("PKU", "Bandar Udara Internasional Sultan Syarif Kasim II, Pekanbaru")},
                {11, new Code ("PLM", "Bandar Udara Internasional Sultan Mahmud Badaruddin II, Palembang")},
                {12, new Code ("TNJ", "Bandar Udara Internasional Raja Haji Fisabilillah, Tanjungpinang")},
                {13, new Code ("BDO", "Bandar Udara Internasional Husein Sastranegara, Bandung")},
                {14, new Code ("CGK", "Bandar Udara Internasional Soekarno-Hatta, Tangerang")},
                {15, new Code ("JOG", "Bandar Udara Internasional Adi Sucipto, Yogyakarta")},
                {16, new Code ("SOC", "Bandar Udara Internasional Adisumarmo, Solo")},
                {17, new Code ("SRG", "Bandar Udara Internasional Achmad Yani, Semarang")},
                {18, new Code ("SUB", "Bandar Udara Internasional Juanda, Surabaya")},
                {19, new Code ("MSI", "Bandar Udara Internasional Valia Rahma, Masalembo")},
                {20, new Code ("JBB", "Bandar Udara Notohadinegoro, Jember")},
                {21, new Code ("BWX", "Bandar Udara Blimbingsari, Banyuwangi")},
                {22, new Code ("DPS", "Bandar Udara Internasional Ngurah Rai, Denpasar")},
                {23, new Code ("LOP", "Bandar Udara Internasional Lombok, Lombok Tengah")},
                {24, new Code ("MOF", "Bandar Udara Wai Oti, Maumere")},
                {25, new Code ("TMC", "Bandar Udara Tambolaka, Waikabubak")},
                {26, new Code ("LKA", "Bandar Udara Gewayantana, Larantuka (mulai 4 Oktober 2014)")},
                {27, new Code ("SWQ", "Bandar Udara Sultan Muhammad Kaharuddin III, Sumbawa Besar")},
                {28, new Code ("MLK", "Bandar Udara Melalan, Sendawar")},
                {29, new Code ("PKY", "Bandar Udara Tjilik Riwut, Palangka Raya")},
                {30, new Code ("TRK", "Bandar Udara Internasional Juwata, Tarakan")},
                {31, new Code ("SRI", "Bandar Udara Temindung, Samarinda")},
                {32, new Code ("BEJ", "Bandar Udara Internasional Kalimarau, Berau")},
                {33, new Code ("BPN", "Bandar Udara Sultan Aji Muhammad Sulaiman, Balikpapan")},
                {34, new Code ("NNX", "Bandar Udara Warukin, Tabalong")},
                {35, new Code ("BDJ", "Bandar Udara Internasional Syamsuddin Noor, Banjarmasin")},
                {36, new Code ("MTW", "Bandar Udara Beringin, Muara Teweh")},
                {37, new Code ("MDC", "Bandar Udara Internasional Sam Ratulangi, Manado")},
                {38, new Code ("UPG", "Bandar Udara Internasional Sultan Hasanuddin, Makassar")},
                {39, new Code ("KDI", "Bandar Udara Internasional Haluoleo, Kendari")},
                {40, new Code ("LUW", "Bandar Udara Syukuran Aminuddin Amir, Luwuk")},
                {41, new Code ("GTO", "Bandar Udara Jalaluddin, Gorontalo")},
                {42, new Code ("WKB", "Bandar Udara Matahora, Wangi-wangi")},
                {43, new Code ("TMI", "Bandar Udara Maranggo, Pulau Tomia")},
                {44, new Code ("NBX", "Bandar Udara Internasional Yos Sudarso, Nabire (mulai 4 Oktober 2014)")},
                {45, new Code ("BIK", "Bandar Udara Frans Kaisiepo, Biak (mulai 4 Oktober 2014)")},
                {46, new Code ("ORG", "Bandara Internasional Iskak, Oksibil (mulai 4 Oktober 2014)")},
                {47, new Code ("TMH", "Bandar Udara Tanah Merah, Tanah Merah (mulai 4 Oktober 2014)")},
            };
        }

        private static Dictionary<int, Code> PopulateAirline()
        {
            return new Dictionary<int, Code>
            {
                {1, new Code ("QZ", " AirAsia")},
                {2, new Code ("JT", " Lion Air")},
                {3, new Code ("SJ", " Sriwijaya Air")},
                {4, new Code ("QG", " Citylink")},
                {5, new Code ("MZ", " Merpati Nusantara")},
                {6, new Code ("MV", " Aviastar")},
                {7, new Code ("ID", " Batik Air")},
                {8, new Code ("TN", " Trigana Air")},
                {9, new Code ("KD", " KalStar Aviation")},
                {10, new Code ("FS", " Airfast Indonesia")},
                {11, new Code ("IW", " Wings Air")},
                {12, new Code ("XN", " Express Air")},
                {13, new Code ("SY", " Sky Aviation")},
                {14, new Code ("SI", " Susi Air")},
                {15, new Code ("GA", " Garuda Indonesia")},
                {16, new Code ("CA", " Air China")},
                {17, new Code ("AK", " AirAsia")},
                {18, new Code ("NH", " All Nippon Airways")},
                {19, new Code ("CX", " Cathay Pacific")},
                {20, new Code ("5J", " Cebu Pacific")},
                {21, new Code ("CI", " China Airlines")},
                {22, new Code ("CZ", " China Southern")},
                {23, new Code ("EK", " Emirates")},
                {24, new Code ("EY", " Etihad Airways")},
                {25, new Code ("BR", " EVA Air")},
                {26, new Code ("JL", " Japan Airlines")},
                {27, new Code ("JQ", " Jetstar Airways")},
                {28, new Code ("KL", " KLM")},
                {29, new Code ("KE", " Korean Air")},
                {30, new Code ("KU", " Kuwait Airways")},
                {31, new Code ("MH", " Malaysia Airlines")},
                {32, new Code ("MJ", " Mihin Lanka")},
                {33, new Code ("PR", " Philipphine Airlines")},
                {34, new Code ("QF", " Qantas")},
                {35, new Code ("QR", " Qatar Airways")},
                {36, new Code ("BI", " Royal Brunei Airlines")},
                {37, new Code ("SV", " Saudi Arabian Airlines")},
                {38, new Code ("3U", " Sichuan Airlines")},
                {39, new Code ("SQ", " Singapore Airlines")},
                {40, new Code ("TQ", " Thai Airways")},
                {41, new Code ("TR", " Tiger Airways")},
                {42, new Code ("TK", " Turkish Airlines")},
                {43, new Code ("VF", " Valuair")},
                {44, new Code ("VN", " Vietnam Airlines")},
                {45, new Code ("IY", " Yemenia")},
            };
        }
    }

    public class Code
    {
        public string Abbr;
        public string Full;

        public Code(string abbr, string full)
        {
            Abbr = abbr;
            Full = full;
        }
    }
}
