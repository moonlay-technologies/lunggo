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
using Lunggo.Framework.BlobStorage;
using Lunggo.Framework.Config;
using Lunggo.Framework.Documents;
using Lunggo.Framework.TableStorage;
using Microsoft.Azure.Documents;
using Microsoft.WindowsAzure.Storage.Table;
using RestSharp;
using FileInfo = Lunggo.Framework.SharedModel.FileInfo;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {
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
            for (var i = 0; i <= 600000; i++)
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
            var destinationList = GetDestinationByCountryList("ID");
            //var destination = destinationList.FirstOrDefault(x => x.Code == "AMI");
            string[] apiKey = GeoCodeApiKeyList;
            var keyIndex = 2;
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
                    try
                    {
                        location = client.GetLocationByGeoCode(hotelDetail.Latitude, hotelDetail.Longitude);
                        if (location[0] == "OVERLIMIT")
                        {
                            throw new Exception();
                        }
                    }
                    catch(Exception)
                    {
                        keyIndex++;
                        if (keyIndex <= apiKey.Length - 1)
                        {
                            client = new GeocodingClient(apiKey[keyIndex]);
                            location = client.GetLocationByGeoCode(hotelDetail.Latitude, hotelDetail.Longitude);
                        }
                    }

                    if (location == null || location[0] == "OVERLIMIT" || string.IsNullOrEmpty(location[0]) || string.IsNullOrEmpty(location[1]))
                    {
                        if ( location != null && location[0] == "OVERLIMIT")
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
                    SaveHotelDetailToTableStorage(hotelDetail, hotelCd); //Update HotelDetail
                }
                UpdateCountryDictionary(destination.CountryCode, destination.Code, zoneDict, areaDict);
            }
            CreateCSV(HotelDestinationCountryDict); //save to blob all

        }

        public void UpdateCountryDictionary(string countryCd, string destinationCd, Dictionary<string, string> zoneDict, Dictionary<string, Areas> areaDict)
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
            var country = HotelDestinationCountryDict[countryCd];
            if (country != null)
            {
                var destination = country.Destinations.First(i => i.Code == destinationCd);
                if (destination != null)
                {
                    var index = country.Destinations.IndexOf(destination);
                    var singleDestination = new Destination
                    {
                        Code = destination.Code,
                        Name = destination.Name,
                        CountryCode = destination.CountryCode,
                        Zones = zoneList
                    };
                    country.Destinations[index] = singleDestination;
                    //country.Destinations = new List<Destination>();
                    //country.Destinations.Add(singleDestination);
                    //var dict = new Dictionary<string, Country>();
                    //dict.Add(country.Code, country);
                    //CreateCSV(dict);
                }
                HotelDestinationCountryDict[countryCd] = country; //TODO
            }
        }

        public void CreateCSV(Dictionary<string, Country> dict)
        {
            var result = new StringBuilder();
            foreach (var country in dict)
            {
                foreach (var destination in country.Value.Destinations)
                {
                    foreach (var zone in destination.Zones)
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
            byte[] csvFile = Encoding.ASCII.GetBytes(result.ToString());
            MemoryStream stream = new MemoryStream(csvFile);

            StreamReader reader = new StreamReader(stream);
            Debug.Print(reader.ReadToEnd());
            Console.WriteLine(reader.ReadToEnd());
            var blobService = BlobStorageService.GetInstance();
            blobService.WriteFileToBlob(new BlobWriteDto
            {
                FileBlobModel = new FileBlobModel
                {
                    FileInfo = new FileInfo
                    {
                        FileName = "Area",
                        ContentType = "text/csv",
                        FileData = csvFile
                    },
                    Container = "HotelDestination"
                },
                SaveMethod = SaveMethod.Force
            });

        }

        public class Areas
        {
            public string ZoneCd { get; set; }
            public string AreaCd { get; set; }
            public string AreaName { get; set; }

        }
    }
}
