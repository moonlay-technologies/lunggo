using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {
        public class Country
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string IsoCode { get; set; }
            public List<Destination> Destinations { get; set; }
        }

        public class Destination
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public List<Zone> Zones { get; set; }
            public string CountryCode { get; set; }
        }

        public class Zone
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public Hotels Hotel { get; set; }
            public string DestinationCode { get; set; }
        }	

        public class Hotels
        {
            public List<int> HotelCodes { get; set; }
            public string ZoneCode { get; set; }
        }
        public class FacilityFilter
        {
            public string Code { get; set; }
            public List<string> FacilityCode { get; set; }
        }

        public class FacilityGroup
        {
            public int Code { get; set; }
            public string NameId { get; set; }
            public string NameEn { get; set; }
            public List<Facility> Facilities { get; set; } 

        }

        public class Facility
        {
            public int Code { get; set; }
            public string NameId { get; set; }
            public string NameEn { get; set; }

        }

        public class Chain
        {
            public string Code { get; set; }
            public string Description { get; set; }
        }

        public class Accommodation
        {
            public string Code { get; set; }
            public string MultiDescription { get; set; }
            public string TypeNameId { get; set; }
            public string TypeNameEn { get; set; }
        }

        public class Board
        {
            public string Code { get; set; }
            public string NameId { get; set; }
            public string NameEn { get; set; }
            public string MultilingualCode { get; set; }
        }

        public class Category {
            public string Code { get; set; }
            public int SimpleCode { get; set; }
            public string AccomodationType { get; set; }
            public string NameEn { get; set; }
            public string NameId { get; set; }
        }

        public class HotelRoomType
        {
            public string Type { get; set; }
            public string DescId { get; set; }
            public string DescEn { get; set; }
        }

        public class Room
        {
            public RoomCharacteristic RoomCharacteristic { get; set; }
            public HotelRoomType RoomType { get; set; }
            public string RoomCd { get; set; }
            public string RoomDescId { get; set; }
            public string RoomDescEn { get; set; }
            public int MinPax { get; set; }
            public int MaxPax { get; set; }
            public int MinAdult { get; set; }
            public int MaxAdult { get; set; }
            public int MinChild { get; set; }
            public int MaxChild { get; set; }

        }

        public class RateClass
        {
            public string Code { get; set; }
            public string DescId { get; set; }
            public string DescEn { get; set; }
        }

        public class RateType
        {
            public string Type { get; set; }
            public string DescId { get; set; }
            public string DescEn { get; set; }
        }

        public class PaymentType
        {
            public string Type { get; set; }
            public string DescId { get; set; }
            public string DescEn { get; set; }
        }

        public class CountryDict
        {
            public string CountryCode;
            public string IsoCode;
            public string Name;
        }
        public class RoomCharacteristic
        {
            public string CharacteristicCd { get; set; }
            public string CharacteristicDescId { get; set; }
            public string CharacteristicDescEn { get; set; }
        }

        public class Autocomplete
        {
            public String Name;
            public AutocompleteType Type;
            public long Id;
            public string Code;
        }

        public enum AutocompleteType
        {
            Destination = 1,
            Zone = 2,
            Area = 3,
            Hotel = 4
        }

        //FOR AUTOCOMPLETE
        public Dictionary<long, Autocomplete> _Autocompletes; 

        public static Dictionary<string, string> HotelSegmentDictId;
        public static Dictionary<string, string> HotelSegmentDictEng;
        
        public static Dictionary<int, string> HotelFacilityGroupDictId;
        public static Dictionary<int, string> HotelFacilityGroupDictEng;
        public static Dictionary<int, Facility> HotelRoomFacility;

        public static Dictionary<string, Chain> HotelChains;
        public static Dictionary<string, Accommodation> HotelAccomodations;
        public static Dictionary<string, Board> HotelBoards;
        public static Dictionary<string, Category> HotelCategories; 

        public static Dictionary<string, Room> HotelRoomDict;
        public static Dictionary<string, HotelRoomType> HotelRoomTypeDict;
        public static Dictionary<string, RoomCharacteristic> HotelRoomCharacteristicDict;
        
        public Dictionary<string, RateClass> HotelRoomRateClassDict;
        public Dictionary<string, RateType> HotelRoomRateTypeDict;
        public Dictionary<string, PaymentType> HotelRoomPaymentTypeDict;
        public Dictionary<string, CountryDict> HotelCountry;
        public Dictionary<string, Destination> HotelDestinationDict;
        public Dictionary<string, Country> HotelDestinationCountryDict;
        public Dictionary<string, Zone> HotelDestinationZoneDict;
        public Dictionary<int, string> HotelCodeAndZoneDict;
        public static Dictionary<string, FacilityFilter> HotelFacilityFilters;

        public static List<Country> Countries;
        public static List<FacilityGroup> FacilityGroups;
        public static List<Room> Rooms;
        public static List<FacilityFilter> FacilityFilters; 

        private const string HotelSegmentFileName = @"HotelSegment.csv";
        private const string HotelFacilityFileName = @"HotelFacilities.csv";
        private const string HotelFacilityGroupFileName = @"HotelFacilityGroup.csv";
        private const string HotelRoomFileName = @"HotelRoom.csv";
        private const string HotelRoomRateClassFileName = @"HotelRoomRateClass.csv";
        private const string HotelRoomRateTypeFileName = @"HotelRoomRateType.csv";
        private const string HotelRoomPaymentTypeFileName = @"HotelRoomPaymentType.csv";
        private const string HotelCountryFileName = @"HotelCountries.csv";
        private const string HotelDestinationFileName = @"HotelDestinations.csv";
        private const string HotelFacilityFilterGroupFileName = @"HotelFacilitiesFilterGroup.csv";

        private const string HotelchainFileName = @"HotelChain.csv";
        private const string HotelBoardFileName = @"HotelBoard.csv";
        private const string HotelAccomodationFileName = @"HotelAccomodation.csv";
        private const string HotelCategoryFileName = @"HotelCategory.csv";


        private static string _hotelSegmentFilePath;
        private static string _hotelFacilitiesFilePath;
        private static string _hotelFacilityGroupFilePath;
        private static string _hotelRoomFilePath;
        private static string _hotelRoomRateClassFilePath;
        private static string _hotelRoomRateTypeFilePath;
        private static string _hotelRoomPaymentTypeFilePath;
        private static string _hotelCountriesFilePath;
        private static string _hotelDestinationsFilePath;
        private static string _hotelAccomodationFilePath;
        private static string _hotelBoardFilePath;
        private static string _hotelChainFilePath;
        private static string _hotelCategoryFilePath;
        private static string _hotelFacilitiesFilter;

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
            _hotelDestinationsFilePath = Path.Combine(_configPath, HotelDestinationFileName);
            _hotelAccomodationFilePath = Path.Combine(_configPath, HotelAccomodationFileName);
            _hotelBoardFilePath = Path.Combine(_configPath, HotelBoardFileName);
            _hotelChainFilePath = Path.Combine(_configPath, HotelchainFileName);
            _hotelCategoryFilePath = Path.Combine(_configPath, HotelCategoryFileName);
            _hotelFacilitiesFilter = Path.Combine(_configPath, HotelFacilityFilterGroupFileName);	
            PopulateHotelSegmentDict(_hotelSegmentFilePath);

            PopulateHotelAccomodationDict(_hotelAccomodationFilePath);
            PopulateHotelBoardDict(_hotelBoardFilePath);
            PopulateHotelChainDict(_hotelChainFilePath);
            PopulateHotelCategoryDict(_hotelCategoryFilePath);

            PopulateHotelFacilityGroupDict(_hotelFacilityGroupFilePath);
            PopulateHotelFacilityGroupList(_hotelFacilitiesFilePath);
            PopulateHotelRoomFacilityDict(FacilityGroups);
            PopulateHotelFacilityFilter(_hotelFacilitiesFilter);

            PopulateHotelRoomList(_hotelRoomFilePath);
            PopulateHotelRoomDict(Rooms);
            PopulateHotelRoomTypeDict(Rooms);
            PopulateHotelRoomCharacteristicDict(Rooms);

            PopulateHotelRoomRateClassDict(_hotelRoomRateClassFilePath);
            PopulateHotelRoomRateTypeDict(_hotelRoomRateTypeFilePath);
            PopulateHotelRoomPaymentTypeDict(_hotelRoomPaymentTypeFilePath);
            
            PopulateHotelCountriesDict(_hotelCountriesFilePath);

            PopulateHotelDestinationList(_hotelDestinationsFilePath);
            PopulateHotelDestinationCountryDict(Countries);
            PopulateHotelDestinationDict(Countries);
            PopulateHotelZoneDict(Countries);
            //PopulateHotelCodeAndZoneDict(Countries);
            PopulateAutocomplete();
            //PopulateHotel();
        }

        private static void PopulateAutocomplete()
        {
            GetInstance()._Autocompletes = new Dictionary<long, Autocomplete>();
            long index = 1;
            foreach (var country in Countries)
            {
                foreach (var destination in country.Destinations)
                {

                    var newValue = new Autocomplete
                    {
                        Id = index,
                        Code = destination.Code,
                        Name = destination.Name + ", " + country.Name,
                        Type = AutocompleteType.Destination
                    };

                    GetInstance()._Autocompletes.Add(index, newValue);
                    index++;

                    foreach (var zone in destination.Zones)
                    {

                        newValue = new Autocomplete
                        {
                            Id = index,
                            Code = zone.Code,
                            Name = zone.Name + ", " + destination.Name + ", " + country.Name,
                            Type = AutocompleteType.Zone
                        };

                        GetInstance()._Autocompletes.Add(index, newValue);
                        index++;
                    }
                }
            }
        }

        private static void PopulateHotel()
        {
            var index = GetInstance()._Autocompletes.Count + 1;
            for (var i = 1; i < 1000; i++)
            {
                var hotel = new HotelDetailsBase();
                try
                {
                    hotel = GetInstance().GetHotelDetailFromTableStorage(i);
                    var input = new Autocomplete
                    {
                        Id = index,
                        Code = hotel.HotelCode.ToString(),
                        Type = AutocompleteType.Hotel,
                        Name = hotel.HotelName + ", " + GetInstance().
                            GetZoneNameFromDict(hotel.ZoneCode) + ", "
                            + GetInstance().GetDestinationNameFromDict(hotel.DestinationCode).Name + ", "
                            + GetInstance().GetCountryNameFromDict(hotel.CountryCode).Name
                    };
                    GetInstance()._Autocompletes.Add(index, input);
                    index++;
                }
                catch
                {

                }

            }
        }

        //POPULATE METHODS REGARDING HOTEL SEGMENT
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

        //POPULATE METHODS REGARDING FACILITY
        private static void PopulateHotelFacilityGroupList(string hotelFacilitiesFilePath)
        {
            FacilityGroups = new List<FacilityGroup>();

            using (var file = new StreamReader(hotelFacilitiesFilePath))
            {
                var line = file.ReadLine();
                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    var splittedLine = line.Split('|');
                    var foundFacilityGroup = FacilityGroups.Where(g => g.Code == Convert.ToInt32(splittedLine[0])/1000).ToList();
                    if (foundFacilityGroup.Count == 0)
                    {
                        var newFacilityGroup = new FacilityGroup
                        {
                            Code = Convert.ToInt32(splittedLine[0]) / 1000,
                            NameEn =
                                GetInstance()
                                    .GetHotelFacilityGroupEng(Convert.ToInt32(splittedLine[0]) / 1000),
                            NameId =
                                GetInstance()
                                    .GetHotelFacilityGroupId(Convert.ToInt32(splittedLine[0]) / 1000),
                            Facilities = new List<Facility>
                            {
                                new Facility
                                {
                                    Code = Convert.ToInt32(splittedLine[0]) % 1000,
                                    NameEn = splittedLine[1],
                                    NameId = splittedLine[2],
                                }
                            }
                        };

                        FacilityGroups.Add(newFacilityGroup);
                    }
                    else
                    {
                        var foundFacility =
                            foundFacilityGroup[0].Facilities.Where(f => f.Code == Convert.ToInt32(splittedLine[0])/1000)
                                .ToList();
                        if (foundFacility.Count == 0)
                        {
                            var newFacility = new Facility
                            {
                                Code = Convert.ToInt32(splittedLine[0]) % 1000,
                                NameEn = splittedLine[1],
                                NameId = splittedLine[2],
                            };
                            FacilityGroups.Where(g => g.Code == Convert.ToInt32(splittedLine[0])/1000).
                                ToList()[0].Facilities.Add(newFacility);
                        }
                    }
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

        private static void PopulateHotelFacilityFilter(String hotelFacilityFilter)
        {
            HotelFacilityFilters = new Dictionary<string, FacilityFilter>();
            FacilityFilters = new List<FacilityFilter>();

            using (var file = new StreamReader(hotelFacilityFilter))
            {
                var line = file.ReadLine();
                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    var splittedLine = line.Split('|');
                    if (HotelFacilityFilters.ContainsKey(splittedLine[1]))
                    {
                        HotelFacilityFilters[splittedLine[1]].FacilityCode.Add(splittedLine[0]);
                    }
                    else
                    {
                        HotelFacilityFilters.Add(splittedLine[1], new FacilityFilter
                        {
                            Code = splittedLine[1],
                            FacilityCode = new List<string> { splittedLine[0] }
                        });
                    }
                }
            }
        }

        private static void PopulateHotelRoomFacilityDict(List<FacilityGroup> facilityGroups)
        {
            HotelRoomFacility = new Dictionary<int, Facility>();

            for (var index = 0; index < facilityGroups.Where(f => f.Code == 60).ToList().Count; index++)
            {
                var facilityGroup = facilityGroups.Where(f => f.Code == 60).ToList()[index];
                foreach (var fac in facilityGroup.Facilities)
                {
                    HotelRoomFacility.Add(fac.Code, fac);
                }
            }
        }

        //POPULATE METHODS REGARDING HOTEL ROOM

        private static void PopulateHotelRoomList(string hotelRoomFilePath)
        {
            Rooms = new List<Room>();
            using (var file = new StreamReader(hotelRoomFilePath))
            {
                var line = file.ReadLine();
                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    var splittedLine = line.Split('|');
                    var newHotelRoom = new Room
                    {
                        RoomCharacteristic = new RoomCharacteristic
                        {
                            CharacteristicCd = splittedLine[2],
                            CharacteristicDescEn = splittedLine[10],
                            CharacteristicDescId = splittedLine[13]
                        },
                        RoomType = new HotelRoomType
                        {
                            Type = splittedLine[1],
                            DescEn = splittedLine[9],
                            DescId = splittedLine[12]
                        },
                        MinPax = Convert.ToInt32(splittedLine[3]),
                        MaxPax = Convert.ToInt32(splittedLine[4]),
                        MaxAdult = Convert.ToInt32(splittedLine[5]),
                        MaxChild = Convert.ToInt32(splittedLine[6]),
                        MinAdult = Convert.ToInt32(splittedLine[7]),
                        RoomCd = splittedLine[0],
                        RoomDescEn = splittedLine[8],
                        RoomDescId = splittedLine[11]
                    };
                    Rooms.Add(newHotelRoom);
                }
            }
        }

        private static void PopulateHotelRoomDict(IEnumerable<Room> rooms )
        {
            HotelRoomDict = new Dictionary<string, Room>();

            foreach (var room in rooms)
            {
                HotelRoomDict.Add(room.RoomCd, room);
            }
            
        }

        private static void PopulateHotelRoomTypeDict(IEnumerable<Room> rooms )
        {
            HotelRoomTypeDict = new Dictionary<string, HotelRoomType>();

            foreach (var room in rooms)
            {
                HotelRoomType x;
                if (!HotelRoomTypeDict.TryGetValue(room.RoomType.Type, out x))
                {
                    HotelRoomTypeDict.Add(room.RoomType.Type, room.RoomType);
                }
            }
        }

        private static void PopulateHotelRoomCharacteristicDict(IEnumerable<Room> rooms)
        {
            HotelRoomCharacteristicDict= new Dictionary<string, RoomCharacteristic>();
            foreach (var room in rooms)
            {
                RoomCharacteristic x;
                if (!HotelRoomCharacteristicDict.TryGetValue(room.RoomCharacteristic.CharacteristicCd, out x))
                {
                    HotelRoomCharacteristicDict.Add(room.RoomCharacteristic.CharacteristicCd, room.RoomCharacteristic);
                }
            }
            
        }

        private static void PopulateHotelRoomRateClassDict(String hotelRoomRateClassFilePath)
        {
            GetInstance().HotelRoomRateClassDict = new Dictionary<string, RateClass>(); ;

            using (var file = new StreamReader(hotelRoomRateClassFilePath))
            {
                var line = file.ReadLine();
                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    var splittedLine = line.Split('|');

                    GetInstance().HotelRoomRateClassDict.Add(splittedLine[0], new RateClass
                    {
                        Code = splittedLine[0],
                        DescEn = splittedLine[1],
                        DescId = splittedLine[2]
                    });
                }
            }
        }

        private static void PopulateHotelRoomRateTypeDict(String hotelRoomRateTypeFilePath)
        {
            GetInstance().HotelRoomRateTypeDict = new Dictionary<string, RateType>();

            using (var file = new StreamReader(hotelRoomRateTypeFilePath))
            {
                var line = file.ReadLine();
                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    var splittedLine = line.Split('|');

                    GetInstance().HotelRoomRateTypeDict.Add(splittedLine[0], new RateType
                    {
                        Type = splittedLine[0],
                        DescEn = splittedLine[1],
                        DescId = splittedLine[2]
                    });
                }
            }
        }

        private static void PopulateHotelRoomPaymentTypeDict(String hotelRoomPaymentTypeFilePath)
        {
            GetInstance().HotelRoomPaymentTypeDict = new Dictionary<string, PaymentType>();

            using (var file = new StreamReader(hotelRoomPaymentTypeFilePath))
            {
                var line = file.ReadLine();
                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    var splittedLine = line.Split('|');

                    GetInstance().HotelRoomPaymentTypeDict.Add(splittedLine[0], new PaymentType
                    {
                        Type = splittedLine[0],
                        DescEn = splittedLine[1],
                        DescId = splittedLine[2]
                    });
                }
            }
        }


        private static void PopulateHotelAccomodationDict(String hotelAccomodationFilePath)
        {
            HotelAccomodations = new Dictionary<string, Accommodation>();

            using (var file = new StreamReader(hotelAccomodationFilePath))
            {
                var line = file.ReadLine();
                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    var splittedLine = line.Split('|');

                    HotelAccomodations.Add(splittedLine[0], new Accommodation
                    {
                        Code = splittedLine[0],
                        MultiDescription = splittedLine[1],
                        TypeNameEn = splittedLine[2],
                        TypeNameId = splittedLine[3]
                    });
                }
            }
        }


        private static void PopulateHotelBoardDict(String hotelBoardFilePath)
        {
            HotelBoards = new Dictionary<string, Board>();

            using (var file = new StreamReader(hotelBoardFilePath))
            {
                var line = file.ReadLine();
                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    var splittedLine = line.Split('|');

                    HotelBoards.Add(splittedLine[0], new Board
                    {
                        Code = splittedLine[0],
                        NameEn = splittedLine[1],
                        NameId = splittedLine[2],
                        MultilingualCode = splittedLine[3]
                    });
                }
            }
        }

        private static void PopulateHotelChainDict(String hotelChainFilePath)
        {
            HotelChains = new Dictionary<string, Chain>();

            using (var file = new StreamReader(hotelChainFilePath))
            {
                var line = file.ReadLine();
                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    var splittedLine = line.Split('|');

                    HotelChains.Add(splittedLine[0], new Chain
                    {
                        Code = splittedLine[0],
                        Description = splittedLine[1],
                    });
                }
            }
        }

        private static void PopulateHotelCategoryDict(String hotelCategoryFilePath)
        {
            HotelCategories = new Dictionary<string, Category>();

            using (var file = new StreamReader(hotelCategoryFilePath))
            {
                var line = file.ReadLine();
                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    var splittedLine = line.Split('|');

                    HotelCategories.Add(splittedLine[0], new Category
                    {
                        Code = splittedLine[0],
                        SimpleCode = int.Parse(splittedLine[1]),
                        AccomodationType = splittedLine[2],
                        NameEn = splittedLine[4],
                        NameId = splittedLine[5]
                    });
                }
            }
        }

        private static void PopulateHotelCountriesDict(String hotelCountriesFilePath)
        {
            GetInstance().HotelCountry = new Dictionary<string, CountryDict>();

            using (var file = new StreamReader(hotelCountriesFilePath))
            {
                var line = file.ReadLine();
                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    var splittedLine = line.Split('|');
                    GetInstance().HotelCountry.Add(splittedLine[0], new CountryDict
                    {
                        CountryCode = splittedLine[0],
                        IsoCode = splittedLine[1],
                        Name = splittedLine[2]
                    });
                }
            }
        }

        //POPULATE METHODS REGARDING DESTINATION AND ZONE
        private static void PopulateHotelDestinationList(String hotelDestinationsFilePath)
        {
            Countries = new List<Country>();
            using (var file = new StreamReader(hotelDestinationsFilePath))
            {
                var line = file.ReadLine();
                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    var splittedLine = line.Split('|');
                    var foundCountry = Countries.Where(c => c.Code == splittedLine[2]).ToList();
                    if (foundCountry.Count == 0)
                    {
                        var newCountry = new Country
                        {
                            Code = splittedLine[2],
                            Name = GetInstance().GetHotelCountryName(splittedLine[2]),
                            IsoCode = GetInstance().GetHotelCountryIsoCode(splittedLine[2]),
                            Destinations = new List<Destination>
                            {
                                new Destination
                                {
                                    Code = splittedLine[0],
                                    Name = splittedLine[1],
                                    Zones = new List<Zone>
                                    {
                                        new Zone
                                        {
                                            Code = splittedLine[0] + "-" + splittedLine[4],
                                            Name = splittedLine[5],
                                            DestinationCode = splittedLine[0],
                                            //Hotel = new Hotels
                                            //    {
                                            //        HotelCodes = GetInstance().GetHotelListByLocationFromStorage(splittedLine[0] + "-" + splittedLine[4]),
                                            //        ZoneCode = splittedLine[0] + "-" + splittedLine[4],
                                            //    }
                                        }
                                    },
                                    CountryCode = splittedLine[2]
                                    }
                                }
                        };
                        Countries.Add(newCountry);
                        
                    }
                    else
                    {
                        var foundDestination = foundCountry[0].Destinations.Where(d => d.Code == splittedLine[0]).ToList();
                        if (foundDestination.Count == 0)
                        {
                            var newDestination = new Destination
                            {
                                Code = splittedLine[0],
                                Name = splittedLine[1],
                                Zones = new List<Zone>
                                {
                                    new Zone
                                    {
                                        Code = splittedLine[0] + "-" + splittedLine[4],
                                        Name = splittedLine[5],
                                        DestinationCode = splittedLine[0],
                                        //Hotel = new Hotels
                                        //    {
                                        //        HotelCodes = GetInstance().GetHotelListByLocationFromStorage(splittedLine[0] + "-" + splittedLine[4]),
                                        //        ZoneCode = splittedLine[0] + "-" + splittedLine[4],
                                        //    }
                                    }
                                },
                                CountryCode = splittedLine[2]
                            };
                            Countries.Where(c => c.Code == splittedLine[2]).ToList()[0].Destinations.Add(newDestination);
                        }
                        else
                        {
                            var foundZone = foundDestination.Where(d => d.Code == splittedLine[0] + "-" + splittedLine[4]).ToList();
                            if (foundZone.Count == 0)
                            {
                                var newZone = new Zone
                                {
                                    Code = splittedLine[0] + "-" + splittedLine[4],
                                    Name = splittedLine[5],
                                    DestinationCode = splittedLine[0],
                                    //Hotel = new Hotels
                                    //{
                                    //    HotelCodes = GetInstance().GetHotelListByLocationFromStorage(splittedLine[0] + "-" + splittedLine[4]),
                                    //    ZoneCode = splittedLine[0] + "-" + splittedLine[4],
                                    //}
                                };
                                Countries.Where(c => c.Code == splittedLine[2]).ToList()[0].Destinations.Where(d => d.Code == splittedLine[0]).ToList()[0].Zones.Add(newZone);
                            }
                        }
                    }               
                }
            }
        }

        private static void PopulateHotelDestinationCountryDict(IEnumerable<Country> countries)
        {
            GetInstance().HotelDestinationCountryDict = new Dictionary<string, Country>();
            foreach (var country in countries)
            {
                GetInstance().HotelDestinationCountryDict.Add(country.Code, country);
            }
        }

        private static void PopulateHotelDestinationDict(IEnumerable<Country> countries)
        {
            GetInstance().HotelDestinationDict = new Dictionary<string, Destination>();
            foreach (var destination in countries.SelectMany(country => country.Destinations))
            {
                GetInstance().HotelDestinationDict.Add(destination.Code, destination);
            }
        }

        private static void PopulateHotelZoneDict(IEnumerable<Country> countries)
        {
            GetInstance().HotelDestinationZoneDict = new Dictionary<string, Zone>();
            foreach (var zone in countries.SelectMany(country => country.Destinations).SelectMany(destination => destination.Zones))
            {
                GetInstance().HotelDestinationZoneDict.Add(zone.Code, zone);
            }
        }

        //private void PopulateHotelCodeAndZoneDict(IEnumerable<Country> countries)
        //{
        //    GetInstance().HotelCodeAndZoneDict = new Dictionary<int, string>();
        //    foreach (var country in countries)
        //    {
        //        foreach (var destination in country.Destinations)
        //        {
        //            foreach (var zone in destination.Zones)
        //            {
        //                foreach (var hotel in zone.Hotel.HotelCodes)
        //                {
        //                    HotelCodeAndZoneDict.Add(hotel, zone.Code);
        //                }
        //            }
        //        }
        //    }
        //}
        //GET METHOD REGARDING AUTOCOMPLETE
        public Autocomplete GetLocationById(long id)
        {
            var value = new Autocomplete();
            _Autocompletes.TryGetValue(id, out value);
            return value;
            }
                
        //GET METHODS REGARDING SEGMENT
        public string GetHotelSegmentId(string code)
        {
            if (code == null)
            {
                return "";
            }
            var value = "";
            HotelSegmentDictId.TryGetValue(code, out value);
            return value;
        }
        public string GetHotelSegmentEng(string code)
        {
            if (code == null)
            {
                return "";
            }
            var value = "";
            HotelSegmentDictEng.TryGetValue(code, out value);
            return value; 
        }

        //GET METHODS REGARDING FACILITY
        public Facility GetHotelFacility(int code)
        {
            //var found = false;
            foreach (var facility in FacilityGroups.Where(@group => @group.Code == code/1000).SelectMany(@group => @group.Facilities.Where(facility => facility.Code == code%1000)))
            {
                return facility;
            }

                return new Facility();
            }

        public string GetHotelFacilityDescId(int code)
        {
            foreach (var facility in FacilityGroups.Where(@group => @group.Code == code/1000).SelectMany(@group => @group.Facilities.Where(facility => facility.Code == code % 1000)))
            {
                return facility.NameId;
            }

                return "";
            }

        public string GetHotelFacilityDescEn(int code)
        {
            foreach (var facility in FacilityGroups.Where(@group => @group.Code == code/1000).SelectMany(@group => @group.Facilities.Where(facility => facility.Code == code % 1000)))
            {
                return facility.NameEn;
            }

                return "";
            }

        public string GetHotelFacilityGroupId(int code)
        {
            var value = "";
            HotelFacilityGroupDictId.TryGetValue(code, out value);
            return value;
            }
        public string GetHotelFacilityGroupEng(int code)
        {
            var value = "";
            HotelFacilityGroupDictEng.TryGetValue(code, out value);
            return value;
            }
        public Facility GetHotelRoomFacility(int code)
        {
            var value = new Facility();
            HotelRoomFacility.TryGetValue(code, out value);
            return value;
            
        }
        public string GetHotelRoomFacilityDescId(int roomFacilityCd)
        {
            foreach (var facility in FacilityGroups.Where(@group => @group.Code == 60).SelectMany(@group => @group.Facilities.Where(facility => facility.Code == roomFacilityCd)))
            {
                return facility.NameId;
            }

                return "";
            }

        public string GetHotelRoomFacilityDescEn(int roomFacilityCd)
        {
            foreach (var facility in FacilityGroups.Where(@group => @group.Code == 60).SelectMany(@group => @group.Facilities.Where(facility => facility.Code == roomFacilityCd)))
            {
                return facility.NameEn;
            }

                return "";
            }

        public List<Facility> GetAllFacilitiesInAGroup(int cd)
        {
            foreach (var @group in FacilityGroups.Where(@group => @group.Code == cd))
            {
                return @group.Facilities;
            }

                return new List<Facility>();
            }

        public string GetNameOfFacilityGroup(int facilityCd, string lang)
        {
            if (lang == "EN")
            {
                foreach (var @group in FacilityGroups.Where(@group => @group.Code == facilityCd/1000))
            {
                    return @group.NameEn;
                }
            }
            else
            {
                foreach (var @group in FacilityGroups.Where(@group => @group.Code == facilityCd / 1000))
            {
                    return @group.NameId;
                }
            }
            
                return "";
            
        }

        //GET METHODS REGARDING HOTEL ROOM
        public Room GetHotelRoom(string code)
        {
            if (code == null)
            {
                return new Room();
            }

            foreach (var room in Rooms.Where(room => room.RoomCd == code))
            {
                return room;
            }

                return new Room();
            }

        public string GetHotelRoomDescEn(String cd)
        {
            if (cd == null)
            {
                return "";
            }
            foreach (var room in Rooms.Where(room => room.RoomCd == cd))
            {
                return room.RoomDescEn;
            }

                return "";
            }

        public string GetHotelRoomDescId(String cd)
        {
            if (cd == null)
            {
                return "";
            }
            foreach (var room in Rooms.Where(room => room.RoomCd == cd))
            {
                return room.RoomDescId;
            }

                return "";
            }

        public int GetMaxAdult(string code)
        {
            return code == null ? 0 : Rooms.Where(room => room.RoomCd == code).Select(room => room.MaxAdult).FirstOrDefault();
        }

        public int GetMaxChild(string code)
        {
            if (code == null)
            {
                return 0;
            }
            return Rooms.Where(room => room.RoomCd == code).Select(room => room.MaxChild).FirstOrDefault();
        }

        public int GetPaxCapacity(string code)
        {
            if (code == null)
            {
                return 0;
            }
            return Rooms.Where(room => room.RoomCd == code).Select(room => room.MaxPax).FirstOrDefault();
        }
        public HotelRoomType GetHotelRoomType(string code)
        {
            if (code == null)
            {
                return new HotelRoomType();
            }
            var value = new HotelRoomType();
            HotelRoomTypeDict.TryGetValue(code, out value);
            return value;
        }

        public string GetHotelRoomTypeDescEn(String cd)
        {
            if (cd == null)
            {
                return "";
            }
            var value = new HotelRoomType();
            var found = HotelRoomTypeDict.TryGetValue(cd, out value);
            return found ? value.DescEn : "";
        }

        public string GetHotelRoomTypeDescId(String cd)
        {
            if (cd == null)
            {
                return "";
            }
            var value = new HotelRoomType();
            var found = HotelRoomTypeDict.TryGetValue(cd, out value);
            return found ? value.DescId : "";
        }

        public RoomCharacteristic GetHotelRoomCharacteristic(string code)
        {
            if (code == null)
            {
                return new RoomCharacteristic();
            }
            var value = new RoomCharacteristic();
            HotelRoomCharacteristicDict.TryGetValue(code, out value);
            return value;
        }

        public string GetHotelRoomCharacteristicDescEn(string code)
        {
            if (code == null)
            {
                return "";
            }
            var value = new RoomCharacteristic();
            var found = HotelRoomCharacteristicDict.TryGetValue(code, out value);
            return found ? value.CharacteristicDescEn : "";
        }

        public string GetHotelRoomCharacteristicDescId(string code)
        {
            if (code == null)
            {
                return "";
            }
            var value = new RoomCharacteristic();
            var found = HotelRoomCharacteristicDict.TryGetValue(code, out value);
            return found ? value.CharacteristicDescId : "";
        }

        public RateClass GetHotelRoomRateClass(string code)
        {
            if (code == null)
            {
                return new RateClass();
            }
            var value = new RateClass();
            HotelRoomRateClassDict.TryGetValue(code, out value);
            return value;
        }

        public string GetHotelRoomRateClassId(string code)
        {
            if (code == null)
            {
                return "";
            }
            var value = new RateClass();
            var found = HotelRoomRateClassDict.TryGetValue(code, out value);
            return found ? value.DescId : "";          
        }

        public string GetHotelRoomRateClassEng(string code)
        {
            if (code == null)
            {
                return "";
            }
            var value = new RateClass();
            var found = HotelRoomRateClassDict.TryGetValue(code, out value);
            return found ? value.DescEn : ""; 
        }

        public RateType GetHotelRoomRateType(string code)
        {
            if (code == null)
            {
                return new RateType();
            }
            var value = new RateType();
            HotelRoomRateTypeDict.TryGetValue(code, out value);
            return value;
        }

        public string GetHotelRoomRateTypeId(string code)
        {
            if (code == null)
            {
                return "";
            }
            var value = new RateType();
            var found = HotelRoomRateTypeDict.TryGetValue(code, out value);
            return found ? value.DescId : "";          
        }

        public string GetHotelRoomRateTypeEng(string code)
        {
            if (code == null)
            {
                return "";
            }
            var value = new RateType();
            var found = HotelRoomRateTypeDict.TryGetValue(code, out value);
            return found ? value.DescEn : "";  
        }

        public PaymentType GetHotelRoomPaymentType(string code)
        {
            if (code == null)
            {
                return new PaymentType();
            }
            var value = new PaymentType();
            HotelRoomPaymentTypeDict.TryGetValue(code, out value);
            return value;
        }

        public string GetHotelRoomPaymentTypeId(string code)
        {
            if (code == null)
            {
                return "";
            }
            var value = new PaymentType();
            var found = HotelRoomPaymentTypeDict.TryGetValue(code, out value);
            return found ? value.DescId : ""; 
        }

        public string GetHotelRoomPaymentTypeEng(string code)
        {
            if (code == null)
            {
                return "";
            }
            var value = new PaymentType();
            var found = HotelRoomPaymentTypeDict.TryGetValue(code, out value);
            return found ? value.DescEn : ""; 
        }

        //
        public string GetHotelCountryName(string code)
        {
            if (code == null)
            {
                return "";
            }
            var value = new CountryDict();
            var found = HotelCountry.TryGetValue(code, out value);
            return found ? value.Name : ""; 
        }

        public string GetHotelCountryIsoCode(string code)
        {
            if (code == null)
            {
                return "";
            }
            var value = new CountryDict();
            var found = HotelCountry.TryGetValue(code, out value);
            return found ? value.IsoCode : ""; 
        }

        public CountryDict GetHotelCountry(string code)
        {
            if (code == null)
            {
                return new CountryDict();
            }
            var value = new CountryDict();
            HotelCountry.TryGetValue(code, out value);
            return value;
        }

        public string GetCountryFromDestination(string cd)
        {
            if (cd == null)
            {
                return "";
            }
            foreach (var country in Countries.Where(country => country.Destinations.Any(d => d.Code == cd)))
            {
                return country.Code;
            }

                return "";
            }

        //GET METHODS REGARDING DESTINATION AND ZONE
        public Country GetHotelCountryFromMasterList(string countryCode)
        {
            if (countryCode == null)
            {
                return new Country();
            }
            foreach (var country in Countries.Where(country => country.Code == countryCode))
            {
                return country;
            }
                return new Country();
            }

        public Country GetCountryNameFromDict(string countryCode)
        {
            if (countryCode == null)
            {
                return new Country();
            }
            var value = new Country();
            HotelDestinationCountryDict.TryGetValue(countryCode, out value);
            return value;
        }

        public List<string> GetAllLocations()
        {
            var locations = new List<string>();

            foreach (var country in Countries)
            {
                foreach (var dest in country.Destinations)
                {
                    locations.Add(dest.Code);
                    locations.AddRange(dest.Zones.Select(zone => zone.Code));
                }
            }

            return locations;
        }

        public Destination GetDestinationNameFromDict(string destinationCode)
        {
            if (destinationCode == null)
            {
                return new Destination();
            }
            var value = new Destination();
            HotelDestinationDict.TryGetValue(destinationCode, out value);
            return value;
        }

        public Zone GetHotelZoneFromDict(string zoneCode)
        {
            if (zoneCode == null)
            {
                return new Zone();
            }
            var value = new Zone();
            HotelDestinationZoneDict.TryGetValue(zoneCode, out value);
            return value;
        }

        public string GetZoneNameFromDict(string zoneCode)
        {
            if (zoneCode == null)
            {
                return "";
            }
            var value = new Zone();
            var found = HotelDestinationZoneDict.TryGetValue(zoneCode, out value);
            return found ? value.Name : "";
        }

        public Zone GetZoneFromHotel(int hotelCd)
        {
            var value = "";
            var found = HotelCodeAndZoneDict.TryGetValue(hotelCd, out value);
            if (!found) return new Zone();
            var zone = new Zone();
            var foundZone = HotelDestinationZoneDict.TryGetValue(value, out zone);
            return foundZone ? zone : new Zone();
            }

        public Destination GetDestinationFromZone(string zoneCd)
        {
            if (zoneCd == null)
            {
                return new Destination();
            }
            var value = new Zone();
            var found = HotelDestinationZoneDict.TryGetValue(zoneCd, out value);
            if (!found) return new Destination();
            var destCd = value.DestinationCode;
            var dest = new Destination();
            var foundDest = HotelDestinationDict.TryGetValue(destCd, out dest);
            return foundDest ? dest : new Destination();
        }

        //GETTER FOR HOTEL CHAIN
        public Chain GetHotelChain(string code)
        {
            if (code == null)
            {
                return new Chain();
            }
            var value = new Chain();
            HotelChains.TryGetValue(code, out value);
            return value;
        }

        public string GetHotelChainDesc(string code)
        {
            if (code == null)
            {
                return "";
            }
            var value = new Chain();
            var found = HotelChains.TryGetValue(code, out value);
            return found ? value.Description : "";
        }

        //GETTER FOR HOTEL ACCOMODATION
        public Accommodation GetHotelAccomodation(string code)
        {
            if (code == null)
            {
                return new Accommodation();
            }
            var value = new Accommodation();
            HotelAccomodations.TryGetValue(code, out value);
            return value;
            }

        public string GetHotelAccomodationMultiDesc(string code)
        {
            if (code == null)
            {
                return "";
            }
            var value = new Accommodation();
            var found = HotelAccomodations.TryGetValue(code, out value);
            return found ? value.MultiDescription : "";
        }

        public string GetHotelAccomodationDescEng(string code)
        {
            if (code == null)
            {
                return "";
            }
            var value = new Accommodation();
            var found = HotelAccomodations.TryGetValue(code, out value);
            return found ? value.TypeNameEn : "";
        }

        public string GetHotelAccomodationDescId(string code)
        {
            if (code == null)
            {
                return "";
            }
            var value = new Accommodation();
            var found = HotelAccomodations.TryGetValue(code, out value);
            return found ? value.TypeNameId : "";
        }

        //GETTER FOR HOTEL BOARD
        public Board GetHotelBoard(string code)
        {
            if (code == null)
            {
                return new Board();
            }
            var value = new Board();
            HotelBoards.TryGetValue(code, out value);
            return value;
        }

        public string GetHotelBoardDescEn(string code)
        {
            if (code == null)
            {
                return "";
            }
            var value = new Board();
            var found = HotelBoards.TryGetValue(code, out value);
            return found ? value.NameEn : "";
        }

        public string GetHotelBoardDescId(string code)
        {
            if (code == null)
            {
                return "";
            }
            var value = new Board();
            var found = HotelBoards.TryGetValue(code, out value);
            return found ? value.NameId : "";
        }

        //GETTER FOR HOTEL CATEGORY
        public Category GetHotelCategory(string code)
        {
            if (code == null)
            {
                return new Category();
            }
            var value = new Category();
            HotelCategories.TryGetValue(code, out value);
            return value;
        }

        public int GetSimpleCodeByCategoryCode(string code)
        {
            if (code == null)
            {
                return 0;
            }
            var value = new Category();
            var found = HotelCategories.TryGetValue(code, out value);
            return found ? value.SimpleCode : 0;
        }

        public string GetHotelCategoryDescEn(string code)
        {
            if (code == null)
            {
                return "";
            }
            var value = new Category();
            var found = HotelCategories.TryGetValue(code, out value);
            return found ? value.NameEn : "";
        }

        public string GetHotelCategoryDescId(string code)
        {
            if (code == null)
            {
                return "";
            }
            var value = new Category();
            var found = HotelCategories.TryGetValue(code, out value);
            return found ? value.NameId : "";
        }
    }
}
