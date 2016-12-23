using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Query;
using Lunggo.ApCommon.Hotel.Wrapper.GoogleMaps.Geocoding;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Content;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Content.Model;
using Lunggo.Framework.BlobStorage;
using Lunggo.Framework.Config;
using Lunggo.Framework.Documents;
using Lunggo.Framework.TableStorage;
using Microsoft.Azure.Documents;
using Microsoft.WindowsAzure.Storage.Table;
using RestSharp;
using FileInfo = Lunggo.Framework.SharedModel.FileInfo;
using Image = Lunggo.ApCommon.Hotel.Model.Image;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {
        public void UpdateHotelContentAll()
        {
            /*Update Content From HotelBeds*/
            UpdateHotelDetailStorage();
            UpdateHotelRateCommentStorage();
            UpdateCountryStorage();
            UpdateDestinationStorage();
            UpdateHotelBoardStorage();
            UpdateHotelChainStorage();
            UpdateHotelAccomodationStorage();
            UpdateHotelCategoryStorage();
            UpdateHotelFacilityStorage();
            UpdateHotelFacilityGroupStorage();
            UpdateHotelSegmentStorage();
            UpdateHotelRoomStorage();

            /*Another Update*/
            UpdateLocation();
            UpdateHotelListByLocationContent();
            SaveHotelDetailByLocation();
            UpdateAutocomplete();
        }
        public void UpdateAutocomplete()
        {
            Console.WriteLine("Update AutoComplete");
            GetInstance()._Autocompletes = new Dictionary<long, Autocomplete>();
            long index = 1;
            foreach (var country in Countries)
            {
                foreach (var destination in country.Destinations)
                {
                    var hotelDestCount = 0;
                    Console.WriteLine("Destination : {0}", destination.Code);
                    if (destination.Zones != null)
                    {
                        foreach (var zone in destination.Zones)
                        {

                            if (!string.IsNullOrEmpty(zone.Code))
                            {
                                Console.WriteLine("Zone : {0}", zone.Code);
                                var hotelZoneTotal = GetInstance().GetHotelListByLocationFromStorage(zone.Code).Count;
                                hotelDestCount += hotelZoneTotal;
                                var singleAutoComplete = new HotelAutoComplete
                                {
                                    Id = index,
                                    Code = zone.Code,
                                    Zone = zone.Name,
                                    Destination = destination.Name,
                                    Country = country.Name,
                                    Type = 2,
                                    HotelCount = hotelZoneTotal,
                                };

                                GetInstance().AutocompleteList.Add(singleAutoComplete);
                                index++;

                                if (zone.Areas != null)
                                {
                                    foreach (var area in zone.Areas)
                                    {
                                        if (!string.IsNullOrEmpty(area.Code))
                                        {
                                            Console.WriteLine("Area : {0}", area.Code);
                                            singleAutoComplete = new HotelAutoComplete
                                            {
                                                Id = index,
                                                Code = area.Code,
                                                Area = area.Name,
                                                Zone = zone.Name,
                                                Destination = destination.Name,
                                                Country = country.Name,
                                                Type = 3,
                                                HotelCount = GetInstance().GetHotelListByLocationFromStorage(area.Code).Count,
                                            };
                                            GetInstance().AutocompleteList.Add(singleAutoComplete);
                                            index++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    var singleDest = new HotelAutoComplete
                    {
                        Id = index,
                        Code = destination.Code,
                        Destination = destination.Name,
                        Country = country.Name,
                        Type = 1,
                        HotelCount = hotelDestCount,
                    };
                    GetInstance().AutocompleteList.Add(singleDest);
                    index++;
                }
            }
            SaveHotelAutocompleteToBlob();
        }
        public void UpdateTruncatedHotelDetailContent()
        {
            for (var i = 1; i <= 150000; i++)
            {
                try
                {
                    var hotel = GetInstance().GetHotelDetailFromTableStorage(i);
                    Console.WriteLine("hotelCd: " + i);
                    var imageHotel = hotel.ImageUrl == null ? null : hotel.ImageUrl.Where(u => u.Type == "GEN").Take(1).FirstOrDefault();
                    var truncatedHotelDetail = new HotelDetailsBase
                    {
                        HotelName = hotel.HotelName,
                        StarRating = hotel.StarRating,
                        ImageUrl = imageHotel== null ?null:new List<Image>
                        {
                            imageHotel
                        },
                        WifiAccess = hotel.Facilities != null && 
                            ((hotel.Facilities != null || hotel.Facilities.Count != 0) && 
                            hotel.Facilities.Any(f => (f.FacilityGroupCode == 60 && f.FacilityCode == 261) 
                            || (f.FacilityGroupCode == 70 && f.FacilityCode == 550))),
                        IsRestaurantAvailable = hotel.Facilities != null && ((hotel.Facilities != null || hotel.Facilities.Count != 0) && 
                            hotel.Facilities.Any(f => (f.FacilityGroupCode == 71 && f.FacilityCode == 200)
                            || (f.FacilityGroupCode == 75 && f.FacilityCode == 840)
                            || (f.FacilityGroupCode == 75 && f.FacilityCode == 845))),
                        Latitude = hotel.Latitude,
                        Longitude = hotel.Longitude,
                        DestinationCode = hotel.DestinationCode,
                        ZoneCode = hotel.ZoneCode,
                        City = hotel.City,
                    };
                    SaveTruncatedHotelDetailToTableStorage(truncatedHotelDetail, hotel.HotelCode);
                    Console.WriteLine("Hotel detail truncated saved for: "+ i);
                }
                catch
                {
                    Console.WriteLine("Hotel with code: " + i + " not found");
                }
            }               
        }

        public void UpdateHotelAmenitiesContent()
        {
            for (var i = 1; i <= 150000; i++)
            {
                try
                {
                    var hotel = GetInstance().GetHotelDetailFromTableStorage(i);
                    Console.WriteLine("hotelCd: " + i);
                    var truncatedHotelDetail = new HotelDetailsBase
                    {
                        HotelName = hotel.HotelName,
                        HotelCode = hotel.HotelCode,
                        Facilities = hotel.Facilities,
                        AccomodationType = hotel.AccomodationType
                    };
                    SaveHotelAmenitiesAndAccomodationTypeToTableStorage(truncatedHotelDetail, hotel.HotelCode);
                    Console.WriteLine("Amenities and Accommodation Type saved for: " + i);
                }
                catch
                {
                    Console.WriteLine("Hotel with code: " + i + " not found");
                }

            }
        }

        public void UpdateHotelListByLocationContent()
        {
            Console.WriteLine("Update Hotel List By Location");
            for (var i = 0; i <= 650000; i++)
            {
                try
                {
                    var hotel = GetHotelDetailFromTableStorage(i);
                    Console.WriteLine("hotelCd: " + i);
                    if (hotel.ZoneCode != null)
                        SaveHotelLocationInStorage(hotel.DestinationCode, hotel.ZoneCode, hotel.HotelCode);
                    if (hotel.AreaCode != null)
                        SaveHotelLocationInStorage(hotel.DestinationCode, hotel.AreaCode, hotel.HotelCode);
                    Console.WriteLine("Hotel Location saved for: " + i);
                }
                catch
                {
                    Console.WriteLine("Hotel with code: " + i + " not found");
                }
            }
        }

        public void UpdateHotelImage()
        {
            //Get hotelDetail
            //Get hotelImage
            var baseBiggerUrl = ConfigManager.GetInstance().GetConfigValue("hotel", "bigSizeImage");
            
            var blobService = BlobStorageService.GetInstance();
            for (var i = 1; i <= 600000; i++)
            {
                try
                {
                    var hotel = GetHotelDetailFromTableStorage(i);
                    Console.WriteLine("hotelCd: " + i);
                    if (hotel.ImageUrl != null)
                    {
                        using (WebClient webClient = new WebClient())
                        {
                            foreach (var url in hotel.ImageUrl)
                            {
                                try
                                {
                                    var completedUrl = string.Concat(baseBiggerUrl, url.Path);
                                    byte[] imageFile = webClient.DownloadData(completedUrl);
                                    string fileName = url.Path.Split('/').LastOrDefault();
                                    blobService.WriteFileToBlob(new BlobWriteDto
                                    {
                                        FileBlobModel = new FileBlobModel
                                        {
                                            FileInfo = new FileInfo
                                            {
                                                FileName = i + "/" + url.Type + "/" +fileName,
                                                ContentType = "image/jpeg",
                                                FileData = imageFile
                                            },
                                            Container = "HotelImage"
                                        },
                                        SaveMethod = SaveMethod.Force
                                    });
                                }
                                catch
                                {
                                    continue;
                                }
                            }
                        }
                    }
                    Console.WriteLine("Hotel Location saved for: " + i);
                }
                catch
                {
                    Console.WriteLine("Hotel with code: " + i + " not found");
                }
            }
        }

        public void UpdateLocation()
        {
            Console.WriteLine("Update Hotel Zone dan Area");
            var destinationList = GetDestinationByCountryList("ID");
            var existedDestination = GetInstance().GetHotelDestinationFromStorage();
            string[] apiKey = GeoCodeApiKeyList;
            var keyIndex = 0;
            Console.WriteLine("Key {0} : {1}", keyIndex, apiKey[keyIndex]);
            var client = new GeocodingClient(apiKey[keyIndex]);
            var HotelAreaDict = new Dictionary<string, string>();
            var HotelZoneDict = new Dictionary<string, string>();
            foreach (var destination in destinationList)
            {
                var hotelList = GetHotelListByLocationFromStorage(destination.Code);
                var zoneDict = new Dictionary<string, string>();
                var areaDict = new Dictionary<string, Areas>();
                var zoneCounter = 1;
                var areaCounter = 1;
                foreach (var hotelCd in hotelList)
                {
                    var hotelDetail = GetHotelDetailFromTableStorage(hotelCd);
                    var location = new List<string>();
                    Debug.Print("HotelCd : {0} || Lat : {1} || Long : {2}", hotelDetail.HotelCode, hotelDetail.Latitude, hotelDetail.Longitude);
                    Console.WriteLine("HotelCd : {0} || Lat : {1} || Long : {2}", hotelDetail.HotelCode, hotelDetail.Latitude, hotelDetail.Longitude);
                    try
                    {
                        if (hotelDetail.Latitude == 0 || hotelDetail.Longitude == 0)
                        {
                            location = null;
                        }
                        else
                        {
                            location = client.GetLocationByGeoCode(hotelDetail.Latitude, hotelDetail.Longitude);
                            if (location != null && location[0] == "OVERLIMIT")
                            {
                                throw new Exception();
                            }
                        }

                    }
                    catch (Exception)
                    {
                        keyIndex++;
                        if (keyIndex <= apiKey.Length - 1)
                        {
                            Console.WriteLine("Key {0} : {1}", keyIndex, apiKey[keyIndex]);
                            client = new GeocodingClient(apiKey[keyIndex]);
                            location = client.GetLocationByGeoCode(hotelDetail.Latitude, hotelDetail.Longitude);
                        }
                    }

                    if (location == null || location[0] == "OVERLIMIT" || string.IsNullOrEmpty(location[0]) || string.IsNullOrEmpty(location[1]))
                    {
                        if (location != null && location[0] == "OVERLIMIT")
                        {
                            Console.WriteLine("Destination Code Error : " + destination.Code);
                        }
                        hotelDetail.ZoneCode = null;
                        hotelDetail.AreaCode = null;
                    }
                    else
                    {
                        if (zoneDict.ContainsKey(location[0]))
                        {
                            hotelDetail.ZoneCode = zoneDict[location[0]];
                        }
                        else
                        {
                            var zoneCd = destination.Code + '-' + zoneCounter;
                            hotelDetail.ZoneCode = zoneCd;
                            zoneDict.Add(location[0], zoneCd);
                            HotelZoneDict.Add(zoneCd, location[0]);
                            zoneCounter++;
                        }

                        if (areaDict.ContainsKey(location[1]))
                        {
                            hotelDetail.AreaCode = areaDict[location[1]].AreaCd;
                        }
                        else
                        {
                            var areaCd = hotelDetail.ZoneCode + '-' + areaCounter;
                            hotelDetail.AreaCode = areaCd;
                            var areac = new Areas
                            {
                                AreaCd = areaCd,
                                AreaName = location[1],
                                ZoneCd = hotelDetail.ZoneCode
                            };
                            areaDict.Add(location[1], areac);
                            HotelAreaDict.Add(areaCd, location[1]);
                            areaCounter++;
                        }
                    }
                    Console.Write("Update Hotel {0}", hotelDetail.HotelCode);
                    SaveHotelDetailToTableStorage(hotelDetail, hotelCd); //Update HotelDetail
                }
                var foundDestination = existedDestination.First(x => x.Code == destination.Code);
                var index = existedDestination.IndexOf(foundDestination);
                var zoneResultList =  PopulateZoneList(destination.CountryCode, destination.Code, zoneDict, areaDict);
                foundDestination.Zones = zoneResultList;
                if (index != -1)
                    existedDestination[index] = foundDestination;
            }
            UpdateZoneAreaInDestination(existedDestination);
            //CreateCSV(HotelDestinationCountryDict); //save to blob all
        }

        public List<Zone> PopulateZoneList(string countryCd, string destinationCd, Dictionary<string, string> zoneDict, Dictionary<string, Areas> areaDict)
        {
            var zoneList = new List<Zone>();
            foreach (var zone in zoneDict)
            {
                var areaList = new List<Area>();
                var areaPerZone = areaDict.Where(x => x.Value.ZoneCd.Equals(zone.Value));
                if (areaPerZone.Count() != 0)
                {
                    foreach (var area in areaPerZone)
                    {
                        var singleArea = new Area
                        {
                            Code = area.Value.AreaCd,
                            Name = area.Value.AreaName,
                            ZoneCode = area.Value.ZoneCd,
                            DestinationCode = destinationCd,
                            CountryCode = countryCd
                        };
                        areaList.Add(singleArea);
                    }
                }
                
                var singleZone = new Zone
                {
                    Code = zone.Value,
                    Name = zone.Key,
                    DestinationCode = zone.Value.Split('-')[0],
                    Areas = areaList
                };
                zoneList.Add(singleZone);
            }
            return zoneList;
        }

        public void CreateCSV(Dictionary<string, Country> dict)
        {
            var result = new StringBuilder();
            foreach (var country in dict)
            {
                foreach (var destination in country.Value.Destinations)
                {
                    if (destination.Zones == null || destination.Zones.Count == 0)
                    {
                        string row = destination.Code + '|' + destination.Name + '|' + country.Value.Code + '|'
                                             + country.Value.IsoCode + '|'  + '|'  + '|' +
                                             '|';
                        result.Append(row);
                        result.Append("\n");
                    }
                    else
                    {
                        foreach (var zone in destination.Zones)
                        {
                            if (zone.Areas == null || zone.Areas.Count == 0)
                            {
                                string row = destination.Code + '|' + destination.Name + '|' + country.Value.Code + '|'
                                             + country.Value.IsoCode + '|' + zone.Code + '|' + zone.Name + '|' +
                                             '|';
                                result.Append(row);
                                result.Append("\n");
                            }
                            else
                            {
                                foreach (var area in zone.Areas)
                                {
                                    string row = destination.Code + '|' + destination.Name + '|' + country.Value.Code + '|'
                                                 + country.Value.IsoCode + '|' + zone.Code + '|' + zone.Name + '|' + area.Code +
                                                 '|' + area.Name;
                                    result.Append(row);
                                    result.Append("\n");
                                }
                            }

                        }    
                    }
                    
                }
            }
            byte[] csvFile = Encoding.ASCII.GetBytes(result.ToString());
            MemoryStream stream = new MemoryStream(csvFile);

            StreamReader reader = new StreamReader(stream);
            Debug.Print(reader.ReadToEnd());
            var blobService = BlobStorageService.GetInstance();
            blobService.WriteFileToBlob(new BlobWriteDto
            {
                FileBlobModel = new FileBlobModel
                {
                    FileInfo = new FileInfo
                    {
                        FileName = "HotelDestination",
                        ContentType = "text/csv",
                        FileData = csvFile
                    },
                    Container = "HotelCSVContent"
                },
                SaveMethod = SaveMethod.Force
            });

        }

        public void UpdateHotelDetailStorage()
        {
            Console.WriteLine("Update HotelDetail");
            var hotel = new HotelBedsService();
            hotel.GetHotelData(1,1000);
        }

        public void UpdateHotelRateCommentStorage()
        {
            Console.WriteLine("Update RateComment");
            var hotel = new HotelBedsService();
            hotel.GetRateCommentData(1,1000);
        }
        public void UpdateCountryStorage()
        {
            Console.WriteLine("Update Country");
            var hotel = new HotelBedsService();
            hotel.GetCountry(1,1000);
            var countries = HotelBedsService.hotelCountryList;
            List<CountryDict> hotelCountry = new List<CountryDict>();
            foreach (var country in countries)
            {
                var singleCountry = new CountryDict
                {
                    CountryCode = country.code,
                    IsoCode = country.isoCode,
                    Name = country.description.content
                };
                hotelCountry.Add(singleCountry);
            }
            SaveHotelCountryToStorage(hotelCountry);

        }

        public void UpdateDestinationStorage()
        {
            Console.WriteLine("Update Destination");
            List<Destination> hotelDestination = new List<Destination>();
            var hotel = new HotelBedsService();
            hotel.GetDestination(1, 1000);
            var destinations = HotelBedsService.hotelDestinationList;
            foreach (var destination in destinations)
            {
                var singleDestination = new Destination
                {
                    Code = destination.code,
                    CountryCode = destination.countryCode,
                    Name = destination.name == null ? null : destination.name.content,
                    Zones = destination.zones.Count == 0 ? new List<Zone>() : destination.zones.Select(zone => new Zone
                    {
                        Code = zone.zoneCode.ToString(),
                        Name = zone.name,
                        DestinationCode = destination.code,
                        Areas = new List<Area>()
                    }).ToList()
                };
                hotelDestination.Add(singleDestination);
            }
            SaveHotelDestinationToStorage(hotelDestination);
        }

        public void UpdateZoneAreaInDestination(List<Destination> destinationList)
        {
            SaveHotelDestinationToStorage(destinationList);
        }

        public void UpdateHotelBoardStorage()
        {
            Console.WriteLine("Update Board");
            var hotel = new HotelBedsService();
            hotel.GetBoard(1,1000);
            var boards = HotelBedsService.hotelBoardList;
            List<Board> hotelBoard = new List<Board>();
            foreach (var board in boards)
            {
                var singleBoard = new Board
                {
                    Code = board.code,
                    MultilingualCode = board.multiLingualCode,
                    NameEn = board.DescriptionEng,
                    NameId = board.DescriptionInd
                };
                hotelBoard.Add(singleBoard);
            }
            SaveHotelBoardToStorage(hotelBoard);
        }

        public void UpdateHotelChainStorage()
        {
            Console.WriteLine("Update Chain");
            var hotel = new HotelBedsService();
            hotel.GetChain(1, 1000);
            var chains = HotelBedsService.hotelChainList;
            List<Chain> hotelChain = new List<Chain>();
            foreach (var chain in chains)
            {
                var singleChain = new Chain
                {
                    Code = chain.code,
                    Description = chain.DescriptionEng
                };
                hotelChain.Add(singleChain);
            }
            SaveHotelChainToStorage(hotelChain);
        }

        public void UpdateHotelAccomodationStorage()
        {
            Console.WriteLine("Update Accomodation");
            var hotel = new HotelBedsService();
            hotel.GetAccomodation(1, 1000);
            var accomodations = HotelBedsService.hotelAccomodationList;
            List<Accommodation> hotelAccomodation = new List<Accommodation>();
            foreach (var accomodation in accomodations)
            {
                var singleAccomodation = new Accommodation
                {
                    Code = accomodation.code,
                    TypeNameEn = accomodation.DescriptionEng,
                    TypeNameId =  accomodation.DescriptionInd,
                    MultiDescription = accomodation.typeDescription
                };
                hotelAccomodation.Add(singleAccomodation);
            }
            SaveHotelAccomodationToStorage(hotelAccomodation);
        }

        public void UpdateHotelCategoryStorage()
        {
            Console.WriteLine("Update Category");
            var hotel = new HotelBedsService();
            hotel.GetCategory(1, 1000);
            var categories = HotelBedsService.hotelCategoryList;
            List<Category> hotelCategory = new List<Category>();
            foreach (var category in categories)
            {
                var singleCategory = new Category
                {
                    Code = category.code,
                    NameEn = category.DescriptionEng,
                    NameId = category.DescriptionInd,
                    SimpleCode = category.simpleCode,
                    AccomodationType = category.accomodationType
                };
                hotelCategory.Add(singleCategory);
            }
            SaveHotelCategoryToStorage(hotelCategory);
        }

        public void UpdateHotelFacilityStorage()
        {
            Console.WriteLine("Update Facility");
            var hotel = new HotelBedsService();
            hotel.GetFacility(1, 1000);
            var facilities = HotelBedsService.hotelFacilityList;
            List<Facility> hotelFacility = new List<Facility>();
            foreach (var facility in facilities)
            {
                var singleFacility = new Facility
                {
                    Code = facility.code,
                    NameEn = facility.DescriptionEng,
                    NameId = facility.DescriptionInd,
                };
                hotelFacility.Add(singleFacility);
            }
            SaveHotelFacilityToStorage(hotelFacility);
        }

        public void UpdateHotelFacilityGroupStorage()
        {
            Console.WriteLine("Update facility Group");
            var hotel = new HotelBedsService();
            hotel.GetFacilityGroup(1, 1000);
            var groups = HotelBedsService.hotelFacilityGroupList;
            List<FacilityGroup> hotelFacilityGroup = new List<FacilityGroup>();
            foreach (var facilityGroup in groups)
            {
                var singleFacilityGroup = new FacilityGroup
                {
                    Code = facilityGroup.code,
                    NameEn = facilityGroup.DescriptionEng,
                    NameId = facilityGroup.DescriptionInd,
                };
                hotelFacilityGroup.Add(singleFacilityGroup);
            }
            SaveHotelFacilityGroupToStorage(hotelFacilityGroup);
        }

        public void UpdateHotelSegmentStorage()
        {
            Console.WriteLine("Update Segment");
            var hotel = new HotelBedsService();
            hotel.GetSegment(1, 1000);
            var segments = HotelBedsService.hotelSegmentList;
            List<SegmentDict> hotelSegment = new List<SegmentDict>();
            foreach (var Segment in segments)
            {
                var singleSegment = new SegmentDict
                {
                    Code = Segment.code.ToString(),
                    NameEn = Segment.DescriptionEng,
                    NameId = Segment.DescriptionInd,
                };
                hotelSegment.Add(singleSegment);
            }
            SaveHotelSegmentToStorage(hotelSegment);
        }

        public void UpdateHotelRoomStorage()
        {
            Console.WriteLine("Update Room");
            var hotel = new HotelBedsService();
            hotel.GetRoom(1, 1000);
            var rooms = HotelBedsService.hotelRoomList;
            List<Room> hotelRoom = new List<Room>();
            foreach (var room in rooms)
            {
                var singleRoom = new Room
                {
                    RoomCd = room.code,
                    MaxAdult = room.maxAdults,
                    MinAdult = room.minAdults,
                    MaxChild = room.maxChildren,
                    MaxPax = room.maxPax,
                    MinPax = room.minPax,
                    RoomDescEn = room.descriptionEng,
                    RoomDescId = room.descriptionInd,
                    RoomCharacteristic = new RoomCharacteristic
                    {
                        CharacteristicCd =  room.characteristic,
                        CharacteristicDescEn = room.characteristicDescriptionEng,
                        CharacteristicDescId = room.characteristicDescriptionInd
                    },
                    RoomType = new HotelRoomType
                    {
                        Type = room.type,
                        DescEn = room.descriptionEng,
                        DescId = room.descriptionInd
                    }
                };
                hotelRoom.Add(singleRoom);
            }
            SaveHotelRoomToStorage(hotelRoom);
        }
 
        public class Areas
        {
            public string ZoneCd { get; set; }
            public string AreaCd { get; set; }
            public string AreaName { get; set; }

        }
    }
}
