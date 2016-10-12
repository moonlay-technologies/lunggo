using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

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
        public static Dictionary<int, string> HotelRoomFacilityDictId;
        public static Dictionary<int, string> HotelRoomFacilityDictEng;
        public static Dictionary<string, string> HotelRoomRateClassDictId;
        public static Dictionary<string, string> HotelRoomRateClassDictEng;
        public static Dictionary<string, string> HotelRoomRateTypeDictId;
        public static Dictionary<string, string> HotelRoomRateTypeDictEng;
        public static Dictionary<string, string> HotelRoomPaymentTypeDictId;
        public static Dictionary<string, string> HotelRoomPaymentTypeDictEng;
        public static Dictionary<string, string> HotelCountry;
        public static Dictionary<string, string> HotelCountryIso;
        public static Dictionary<string, string> HotelCountryIsoName;
        public static Dictionary<string, string> HotelBoardDictEng;
        public static Dictionary<string, string> HotelBoardDictId;
        public static Dictionary<string, string> HotelChain;
        public static Dictionary<string, string> HotelAccomodationDictEng;
        public static Dictionary<string, string> HotelAccomodationDictid;
        public static Dictionary<string, string> HotelAccomodationMultiDict;
        public static Dictionary<string, string> HotelCategoryDictEng;
        public static Dictionary<string, string> HotelCategoryDictInd;

        private const string HotelSegmentFileName = @"HotelSegment.csv";
        private const string HotelFacilityFileName = @"HotelFacilities.csv";
        private const string HotelFacilityGroupFileName = @"HotelFacilityGroup.csv";
        private const string HotelRoomFileName = @"HotelRoom.csv";
        private const string HotelRoomRateClassFileName = @"HotelRoomRateClass.csv";
        private const string HotelRoomRateTypeFileName = @"HotelRoomRateType.csv";
        private const string HotelRoomPaymentTypeFileName = @"HotelRoomPaymentType.csv";
        private const string HotelCountryFileName = @"HotelCountries.csv";
        private const string HotelBoardFileName = @"HotelBoard.csv";
        private const string HotelAccomodationFileName = @"HotelAccomodation.csv";
        private const string HotelChainFileName = @"HotelChain.csv";
        private const string HotelCategoryFileName = @"HotelCategory.csv";

        private static string _hotelSegmentFilePath;
        private static string _hotelFacilitiesFilePath;
        private static string _hotelFacilityGroupFilePath;
        private static string _hotelRoomFilePath;
        private static string _hotelRoomRateClassFilePath;
        private static string _hotelRoomRateTypeFilePath;
        private static string _hotelRoomPaymentTypeFilePath;
        private static string _hotelCountriesFilePath;
        private static string _hotelBoardFilePath;
        private static string _hotelChainFilePath;
        private static string _hotelAccomodationFilePath;
        private static string _hotelCategoryFilePath;
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
            _hotelRoomRateClassFilePath = Path.Combine(_configPath, HotelRoomRateClassFileName);
            _hotelRoomRateTypeFilePath = Path.Combine(_configPath, HotelRoomRateTypeFileName);
            _hotelRoomPaymentTypeFilePath = Path.Combine(_configPath, HotelRoomPaymentTypeFileName);
            _hotelCountriesFilePath = Path.Combine(_configPath, HotelCountryFileName);
            _hotelBoardFilePath = Path.Combine(_configPath, HotelBoardFileName);
            _hotelChainFilePath = Path.Combine(_configPath, HotelChainFileName);
            _hotelAccomodationFilePath = Path.Combine(_configPath, HotelAccomodationFileName);
            _hotelCategoryFilePath = Path.Combine(_configPath, HotelCategoryFileName);

            PopulateHotelSegmentDict(_hotelSegmentFilePath);
            PopulateHotelFacilitiesDict(_hotelFacilitiesFilePath);
            PopulateHotelFacilityGroupDict(_hotelFacilityGroupFilePath);
            PopulateHotelRoomDict(_hotelRoomFilePath);
            PopulateHotelRoomTypeDict(_hotelRoomFilePath);
            PopulateHotelRoomCharacteristicDict(_hotelRoomFilePath);
            PopulateHotelRoomFacilityDict(_hotelFacilitiesFilePath);
            PopulateHotelRoomRateClassDict(_hotelRoomRateClassFilePath);
            PopulateHotelRoomRateTypeDict(_hotelRoomRateTypeFilePath);
            PopulateHotelRoomPaymentTypeDict(_hotelRoomPaymentTypeFilePath);
            PopulateHotelCountriesDict(_hotelCountriesFilePath);
            PopulateHotelBoardDict(_hotelBoardFilePath);
            PopulateHotelChainDict(_hotelChainFilePath);
            PopulateHotelCategoryDict(_hotelCategoryFilePath);
            PopulateHotelAccomodationDict(_hotelAccomodationFilePath);
        }

        private static void PopulateHotelSegmentDict(String hotelSegmentFilePath)
        {
            HotelSegmentDictEng = new Dictionary<string, string>();
            HotelSegmentDictId = new Dictionary<string, string>();

            using (var file = new StreamReader(hotelSegmentFilePath))
            {
                var line = file.ReadLine();
                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    var splittedLine = line.Split('|');
                    HotelSegmentDictEng.Add(splittedLine[0],splittedLine[1]);
                    HotelSegmentDictId.Add(splittedLine[0], splittedLine[2]);
                }
            }
            
        }

        private static void PopulateHotelFacilitiesDict(String hotelFacilitiesFilePath)
        {
            HotelFacilityDictEng = new Dictionary<int, string>();
            HotelFacilityDictId = new Dictionary<int, string>();

            using (var file = new StreamReader(hotelFacilitiesFilePath))
            {
                var line = file.ReadLine();
                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    var splittedLine = line.Split('|');
                    HotelFacilityDictEng.Add(Convert.ToInt32(splittedLine[0]), splittedLine[1]);
                    HotelFacilityDictId.Add(Convert.ToInt32(splittedLine[0]), splittedLine[2]);
                }
            }
        }

        private static void PopulateHotelFacilityGroupDict(String hotelFacilityGroupFilePath)
        {
            HotelFacilityGroupDictEng = new Dictionary<int, string>();
            HotelFacilityGroupDictId = new Dictionary<int, string>();

            using (var file = new StreamReader(hotelFacilityGroupFilePath))
            {
                var line = file.ReadLine();
                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    var splittedLine = line.Split('|');
                    HotelFacilityGroupDictEng.Add(Convert.ToInt32(splittedLine[0]), splittedLine[1]);
                    HotelFacilityGroupDictId.Add(Convert.ToInt32(splittedLine[0]), splittedLine[2]);
                }
            }
        }

        private static void PopulateHotelRoomDict(String hotelRoomFilePath)
        {
            HotelRoomDictEng = new Dictionary<string, string>();
            HotelRoomDictId = new Dictionary<string, string>();

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
            HotelRoomTypeDictEng = new Dictionary<string, string>();
            HotelRoomTypeDictId = new Dictionary<string, string>();

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

        private static void PopulateHotelRoomCharacteristicDict(String hotelRoomCharactristicFilePath)
        {
            HotelRoomCharacteristicDictEng = new Dictionary<string, string>();
            HotelRoomCharacteristicDictId = new Dictionary<string, string>();

            using (var file = new StreamReader(hotelRoomCharactristicFilePath))
            {
                var line = file.ReadLine();
                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    var splittedLine = line.Split('|');
                    string x;
                    if (!HotelRoomCharacteristicDictEng.TryGetValue(splittedLine[2], out x))
                    {
                        HotelRoomCharacteristicDictEng.Add(splittedLine[2], splittedLine[10]);
                        HotelRoomCharacteristicDictId.Add(splittedLine[2], splittedLine[13]);
                    }

                }
            }
        }

        private static void PopulateHotelRoomFacilityDict(String hotelFacilitiesFilePath)
        {
            HotelRoomFacilityDictEng = new Dictionary<int, string>();
            HotelRoomFacilityDictId = new Dictionary<int, string>();

            using (var file = new StreamReader(hotelFacilitiesFilePath))
            {
                var line = file.ReadLine();
                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    var splittedLine = line.Split('|');
                    if (Convert.ToInt32(splittedLine[0])/1000 == 60)
                    {
                        HotelRoomFacilityDictEng.Add(Convert.ToInt32(splittedLine[0]), splittedLine[1]);
                        HotelRoomFacilityDictId.Add(Convert.ToInt32(splittedLine[0]), splittedLine[2]);
                    }
                }
            }
        }

        private static void PopulateHotelRoomRateClassDict(String hotelRoomRateClassFilePath)
        {
            HotelRoomRateClassDictEng = new Dictionary<string, string>();
            HotelRoomRateClassDictId = new Dictionary<string, string>();

            using (var file = new StreamReader(hotelRoomRateClassFilePath))
            {
                var line = file.ReadLine();
                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    var splittedLine = line.Split('|');
                    
                    HotelRoomRateClassDictEng.Add(splittedLine[0], splittedLine[1]);
                    HotelRoomRateClassDictId.Add(splittedLine[0], splittedLine[2]);
                }
            }
        }

        private static void PopulateHotelRoomRateTypeDict(String hotelRoomRateTypeFilePath)
        {
            HotelRoomRateTypeDictEng = new Dictionary<string, string>();
            HotelRoomRateTypeDictId = new Dictionary<string, string>();

            using (var file = new StreamReader(hotelRoomRateTypeFilePath))
            {
                var line = file.ReadLine();
                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    var splittedLine = line.Split('|');

                    HotelRoomRateTypeDictEng.Add(splittedLine[0], splittedLine[1]);
                    HotelRoomRateTypeDictId.Add(splittedLine[0], splittedLine[2]);
                }
            }
        }

        private static void PopulateHotelRoomPaymentTypeDict(String hotelRoomPaymentTypeFilePath)
        {
            HotelRoomPaymentTypeDictEng = new Dictionary<string, string>();
            HotelRoomPaymentTypeDictId = new Dictionary<string, string>();

            using (var file = new StreamReader(hotelRoomPaymentTypeFilePath))
            {
                var line = file.ReadLine();
                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    var splittedLine = line.Split('|');

                    HotelRoomPaymentTypeDictEng.Add(splittedLine[0], splittedLine[1]);
                    HotelRoomPaymentTypeDictId.Add(splittedLine[0], splittedLine[2]);
                }
            }
        }

        private static void PopulateHotelCountriesDict(String hotelCountriesFilePath)
        {
            HotelCountry = new Dictionary<string, string>();
            HotelCountryIso = new Dictionary<string, string>();
            HotelCountryIsoName = new Dictionary<string, string>();

            using (var file = new StreamReader(hotelCountriesFilePath))
            {
                var line = file.ReadLine();
                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    var splittedLine = line.Split('|');
                    string x;
                    HotelCountryIso.Add(splittedLine[0], splittedLine[1]);
                    HotelCountry.Add(splittedLine[0], splittedLine[2]);
                    if (!HotelCountryIsoName.TryGetValue(splittedLine[1], out x))
                    {
                        HotelCountryIsoName.Add(splittedLine[1], splittedLine[2]);
                    }
                }
            }
        }

        private static void PopulateHotelBoardDict(string hotelBoardFilePath)
        {
            HotelBoardDictEng = new Dictionary<string, string>();
            HotelBoardDictId = new Dictionary<string, string>();

            using (var file = new StreamReader(hotelBoardFilePath))
            {
                var line = file.ReadLine();
                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    var splittedLine = line.Split('|');
                    HotelBoardDictEng.Add(splittedLine[0], splittedLine[1]);
                    HotelBoardDictId.Add(splittedLine[0], splittedLine[2]);
                }
            }
        }

        private static void PopulateHotelChainDict(string hotelBoardFilePath)
        {
            HotelChain = new Dictionary<string, string>();
          
            using (var file = new StreamReader(hotelBoardFilePath))
            {
                var line = file.ReadLine();
                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    var splittedLine = line.Split('|');
                    HotelChain.Add(splittedLine[0], splittedLine[1]);;
                }
            }
        }

        private static void PopulateHotelAccomodationDict(string hotelBoardFilePath)
        {
            HotelAccomodationDictEng = new Dictionary<string, string>();
            HotelAccomodationDictid = new Dictionary<string, string>();
            HotelAccomodationMultiDict = new Dictionary<string, string>();

            using (var file = new StreamReader(hotelBoardFilePath))
            {
                var line = file.ReadLine();
                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    var splittedLine = line.Split('|');
                    HotelAccomodationMultiDict.Add(splittedLine[0], splittedLine[1]);
                    HotelAccomodationDictEng.Add(splittedLine[0], splittedLine[2]);
                    HotelAccomodationDictid.Add(splittedLine[0], splittedLine[3]);
                }
            }
        }

        private static void PopulateHotelCategoryDict(string hotelBoardFilePath)
        {
            HotelCategoryDictEng= new Dictionary<string, string>();
            HotelCategoryDictInd = new Dictionary<string, string>();

            using (var file = new StreamReader(hotelBoardFilePath))
            {
                var line = file.ReadLine();
                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    var splittedLine = line.Split('|');
                    HotelCategoryDictEng.Add(splittedLine[0], splittedLine[4]);
                    HotelCategoryDictInd.Add(splittedLine[0], splittedLine[5]);
                }
            }
        }

        public string GetHotelChain(string code)
        {
            try
            {
                return HotelChain[code];
            }
            catch
            {
                return "";
            }
        }
        
        public string GetHotelCategoryId(string code)
        {
            try
            {
                return HotelCategoryDictInd[code];
            }
            catch
            {
                return "";
            }
        }

        public string GetHotelCategoryEng(string code)
        {
            try
            {
                return HotelCategoryDictEng[code];
            }
            catch
            {
                return "";
            }
        }


        public string GetHotelAccomodationId(string code)
        {
            try
            {
                return HotelAccomodationDictid[code];
            }
            catch
            {
                return "";
            }
        }

        public string GetHotelAccomodationEng(string code)
        {
            try
            {
                return HotelAccomodationDictEng[code];
            }
            catch
            {
                return "";
            }
        }

        public string GetHotelAccomodationMultiDesc(string code)
        {
            try
            {
                return HotelAccomodationMultiDict[code];
            }
            catch
            {
                return "";
            }
        }
        
        public string GetHotelBoardId(string code)
        {
            try
            {
                return HotelBoardDictId[code];
            }
            catch
            {
                return "";
            }
        }

        public string GetHotelBoardEng(string code)
        {
            try
            {
                return HotelBoardDictEng[code];
            }
            catch
            {
                return "";
            }
        }

        public string GetHotelSegmentId(string code)
        {
            try
            {
                return HotelSegmentDictId[code];
            }
            catch
            {
                return "";
            }
        }

        public string GetHotelSegmentEng(string code)
        {
            try
            {
                return HotelSegmentDictEng[code];
            }
            catch
            {
                return "";
            }
        }

        public string GetHotelFacilityId(int code)
        {
            try
            {
                return HotelFacilityDictId[code];
            }
            catch
            {
                return "";
            }
        }

        public string GetHotelFacilityEng(int code)
        {
            try
            {
                return HotelFacilityDictEng[code];
            }
            catch
            {
                return "";
            }
        }

        public string GetHotelFacilityGroupId(int code)
        {
            try
            {
                return HotelFacilityGroupDictId[code];
            }
            catch
            {
                return "";
            }
        }

        public string GetHotelFacilityGroupEng(int code)
        {
            try
            {
                return HotelFacilityGroupDictEng[code];
            }
            catch
            {
                return "";
            }
        }

        public string GetHotelRoomId(string code)
        {
            try
            {
                return HotelRoomDictId[code];
            }
            catch
            {
                return "";
            }
        }

        public string GetHotelRoomEng(string code)
        {
            try
            {
                return HotelRoomDictEng[code];
            }
            catch
            {
                return "";
            }
        }

        public string GetHotelRoomTypeId(string code)
        {
            try
            {
                return HotelRoomTypeDictId[code];
            }
            catch
            {
                return "";
            }
        }

        public string GetHotelRoomTypeEng(string code)
        {
            try
            {
                return HotelRoomTypeDictEng[code];
            }
            catch
            {
                return "";
            }
        }

        public string GetHotelRoomCharacteristicId(string code)
        {
            try
            {
                return HotelRoomCharacteristicDictId[code];
            }
            catch
            {
                return "";
            }
        }

        public string GetHotelRoomCharacteristicEng(string code)
        {
            try
            {
                return HotelRoomCharacteristicDictEng[code];
            }
            catch
            {
                return "";
            }
        }

        public string GetHotelRoomFacilityId(int code)
        {
            try
            {
                return HotelRoomFacilityDictId[code];
            }
            catch
            {
                return "";
            }
        }

        public string GetHotelRoomFacilityEng(int code)
        {
            try
            {
                return HotelRoomFacilityDictEng[code];
            }
            catch
            {
                return "";
            }
        }

        public string GetHotelRoomRateClassId(string code)
        {
            try
            {
                return HotelRoomRateClassDictId[code];
            }
            catch
            {
                return "";
            }
        }

        public string GetHotelRoomRateClassEng(string code)
        {
            try
            {
                return HotelRoomRateClassDictEng[code];
            }
            catch
            {
                return "";
            }
        }

        public string GetHotelRoomRateTypeId(string code)
        {
            try
            {
                return HotelRoomRateTypeDictId[code];
            }
            catch
            {
                return "";
            }
        }

        public string GetHotelRoomRateTypeEng(string code)
        {
            try
            {
                return HotelRoomRateTypeDictEng[code];
            }
            catch
            {
                return "";
            }
        }

        public string GetHotelRoomPaymentTypeId(string code)
        {
            try
            {
                return HotelRoomPaymentTypeDictId[code];
            }
            catch
            {
                return "";
            }
        }

        public string GetHotelRoomPaymentTypeEng(string code)
        {
            try
            {
                return HotelRoomPaymentTypeDictEng[code];
            }
            catch
            {
                return "";
            }
        }

        public string GetHotelCountryNameByCode(string code)
        {
            try
            {
                return HotelCountry[code];
            }
            catch
            {
                return "";
            }
        }

        public string GetHotelCountryIsoCode(string code)
        {
            try
            {
                return HotelCountryIso[code];
            }
            catch
            {
                return "";
            }
        }

        public string GetHotelCountryNameByIsoCode(string isoCode)
        {
            try
            {
                return HotelCountryIsoName[isoCode];
            }
            catch
            {
                return "";
            }
        }

    }

    

    
}
