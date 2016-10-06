using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.WebPages;
using Lunggo.ApCommon.Flight.Constant;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {
        public static Dictionary<string, string> HotelSegmentDictId;
        public static Dictionary<string, string> HotelSegmentDictEng;
        public static Dictionary<int, string> HotelFacilityDictId;
        public static Dictionary<int, string> HotelFacilityDictEng;
        public static Dictionary<int, string> HotelFacilityGroupDictId;
        public static Dictionary<int, string> HotelFacilityGroupDictEng;
        public static Dictionary<string, string> HotelRoomDictId;
        public static Dictionary<string, string> HotelRoomDictEng;
        public static Dictionary<string, string> HotelRoomTypeDictId;
        public static Dictionary<string, string> HotelRoomTypeDictEng;
        public static Dictionary<string, string> HotelRoomCharacteristicDictId;
        public static Dictionary<string, string> HotelRoomCharacteristicDictEng;

        private const string HotelSegmentFileName = @"HotelSegment.csv";
        private const string HotelFacilityFileName = @"HotelFacilities.csv";
        private const string HotelFacilityGroupFileName = @"HotelFacilityGroup.csv";
        private const string HotelRoomFileName = @"HotelRoom.csv";

        private static string _hotelSegmentFilePath;
        private static string _hotelFacilitiesFilePath;
        private static string _hotelFacilityGroupFilePath;
        private static string _hotelRoomFilePath;
        private static string _configPath;

        public void InitDictionary(string folderName)
        {
            _configPath = HttpContext.Current != null
                ? HttpContext.Current.Server.MapPath(@"~/" + folderName + @"/")
                : string.IsNullOrEmpty(folderName)
                    ? ""
                    : folderName + @"\";

            _hotelSegmentFilePath = Path.Combine(_configPath, HotelSegmentFileName);
            _hotelFacilitiesFilePath = Path.Combine(_configPath, HotelFacilityFileName);
            _hotelFacilityGroupFilePath = Path.Combine(_configPath, HotelFacilityGroupFileName);
            _hotelRoomFilePath = Path.Combine(_configPath, HotelRoomFileName);

            PopulateHotelSegmentDict(_hotelSegmentFilePath);
            PopulateHotelFacilitiesDict(_hotelFacilitiesFilePath);
            PopulateHotelFacilityGroupDict(_hotelFacilityGroupFilePath);
            PopulateHotelRoomDict(_hotelRoomFilePath);
            PopulateHotelRoomTypeDict(_hotelRoomFilePath);
            PopulateHotelRoomCharactristicDict(_hotelRoomFilePath);
        }

        private static void PopulateHotelSegmentDict(String hotelSegmentFilePath)
        {
            using (var file = new StreamReader(hotelSegmentFilePath))
            {
                var line = file.ReadLine();
                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    var splittedLine = line.Split('|');
                    HotelSegmentDictId.Add(splittedLine[0],splittedLine[1]);
                    HotelSegmentDictEng.Add(splittedLine[0], splittedLine[2]);
                }
            }
            
        }

        private static void PopulateHotelFacilitiesDict(String hotelFacilitiesFilePath)
        {
            using (var file = new StreamReader(hotelFacilitiesFilePath))
            {
                var line = file.ReadLine();
                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    var splittedLine = line.Split('|');
                    HotelFacilityDictId.Add(Convert.ToInt16(splittedLine[0]), splittedLine[1]);
                    HotelFacilityDictId.Add(Convert.ToInt16(splittedLine[0]), splittedLine[2]);
                }
            }
        }

        private static void PopulateHotelFacilityGroupDict(String hotelFacilityGroupFilePath)
        {
            using (var file = new StreamReader(hotelFacilityGroupFilePath))
            {
                var line = file.ReadLine();
                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    var splittedLine = line.Split('|');
                    HotelFacilityGroupDictEng.Add(Convert.ToInt16(splittedLine[0]), splittedLine[1]);
                    HotelFacilityGroupDictId.Add(Convert.ToInt16(splittedLine[0]), splittedLine[2]);
                }
            }
        }

        private static void PopulateHotelRoomDict(String hotelRoomFilePath)
        {
            using (var file = new StreamReader(hotelRoomFilePath))
            {
                var line = file.ReadLine();
                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    var splittedLine = line.Split('|');
                    HotelRoomDictEng.Add(splittedLine[0], splittedLine[8]);
                    HotelRoomDictId.Add(splittedLine[0], splittedLine[11]);
                }
            }
        }

        private static void PopulateHotelRoomTypeDict(String hotelRoomTypeFilePath)
        {
            using (var file = new StreamReader(hotelRoomTypeFilePath))
            {
                var line = file.ReadLine();
                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    var splittedLine = line.Split('|');
                    string x;
                    if (!HotelRoomTypeDictEng.TryGetValue(splittedLine[1], out x))
                    {
                        HotelRoomTypeDictEng.Add(splittedLine[1], splittedLine[9]);
                        HotelRoomTypeDictId.Add(splittedLine[1], splittedLine[12]);
                    }
                    
                }
            }
        }

        private static void PopulateHotelRoomCharactristicDict(String hotelRoomCharactristicFilePath)
        {
            using (var file = new StreamReader(hotelRoomCharactristicFilePath))
            {
                var line = file.ReadLine();
                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    var splittedLine = line.Split('|');
                    string x;
                    if (!HotelRoomTypeDictEng.TryGetValue(splittedLine[2], out x))
                    {
                        HotelRoomCharacteristicDictEng.Add(splittedLine[2], splittedLine[10]);
                        HotelRoomCharacteristicDictId.Add(splittedLine[2], splittedLine[13]);
                    }

                }
            }
        }

    }

    

    
}
