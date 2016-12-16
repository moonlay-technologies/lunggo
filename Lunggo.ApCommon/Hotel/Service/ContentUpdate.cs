﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Query;
using Lunggo.Framework.BlobStorage;
using Lunggo.Framework.Config;
using Lunggo.Framework.Documents;
using Lunggo.Framework.SharedModel;
using Lunggo.Framework.TableStorage;
using Microsoft.Azure.Documents;
using Microsoft.WindowsAzure.Storage.Table;
using RestSharp;

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
                        SaveHotelLocationInStorage(hotel.DestinationCode, hotel.DestinationCode + '-' + hotel.ZoneCode, hotel.HotelCode);
                    if (hotel.AreaCode != null)
                        SaveHotelLocationInStorage(hotel.DestinationCode, hotel.DestinationCode + '-' + hotel.ZoneCode + '-' + hotel.AreaCode, hotel.HotelCode);
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
    }
}