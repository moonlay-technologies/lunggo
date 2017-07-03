using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Constant;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model;
using Occupancy = Lunggo.ApCommon.Hotel.Model.Occupancy;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {
        //Filtering
        public List<HotelDetail> FilterHotel(List<HotelDetail> hotels, HotelFilter filterParam)
        {

            var facilityData = new Dictionary<string,List<string>>();

            if (hotels != null && filterParam != null)
            {
                SetValidFilter(filterParam);
                if (filterParam.FacilityFilter != null)
                {
                    foreach (var facility in filterParam.FacilityFilter)
                    {
                        var facilityList = HotelFacilityFilters[facility].FacilityCode;
                        facilityData.Add(facility,facilityList);
                    }
                }

                //NameFilter
                hotels = hotels.Where(p=>string.IsNullOrEmpty(filterParam.NameFilter) || p.HotelName.IndexOf(filterParam.NameFilter, StringComparison.CurrentCultureIgnoreCase)> -1).ToList();
                //AreaFilter
                hotels = hotels.Where(p => filterParam.AreaFilter == null || filterParam.AreaFilter.Contains(p.AreaCode)).ToList();
                //ZoneFilter
                hotels = hotels.Where(p => filterParam.ZoneFilter == null || filterParam.ZoneFilter.Contains(p.ZoneCode)).ToList();
                //FacilityFilter
                if (facilityData.Count != 0)
                {
                    foreach (var facItem in facilityData)
                    {
                        hotels = hotels.Where(p => p.Facilities == null ||  facItem.Value.Any(e => p.Facilities.Select(x => x.FacilityGroupCode + "" + x.FacilityCode ).ToList().Contains(e))).ToList();
                    }
                }
                
                //StarFilter
                hotels = hotels.Where(p => filterParam.StarFilter == null || filterParam.StarFilter.Contains(p.StarCode)).ToList();
                //PriceFilter
                hotels = hotels.Where(p => filterParam.PriceFilter == null || (p.NetCheapestFare >= filterParam.PriceFilter.MinPrice && p.NetCheapestFare <= filterParam.PriceFilter.MaxPrice)).ToList();

            }
            return hotels;
        }

        public void SetValidFilter(HotelFilter filter)
        {
            if (filter.ZoneFilter == null || filter.ZoneFilter.Count == 0)
                filter.ZoneFilter = null;
            if (filter.AccommodationTypeFilter == null ||filter.AccommodationTypeFilter.Count == 0)
                filter.AccommodationTypeFilter = null;
            if (filter.StarFilter == null || filter.StarFilter.Count == 0)
                filter.StarFilter = null;
            if (filter.AreaFilter == null || filter.AreaFilter.Count == 0)
                filter.AreaFilter = null;
            if (filter.BoardFilter == null || filter.BoardFilter.Count == 0)
                filter.BoardFilter = null;
            if (filter.PriceFilter == null || (filter.PriceFilter.MaxPrice <= 0))
                filter.PriceFilter = null;
            if (filter.FacilityFilter == null || filter.FacilityFilter.Count == 0)
                filter.FacilityFilter = null;
        }

        //Sorting
        public List<HotelDetail> SortHotel(List<HotelDetail> hotels, string param)
        {
            var sortedHotel = new List<HotelDetail>();
            switch (SortingTypeCd.Mnemonic(param))
            {
                case SortingType.AscendingPrice:
                    sortedHotel = hotels.OrderBy(p => p.NetCheapestFare).ToList();
                    break;
                case SortingType.DescendingPrice:
                    sortedHotel = hotels.OrderByDescending(p => p.NetCheapestFare).ToList();
                    break;
                case SortingType.AscendingStar:
                    sortedHotel = hotels.OrderBy(p => p.StarCode).ToList();
                    break;
                case SortingType.DescendingStar:
                    sortedHotel = hotels.OrderByDescending(p => p.StarCode).ToList();
                    break;
                default:
                    sortedHotel = hotels.OrderBy(p => p.NetCheapestFare).ToList();
                    break;
            }
            return sortedHotel;
        }

        //Tiket Set Filter
        public HotelFilterDisplayInfo TiketSetFilter(List<HotelDetail> hotels)
        {
            var filter = new HotelFilterDisplayInfo();
            var zoneDict = new Dictionary<string, ZoneFilterInfo>();
            var areaDict = new Dictionary<string, AreaFilterInfo>();
            var starDict = new Dictionary<int, StarFilterInfo>();
            try
            {
                foreach (var hotelDetail in hotels)
                {
                    if (!(zoneDict.ContainsKey(hotelDetail.ZoneCode)))
                    {
                        zoneDict.Add(hotelDetail.ZoneCode, new ZoneFilterInfo
                        {
                            Code = hotelDetail.ZoneCode,
                            Count = 1,
                            Name = hotelDetail.ZoneCode
                        });
                    }
                    else
                    {
                        zoneDict[hotelDetail.ZoneCode].Count += 1;
                    }

                    if (!(starDict.ContainsKey(hotelDetail.StarCode)))
                    {
                        starDict.Add(hotelDetail.StarCode, new StarFilterInfo
                        {
                            Code = hotelDetail.StarCode,
                            Count = 1,
                        });
                    }
                    else
                    {
                        starDict[hotelDetail.StarCode].Count += 1;
                    }

                    filter.ZoneFilter = new List<ZoneFilterInfo>();
                    filter.StarFilter = new List<StarFilterInfo>();
                    foreach (var zone in zoneDict.Keys)
                    {
                        if (!string.IsNullOrEmpty(zoneDict[zone].Name))
                        {
                            filter.ZoneFilter.Add(new ZoneFilterInfo
                            {
                                Code = zoneDict[zone].Code,
                                Count = zoneDict[zone].Count,
                                Name = zoneDict[zone].Name,
                            });
                        }
                    }

                    foreach (var key in starDict.Keys)
                    {
                        filter.StarFilter.Add(new StarFilterInfo
                        {
                            Code = starDict[key].Code,
                            Count = starDict[key].Count
                        });
                    }
                }
                
            }
            catch (Exception e)
            {
                Debug.Print(e.Message);
            }

            return filter;
            
        }

        //Set Filter Info
        public HotelFilterDisplayInfo SetHotelFilterDisplayInfo(List<HotelDetail> hotels, AutocompleteType searchLocationType)
        {
            var filter = new HotelFilterDisplayInfo();
            var zoneDict = new Dictionary<string, ZoneFilterInfo>();
            var areaDict = new Dictionary<string, AreaFilterInfo>();
            var starDict = new Dictionary<int, StarFilterInfo>();
            var accDict = new Dictionary<string, AccomodationFilterInfo>();
            var facilityDict = HotelFacilityFilters.Keys.ToDictionary(key => key, key => new FacilitiesFilterInfo
            {
                Name = key,
                Code = key
            });
            try
            {
                foreach (var hotelDetail in hotels)
                {
                    //Zone
                    if (searchLocationType == AutocompleteType.Destination)
                    {
                        if (!(zoneDict.ContainsKey(hotelDetail.ZoneCode)))
                        {
                            zoneDict.Add(hotelDetail.ZoneCode, new ZoneFilterInfo
                            {
                                Code = hotelDetail.ZoneCode,
                                Count = 1,
                                Name = GetZoneNameFromDict(hotelDetail.ZoneCode)
                            });
                        }
                        else
                        {
                            zoneDict[hotelDetail.ZoneCode].Count += 1;
                        }
                    }

                    //Area
                    if (searchLocationType == AutocompleteType.Zone)
                    {
                        if (!(areaDict.ContainsKey(hotelDetail.AreaCode)))
                        {
                            areaDict.Add(hotelDetail.AreaCode, new AreaFilterInfo
                            {
                                Code = hotelDetail.AreaCode,
                                Count = 1,
                                Name = GetAreaNameFromDict(hotelDetail.AreaCode)
                            });
                        }
                        else
                        {
                            areaDict[hotelDetail.AreaCode].Count += 1;
                        }
                    }

                    //Accomodation
                    if (!(accDict.ContainsKey(hotelDetail.AccomodationType)))
                    {
                        accDict.Add(hotelDetail.AccomodationType, new AccomodationFilterInfo
                        {
                            Code = hotelDetail.AccomodationType,
                            Count = 1,
                            Name = GetHotelAccomodationMultiDesc(hotelDetail.AccomodationType)
                        });
                    }
                    else
                    {
                        accDict[hotelDetail.AccomodationType].Count += 1;
                    }


                    if (!(starDict.ContainsKey(hotelDetail.StarCode)))
                    {
                        starDict.Add(hotelDetail.StarCode, new StarFilterInfo
                        {
                            Code = hotelDetail.StarCode,
                            Count = 1,
                        });
                    }
                    else
                    {
                        starDict[hotelDetail.StarCode].Count += 1;
                    }

                    //Facilities
                    if (hotelDetail.Facilities != null)
                    {
                        var keys = facilityDict.Keys;
                        var hotelFacilityDict = new Dictionary<string, bool>();
                        if (hotelDetail.Facilities != null)
                        {
                            foreach (var facility in hotelDetail.Facilities)
                            {
                                var concatedFacility = facility.FacilityGroupCode+ "" + facility.FacilityCode;

                                foreach (var key in keys)
                                {
                                    if (!hotelFacilityDict.ContainsKey(key))
                                        hotelFacilityDict[key] = false;
                                    if (HotelFacilityFilters[key].FacilityCode.Contains(concatedFacility))
                                    {
                                        hotelFacilityDict[key] = true;
                                    }
                                }
                            }
                            foreach (var key in keys)
                            {
                                facilityDict[key].Count += hotelFacilityDict[key] ? 1 : 0;
                            }
                        }
                    }

                    filter.ZoneFilter = new List<ZoneFilterInfo>();
                    filter.AreaFilter = new List<AreaFilterInfo>();
                    filter.AccomodationFilter = new List<AccomodationFilterInfo>();
                    filter.FacilityFilter = new List<FacilitiesFilterInfo>();
                    filter.StarFilter = new List<StarFilterInfo>();

                    if (searchLocationType == AutocompleteType.Destination)
                    {
                        foreach (var zone in zoneDict.Keys)
                        {
                            if (!string.IsNullOrEmpty(zoneDict[zone].Name))
                            {
                                filter.ZoneFilter.Add(new ZoneFilterInfo
                                {
                                    Code = zoneDict[zone].Code,
                                    Count = zoneDict[zone].Count,
                                    Name = zoneDict[zone].Name,
                                });
                            }
                        }
                    }

                    if (searchLocationType == AutocompleteType.Zone)
                    {
                        
                        foreach (var area in areaDict.Keys)
                        {
                            if (!string.IsNullOrEmpty(areaDict[area].Name))
                            {
                                filter.AreaFilter.Add(new AreaFilterInfo
                                {
                                    Code = areaDict[area].Code,
                                    Count = areaDict[area].Count,
                                    Name = areaDict[area].Name,
                                });
                            }
                        }
                    }

                    foreach (var accomodation in accDict.Keys)
                    {
                        filter.AccomodationFilter.Add(new AccomodationFilterInfo
                        {
                            Code = accDict[accomodation].Code,
                            Count = accDict[accomodation].Count,
                            Name = accDict[accomodation].Name
                        });
                    }

                    foreach (var key in facilityDict.Keys)
                    {
                        filter.FacilityFilter.Add(new FacilitiesFilterInfo
                        {
                            Code = facilityDict[key].Code,
                            Count = facilityDict[key].Count,
                            Name = facilityDict[key].Name
                        });
                    }

                    foreach (var key in starDict.Keys)
                    {
                        filter.StarFilter.Add(new StarFilterInfo
                        {
                            Code = starDict[key].Code,
                            Count = starDict[key].Count
                        });
                    }

                }
            }
            catch (Exception e)
            {
                Debug.Print(e.Message);
            }

            return filter;
 
        }

        //Star Filter
        public List<int> GetStarFilter(List<bool> starFilter)
        {
            if (starFilter != null)
            {
                int count = 0;
                var completedList = new List<int> { 1, 2, 3, 4, 5 };
                var listStar = new List<int>();
                foreach (var star in starFilter)
                {
                    if (star)
                    {
                        listStar.Add(completedList[count]);
                    }
                    count++;
                }
                return listStar;
            }
            else
            {
                return null;
            }
        }

        public List<HotelDetail> SetPagination(List<HotelDetail> hotels, int page, int perPage)
        {
            List<HotelDetail> shortedList = hotels.Skip((page - 1)*perPage).Take(perPage).ToList();
            return shortedList;
        }

        public List<HotelDetail> FilterSearchRoomByCapacity(List<HotelDetail> hotels, List<Occupancy> occupancies )
        {
            var maxPax = 0;
            var maxAdult = 0;
            var maxChild = 0;
            var roomCount = occupancies.Count;
            var hotelList = new List<HotelDetail>();
            foreach (var data in occupancies)
            {
                var totalPax = data.AdultCount + data.ChildCount;
                if (maxPax == 0)
                {
                    maxPax = totalPax;
                }
                if (totalPax > maxPax)
                {
                    maxPax = totalPax;
                }
                if (data.AdultCount > maxAdult)
                    maxAdult = data.AdultCount;
                if (data.ChildCount > maxChild)
                    maxChild = data.ChildCount;
            }
            foreach (var hotel in hotels)
            {
                var roomList = new List<HotelRoom>();
                foreach (var room in hotel.Rooms)
                {
                    var roomPax = GetPaxCapacity(room.RoomCode);
                    var roomAdult = GetMaxAdult(room.RoomCode);
                    var roomChild = GetMaxChild(room.RoomCode);
                    var rateList = new List<HotelRate>();
                    foreach (var rate in room.Rates)
                    {
                        if (roomPax >= maxPax && roomAdult >= maxAdult && roomChild >= maxChild && rate.Allotment >= roomCount)
                        {
                            rateList.Add(rate);
                        }    
                    }
                    room.Rates = rateList;
                    if (room.Rates.Count != 0)
                    {
                        roomList.Add(room);    
                    }
                }
                if (roomList.Count != 0)
                {
                    hotel.Rooms = roomList;
                    hotelList.Add(hotel);
                }
            }
            return hotelList;
        } 
    }
}
